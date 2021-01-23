using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Problem_9
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = File.ReadAllLines(args[1])
                .SelectMany(l => l.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                .Select(ln => long.Parse(ln))
                .ToArray();

            (long invalidNumber, int index) = SolvePart1(numbers, 25);
            long encryptionWeakness = SolvePart2(invalidNumber, index, numbers);

            Console.WriteLine($"Part 1 solution {invalidNumber}");
            Console.WriteLine($"Part 2 solution {encryptionWeakness}");

            Console.ReadKey();
        }

        private static (long invalidNumber, int index) SolvePart1(long[] numbers, int preamble)
        {
            for (int i = preamble; i < numbers.Length; i++)
            {
                var preambleList = GetPreambleList(numbers, preamble, i);
                var success = GetPreambleSum(preambleList, numbers[i]);
                if (!success)
                    return (numbers[i], i);
            }

            throw new Exception("Not expected that all numbers are valid.");
        }

        private static long SolvePart2(long invalidNumber, int index, long[] numbers)
        {
            Queue<long> numbersQueue = new Queue<long>();
            long sum = 0;
            for (int i = index - 1; i >= 0; i--)
            {
                long number = numbers[i];
                if (number > invalidNumber)
                {
                    sum = 0;
                    numbersQueue.Clear();
                    continue;
                }

                if (numbersQueue.Count == 0)
                {
                    numbersQueue.Enqueue(number);
                    if (sum != 0) throw new Exception($"Sum is expected to be 0, sum = {sum}");

                    sum += number;
                    continue;
                }

                numbersQueue.Enqueue(number);
                sum += number;
                if (sum > invalidNumber)
                {
                    sum -= numbersQueue.Dequeue();
                }

                if (sum == invalidNumber)
                {
                    if (numbersQueue.Count > 1) break;
                    sum -= numbersQueue.Dequeue();
                }
            }

            long min = numbersQueue.Min();
            long max = numbersQueue.Max();
            return min + max;
        }

        private static bool GetPreambleSum(long[] preambleList, long number)
        {
            foreach (int pn in preambleList)
            {
                long numToSearch = number - pn;
                if (numToSearch == pn) continue;

                if (preambleList.Contains(numToSearch))
                    return true;
            }

            return false;
        }

        private static long[] GetPreambleList(long[] numbers, int preamble, int i) => numbers[(i - preamble)..i];
    }
}