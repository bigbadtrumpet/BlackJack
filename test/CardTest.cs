using NUnit.Framework;

namespace BlackJack.Test
{
    [TestFixture]
    public class CardTest
    {
        [Test]
        public void CalculateScore_WithoutAce()
        {
            var cards = new Card[]
            {
                new Card(Suit.Diamonds, Number.Five),
                new Card(Suit.Diamonds, Number.Five),
                new Card(Suit.Diamonds, Number.Five),
            };
            Assert.True(cards.CalculateScore() == 15);
        }

        [Test]
        public void CalculateScore_WithAce_LessThan21()
        {
            var cards = new Card[]
            {
                new Card(Suit.Diamonds, Number.Four),
                new Card(Suit.Diamonds, Number.Ace),
            };
            Assert.True(cards.CalculateScore() == 15);

        }

        [Test]
        public void CalculateScore_WithAce_GreaterThan21()
        {
            var cards = new Card[]
            {
                new Card(Suit.Diamonds, Number.Four),
                new Card(Suit.Diamonds, Number.Ten),
                new Card(Suit.Diamonds, Number.Ace),
            };
            Assert.True(cards.CalculateScore() == 15);
        }

        [Test]
        public void CalculateScore_WithMultipleAces()
        {
            var cards = new Card[]
            {
                new Card(Suit.Diamonds, Number.Ace),
                new Card(Suit.Diamonds, Number.Ace),
            };
            Assert.True(cards.CalculateScore() == 12);
        }

        [Test]
        public void CalculateScore_FaceCards()
        {
            var cards = new Card[]
            {
                new Card(Suit.Diamonds, Number.Kinq),
                new Card(Suit.Diamonds, Number.Queen),
                new Card(Suit.Diamonds, Number.Jack),
            };
            Assert.True(cards.CalculateScore() == 30);
        }
    }
}