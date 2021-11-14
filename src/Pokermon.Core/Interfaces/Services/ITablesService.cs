using Pokermon.Core.Model;
using Pokermon.Core.Model.Enums;
using Pokermon.Core.Model.Requests;
using Pokermon.Core.Model.Responses;
using System;
using System.Collections.Generic;

namespace Pokermon.Core.Interfaces.Services
{
    public interface ITablesService
    {
        ResponseResult<List<TableResponse>> ListTables();
        ResponseResult<JoinTableResponse> Join(int tableId);
        OperationError Leave(int tableId, Guid playerId);
        OperationError Create(CreateTableRequest request);
    }
}