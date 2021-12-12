using System;
using System.Collections.Generic;

namespace Pokermon.Core.Model.Entities
{
    public class Player
    {
        public Guid Id { get; set; }
        public bool CanRaise { get; set; } = true;
        public bool IsAllIn { get; set; } = false;
        public int CurrentCash { get; set; } = 50_000;
        public int CurrentBet { get; set; }
        public int TotalBet { get; set; }
        public int? WonCash { get; set; } = 0;
        public List<Card> PocketCards { get; set; }

        public Player(Guid id)
        {
            Id = id;
        }
    }
}
