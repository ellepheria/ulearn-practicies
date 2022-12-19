using System;
using System.Linq;

namespace Names
{
    internal static class HistogramTask
    {
        public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
        {
            var xLabels = new string[31];
            var yLabels = new double[31];

            foreach (var nameData in names)
            {
                if (nameData.Name == name && nameData.BirthDate.Day != 1)
                {
                    yLabels[nameData.BirthDate.Day -1]++;
                }
            }

            for (var i = 0; i < xLabels.Length; i++) 
                xLabels[i] = $"{i + 1}";

            return new HistogramData(
                $"Рождаемость людей с именем '{name}'", 
                xLabels, 
                yLabels);
        }
    }
}