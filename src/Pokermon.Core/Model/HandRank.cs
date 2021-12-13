using System;

namespace Pokermon.Core.Model
{
    public class HandRank : IComparable<HandRank>
    {
        public int CategoryRank { get; set; }
        public int CardsRank { get; set; }

        public HandRank(int categoryRank, int cardsRank)
        {
            CategoryRank = categoryRank;
            CardsRank = cardsRank;
        }

        public int CompareTo(HandRank other)
        {
            if (other == null)
                return 1;

            var categoryComparison = CategoryRank.CompareTo(other.CategoryRank);

            return categoryComparison != 0 
                ? categoryComparison 
                : CardsRank.CompareTo(other.CardsRank);
        }
    }
}
