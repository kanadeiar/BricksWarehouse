using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace BricksWarehouse.Mobile.Converters
{
    public class DoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "0,0";
            var doubleVal = (double)value;

            var strVal = doubleVal.ToString("f");
            return strVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return 0.0;
            var strVal = (string)value;

            if (double.TryParse(strVal, out double doubleVal))
            {
                return doubleVal;
            }
            else
            {
                return 0.0;
            }
        }
    }
}
