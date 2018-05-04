using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows.Core.Utils
{
    public class RandomGenerator
    {
        private static Random rnd = new Random();
        public static int GetNumber(int min = 1000, int max = 10000)
        {

            return rnd.Next(min, max);
        }
    }
}
