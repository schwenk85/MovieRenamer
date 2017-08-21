using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                if (values[0] != DependencyProperty.UnsetValue && 
                    values[1] != DependencyProperty.UnsetValue)
                {
                    MovieCollection selectedMovieCollecion = (MovieCollection)values[0];
                    MovieCollections MovieCollections = (MovieCollections)values[1];

                    return (MovieCollections.IndexOf(selectedMovieCollecion) + 1).ToString() + "/" + MovieCollections.Count.ToString();
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
