using NUnit.Framework;
using NSubstitute;

namespace BlackJack.Test
{
    [TestFixture]
    public class HandTest
    {
        [Test]
        public void ValidBet()
        {
            var player = Substitute.For<IPlayer>();
            player.CashOnHand.Returns(10f);
            player.GetBet().Returns(10f);
            var hand = new Hand(player);
            Assert.AreEqual(hand.Bet, 10, .01f);
        }

        [Test]
        public void InvalidBet_SetsToMaxAmount()
        {
            var player = Substitute.For<IPlayer>();
            player.CashOnHand.Returns(10f);
            player.GetBet().Returns(11f);
            var hand = new Hand(player);
            Assert.AreEqual(hand.Bet, 10, .01f);
        }

        [Test]
        public void AddCard_WithoutBlackJack()
        {
            var hand = new Hand(GetDefaultPlayer());
            hand.AddCard(new Card(Suit.Clubs, Number.Ace));
            hand.AddCard(new Card(Suit.Clubs, Number.Eight));
            Assert.That(!hand.GotBlackJack);
        }

        [Test]
        public void AddCard_WithBlackJack()
        {
            var hand = new Hand(GetDefaultPlayer());
            hand.AddCard(new Card(Suit.Clubs, Number.Ace));
            hand.AddCard(new Card(Suit.Clubs, Number.Ten));
            Assert.That(hand.GotBlackJack);
        }

        [Test]
        public void AddCard_TellsPlayerItsCard()
        {
            var player = GetDefaultPlayer();
            var hand = new Hand(player);
            player.ClearReceivedCalls();
            var playerCard = new Card(Suit.Hearts, Number.Queen);
            hand.AddCard(playerCard);
            player.Received().HandleCard(playerCard);
        }

        [Test]
        public void HandleResult_Blackjack()
        {
            var player = GetDefaultPlayer();
            var hand = new Hand(player);
            hand.AddCard(new Card(Suit.Diamonds, Number.Ace));
            hand.AddCard(new Card(Suit.Diamonds, Number.Kinq));
            var result = hand.HandleResults(18);
            Assert.That(result == Hand.BlackJackResult);
            player.Received().HandleResult(15f);
        }

        [Test]
        public void HandleResult_UserBusts()
        {
            var player = GetDefaultPlayer();
            var hand = new Hand(player);
            hand.AddCard(new Card(Suit.Diamonds, Number.Queen));
            hand.AddCard(new Card(Suit.Diamonds, Number.Kinq));
            hand.AddCard(new Card(Suit.Diamonds, Number.Five));
            var result = hand.HandleResults(18);
            Assert.That(result == Hand.BustedResult);
            player.Received().HandleResult(-10f);
        }

        [Test]
        public void HandleResult_DealerBusts()
        {
            var player = GetDefaultPlayer();
            var hand = new Hand(player);
            hand.AddCard(new Card(Suit.Diamonds, Number.Queen));
            hand.AddCard(new Card(Suit.Diamonds, Number.Kinq));
            var result = hand.HandleResults(25);
            Assert.That(result == Hand.WonResult);
            player.Received().HandleResult(10f);
        }

        [Test]
        public void HandleResult_DealerAndUserBusts()
        {
            var player = GetDefaultPlayer();
            var hand = new Hand(player);
            hand.AddCard(new Card(Suit.Diamonds, Number.Queen));
            hand.AddCard(new Card(Suit.Diamonds, Number.Kinq));
            hand.AddCard(new Card(Suit.Diamonds, Number.Five));
            var result = hand.HandleResults(25);
            Assert.That(result == Hand.BustedResult);
            player.Received().HandleResult(-10f);
        }

        [Test]
        public void HandleResult_DealerWins()
        {
            var player = GetDefaultPlayer();
            var hand = new Hand(player);
            hand.AddCard(new Card(Suit.Diamonds, Number.Kinq));
            hand.AddCard(new Card(Suit.Diamonds, Number.Five));
            var result = hand.HandleResults(18);
            Assert.That(result == Hand.LostResult);
            player.Received().HandleResult(-10f);
        }

        [Test]
        public void HandleResult_UserWins()
        {
            var player = GetDefaultPlayer();
            var hand = new Hand(player);
            hand.AddCard(new Card(Suit.Diamonds, Number.Queen));
            hand.AddCard(new Card(Suit.Diamonds, Number.Kinq));
            var result = hand.HandleResults(18);
            Assert.That(result == Hand.WonResult);
            player.Received().HandleResult(10f);
        }

        [Test]
        public void HandleResult_Tie()
        {
            var player = GetDefaultPlayer();
            var hand = new Hand(player);
            hand.AddCard(new Card(Suit.Diamonds, Number.Queen));
            hand.AddCard(new Card(Suit.Diamonds, Number.Kinq));
            var result = hand.HandleResults(20);
            Assert.That(result == Hand.TiedResult);
            player.Received().HandleResult(0);
        }

        private IPlayer GetDefaultPlayer()
        {
            var player = Substitute.For<IPlayer>();
            player.CashOnHand.Returns(10f);
            player.GetBet().Returns(10f);
            return player;
        }
    }
}