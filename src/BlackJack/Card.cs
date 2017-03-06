using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJack
{
    public class Card
    {
        public readonly Suit Suit;
        public readonly Number Number;
        public readonly int Value;

        public Card(Suit suit, Number number)
        {
            Number = number;
            Value = number.Value();
            Suit = suit;
        }

        public bool IsAce => Number == Number.Ace;

        public string PrettyPrint()
        {
            return $"{Number.PrettyPrint()}{Suit.PrettyPrint()}";
        }

        public override string ToString()
        {
            return PrettyPrint();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Card);
        }

        protected bool Equals(Card other)
        {
            return other != null && Suit == other.Suit && Number == other.Number;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Suit;
                hashCode = (hashCode * 397) ^ (int) Number;
                return hashCode;
            }
        }
    }

    public static class CardExtensions
    {
        public static int CalculateScore(this IEnumerable<Card> cards)
        {
            int remainingAces;
            return CalculateScore(cards, out remainingAces);
        }

        public static string Display(this IEnumerable<Card> cards)
        {
            if (cards == null)
            {
                return string.Empty;
            }
            StringBuilder cardBuilder = new StringBuilder();
            foreach (var card in cards)
            {
                cardBuilder.AppendFormat("{0} ", card.PrettyPrint());
            }
            return cardBuilder.ToString();

        }

        public static int CalculateScore(this IEnumerable<Card> cards, out int remainingAces)
        {
            if (cards == null)
            {
                remainingAces = 0;
                return 0;
            }
            int score = 0;
            remainingAces = 0;
            foreach (var card in cards)
            {
                score += card.Value;
                if (card.IsAce)
                {
                    remainingAces++;
                }
            }
            while (remainingAces > 0 && score > 21)
            {
                score -= 10;
                remainingAces--;
            }
            return score;
        }
    }

    public static class CardEnumExtensions
    {
        public static int Value(this Number number)
        {
            switch (number)
            {
                case Number.Two:
                    return 2;
                case Number.Three:
                    return 3;
                case Number.Four:
                    return 4;
                case Number.Five:
                    return 5;
                case Number.Six:
                    return 6;
                case Number.Seven:
                    return 7;
                case Number.Eight:
                    return 8;
                case Number.Nine:
                    return 9;
                case Number.Ten:
                case Number.Jack:
                case Number.Queen:
                case Number.Kinq:
                    return 10;
                case Number.Ace:
                    return 11;
                default:
                    throw new ArgumentOutOfRangeException(nameof(number), number, null);
            }
        }

        public static string PrettyPrint(this Number number)
        {
            switch (number)
            {
                case Number.Two:
                    return "2";
                case Number.Three:
                    return "3";
                case Number.Four:
                    return "4";
                case Number.Five:
                    return "5";
                case Number.Six:
                    return "6";
                case Number.Seven:
                    return "7";
                case Number.Eight:
                    return "8";
                case Number.Nine:
                    return "9";
                case Number.Ten:
                    return "10";
                case Number.Jack:
                    return "J";
                case Number.Queen:
                    return "Q";
                case Number.Kinq:
                    return "K";
                case Number.Ace:
                    return "A";
                default:
                    throw new ArgumentOutOfRangeException(nameof(number), number, null);
            }
        }

        public static string PrettyPrint(this Suit suit)
        {
            switch (suit)
            {
                case Suit.Diamonds:
                    return "D";
                case Suit.Hearts:
                    return "H";
                case Suit.Spades:
                    return "S";
                case Suit.Clubs:
                    return "C";
                default:
                    throw new ArgumentOutOfRangeException(nameof(suit), suit, null);
            }
        }
    }

    public enum Number
    {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        Kinq,
        Ace
    }

    public enum Suit
    {
        Diamonds,
        Hearts,
        Spades,
        Clubs
    }
}