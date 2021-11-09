using Microsoft.AspNetCore.Mvc;
using Pokermon.Core.Model;
using Pokermon.Core.Model.Requests;
using System;

namespace Pokermon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        [HttpGet("{id}")]
        public GameState Index(int id, [FromHeader] Guid playerId)
        {
            throw new NotImplementedException();
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
