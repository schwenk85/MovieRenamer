using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace MovieRenamer.MVVM.Converters
{
    public class MoviesCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var counter = 0;

            if (value != null)
            {
                var movieCollections = (ObservableCollection<MovieCollection>) value;
                counter += movieCollections.Sum(movieCollection => movieCollection.Movies.Count);
            }

            return counter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}