using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using iReverse_UniSPD_FRP.Modules;

namespace iReverse_UniSPD_FRP.Modules.Unisoc
{
    /// <summary>
    /// View do módulo Unisoc
    /// </summary>
    public partial class UnisocView : UserControl
    {
        private UnisocViewModel _viewModel;
        private FlowLayoutPanel _commandsPanel;
        private Label _titleLabel;

        public UnisocView()
        {
            InitializeComponent();
        }

        public void SetViewModel(IModuleViewModel viewModel)
        {
            _viewModel = viewModel as UnisocViewModel;
            if (_viewModel != null)
            {
                LoadCommands();
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Título
            _titleLabel = new Label
            {
                Text = "Unisoc / Spreadtrum",
                Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 10),
                ForeColor = Color.DarkBlue
            };

            // Painel de comandos
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

            if (_viewModel?.Commands == null)
            {
                return;
            }

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

                // Binding do comando
                button.Click += (s, e) =>
                {
                    if (command.Command.CanExecute(null))
                    {
                        command.Command.Execute(null);
                    }
                };

                // Atualiza estado do botão
                UpdateButtonState(button, command);
                _commandsPanel.Controls.Add(button);
            }
        }

        private void UpdateButtonState(Button button, IModuleCommand command)
        {
            button.Enabled = command.CanExecute;
            button.BackColor = command.CanExecute ? Color.White : Color.LightGray;
        }
    }
}

