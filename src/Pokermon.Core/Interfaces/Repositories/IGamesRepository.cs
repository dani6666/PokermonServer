using Pokermon.Core.Model.Entities;
using System;
using System.Collections.Generic;

namespace Pokermon.Core.Interfaces.Repositories
{
    public interface IGamesRepository
    {
        GameState GetGame(int id);
        void AddGame(int id, GameState newGameState);
        void AddPlayerToGame(int gameId, Player player, int seat);
        IEnumerable<GameState> GetGamesToRestart(DateTime endOfHandTime);
    }
}