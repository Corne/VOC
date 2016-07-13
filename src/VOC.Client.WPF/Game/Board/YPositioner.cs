using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace VOC.Client.WPF.Game.Board
{
    public class YPositioner : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            culture = culture ?? CultureInfo.InvariantCulture;

            if (values == null || values.Length != 2 || parameter == null)
                return DependencyProperty.UnsetValue;

            double parsedX;
            double parsedY;
            double parsedParam;

            if (!double.TryParse(values[0].ToString(), NumberStyles.Any, culture, out parsedX))
                return DependencyProperty.UnsetValue;

            if (!double.TryParse(values[1].ToString(), NumberStyles.Any, culture, out parsedY))
                return DependencyProperty.UnsetValue;

            if (!double.TryParse(parameter.ToString(), NumberStyles.Any, culture, out parsedParam))
                return DependencyProperty.UnsetValue;

            if (Math.Abs(parsedX) % 2 == 1)
                return parsedY * parsedParam + parsedParam*0.5;
            return parsedY * parsedParam;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
