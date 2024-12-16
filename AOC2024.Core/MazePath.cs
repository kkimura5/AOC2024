using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024.Core
{
    public class MazePath
    {
        public List<Point> PreviousPoints { get; set; } = new List<Point>();
        public Direction Direction { get; set; }
        public long Score { get; set; }

        public int GetTurnScore(Direction newDirection)
        {
            var diff = newDirection.ToVector() - new Size(Direction.ToVector());

            if (diff == new Point(0, 0))
            {
                return 0;
            }
            else if (diff.X == 0 || diff.Y == 0)
            {
                return 2000;
            }
            else
            {
                return 1000;
            }
        }
    }
}
