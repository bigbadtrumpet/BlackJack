using System;
using System.Collections.Generic;

namespace BlackJack
{
    public class Hand
    {
        public const string BlackJackResult = "blackjack";
        public const string BustedResult = "busted";
        public const string WonResult = "won";
        public const string TiedResult = "tied";
        public const string LostResult = "lost";
        public readonly IPlayer Player;
        private readonly List<Card> m_Cards;
        public float Bet => m_Bet;
        private readonly float m_Bet;
        public IEnumerable<Card> Cards => m_Cards;
        public bool GotBlackJack { get; private set; }
        public int NumberOfCards => m_Cards.Count;

        private int m_Score;

        public Hand(IPlayer player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }
            Player = player;
            float bet = player.GetBet();
            if (Player.CashOnHand < bet)
            {
                bet = player.CashOnHand;
            }
            Player = player;
            m_Bet = bet;
            m_Cards = new List<Card>();
        }

        public void AddCard(Card card)
        {
            m_Cards.Add(card);
            m_Score = Cards.CalculateScore();
            if (m_Cards.Count == 2 && m_Score == 21)
            {
                GotBlackJack = true;
            }
            Player.HandleCard(card);
        }

        public int Score()
        {
            return m_Score;
        }

        public string HandleResults(int dealerScore)
        {
            if (GotBlackJack)
            {
                Player.HandleResult(m_Bet * 1.5f);
                return BlackJackResult;
            }
            var roundScore = Score();
            //bust!
            if (roundScore > 21)
            {
                Player.HandleResult(-m_Bet);
                return BustedResult;
            }
            //dealer bust or player beats the score
            if (dealerScore > 21 || roundScore > dealerScore)
            {
                Player.HandleResult(m_Bet);
                return WonResult;
            }
            if (dealerScore == roundScore)
            {
                Player.HandleResult(0);
                return TiedResult;
            }
            Player.HandleResult(-m_Bet);
            return LostResult;
        }

        public Card GetCard(int cardNumber)
        {
            return m_Cards[cardNumber];
        }
    }
}