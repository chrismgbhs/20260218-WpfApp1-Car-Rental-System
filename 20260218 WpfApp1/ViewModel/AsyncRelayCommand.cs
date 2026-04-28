using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _20260218_WpfApp1.ViewModel
{
    // A specialized command for database and network operations
    public class AsyncRelayCommand : ICommand
    {
        // Notice we use Func<object?, Task> instead of Action
        private readonly Func<object, Task> _executeAsync;
        private bool _isExecuting;

        public AsyncRelayCommand(Func<object, Task> executeAsync)
        {
            _executeAsync = executeAsync;
        }

        // The button is only clickable if the task is NOT currently executing
        public bool CanExecute(object parameter) => !_isExecuting;

        // Links to the WPF CommandManager to refresh button states
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // ICommand forces this method to be void, but we safely manage it here
        public async void Execute(object parameter)
        {
            if (!CanExecute(parameter)) return;

            // 1. Lock the command so it can't be clicked again
            _isExecuting = true;
            CommandManager.InvalidateRequerySuggested();

            try
            {
                // 2. Safely AWAIT the background database job
                await _executeAsync(parameter);
            }
            finally
            {
                // 3. Unlock the command when the job finishes or fails
                _isExecuting = false;
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }
}
