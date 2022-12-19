using System;

namespace Names
{
    internal static class HeatmapTask
    {
        public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
        {
            const int dayCount = 30;
            const int monthCount = 12;
            
            var heat = new double[30, 12];
            var xLabels = new string[30];
            var yLabels = new string[12];

            for (var i = 0; i < dayCount; i++)
                xLabels[i] = $"{i + 2}";
            for (var i = 0; i < monthCount; i++)
                yLabels[i] = $"{i + 1}";

            foreach (var name in names)
            {
                if (name.BirthDate.Day != 1)
                    heat[name.BirthDate.Day - 2, name.BirthDate.Month - 1]++;
            }

            return new HeatmapData(
                "Карта интенсивностей",
                heat,
                xLabels,
                yLabels);
        }
    }
}