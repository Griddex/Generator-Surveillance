using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Panel.Commands
{
    public class GenericDelegateCommand<T> : ICommand
    {
        private Action<T> execute;
        private Predicate<T> canExecute;

        public GenericDelegateCommand(Action<T> execute) : this(execute, null)
        {

        }
        public GenericDelegateCommand(Action<T> execute, Predicate<T> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        
        public bool CanExecute(object parameter)
        {
            return this.execute == null ? true : this.canExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            this.execute((T)parameter);
        }
    }
}
