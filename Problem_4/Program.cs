using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;

namespace Problem_4
{
    class Program
    {
        public const string NoUnit = "nu";

        static void Main(string[] args)
        {
            var inputLines = File.ReadAllText(args[1]).Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            int numberOfValidPassports1 = Solve1(inputLines);
            int numberOfValidPassports2 = Solve2(inputLines);
            Console.WriteLine($"Part 1 solution {numberOfValidPassports1}");
            Console.WriteLine($"Part 1 solution {numberOfValidPassports2}");

            Console.ReadKey();
        }

        #region part2
        private static int Solve2(string[] passports)
        {
            ReadOnlyDictionary<string, Rule> rules = DefineRules();
            int validPassports = 0;
            foreach (var passport in passports)
            {
                if (!AllNeededInfoExists(passport)) continue;

                IEnumerable<Token> tokens = GetTokens(passport);
                if (!IsDataValid(tokens, rules)) continue;

                validPassports++;
            }

            return validPassports;
        }

        private static ReadOnlyDictionary<string, Rule> DefineRules()
        {
            Dictionary<string, Rule> rules = new Dictionary<string, Rule>()
            {
                {"byr", new MinMaxNumberRule(4, new Dictionary<string, (int?, int?)>() { { NoUnit, (1920, 2002) } }) },
                {"iyr", new MinMaxNumberRule(4, new Dictionary<string, (int?, int?)>() { { NoUnit, (2010, 2020) } }) },
                {"eyr", new MinMaxNumberRule(4, new Dictionary<string, (int?, int?)>() { { NoUnit, (2020, 2030) } }) },
                {"hgt", new MinMaxNumberRule(null, new Dictionary<string, (int?, int?)>() { { "cm", (150, 193) }, { "in", (59, 76) } }) },
                {"hcl", new HexColorRule() },
                {"ecl", new StringColorRule() },
                {"pid", new MinMaxNumberRule(9, new Dictionary<string, (int?, int?)>() { { NoUnit, (null, null) } }) },
            };

            return new ReadOnlyDictionary<string, Rule>(rules);
        }

        private static bool IsDataValid(IEnumerable<Token> tokens, ReadOnlyDictionary<string, Rule> rules)
        {
            foreach (var token in tokens)
            {
                if (!rules[token.TokenCode].IsValid(token)) return false;
            }

            return true;
        }

        private static IEnumerable<Token> GetTokens(string passport)
        {
            var data = passport.Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var heightRegex = new Regex(@"(\d+)(cm|in)");
            foreach (var dataEntry in data)
            {
                var keyValueData = dataEntry.Split(':');

                if (keyValueData[0] == "cid") continue;

                var match = heightRegex.Match(keyValueData[1]);

                if (match.Success)
                    yield return new Token(
                        keyValueData[0],
                        match.Groups[1].Value,
                        match.Groups[2].Value);
                else
                    yield return new Token(
                        keyValueData[0],
                        keyValueData[1],
                        NoUnit);
            }
        }

        class Token
        {
            public Token(string tokenCode, string tokenValue, string unit)
            {
                TokenCode = tokenCode.Trim();
                TokenValue = tokenValue.Trim();
                Unit = unit.Trim();
            }

            public string TokenCode { get; }
            public string TokenValue { get; }
            public string Unit { get; }
        }

        class StringColorRule : Rule
        {
            private readonly static List<string> allowedValues = new List<string>() { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

            public override bool IsValid(Token token)
            {
                return allowedValues.Contains(token.TokenValue);
            }
        }

        class MinMaxNumberRule : Rule
        {
            private readonly int? _numberOfDigits;
            private readonly ReadOnlyDictionary<string, (int? min, int? max)> _minMaxPerUnit;

            public MinMaxNumberRule(
                int? numberOfDigits,
                Dictionary<string, (int?, int?)> minMaxPerUnit)
            {
                _numberOfDigits = numberOfDigits;
                _minMaxPerUnit = new ReadOnlyDictionary<string, (int? min, int? max)>(minMaxPerUnit);
            }

            public override bool IsValid(Token token)
            {
                if (!_minMaxPerUnit.ContainsKey(token.Unit)) return false;
                (int? min, int? max) = _minMaxPerUnit[token.Unit];

                if (_numberOfDigits.HasValue)
                    if (token.TokenValue.Length != _numberOfDigits) return false;

                int ruleValue = int.Parse(token.TokenValue);

                bool a = (!min.HasValue || ruleValue >= min)
                    && (!max.HasValue || ruleValue <= max);
                return a;
            }
        }

        class HexColorRule : Rule
        {
            private const int NumberOfChars = 7;
            private const int MinCharCode = 'a';
            private const int MaxCharCode = 'f';

            public override bool IsValid(Token token)
            {
                if (token.TokenValue.Length != NumberOfChars) return false;
                if (token.TokenValue[0] != '#') return false;

                foreach (var c in token.TokenValue[1..])
                {
                    bool validChar = IsValidDigit(c) || IsValidLetter(c);
                    if (!validChar) return false;
                }

                return true;
            }

            private bool IsValidLetter(char c)
            {
                return c >= MinCharCode && c <= MaxCharCode;
            }

            private bool IsValidDigit(char c)
            {
                return int.TryParse(c.ToString(), out _);
            }
        }

        abstract class Rule
        {
            public abstract bool IsValid(Token token);
        }
        #endregion part2

        #region part1
        private static int Solve1(string[] passports)
        {

            int validPassports = 0;
            foreach (var passport in passports)
            {
                if (AllNeededInfoExists(passport))
                    validPassports++;
            }

            return validPassports;
        }

        private static bool AllNeededInfoExists(string passport)
        {
            string[] rules = { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

            foreach (var rule in rules)
            {
                if (!passport.Contains(rule))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion part1
    }
}