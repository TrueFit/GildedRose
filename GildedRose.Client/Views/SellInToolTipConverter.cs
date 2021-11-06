using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace GildedRose.Client.Views
{
    /// <summary>
    /// Converter from the SellIn value of an item to a tool-tip text
    /// </summary>
    public class SellInToolTipConverter : MarkupExtension, IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return $"SellIn: null";

            return $"SellIn: {value}";
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
