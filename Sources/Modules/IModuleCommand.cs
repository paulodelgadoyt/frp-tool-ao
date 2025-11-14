using System;
using System.Windows.Input;

namespace iReverse_UniSPD_FRP.Modules
{
    /// <summary>
    /// Interface para comandos de módulo
    /// </summary>
    public interface IModuleCommand
    {
        /// <summary>
        /// Nome do comando (exibido na UI)
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Descrição do comando
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Comando ICommand para binding (System.Windows.Input.ICommand)
        /// </summary>
        System.Windows.Input.ICommand Command { get; }

        /// <summary>
        /// Indica se o comando pode ser executado
        /// </summary>
        bool CanExecute { get; }
    }
}

