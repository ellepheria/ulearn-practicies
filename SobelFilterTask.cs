using System;

namespace Recognizer
{
    internal static class SobelFilterTask
    {
        public static double[,] SobelFilter(double[,] mainMatrix, double[,] coreMatrix)
        {
            var width = mainMatrix.GetLength(0);
            var height = mainMatrix.GetLength(1);
            var size = coreMatrix.GetLength(0);
            var result = new double[width, height];

            var offset = coreMatrix.GetLength(0) / 2;

            for (var x = offset; x < width - offset; x++)
            {
                for (var y = offset; y < height - offset; y++)
                {
                    var gCropped = CropMatrix(mainMatrix, x, y, size);
                    var gx = Convolute(gCropped, coreMatrix, size);
                    var transposedCoreMatrix = Transpose(coreMatrix, size);
                    var gy = Convolute(gCropped, transposedCoreMatrix, size);

                    result[x, y] = Math.Sqrt(gx * gx + gy * gy);
                }
            }

            return result;
        }

        private static double[,] Transpose(double[,] matrix, int size)
        {
            var result = new double[size, size];

            for (var y = 0; y < size; y++)
                for (var x = 0; x < size; x++)
                    result[x, y] = matrix[y, x];

            return result;
        }

        private static double[,] CropMatrix(double[,] g, int x, int y, int size)
        {
            var result = new double[size, size];
            var offset = size / 2;

            for (var i = -offset; i <= offset; i++)
                for (var j = -offset; j <= offset; j++)
                    result[i + offset, j + offset] = g[x + j, y + i];

            return result;
        }

        private static double Convolute(double[,] firstMatrix, double[,] secondMatrix, int size)
        {
            var result = 0.0;

            for (var i = 0; i < size; i++)
                for (var j = 0; j < size; j++)
                    result += firstMatrix[i, j] * secondMatrix[i, j];

            return result;
        }
    }
}