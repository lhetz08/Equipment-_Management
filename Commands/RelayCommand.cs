using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Equipment__Management.Commands
{
    internal class RelayCommand : ICommand
    {
        private readonly Action<object> _executeAction;

        public RelayCommand(Action<object> executionAction)
        {
            _executeAction = executionAction;
        }

        public bool CanExecute(object? parameter) => true;


        public void Execute(object? parameter) => _executeAction(parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }

    }
}
