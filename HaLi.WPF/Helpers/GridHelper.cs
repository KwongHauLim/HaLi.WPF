using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HaLi.WPF.Helpers
{
    internal static class GridHelper
    {
        public static GridLength[] ConvertToGridLengths(string value)
        {
            if (string.IsNullOrEmpty(value))
                return new GridLength[0];

            var parts = value.Split(',');
            var result = new GridLength[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                string k = parts[i];
                if (string.IsNullOrEmpty(k) || k == "auto")
                    result[i] = new GridLength(1, GridUnitType.Auto);
                else if (k == "*")
                    result[i] = new GridLength(1, GridUnitType.Star);
                else if (k.EndsWith("*"))
                {
                    if (double.TryParse(k.Substring(0, k.Length - 1), out double val))
                        result[i] = new GridLength(val, GridUnitType.Star);
                    else
                        result[i] = new GridLength(1, GridUnitType.Star);
                }
                else if (k.EndsWith("px"))
                {
                    if (double.TryParse(k.Substring(0, k.Length - 2), out double val))
                        result[i] = new GridLength(val, GridUnitType.Pixel);
                    else
                        result[i] = new GridLength(1, GridUnitType.Star);
                }
                else if (double.TryParse(k, out double val))
                    result[i] = new GridLength(val, GridUnitType.Pixel);
                else
                    result[i] = new GridLength(1, GridUnitType.Star);
            }
            return result;
        }

        public static void ConvertToWidths(GridLength[] lengths, double availableWidth, out double[] widths)
        {
            widths = new double[lengths.Length];
            for (int i = 0; i < lengths.Length; i++)
            {
                if (lengths[i].IsAbsolute)
                    widths[i] = lengths[i].Value;
                else if (lengths[i].IsStar)
                    widths[i] = lengths[i].Value * availableWidth;
                else
                    widths[i] = 0;
            }
        }
    }
}
