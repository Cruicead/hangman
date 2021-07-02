using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hangman
{
    class Hangman
    {
        public static void Main()
        {
            string path = @"countries_and_capitals.txt";

            var countries =
            from line in File.ReadAllLines(path)
            let columns = line.Split('|')
            select new
            {
                country = columns[0].Trim(),
                capital = columns[1].Trim()
            };

            Random rnd = new Random();

            var result = countries.ElementAt(rnd.Next() %countries.Count());

            string regex = "\\S";
            string puzzle = Regex.Replace(result.capital, regex, "_");
            string letters = "";
            string letter = "";
            int lives = 6;
            bool victory = false;

            Console.WriteLine("Welcome to Hangman: World Capitals! You have six lives. Guessing a letter wrongly costs one life, but guessing the word wrongly costs two, so choose wisely!");

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
                        if (!result.capital.Contains(letter))
                        {
                            lives-=2;
                        }
                        else
                        {
                            victory = true;
                        }
                        break;
                }
                puzzle = Regex.Replace(result.capital, regex, "_");
                if (lives < 1)
                {
                    Console.WriteLine("Game over!");
                    break;
                }
                if (victory || puzzle.Equals(result.capital))
                {
                    Console.WriteLine($"Correct, it's {result.capital}, the capital of {result.country}. Congratulations, you won!");
                    break;
                }
            }
        }
    }
}
