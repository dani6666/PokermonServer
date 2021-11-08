using Microsoft.AspNetCore.Mvc;
using System;

namespace Pokermon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        [HttpGet("/{id}")]
        public void Index()
        {
            throw new NotImplementedException();
        }
    }
}
