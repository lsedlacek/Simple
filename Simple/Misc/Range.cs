using System.Collections.Generic;

namespace Simple.Misc
{
    public static class Range
    {
        public static IEnumerable<int> Increasing(int first, int lastInclusive)
        {
            for (int i = first; i <= lastInclusive; ++i)
            {
                yield return i;
            }
        }

        public static IEnumerable<int> IncreasingExclusive(int first, int lastExclusive)
        {
            for (int i = first; i < lastExclusive; ++i)
            {
                yield return i;
            }
        }

        public static IEnumerable<int> Decreasing(int first, int lastInclusive)
        {
            for (int i = first; i >= lastInclusive; --i)
            {
                yield return i;
            }
        }

        public static IEnumerable<int> DecreasingExclusive(int first, int lastExclusive)
        {
            for (int i = first; i > lastExclusive; --i)
            {
                yield return i;
            }
        }
    }
}
