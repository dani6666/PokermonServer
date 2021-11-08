using Microsoft.AspNetCore.Mvc;
using Pokermon.Core.Model;
using Pokermon.Core.Model.Requests;
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

        [HttpPost("/{id}")]
        public void Join(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public void Create(CreateTableRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
