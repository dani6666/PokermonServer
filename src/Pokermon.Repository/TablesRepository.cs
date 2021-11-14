using Pokermon.Core.Interfaces.Repositories;
using Pokermon.Core.Model.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Pokermon.Repository
{
    public class TablesRepository : ITablesRepository
    {
        private static readonly ConcurrentDictionary<int, Table> Tables = new();
        private static int _nextTableId;

        private static readonly object TableIdLock = new();

        public List<Table> GetAllTables()
        {
            return Tables.Values.ToList();
        }

        public void CreateTable(string name)
        {
            int id;
            lock (TableIdLock)
            {
                id = _nextTableId++;
            }

            Tables.TryAdd(id, new Table(id, name));
        }

        public void DeleteTable(int tableId)
        {
            Tables.Remove(tableId, out _);
        }

        public bool TableExists(int id) => Tables.ContainsKey(id);

        public bool TableExists(string name) => Tables.Values.Any(t => t.Name == name);

        public bool PlayerExists(int tableId, Guid playerId) => Tables[tableId].Players.Any(p => p?.Id == playerId);

        public int? AddPlayer(int tableId, Guid playerId)
        {
            var table = Tables[tableId];

            var freePosition = Array.IndexOf(table.Players, null);

            if (freePosition == -1)
                return null;

            table.Players[freePosition] = new Player(playerId);

            return freePosition;
        }

        public int RemovePlayer(int tableId, Guid playerId)
        {
            var table = Tables[tableId];
            var leftPlayers = 0;

            for (var i = 0; i < 8; i++)
            {
                if (table.Players[i] == null)
                    continue;

                if (table.Players[i].Id == playerId)
                    table.Players[i] = null;
                else
                    leftPlayers++;
            }

            return leftPlayers;
        }
    }
}
