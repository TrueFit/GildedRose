using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace GildedRose.Client.Views
{
    /// <summary>
    /// Converting the quality value of an item to a displayable text
    /// </summary>
    public class QualityTextConverter : MarkupExtension, IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return "unknown";

            if (int.TryParse(value.ToString(), out var quality))
                return quality <= 0 ? "worthless" : quality.ToString();

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
