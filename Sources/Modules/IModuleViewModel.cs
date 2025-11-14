using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace iReverse_UniSPD_FRP.Modules
{
    /// <summary>
    /// Interface base para ViewModels de módulos
    /// </summary>
    public interface IModuleViewModel
    {
        /// <summary>
        /// Nome da marca
        /// </summary>
        string BrandName { get; }

        /// <summary>
        /// Lista de comandos disponíveis
        /// </summary>
        ObservableCollection<IModuleCommand> Commands { get; }

        /// <summary>
        /// Indica se uma operação está em execução
        /// </summary>
        bool IsOperationRunning { get; set; }

        /// <summary>
        /// Evento disparado quando o estado de operação muda
        /// </summary>
        event EventHandler<bool> OperationRunningChanged;

        /// <summary>
        /// Inicializa o ViewModel
        /// </summary>
        Task Initialize();

        /// <summary>
        /// Cancela operação em execução
        /// </summary>
        void CancelOperation();
    }
}

