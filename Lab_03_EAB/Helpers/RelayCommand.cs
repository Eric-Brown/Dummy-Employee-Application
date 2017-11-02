using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lab_03_EAB.Helpers
{
    class RelayCommand : ICommand
    {
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> action)
            :this(action, null)
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