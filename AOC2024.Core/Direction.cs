using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024.Core
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left,
        None
    }

    public static class DirectionExtensions
    {
        public static Point ToVector(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Point(0, -1);

                case Direction.Right:
                    return new Point(1, 0);

                case Direction.Down:
                    return new Point(0, 1);

                case Direction.Left:
                    return new Point(-1, 0);

                default:
                    return new Point(0, 0);
            }
        }
    }
}