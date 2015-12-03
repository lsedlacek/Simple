using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Simple.Monad
{
    public class MaybeCollectionExtensionsTests
    {
        [Fact]
        public void SingleOrNothing_Empty_NoHasValue()
        {
            var src = Enumerable.Empty<int>();
            var result = src.SingleOrNothing().HasValue;

            Assert.False(result);
        }

        [Fact]
        public void SingleOrNothing_Single_HasValue()
        {
            var src = new[] { 42 };
            var result = src.SingleOrNothing().HasValue;

            Assert.True(result);
        }

        [Fact]
        public void SingleOrNothing_Multiple_Throws()
        {
            var src = new[] { 42, 41 };

            Assert.Throws<InvalidOperationException>(() => src.SingleOrNothing());
        }

        [Fact]
        public void FirstOrNothing_Empty_NoHasValue()
        {
            var src = Enumerable.Empty<int>();
            var result = src.FirstOrNothing().HasValue;

            Assert.False(result);
        }

        [Fact]
        public void FirstOrNothing_Single_HasValue()
        {
            var src = new[] { 42 };
            var result = src.FirstOrNothing().HasValue;

            Assert.True(result);
        }

        [Fact]
        public void FirstOrNothing_Multiple_HasValue()
        {
            var src = new[] { 42, 41 };
            var result = src.FirstOrNothing().HasValue;

            Assert.True(result);
        }

        [Fact]
        public void GetOrNothing_NotContained_NoHasValue()
        {
            var dict = new Dictionary<int, int>();
            var result = dict.GetOrNothing(42);

            Assert.False(result.HasValue);
        }

        [Fact]
        public void GetOrNothing_Contained_HasValue()
        {
            var dict = new Dictionary<int, int>
            {
                {42, 43}
            };

            var result = dict.GetOrNothing(42);

            Assert.True(result.HasValue);
        }
    }
}
