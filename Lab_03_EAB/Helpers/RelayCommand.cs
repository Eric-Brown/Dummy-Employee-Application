using System;
using System.Windows.Input;

namespace Lab_03_EAB.Helpers
{
    internal class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> action)
            : this(action, null)
        {
        }

        public RelayCommand(Action<object> action, Predicate<object> predicate)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            _execute = action;
            _canExecute = predicate;
        }

        public bool CanExecute(object parameter)
        {
            return (_canExecute == null) ? true : _canExecute(parameter);
        }

        public void Execute(object parameter) => _execute(parameter);
    }
}