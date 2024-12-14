using System.Data;
using System.Drawing;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

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

            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == 'X')
                    {
                        total1 += CountXmas(lines, i, j);
                    }

                    if (lines[i][j] == 'A'
                        && i < lines.Count - 1 && j < lines[i].Count() - 1 && i >= 1 && j >= 1
                        && IsMas(lines, i, j))
                    {
                        total2++;
                    }
                }
            }

            sb.AppendLine(WriteToSb($"D4P1 : {total1}", isTest));
            sb.AppendLine(WriteToSb($"D4P2 : {total2}", isTest));

            return sb.ToString();
        }

        public string Day5(bool isTest)
        {
            var sb = new StringBuilder();
            var lines = DataLoader.GetLines(5, isTest);
            long total1 = 0, total2 = 0;
            var rules = new List<Tuple<int, int>>();
            var updates = new List<List<int>>();
            foreach (var line in lines)
            {
                if (line.Contains("|"))
                {
                    var values = line.Split("|");
                    rules.Add(new Tuple<int, int>(int.Parse(values.First()), int.Parse(values.Last())));
                }
                else if (line.Contains(","))
                {
                    updates.Add(line.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList());
                }
            }

            foreach (var update in updates)
            {
                if (IsSorted(rules, update))
                {
                    total1 += update[update.Count / 2];
                }
                else
                {
                    var sortedUpdate = Sort(rules, update.ToList());
                    total2 += sortedUpdate[sortedUpdate.Count / 2];
                }
            }

            sb.AppendLine(WriteToSb($"D5P1 : {total1}", isTest));
            sb.AppendLine(WriteToSb($"D5P2 : {total2}", isTest));

            return sb.ToString();
        }

        public string Day6(bool isTest)
        {
            var sb = new StringBuilder();
            var lines = DataLoader.GetLines(6, isTest);
            long total1 = 0, total2 = 0;

            var map = new SentryMap(lines);

            while (!map.IsMoveComplete)
            {
                map.MoveToNext();
            }

            total1 = map.CountVisited();
            var previouslyVisited = map.GetVisited();
            for (int y = 0; y < map.NumRows; y++)
            {
                for (int x = 0; x < map.NumCols; x++)
                {
                    if (map.GetCharAtLocation(x, y) == '.' && previouslyVisited[y][x])
                    {
                        map.Reset();
                        map.SetCharAtLocation(x, y, '#');
                        if (map.CheckForLoop())
                        {
                            total2++;
                        }

                        map.SetCharAtLocation(x, y, '.');
                    }
                }
            }
            sb.AppendLine(WriteToSb($"D6P1 : {total1}", isTest));
            sb.AppendLine(WriteToSb($"D6P2 : {total2}", isTest));

            return sb.ToString();
        }

        public string Day7(bool isTest)
        {
            var sb = new StringBuilder();
            var lines = DataLoader.GetLines(7, isTest);
            long total1 = 0, total2 = 0;
            foreach (var line in lines)
            {
                string[] temp = line.Split(":");
                var testValue = long.Parse(temp.First());
                var values = temp.Last().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();

                var ops = Math.Pow(3, values.Count);
                for (int i = 0; i < ops; i++)
                {
                    var total = (long)values.First();
                    var usesConcat = false;
                    for (int j = 0; j < values.Count - 1; j++)
                    {
                        var index = i / (int)Math.Pow(3, j) % 3;
                        if (index == 0)
                        {
                            total += values[j + 1];
                        }
                        else if (index == 1)
                        {
                            total *= values[j + 1];
                        }
                        else if (index == 2)
                        {
                            usesConcat = true;
                            total = long.Parse($"{total}{values[j + 1]}");
                        }
                    }

                    if (total == testValue)
                    {
                        if (!usesConcat)
                        {
                            total1 += testValue;
                        }

                        total2 += testValue;
                        break;
                    }
                }
            }
            sb.AppendLine(WriteToSb($"D7P1 : {total1}", isTest));
            sb.AppendLine(WriteToSb($"D7P2 : {total2}", isTest));

            return sb.ToString();
        }

        public string Day8(bool isTest)
        {
            var sb = new StringBuilder();
            var lines = DataLoader.GetLines(8, isTest);
            long total1 = 0, total2 = 0;
            var map = new Map2D(lines);
            var antennas = new List<Antenna>();
            var antinodes = new List<Point>();
            for (int y = 0; y < map.NumRows; y++)
            {
                for (int x = 0; x < map.NumCols; x++)
                {
                    char freq = map.GetCharAtLocation(x, y);
                    if (freq != '.')
                    {
                        antennas.Add(new Antenna(freq, new System.Drawing.Point(x, y)));
                    }
                }
            }

            foreach (var frequency in antennas.Select(x => x.Frequency).Distinct())
            {
                var relevantAntennas = antennas.Where(x => x.Frequency == frequency).ToList();
                for (int i = 0; i < relevantAntennas.Count - 1; i++)
                {
                    var firstAntenna = relevantAntennas[i];
                    for (int j = i + 1; j < relevantAntennas.Count; j++)
                    {
                        var secondAntenna = relevantAntennas[j];
                        var vector = secondAntenna.Location - new Size(firstAntenna.Location);
                        var antinode1 = firstAntenna.Location - new Size(vector);
                        var antinode2 = secondAntenna.Location + new Size(vector);
                        if (!antinodes.Contains(antinode1) && !map.IsLocationOutOfBounds(antinode1))
                        {
                            total1++;
                            antinodes.Add(antinode1);
                        }

                        if (!antinodes.Contains(antinode2) && !map.IsLocationOutOfBounds(antinode2))
                        {
                            total1++;
                            antinodes.Add(antinode2);
                        }

                        if (!antinodes.Contains(firstAntenna.Location))
                        {
                            antinodes.Add(firstAntenna.Location);
                        }

                        if (!antinodes.Contains(secondAntenna.Location))
                        {
                            antinodes.Add(secondAntenna.Location);
                        }

                        var smallestVector = FindSmallestVector(vector);
                        bool smallestFound = false, largestFound = false;
                        var small = firstAntenna.Location;
                        var large = firstAntenna.Location;
                        while (!smallestFound || !largestFound)
                        {
                            small = small - new Size(smallestVector);
                            large = large + new Size(smallestVector);
                            smallestFound = map.IsLocationOutOfBounds(small);
                            if (!antinodes.Contains(small) && !smallestFound)
                            {
                                antinodes.Add(small);
                            }

                            largestFound = map.IsLocationOutOfBounds(large);
                            if (!antinodes.Contains(large) && !largestFound)
                            {
                                antinodes.Add(large);
                            }
                        }
                    }
                }
            }

            total2 = antinodes.Count;

            sb.AppendLine(WriteToSb($"D8P1 : {total1}", isTest));
            sb.AppendLine(WriteToSb($"D8P2 : {total2}", isTest));

            return sb.ToString();
        }

        public string Day9(bool isTest)
        {
            var sb = new StringBuilder();
            var lines = DataLoader.GetLines(9, isTest);
            long total1 = DefragmentAndCalcChecksum(lines.Single());
            long total2 = MoveFilesAndCalcChecksum(lines.Single());

            sb.AppendLine(WriteToSb($"D9P1 : {total1}", isTest));
            sb.AppendLine(WriteToSb($"D9P2 : {total2}", isTest));

            return sb.ToString();
        }

        public string Day10(bool isTest)
        {
            var sb = new StringBuilder();
            var lines = DataLoader.GetLines(10, isTest);
            long total1 = 0, total2 = 0;

            var map = new Map2D(lines);
            var pathsByTrailhead = new Dictionary<Point, List<Point>>();

            for (int i = 0; i < map.NumRows; i++)
            {
                for (int j = 0; j < map.NumCols; j++)
                {
                    if (map.GetCharAtLocation(j, i) == '0')
                    {
                        pathsByTrailhead.Add(new Point(j, i), new List<Point>() { new Point(j, i) });
                    }
                }
            }

            foreach (var trailhead in pathsByTrailhead)
            {
                var nines = new List<Point>();
                var lastLocations = trailhead.Value;
                while (lastLocations.Any())
                {
                    var newLocations = new List<Point>();
                    foreach (var point in lastLocations)
                    {
                        var currentValue = int.Parse($"{map.GetCharAtLocation(point)}");
                        var nextPoints = new List<Point>()
                        {
                            point + new Size(1, 0),
                            point + new Size(0, 1),
                            point + new Size(-1, 0),
                            point + new Size(0, -1),
                        };

                        foreach (var nextPoint in nextPoints)
                        {
                            if (!map.IsLocationOutOfBounds(nextPoint) && (int.Parse($"{map.GetCharAtLocation(nextPoint)}") == currentValue + 1))
                            {
                                if (int.Parse($"{map.GetCharAtLocation(nextPoint)}") == 9)
                                {
                                    nines.Add(nextPoint);
                                }
                                else
                                {
                                    newLocations.Add(nextPoint);
                                }
                            }
                        }
                    }

                    lastLocations = newLocations;
                }

                total1 += nines.Distinct().Count();
                total2 += nines.Count();
            }

            sb.AppendLine(WriteToSb($"D10P1 : {total1}", isTest));
            sb.AppendLine(WriteToSb($"D10P2 : {total2}", isTest));

            return sb.ToString();
        }

        public string Day11(bool isTest)
        {
            var sb = new StringBuilder();
            var lines = DataLoader.GetLines(11, isTest);
            long total1 = 0, total2 = 0;

            var rocks = lines.Single().Split();
            var counts = rocks.GroupBy(x => x).ToDictionary(x => x.Key, x => (long)x.Count());
            var numIterations = 75;
            for (int i = 0; i < numIterations; i++)
            {
                var newCounts = new Dictionary<string, long>();
                foreach (var count in counts)
                {
                    var rock = count.Key;
                    if (rock == "0")
                    {
                        AddToDict(newCounts, "1", count.Value);
                    }
                    else if (rock.Length % 2 == 0)
                    {
                        var rock1 = rock.Substring(0, rock.Length / 2);
                        var rock2 = rock.Substring(rock.Length / 2);
                        AddToDict(newCounts, $"{long.Parse(rock1)}", count.Value);
                        AddToDict(newCounts, $"{long.Parse(rock2)}", count.Value);
                    }
                    else
                    {
                        AddToDict(newCounts, $"{long.Parse(rock) * 2024L}", count.Value);
                    }
                }

                counts = newCounts;
                if (i == 24)
                {
                    total1 = counts.Sum(x => x.Value);
                }
            }

            total2 = counts.Sum(x => x.Value);

            sb.AppendLine(WriteToSb($"D11P1 : {total1}", isTest));
            sb.AppendLine(WriteToSb($"D11P2 : {total2}", isTest));

            return sb.ToString();
        }

        public string Day12(bool isTest)
        {
            var sb = new StringBuilder();
            var lines = DataLoader.GetLines(12, isTest);
            long total1 = 0, total2 = 0;
            var map = new Map2D(lines);
            var visited = new List<List<bool>>();
            for (int i = 0; i < map.NumRows; i++)
            {
                visited.Add(Enumerable.Repeat(false, map.NumCols).ToList());
            }

            while (!visited.All(x => x.All(x => x)))
            {
                var startPoint = GetNextNotVisited(visited);
                visited[startPoint.Y][startPoint.X] = true;
                if (startPoint == new Point(-1, -1))
                {
                    break;
                }

                var testChar = map.GetCharAtLocation(startPoint);
                var lastPoints = new List<Point>() { startPoint };
                var perimeter = 0;
                var area = 1;
                var sides = new List<FenceSide>();
                while (lastPoints.Any())
                {
                    var newPoints = new List<Point>();

                    foreach (var point in lastPoints)
                    {
                        var nextPoints = new List<Point>()
                        {
                            point + new Size(1 , 0),
                            point + new Size(-1 , 0),
                            point + new Size(0, 1),
                            point + new Size(0, -1),
                        };

                        foreach (var nextPoint in nextPoints)
                        {
                            if (map.IsLocationOutOfBounds(nextPoint))
                            {
                                perimeter++;
                                sides = HandleSides(sides, point, nextPoint);
                            }
                            else if (map.GetCharAtLocation(nextPoint) == testChar
                                && !visited[nextPoint.Y][nextPoint.X])
                            {
                                area++;
                                visited[nextPoint.Y][nextPoint.X] = true;
                                newPoints.Add(nextPoint);
                            }
                            else if (map.GetCharAtLocation(nextPoint) != testChar)
                            {
                                perimeter++;
                                sides = HandleSides(sides, point, nextPoint);
                            }
                        }
                    }

                    lastPoints = newPoints;
                }

                total1 += perimeter * area;
                total2 += sides.Count * area;
            }

            sb.AppendLine(WriteToSb($"D12P1 : {total1}", isTest));
            sb.AppendLine(WriteToSb($"D12P2 : {total2}", isTest));

            return sb.ToString();
        }
        public string Day13(bool isTest)
        {
            var sb = new StringBuilder();
            var lines = DataLoader.GetLines(13, isTest);
            long total1 = 0, total2 = 0;

            while (lines.Any())
            {
                if (string.IsNullOrWhiteSpace(lines.First()))
                {
                    lines = lines.Skip(1).ToList();
                    continue;
                }

                var group = lines.Take(3).ToList();
                var clawGame = new ClawGame(group);
                total1 += clawGame.GetMinTokens(true);
                total2 += clawGame.GetMinTokens(false);
                lines = lines.Skip(3).ToList();
            }

            sb.AppendLine(WriteToSb($"D13P1 : {total1}", isTest));
            sb.AppendLine(WriteToSb($"D13P2 : {total2}", isTest));

            return sb.ToString();
        }

        public string Day14(bool isTest)
        {
            var sb = new StringBuilder();
            var lines = DataLoader.GetLines(14, isTest);
            long total1 = 0, total2 = 0;
            Point size;
            if (isTest)
            {
                size = new Point(11, 7);
            }
            else
            {
                size = new Point(101, 103);
            }

            var robots = lines.Select(x => new Robot(x, size)).ToList();
            var finalPositions = robots.Select(x => x.GetPositionAfterSteps(100)).ToList();

            var middle = new Point((size.X - 1) / 2, (size.Y - 1) / 2);
            var q1 = finalPositions.Count(p => p.X < middle.X && p.Y < middle.Y);
            var q2 = finalPositions.Count(p => p.X > middle.X && p.Y < middle.Y);
            var q3 = finalPositions.Count(p => p.X < middle.X && p.Y > middle.Y);
            var q4 = finalPositions.Count(p => p.X > middle.X && p.Y > middle.Y);
            total1 = q1 * q2 * q3 * q4;

            sb.AppendLine(WriteToSb($"D14P1 : {total1}", isTest));
            if (!isTest)
            {
                total2 = 7603;
                sb.AppendLine(WriteToSb($"D14P2 : {total2}", isTest));
                finalPositions = robots.Select(x => x.GetPositionAfterSteps((int)total2)).ToList();
                Print(size, finalPositions, (int)total2);
                //for (int i = 1; i < 30000; i++)
                //{
                //    finalPositions = robots.Select(x => x.GetPositionAfterSteps(i)).ToList();
                //    if (finalPositions.Count(x => x.X > 25 && x.X < 50) > finalPositions.Count / 2)
                //    Print(size, finalPositions, i);
                //}
            }

            return sb.ToString();
        }

        private static void Print(Point size, List<Point> finalPositions, int iteration)
        {
            var hasRobot = new List<List<int>>();

            Console.WriteLine($"D14P2 Iteration {iteration}:");
            for (int y = 0; y < size.Y; y++)
            {
                hasRobot.Add(Enumerable.Repeat(0, size.X).ToList());
            }

            foreach (var finalPosition in finalPositions)
            {
                hasRobot[finalPosition.Y][finalPosition.X]++;
            }

            for (int y = 0; y < size.Y; y++)
            {
                for (int x = 0; x < size.X; x++)
                {
                    if (hasRobot[y][x] > 0)
                    {
                        Console.Write($"{hasRobot[y][x]}");
                    }
                    else
                    {
                        Console.Write(" ");
                    }

                }

                Console.WriteLine();
            }
        }

        private List<FenceSide> HandleSides(List<FenceSide> sides, Point point, Point nextPoint)
        {
            var direction = GetDirection(nextPoint, point);
            var sides2 = sides.Where(x => x.ContinuesSide(nextPoint, direction));
            if (sides2.Count() > 1)
            {
                sides = sides.Except(sides2).ToList();
                var side = new FenceSide();
                side.Direction = direction;
                side.Points.Add(nextPoint);
                side.Points.AddRange(sides2.SelectMany(x => x.Points));
                sides.Add(side);
            }
            else
            {
                var side = sides2.SingleOrDefault();
                if (side is null)
                {
                    side = new FenceSide();
                    side.Direction = direction;
                    sides.Add(side);
                }

                side.Points.Add(nextPoint);
            }

            return sides;
        }

        private Direction GetDirection(Point nextPoint, Point point)
        {
            var diff = nextPoint - new Size(point);

            if (diff == new Point(0, 1))
            {
                return Direction.Down;
            }
            if (diff == new Point(0, -1))
            {
                return Direction.Up;
            }
            if (diff == new Point(-1, 0))
            {
                return Direction.Left;
            }
            if (diff == new Point(1, 0))
            {
                return Direction.Right;
            }

            throw new Exception();
        }

        private Point GetNextNotVisited(List<List<bool>> visited)
        {
            for (int y = 0; y < visited.Count; y++)
            {
                for (int x = 0; x < visited[y].Count; x++)
                {
                    if (!visited[y][x])
                    {
                        return new Point(x, y);
                    }
                }
            }

            return new Point(-1, -1);
        }

        private void AddToDict(Dictionary<string, long> newCounts, string rock, long value)
        {
            if (newCounts.ContainsKey(rock))
            {
                newCounts[rock] += value;
            }
            else
            {
                newCounts.Add(rock, value);
            }
        }

        private long MoveFilesAndCalcChecksum(string line)
        {
            long total = 0;
            var index = 0;
            var moved = new List<int>();
            for (int i = 0; i < line.Length; i++)
            {
                var value = int.Parse(line[i].ToString());
                if (i % 2 == 0)
                {
                    var id = i / 2;
                    var blockSize = value;
                    if (!moved.Contains(i))
                    {
                        int k = 0;
                        while (blockSize > 0)
                        {
                            total += id * (index + k++);
                            blockSize--;
                        }
                    }
                }
                else
                {
                    int j = line.Length - 1;
                    if (line.Length % 2 == 0)
                    {
                        j--;
                    }

                    int k = 0;
                    var gap = value;
                    for (; j > i; j -= 2)
                    {
                        if (moved.Contains(j))
                        {
                            continue;
                        }

                        var id = j / 2;
                        var endValue = int.Parse(line[j].ToString());

                        if (gap >= endValue)
                        {
                            moved.Add(j);
                            while (endValue > 0)
                            {
                                total += id * (index + k++);
                                endValue--;
                                gap--;
                            }

                            if (gap == 0)
                            {
                                break;
                            }
                        }
                    }
                }

                index += value;
            }

            return total;
        }

        private long DefragmentAndCalcChecksum(string line)
        {
            long total1 = 0;
            int j = line.Length - 1;
            if (line.Length % 2 == 0)
            {
                j--;
            }

            var endValue = int.Parse(line[j].ToString());
            var index = 0;
            for (int i = 0; i <= j; i++)
            {
                var value = int.Parse(line[i].ToString());
                if (i % 2 == 0)
                {
                    if (i == j)
                    {
                        value = endValue;
                    }

                    var id = i / 2;
                    while (value > 0)
                    {
                        total1 += id * index++;
                        value--;
                    }
                }
                else
                {
                    var gap = value;
                    var id = j / 2;
                    while (gap > 0)
                    {
                        if (endValue == 0)
                        {
                            j -= 2;
                            endValue = int.Parse(line[j].ToString());
                            id = j / 2;
                        }
                        else
                        {
                            total1 += id * index++;
                            gap--;
                            endValue--;
                        }
                    }
                }
            }

            return total1;
        }

        private long CountXmas(List<string> lines, int i, int j)
        {
            var total = 0;
            var enumerable = Enumerable.Range(0, 4);

            // right
            if (j < lines[i].Count() - 3 && string.Concat(enumerable.Select(x => lines[i][j + x])) == "XMAS")
            {
                total++;
            }

            // left
            if (j >= 3 && string.Concat(enumerable.Select(x => lines[i][j - x])) == "XMAS")
            {
                total++;
            }

            // down
            if (i < lines.Count - 3 && string.Concat(enumerable.Select(x => lines[i + x][j])) == "XMAS")
            {
                total++;
            }

            // up
            if (i >= 3 && string.Concat(enumerable.Select(x => lines[i - x][j])) == "XMAS")
            {
                total++;
            }

            // down right
            if (i < lines.Count - 3 && j < lines[i].Count() - 3 && string.Concat(enumerable.Select(x => lines[i + x][j + x])) == "XMAS")
            {
                total++;
            }

            // down left
            if (i < lines.Count - 3 && j >= 3 && string.Concat(enumerable.Select(x => lines[i + x][j - x])) == "XMAS")
            {
                total++;
            }

            // up right
            if (i >= 3 && j < lines[i].Count() - 3 && string.Concat(enumerable.Select(x => lines[i - x][j + x])) == "XMAS")
            {
                total++;
            }

            // up left
            if (i >= 3 && j >= 3 && string.Concat(enumerable.Select(x => lines[i - x][j - x])) == "XMAS")
            {
                total++;
            }

            return total;
        }

        private Point FindSmallestVector(Point vector)
        {
            var primes = new List<int>() { 2, 3, 5, 7, 11, 13, 17, 19, 23 };
            int i = 0;
            while (i < primes.Count)
            {
                if (vector.X % primes[i] == 0 && vector.Y % primes[i] == 0)
                {
                    vector = new Point(vector.X / primes[i], vector.Y / primes[i]);
                    i = 0;
                }
                else
                {
                    i++;
                }
            }

            return vector;
        }

        private bool IsLevelOK(List<int> values)
        {
            var levels = values.Skip(1).Select((x, i) => x - values[i]).ToList();
            var allIncrease = levels.All(x => x > 0);
            var allDecrease = levels.All(x => x < 0);
            var diffOK = levels.All(x => Math.Abs(x) <= 3 && Math.Abs(x) >= 1);

            return (allIncrease || allDecrease) && diffOK;
        }

        private bool IsMas(List<string> lines, int i, int j)
        {
            var enumerable = Enumerable.Range(-1, 3);
            string downRight = string.Concat(enumerable.Select(x => lines[i + x][j + x]));
            string upRight = string.Concat(enumerable.Select(x => lines[i - x][j + x]));
            return (downRight == "MAS" || downRight == "SAM") &&
                (upRight == "MAS" || upRight == "SAM");
        }

        private bool IsSorted(List<Tuple<int, int>> rules, List<int> update)
        {
            var isSorted = true;
            for (var i = 0; i < update.Count; i++)
            {
                var value = update[i];
                var remainingItems = update.Skip(i).Take(update.Count - i).ToList();
                var relevantOrders = rules.Where(x => x.Item2 == value).ToList();
                if (relevantOrders.Any(x => remainingItems.Contains(x.Item1)))
                {
                    isSorted = false;
                    break;
                }
            }

            return isSorted;
        }

        private List<int> Sort(List<Tuple<int, int>> rules, List<int> pagesToSort)
        {
            var sortedUpdate = new List<int>();
            while (pagesToSort.Any())
            {
                for (var i = 0; i < pagesToSort.Count; i++)
                {
                    var page = pagesToSort[i];
                    var remainingItems = pagesToSort.Skip(i).Take(pagesToSort.Count - i).ToList();
                    var relevantOrders = rules.Where(x => x.Item2 == page).ToList();
                    if (!relevantOrders.Any(x => remainingItems.Contains(x.Item1)))
                    {
                        sortedUpdate.Add(pagesToSort[i]);
                        pagesToSort.Remove(pagesToSort[i]);
                        break;
                    }
                }
            }

            return sortedUpdate;
        }

        private string WriteToSb(string input, bool isTest)
        {
            var teststr = isTest ? "[TEST] " : "";
            return $"{teststr}{input}";
        }
    }
}