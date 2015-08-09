using System;
using System.Drawing;
using System.Globalization;
using MAF.ExtensionMethods;

namespace MAF.Converters
{
    public class BitmapToBitmapSourceConverter : ConverterBase<BitmapToBitmapSourceConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Bitmap)
            {
                return (value as Bitmap).ToBitmapSource();
            }
            if (value is Image)
            {
                return (value as Image).ToBitmapSource();
            }

            return value;
        }
    }
}
