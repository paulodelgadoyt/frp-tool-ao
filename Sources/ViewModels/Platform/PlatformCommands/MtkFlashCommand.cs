using System;
using System.Windows.Input;
using iReverse_UniSPD_FRP.Modules;
using iReverse_UniSPD_FRP.Services;

namespace iReverse_UniSPD_FRP.ViewModels.Platform.PlatformCommands
{
    public class MtkFlashCommand : IModuleCommand
    {
        private readonly string _operation;
        private readonly MtkFlashViewModel _viewModel;
        private readonly RelayCommand _command;

        public string Name => _operation;
        public string Description => $"Executa {_operation} em dispositivos MediaTek";
        public System.Windows.Input.ICommand Command => _command;
        public bool CanExecute => !_viewModel.IsOperationRunning;

        public MtkFlashCommand(string operation, MtkFlashViewModel viewModel)
        {
            _operation = operation;
            _viewModel = viewModel;

            _command = new RelayCommand(
                async (obj) =>
                {
                    if (operation == "Connect MediaTek")
                    {
                        await _viewModel.ConnectMtkAsync();
                    }
                    else
                    {
                        await _viewModel.ExecuteMtkOperationAsync(operation);
                    }
                },
                (obj) => !_viewModel.IsOperationRunning
            );

            _viewModel.OperationRunningChanged += (s, isRunning) =>
            {
                _command.RaiseCanExecuteChanged();
            };
        }
    }
}

