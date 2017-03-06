using System.Collections.Generic;
using System.Text;

namespace BlackJack
{
    public class Game
    {
        public Game(IDeck deck, int numberOfComputerPlayers)
        {
            m_Players = new List<IPlayer>();
            m_Deck = deck;
            for (int i = 0; i < numberOfComputerPlayers; i++)
            {
                m_Players.Add(new ComputerPlayer(1000, $"Computer {i}"));
            }
        }

        private readonly List<IPlayer> m_Players;
        private readonly IDeck m_Deck;

        public void AddPlayer(IPlayer player)
        {
            m_Players.Add(player);
        }

        public string PlayRound()
        {
            var round = new Round(m_Players, m_Deck);
            round.DealInitialCards();
            round.RunUserMoves();
            round.RunDealerMoves();
            var results = round.GetRoundResults();
            m_Deck.Shuffle();
            return results;
        }

        public string GetPlayerSummary()
        {
            var builder = new StringBuilder();
            const string summaryFormat = "{0, 20} | {1, 10} | {2, 5} | {3, 5} | {4, 5} | {5, 5}\n";
            builder.AppendFormat(summaryFormat, "Name", "Cash", "Won", "Played", "Max", "Min");
            builder.AppendLine(new string('-', builder.Length));
            foreach (var player in m_Players)
            {
                builder.AppendFormat(summaryFormat, player.Name, player.CashOnHand, player.HandsWon,
                    player.HandsPlayed, player.MaxMoney, player.MinMoney);
            }

            return builder.ToString();
        }
    }

    public enum Move
    {
        Hit,
        Stand,
    }
}