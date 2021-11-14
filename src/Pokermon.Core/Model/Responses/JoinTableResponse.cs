using System;

namespace Pokermon.Core.Model.Responses
{
    public class JoinTableResponse
    {
        public Guid PlayerId { get; set; }
        public int TablePosition { get; set; }

        public JoinTableResponse(Guid playerId, int tablePosition)
        {
            PlayerId = playerId;
            TablePosition = tablePosition;
        }
    }
}
