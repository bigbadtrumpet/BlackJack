using System;
using System.Collections.Generic;

namespace BlackJack
{
    public interface IDeck
    {
        Card DrawCard();
        void Shuffle();
    }

    public class Deck : IDeck
    {
        public Deck(int numberOfDecks)
        {
            m_Random = new Random();
            m_Cards = new List<Card>(numberOfDecks * 12);
            m_RemovedCards = new List<Card>();
            //iterate over suit first since we are reflecting the values
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                for (int i = 0; i < numberOfDecks; i++)
                {
                    AddSuits(suit);
                }
            }
        }

        private readonly Random m_Random;
        private readonly List<Card> m_Cards;
        private readonly List<Card> m_RemovedCards;

        private void AddSuits(Suit suit)
        {
            foreach (var number in Enum.GetValues(typeof(Number)))
            {
                m_Cards.Add(new Card(suit, (Number) number));
            }
        }

        public Card DrawCard()
        {
            var cardNumber = m_Random.Next(m_Cards.Count);
            var card = m_Cards[cardNumber];
            m_RemovedCards.Add(card);
            m_Cards.RemoveAt(cardNumber);
            return card;
        }

        public void Shuffle()
        {
            m_Cards.AddRange(m_RemovedCards);
            m_RemovedCards.Clear();
        }
    }

}
