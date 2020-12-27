using System;
using System.IO;
using System.Linq;

namespace Problem_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputLines = File.ReadAllLines(args[1]).Select(int.Parse).ToArray();
            var solution1 = SolvePart1(inputLines);
            var solution2 = SolvePart2(inputLines);
            Console.WriteLine($"Part 1 solution in O(n2): {solution1}");
            Console.WriteLine($"Part 2 solution in O(n3): {solution2}");
            Console.ReadKey();
        }

        private static int SolvePart1(int[] inputLines)
        {
            for (int i = 0; i < inputLines.Length; i++)
            {
                for (int j = 0; j < inputLines.Length; j++)
                {
                    if (inputLines[i] == inputLines[j]) continue;
                    if (inputLines[i] + inputLines[j] == 2020)
                    {
                        return inputLines[i] * inputLines[j];
                    }
                }
            }

            throw new Exception("Could not solve problem");
        }

        private static int SolvePart2(int[] inputLines)
        {
            for (int i = 0; i < inputLines.Length; i++)
            {
                for (int j = 0; j < inputLines.Length; j++)
                {
                    if (inputLines[i] == inputLines[j]) continue;
                    for (int k = 0; k < inputLines.Length; k++)
                    {
                        if (inputLines[i] == inputLines[k] ||
                            inputLines[j] == inputLines[k]) continue;

                        if (inputLines[i] + inputLines[j] + inputLines[k] == 2020)
                            return inputLines[i] * inputLines[j] * inputLines[k];

                    }
                }
            }

            throw new Exception("Could not solve problem");
        }
    }
}
