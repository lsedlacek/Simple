using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Simple.Math
{
    public struct TinySet : IEnumerable<int>
    {
        public const int Minimum = 0;
        public const int Maximum = sizeof(ulong) * 8 - 1;

        public static TinySet Empty { get; } = default(TinySet);

        public TinySet(ulong bits)
        {
            Bits = bits;
        }

        public TinySet Add(int value)
        {
            CheckWhetherInRange(value);
            return new TinySet(Bits | (1UL << value));
        }

        public TinySet Remove(int value)
        {
            CheckWhetherInRange(value);
            return new TinySet(Bits ^ (1UL << value));
        }

        public bool Contains(int value)
        {
            CheckWhetherInRange(value);
            var bit = 1UL << value;
            return (Bits & bit) == bit;
        }

        private static void CheckWhetherInRange(int bit)
        {
            if (bit < Minimum || bit > Maximum)
            {
                throw new ArgumentOutOfRangeException(nameof(bit), $"{bit} is not in range [{Minimum}, {Maximum}].");
            }
        }

        private ulong Bits { get; }

        public IEnumerator<int> GetEnumerator()
        {
            var set = this;

            return Enumerable.Range(Minimum, Maximum + 1)
                .Where(value => set.Contains(value))
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
