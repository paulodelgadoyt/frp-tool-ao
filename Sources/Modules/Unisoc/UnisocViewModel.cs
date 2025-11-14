using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using iReverse_UniSPD_FRP.Modules;
using iReverse_UniSPD_FRP.My;
using iReverse_UniSPD_FRP.Services;
using iReverse_UniSPD_FRP.ViewModels;

namespace iReverse_UniSPD_FRP.Modules.Unisoc
{
    /// <summary>
    /// ViewModel do módulo Unisoc
    /// </summary>
    public class UnisocViewModel : IModuleViewModel
    {
        private readonly MainViewModel _mainViewModel;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isOperationRunning;

        public string BrandName => "Unisoc";

        public ObservableCollection<IModuleCommand> Commands { get; private set; }

        public bool IsOperationRunning
        {
            get => _isOperationRunning;
            set
            {
                if (_isOperationRunning != value)
                {
                    _isOperationRunning = value;
                    OperationRunningChanged?.Invoke(this, value);
                }
            }
        }

        public event EventHandler<bool> OperationRunningChanged;

        public UnisocViewModel()
        {
            _mainViewModel = Main.SharedUI?.ViewModel ?? new MainViewModel();
            Commands = new ObservableCollection<IModuleCommand>();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task Initialize()
        {
            // Carrega comandos disponíveis
            Commands.Clear();

            var operations = FRPService.AvailableOperations;
            foreach (var operation in operations)
            {
                Commands.Add(new UnisocCommand(operation, this));
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Executa operação FRP
        /// </summary>
        public async Task ExecuteOperationAsync(string operation)
        {
            try
            {
                IsOperationRunning = true;
                await _mainViewModel.ExecuteFRPOperationAsync(operation);
            }
            catch (Exception ex)
            {
                MyDisplay.RichLogs($"Erro ao executar operação: {ex.Message}", 
                    System.Drawing.Color.Red, true, true);
            }
            finally
            {
                IsOperationRunning = false;
            }
        }

        public void CancelOperation()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            _mainViewModel?.CancelOperation();
            IsOperationRunning = false;
        }
    }
}

