using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using iReverse_UniSPD_FRP.My;
using iReverse_UniSPD_FRP.UniSPD;
using iReverse_UniSPD_FRP.UniSPD.Method;
using iReverse_UniSPD_FRP.ViewModels;
using iReverse_UniSPD_FRP.ViewModels.Platform;
using iReverse_UniSPD_FRP.Modules;
using iReverse_UniSPD_FRP.UI;
using System.Linq;

namespace iReverse_UniSPD_FRP
{
    public partial class Main : Form
    {
        public static Main SharedUI;
        public static bool isUniSPDRunning = false;
        public static CancellationTokenSource cts = new CancellationTokenSource();
        public static MySerialDevice myserial;

        private MainViewModel _viewModel;
        private ModuleManager _moduleManager;
        private PlatformViewModelManager _platformManager;

        /// <summary>
        /// ViewModel principal (acessível para outras classes)
        /// </summary>
        public MainViewModel ViewModel => _viewModel;

        public Main()
        {
            InitializeComponent();
            SharedUI = this;
            
            // Inicializa ViewModel
            _viewModel = new MainViewModel();
            _viewModel.OperationRunningChanged += ViewModel_OperationRunningChanged;
            
            // Inicializa ModuleManager
            _moduleManager = new ModuleManager();
            _moduleManager.ModuleChanged += ModuleManager_ModuleChanged;
            
            // Inicializa PlatformViewModelManager (troca automática de views)
            _platformManager = new PlatformViewModelManager();
            _platformManager.ViewChanged += PlatformManager_ViewChanged;
            
            // Inicializa componentes
            MyUSBFastConnect.getcomInfo();
            MyListSPDDevice.CreateListDevice();
            
            // Carrega módulos automaticamente via ModuleLoader
            LoadModulesAutomatically();
            
            // Ajusta posição do copyright após form carregar
            this.Load += Main_Load;
        }

        /// <summary>
        /// Ajusta layout após form carregar
        /// </summary>
        private void Main_Load(object sender, EventArgs e)
        {
            // Ajusta posição do copyright à direita
            if (lblCopyright != null && panelHeader != null)
            {
                lblCopyright.Location = new Point(
                    panelHeader.Width - lblCopyright.Width - 10,
                    (panelHeader.Height - lblCopyright.Height) / 2
                );
            }
            
            // Aplica estilos dos botões (não pode ser feito no Designer)
            ModernTheme.ApplyButtonStyle(this.btnInfo);
            ModernTheme.ApplyButtonStyle(this.btnReboot);
            ModernTheme.ApplyButtonStyle(this.btn_STOP, true);
            ModernTheme.ApplyButtonStyle(this.btnSamsungFirmware);
            ModernTheme.ApplyButtonStyle(this.btnTelegram);
            ModernTheme.ApplyButtonStyle(this.btnConfig);
            ModernTheme.ApplyButtonStyle(this.btnAccount);
        }

        /// <summary>
        /// Carrega módulos automaticamente usando ModuleLoader
        /// </summary>
        private async void LoadModulesAutomatically()
        {
            try
            {
                // Carrega módulos via reflection (descoberta automática)
                await _moduleManager.LoadModulesAutomaticallyAsync();
                
                MyDisplay.RichLogs($"Total de módulos carregados: {_moduleManager.AllModules.Count()}", 
                    System.Drawing.Color.Green, true, true);
                
                // Popula tabs de marcas após carregar módulos
                PopulateBrandTabs();
            }
            catch (Exception ex)
            {
                MyDisplay.RichLogs($"Erro ao carregar módulos automaticamente: {ex.Message}", 
                    System.Drawing.Color.Red, true, true);
            }
        }

        /// <summary>
        /// Popula tabs de marcas automaticamente baseado nos módulos carregados
        /// </summary>
        private void PopulateBrandTabs()
        {
            try
            {
                brandTabControl.ClearTabs();
                
                // Adiciona tab para cada módulo carregado
                foreach (var module in _moduleManager.AllModules)
                {
                    brandTabControl.AddBrandTab(module.Name, module);
                }
                
                // Seleciona primeiro módulo se houver
                if (brandTabControl.Controls.Count > 0)
                {
                    var firstTab = brandTabControl.Controls[0] as BrandTabItem;
                    if (firstTab != null)
                    {
                        brandTabControl.SelectedTab = firstTab;
                    }
                }
                
                // Evento quando tab muda
                brandTabControl.TabChanged += BrandTabControl_TabChanged;
            }
            catch (Exception ex)
            {
                MyDisplay.RichLogs($"Erro ao popular tabs: {ex.Message}", 
                    System.Drawing.Color.Red, true, true);
            }
        }

        /// <summary>
        /// Evento quando tab de marca muda
        /// </summary>
        private async void BrandTabControl_TabChanged(object sender, BrandTabItem tab)
        {
            if (tab?.Module != null)
            {
                await _moduleManager.LoadModuleAsync(tab.Module.Name);
            }
        }

