using System;
using System.Globalization;
using System.Windows;

namespace MAF.Converters
{
    internal class BooleanToVisibilityConverter : ConverterBase<BooleanToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                return ((bool) value) ? Visibility.Visible : Visibility.Collapsed;
            return Visibility.Collapsed;
        }
    }
}
