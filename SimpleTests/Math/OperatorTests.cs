using System;
using Xunit;

namespace Simple.Math
{
    public class OperatorTests
    {
        [Theory]
        [InlineData(1,2,3)]
        [InlineData(13,37,50)]
        [InlineData(1000, 100, 1100)]
        public void Add_WithInts_ReturnsAddedInt(int x, int y, int expected)
        {
            var result = Operator.Add(x, y);

            Assert.Equal(expected, result);
        }

        private struct HasAddOperator
        {
            private int Value { get; }

            public HasAddOperator(int value)
            {
                Value = value;
            }

            public static HasAddOperator operator+(HasAddOperator x, HasAddOperator y)
            {
                return new HasAddOperator(x.Value + y.Value);
            }
        }

        [Fact]
        public void Add_WithCustomAddOperator_ReturnsAddedResult()
        {
            var x = new HasAddOperator(2);
            var y = new HasAddOperator(4);

            var result = Operator.Add(x, y);

            Assert.Equal(new HasAddOperator(6), result);
        }

        [Fact]
        public void Subtract_WithNonexistentOperator_Throws()
        {
            var x = new HasAddOperator(1);
            var y = new HasAddOperator(7);

            Assert.Throws<TypeInitializationException>(() => Operator.Subtract(x, y));
        }

        private struct Negatable
        {
            private bool Value { get; }

            public Negatable(bool value)
            {
                Value = value;
            }

            public static Negatable operator!(Negatable x)
            {
                return new Negatable(!x.Value);
            }
        }

        [Fact]
        public void OtherArithmetic_WithValues_ReturnExpectedResult()
        {
            Assert.Equal(15L, Operator.Divide(31L, 2L));
            Assert.Equal(144F, Operator.Multiply(12F, 12F));
            Assert.Equal(7, Operator.Modulo(7, 13));
            Assert.Equal(new Negatable(false), Operator.Not(new Negatable(true)));
        }

        private struct OnesComplement
        {
            private int Value { get; }

            public OnesComplement(int value)
            {
                Value = value;
            }

            public static OnesComplement operator ~(OnesComplement x)
            {
                return new OnesComplement(~x.Value);
            }
        }

        [Fact]
        public void Bitwise_WithValues_ReturnsExpectedResult()
        {
            Assert.Equal(4L, Operator.BitwiseAnd(12L, 7L));
            Assert.Equal(15, Operator.BitwiseOr(5, 10));
            Assert.Equal(2U, Operator.Xor(uint.MaxValue, uint.MaxValue - 2));
            Assert.Equal(new OnesComplement(-1), Operator.BitwiseNot(new OnesComplement(0)));
        }

        [Fact]
        public void Comparison_WithValues_ReturnsExpected()
        {
            Assert.Equal(true, Operator.Equal(true, true));
            Assert.Equal(false, Operator.NotEqual(7L, 7L));
            Assert.Equal(false, Operator.GreaterThan(42U, 42U));
            Assert.Equal(true, Operator.LessThan(13UL, 37UL));
            Assert.Equal(true, Operator.LessThanOrEqual((int?)1, 3));
            Assert.Equal(false, Operator.GreaterThanOrEqual(42, 1001));
        }
    }
}
