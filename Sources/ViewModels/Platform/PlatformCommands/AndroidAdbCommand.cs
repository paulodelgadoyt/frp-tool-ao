using System;
using System.Windows.Input;
using iReverse_UniSPD_FRP.Modules;
using iReverse_UniSPD_FRP.Services;

namespace iReverse_UniSPD_FRP.ViewModels.Platform.PlatformCommands
{
    public class AndroidAdbCommand : IModuleCommand
    {
        private readonly string _operation;
        private readonly AndroidAdbViewModel _viewModel;
        private readonly RelayCommand _command;

        public string Name => _operation;
        public string Description => $"Executa {_operation} via ADB";
        public System.Windows.Input.ICommand Command => _command;
        public bool CanExecute => !_viewModel.IsOperationRunning;

        public AndroidAdbCommand(string operation, AndroidAdbViewModel viewModel)
        {
            _operation = operation;
            _viewModel = viewModel;

            _command = new RelayCommand(
                async (obj) =>
                {
                    if (operation == "Connect ADB")
                    {
                        await _viewModel.ConnectAdbAsync();
                    }
                    else
                    {
                        await _viewModel.ExecuteAdbOperationAsync(operation);
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

