using System;
using System.Globalization;
using System.Windows.Data;

namespace MovieRenamer.MVVM.Converters
{
    public class BlockingConverter : IValueConverter
    {
        private object _lastValue;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _lastValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            _lastValue = value;
            return value;
        }
    }
}