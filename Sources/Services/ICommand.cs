using System;

namespace iReverse_UniSPD_FRP.Services
{
    /// <summary>
    /// Interface para comandos que podem ser executados pela UI
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Indica se o comando pode ser executado
        /// </summary>
        bool CanExecute(object parameter);

        /// <summary>
        /// Executa o comando
        /// </summary>
        void Execute(object parameter);

        /// <summary>
        /// Evento disparado quando CanExecute muda
        /// </summary>
        event EventHandler CanExecuteChanged;
    }
}

