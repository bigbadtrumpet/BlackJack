using System.Collections.Generic;
using System.Text;

namespace BlackJack
{
    public class Round
    {
        public Round(List<IPlayer> players, IDeck deck)
        {
            m_Deck = deck;
            m_DealerCards = new List<Card>();
            int numOfPlayer = players.Count;

            m_Hands = new Dictionary<string, Hand>();
            //create each players hands for the players who still have money
            for (int i = 0; i < numOfPlayer; i++)
            {
                var player = players[i];
                if (player.CashOnHand > 0)
                {
                    m_Hands.Add(player.Name, new Hand(player));
                }
            }
        }

        private readonly List<Card> m_DealerCards;
        private readonly Dictionary<string, Hand> m_Hands;
        private readonly IDeck m_Deck;

        public int NumberOfHands => m_Hands.Count;
        public int NumberOfDealerCards => m_DealerCards.Count;

        public void DealInitialCards()
        {
            //give each player there first card
            foreach (var kvp in m_Hands)
            {
                var hand = kvp.Value;
                hand.AddCard(m_Deck.DrawCard());
            }
            //dealers first card is the face down card
            m_DealerCards.Add(m_Deck.DrawCard());
            //everyones second card
            foreach (var kvp in m_Hands)
            {
                var hand = kvp.Value;
                hand.AddCard(m_Deck.DrawCard());
            }
            //dealers second card is the face UP card
            m_DealerCards.Add(m_Deck.DrawCard());
        }

        public void RunUserMoves()
        {
            //if the dealer got black jack, everyone loses except for the people who also got blackjack
            //technically there is some more logic here with insursance but i didn't includ that for this demo
            if (m_DealerCards.CalculateScore() == 21)
            {
                return;
            }
            foreach (var kvp in m_Hands)
            {
                var hand = kvp.Value;
                while (hand.Score() < 21)
                {
                    //second card is the face up card
                    var move = hand.Player.GetMove(hand.Cards, m_DealerCards[1]);
                    if (move == Move.Hit)
                    {
                        hand.AddCard(m_Deck.DrawCard());
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public void RunDealerMoves()
        {
            do
            {
                int remainingAces;
                var dealerScore = m_DealerCards.CalculateScore(out remainingAces);
                //dealer stands 18+, and hard 17
                if (dealerScore > 17 || (dealerScore == 17 && remainingAces <= 0))
                    break;
                var newCard = m_Deck.DrawCard();
                m_DealerCards.Add(newCard);
            } while (true);
        }

        public string GetRoundResults()
        {
            int dealerScore = m_DealerCards.CalculateScore();
            StringBuilder result = CreateResultsBuilder(dealerScore, m_DealerCards);
            foreach (var kvp in m_Hands)
            {
                var hand = kvp.Value;
                string handResult = hand.HandleResults(dealerScore);
                result.AppendFormat(ResultsFormat, hand.Player.Name, handResult, hand.Score(),
                    hand.Cards.Display(), hand.Player.CashOnHand);

            }
            return result.ToString();
        }

        private static StringBuilder CreateResultsBuilder(int dealerScore, IEnumerable<Card> dealerCards)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(ResultsFormat, "Name", "Result", "Score", "Cards", "Cash on hand");
            builder.AppendLine(new string('-', builder.Length));
            builder.AppendFormat(ResultsFormat, "Dealer", "N/A", dealerScore, dealerCards.Display(), "N/A");
            return builder;
        }

        private static readonly string ResultsFormat = "{0, 20} | {1, 15} | {2, 5} | {3,26} | {4, 15}\n";

        public Card GetDealerCard(int cardNumber)
        {
            return m_DealerCards[cardNumber];
        }

        public Card GetPlayerCard(string playerName, int cardNumber)
        {
            return m_Hands[playerName].GetCard(cardNumber);
        }

        public int GetPlayerCardCount(string playerName)
        {
            return m_Hands[playerName].NumberOfCards;
        }
    }

}