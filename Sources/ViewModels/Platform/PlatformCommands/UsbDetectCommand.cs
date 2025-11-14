using System;
using System.Windows.Input;
using iReverse_UniSPD_FRP.Modules;
using iReverse_UniSPD_FRP.Services;

namespace iReverse_UniSPD_FRP.ViewModels.Platform.PlatformCommands
{
    public class UsbDetectCommand : IModuleCommand
    {
        private readonly string _operation;
        private readonly UsbDetectViewModel _viewModel;
        private readonly RelayCommand _command;

        public string Name => _operation;
        public string Description => $"Executa {_operation}";
        public System.Windows.Input.ICommand Command => _command;
        public bool CanExecute => !_viewModel.IsOperationRunning;

        public UsbDetectCommand(string operation, UsbDetectViewModel viewModel)
        {
            _operation = operation;
            _viewModel = viewModel;

            _command = new RelayCommand(
                async (obj) =>
                {
                    if (operation == "Scan USB Devices")
                    {
                        await _viewModel.ScanUsbDevicesAsync();
                    }
                    else
                    {
                        await _viewModel.ExecuteUsbOperationAsync(operation);
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

