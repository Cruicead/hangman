using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hangman
{
    class Hangman
    {
        public static void Main()
        {

            bool restart = true;

            var countries =
            from line in File.ReadAllLines(@"countries_and_capitals.txt")
            let columns = line.Split('|')
            select new
            {
                country = columns[0].Trim(),
                capital = columns[1].Trim()
            };

            List<string> ascii = new List<string>()
            {
                "    _____\n    |   |\n    O   |\n   /|\\  |\n   / \\  |\n        |\n      __|",
                "    _____\n    |   |\n    O   |\n   /|\\  |\n   /    |\n        |\n      __|",
                "    _____\n    |   |\n    O   |\n   /|\\  |\n        |\n        |\n      __|",
                "    _____\n    |   |\n    O   |\n   /|   |\n        |\n        |\n      __|",
                "    _____\n    |   |\n    O   |\n    |   |\n        |\n        |\n      __|",
                "    _____\n    |   |\n    O   |\n        |\n        |\n        |\n      __|",
                "    _____\n    |   |\n        |\n        |\n        |\n        |\n      __|"
            };

            while (restart)
            {
                Random rnd = new Random();

                var result = countries.ElementAt(rnd.Next() %countries.Count());

                string regex = "\\S";
                string puzzle = Regex.Replace(result.capital, regex, "_");
                string letters = "";
                string letter = "";
                int lives = 6;
                int counter = 0;
                bool victory = false;
                restart = false;

                Console.WriteLine("Welcome to Hangman: World Capitals! You have six lives. Guessing a letter wrongly costs one life, but guessing the word wrongly costs two, so choose wisely!");
                Stopwatch timer = Stopwatch.StartNew();

                while (true)
                {
                    counter++;
                    Console.WriteLine($"{ascii[lives]}\nThe word has {result.capital.Count(char.IsLetter)} letters.\n{puzzle}   guessed letters: {letters}");
                    if (lives < 3)
                    {
                        Console.WriteLine($"Hint: The capital of {result.country}.");
                    }
                    Console.WriteLine("Write \"lt\" if you want to guess a letter, or \"wd\" if you want to guess the word.");
                    switch (Console.ReadLine())
                    {
                        case "lt":
                            Console.WriteLine("Please enter a letter.");
                            letter = Console.ReadLine();
                            letters += letter;
                            regex = "[^\\s" + letters + letters.ToUpper() + "]";
                            if (!result.capital.Contains(letter) && !result.capital.Contains(letter.ToUpper()))
                            {
                                lives--;
                            }
                            break;
                        case "wd":
                            Console.WriteLine("Please enter your guess.");
                            letter = Console.ReadLine();
                            if (letter.Contains(result.capital) || letter.Contains(result.capital.ToLower()))
                            {
                                victory = true;
                            }
                            else
                            {
                                lives-=2;
                            }
                            break;
                    }
                    puzzle = Regex.Replace(result.capital, regex, "_");
                    if (lives < 1)
                    {
                        timer.Stop();
                        Console.WriteLine($"{ascii[0]}\nGame over! The answer was {result.capital}, the capital of {result.country}.");
                        var highscores = File.ReadLines(@"highscores.txt").TakeLast(10);
                        Console.WriteLine("Last ten high scores:");
                        highscores.ToList().ForEach(a => Console.WriteLine("{0}", a));
                        Console.WriteLine("Do you want to restart the game? Write \"yes\" or \"no\".");
                        letter = Console.ReadLine();
                        if (letter.Contains("yes"))
                        {
                            restart = true;
                        }
                        break;
                    }
                    if (victory || puzzle.Equals(result.capital))
                    {
                        timer.Stop();
                        double timespan = timer.Elapsed.TotalSeconds;
                        string time = string.Format("{0:0}",timespan);
                        Console.WriteLine($"Correct, it's {result.capital}, the capital of {result.country}. Congratulations, you won! It took you {time} seconds and {counter} guesses.\nPlease enter your name for the high scores.");
                        string name = Console.ReadLine();
                        DateTime date = DateTime.Now;
                        string highscore = $"{name} | {date.ToString("MM.dd.yyyy HH:mm")} | {time} seconds | {counter} guesses | {result.capital}" + Environment.NewLine;
                        File.AppendAllText(@"highscores.txt", highscore);
                        var highscores = File.ReadLines(@"highscores.txt").TakeLast(10);
                        Console.WriteLine("Last ten high scores:");
                        highscores.ToList().ForEach(a => Console.WriteLine("{0}", a));
                        Console.WriteLine("Do you want to restart the game? Write \"yes\" or \"no\".");
                        letter = Console.ReadLine();
                        if (letter.Contains("yes"))
                        {
                            restart = true;
                        }
                        break;
                    }
                }
            }
        }
    }
}
