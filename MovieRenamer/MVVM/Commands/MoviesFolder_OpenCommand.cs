using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Forms;

namespace MovieRenamer.MVVM.Commands
{
    public class MoviesFolder_OpenCommand : ICommand
    {
        #region Fields

        protected readonly MovieRenamerViewModel _ViewModel;

        #endregion

        #region Constructors

        public MoviesFolder_OpenCommand(MovieRenamerViewModel viewModel)
        {
            _ViewModel = viewModel;
        }

        #endregion

        #region CanExecute/Execute

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                _ViewModel.MoviesFolder = dialog.SelectedPath;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion
    }
}
