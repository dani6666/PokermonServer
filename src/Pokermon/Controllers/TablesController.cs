using Microsoft.AspNetCore.Mvc;
using Pokermon.Core.Model.Requests;
using Pokermon.Core.Model.Responses;
using System;
using System.Collections.Generic;

namespace Pokermon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<TableResponse> Index()
        {
            throw new NotImplementedException();
        }

        [HttpPost("join/{id}")]
        public JoinTableResponse Join(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("leave/{id}")]
        public void Leave(int id, [FromHeader] Guid playerId)
        {
            throw new NotImplementedException();
        }

        [HttpPost("create")]
        public void Create(CreateTableRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
