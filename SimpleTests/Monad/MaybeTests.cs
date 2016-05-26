using System;
using Xunit;

namespace Simple.Monad
{
    public class MaybeTests
    {
        [Fact]
        public void MaybeReturn_WithValue_ContainsValue()
        {
            var result = Maybe.Return(1);

            Assert.True(result.HasValue);
            Assert.Equal(1, result.UnsafeValue);
        }

        [Fact]
        public void MaybeReturn_WithNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => Maybe.Return<string>(null));
        }

        [Fact]
        public void Default_Maybe_NoHasValue()
        {
            var result = default(Maybe<int>).HasValue;
            Assert.False(result);
        }

        [Theory]
        [InlineData(42, 42, true)]
        [InlineData(24, 42, false)]
        public void Equals_WithSomethings_ReturnsValueEquality(int left, int right, bool result)
        {
            var maybeLeft = Maybe.Return(left);
            var maybeRight = Maybe.Return(right);

            var areEqual = maybeLeft == maybeRight;
            Assert.Equal(result, areEqual);
        }

        [Fact]
        public void Equals_WithNothings_ReturnsTrue()
        {
            var maybeLeft = Maybe<int>.Nothing;
            var maybeRight = Maybe<int>.Nothing;

            var areEqual = maybeLeft == maybeRight;
            Assert.True(areEqual);
        }

        [Fact]
        public void Equals_SomethingAndNothing_ReturnsFalse()
        {
            var maybeLeft = Maybe<int>.Nothing;
            var maybeRight = Maybe.Return(3);

            var areEqual = maybeLeft == maybeRight;
            Assert.False(areEqual);
        }

        [Theory]
        [InlineData(42, 42, 0)]
        [InlineData(24, 42, -1)]
        [InlineData(42, 24, 1)]
        public void CompareTo_WithSomethings_ReturnsValueComparison(int left, int right, int result)
        {
            var mLeft = Maybe.Return(left);
            var mRight = Maybe.Return(right);

            var comparisonResult = mLeft.CompareTo(mRight);
            Assert.Equal(result, comparisonResult);
        }

        [Fact]
        public void CompareTo_WithNothings_ReturnsEqual()
        {
            var left = Maybe<int>.Nothing;
            var right = Maybe<int>.Nothing;

            var comparison = left.CompareTo(right);
            Assert.Equal(0, comparison);
        }

        [Fact]
        public void CompareTo_WithNothingAndMinSomething_ReturnsLess()
        {
            var maybeLeft = Maybe.OfNullable((int?)null);
            var maybeRight = Maybe.Return(int.MinValue);

            var comparison = maybeLeft.CompareTo(maybeRight);
            Assert.Equal(-1, comparison);
        }

        [Fact]
        public void OfNullable_WithNull_ReturnsNothing()
        {
            var result = Maybe.OfNullable((int?) null);
            Assert.Equal(Maybe<int>.Nothing, result);
        }

        [Fact]
        public void OfNullable_WithValue_ReturnsSomething()
        {
            var result = Maybe.OfNullable((int?) 42);
            Assert.Equal(Maybe.Return(42), result);
        }

        [Fact]
        public void SelectMany_WithSomething_RunsFunc()
        {
            int i = 0;
            var maybe = Maybe.Return(1);
            maybe.SelectMany(x => Maybe.Return(i = x));

            Assert.Equal(1, i);
        }

        [Fact]
        public void SelectMany_WithNothing_DoesNotRunFunc()
        {
            int i = 0;
            var maybe = Maybe<int>.Nothing;
            maybe.SelectMany(x => Maybe.Return(i = x));

            Assert.Equal(0, i);
        }

        [Fact]
        public void OrElse_WithSomething_ReturnsLeft()
        {
            var maybe = Maybe.Return(3);

            var result = maybe.OrElse(4);
            Assert.Equal(result, 3);
        }

        [Fact]
        public void OrElse_WithNothing_ReturnsRight()
        {
            var maybe = Maybe<int>.Nothing;

            var result = maybe.OrElse(2);
            Assert.Equal(2, result);
        }

        [Theory]
        [InlineData(1, 2, 1)]
        [InlineData(2, null, 2)]
        [InlineData(null, 3, 3)]
        [InlineData(null, null, null)]
        public void Append_WithSuppliedValues_IfHasValueThenLeftElseRight(int? left, int? right, int? expected)
        {
            var mLeft = Maybe.OfNullable(left);
            var mRight = Maybe.OfNullable(right);

            var result = mLeft.Append(mRight);

            Assert.Equal(Maybe.OfNullable(expected), result);
        }
    }
}
