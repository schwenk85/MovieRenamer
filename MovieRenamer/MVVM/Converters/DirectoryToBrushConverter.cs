using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;

namespace MovieRenamer.MVVM.Converters
{
    public class DirectoryToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && Directory.Exists((string) value))
            {
                return Brushes.Black;
            }

            return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}