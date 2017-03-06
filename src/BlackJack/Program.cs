using System;

namespace BlackJack
{
    public class Program
    {
        private static readonly int LineSeperatorCount = 30;
        private static readonly int MinNumberOfComputers = 1;
        private static readonly int MaxNumberOfComputers = 5;
        public delegate bool ConirmationFunc<T>(string input, out T value);

        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Blackjack!");
            var hands = ReadLine<int>("How many hands do you want to play?",
                "Please enter a whole number of hands greater than zero", ValidPositiveInt);
            var computerPlayers = ReadLine<int>($"How many computers would you like to play ({MinNumberOfComputers}-{MaxNumberOfComputers})",
                "Please enter at a positive whole number of users", ValidComputerPlayers);
            var userPlay = ReadLine<bool>("Would you like to play (yes or no)?", "Please type yes or no", ValidYesNo);
            var game = new Game(new Deck(6), computerPlayers);
            if (userPlay)
            {
                var name = ReadLine<string>("Please enter your name (max 20 characters)", "Name must not be empty", ValidUserName);
                game.AddPlayer(new HumanPlayer(100, name));
            }
            for (var i = 0; i < hands; i++)
            {
                Console.WriteLine($"Playing round {i + 1} \n{new string('-', LineSeperatorCount)}");
                var result = game.PlayRound();
                Console.WriteLine(result);
            }
            Console.WriteLine($"Total summary\n{new string('-', LineSeperatorCount)}");
            var summary = game.GetPlayerSummary();
            Console.WriteLine(summary);
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

        private static bool ValidUserName(string input, out string value)
        {
            if (input.Length > 0 && input.Length <= 20)
            {
                value = input;
                return true;
            }
            value = string.Empty;
            return false;
        }

        private static bool ValidYesNo(string input, out bool value)
        {
            if (string.Equals(input, "yes", StringComparison.OrdinalIgnoreCase))
            {
                value = true;
                return true;
            }
            if (string.Equals(input, "no", StringComparison.OrdinalIgnoreCase))
            {
                value = false;
                return true;
            }
            value = false;
            return false;
        }

        private static bool ValidPositiveInt(string input, out int value)
        {
            return int.TryParse(input, out value) && value > 0;

        }

        private static bool ValidComputerPlayers(string input, out int value)
        {
            return int.TryParse(input, out value) && value >= MinNumberOfComputers && value <= MaxNumberOfComputers;

        }

        private static T ReadLine<T>(string prompt, string retryMessage, ConirmationFunc<T> confirmationFunc)
        {
            Console.WriteLine(prompt);
            while (true)
            {
                var input = Console.ReadLine();
                T value;
                if (confirmationFunc(input, out value))
                {
                    return value;
                }
                Console.WriteLine(retryMessage);
            }
        }
    }
}
