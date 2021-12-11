using Pokermon.Core.Model.Entities;
using System;
using System.Collections.Generic;

namespace Pokermon.Core.Interfaces.Repositories
{
    public interface ITablesRepository
    {
        List<Table> GetAllTables();
        bool TableExists(int id);
        bool TableExists(string name);
        bool PlayerExists(int tableId, Guid playerId);
        int CreateTable(string name);
        void DeleteTable(int tableId);
        int? AddPlayer(int tableId, Guid playerId);
        int RemovePlayer(int tableId, Guid playerId);
    }
}