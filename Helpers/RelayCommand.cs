using System;
using System.Windows.Input;
using System.Diagnostics;

namespace WpfAdminPanel.Helpers
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;
        //private readonly Predicate<T> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            bool result = _canExecute == null || (parameter is T param && _canExecute(param));
            Debug.WriteLine($"CanExecute({parameter}): {result}");
            return result;
        }
        
            
        public void Execute(object parameter) 
        {
            if (parameter is T param)
            {
                Debug.WriteLine($"Executing command with parameter: {param}");
                _execute(param);
            }
            else
            {
                Debug.WriteLine($"Executing command without parameter");
                _execute(default(T));
            }
        }
        
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
