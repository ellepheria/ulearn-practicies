namespace Recognizer
{
	public static class GrayscaleTask
	{
		private const double RedСoeff = 0.299;
		private const double GreenСoeff = 0.587;
		private const double BlueСoeff = 0.114;
		private const double RgbСoeff = 255;

        public static double[,] ToGrayscale(Pixel[,] original)
		{
			var xLength = original.GetLength(0);
			var yLength = original.GetLength(1);
			var grayscale = new double[xLength, yLength];

            for (var x = 0; x < xLength; x++)
			{
				for (var y = 0; y < yLength; y++)
				{
					var pixel = original[x, y];
					grayscale[x, y] = (RedСoeff * pixel.R + GreenСoeff * pixel.G + BlueСoeff * pixel.B) / RgbСoeff;
				}
			}

			return grayscale;
		}
	}
}