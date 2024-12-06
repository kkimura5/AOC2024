using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AOC2024.Core;

namespace AOC2024.Core
{
    public class Map2D
    {
        private Direction currentDirection;
        private Point currentLocation;
        private List<string> lines;
        private List<Movement> previousMoves;
        private List<List<bool>> visited;

        public Map2D(List<string> lines)
        {
            this.lines = lines;
            Reset();
        }

        public bool IsMoveComplete { get; private set; }
        public int NumCols => lines.First().Length;

        public int NumRows => lines.Count;
        public List<List<bool>> GetVisited()
        {
            return visited.Select(x => x.ToList()).ToList();
        }

        public long CountVisited()
        {
            return visited.Select(x => x.Count(x => x)).Sum();
        }

        public char GetCharAtLocation(int x, int y)
        {
            return lines[y][x];
        }

        public char GetCurrentLocationChar()
        {
            return GetCharAtLocation(currentLocation.X, currentLocation.Y);
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
            for (int i = 0; i < lines.Count; i++)
            {
                visited.Add(lines[i].Select(x => false).ToList());
                if (!foundStart)
                {
                    for (int j = 0; j < lines[i].Length; j++)
                    {
                        if (lines[i][j] == '^')
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

        public void SetCharAtLocation(int x, int y, char v)
        {
            lines[y] = string.Concat(lines[y].Substring(0, x), v.ToString(), lines[y].Substring(x + 1));
        }

        public bool CheckForLoop()
        {
            while (!IsMoveComplete && previousMoves.Distinct().Count() == previousMoves.Count)
            {
                MoveToNext();
            }

            return !IsMoveComplete;
        }

        private bool IsLocationOutOfBounds(Point location)
        {
            return location.Y < 0 || location.X < 0 || location.X >= lines.First().Length || location.Y >= lines.Count();
        }
    }
}