using Pokermon.Core.Interfaces.Repositories;
using Pokermon.Core.Interfaces.Services;
using Pokermon.Core.Model;
using Pokermon.Core.Model.Enums;
using Pokermon.Core.Model.Requests;
using Pokermon.Core.Model.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokermon.Core.Services
{
    public class TablesService : ITablesService
    {
        private readonly ITablesRepository _tablesRepository;
        private readonly IGameService _gameService;
        private static readonly Synchronizer TablesSynchronizer = new();
        private static readonly object CreateTableLock = new();

        public TablesService(ITablesRepository tablesRepository, IGameService gameService)
        {
            _tablesRepository = tablesRepository;
            _gameService = gameService;
        }

        public ResponseResult<List<TableResponse>> ListTables()
        {
            return new ResponseResult<List<TableResponse>>(_tablesRepository.GetAllTables()
                .Select(t => new TableResponse { Id = t.Id, Name = t.Name, Players = t.PlayerIds.Count(p => p != default) })
                .ToList());
        }

        public ResponseResult<JoinTableResponse> Join(int tableId)
        {
            var playerId = Guid.NewGuid();
            int? position;

            lock (TablesSynchronizer[tableId])
            {
                if (!_tablesRepository.TableExists(tableId))
                    return new ResponseResult<JoinTableResponse>(OperationError.TableDoesNotExist);

                position = _tablesRepository.AddPlayer(tableId, playerId);

                if (!position.HasValue)
                    return new ResponseResult<JoinTableResponse>(OperationError.NoSeatLeftAtTable);

                _gameService.AddPlayer(tableId, position.Value, playerId);
            }

            return new ResponseResult<JoinTableResponse>(new JoinTableResponse(playerId, position.Value));
        }

        public OperationError Leave(int tableId, Guid playerId)
        {
            lock (TablesSynchronizer[tableId])
            {
                if (!_tablesRepository.TableExists(tableId))
                    return OperationError.TableDoesNotExist;

                if (!_tablesRepository.PlayerExists(tableId, playerId))
                    return OperationError.PlayerDoesNotExist;

                _gameService.Fold(tableId, playerId, true);

                var playersLeft = _tablesRepository.RemovePlayer(tableId, playerId);

                if (playersLeft == 0)
                {
                    _tablesRepository.DeleteTable(tableId);
                    TablesSynchronizer.Remove(tableId);
                }
            }

            return OperationError.NoError;
        }

        public OperationError Create(CreateTableRequest request)
        {
            lock (CreateTableLock)
            {
                if (_tablesRepository.TableExists(request.Name))
                    return OperationError.TableAlreadyExists;

                var tableId = _tablesRepository.CreateTable(request.Name);

                _gameService.CreateNewGame(tableId);
            }

            return OperationError.NoError;
        }
    }
}
