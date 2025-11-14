using System.Drawing;
using System.Windows.Forms;
using iReverse_UniSPD_FRP.ViewModels.Platform;

namespace iReverse_UniSPD_FRP.Views.Platform
{
    public partial class UsbDetectView : UserControl
    {
        private UsbDetectViewModel _viewModel;
        private FlowLayoutPanel _commandsPanel;
        private Label _titleLabel;
        private Label _statusLabel;
        private Label _deviceLabel;

        public UsbDetectView(UsbDetectViewModel viewModel)
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
                Text = "USB Detection",
                Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 10),
                ForeColor = Color.DarkViolet
            };

            _deviceLabel = new Label
            {
                Text = "Dispositivo: Nenhum",
                AutoSize = true,
                Location = new Point(10, 35),
                ForeColor = Color.Gray,
                MaximumSize = new Size(200, 0)
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
            this.Controls.Add(_deviceLabel);
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
                button.FlatAppearance.MouseOverBackColor = Color.Lavender;

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
            _deviceLabel.Text = string.IsNullOrEmpty(_viewModel.DetectedDevice)
                ? "Dispositivo: Nenhum"
                : $"Dispositivo: {_viewModel.DetectedDevice}";
            _deviceLabel.ForeColor = string.IsNullOrEmpty(_viewModel.DetectedDevice)
                ? Color.Gray
                : Color.Green;

            _statusLabel.Text = _viewModel.IsOperationRunning
                ? "Status: Executando..."
                : "Status: Pronto";
            _statusLabel.ForeColor = _viewModel.IsOperationRunning
                ? Color.Orange
                : Color.Gray;
        }
    }
}

