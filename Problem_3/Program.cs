using System;
using System.IO;

namespace Problem_3
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputLines = File.ReadAllLines(args[1]);
            long numberOrTrees1 = Solve1(inputLines);
            long numberOrTrees2 = Solve2(inputLines);
            Console.WriteLine($"Part 1 solution {numberOrTrees1}");
            Console.WriteLine($"Part 2 solution {numberOrTrees2}");

            Console.ReadKey();
        }

        private static long Solve1(string[] inputLines)
        {
            Map map = new Map(inputLines);
            long numberOfTrees = GetNumberOfTrees(map, 3, 1);
            return numberOfTrees;
        }

        private static long Solve2(string[] inputLines)
        {
            Map map = new Map(inputLines);
            return ResolveSlopes(map, (1, 1), (3, 1), (5, 1), (7, 1), (1, 2));
        }

        private static long ResolveSlopes(Map map, params (int right, int down)[] slopes)
        {
            long multipliedNumberOfTrees = 1;
            foreach (var (right, down) in slopes)
            {
                multipliedNumberOfTrees *= GetNumberOfTrees(map, right, down);
            }

            return multipliedNumberOfTrees;
        }

        private static long GetNumberOfTrees(Map map, int right, int down)
        {
            int numberOfTrees = 0;
            while (map.Move(right, down, out bool isTree))
            {
                if (isTree) numberOfTrees++;
            }

            map.ResetPosition();
            return numberOfTrees;
        }

        class Map
        {
            private readonly string[] _rows;
            private (int x, int y) currentLocation = (0, 0);
            public Map(string[] rows)
            {
                _rows = rows;
            }

            public bool Move(int right, int down, out bool isTree)
            {
                isTree = false;
                (int x, int y) nextLocation = (currentLocation.x + right, currentLocation.y + down);

                if (nextLocation.y > _rows.Length - 1)
                    return false;

                var d = nextLocation.x - _rows[nextLocation.y].Length;
                if (d >= 0)
                {
                    nextLocation = (d, nextLocation.y);
                }

                isTree = IsTree(nextLocation);

                currentLocation = nextLocation;
                return true;
            }

            public void ResetPosition()
            {
                currentLocation = (0, 0);
            }

            private bool IsTree((int x, int y) nextLocation)
            {
                string landingRow = _rows[nextLocation.y];
                return landingRow[nextLocation.x] == '#';
            }
        }
    }
}