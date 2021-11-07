using Microsoft.AspNetCore.Mvc;
using Pokermon.Core.Model;
using System;
using System.Collections.Generic;

namespace Pokermon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Table> Index()
        {
            throw new NotImplementedException();
        }
    }
}
