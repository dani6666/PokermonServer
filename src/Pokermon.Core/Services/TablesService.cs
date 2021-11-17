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
        private readonly ITablesRepository _repository;
        private static readonly Synchronizer TablesSynchronizer = new();
        private static readonly object CreateTableLock = new();

        public TablesService(ITablesRepository repository)
        {
            _repository = repository;
        }

        public ResponseResult<List<TableResponse>> ListTables()
        {
            return new ResponseResult<List<TableResponse>>(_repository.GetAllTables()
                .Select(t => new TableResponse { Id = t.Id, Name = t.Name, Players = t.PlayerIds.Count(p => p != default) })
                .ToList());
        }

        public ResponseResult<JoinTableResponse> Join(int tableId)
        {
            var playerId = Guid.NewGuid();
            int? position;

            lock (TablesSynchronizer[tableId])
            {
                if (!_repository.TableExists(tableId))
                    return new ResponseResult<JoinTableResponse>(OperationError.TableDoesNotExist);

                position = _repository.AddPlayer(tableId, playerId);
            }

            if (!position.HasValue)
                return new ResponseResult<JoinTableResponse>(OperationError.NoSeatLeftAtTable);

            return new ResponseResult<JoinTableResponse>(new JoinTableResponse(playerId, position.Value));
        }

        public OperationError Leave(int tableId, Guid playerId)
        {
            lock (TablesSynchronizer[tableId])
            {
                if (!_repository.TableExists(tableId))
                    return OperationError.TableDoesNotExist;

                if (!_repository.PlayerExists(tableId, playerId))
                    return OperationError.PlayerDoesNotExist;

                var playersLeft = _repository.RemovePlayer(tableId, playerId);

                if (playersLeft == 0)
                {
                    _repository.DeleteTable(tableId);
                    TablesSynchronizer.Remove(tableId);
                }
            }

            return OperationError.NoError;
        }

        public OperationError Create(CreateTableRequest request)
        {
            lock (CreateTableLock)
            {
                if (_repository.TableExists(request.Name))
                    return OperationError.TableAlreadyExists;

                _repository.CreateTable(request.Name);
            }

            return OperationError.NoError;
        }
    }
}
