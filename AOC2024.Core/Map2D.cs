using System.Drawing;

namespace AOC2024.Core
{
    public class Map2D
    {
        public Map2D(List<string> lines)
        {
            Lines = lines;
        }

        public int NumCols => Lines.First().Length;

        public int NumRows => Lines.Count;

        protected List<string> Lines { get; private set; }

        public char GetCharAtLocation(int x, int y)
        {
            return Lines[y][x];
        }

        public char GetCharAtLocation(Point location) => GetCharAtLocation(location.X, location.Y);

        public bool IsLocationOutOfBounds(Point location)
        {
            return location.Y < 0 || location.X < 0 || location.X >= Lines.First().Length || location.Y >= Lines.Count();
        }
        public void SetCharAtLocation(Point point, char v) => SetCharAtLocation(point.X, point.Y, v);

        public void SetCharAtLocation(int x, int y, char v)
        {
            Lines[y] = string.Concat(Lines[y].Substring(0, x), v.ToString(), Lines[y].Substring(x + 1));
        }

        public void Print()
        {
            foreach (var line in Lines)
            {
                Console.WriteLine(line);
            }
        }
    }
}