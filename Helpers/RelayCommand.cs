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
            bool result;

            if (_canExecute == null)
            {
                result = true; // Команда всегда доступна
            }
            else if (parameter is T param)
            {
                result = _canExecute(param);
            }
            else
            {
                result = _canExecute(default); // Передаём `default(T)`, если параметр `null`
            }

            Debug.WriteLine($"CanExecute({parameter}): {result}");
            return result;
        }


        public void Execute(object parameter) 
        {
            Debug.WriteLine($"Execute called with parameter: {parameter?.GetType().FullName ?? "null"}");

            if (parameter is T param)
            {
                Debug.WriteLine($"Executing command with parameter: {param}");
                _execute(param);
            }
            else
            {

                Debug.WriteLine($"Executing command without parameter");
                Debug.WriteLine(parameter);
                _execute(default(T));
            }
        }
        
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
