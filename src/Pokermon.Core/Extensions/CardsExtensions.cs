using Pokermon.Core.Model;
using Pokermon.Core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public static HandRank CalculateRank(this List<Card> cards)
        {
            var flush = CheckForFlush(cards);
            var straight = CheckForStraight(cards);

            if (flush && straight)
                return new HandRank(7, cards.Max(c => c.Value));

            var cardGroups = cards.GroupBy(c => c.Value).ToList();

            if (cardGroups.Count == 2)
            {
                var orderedGroups = cardGroups.OrderBy(g => g.Count()).ToArray();
                var categoryRank = orderedGroups[0].Count() == 4 ? 0 : 0;

                return new HandRank(6, categoryRank + orderedGroups[0].Key * 14 + orderedGroups[1].Key);
            }

            if (flush)
                return new HandRank(5, cards.Max(c => c.Value));

            if (straight)
                return new HandRank(4, cards.Max(c => c.Value));

            if (cardGroups.Count == 3)
            {
                var threeOfKindGroup = cardGroups.FirstOrDefault(g => g.Count() == 3);
                var singleKindOrderedGroups = cardGroups.Where(g => g.Count() == 1).OrderByDescending(g => g.Key).ToArray();

                if (threeOfKindGroup != null)
                    return new HandRank(3, threeOfKindGroup.Key * 14 * 14 + singleKindOrderedGroups[0].Key * 14 + singleKindOrderedGroups[1].Key);

                var pairGroups = cardGroups.Where(g => g.Count() == 2).OrderByDescending(g => g.Key).ToArray();

                return new HandRank(2, pairGroups[0].Key * 14 * 14 + pairGroups[1].Key * 14 + singleKindOrderedGroups[0].Key);
            }

            if (cardGroups.Count == 4)
            {
                var singleKindOrderedRanks = cardGroups.Where(g => g.Count() == 1).Select(g => g.Key).OrderByDescending(g => g).ToList();

                var pairRank = cardGroups.First(g => g.Count() == 2).Key;
                foreach (var rank in singleKindOrderedRanks)
                {
                    pairRank *= 14;
                    pairRank += rank;
                }

                return new HandRank(1, pairRank);
            }

            var orderedCardRanks = cards.Select(c => c.Value).OrderByDescending(v => v).ToList();

            var highCardRank = 0;
            foreach (var rank in orderedCardRanks)
            {
                highCardRank *= 14;
                highCardRank += rank;
            }

            return new HandRank(0, highCardRank);
        }

        public static IEnumerable<List<Card>> GetAllPossibleHands(this List<Card> cards)
        {
            for (var i = 0; i < 7; i++)
            {
                for (var j = i + 1; j < 7; j++)
                {
                    yield return cards.Where((c, k) => k != i && k != j).ToList();
                }
            }
        }

        private static bool CheckForStraight(List<Card> cards)
        {
            var minValue = cards.Min(c => c.Value);

            for (var i = 1; i < 5; i++)
            {
                if (cards.All(c => c.Value != minValue + i))
                    return false;
            }

            return true;
        }

        private static bool CheckForFlush(List<Card> cards) =>
            cards.Select(c => c.Color).Distinct().Count() == 1;
    }
}
