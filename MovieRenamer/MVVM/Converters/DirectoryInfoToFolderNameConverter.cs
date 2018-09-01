using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace MovieRenamer.MVVM.Converters
{
    public class DirectoryInfoToFolderNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? string.Empty : ((DirectoryInfo)value).Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}