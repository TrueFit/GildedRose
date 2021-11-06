using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace GildedRose.Client.Views
{
    /// <summary>
    /// Converter from the Quality value of an item to a tool-tip text
    /// </summary>
    public class QualityToolTipConverter : MarkupExtension, IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return $"Quality: null";

            return $"Quality: {value}";
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
