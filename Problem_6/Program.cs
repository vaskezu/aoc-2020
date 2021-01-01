using System;
using System.Collections.Generic;
using System.IO;

namespace Problem_6
{
    class Program
    {
        static void Main(string[] args)
        {
            List<FlightGroup> groups = CreateGroups(args[1]);
            int answeredAnyQuestions = SolvePart1(groups);
            int answeredAllQuestions = SolvePart2(groups);
            Console.WriteLine($"Part 1 solution {answeredAnyQuestions}");
            Console.WriteLine($"Part 2 solution {answeredAllQuestions}");

            Console.ReadKey();
        }

        private static int SolvePart1(List<FlightGroup> groups)
        {
            int anyoneAnsweredYesPerGroupSum = 0;
            foreach (var group in groups)
            {
                anyoneAnsweredYesPerGroupSum += GetGroupAnyoneAnsweredYesQuestions(group);
            }

            return anyoneAnsweredYesPerGroupSum;
        }

        private static int SolvePart2(List<FlightGroup> groups)
        {
            int allAnsweredYesPerGroupSum = 0;
            foreach (var group in groups)
            {
                allAnsweredYesPerGroupSum += GetGroupAllAnsweredYesQuestions(group);
            }

            if (allAnsweredYesPerGroupSum == 0) throw new Exception("Expected to have some answers.");

            return allAnsweredYesPerGroupSum;
        }

        private static int GetGroupAnyoneAnsweredYesQuestions(FlightGroup group)
        {
            HashSet<char> answered = new HashSet<char>();
            foreach (var personAnswers in group.Answers)
            {
                answered.UnionWith(personAnswers);
            }

            return answered.Count;
        }

        private static int GetGroupAllAnsweredYesQuestions(FlightGroup group)
        {
            HashSet<char> answered = new HashSet<char>();
            answered.UnionWith(group.Answers[0]);

            for (int i = 1; i < group.Answers.Count; i++)
            {
                answered.IntersectWith(group.Answers[i]);
                if (answered.Count == 0) return answered.Count;
            }

            return answered.Count;
        }

        private static List<FlightGroup> CreateGroups(string path)
        {
            List<FlightGroup> groups = new List<FlightGroup>();

            string line;
            FlightGroup currectGroup = new FlightGroup();
            using StreamReader sr = new StreamReader(path);
            while ((line = sr.ReadLine()) != null)
            {
                if (line == string.Empty)
                {
                    groups.Add(currectGroup);
                    currectGroup = new FlightGroup();
                    continue;
                }

                currectGroup.Answers.Add(new List<char>(line.ToCharArray()));
            }

            groups.Add(currectGroup);
            return groups;
        }

        private class FlightGroup
        {
            public FlightGroup()
            {
                Answers = new List<List<char>>();
            }

            public List<List<char>> Answers { get; }
        }
    }
}