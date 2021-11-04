using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace GildedRose.Client.Views
{
    /// <summary>
    /// Converting the SellIn value of an item into an expiration date
    /// </summary>
    public class ExpirationDateConverter : MarkupExtension, IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return "unknown";

            if (int.TryParse(value.ToString(), out var sellIn))
                return sellIn < 0 ? "expired" : $"{sellIn} days";

            return value;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
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
