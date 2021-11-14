using Pokermon.Core.Interfaces.Repositories;
using Pokermon.Core.Model;
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
        private static readonly Synchronizer TablesSynchronizer = new();
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

        public int? AddPlayer(int tableId, Guid playerId)
        {
            lock (TablesSynchronizer[tableId])
            {
                if (!Tables.TryGetValue(tableId, out var table))
                    return null;

                var freePosition = Array.IndexOf(table.Players, null);

                table.Players[freePosition] = new Player(playerId);

                return freePosition;
            }
        }

        public void RemovePlayer(int tableId, Guid playerId)
        {
            lock (TablesSynchronizer[tableId])
            {
                if (!Tables.TryGetValue(tableId, out var table))
                    return;

                for (var i = 0; i < 8; i++)
                {
                    if (table.Players[i].Id == playerId)
                    {
                        table.Players[i] = null;
                        break;
                    }
                }

                if (table.Players.All(p => p == null))
                    Tables.TryRemove(tableId, out _);
            }
        }
    }
}
