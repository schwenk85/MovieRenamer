using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.IO;

namespace MovieRenamer.MVVM.Commands
{
    class MoviesFolder_ScanCommand : ICommand
    {
        #region Fields

        protected readonly MovieRenamerViewModel _ViewModel;

        #endregion

        #region Constructors

        public MoviesFolder_ScanCommand(MovieRenamerViewModel viewModel)
        {
            _ViewModel = viewModel;
        }

        #endregion

        #region CanExecute/Execute

        public bool CanExecute(object parameter)
        {
            return Directory.Exists(_ViewModel.MoviesFolder);
        }

        public void Execute(object parameter)
        {
            _ViewModel.ReadMovieCollections();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion
    }
}