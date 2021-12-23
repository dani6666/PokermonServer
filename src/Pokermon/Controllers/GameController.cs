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
                OperationError.TableDoesNotExist => NotFound(),
                _ => throw new ApplicationException("Unexpected error occured")
            };
        }

        [HttpPost("fold/{id}")]
        public ActionResult Fold(int id, [FromHeader] Guid playerId)
        {
            var error = _gameService.Fold(id, playerId);

            return error switch
            {
                OperationError.NoError => Ok(),
                OperationError.PlayerDoesNotExist => Unauthorized(),
                OperationError.TableDoesNotExist => NotFound(),
                OperationError.OtherPlayersTurn => BadRequest(),
                _ => throw new ApplicationException("Unexpected error occured")
            };
        }

        [HttpPost("check/{id}")]
        public ActionResult Check(int id, [FromHeader] Guid playerId)
        {
            var error = _gameService.Check(id, playerId);

            return error switch
            {
                OperationError.NoError => Ok(),
                OperationError.PlayerDoesNotExist => Unauthorized(),
                OperationError.TableDoesNotExist => NotFound(),
                OperationError.OtherPlayersTurn => BadRequest(),
                OperationError.BetTooLow => BadRequest(),
                _ => throw new ApplicationException("Unexpected error occured")
            };
        }

        [HttpPost("bet/{id}")]
        public ActionResult Bet(int id, [FromHeader] Guid playerId, [FromBody] BetRequest request)
        {
            var error = _gameService.PlaceBet(id, playerId, request.Value);

            return error switch
            {
                OperationError.NoError => Ok(),
                OperationError.PlayerDoesNotExist => Unauthorized(),
                OperationError.TableDoesNotExist => NotFound(),
                OperationError.OtherPlayersTurn => BadRequest(),
                OperationError.BetTooLow => BadRequest(),
                OperationError.PlayerCannotRaise => BadRequest(),
                OperationError.NotEnoughCashToBet => BadRequest(),
                _ => throw new ApplicationException("Unexpected error occured")
            };
        }
    }
}
