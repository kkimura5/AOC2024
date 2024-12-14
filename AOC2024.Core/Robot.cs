using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2024.Core
{
    public class Robot
    {
        public Robot(string line, Point size) 
        {
            var pattern = @"p=(?<px>-?\d+),(?<py>-?\d+) v=(?<vx>-?\d+),(?<vy>-?\d+)";
            var match = Regex.Match(line, pattern);

            Position = new Point(int.Parse(match.Groups["px"].Value), int.Parse(match.Groups["py"].Value));
            Velocity = new Point(int.Parse(match.Groups["vx"].Value), int.Parse(match.Groups["vy"].Value));
            Size = size;
        }

        public Point Position { get; private set; }
        public Point Velocity { get; private set; }
        public Point Size { get; }

        public Point GetPositionAfterSteps(int numSteps)
        {
            var finalPosition = Position + new Size(Velocity.X * numSteps, Velocity.Y * numSteps);
            while (finalPosition.X < 0)
            {
                finalPosition.X += Size.X;
            }

            while (finalPosition.Y < 0)
            {
                finalPosition.Y += Size.Y;
            }

            finalPosition.X %= Size.X;
            finalPosition.Y %= Size.Y;

            return finalPosition;
        }
    }
}
