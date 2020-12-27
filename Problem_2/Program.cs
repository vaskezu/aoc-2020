using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Problem_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputLines = File.ReadAllLines(args[1]);
            var numOfValidPasswords = SolvePart1(inputLines);
            var numOfValidPasswords2 = SolvePart2(inputLines);
            Console.WriteLine($"Part 1 solution {numOfValidPasswords}");
            Console.WriteLine($"Part 2 solution {numOfValidPasswords2}");

            Console.ReadKey();
        }

        private static object SolvePart1(string[] inputLines)
        {
            int validPasswords = 0;

            Regex reg = new Regex(@"(\d+)-(\d+)\s(\w):\s(\w*)");
            foreach (var line in inputLines)
            {
                Match match = reg.Match(line);

                var minValue = int.Parse(match.Groups[1].Value);
                var maxValue = int.Parse(match.Groups[2].Value);
                var letter = Convert.ToChar(match.Groups[3].Value);
                var password = match.Groups[4].Value;

                int validCaracters = 0;
                for (int i = 0; i < password.Length; i++)
                {
                    if (password[i] == letter)
                    {
                        validCaracters++;
                    }
                }

                if (validCaracters >= minValue && validCaracters <= maxValue)
                    validPasswords++;
            }

            return validPasswords;
        }

        private static int SolvePart2(string[] inputLines)
        {
            int validPasswords = 0;

            Regex reg = new Regex(@"(\d+)-(\d+)\s(\w):\s(\w*)");
            foreach (var line in inputLines)
            {
                Match match = reg.Match(line);

                var minValue = int.Parse(match.Groups[1].Value);
                var maxValue = int.Parse(match.Groups[2].Value);
                var letter = Convert.ToChar(match.Groups[3].Value);
                var password = match.Groups[4].Value;

                bool firstPosition = password[minValue - 1] == letter;
                bool secondPosition = password[maxValue - 1] == letter;

                if (firstPosition && secondPosition)
                    continue;

                if (firstPosition || secondPosition)
                    validPasswords++;
            }

            return validPasswords;
        }
    }
}