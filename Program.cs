using System;
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

            while (restart)
            {
                Random rnd = new Random();

                var result = countries.ElementAt(rnd.Next() %countries.Count());

                string regex = "\\S";
                string puzzle = Regex.Replace(result.capital, regex, "_");
                string letters = "";
                string letter = "";
                int lives = 6;
                bool victory = false;
                restart = false;

                Console.WriteLine("Welcome to Hangman: World Capitals! You have six lives. Guessing a letter wrongly costs one life, but guessing the word wrongly costs two, so choose wisely!");
                Stopwatch timer = Stopwatch.StartNew();

                while (true)
                {
                    Console.WriteLine($"{puzzle}   lives: {lives}   guessed letters: {letters}");
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
                        Console.WriteLine($"Game over! The answer was {result.capital}, the capital of {result.country}.");
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
                        Console.WriteLine($"Correct, it's {result.capital}, the capital of {result.country}. Congratulations, you won! It took you {time} seconds.");
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
