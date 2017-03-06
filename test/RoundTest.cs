using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
 using NSubstitute;

 namespace BlackJack.Test
 {
     [TestFixture]
     public class RoundTest
     {
         [Test]
         public void DealCards_PlayerFirst()
         {
             var player = GetValidPlayer();
             var deck = Substitute.For<IDeck>();
             var round = new Round(new List<IPlayer> {player}, deck);
             deck.DrawCard()
                 .Returns(
                     new Card(Suit.Clubs, Number.Two), new Card(Suit.Clubs, Number.Three),
                     new Card(Suit.Clubs, Number.Four), new Card(Suit.Clubs, Number.Five));
             round.DealInitialCards();
             //drew only 4 cards
             deck.Received(4).DrawCard();
             Assert.AreEqual(new Card(Suit.Clubs, Number.Three), round.GetDealerCard(0));
             Assert.AreEqual(new Card(Suit.Clubs, Number.Five), round.GetDealerCard(1));
             Assert.AreEqual(new Card(Suit.Clubs, Number.Two), round.GetPlayerCard(PlayerName, 0));
             Assert.AreEqual(new Card(Suit.Clubs, Number.Four), round.GetPlayerCard(PlayerName, 1));
         }

         [Test]
         public void DealCards_OnlyPlayersWithMoney()
         {
             var validPlayer = GetValidPlayer();
             var brokePlayer = Substitute.For<IPlayer>();

             var deck = Substitute.For<IDeck>();
             var round = new Round(new List<IPlayer> {validPlayer, brokePlayer}, deck);
             Assert.AreEqual(round.NumberOfHands, 1);
         }

         [Test]
         public void DealerMoves_HitsOnLessThan17()
         {
             var player = GetValidPlayer();
             var deck = Substitute.For<IDeck>();
             var round = new Round(new List<IPlayer> {player}, deck);
             deck.DrawCard()
                 .Returns(
                     new Card(Suit.Clubs, Number.Two), new Card(Suit.Clubs, Number.Six),
                     new Card(Suit.Clubs, Number.Four), new Card(Suit.Clubs, Number.Kinq),
                     new Card(Suit.Diamonds, Number.Five));
             round.DealInitialCards();
             round.RunDealerMoves();
             Assert.AreEqual(3, round.NumberOfDealerCards);
         }

         [Test]
         public void DealerMoves_StaysOnHard17()
         {
             var player = GetValidPlayer();
             var deck = Substitute.For<IDeck>();
             var round = new Round(new List<IPlayer> {player}, deck);
             deck.DrawCard()
                 .Returns(
                     new Card(Suit.Clubs, Number.Two), new Card(Suit.Clubs, Number.Seven),
                     new Card(Suit.Clubs, Number.Four), new Card(Suit.Clubs, Number.Queen));
             round.DealInitialCards();
             round.RunDealerMoves();
             Assert.AreEqual(2, round.NumberOfDealerCards);
         }

         [Test]
         public void DealerMoves_StaysOnHard17WithAce()
         {
             var player = GetValidPlayer();
             var deck = Substitute.For<IDeck>();
             var round = new Round(new List<IPlayer> {player}, deck);
             deck.DrawCard()
                 .Returns(
                     new Card(Suit.Clubs, Number.Two), new Card(Suit.Clubs, Number.Ace),
                     new Card(Suit.Clubs, Number.Four), new Card(Suit.Clubs, Number.Six),
                     new Card(Suit.Spades, Number.Jack));
             round.DealInitialCards();
             round.RunDealerMoves();
             Assert.AreEqual(3, round.NumberOfDealerCards);
         }

         [Test]
         public void DealerMoves_StaysOnAbove17()
         {
             var player = GetValidPlayer();
             var deck = Substitute.For<IDeck>();
             var round = new Round(new List<IPlayer> {player}, deck);
             deck.DrawCard()
                 .Returns(
                     new Card(Suit.Clubs, Number.Two), new Card(Suit.Clubs, Number.Queen),
                     new Card(Suit.Clubs, Number.Four), new Card(Suit.Clubs, Number.Queen));
             round.DealInitialCards();
             round.RunDealerMoves();
             Assert.AreEqual(2, round.NumberOfDealerCards);
         }

         [Test]
         public void DealerMoves_HitsOnSoft17()
         {
             var player = GetValidPlayer();
             var deck = Substitute.For<IDeck>();
             var round = new Round(new List<IPlayer> {player}, deck);
             deck.DrawCard()
                 .Returns(
                     new Card(Suit.Clubs, Number.Two), new Card(Suit.Clubs, Number.Six),
                     new Card(Suit.Clubs, Number.Four), new Card(Suit.Clubs, Number.Ace),
                     new Card(Suit.Diamonds, Number.Ten));
             round.DealInitialCards();
             round.RunDealerMoves();
             Assert.AreEqual(3, round.NumberOfDealerCards);
         }

         [Test]
         public void UserMoves_AddsCardOnHit()
         {
             var player = GetValidPlayer();
             var deck = Substitute.For<IDeck>();
             var round = new Round(new List<IPlayer> {player}, deck);
             player.GetMove(Arg.Any<IEnumerable<Card>>(), Arg.Any<Card>()).Returns(Move.Hit, Move.Stand);

             deck.DrawCard()
                 .Returns(
                     new Card(Suit.Clubs, Number.Two), new Card(Suit.Clubs, Number.Six),
                     new Card(Suit.Clubs, Number.Four), new Card(Suit.Clubs, Number.Ace),
                     new Card(Suit.Diamonds, Number.Ten));
             round.DealInitialCards();
             round.RunUserMoves();
             Assert.AreEqual(3, round.GetPlayerCardCount(PlayerName));
         }

         [Test]
         public void UserMoves_StaysOnStay()
         {
             var player = GetValidPlayer();
             var deck = Substitute.For<IDeck>();
             var round = new Round(new List<IPlayer> {player}, deck);
             player.GetMove(Arg.Any<IEnumerable<Card>>(), Arg.Any<Card>()).Returns(Move.Stand);

             deck.DrawCard()
                 .Returns(
                     new Card(Suit.Clubs, Number.Two), new Card(Suit.Clubs, Number.Six),
                     new Card(Suit.Clubs, Number.Four), new Card(Suit.Clubs, Number.Ace),
                     new Card(Suit.Diamonds, Number.Ten));
             round.DealInitialCards();
             round.RunUserMoves();
             Assert.AreEqual(2, round.GetPlayerCardCount(PlayerName));
         }

         [Test]
         public void UserMoves_StopsAfterUserReaches21()
         {
             var player = GetValidPlayer();
             var deck = Substitute.For<IDeck>();
             var round = new Round(new List<IPlayer> {player}, deck);
             player.GetMove(Arg.Any<IEnumerable<Card>>(), Arg.Any<Card>()).Returns(Move.Hit);

             deck.DrawCard()
                 .Returns(
                     new Card(Suit.Clubs, Number.Five), new Card(Suit.Clubs, Number.Six),
                     new Card(Suit.Clubs, Number.Six), new Card(Suit.Clubs, Number.Ace),
                     new Card(Suit.Diamonds, Number.Ten));
             round.DealInitialCards();
             round.RunUserMoves();
             Assert.AreEqual(3, round.GetPlayerCardCount(PlayerName));
         }

         [Test]
         public void UserMoves_StopsIfUserBusts()
         {
             var player = GetValidPlayer();
             var deck = Substitute.For<IDeck>();
             var round = new Round(new List<IPlayer> {player}, deck);
             player.GetMove(Arg.Any<IEnumerable<Card>>(), Arg.Any<Card>()).Returns(Move.Hit);

             deck.DrawCard()
                 .Returns(
                     new Card(Suit.Clubs, Number.Ten), new Card(Suit.Clubs, Number.Six),
                     new Card(Suit.Clubs, Number.Four), new Card(Suit.Clubs, Number.Ace),
                     new Card(Suit.Diamonds, Number.Ten));
             round.DealInitialCards();
             round.RunUserMoves();
             Assert.AreEqual(3, round.GetPlayerCardCount(PlayerName));
         }

         [Test]
         public void GetResults()
         {
             var player = GetValidPlayer();
             var deck = Substitute.For<IDeck>();
             var round = new Round(new List<IPlayer> {player}, deck);
             player.GetMove(Arg.Any<IEnumerable<Card>>(), Arg.Any<Card>()).Returns(Move.Hit);

             deck.DrawCard()
                 .Returns(
                     new Card(Suit.Clubs, Number.Ten), new Card(Suit.Clubs, Number.Seven),
                     new Card(Suit.Clubs, Number.Four), new Card(Suit.Clubs, Number.Ace),
                     new Card(Suit.Diamonds, Number.Ten));
             round.DealInitialCards();
             round.RunUserMoves();
             var result = round.GetRoundResults();
             Assert.That(result.Contains(PlayerName));
             Assert.That(result.Contains(Hand.BustedResult));
             Assert.That(result.Contains(player.CashOnHand.ToString(CultureInfo.InvariantCulture)));
             Assert.That(result.Contains("Dealer"));
             //dealer score
             Assert.That(result.Contains("18"));
             player.Received().HandleResult(-10f);
         }

         private IPlayer GetValidPlayer()
         {
             var player = Substitute.For<IPlayer>();
             player.CashOnHand.Returns(15);
             player.GetBet().Returns(10f);
             player.Name.Returns(PlayerName);
             return player;
         }

         private const string PlayerName = "Player1";
     }
 }