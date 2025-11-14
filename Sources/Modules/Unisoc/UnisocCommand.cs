using System;
using System.Windows.Input;
using iReverse_UniSPD_FRP.Modules;
using iReverse_UniSPD_FRP.Services;

namespace iReverse_UniSPD_FRP.Modules.Unisoc
{
    /// <summary>
    /// Comando do módulo Unisoc
    /// </summary>
    public class UnisocCommand : IModuleCommand
    {
        private readonly string _operation;
        private readonly UnisocViewModel _viewModel;
        private readonly RelayCommand _command;

        public string Name => _operation;
        public string Description => $"Executa operação {_operation} para dispositivos Unisoc/Spreadtrum";
        public System.Windows.Input.ICommand Command => _command;
        public bool CanExecute => !_viewModel.IsOperationRunning;

        public UnisocCommand(string operation, UnisocViewModel viewModel)
        {
            _operation = operation ?? throw new ArgumentNullException(nameof(operation));
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            _command = new RelayCommand(
                async (obj) => await _viewModel.ExecuteOperationAsync(_operation),
                (obj) => !_viewModel.IsOperationRunning
            );

            // Atualiza CanExecute quando o estado muda
            _viewModel.OperationRunningChanged += (s, isRunning) =>
            {
                _command.RaiseCanExecuteChanged();
            };
        }
    }
}

