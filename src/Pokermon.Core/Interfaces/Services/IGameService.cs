﻿using Pokermon.Core.Model;
using Pokermon.Core.Model.Enums;
using Pokermon.Core.Model.Responses;
using System;

namespace Pokermon.Core.Interfaces.Services
{
    public interface IGameService
    {
        ResponseResult<GameStateResponse> GetGame(int gameId, Guid playerId);
        OperationError PlaceBet(int id, Guid playerId, int bet);
        OperationError Check(int id, Guid playerId);
        OperationError Fold(int id, Guid playerId, bool force = false);
        void RemovePlayer(int id, Guid playerId);
        void CreateNewGame(int id);
        void AddPlayer(int id, int position, Guid playerId);
        void RestartGames();
    }
}