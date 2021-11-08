using Pokermon.Core.Model.Enums;
using System.Collections.Generic;

namespace Pokermon.Core.Model
{
    public class GameState
    {
        public int PotValue { get; set; }
        public List<Card> TableCards { get; set; }
        public List<Card> PocketCards { get; set; }
        public List<Player> Players { get; set; }
        public int CashToCall { get; set; }
        public bool CanRaise { get; set; }
    }
}
