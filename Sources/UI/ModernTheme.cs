using System.Drawing;
using System.Windows.Forms;

namespace iReverse_UniSPD_FRP.UI
{
    /// <summary>
    /// Tema moderno escuro baseado no design XAML
    /// </summary>
    public static class ModernTheme
    {
        // Cores principais
        public static Color BackgroundMain => Color.FromArgb(0x2d, 0x32, 0x3b);      // #2d323b
        public static Color PanelBackground => Color.FromArgb(0x3c, 0x42, 0x4e);   // #3c424e
        public static Color TabBackground => Color.FromArgb(0x4a, 0x51, 0x60);      // #4a5160
        public static Color TabSelected => Color.FromArgb(0x25, 0x63, 0xeb);         // #2563eb
        public static Color TabUnselected => Color.FromArgb(0x1e, 0x40, 0xaf);      // #1e40af
        public static Color TextPrimary => Color.FromArgb(0xd1, 0xd5, 0xdb);        // #d1d5db
        public static Color TextSecondary => Color.FromArgb(0x9c, 0xa3, 0xaf);       // #9ca3af
        public static Color LogBackground => Color.FromArgb(0x1f, 0x29, 0x37);       // #1f2937
        public static Color StopButton => Color.FromArgb(0xef, 0x44, 0x44);         // #ef4444

        /// <summary>
        /// Aplica estilo de botão moderno
        /// </summary>
        public static void ApplyButtonStyle(Button button, bool isStopButton = false)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = isStopButton ? StopButton : TabBackground;
            button.ForeColor = isStopButton ? Color.White : TextPrimary;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            button.Padding = new Padding(12, 6, 12, 6);
            button.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Aplica estilo de tab (RadioButton)
        /// </summary>
        public static void ApplyTabStyle(RadioButton radioButton, bool isSelected = false)
        {
            radioButton.FlatStyle = FlatStyle.Flat;
            radioButton.FlatAppearance.BorderSize = 0;
            radioButton.BackColor = isSelected ? TabSelected : TabUnselected;
            radioButton.ForeColor = TextPrimary;
            radioButton.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            radioButton.Padding = new Padding(15, 8, 15, 8);
            radioButton.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Aplica estilo de sub-tab
        /// </summary>
        public static void ApplySubTabStyle(RadioButton radioButton, bool isSelected = false)
        {
            radioButton.FlatStyle = FlatStyle.Flat;
            radioButton.FlatAppearance.BorderSize = 0;
            radioButton.BackColor = isSelected ? TabSelected : TabBackground;
            radioButton.ForeColor = isSelected ? Color.White : TextPrimary;
            radioButton.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            radioButton.Padding = new Padding(10, 5, 10, 5);
            radioButton.Cursor = Cursors.Hand;
        }
    }
}

