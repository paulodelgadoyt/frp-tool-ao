# Sistema de Módulos por Marca

Sistema de plugins modular que permite adicionar suporte para diferentes marcas de dispositivos.

## Estrutura

```
Modules/
├── IBrandModule.cs          # Interface base para módulos
├── IModuleViewModel.cs      # Interface para ViewModels
├── IModuleCommand.cs        # Interface para comandos
├── ModuleBase.cs            # Classe base abstrata
├── ModuleManager.cs         # Gerenciador de módulos
├── Unisoc/                  # Módulo Unisoc/Spreadtrum
│   ├── UnisocModule.cs
│   ├── UnisocViewModel.cs
│   ├── UnisocCommand.cs
│   └── UnisocView.cs
└── Samsung/                 # Módulo Samsung (exemplo)
    ├── SamsungModule.cs
    ├── SamsungViewModel.cs
    ├── SamsungCommand.cs
    └── SamsungView.cs
```

## Como Adicionar uma Nova Marca

### Passo 1: Criar Estrutura de Pastas

Crie uma nova pasta em `Modules/` com o nome da marca:
```
Modules/
└── Xiaomi/
```

### Passo 2: Criar os Arquivos do Módulo

#### 2.1. XiaomiModule.cs
```csharp
using System.Threading.Tasks;
using System.Windows.Forms;
using iReverse_UniSPD_FRP.Modules;

namespace iReverse_UniSPD_FRP.Modules.Xiaomi
{
    public class XiaomiModule : ModuleBase
    {
        public override string Name => "Xiaomi";

        protected override UserControl CreateView()
        {
            var view = new XiaomiView();
            view.SetViewModel(ViewModel);
            return view;
        }

        protected override IModuleViewModel CreateViewModel()
        {
            return new XiaomiViewModel();
        }
    }
}
```

#### 2.2. XiaomiViewModel.cs
```csharp
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using iReverse_UniSPD_FRP.Modules;
using iReverse_UniSPD_FRP.My;

namespace iReverse_UniSPD_FRP.Modules.Xiaomi
{
    public class XiaomiViewModel : IModuleViewModel
    {
        private bool _isOperationRunning;

        public string BrandName => "Xiaomi";
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

        public XiaomiViewModel()
        {
            Commands = new ObservableCollection<IModuleCommand>();
        }

        public async Task Initialize()
        {
            Commands.Clear();
            // Adicione seus comandos aqui
            Commands.Add(new XiaomiCommand("Xiaomi FRP Remove", this));
            await Task.CompletedTask;
        }

        public async Task ExecuteOperationAsync(string operation)
        {
            try
            {
                IsOperationRunning = true;
                // Implemente sua lógica aqui
                MyDisplay.RichLogs($"Executando: {operation}", 
                    System.Drawing.Color.Blue, true, true);
                await Task.Delay(1000);
            }
            finally
            {
                IsOperationRunning = false;
            }
        }

        public void CancelOperation()
        {
            IsOperationRunning = false;
        }
    }
}
```

#### 2.3. XiaomiCommand.cs
```csharp
using System;
using System.Windows.Input;
using iReverse_UniSPD_FRP.Modules;
using iReverse_UniSPD_FRP.Services;

namespace iReverse_UniSPD_FRP.Modules.Xiaomi
{
    public class XiaomiCommand : IModuleCommand
    {
        private readonly string _operation;
        private readonly XiaomiViewModel _viewModel;
        private readonly RelayCommand _command;

        public string Name => _operation;
        public string Description => $"Executa {_operation} para dispositivos Xiaomi";
        public ICommand Command => _command;
        public bool CanExecute => !_viewModel.IsOperationRunning;

        public XiaomiCommand(string operation, XiaomiViewModel viewModel)
        {
            _operation = operation;
            _viewModel = viewModel;

            _command = new RelayCommand(
                async (obj) => await _viewModel.ExecuteOperationAsync(_operation),
                (obj) => !_viewModel.IsOperationRunning
            );

            _viewModel.OperationRunningChanged += (s, isRunning) =>
            {
                System.Windows.Input.CommandManager.InvalidateRequerySuggested();
            };
        }
    }
}
```

