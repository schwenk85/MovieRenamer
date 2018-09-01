using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace MovieRenamer.MVVM.Converters
{
    public class MovieCollectionsCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((ObservableCollection<MovieCollection>) value)?.Count ?? 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}