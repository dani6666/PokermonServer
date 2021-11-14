using Pokermon.Core.Model.Enums;
using System.Collections.Generic;

namespace Pokermon.Core.Model.Responses
{
    public class GameStateResponse
    {
        public bool IsEndOfHand { get; set; }
        public int CurrentPlayerPosition { get; set; }
        public int PotValue { get; set; }
        public List<Card> TableCards { get; set; }
        public List<Card> PocketCards { get; set; }
        public List<PlayerResponse> Players { get; set; }
        public int CashToCall { get; set; }
        public bool CanRaise { get; set; }
    }
}
