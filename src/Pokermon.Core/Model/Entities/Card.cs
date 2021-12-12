using Pokermon.Core.Model.Enums;
using System;

namespace Pokermon.Core.Model.Entities
{
    public class Card : IComparable<Card>
    {
        public CardColor Color { get; set; }
        public int Value { get; set; }

        public Card(CardColor color, int value)
        {
            Color = color;
            Value = value;
        }

        public int CompareTo(Card other)
        {
            var valueComparison = Value.CompareTo(other.Value);

            return valueComparison != 0 ? valueComparison : Color.CompareTo(other.Color);
        }

        public static implicit operator int(Card card) =>
            (int)card.Color * 13 + card.Value;
    }
}
