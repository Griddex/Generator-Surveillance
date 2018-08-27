using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Panel.Commands
{
    public class DelegateCommand : ICommand
    {
        private Action<object> execute;
        private Predicate<object> canExecute;

        public DelegateCommand(Action<object> execute) : this(execute, null)
        {

        }

        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
      
        public bool CanExecute(object parameter)
        {
            return this.execute == null ? true : this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
