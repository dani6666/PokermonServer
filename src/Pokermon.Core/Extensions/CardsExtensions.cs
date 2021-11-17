using Pokermon.Core.Model.Entities;
using System;
using System.Collections.Generic;

namespace Pokermon.Core.Extensions
{
    public static class CardsExtensions
    {
        public static void Shuffle(this List<Card> cards)
        {
            var cardsCount = cards.Count;

            var random = new Random(DateTime.Now.Millisecond + DateTime.Now.Second);

            for (var i = 0; i < 300; i++)
            {
                var firstCardPosition = random.Next(cardsCount);
                int secondCardPosition;
                do
                {
                    secondCardPosition = random.Next(cardsCount);

                } while (firstCardPosition == secondCardPosition);

                (cards[firstCardPosition], cards[secondCardPosition]) = (cards[secondCardPosition], cards[firstCardPosition]);
            }
        }
    }
}
