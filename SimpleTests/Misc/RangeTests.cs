using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Simple.Misc
{
    public class RangeTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(54)]
        [InlineData(980800)]
        public void Increasing_WithSameParameter_YieldsTheParameter(int x)
        {
            var result = Range.Increasing(x, x);

            Assert.Equal(new[] { x }, result);
        }

        private static IEnumerable<object[]> IncreasingRanges()
            => new List<object[]>
            {
                new object[] { 4, 7, new[] { 4, 5, 6, 7 } },
                new object[] { 1, 2, new[] { 1, 2 } },
                new object[] { 66, 72, new[] { 66, 67, 68, 69, 70, 71, 72 } }
            };

        [Theory, MemberData(nameof(IncreasingRanges))]
        public void Increasing_WithParameters_YieldsExpected(int first, int lastInclusive, IEnumerable<int> expected)
        {
            var result = Range.Increasing(first, lastInclusive);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(57, 12)]
        [InlineData(34567, 345)]
        [InlineData(-3, -7)]
        public void Increasing_WithLastLowerThanFirst_YieldsNothing(int first, int lastInclusive)
        {
            var result = Range.Increasing(first, lastInclusive);

            Assert.Equal(Enumerable.Empty<int>(), result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(54)]
        [InlineData(980800)]
        public void IncreasingExclusive_WithSameParameter_YieldsNothing(int x)
        {
            var result = Range.IncreasingExclusive(x, x);

            Assert.Equal(Enumerable.Empty<int>(), result);
        }

        private static IEnumerable<object[]> IncreasingExclusiveRanges()
            => new List<object[]>
            {
                new object[] { 4, 7, new[] { 4, 5, 6 } },
                new object[] { 1, 2, new[] { 1 } },
                new object[] { 66, 72, new[] { 66, 67, 68, 69, 70, 71 } }
            };

        [Theory, MemberData(nameof(IncreasingExclusiveRanges))]
        public void IncreasingExclusive_WithParameters_YieldsExpected(int first, int lastInclusive, IEnumerable<int> expected)
        {
            var result = Range.IncreasingExclusive(first, lastInclusive);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(57, 12)]
        [InlineData(34567, 345)]
        [InlineData(-3, -7)]
        public void IncreasingExclusive_WithLastLowerThanFirst_YieldsNothing(int first, int lastInclusive)
        {
            var result = Range.IncreasingExclusive(first, lastInclusive);

            Assert.Equal(Enumerable.Empty<int>(), result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(54)]
        [InlineData(980800)]
        public void Decreasing_WithSameParameter_YieldsTheParameter(int x)
        {
            var result = Range.Decreasing(x, x);

            Assert.Equal(new[] { x }, result);
        }

        private static IEnumerable<object[]> DecreasingRanges()
            => new List<object[]>
            {
                new object[] { 8, 5, new[] { 8, 7, 6, 5 } },
                new object[] { 99, 96, new[] { 99, 98, 97, 96 } },
                new object[] { 555, 553, new[] { 555, 554, 553 } }
            };

        [Theory, MemberData(nameof(DecreasingRanges))]
        public void Decreasing_WithParameters_YieldsExpected(int first, int lastInclusive, IEnumerable<int> expected)
        {
            var result = Range.Decreasing(first, lastInclusive);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(12, 57)]
        [InlineData(345, 34567)]
        [InlineData(-1111, -4)]
        public void Decreasing_WithLastHigherThanFirst_YieldsNothing(int first, int lastInclusive)
        {
            var result = Range.Decreasing(first, lastInclusive);

            Assert.Equal(Enumerable.Empty<int>(), result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(54)]
        [InlineData(980800)]
        public void DecreasingExclusive_WithSameParameter_YieldsNothing(int x)
        {
            var result = Range.DecreasingExclusive(x, x);

            Assert.Equal(Enumerable.Empty<int>(), result);
        }

        private static IEnumerable<object[]> DecreasingExclusiveRanges()
            => new List<object[]>
            {
                new object[] { 8, 5, new[] { 8, 7, 6 } },
                new object[] { 99, 96, new[] { 99, 98, 97 } },
                new object[] { 555, 553, new[] { 555, 554 } }
            };

        [Theory, MemberData(nameof(DecreasingExclusiveRanges))]
        public void DecreasingExclusive_WithParameters_YieldsExpected(int first, int lastInclusive, IEnumerable<int> expected)
        {
            var result = Range.DecreasingExclusive(first, lastInclusive);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(12, 57)]
        [InlineData(345, 34567)]
        [InlineData(-1111, -4)]
        public void DecreasingExclusive_WithLastHigherThanFirst_YieldsNothing(int first, int lastInclusive)
        {
            var result = Range.DecreasingExclusive(first, lastInclusive);

            Assert.Equal(Enumerable.Empty<int>(), result);
        }
    }
}