        /// <summary>
        /// Evento quando módulo muda
        /// </summary>
        private void ModuleManager_ModuleChanged(object sender, IBrandModule module)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ModuleManager_ModuleChanged(sender, module)));
                return;
            }

            // Atualiza o painel com a view do módulo
            if (module?.View != null)
            {
                PanelSPDOneClick.Controls.Clear();
                module.View.Dock = DockStyle.Fill;
                PanelSPDOneClick.Controls.Add(module.View);
            }
        }

        /// <summary>
        /// Evento quando view de plataforma muda (equivalente ao DataBinding)
        /// </summary>
        private void PlatformManager_ViewChanged(object sender, UserControl newView)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => PlatformManager_ViewChanged(sender, newView)));
                return;
            }

            // Atualiza o painel com a nova view (troca automática)
            if (newView != null)
            {
                PanelSPDOneClick.Controls.Clear();
                newView.Dock = DockStyle.Fill;
                PanelSPDOneClick.Controls.Add(newView);
            }
        }

        private void comboBoxTimeout_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int timeout = Convert.ToInt32(
                    comboBoxTimeout.Text
                        .Replace("Timeout", "")
                        .Replace("-", "")
                        .Replace("ms", "")
                        .Replace(" ", "")
                );
                _viewModel?.UpdateTimeout(timeout);
            }
            catch (Exception ex)
            {
                MyDisplay.RichLogs($"Erro ao atualizar timeout: {ex.Message}", 
                    Color.Red, true, true);
            }
        }

        private async void ComboPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUniSPDRunning)
            {
                return; // Não permite mudar porta durante operação
            }

            if (!String.IsNullOrEmpty(ComboPort.Text))
            {
                Match match1 = Regex.Match(ComboPort.Text, @"\((COM\d+)\)");
                if (match1.Success)
                {
                    string portCom = match1.Groups[1].Value;
                    uni.PortCom = portCom.Replace("COM", "");
                    
                    // Conecta via ViewModel
                    await _viewModel.ConnectToPortAsync(portCom);
                }
            }
            else
            {
                uni.PortCom = "";
                _viewModel?.DisconnectFromPort();
            }
        }

        private void ListBoxViewSearch_TextChanged(object sender, EventArgs e)
        {
            if (ListBoxViewSearch.Text.Length > 0)
            {
                int i = 0;
                for (i = 0; i < ListBoxview.Items.Count; i++)
                {
                    if (
                        ListBoxview
                            .GetItemText(ListBoxview.Items[i])
                            .Contains(ListBoxViewSearch.Text)
                    )
                    {
                        ListBoxview.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private async void ListBoxview_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (object item in ListBoxview.SelectedItems)
            {
                if (!isUniSPDRunning)
                {
                    MyListSPDDevice.Info list = item as MyListSPDDevice.Info;
                    MyListSPDDevice.DevicesName = list.Devices;
                    MyListSPDDevice.ModelName = list.Models;
                    MyListSPDDevice.Platform = list.Platform;

                    string[] Brand = list.Devices.Split(
                        " ".ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries
                    );

                    MyListSPDDevice.Brand = Brand[0];

                    MyDisplay.RtbClear();
                    MyDisplay.RichLogs(
                        "Selected : " + list.Devices + " " + list.Models + " " + list.Platform,
                        Color.Black,
                        true,
                        true
                    );
                    
                    // Carrega módulo baseado na marca
                    await _moduleManager.LoadModuleByBrandAsync(Brand[0]);
                    
                    // Carrega ViewModel de plataforma baseado na marca/plataforma
                    // Exemplo: Samsung -> SamsungFrpViewModel, Android -> AndroidAdbViewModel
                    string platform = DeterminePlatform(Brand[0], list.Platform);
                    await _platformManager.LoadPlatformAsync(platform);
                    
                    // Mantém compatibilidade com código antigo
                    MethodOneClick.SPDOneClickExecModel();
                    break;
                }
            }
        }

        /// <summary>
        /// Determina qual plataforma carregar baseado na marca e plataforma do dispositivo
        /// </summary>
        private string DeterminePlatform(string brand, string platform)
        {
            // Lógica para determinar plataforma
            string brandUpper = brand.ToUpper();
            string platformUpper = platform?.ToUpper() ?? "";

            // Samsung -> Samsung FRP
            if (brandUpper.Contains("SAMSUNG"))
            {
                return "Samsung FRP";
            }

            // MediaTek -> MediaTek Flash
            if (brandUpper.Contains("MEDIATEK") || platformUpper.Contains("MTK") || platformUpper.Contains("MEDIATEK"))
            {
                return "MediaTek Flash";
            }

            // Android genérico -> Android ADB
            if (platformUpper.Contains("ANDROID") || platformUpper.Contains("ADB"))
            {
                return "Android ADB";
            }

            // Padrão: USB Detection
            return "USB Detection";
        }

        private void btn_STOP_Click(object sender, EventArgs e)
        {
            // Usa ViewModel para cancelar operação
            _viewModel?.CancelOperation();
        }

        private void ViewModel_OperationRunningChanged(object sender, bool isRunning)
        {
            // Atualiza estado da UI baseado no ViewModel
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ViewModel_OperationRunningChanged(sender, isRunning)));
                return;
            }

            btn_STOP.Enabled = isRunning;
            isUniSPDRunning = isRunning;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Libera recursos
            _platformManager?.Dispose();
            _moduleManager?.Dispose();
            _viewModel?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
