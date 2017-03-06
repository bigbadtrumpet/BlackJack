using System.Collections.Generic;
using NUnit.Framework;

namespace BlackJack.Test
{
    [TestFixture]
    public class PlayerTest
    {
        private class TestPlayer : Player
        {
            public TestPlayer(int startingCash, string name) : base(startingCash, name)
            {
            }

            public override float GetBet()
            {
                throw new System.NotImplementedException();
            }

            public override Move GetMove(IEnumerable<Card> currentCards, Card dealerShownCard)
            {
                throw new System.NotImplementedException();
            }
        }

        [Test]
        public void HandleResult_Won()
        {
            var player = new TestPlayer(10, "Player");
            player.HandleResult(10f);
            Assert.AreEqual(1, player.HandsWon);
            Assert.AreEqual(1, player.HandsPlayed);
            Assert.AreEqual(20, player.CashOnHand);
        }

        [Test]
        public void Constructor()
        {
            var player = new TestPlayer(10, "Player");
            Assert.AreEqual(10, player.CashOnHand);
            Assert.AreEqual("Player", player.Name);
            Assert.AreEqual(0, player.HandsWon);
            Assert.AreEqual(0, player.HandsPlayed);
        }

        [Test]
        public void HandleResult_Lost()
        {
            var player = new TestPlayer(10, "Player");
            player.HandleResult(-10f);
            Assert.AreEqual(0, player.HandsWon);
            Assert.AreEqual(1, player.HandsPlayed);
            Assert.AreEqual(0, player.CashOnHand);
        }

        [Test]
        public void HandleResult_Mutliple()
        {
            var player = new TestPlayer(10, "Player");
            player.HandleResult(20f);
            player.HandleResult(-10f);
            Assert.AreEqual(1, player.HandsWon);
            Assert.AreEqual(2, player.HandsPlayed);
            Assert.AreEqual(20, player.CashOnHand);
        }
    }
}