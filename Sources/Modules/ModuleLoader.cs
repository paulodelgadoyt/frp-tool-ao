using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using iReverse_UniSPD_FRP.My;

namespace iReverse_UniSPD_FRP.Modules
{
    /// <summary>
    /// Loader que descobre, instancia e injeta módulos automaticamente via reflection
    /// </summary>
    public class ModuleLoader
    {
        private readonly Dictionary<string, IBrandModule> _loadedModules;

        /// <summary>
        /// Lista de módulos carregados
        /// </summary>
        public List<IBrandModule> LoadedModules => _loadedModules.Values.ToList();

        /// <summary>
        /// Evento disparado quando um módulo é carregado
        /// </summary>
        public event EventHandler<IBrandModule> ModuleLoaded;

        /// <summary>
        /// Evento disparado quando ocorre erro ao carregar módulo
        /// </summary>
        public event EventHandler<(string ModuleName, Exception Error)> ModuleLoadError;

        public ModuleLoader()
        {
            _loadedModules = new Dictionary<string, IBrandModule>();
        }

        /// <summary>
        /// Carrega todos os módulos disponíveis automaticamente via reflection
        /// </summary>
        public async Task LoadModulesAsync()
        {
            try
            {
                MyDisplay.RichLogs("Iniciando descoberta automática de módulos...", 
                    System.Drawing.Color.Blue, true, true);

                // Busca todos os tipos que implementam IBrandModule
                var moduleTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a =>
                    {
                        try
                        {
                            return a.GetTypes();
                        }
                        catch (ReflectionTypeLoadException ex)
                        {
                            // Ignora assemblies que não podem ser carregados
                            return ex.Types.Where(t => t != null);
                        }
                        catch
                        {
                            return Enumerable.Empty<Type>();
                        }
                    })
                    .Where(t => t != null &&
                                typeof(IBrandModule).IsAssignableFrom(t) &&
                                !t.IsInterface &&
                                !t.IsAbstract &&
                                !t.IsGenericTypeDefinition);

                int loadedCount = 0;
                int errorCount = 0;

                foreach (var type in moduleTypes)
                {
                    try
                    {
                        // Cria instância do módulo
                        var module = (IBrandModule)Activator.CreateInstance(type);
                        
                        if (module == null)
                        {
                            MyDisplay.RichLogs($"Falha ao criar instância de {type.Name}", 
                                System.Drawing.Color.Red, true, true);
                            errorCount++;
                            continue;
                        }

                        // Verifica se já existe módulo com mesmo nome
                        if (_loadedModules.ContainsKey(module.Name))
                        {
                            MyDisplay.RichLogs($"Módulo '{module.Name}' já está carregado. Ignorando duplicata.", 
                                System.Drawing.Color.Orange, true, true);
                            continue;
                        }

                        // Inicializa o módulo
                        await module.Initialize();

                        // Adiciona à lista de módulos carregados
                        _loadedModules[module.Name] = module;
                        loadedCount++;

                        MyDisplay.RichLogs($"Módulo '{module.Name}' carregado com sucesso", 
                            System.Drawing.Color.Green, true, true);

                        // Dispara evento
                        ModuleLoaded?.Invoke(this, module);
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        string moduleName = type.Name;
                        
                        MyDisplay.RichLogs($"Erro ao carregar módulo '{moduleName}': {ex.Message}", 
                            System.Drawing.Color.Red, true, true);

                        // Dispara evento de erro
                        ModuleLoadError?.Invoke(this, (moduleName, ex));
                    }
                }

                MyDisplay.RichLogs($"Descoberta concluída: {loadedCount} módulos carregados, {errorCount} erros", 
                    System.Drawing.Color.Black, true, true);
            }
            catch (Exception ex)
            {
                MyDisplay.RichLogs($"Erro crítico ao carregar módulos: {ex.Message}", 
                    System.Drawing.Color.Red, true, true);
                throw;
            }
        }

        /// <summary>
        /// Carrega módulos de forma síncrona (para compatibilidade)
        /// </summary>
        public void LoadModules()
        {
            LoadModulesAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Obtém módulo por nome
        /// </summary>
        public IBrandModule GetModule(string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName))
            {
                return null;
            }

            _loadedModules.TryGetValue(moduleName, out IBrandModule module);
            return module;
        }

        /// <summary>
        /// Obtém módulo por tipo
        /// </summary>
        public T GetModule<T>() where T : class, IBrandModule
        {
            return _loadedModules.Values.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Verifica se módulo está carregado
        /// </summary>
        public bool IsModuleLoaded(string moduleName)
        {
            return _loadedModules.ContainsKey(moduleName);
        }

        /// <summary>
        /// Obtém todos os módulos de um tipo específico
        /// </summary>
        public IEnumerable<T> GetModules<T>() where T : class, IBrandModule
        {
            return _loadedModules.Values.OfType<T>();
        }

        /// <summary>
        /// Descarrega todos os módulos
        /// </summary>
        public void UnloadAll()
        {
            foreach (var module in _loadedModules.Values)
            {
                try
                {
                    module?.Dispose();
                }
                catch (Exception ex)
                {
                    MyDisplay.RichLogs($"Erro ao descarregar módulo '{module?.Name}': {ex.Message}", 
                        System.Drawing.Color.Red, true, true);
                }
            }

            _loadedModules.Clear();
            MyDisplay.RichLogs("Todos os módulos foram descarregados", 
                System.Drawing.Color.Orange, true, true);
        }

        /// <summary>
        /// Recarrega todos os módulos
        /// </summary>
        public async Task ReloadModulesAsync()
        {
            UnloadAll();
            await LoadModulesAsync();
        }
    }
}

