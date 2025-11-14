using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using iReverse_UniSPD_FRP.Modules;

namespace iReverse_UniSPD_FRP.UI
{
    /// <summary>
    /// Controle customizado para tabs de marcas (similar ao ItemsControl do XAML)
    /// </summary>
    public class BrandTabControl : FlowLayoutPanel
    {
        private List<BrandTabItem> _tabs = new List<BrandTabItem>();
        private BrandTabItem _selectedTab;

        public event EventHandler<BrandTabItem> TabChanged;

        public BrandTabItem SelectedTab
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

        public BrandTabControl()
        {
            this.FlowDirection = FlowDirection.LeftToRight;
            this.WrapContents = false;
            this.AutoSize = false;
            this.Height = 45;
            this.BackColor = System.Drawing.Color.FromArgb(0x4a, 0x51, 0x60); // ModernTheme.TabBackground
            this.Padding = new Padding(5);
        }

        /// <summary>
        /// Adiciona tab de marca
        /// </summary>
        public void AddBrandTab(string name, IBrandModule module = null)
        {
            var tab = new BrandTabItem(name, module);
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

        /// <summary>
        /// Seleciona tab por nome
        /// </summary>
        public void SelectTab(string name)
        {
            var tab = _tabs.FirstOrDefault(t => t.Name == name);
            if (tab != null)
            {
                SelectedTab = tab;
            }
        }
    }

    /// <summary>
    /// Item de tab de marca
    /// </summary>
    public class BrandTabItem : RadioButton
    {
        private bool _isSelected;
        public IBrandModule Module { get; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                ApplyTabStyleInternal(this, value);
                this.Checked = value;
            }
        }

        public BrandTabItem(string name, IBrandModule module = null)
        {
            this.Text = name;
            this.Name = name;
            this.Module = module;
            ApplyTabStyleInternal(this, false);
            this.AutoSize = true;
        }
        
        private void ApplyTabStyleInternal(RadioButton radioButton, bool isSelected)
        {
            radioButton.FlatStyle = FlatStyle.Flat;
            radioButton.FlatAppearance.BorderSize = 0;
            radioButton.BackColor = isSelected ? System.Drawing.Color.FromArgb(0x25, 0x63, 0xeb) : System.Drawing.Color.FromArgb(0x1e, 0x40, 0xaf);
            radioButton.ForeColor = System.Drawing.Color.FromArgb(0xd1, 0xd5, 0xdb);
            radioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            radioButton.Padding = new Padding(15, 8, 15, 8);
            radioButton.Cursor = Cursors.Hand;
        }
    }
}

