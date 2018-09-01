using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace MovieRenamer.MVVM.Converters
{
    class SelectedMovieCollectionToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length > 1)
            {
                if (values[0] != DependencyProperty.UnsetValue && values[1] != DependencyProperty.UnsetValue)
                {
                    var selectedMovieCollection = (MovieCollection)values[0];
                    var movieCollections = (ObservableCollection<MovieCollection>)values[1];

                    return 
                        (movieCollections.IndexOf(selectedMovieCollection) + 1).ToString() +
                        "/" + movieCollections.Count.ToString();
                }
            }

            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}