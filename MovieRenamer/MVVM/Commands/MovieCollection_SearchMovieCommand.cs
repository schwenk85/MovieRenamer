using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MovieRenamer.MVVM.Commands
{
    public class MovieCollection_SearchMovieCommand : ICommand
    {
        #region Fields

        protected readonly MovieCollection _MovieCollection;

        #endregion

        #region Constructors

        public MovieCollection_SearchMovieCommand(MovieCollection movieCollection)
        {
            _MovieCollection = movieCollection;
        }

        #endregion

        #region CanExecute/Execute

        public bool CanExecute(object parameter)
        {
            return (_MovieCollection.SelectedMovie != null);
        }

        public void Execute(object parameter)
        {
            string movieName = _MovieCollection.SelectedMovie.OriginalMovieName;
            _MovieCollection.SetImdbSearchString(movieName);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion
    }
}
