using Xunit;

namespace Simple.Monad
{
    public class MaybeTests
    {
        [Fact]
        public void MaybeReturn_WithValue_HasValue()
        {
            var result = Maybe.Return(1).HasValue;
            Assert.True(result);
        }

        [Fact]
        public void MaybeReturn_WithNull_NoHasValue()
        {
            var result = Maybe.Return((int?)null).HasValue;
            Assert.False(result);
        }

        [Fact]
        public void Default_Maybe_NoHasValue()
        {
            var result = default(Maybe<int>).HasValue;
            Assert.False(result);
        }

        [Fact]
        public void MaybeReturn_WithNullable_ReturnsMaybeOfNonNullable()
        {
            var result = Maybe.Return((int?) 42);
            Assert.Equal(typeof(Maybe<int>), result.GetType());
        }

        [Fact]
        public void Bind_WithValue_RunsFunc()
        {
            int i = 0;
            var maybe = Maybe.Return(1);
            maybe.Bind(x => Maybe.Return(i = x));

            Assert.Equal(1, i);
        }

        [Fact]
        public void Bind_WithoutValue_DoesNotRunFunc()
        {
            int i = 0;
            var maybe = Maybe.Return((int?)null);
            maybe.Bind(x => Maybe.Return(i = x));

            Assert.Equal(0, i);
        }

        [Theory]
        [InlineData(42, 42, true)]
        [InlineData(24, 42, false)]
        public void Equality_WithValues_ReturnsValueEquality(int left, int right, bool result)
        {
            var maybeLeft = Maybe.Return(left);
            var maybeRight = Maybe.Return(right);

            var areEqual = maybeLeft == maybeRight;
            Assert.Equal(result, areEqual);
        }

        [Fact]
        public void Equality_WithoutValues_ReturnsTrue()
        {
            var maybeLeft = Maybe.Return((int?) null);
            var maybeRight = Maybe.Return((int?) null);

            var areEqual = maybeLeft == maybeRight;
            Assert.True(areEqual);
        }

        [Fact]
        public void Equality_ValueAndNoValue_ReturnsFalse()
        {
            var maybeLeft = Maybe.Return((int?)null);
            var maybeRight = Maybe.Return(3);

            var areEqual = maybeLeft == maybeRight;
            Assert.False(areEqual);
        }

        [Theory]
        [InlineData(42, 42, 0)]
        [InlineData(24, 42, -1)]
        [InlineData(42, 24, 1)]
        public void Comparison_WithValues_ReturnsValueComparison(int left, int right, int result)
        {
            var maybeLeft = Maybe.Return(left);
            var maybeRight = Maybe.Return(right);

            var comparisonResult = maybeLeft.CompareTo(maybeRight);
            Assert.Equal(result, comparisonResult);
        }

        [Fact]
        public void Comparison_WithNoValues_ReturnsEqual()
        {
            var maybeLeft = Maybe.Return((int?)null);
            var maybeRight = Maybe.Return((int?)null);

            var comparison = maybeLeft.CompareTo(maybeRight);
            Assert.Equal(0, comparison);
        }

        [Fact]
        public void Comparison_WithMinValueAndNoValue_NoValueIsLess()
        {
            var maybeLeft = Maybe.Return((int?)null);
            var maybeRight = Maybe.Return(int.MinValue);

            var comparison = maybeLeft.CompareTo(maybeRight);
            Assert.Equal(-1, comparison);
        }

        [Fact]
        public void OrElse_WithValue_ReturnsValue()
        {
            var maybe = Maybe.Return(3);

            var result = maybe.OrElse(4);
            Assert.Equal(result, 3);
        }

        [Fact]
        public void OrElse_WithNothing_ReturnsElse()
        {
            Maybe<int> maybe = Maybe.Nothing;

            var result = maybe.OrElse(2);
            Assert.Equal(2, result);
        }
    }
}
