using System.Linq;
using Xunit;

namespace Simple.Math
{
    public class CombinationTests
    {
        [Theory]
        [InlineData(7, 4)]
        [InlineData(12, 10)]
        [InlineData(10, 5)]
        [InlineData(34, 30)]
        [InlineData(62, 59)]
        public void GetCombinations_NOverK_AreDistinct(int n, int k)
        {
            var combinations = Combination.GetAll(n, k).ToArray();
            var distinct = combinations.Distinct();

            Assert.Equal(distinct, combinations);
        }

        [Theory]
        [InlineData(1, 0, 1)]
        [InlineData(2, 2, 1)]
        [InlineData(6, 3, 20)]
        [InlineData(10, 10, 1)]
        [InlineData(11, 9, 55)]
        [InlineData(33, 29, 40920)]
        [InlineData(63, 60, 39711)]
        public void GetCombinations_NOverK_ProduceCountCombinations(int n, int k, int count)
        {
            var result = Combination.GetAll(n, k).Count();

            Assert.Equal(count, result);
        }
    }
}
