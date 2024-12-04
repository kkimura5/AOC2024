using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024.Core
{
    public static class DataLoader
    {
        public static List<string> GetLines(int day, bool isTest)
        {
            var testStr = isTest ? "_test" : string.Empty;
            var filePath = $".\\Input\\Day{day}{testStr}.txt";
            return File.ReadAllLines(filePath).ToList();
        }
    }
}
