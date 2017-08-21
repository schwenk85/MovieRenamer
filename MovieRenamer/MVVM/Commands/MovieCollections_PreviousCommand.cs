using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MovieRenamer.MVVM.Commands
{
    class MovieCollections_PreviousCommand : ICommand
    {
        #region Fields

        protected readonly MovieRenamerViewModel _ViewModel;

        #endregion

        #region Constructors

        public MovieCollections_PreviousCommand(MovieRenamerViewModel viewModel)
        {
            _ViewModel = viewModel;
        }

        #endregion

        #region CanExecute/Execute

        public bool CanExecute(object parameter)
        {
            return _ViewModel.MovieCollections.Count > 0;
        }

        public void Execute(object parameter)
        {
            _ViewModel.SetPreviousMovieCollection();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion
    }
}
