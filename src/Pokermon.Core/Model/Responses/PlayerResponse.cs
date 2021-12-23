using Pokermon.Core.Model.Entities;
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
        public List<int> PocketCards { get; set; }

        public PlayerResponse(Player player)
        {
            IsPlaying = player.PocketCards != null;
            IsAllIn = player.IsAllIn;
            CurrentCash = player.CurrentCash;
            CurrentBet = player.CurrentBet;
            WonCash = player.WonCash;
        }
    }
}
