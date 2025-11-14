using System;
using System.Windows.Input;

namespace iReverse_UniSPD_FRP.Services
{
    /// <summary>
    /// Implementação básica de ICommand (System.Windows.Input) para facilitar binding
    /// Compatível com WinForms (não usa CommandManager do WPF)
    /// </summary>
    public class RelayCommand : System.Windows.Input.ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;
        private EventHandler _canExecuteChanged;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
            
            _execute = (o) => execute();
            _canExecute = canExecute != null ? (Func<object, bool>)((o) => canExecute()) : null;
        }

        public event EventHandler CanExecuteChanged
        {
            add { _canExecuteChanged += value; }
            remove { _canExecuteChanged -= value; }
        }

        /// <summary>
        /// Dispara o evento CanExecuteChanged manualmente (para WinForms)
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            _canExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}

