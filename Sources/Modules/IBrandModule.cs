using System.Threading.Tasks;
using System.Windows.Forms;

namespace iReverse_UniSPD_FRP.Modules
{
    /// <summary>
    /// Interface base para módulos de marca
    /// </summary>
    public interface IBrandModule
    {
        /// <summary>
        /// Nome da marca (ex: "Samsung", "Xiaomi", "Unisoc")
        /// </summary>
        string Name { get; }

        /// <summary>
        /// View (UserControl) do módulo
        /// </summary>
        UserControl View { get; }

        /// <summary>
        /// ViewModel do módulo
        /// </summary>
        IModuleViewModel ViewModel { get; }

        /// <summary>
        /// Inicializa o módulo (carrega recursos, configura serviços, etc.)
        /// </summary>
        Task Initialize();

        /// <summary>
        /// Libera recursos do módulo
        /// </summary>
        void Dispose();
    }
}

