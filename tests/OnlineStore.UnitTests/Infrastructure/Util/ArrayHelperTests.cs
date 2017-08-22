namespace OnlineStore.UnitTests.Infrastructure.Util
{
    using OnlineStore.Infrastructure.Util;
    using Shouldly;
    using System.Linq;
    using Xunit;

    public class ArrayHelperTests
    {
        [Fact]
        public void ConcatenatingOneArrayReturnsArray()
        {
            int[] array = new[] { 1, 2, 34, 5152, 34, 214, 151, 1 };

            int[] concatenated = ArrayHelper.ConcatArrays(array);

            concatenated.SequenceEqual(array).ShouldBeTrue();
        }

        [Fact]
        public void ConcatenatesTwoArrays()
        {
            int[] firstArray = new[] { 1, 2, 3, 4, 5, 6, 7 };
            int[] secondArray = new[] { 8, 9, 0, 124, 5 };

            int[] concatenated = ArrayHelper.ConcatArrays(firstArray, secondArray);

            concatenated.Length.ShouldBe(firstArray.Length + secondArray.Length);

            for (int i = 0; i < concatenated.Length; i++)
            {
                if (i < firstArray.Length)
                {
                    (concatenated[i] == firstArray[i]).ShouldBeTrue();
                }
                else
                {
                    (concatenated[i] == secondArray[i - firstArray.Length]).ShouldBeTrue();
                }
            }
        }

        [Fact]
        public void ConcatenatesTenArrays()
        {
            int[][] arrays = new[]
            {
                new[] { 1, 2, 3, 4 },
                new[] { 5, 6, 7, 8 },
                new[] { 9, 10, 11, 12 },
                new[] { 13, 14, 15, 16 },
                new[] { 17, 18, 19, 20 },
                new[] { 21, 78, 2135, 4 },
                new[] { 25, 671, 15, 4 },
                new[] { 29, 43, 15, 61 },
                new[] { 33, 671, 15, 8139 },
                new[] { 37, 54, 15, 16 },
            };

            int[] concatenated = ArrayHelper.ConcatArrays(arrays);

            int currentIndex = 0;

            while (currentIndex < concatenated.Length)
            {
                for (int i = 0; i < arrays.Length; i++)
                {
                    var currentArray = arrays[i];

                    for (int j = 0; j < currentArray.Length; j++)
                    {
                        (concatenated[currentIndex] == currentArray[j]).ShouldBeTrue();
                        currentIndex++;
                    }
                }
            }
        }
    }
}
