using System;
using System.Windows.Input;
using iReverse_UniSPD_FRP.Modules;
using iReverse_UniSPD_FRP.Services;

namespace iReverse_UniSPD_FRP.ViewModels.Platform.PlatformCommands
{
    public class SamsungFrpCommand : IModuleCommand
    {
        private readonly string _operation;
        private readonly SamsungFrpViewModel _viewModel;
        private readonly RelayCommand _command;

        public string Name => _operation;
        public string Description => $"Executa {_operation} em dispositivos Samsung";
        public System.Windows.Input.ICommand Command => _command;
        public bool CanExecute => !_viewModel.IsOperationRunning;

        public SamsungFrpCommand(string operation, SamsungFrpViewModel viewModel)
        {
            _operation = operation;
            _viewModel = viewModel;

            _command = new RelayCommand(
                async (obj) => await _viewModel.ExecuteOperationAsync(_operation),
                (obj) => !_viewModel.IsOperationRunning
            );

            _viewModel.OperationRunningChanged += (s, isRunning) =>
            {
                _command.RaiseCanExecuteChanged();
            };
        }
    }
}

