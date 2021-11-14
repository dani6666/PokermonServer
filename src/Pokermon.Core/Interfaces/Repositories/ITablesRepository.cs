using Pokermon.Core.Model;
using System;
using System.Collections.Generic;

namespace Pokermon.Core.Interfaces.Repositories
{
    public interface ITablesRepository
    {
        List<Table> GetAllTables();
        void CreateTable(string name);
        int? AddPlayer(int tableId, Guid playerId);
        void RemovePlayer(int tableId, Guid playerId);
    }
}