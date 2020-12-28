using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Problem_5
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputLines = File.ReadAllLines(args[1]);
            int highestSeatId = SolvePart1(inputLines);
            int mySeat = SolvePart2(inputLines);
            Console.WriteLine($"Part 1 solution {highestSeatId}");
            Console.WriteLine($"Part 2 solution {mySeat}");
            Console.ReadKey();
        }

        private static int SolvePart1(string[] inputLines)
        {
            int biggestSeatId = -1;
            foreach (var seatCode in inputLines)
            {
                int row = GetRow(seatCode);
                int column = GetColumn(seatCode);
                int seatId = CalculateSeatId(row, column);
                if (seatId > biggestSeatId)
                    biggestSeatId = seatId;
            }

            if (biggestSeatId == -1)
                throw new Exception("Biggest seat id not set");

            return biggestSeatId;
        }

        private static int SolvePart2(string[] inputLines)
        {
            List<int> seatIds = new List<int>();
            foreach (var seatCode in inputLines)
            {
                int row = GetRow(seatCode);
                int column = GetColumn(seatCode);
                int seatId = CalculateSeatId(row, column);
                seatIds.Add(seatId);
            }

            if (seatIds.Count < 1)
                throw new Exception("Seat ids collection is empty.");

            return FindMissingSeat(seatIds.OrderBy(s => s));
        }

        private static int FindMissingSeat(IOrderedEnumerable<int> orderedEnumerables)
        {
            var seatIds = orderedEnumerables.ToList();
            for (int i = 0; i < seatIds.Count; i++)
            {
                if (i == 0) continue;
                if (seatIds[i] == seatIds[i - 1] + 2) return seatIds[i] - 1;
            }

            throw new Exception("Could not find seat");
        }

        private static int GetRow(string seatCode)
        {
            int min = 0;
            int max = 127;

            for (int i = 0; i < 7; i++)
            {
                int middle = (max + min) / 2;
                switch (seatCode[i])
                {
                    case 'F':
                        max = middle;
                        break;
                    case 'B':
                        min = middle + 1;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("command", seatCode[i], "Not supported command");
                }
            }

            if (min != max) throw new Exception("Range min and max is expected to be the same");

            return max;
        }

        private static int GetColumn(string seatCode)
        {
            int min = 0;
            int max = 7;

            for (int i = 7; i < 10; i++)
            {
                int middle = (max + min) / 2;
                switch (seatCode[i])
                {
                    case 'L':
                        max = middle;
                        break;
                    case 'R':
                        min = middle + 1;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("command", seatCode[i], "Not supported command");
                }
            }

            if (min != max) throw new Exception("Range min and max is expected to be the same");

            return max;
        }

        private static int CalculateSeatId(int row, int column) => row * 8 + column;
    }
}