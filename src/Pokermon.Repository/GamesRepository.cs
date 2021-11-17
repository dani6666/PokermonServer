using Pokermon.Core.Interfaces.Repositories;
using Pokermon.Core.Model.Entities;
using System.Collections.Concurrent;
using System.Collections.Generic;

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
    }
}
