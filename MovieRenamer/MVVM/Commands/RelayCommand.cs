using System;
using System.Windows.Input;

namespace MovieRenamer.MVVM.Commands
{
    /// <summary>
    /// A reusable ICommand
    /// Relay Command: https://msdn.microsoft.com/en-us/magazine/dd419663.aspx#id0090030
    /// Delegate Command with "canExecute": https://www.wpftutorial.net/DelegateCommand.html
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        public RelayCommand(Action<object> execute) : this(execute, null) { }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}