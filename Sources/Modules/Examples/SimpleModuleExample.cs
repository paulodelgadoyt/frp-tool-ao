using System.Windows.Forms;
using iReverse_UniSPD_FRP.Modules;
using iReverse_UniSPD_FRP.Modules.Examples;

namespace iReverse_UniSPD_FRP.Modules.Examples
{
    /// <summary>
    /// Exemplo super simples de como criar um módulo usando BrandModuleBase
    /// Este é o padrão mais simples possível
    /// </summary>
    public class SimpleModuleExample : BrandModuleBase
    {
        private readonly SimpleViewModel _viewModel = new SimpleViewModel();

        public override string Name => "Example";

        public override UserControl View => new SimpleView { DataContext = _viewModel };

        public override IModuleViewModel ViewModel => _viewModel;
    }

    // ViewModel de exemplo
    public class SimpleViewModel : IModuleViewModel
    {
        public string BrandName => "Example";
        public System.Collections.ObjectModel.ObservableCollection<IModuleCommand> Commands { get; } 
            = new System.Collections.ObjectModel.ObservableCollection<IModuleCommand>();
        public bool IsOperationRunning { get; set; }
        public event System.EventHandler<bool> OperationRunningChanged;

        public System.Threading.Tasks.Task Initialize()
        {
            return System.Threading.Tasks.Task.CompletedTask;
        }

        public void CancelOperation() { }
    }

    // View de exemplo
    public class SimpleView : UserControl
    {
        public object DataContext { get; set; }
    }
}

