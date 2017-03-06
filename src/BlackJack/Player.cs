using System;
using System.Collections.Generic;

namespace BlackJack
{
    public interface IPlayer
    {
        string Name { get; }
        float CashOnHand { get; }
        int HandsWon { get; }
        int HandsPlayed { get; }
        float MinMoney { get; }
        float MaxMoney { get; }
        void HandleResult(float money);
        float GetBet();
        Move GetMove(IEnumerable<Card> currentCards, Card dealerShownCard);
        void HandleCard(Card card);
    }

    public class HumanPlayer : Player
    {
        public HumanPlayer(int startingCash, string name) : base(startingCash, name)
        {
        }

        public override float GetBet()
        {
            Console.WriteLine($"Place your bet! Current cash {CashOnHand}, Min bet = 1");
            float betValue = 0;
            while (betValue < 1 || betValue > CashOnHand)
            {
                var bet = Console.ReadLine();
                if (float.TryParse(bet, out betValue) && betValue > 1 && betValue < CashOnHand) continue;
                Console.WriteLine("Please place a valid bet");
            }
            return betValue;
        }

        public override Move GetMove(IEnumerable<Card> currentCards, Card dealerShownCard)
        {
            Console.WriteLine(
                $"Dealer has a {dealerShownCard.PrettyPrint()}, you have {currentCards.Display()}, Hit (h) or Stand(s)");
            while (true)
            {
                var move = Console.ReadLine().Trim().ToLower();
                if (move == "h")
                {
                    return Move.Hit;
                }
                if (move == "s")
                {
                    return Move.Stand;
                }
                Console.WriteLine("Please type either \"h\" for Hit or \"s\" for stand");
            }
        }

        public override void HandleCard(Card card)
        {
            Console.WriteLine($"You were dealt {card.PrettyPrint()}");
        }
    }

    public class ComputerPlayer : Player
    {
        public const int ComputerNormalBet = 10;

        public ComputerPlayer(int startingCash, string name) : base(startingCash, name)
        {
        }

        public override float GetBet()
        {
            return Math.Min(CashOnHand, ComputerNormalBet);
        }

        public override Move GetMove(IEnumerable<Card> currentCards, Card dealerShownCard)
        {
            var currentScore = currentCards.CalculateScore();
            //stand on hard and soft 17
            if (currentScore > 16)
            {
                return Move.Stand;
            }
            //if the dealer has greater than a 1 and you have 12 to 21 you stand
            if (dealerShownCard.Value > 1 && currentScore > 11)
            {
                return Move.Stand;
            }
            return Move.Hit;
        }
    }

    public abstract class Player : IPlayer
    {
        protected Player(int startingCash, string name)
        {
            CashOnHand = startingCash;
            Name = name;
            MinMoney = MaxMoney = startingCash;
        }

        public float CashOnHand { get; private set; }
        public int HandsWon { get; private set; }
        public int HandsPlayed { get; private set; }
        public float MaxMoney { get; private set; }
        public float MinMoney { get; private set; }
        public string Name { get; }

        public abstract float GetBet();
        public abstract Move GetMove(IEnumerable<Card> currentCards, Card dealerShownCard);

        public void HandleResult(float money)
        {
            HandsPlayed++;
            if (money > 0)
            {
                HandsWon++;
            }
            CashOnHand += money;
            MaxMoney = Math.Max(MaxMoney, CashOnHand);
            MinMoney = Math.Min(MinMoney, CashOnHand);
        }

        public virtual void HandleCard(Card card)
        {

        }
    }
}