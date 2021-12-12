using Microsoft.AspNetCore.Mvc;
using Pokermon.Core.Interfaces.Services;
using Pokermon.Core.Model.Enums;
using Pokermon.Core.Model.Requests;
using Pokermon.Core.Model.Responses;
using System;

namespace Pokermon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private IGameService _gameService;
        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("{id}")]
        public ActionResult<GameStateResponse> Index(int id, [FromHeader] Guid playerId)
        {
            var response = _gameService.GetGame(id, playerId);

            return response.Error switch
            {
                OperationError.NoError => response.Data,
                OperationError.PlayerDoesNotExist => Unauthorized(),
                _ => throw new ApplicationException("Unexpected error occured")
            };
        }

        [HttpPost("fold/{id}")]
        public void Fold(int id, [FromHeader] Guid playerId)
        {
            throw new NotImplementedException();
        }

        [HttpPost("check/{id}")]
        public void Check(int id, [FromHeader] Guid playerId)
        {
            throw new NotImplementedException();
        }

        [HttpPost("bet/{id}")]
        public void Bet(int id, [FromHeader] Guid playerId, [FromBody] BetRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
