using Pokermon.Core.Model.Enums;

namespace Pokermon.Core.Model
{
    public class ResponseResult<T>
    {
        public OperationError Error { get; set; }

        public T Data { get; set; }

        public ResponseResult(T data)
        {
            Data = data;
        }

        public ResponseResult(OperationError operationError)
        {
            Error = operationError;
        }
    }
}
