using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HaLi.WPF.Converters
{
    public class ScaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double d = 1d;
            double s = 1d;
            try
            {
                d = System.Convert.ToDouble(value);
                s = System.Convert.ToDouble(parameter);
                return d * s;
            }
            catch 
            {
                return d;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double d = 1d;
            double s = 1d;
            try
            {
                d = System.Convert.ToDouble(value);
                s = System.Convert.ToDouble(parameter);
                return d / s;
            }
            catch
            {
                return d;
            }
        }
    }
}
