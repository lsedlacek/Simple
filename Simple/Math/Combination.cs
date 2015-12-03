using System;
using System.Collections.Generic;

namespace Simple.Math
{
    public static class Combination
    {
        /// <summary>
        /// Refer to <a href="http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.205.2921&rep=rep1&type=pdf">the original</a>
        /// for the smarts, I just implemented it in C#.
        /// </summary>
        /// <returns>All distinct ulongs with <paramref name="k"/> positive bits in the <paramref name="n"/> bit space.</returns>
        public static IEnumerable<ulong> GetAll(int n, int k)
        {
            if (n < 1 || n > 63) throw new ArgumentOutOfRangeException(nameof(n), $"{n} is not within range [1, 63].");
            if (k < 0 || k > n) throw new ArgumentOutOfRangeException(nameof(k), $"{k} it not withing range [0, {n}]");

            if (k == 0)
            {
                yield return 0;
                yield break;
            }

            var capBit = (1UL << n);

            var result = (1UL << k) - 1UL;
            do
            {
                yield return result;

                var x = result & (result + 1UL);
                var y = x ^ (x - 1UL);
                x = y + 1UL;
                y = y & result;
                var z = x & result;
                x = (z == 0UL) ? z : z - 1UL;
                result = result + y - x;
            } while ((result & capBit) != capBit);
        }
    }
}