#### 2.4. XiaomiView.cs
```csharp
using System.Drawing;
using System.Windows.Forms;
using iReverse_UniSPD_FRP.Modules;

namespace iReverse_UniSPD_FRP.Modules.Xiaomi
{
    public partial class XiaomiView : UserControl
    {
        private XiaomiViewModel _viewModel;
        private FlowLayoutPanel _commandsPanel;
        private Label _titleLabel;

        public XiaomiView()
        {
            InitializeComponent();
        }

        public void SetViewModel(IModuleViewModel viewModel)
        {
            _viewModel = viewModel as XiaomiViewModel;
            if (_viewModel != null)
            {
                LoadCommands();
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            _titleLabel = new Label
            {
                Text = "Xiaomi",
                Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 10),
                ForeColor = Color.DarkBlue
            };

            _commandsPanel = new FlowLayoutPanel
            {
                Location = new Point(10, 40),
                Size = new Size(200, 300),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(5),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom
            };

            this.Controls.Add(_titleLabel);
            this.Controls.Add(_commandsPanel);
            this.Size = new Size(220, 350);
            this.ResumeLayout(false);
        }

        private void LoadCommands()
        {
            _commandsPanel.Controls.Clear();
            if (_viewModel?.Commands == null) return;

            foreach (var command in _viewModel.Commands)
            {
                var button = new Button
                {
                    Text = command.Name,
                    Size = new Size(200, 30),
                    Margin = new Padding(5),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    Cursor = Cursors.Hand
                };

                button.FlatAppearance.BorderColor = Color.Gray;
                button.FlatAppearance.MouseOverBackColor = Color.LightGray;

                button.Click += (s, e) =>
                {
                    if (command.Command.CanExecute(null))
                    {
                        command.Command.Execute(null);
                    }
                };

                button.Enabled = command.CanExecute;
                _commandsPanel.Controls.Add(button);
            }
        }
    }
}
```

### Passo 3: Registrar o Módulo

No `Main.cs`, adicione o registro do módulo:

```csharp
private void RegisterModules()
{
    _moduleManager.RegisterModule(new UnisocModule());
    _moduleManager.RegisterModule(new SamsungModule());
    _moduleManager.RegisterModule(new XiaomiModule()); // ← Adicione aqui
}
```

### Passo 4: Adicionar ao Projeto

No arquivo `.csproj`, adicione os novos arquivos:

```xml
<Compile Include="Modules\Xiaomi\XiaomiModule.cs" />
<Compile Include="Modules\Xiaomi\XiaomiViewModel.cs" />
<Compile Include="Modules\Xiaomi\XiaomiCommand.cs" />
<Compile Include="Modules\Xiaomi\XiaomiView.cs" />
```

## Funcionamento

1. **Registro**: Módulos são registrados no `ModuleManager` durante a inicialização
2. **Carregamento**: Quando um dispositivo é selecionado, o sistema tenta carregar o módulo correspondente à marca
3. **View**: A View do módulo é exibida no painel `PanelSPDOneClick`
4. **Comandos**: Comandos do módulo são exibidos como botões na View

## Exemplo de Uso

```csharp
// O sistema automaticamente carrega o módulo quando um dispositivo é selecionado
// No ListBoxview_SelectedIndexChanged:
await _moduleManager.LoadModuleByBrandAsync("Xiaomi");

// Ou manualmente:
await _moduleManager.LoadModuleAsync("Xiaomi");
```

## Benefícios

- ✅ **Modular**: Cada marca tem seu próprio módulo isolado
- ✅ **Extensível**: Fácil adicionar novas marcas
- ✅ **Organizado**: Código separado por marca
- ✅ **Reutilizável**: ViewModels e Commands podem ser compartilhados
- ✅ **Testável**: Cada módulo pode ser testado independentemente

