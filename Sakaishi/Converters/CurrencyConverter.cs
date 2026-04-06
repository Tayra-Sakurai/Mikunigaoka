using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization.NumberFormatting;


namespace Sakaishi.Converters
{
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double num)
                return num.ToString("C3");

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string str)
            {
                CultureInfo culture = string.IsNullOrEmpty(language) ? CultureInfo.CurrentCulture : CultureInfo.CreateSpecificCulture(language);
                
                if (double.TryParse(str, culture, out double result))
                    return result;
            }

            return DependencyProperty.UnsetValue;
        }
    }
}
