using System.Drawing;
using System.Windows.Forms;
using iReverse_UniSPD_FRP.ViewModels.Platform;

namespace iReverse_UniSPD_FRP.Views.Platform
{
    public partial class MtkFlashView : UserControl
    {
        private MtkFlashViewModel _viewModel;
        private FlowLayoutPanel _commandsPanel;
        private Label _titleLabel;
        private Label _statusLabel;
        private Label _connectionLabel;

        public MtkFlashView(MtkFlashViewModel viewModel)
        {
            _viewModel = viewModel;
            InitializeComponent();
            LoadCommands();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            _titleLabel = new Label
            {
                Text = "MediaTek Flash",
                Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 10),
                ForeColor = Color.DarkOrange
            };

            _connectionLabel = new Label
            {
                Text = "MediaTek: Desconectado",
                AutoSize = true,
                Location = new Point(10, 35),
                ForeColor = Color.Red
            };

            _statusLabel = new Label
            {
                Text = "Status: Pronto",
                AutoSize = true,
                Location = new Point(10, 55),
                ForeColor = Color.Gray
            };

            _commandsPanel = new FlowLayoutPanel
            {
                Location = new Point(10, 80),
                Size = new Size(200, 280),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(5),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom
            };

            this.Controls.Add(_titleLabel);
            this.Controls.Add(_connectionLabel);
            this.Controls.Add(_statusLabel);
            this.Controls.Add(_commandsPanel);
            this.Size = new Size(220, 370);
            this.BackColor = Color.White;
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
                button.FlatAppearance.MouseOverBackColor = Color.Orange;

                button.Click += (s, e) =>
                {
                    if (command.Command.CanExecute(null))
                    {
                        command.Command.Execute(null);
                        UpdateStatus();
                    }
                };

                button.Enabled = command.CanExecute;
                _commandsPanel.Controls.Add(button);
            }

            _viewModel.OperationRunningChanged += (s, isRunning) =>
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new System.Action(UpdateStatus));
                }
                else
                {
                    UpdateStatus();
                }
            };
        }

        private void UpdateStatus()
        {
            _connectionLabel.Text = _viewModel.IsMtkConnected 
                ? "MediaTek: Conectado" 
                : "MediaTek: Desconectado";
            _connectionLabel.ForeColor = _viewModel.IsMtkConnected 
                ? Color.Green 
                : Color.Red;

            _statusLabel.Text = _viewModel.IsOperationRunning 
                ? "Status: Executando..." 
                : "Status: Pronto";
            _statusLabel.ForeColor = _viewModel.IsOperationRunning 
                ? Color.Orange 
                : Color.Gray;
        }
    }
}

