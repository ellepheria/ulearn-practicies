using System;
using System.Collections.Generic;

namespace Recognizer
{
	internal static class MedianFilterTask
	{
		public static double[,] MedianFilter(double[,] original)
		{
			var originalSizes = ( original.GetLength(0), original.GetLength(1) );
			var result = new double[originalSizes.Item1, originalSizes.Item2];
			
			for (var x = 0; x < originalSizes.Item1; x++)
			{
                for (var y = 0; y < originalSizes.Item2; y++)
				{
					var neighborhood = GetPixelNeighborhood(original, x, y, originalSizes);
					result[x, y] = CalculateMedian(neighborhood);
                }
            }
			return result;
        }

		private static double CalculateMedian(double[] numbers)
		{
			var sortedNumbers = (double[])numbers.Clone();
			Array.Sort(sortedNumbers);

			var size = sortedNumbers.Length;
            var mid = size / 2;
			var median = (size % 2 != 0) ?
				sortedNumbers[mid] :
				(sortedNumbers[mid] + sortedNumbers[mid - 1]) / 2;
			return median;
		}

		private static double[] GetPixelNeighborhood(double[,] original, int xIndex, int yIndex, (int, int) sizes)
		{
			var neighborhood = new List<double>();
			
            for (var i = -1; i < 2; i++)
				for (var j = -1; j < 2; j++)
				{
					var newXindex = xIndex + i;
					var newYindex = yIndex + j;

                    if ((newXindex) < sizes.Item1 &&
						(newYindex) < sizes.Item2 &&
                        newXindex > -1 &&
						newYindex > -1)
						neighborhood.Add(original[newXindex, newYindex]);
				}
			return neighborhood.ToArray();
		}
	}
}