using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace iReverse_UniSPD_FRP.UI
{
    /// <summary>
    /// Controle para sub-tabs (similar ao ItemsControl do XAML)
    /// </summary>
    public class SubTabControl : FlowLayoutPanel
    {
        private List<SubTabItem> _tabs = new List<SubTabItem>();
        private SubTabItem _selectedTab;

        public event EventHandler<SubTabItem> TabChanged;

        public SubTabItem SelectedTab
        {
            get => _selectedTab;
            set
            {
                if (_selectedTab != value)
                {
                    if (_selectedTab != null)
                    {
                        _selectedTab.IsSelected = false;
                    }
                    _selectedTab = value;
                    if (_selectedTab != null)
                    {
                        _selectedTab.IsSelected = true;
                    }
                    TabChanged?.Invoke(this, _selectedTab);
                }
            }
        }

        public SubTabControl()
        {
            this.FlowDirection = FlowDirection.LeftToRight;
            this.WrapContents = false;
            this.AutoSize = false;
            this.Height = 40;
            this.BackColor = System.Drawing.Color.FromArgb(0x4a, 0x51, 0x60); // ModernTheme.TabBackground
            this.Padding = new Padding(5);
        }

        /// <summary>
        /// Adiciona sub-tab
        /// </summary>
        public void AddSubTab(string name)
        {
            var tab = new SubTabItem(name);
            tab.Click += (s, e) => SelectedTab = tab;
            _tabs.Add(tab);
            this.Controls.Add(tab);
        }

        /// <summary>
        /// Limpa todas as tabs
        /// </summary>
        public void ClearTabs()
        {
            _tabs.Clear();
            this.Controls.Clear();
        }
    }

    /// <summary>
    /// Item de sub-tab
    /// </summary>
    public class SubTabItem : RadioButton
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                ApplySubTabStyleInternal(this, value);
                this.Checked = value;
            }
        }

        public SubTabItem(string name)
        {
            this.Text = name;
            this.Name = name;
            ApplySubTabStyleInternal(this, false);
            this.AutoSize = true;
        }
        
        private void ApplySubTabStyleInternal(RadioButton radioButton, bool isSelected)
        {
            radioButton.FlatStyle = FlatStyle.Flat;
            radioButton.FlatAppearance.BorderSize = 0;
            radioButton.BackColor = isSelected ? System.Drawing.Color.FromArgb(0x25, 0x63, 0xeb) : System.Drawing.Color.FromArgb(0x4a, 0x51, 0x60);
            radioButton.ForeColor = isSelected ? System.Drawing.Color.White : System.Drawing.Color.FromArgb(0xd1, 0xd5, 0xdb);
            radioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            radioButton.Padding = new Padding(10, 5, 10, 5);
            radioButton.Cursor = Cursors.Hand;
        }
    }
}

