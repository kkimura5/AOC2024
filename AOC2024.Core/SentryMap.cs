using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024.Core
{
    public class SentryMap : Map2D
    {
        private Direction currentDirection;
        private Point currentLocation;
        private List<Movement> previousMoves;
        private List<List<bool>> visited;

        public SentryMap(List<string> lines) :
            base(lines)
        {
            Reset();
        }

        public bool IsMoveComplete { get; private set; }

        public bool CheckForLoop()
        {
            while (!IsMoveComplete && previousMoves.Distinct().Count() == previousMoves.Count)
            {
                MoveToNext();
            }

            return !IsMoveComplete;
        }

        public long CountVisited()
        {
            return visited.Select(x => x.Count(x => x)).Sum();
        }

        public char GetCurrentLocationChar()
        {
            return GetCharAtLocation(currentLocation.X, currentLocation.Y);
        }

        public List<List<bool>> GetVisited()
        {
            return visited.Select(x => x.ToList()).ToList();
        }

        public void MoveToNext()
        {
            previousMoves.Add(new Movement() { Direction = currentDirection, Location = currentLocation });
            Size increment = new Size(0, 0);
            switch (currentDirection)
            {
                case Direction.Up:
                    increment = new Size(0, -1);
                    break;

                case Direction.Right:
                    increment = new Size(1, 0);
                    break;

                case Direction.Left:
                    increment = new Size(-1, 0);
                    break;

                case Direction.Down:
                    increment = new Size(0, 1);
                    break;
            }

            var currentChar = GetCurrentLocationChar();
            while (true)
            {
                visited[currentLocation.Y][currentLocation.X] = true;
                var nextLocation = currentLocation + increment;

                if (IsLocationOutOfBounds(nextLocation))
                {
                    IsMoveComplete = true;
                    break;
                }

                if (GetCharAtLocation(nextLocation.X, nextLocation.Y) == '#')
                {
                    break;
                }

                currentLocation = nextLocation;
            }

            currentDirection++;
            if (currentDirection == Direction.None)
            {
                currentDirection = Direction.Up;
            }
        }

        public void Reset()
        {
            IsMoveComplete = false;
            previousMoves = new List<Movement>();
            visited = new List<List<bool>>();
            var foundStart = false;
            for (int i = 0; i < Lines.Count; i++)
            {
                visited.Add(Lines[i].Select(x => false).ToList());
                if (!foundStart)
                {
                    for (int j = 0; j < Lines[i].Length; j++)
                    {
                        if (Lines[i][j] == '^')
                        {
                            currentLocation = new Point(j, i);
                            currentDirection = Direction.Up;
                            foundStart = true;
                            break;
                        }
                    }
                }
            }
        }
    }
}