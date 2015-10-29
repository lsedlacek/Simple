using System;
using Xunit;

namespace Simple.Extensions
{
    public class EnumExtensionsTests
    {
        [Flags]
        public enum Foo
        {
            None = 0,
            Bar = 1,
            Baz = 2,
            Qux = 4,
        }

        [Theory]
        [InlineData(Foo.None, Foo.None, true)]
        [InlineData(Foo.None, Foo.Bar, false)]
        [InlineData(Foo.Bar, Foo.Bar, true)]
        [InlineData(Foo.Bar, Foo.Baz, false)]
        [InlineData(Foo.Bar | Foo.Baz, Foo.Bar, true)]
        [InlineData(Foo.Bar | Foo.Baz, Foo.Baz, true)]
        [InlineData(Foo.Bar | Foo.Baz, Foo.Qux, false)]
        [InlineData(Foo.Bar | Foo.Baz, Foo.Bar | Foo.Baz, true)]
        [InlineData(Foo.Bar | Foo.Baz, Foo.Baz | Foo.Qux, false)]
        [InlineData(Foo.Bar | Foo.Baz | Foo.Qux, Foo.Bar | Foo.Baz, true)]
        public void HasFlags_WithFlag_ReturnsExpected(Foo extended, Foo flag, bool expected)
        {
            Assert.Equal(expected, extended.HasFlags(flag));
        }

        [Fact]
        public void HasFlags_WithNonEnum_Throws()
        {
            Assert.Throws<TypeInitializationException>(() => 3.HasFlags(2));
        }
    }
}
