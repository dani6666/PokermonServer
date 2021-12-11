using Pokermon.Core.Model;
using Pokermon.Core.Model.Responses;
using System;

namespace Pokermon.Core.Interfaces.Services
{
    public interface IGameService
    {
        ResponseResult<GameStateResponse> GetGame(int gameId, Guid playerId);
        void CreateNewGame(int id);
        void AddPlayer(int id, int position, Guid playerId);
    }
}