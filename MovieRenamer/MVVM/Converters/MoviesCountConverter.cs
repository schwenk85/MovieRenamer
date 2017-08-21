using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace MovieRenamer.MVVM.Converters
{
    public class MoviesCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int counter = 0;

            if (value != null)
            {
                foreach (MovieCollection movieCollection in (MovieCollections)value)
                {
                    counter += movieCollection.Movies.Count;
                }
            }

            return counter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
