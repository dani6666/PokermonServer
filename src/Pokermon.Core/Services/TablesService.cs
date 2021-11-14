using Pokermon.Core.Interfaces.Repositories;
using Pokermon.Core.Model.Requests;
using Pokermon.Core.Model.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokermon.Core.Services
{
    public class TablesService
    {
        private readonly ITablesRepository _repository;

        public TablesService(ITablesRepository repository)
        {
            _repository = repository;
        }

        public List<TableResponse> ListTables()
        {
            return _repository.GetAllTables()
                .Select(t => new TableResponse { Id = t.Id, Name = t.Name, Players = t.Players.Count })
                .ToList();
        }

        public JoinTableResponse Join(int id)
        {
            var playerId = Guid.NewGuid();

            var position = _repository.AddPlayer(id, playerId);

            return new JoinTableResponse(playerId, position);
        }

        public void Leave(int id, Guid playerId)
        {
            _repository.RemovePlayer(id, playerId);
        }

        public void Create(CreateTableRequest request)
        {
            _repository.CreateTable(request.Name);
        }
    }
}
