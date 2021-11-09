using System;

namespace Pokermon.Core.Model.Responses
{
    public class JoinTableResponse
    {
        public Guid PlayerId { get; set; }
        public int TablePosition { get; set; }
    }
}
