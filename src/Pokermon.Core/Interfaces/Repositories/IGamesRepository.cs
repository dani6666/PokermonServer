using Pokermon.Core.Model.Entities;

namespace Pokermon.Core.Interfaces.Repositories
{
    public interface IGamesRepository
    {
        GameState GetGame(int id);
        void AddGame(int id, GameState newGameState);
        void AddPlayerToGame(int gameId, Player player, int seat);
    }
}