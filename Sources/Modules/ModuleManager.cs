using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using iReverse_UniSPD_FRP.My;

namespace iReverse_UniSPD_FRP.Modules
{
    /// <summary>
    /// Gerenciador de módulos de marca
    /// </summary>
    public class ModuleManager
    {
        private readonly Dictionary<string, IBrandModule> _modules = new Dictionary<string, IBrandModule>();
        private IBrandModule _currentModule;
        private ModuleLoader _moduleLoader;

        /// <summary>
        /// Módulo atualmente ativo
        /// </summary>
        public IBrandModule CurrentModule => _currentModule;

        /// <summary>
        /// ModuleLoader usado para carregar módulos automaticamente
        /// </summary>
        public ModuleLoader Loader => _moduleLoader;

        /// <summary>
        /// Lista de todos os módulos registrados
        /// </summary>
        public IEnumerable<IBrandModule> AllModules => _modules.Values;

        /// <summary>
        /// Inicializa o ModuleManager com ModuleLoader
        /// </summary>
        public ModuleManager()
        {
            _moduleLoader = new ModuleLoader();
            _moduleLoader.ModuleLoaded += OnModuleLoaded;
            _moduleLoader.ModuleLoadError += OnModuleLoadError;
        }

        /// <summary>
        /// Carrega módulos automaticamente via ModuleLoader
        /// </summary>
        public async Task LoadModulesAutomaticallyAsync()
        {
            await _moduleLoader.LoadModulesAsync();
            
            // Registra todos os módulos carregados
            foreach (var module in _moduleLoader.LoadedModules)
            {
                RegisterModule(module);
            }
        }

        /// <summary>
        /// Evento quando módulo é carregado pelo ModuleLoader
        /// </summary>
        private void OnModuleLoaded(object sender, IBrandModule module)
        {
            // Módulo já foi adicionado ao Loader, apenas log
            MyDisplay.RichLogs($"Módulo '{module.Name}' pronto para uso", 
                System.Drawing.Color.Green, true, true);
        }

        /// <summary>
        /// Evento quando ocorre erro ao carregar módulo
        /// </summary>
        private void OnModuleLoadError(object sender, (string ModuleName, Exception Error) errorInfo)
        {
            MyDisplay.RichLogs($"Erro ao carregar módulo '{errorInfo.ModuleName}': {errorInfo.Error.Message}", 
                System.Drawing.Color.Red, true, true);
        }

        /// <summary>
        /// Evento disparado quando o módulo ativo muda
        /// </summary>
        public event EventHandler<IBrandModule> ModuleChanged;

        /// <summary>
        /// Registra um módulo
        /// </summary>
        public void RegisterModule(IBrandModule module)
        {
            if (module == null)
            {
                throw new ArgumentNullException(nameof(module));
            }

            if (string.IsNullOrEmpty(module.Name))
            {
                throw new ArgumentException("Nome do módulo não pode ser vazio", nameof(module));
            }

            if (_modules.ContainsKey(module.Name))
            {
                MyDisplay.RichLogs($"Módulo '{module.Name}' já está registrado. Substituindo...", 
                    System.Drawing.Color.Orange, true, true);
            }

            _modules[module.Name] = module;
            MyDisplay.RichLogs($"Módulo '{module.Name}' registrado com sucesso", 
                System.Drawing.Color.Green, true, true);
        }

        /// <summary>
        /// Carrega módulo por nome
        /// </summary>
        public async Task<bool> LoadModuleAsync(string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName))
            {
                return false;
            }

            if (!_modules.TryGetValue(moduleName, out IBrandModule module))
            {
                MyDisplay.RichLogs($"Módulo '{moduleName}' não encontrado", 
                    System.Drawing.Color.Red, true, true);
                return false;
            }

            try
            {
                // Desativa módulo atual
                if (_currentModule != null && _currentModule != module)
                {
                    _currentModule.Dispose();
                }

                // Inicializa novo módulo
                await module.Initialize();
                _currentModule = module;

                ModuleChanged?.Invoke(this, module);

                MyDisplay.RichLogs($"Módulo '{moduleName}' carregado com sucesso", 
                    System.Drawing.Color.Green, true, true);
                return true;
            }
            catch (Exception ex)
            {
                MyDisplay.RichLogs($"Erro ao carregar módulo '{moduleName}': {ex.Message}", 
                    System.Drawing.Color.Red, true, true);
                return false;
            }
        }

        /// <summary>
        /// Carrega módulo baseado na marca do dispositivo
        /// </summary>
        public async Task<bool> LoadModuleByBrandAsync(string brand)
        {
            if (string.IsNullOrEmpty(brand))
            {
                return false;
            }

            // Normaliza o nome da marca
            string normalizedBrand = brand.ToUpper().Trim();

            // Procura módulo que corresponda à marca
            var module = _modules.Values.FirstOrDefault(m => 
                m.Name.ToUpper() == normalizedBrand || 
                normalizedBrand.Contains(m.Name.ToUpper()) ||
                m.Name.ToUpper().Contains(normalizedBrand));

            if (module != null)
            {
                return await LoadModuleAsync(module.Name);
            }

            // Se não encontrar, tenta carregar módulo padrão (Unisoc)
            if (_modules.ContainsKey("Unisoc"))
            {
                MyDisplay.RichLogs($"Módulo específico para '{brand}' não encontrado. Usando módulo Unisoc padrão.", 
                    System.Drawing.Color.Orange, true, true);
                return await LoadModuleAsync("Unisoc");
            }

            MyDisplay.RichLogs($"Nenhum módulo disponível para marca '{brand}'", 
                System.Drawing.Color.Red, true, true);
            return false;
        }

        /// <summary>
        /// Obtém módulo por nome
        /// </summary>
        public IBrandModule GetModule(string moduleName)
        {
            _modules.TryGetValue(moduleName, out IBrandModule module);
            return module;
        }

        /// <summary>
        /// Libera todos os recursos
        /// </summary>
        public void Dispose()
        {
            foreach (var module in _modules.Values)
            {
                module?.Dispose();
            }
            _modules.Clear();
            _currentModule = null;
        }
    }
}

