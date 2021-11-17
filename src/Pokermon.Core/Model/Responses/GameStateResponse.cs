using Pokermon.Core.Model.Entities;
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
        public PlayerResponse[] Players { get; set; }
        public int CashToCall { get; set; }
        public bool CanRaise { get; set; }

        public GameStateResponse(GameState gameState)
        {
            
        }
    }
}
