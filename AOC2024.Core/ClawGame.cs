using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2024.Core
{
    public class ClawGame
    {
        public ClawGame(List<string> lines)
        {
            var buttonA = Regex.Match(lines[0], @"X\+(?<X>\d+), Y\+(?<Y>\d+)");
            var buttonB = Regex.Match(lines[1], @"X\+(?<X>\d+), Y\+(?<Y>\d+)");
            var prize = Regex.Match(lines[2], @"X=(?<X>\d+), Y=(?<Y>\d+)");
            ButtonA = new Point(int.Parse(buttonA.Groups["X"].Value), int.Parse(buttonA.Groups["Y"].Value));
            ButtonB = new Point(int.Parse(buttonB.Groups["X"].Value), int.Parse(buttonB.Groups["Y"].Value));
            Prize = new Tuple<long, long>(long.Parse(prize.Groups["X"].Value), long.Parse(prize.Groups["Y"].Value));
            Prize2 = new Tuple<long, long>(Prize.Item1 + 10000000000000, Prize.Item2 + 10000000000000);
        }

        public Point ButtonA { get; private set; }
        public Point ButtonB { get; private set; }
        public Tuple<long, long> Prize { get; private set; }
        public Tuple<long, long> Prize2 { get; private set; }

        public long GetMinTokens(bool isSmallPrize)
        {
            var prize = isSmallPrize ? Prize : Prize2;
            long mult1 = ButtonA.Y;
            long mult2 = ButtonA.X;

            var bCoeff = ButtonB.X * mult1 - ButtonB.Y * mult2;
            var output = prize.Item1 * mult1 - prize.Item2 * mult2;

            if (output % bCoeff == 0)
            {
                var bTokens = output / bCoeff;
                var aTokens = (prize.Item1 - (bTokens * ButtonB.X)) / ButtonA.X;
                long numTokens = aTokens * 3 + bTokens;

                return numTokens;
            }

            return 0;
        }
    }
}
