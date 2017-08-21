using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MovieRenamer.MVVM.Commands
{
    public class MovieCollection_ResetCommand : ICommand
    {
        #region Fields

        protected readonly MovieCollection _MovieCollection;

        #endregion

        #region Constructors

        public MovieCollection_ResetCommand(MovieCollection movieCollection)
        {
            _MovieCollection = movieCollection;
        }

        #endregion

        #region CanExecute/Execute

        public bool CanExecute(object parameter)
        {
            return (_MovieCollection != null);
        }

        public void Execute(object parameter)
        {
            _MovieCollection.Reset();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion
    }
}
