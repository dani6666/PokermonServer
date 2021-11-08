using Microsoft.AspNetCore.Mvc;
using Pokermon.Core.Model;
using System;

namespace Pokermon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        [HttpGet("{id}")]
        public GameState Index()
        {
            throw new NotImplementedException();
        }
    }
}
