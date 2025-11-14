using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using iReverse_UniSPD_FRP.Modules;
using iReverse_UniSPD_FRP.My;

namespace iReverse_UniSPD_FRP.Modules
{
    /// <summary>
    /// Classe base abstrata que simplifica a criação de módulos de marca
    /// Basta herdar esta classe para criar qualquer nova marca
    /// </summary>
    public abstract class BrandModuleBase : IBrandModule, IDisposable
    {
        protected bool _disposed = false;

        /// <summary>
        /// Nome da marca (deve ser implementado)
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// View (UserControl) do módulo (deve ser implementado)
        /// </summary>
        public abstract UserControl View { get; }

        /// <summary>
        /// ViewModel do módulo (deve ser implementado)
        /// </summary>
        public abstract IModuleViewModel ViewModel { get; }

        /// <summary>
        /// Inicializa o módulo
        /// Override para adicionar lógica específica (logs, drivers, dependências, etc.)
        /// </summary>
        public virtual async Task Initialize()
        {
            try
            {
                MyDisplay.RichLogs($"Inicializando módulo '{Name}'...", 
                    System.Drawing.Color.Blue, true, true);

                // Inicializa o ViewModel se necessário
                if (ViewModel != null)
                {
                    await ViewModel.Initialize();
                }

                MyDisplay.RichLogs($"Módulo '{Name}' inicializado com sucesso", 
                    System.Drawing.Color.Green, true, true);
            }
            catch (Exception ex)
            {
                MyDisplay.RichLogs($"Erro ao inicializar módulo '{Name}': {ex.Message}", 
                    System.Drawing.Color.Red, true, true);
                throw;
            }
        }

        /// <summary>
        /// Libera recursos do módulo
        /// </summary>
        public virtual void Dispose()
        {
            if (!_disposed)
            {
                try
                {
                    ViewModel?.CancelOperation();
                    View?.Dispose();
                    
                    MyDisplay.RichLogs($"Módulo '{Name}' descarregado", 
                        System.Drawing.Color.Orange, true, true);
                }
                catch (Exception ex)
                {
                    MyDisplay.RichLogs($"Erro ao descarregar módulo '{Name}': {ex.Message}", 
                        System.Drawing.Color.Red, true, true);
                }
                finally
                {
                    _disposed = true;
                }
            }
        }
    }
}

