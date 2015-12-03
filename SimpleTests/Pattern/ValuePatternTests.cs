using System;
using Xunit;

namespace Simple.Pattern
{
    public class ValuePatternTests
    {
        [Fact]
        public void MatchSingleOption_WithMatchingValue_ReturnsResult()
        {
            var pattern = new ValuePattern<int, long>()
                .If(4, x => 5);

            var result = pattern.Match(4);
            Assert.Equal(5, result);
        }

        [Theory]
        [InlineData(4,5)]
        [InlineData(6,7)]
        [InlineData(8,42)]
        public void MatchMultipleOptions_WithValue_ReturnsMatchingResult(int toMatch, long expected)
        {
            var pattern = new ValuePattern<int, long>()
                .If(4, x => 5)
                .If(6, x => 7)
                .If(8, x => 42);

            var result = pattern.Match(toMatch);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void MatchNoOptions_WithValue_Throws()
        {
            var pattern = new ValuePattern<string, object>();

            Assert.Throws<InvalidOperationException>(() => pattern.Match("foo"));
        }

        [Fact]
        public void MatchMultipleOptions_WithUnmatched_Throws()
        {
            var pattern = new ValuePattern<int, long>()
                .If(4, x => 5)
                .If(6, x => 7)
                .If(8, x => 42);

            Assert.Throws<InvalidOperationException>(() => pattern.Match(2));
        }

        [Fact]
        public void MatchWithDefault_WithMatched_ReturnsMatch()
        {
            var pattern = new ValuePattern<int, long>()
                .If(4, x => 5)
                .If(6, x => 7)
                .If(8, x => 42)
                .Default(x => 1337);

            var result = pattern.Match(6);
            Assert.Equal(7, result);
        }

        [Fact]
        public void MatchWithDefault_WithUnmatched_ReturnsDefault()
        {
            var pattern = new ValuePattern<string, string>()
                .If("foo", x => "bar")
                .Default(x => "baz");

            var result = pattern.Match("quux");
            Assert.Equal("baz", result);
        }
    }
}
