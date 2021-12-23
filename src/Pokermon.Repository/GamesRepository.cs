using Pokermon.Core.Interfaces.Repositories;
using Pokermon.Core.Model.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Pokermon.Repository
{
    public class GamesRepository : IGamesRepository
    {
        private static readonly ConcurrentDictionary<int, GameState> Games = new();

        public GameState GetGame(int id) => 
            Games.GetValueOrDefault(id);

        public void AddGame(int id, GameState newGameState)
        {
            Games[id] = newGameState;
        }

        public void AddPlayerToGame(int gameId, Player player, int seat)
        {
            Games[gameId].Players[seat] = player;
        }

        public IEnumerable<GameState> GetGamesToRestart(DateTime endOfHandTime) =>
            Games.Values.Where(g => g.IsEndOfHand && g.HandEndTime < endOfHandTime);
    }
}
