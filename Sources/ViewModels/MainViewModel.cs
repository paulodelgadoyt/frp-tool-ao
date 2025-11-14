using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using iReverse_UniSPD_FRP.My;
using iReverse_UniSPD_FRP.Services;
using iReverse_UniSPD_FRP.UniSPD;

namespace iReverse_UniSPD_FRP.ViewModels
{
    /// <summary>
    /// ViewModel principal que gerencia a lógica de negócio e comandos
    /// </summary>
    public class MainViewModel
    {
        private readonly UnisocPortService _portService;
        private readonly FlashService _flashService;
        private readonly FRPService _frpService;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isOperationRunning = false;

        public MainViewModel()
        {
            _portService = new UnisocPortService();
            _flashService = new FlashService(_portService);
            _frpService = new FRPService(_portService, _flashService);
            _cancellationTokenSource = new CancellationTokenSource();

            // Eventos do serviço de porta
            _portService.Connected += OnPortConnected;
            _portService.Disconnected += OnPortDisconnected;
        }

        /// <summary>
        /// Indica se uma operação está em execução
        /// </summary>
        public bool IsOperationRunning
        {
            get => _isOperationRunning;
            private set
            {
                if (_isOperationRunning != value)
                {
                    _isOperationRunning = value;
                    OperationRunningChanged?.Invoke(this, value);
                }
            }
        }

        /// <summary>
        /// Evento disparado quando o estado de operação muda
        /// </summary>
        public event EventHandler<bool> OperationRunningChanged;

        /// <summary>
        /// Conecta à porta COM especificada
        /// </summary>
        public async Task<bool> ConnectToPortAsync(string portCom)
        {
            try
            {
                MyDisplay.RichLogs($"Conectando à porta {portCom}...", 
                    System.Drawing.Color.Black, true, true);
                
                bool connected = await _portService.ConnectAsync(portCom, _cancellationTokenSource.Token);
                
                if (connected)
                {
                    MyDisplay.RichLogs($"Conectado à porta COM{_portService.PortCom}", 
                        System.Drawing.Color.Green, true, true);
                }
                else
                {
                    MyDisplay.RichLogs("Falha ao conectar na porta", 
                        System.Drawing.Color.Red, true, true);
                }
                
                return connected;
            }
            catch (Exception ex)
            {
                MyDisplay.RichLogs($"Erro ao conectar: {ex.Message}", 
                    System.Drawing.Color.Red, true, true);
                return false;
            }
        }

        /// <summary>
        /// Desconecta da porta COM
        /// </summary>
        public void DisconnectFromPort()
        {
            _portService.Disconnect();
        }

        /// <summary>
        /// Carrega e faz download dos arquivos FDL
        /// </summary>
        public async Task<bool> LoadAndDownloadFDLAsync(string brand, string modelName)
        {
            if (!_portService.IsConnected)
            {
                MyDisplay.RichLogs("Conecte à porta COM primeiro", 
                    System.Drawing.Color.Red, true, true);
                return false;
            }

            try
            {
                IsOperationRunning = true;
                MyDisplay.RichLogs("Carregando arquivos FDL...", 
                    System.Drawing.Color.Black, true, true);

                // Garante que Main.myserial está configurado
                if (Main.myserial == null)
                {
                    Main.myserial = _portService.GetSerialDevice();
                }

                // Carrega arquivos FDL
                var fdlFiles = _flashService.LoadFDLFiles(brand, modelName);
                _flashService.LoadFDLAddresses(brand, modelName, fdlFiles);

                if (fdlFiles.FDL1.Length == 0 && fdlFiles.FDL2.Length == 0)
                {
                    MyDisplay.RichLogs("Arquivos FDL não encontrados", 
                        System.Drawing.Color.Red, true, true);
                    return false;
                }

                MyDisplay.RichLogs($"FDL1: {fdlFiles.FDL1.Length} bytes, FDL2: {fdlFiles.FDL2.Length} bytes", 
                    System.Drawing.Color.Black, true, true);

                // Faz download
                bool success = await _flashService.DownloadFDLAsync(fdlFiles, _cancellationTokenSource.Token);

                if (success)
                {
                    Main.SharedUI?.CkFDLLoaded?.Invoke(new Action(() => 
                    {
                        Main.SharedUI.CkFDLLoaded.Checked = true;
                    }));
                    MyDisplay.RichLogs("FDL carregado com sucesso", 
                        System.Drawing.Color.Green, true, true);
                }
                else
                {
                    MyDisplay.RichLogs("Falha ao carregar FDL", 
                        System.Drawing.Color.Red, true, true);
                }

                return success;
            }
            catch (OperationCanceledException)
            {
                MyDisplay.RichLogs("Download FDL cancelado", 
                    System.Drawing.Color.Orange, true, true);
                return false;
            }
            catch (Exception ex)
            {
                MyDisplay.RichLogs($"Erro ao carregar FDL: {ex.Message}", 
                    System.Drawing.Color.Red, true, true);
                return false;
            }
            finally
            {
                IsOperationRunning = false;
            }
        }

