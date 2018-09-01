using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace MovieRenamer.MVVM.Converters
{
    public class FileInfoToFileNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? string.Empty : ((FileInfo)value).Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}