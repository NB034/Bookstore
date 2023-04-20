using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bookstore.Command
{
    class AutoEventCommandBase : ICommand
    {
        private readonly Action<object> _action;
        private readonly Func<object, bool> _predicate;

        public AutoEventCommandBase(Action<object> action, Func<object, bool> predicate)
        {
            _action = action;
            _predicate = predicate;
        }

        public bool CanExecute(object parameter) => _predicate(parameter);

        public void Execute(object parameter) => _action(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
