using Pokermon.Core.Model.Entities;
using System.Collections.Generic;

namespace Pokermon.Core.Model.Responses
{
    public class GameStateResponse
    {
        public bool IsEndOfHand { get; set; }
        public int CurrentPlayerPosition { get; set; }
        public int PotValue { get; set; }
        public int?[] TableCards { get; set; } = new int?[5];
        public List<int> PocketCards { get; set; }
        public PlayerResponse[] Players { get; set; }
        public int CashToCall { get; set; }
        public bool CanRaise { get; set; }

        public GameStateResponse(GameState gameState)
        {
            IsEndOfHand = gameState.IsEndOfHand;
            CurrentPlayerPosition = gameState.CurrentPlayerPosition;
            PotValue = gameState.PotValue;
            for (int i = 0; i < (gameState.TableCards?.Count ?? 0); i++)
            {
                TableCards[i] = gameState.TableCards?[i];
            }
        }
    }
}
