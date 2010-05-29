using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ION.Tools
{
    class Damage
    {

        private static Random random;

        public const int TYPE_SMALL_GUN = 1;
        public const int TYPE_MEDIUM_GUN = 2;
        public const int TYPE_HEAVY_GUN = 3;

        public static void init(int seed)
        {
            random = new Random(seed);
        }

        public static int getDamage(int lowerBound, int upperBound)
        {
            int damage = lowerBound + (int)(random.NextDouble() * (upperBound - lowerBound));
            return damage;
        }

    }
}
