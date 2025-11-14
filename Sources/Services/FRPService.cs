using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using iReverse_UniSPD_FRP.My;
using iReverse_UniSPD_FRP.UniSPD;

namespace iReverse_UniSPD_FRP.Services
{
    /// <summary>
    /// Serviço responsável por operações de remoção de FRP
    /// </summary>
    public class FRPService
    {
        private readonly UnisocPortService _portService;
        private readonly FlashService _flashService;

        public FRPService(UnisocPortService portService, FlashService flashService)
        {
            _portService = portService ?? throw new ArgumentNullException(nameof(portService));
            _flashService = flashService ?? throw new ArgumentNullException(nameof(flashService));
        }

        /// <summary>
        /// Executa operação de remoção de FRP
        /// Nota: Este método é mantido para compatibilidade, mas a execução real
        /// é feita via uni_worker no ViewModel para manter o fluxo original
        /// </summary>
        public async Task<bool> ExecuteFRPOperationAsync(string operation, CancellationToken cancellationToken = default)
        {
            if (!_portService.IsConnected)
            {
                throw new InvalidOperationException("Porta COM não está conectada");
            }

            if (string.IsNullOrEmpty(operation))
            {
                throw new ArgumentException("Operação não pode ser vazia", nameof(operation));
            }

            try
            {
                // Verifica se FDL está carregado
                if (!_flashService.IsFDLLoaded())
                {
                    MyDisplay.RichLogs("FDL não está carregado. Execute o download primeiro.", 
                        System.Drawing.Color.Red, true, true);
                    return false;
                }

                // Define o método de trabalho
                uni_worker.WorkerMethod = operation;

                // Executa a operação
                await uni_worker.UniworkerTodo(cancellationToken);

                return true;
            }
            catch (OperationCanceledException)
            {
                MyDisplay.RichLogs("Operação cancelada pelo usuário", 
                    System.Drawing.Color.Orange, true, true);
                return false;
            }
            catch (Exception ex)
            {
                MyDisplay.RichLogs($"Erro ao executar operação FRP: {ex.Message}", 
                    System.Drawing.Color.Red, true, true);
                return false;
            }
        }

        /// <summary>
        /// Lista de operações FRP disponíveis
        /// </summary>
        public static readonly string[] AvailableOperations = new[]
        {
            "RECOVERY WIPE DATA I + FRP",
            "RECOVERY WIPE DATA II + FRP",
            "RECOVERY FORMAT DATA + FRP",
            "RECOVERY WIPE APP DATA + FRP",
            "RECOVERY WIPE DATA ONLY + FRP",
            "ERASE DATA + FRP",
            "ERASE FRP ONLY"
        };
    }
}

