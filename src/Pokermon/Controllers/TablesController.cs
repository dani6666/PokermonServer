using Microsoft.AspNetCore.Mvc;
using Pokermon.Core.Interfaces.Services;
using Pokermon.Core.Model.Enums;
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
        private readonly ITablesService _tablesService;

        public TablesController(ITablesService tablesService)
        {
            _tablesService = tablesService;
        }
        
        [HttpGet]
        public IEnumerable<TableResponse> Index()
        {
            return _tablesService.ListTables().Data;
        }

        [HttpPost("create")]
        public ActionResult Create(CreateTableRequest request)
        {
            var operationError = _tablesService.Create(request);

            return operationError switch
            {
                OperationError.NoError => NoContent(),
                OperationError.TableAlreadyExists => BadRequest(),
                _ => throw new ApplicationException("Unexpected error occured")
            };
        }

        [HttpPost("join/{id}")]
        public ActionResult<JoinTableResponse> Join(int id)
        {
            var response = _tablesService.Join(id);

            return response.Error switch
            {
                OperationError.NoError => response.Data,
                OperationError.TableDoesNotExist => NotFound(),
                OperationError.NoSeatLeftAtTable => BadRequest(),
                _ => throw new ApplicationException("Unexpected error occured")
            };
        }

        [HttpPost("leave/{id}")]
        public ActionResult Leave(int id, [FromHeader] Guid playerId)
        {
            var operationError = _tablesService.Leave(id, playerId);

            return operationError switch
            {
                OperationError.NoError => NoContent(),
                OperationError.TableDoesNotExist => NotFound(),
                OperationError.PlayerDoesNotExist => Unauthorized(),
                _ => throw new ApplicationException("Unexpected error occured")
            };
        }
    }
}
