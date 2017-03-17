using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VendingMachine.Gui.BaseClasses
{
    public class CommandHandler:ICommand
    {
        readonly Action<object> _commandAction;
        readonly bool _canExecute;
        
        public CommandHandler(Action<object> commandAction, bool canExecute)
        {
            _commandAction = commandAction;
            _canExecute = canExecute;
        }
       
        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
           _commandAction(parameter);
        }
      
    }
}
