using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024.Core
{
    public class FenceSide
    {
        public List<Point> Points {  get; set; } = new List<Point>();
        public Direction Direction { get; set; }

        public bool ContinuesSide(Point nextPoint, Direction direction)
        {
            if (Direction != direction)
            {
                return false;
            }
            
            foreach (var point in Points)
            {
                var diff = point - new Size(nextPoint);

                if ((direction == Direction.Up || direction == Direction.Down) &&
                    (diff == new Point(1, 0) || diff == new Point(-1, 0)))
                {
                    return true;
                }
                else if ((direction == Direction.Left || direction == Direction.Right) &&
                    (diff == new Point(0, 1) || diff == new Point(0, -1)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
