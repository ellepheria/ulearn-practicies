using System.Linq;
using System.Collections.Generic;
using System;

namespace Recognizer
{
    public static class ThresholdFilterTask
    {
        public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
        {
            var sizes = new[] { original.GetLength(0), original.GetLength(1) };
            var result = new double[sizes[0], sizes[1]];
            var threshold = CalculateThreshold(original, whitePixelsFraction, sizes);

            for (var x = 0; x < sizes[0]; x++)
                for (var y = 0; y < sizes[1]; y++)
                    result[x, y] = original[x, y] >= threshold ? 1 : 0;
            
            return result;
        }

        private static double[] MakeOneDimensionalArray(double[,] original, int[] sizes)
        {
            var oneDimensionalArray = new List<double>();

            for (var x = 0; x < sizes[0]; x++)
                for (var y = 0; y < sizes[1]; y++)
                    oneDimensionalArray.Add(original[x, y]);

            return oneDimensionalArray.ToArray();
        }

        private static double CalculateThreshold(double[,] original, double whitePixelsFraction, int[] sizes)
        {
            var countWhitePixels = (int)Math.Floor((sizes[0] * sizes[1] * whitePixelsFraction));
            var originalArr = MakeOneDimensionalArray(original, sizes);

            originalArr = originalArr.OrderByDescending(x => x).ToArray();

            return countWhitePixels > 0 ? originalArr[countWhitePixels - 1] : originalArr.Max() + 1;
        }
    }
}