using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024.Core
{
    public class Antenna
    {
        public Antenna(char frequency, Point location)
        {
            Frequency = frequency;
            Location = location;
        }

        public char Frequency { get; set; }
        public Point Location { get; set; }
    }
}