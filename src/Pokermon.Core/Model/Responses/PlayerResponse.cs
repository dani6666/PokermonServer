using Pokermon.Core.Model.Enums;
using System.Collections.Generic;

namespace Pokermon.Core.Model.Responses
{
    public class PlayerResponse
    {
        public bool IsPlaying { get; set; }
        public bool IsAllIn { get; set; }
        public int CurrentCash { get; set; }
        public int CurrentBet { get; set; }
        public int? WonCash { get; set; }
        public List<Card> PocketCards { get; set; }
    }
}
