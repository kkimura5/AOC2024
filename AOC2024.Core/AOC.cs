
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace AOC2024.Core
{
    public class AOC
    {
        public string Day1(bool isTest)
        {
            var sb = new StringBuilder();
            var lines = DataLoader.GetLines(1, isTest);
            List<int> list1 = new List<int>(), list2 = new List<int>();

            foreach (var line in lines)
            {
                var values = line.Split(' ');
                list1.Add(int.Parse(values.First()));
                list2.Add(int.Parse(values.Last()));
            }

            var ordered1 = list1.OrderBy(X => X).ToList();
            var ordered2 = list2.OrderBy(X => X).ToList();

            var totaldistance = ordered1.Select((x, i) => Math.Abs(x - ordered2[i])).Sum();
            sb.AppendLine(WriteToSb($"D1P1 : {totaldistance}", isTest));

            var similarity = 0L;
            foreach (var value in ordered1)
            {
                var count = ordered2.Count(x => x == value);
                similarity += value * count;
            }

            sb.AppendLine(WriteToSb($"D1P2 : {similarity}", isTest));
            return sb.ToString();
        }

        public string Day2(bool isTest)
        {
            var sb = new StringBuilder();
            var lines = DataLoader.GetLines(2, isTest);
            int total1 = 0, total2 = 0;
            foreach (var line in lines)
            {
                var values = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();
                if (IsLevelOK(values))
                {
                    total1++;
                    total2++;
                }
                else
                {
                    for (int i = 0; i < values.Count; i++)
                    {
                        var newValues = values.Take(i).Concat(values.Skip(i + 1)).ToList();
                        if (IsLevelOK(newValues))
                        {
                            total2++;
                            break;
                        }
                    }
                }
            }

            sb.AppendLine(WriteToSb($"D2P1 : {total1}", isTest));
            sb.AppendLine(WriteToSb($"D2P2 : {total2}", isTest));

            return sb.ToString();
        }

        private static bool IsLevelOK(List<int> values)
        {
            var levels = values.Skip(1).Select((x, i) => x - values[i]).ToList();
            var allIncrease = levels.All(x => x > 0);
            var allDecrease = levels.All(x => x < 0);
            var diffOK = levels.All(x => Math.Abs(x) <= 3 && Math.Abs(x) >= 1);

            return (allIncrease || allDecrease) && diffOK;
        }

        public string WriteToSb(string input, bool isTest)
        {
            var teststr = isTest ? "[TEST] " : "";
            return $"{teststr}{input}";
        }

        public string Day3(bool isTest)
        {
            var sb = new StringBuilder();
            var lines = DataLoader.GetLines(3, isTest);
            long total1 = 0, total2 = 0;
            var pattern = @"mul\((?<num1>\d+),(?<num2>\d+)\)";
            var pattern2 = @"do\(\)";
            var pattern3 = @"don't\(\)";
            var all = string.Join(Environment.NewLine, lines);
            var onIndices = Regex.Matches(all, pattern2).Select(x => x.Index).ToList();
            var offIndices = Regex.Matches(all, pattern3).Select(x => x.Index).ToList();
            foreach (Match match in Regex.Matches(all, pattern))
            {
                int value = int.Parse(match.Groups["num1"].Value) * int.Parse(match.Groups["num2"].Value);
                total1 += value;
                var lastOnIndex = onIndices.LastOrDefault(x => match.Index > x);
                var lastOffIndex = offIndices.LastOrDefault(x => match.Index > x);
                if (lastOffIndex < lastOnIndex || (lastOnIndex == 0 && lastOffIndex == 0))
                {
                    total2 += value;
                }
            }

            sb.AppendLine(WriteToSb($"D3P1 : {total1}", isTest));
            sb.AppendLine(WriteToSb($"D3P2 : {total2}", isTest));

            return sb.ToString();
        }

        public string Day4(bool isTest)
        {
            var sb = new StringBuilder();
            var lines = DataLoader.GetLines(4, isTest);
            long total1 = 0, total2 = 0;

            sb.AppendLine(WriteToSb($"D4P1 : {total1}", isTest));
            sb.AppendLine(WriteToSb($"D4P2 : {total2}", isTest));

            return sb.ToString();
        }
    }
}
