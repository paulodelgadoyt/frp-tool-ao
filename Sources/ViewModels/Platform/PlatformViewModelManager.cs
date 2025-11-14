using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using iReverse_UniSPD_FRP.My;

namespace iReverse_UniSPD_FRP.ViewModels.Platform
{
    /// <summary>
    /// Gerenciador de ViewModels de plataforma
    /// Gerencia a troca automática de views baseado na plataforma selecionada
    /// </summary>
    public class PlatformViewModelManager
    {
        private BasePlatformViewModel _currentViewModel;
        private readonly Dictionary<string, BasePlatformViewModel> _viewModels;
        private UserControl _currentView;

        /// <summary>
        /// View atual (equivalente a CurrentModuleView no DataBinding)
        /// </summary>
        public UserControl CurrentModuleView
        {
            get => _currentView;
            private set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    ViewChanged?.Invoke(this, value);
                }
            }
        }

        /// <summary>
        /// ViewModel atual
        /// </summary>
        public BasePlatformViewModel CurrentViewModel => _currentViewModel;

        /// <summary>
        /// Evento disparado quando a view muda
        /// </summary>
        public event EventHandler<UserControl> ViewChanged;

        public PlatformViewModelManager()
        {
            _viewModels = new Dictionary<string, BasePlatformViewModel>();
            InitializeViewModels();
        }

        /// <summary>
        /// Inicializa todos os ViewModels disponíveis
        /// </summary>
        private void InitializeViewModels()
        {
            RegisterViewModel(new SamsungFrpViewModel());
            RegisterViewModel(new AndroidAdbViewModel());
            RegisterViewModel(new MtkFlashViewModel());
            RegisterViewModel(new UsbDetectViewModel());
        }

        /// <summary>
        /// Registra um ViewModel
        /// </summary>
        public void RegisterViewModel(BasePlatformViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            string key = viewModel.PlatformName;
            if (_viewModels.ContainsKey(key))
            {
                MyDisplay.RichLogs($"ViewModel '{key}' já está registrado. Substituindo...", 
                    System.Drawing.Color.Orange, true, true);
            }

            _viewModels[key] = viewModel;
            MyDisplay.RichLogs($"ViewModel '{key}' registrado", 
                System.Drawing.Color.Green, true, true);
        }

        /// <summary>
        /// Carrega ViewModel por nome da plataforma
        /// </summary>
        public async Task<bool> LoadPlatformAsync(string platformName)
        {
            if (string.IsNullOrEmpty(platformName))
            {
                return false;
            }

            // Normaliza o nome
            string normalized = platformName.ToUpper().Trim();

            // Procura ViewModel correspondente
            BasePlatformViewModel viewModel = null;
            foreach (var kvp in _viewModels)
            {
                if (kvp.Key.ToUpper().Contains(normalized) || 
                    normalized.Contains(kvp.Key.ToUpper()))
                {
                    viewModel = kvp.Value;
                    break;
                }
            }

            if (viewModel == null)
            {
                MyDisplay.RichLogs($"ViewModel para plataforma '{platformName}' não encontrado", 
                    System.Drawing.Color.Red, true, true);
                return false;
            }

            return await LoadViewModelAsync(viewModel);
        }

        /// <summary>
        /// Carrega ViewModel diretamente
        /// </summary>
        public async Task<bool> LoadViewModelAsync(BasePlatformViewModel viewModel)
        {
            if (viewModel == null)
            {
                return false;
            }

            try
            {
                // Desativa ViewModel atual
                if (_currentViewModel != null && _currentViewModel != viewModel)
                {
                    _currentViewModel.CancelOperation();
                }

                // Inicializa novo ViewModel
                await viewModel.Initialize();
                _currentViewModel = viewModel;

                // Atualiza view atual (equivalente ao DataBinding)
                CurrentModuleView = viewModel.View;

                MyDisplay.RichLogs($"Plataforma '{viewModel.PlatformName}' carregada", 
                    System.Drawing.Color.Green, true, true);
                return true;
            }
            catch (Exception ex)
            {
                MyDisplay.RichLogs($"Erro ao carregar plataforma: {ex.Message}", 
                    System.Drawing.Color.Red, true, true);
                return false;
            }
        }

        /// <summary>
        /// Carrega ViewModel por tipo
        /// </summary>
        public async Task<bool> LoadViewModelByTypeAsync<T>() where T : BasePlatformViewModel
        {
            foreach (var viewModel in _viewModels.Values)
            {
                if (viewModel is T)
                {
                    return await LoadViewModelAsync(viewModel);
                }
            }
            return false;
        }

        /// <summary>
        /// Obtém ViewModel por nome
        /// </summary>
        public BasePlatformViewModel GetViewModel(string platformName)
        {
            _viewModels.TryGetValue(platformName, out BasePlatformViewModel viewModel);
            return viewModel;
        }

        /// <summary>
        /// Lista todas as plataformas disponíveis
        /// </summary>
        public IEnumerable<string> GetAvailablePlatforms()
        {
            return _viewModels.Keys;
        }

        /// <summary>
        /// Libera recursos
        /// </summary>
        public void Dispose()
        {
            _currentViewModel?.CancelOperation();
            foreach (var viewModel in _viewModels.Values)
            {
                viewModel?.CancelOperation();
            }
            _viewModels.Clear();
            _currentView = null;
            _currentViewModel = null;
        }
    }
}