        /// <summary>
        /// Executa operação de remoção de FRP
        /// </summary>
        public async Task<bool> ExecuteFRPOperationAsync(string operation)
        {
            if (!_portService.IsConnected)
            {
                MyDisplay.RichLogs("Conecte à porta COM primeiro", 
                    System.Drawing.Color.Red, true, true);
                return false;
            }

            try
            {
                IsOperationRunning = true;
                Main.isUniSPDRunning = true;

                MyDisplay.RichLogs($"Executando: {operation}", 
                    System.Drawing.Color.Black, true, true);

                // Garante que Main.myserial está configurado
                if (Main.myserial == null)
                {
                    Main.myserial = _portService.GetSerialDevice();
                }

                // Executa operação via uni_worker (mantém compatibilidade)
                uni_worker.WorkerMethod = operation;
                await Task.Run(() => uni_worker.UniworkerTodo(_cancellationTokenSource.Token));

                MyDisplay.RichLogs("Operação concluída com sucesso", 
                    System.Drawing.Color.Green, true, true);
                return true;
            }
            catch (OperationCanceledException)
            {
                MyDisplay.RichLogs("Operação cancelada", 
                    System.Drawing.Color.Orange, true, true);
                return false;
            }
            catch (Exception ex)
            {
                MyDisplay.RichLogs($"Erro na operação: {ex.Message}", 
                    System.Drawing.Color.Red, true, true);
                return false;
            }
            finally
            {
                IsOperationRunning = false;
                Main.isUniSPDRunning = false;
            }
        }

        /// <summary>
        /// Cancela a operação em execução
        /// </summary>
        public void CancelOperation()
        {
            try
            {
                _cancellationTokenSource?.Cancel();
                Thread.Sleep(100);
                _cancellationTokenSource = new CancellationTokenSource();
                IsOperationRunning = false;
                Main.isUniSPDRunning = false;
                
                Main.SharedUI?.CkFDLLoaded?.Invoke(new Action(() => 
                {
                    Main.SharedUI.CkFDLLoaded.Checked = false;
                }));

                MyDisplay.RichLogs("Operação cancelada", 
                    System.Drawing.Color.Orange, true, true);
            }
            catch (Exception ex)
            {
                MyDisplay.RichLogs($"Erro ao cancelar: {ex.Message}", 
                    System.Drawing.Color.Red, true, true);
            }
        }

        /// <summary>
        /// Atualiza o timeout do serviço de porta
        /// </summary>
        public void UpdateTimeout(int timeoutMs)
        {
            _portService.Timeout = timeoutMs;
            MySerialDevice.maxtimeout = timeoutMs;
        }

        private void OnPortConnected(object sender, string portCom)
        {
            MyDisplay.RichLogs($"Porta COM{portCom} conectada", 
                System.Drawing.Color.Green, true, true);
        }

        private void OnPortDisconnected(object sender, EventArgs e)
        {
            MyDisplay.RichLogs("Porta COM desconectada", 
                System.Drawing.Color.Orange, true, true);
        }

        /// <summary>
        /// Libera recursos
        /// </summary>
        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _portService?.Dispose();
        }
    }
}

