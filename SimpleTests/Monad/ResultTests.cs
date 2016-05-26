using System.Collections.Generic;
using Xunit;

namespace Simple.Monad
{
    public class ResultTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(42)]
        [InlineData(4957)]
        public void ResultReturn_WithValue_ContainsValue(int value)
        {
            var result = Result.Return<int, int>(value);

            Assert.Equal(value, result.Match(x => x, _ => -1));
        }

        [Theory]
        [InlineData("foo")]
        [InlineData("bar")]
        [InlineData("baz")]
        public void ResultFail_WithError_ContainsError(string error)
        {
            var result = Result.Fail<string, string>(error);

            Assert.Equal(error, result.Match(_ => string.Empty, x => x));
        }

        [Theory]
        [InlineData(42, 42, true)]
        [InlineData(24, 42, false)]
        public void Equals_Successes_ReturnsValueEquality(int left, int right, bool expected)
        {
            var rLeft = Result.Return<int, string>(left);
            var rRight = Result.Return<int, string>(right);

            var areEqual = rLeft.Equals(rRight);
            Assert.Equal(expected, areEqual);
        }

        [Theory]
        [InlineData("foo", "foo", true)]
        [InlineData("bar", "quux", false)]
        public void Equals_Failures_ReturnsErrorEquality(string left, string right, bool expected)
        {
            var rLeft = Result.Fail<int, string>(left);
            var rRight = Result.Fail<int, string>(right);

            var areEqual = rLeft.Equals(rRight);
            Assert.Equal(expected, areEqual);
        }

        private static IEnumerable<object[]> AppendTestValues()
            => new[]
            {
                new object[]
                { Result.Return<int, string>(42), Result.Return<int, string>(31), Result.Return<int, string>(42) },
                new object[]
                { Result.Return<int, string>(53), Result.Fail<int, string>("foo"), Result.Return<int, string>(53) },
                new object[]
                { Result.Fail<int, string>("foo"), Result.Return<int, string>(64), Result.Return<int, string>(64) },
                new object[]
                { Result.Fail<int, string>("foo"), Result.Fail<int, string>("bar"), Result.Fail<int, string>("bar") }
            };

        [Theory]
        [MemberData(nameof(AppendTestValues))]
        public void Append_WithSuppliedValues_IfSuccessThenLeftElseRight(IResult<int, string> left,
            IResult<int, string> right, IResult<int, string> expected)
        {
            var result = left.Append(right);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Equals_SuccessAndFailure_ReturnsFalse()
        {
            var left = Result.Return<int, string>(3);
            var right = Result.Fail<int, string>("foo");

            var areEqual = left.Equals(right);
            Assert.False(areEqual);
        }

        [Fact]
        public void OrFail_WithNothing_ReturnsFailure()
        {
            var result = Maybe<int>.Nothing.OrFail("no number");

            Assert.Equal(Result.Fail<int, string>("no number"), result);
        }

        [Fact]
        public void OrFail_WithSomething_ReturnsSuccess()
        {
            var result = Maybe.Return(3).OrFail("no number");

            Assert.Equal(Result.Return<int, string>(3), result);
        }

        [Fact]
        public void SelectMany_WithFailure_DoesNotRunFunc()
        {
            var i = 0;
            var result = Result.Fail<int, string>("foo");
            result.SelectMany(x => Result.Return<int, string>(i = 2));

            Assert.Equal(0, i);
        }

        [Fact]
        public void SelectMany_WithSuccess_RunsFunc()
        {
            var i = 0;
            var result = Result.Return<int, string>(42);
            result.SelectMany(x => Result.Return<int, string>(i = 1));

            Assert.Equal(1, i);
        }
    }
}
