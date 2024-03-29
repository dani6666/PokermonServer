﻿using System;
using System.Collections.Generic;

namespace Pokermon.Core.Model.Entities
{
    public class GameState
    {
        public int TableId { get; set; }
        public bool IsWaitingForPlayers { get; set; } = true;
        public bool IsEndOfHand { get; set; }
        public DateTime HandEndTime { get; set; }
        public int CurrentPlayerPosition { get; set; }
        public int LastRaisingPlayerPosition { get; set; }
        public int PotValue { get; set; }
        public int HighestBet { get; set; }
        public List<Card> Deck { get; set; } = new();
        public List<Card> TableCards { get; set; } = new();
        public Player[] Players { get; set; } = new Player[8];

        public GameState(int id)
        {
            TableId = id;
        }
    }
}
