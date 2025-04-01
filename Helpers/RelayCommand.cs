using System;
using System.Windows.Input;
using System.Diagnostics;

namespace WpfAdminPanel.Helpers
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool>? _canExecute;
        //private readonly Predicate<T> _canExecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<T> execute, Func<T, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }



        public bool CanExecute(object? parameter)
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
                if (default(T) != null || typeof(T).IsClass)
                {
                    result = _canExecute(default!);
                }
                else
                {
                    result = false; // Если T - значимый тип, `null` недопустим, команда недоступна
                }
            }
            return result;
        }


        public void Execute(object? parameter) 
        {
            if (parameter is T param)
            {
                _execute(param);
            }
            else
            {
                if (default(T) is null || typeof(T).IsClass)
                {
                    _execute(default!); // `default!` подавляет предупреждение
                }
                else
                {
                    throw new InvalidOperationException($"Команда ожидает параметр типа {typeof(T)}, но получила null.");
                }
            }
        }
        
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
