using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace VOC.Client.WPF.Converters
{
    public class Multiply : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            culture = culture ?? CultureInfo.InvariantCulture;

            if (value == null || parameter == null)
                return DependencyProperty.UnsetValue;

            double parsedValue;
            double parsedParam;

            if (!double.TryParse(value.ToString(), NumberStyles.Any, culture, out parsedValue))
                return DependencyProperty.UnsetValue;

            if (!double.TryParse(parameter.ToString(), NumberStyles.Any, culture, out parsedParam))
                return DependencyProperty.UnsetValue;

            return parsedValue * parsedParam;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //YAGNI for now
            throw new NotImplementedException();
        }
    }
}
