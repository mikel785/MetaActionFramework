using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace MAF.Converters
{
    /// <summary>
    /// Base class for all single value converters.
    /// Supprorts markup extension for XAML coding sugar.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ConverterBase<T> : MarkupExtension, IValueConverter where T : class, new()
    {
        /// <summary>
        /// Must be implemented in inheritor.
        /// </summary>
        public abstract object Convert(object value, Type targetType, object parameter,
            CultureInfo culture);

        /// <summary>
        /// Override if needed.
        /// </summary>
        public virtual object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #region MarkupExtension members

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }

        private static T _converter;

        public static T Instance { get { return _converter ?? (_converter = new T()); } }

        #endregion
    }
}
