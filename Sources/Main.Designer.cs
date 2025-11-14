
using System.Drawing;
using System.Windows.Forms;
using iReverse_UniSPD_FRP.UI;

namespace iReverse_UniSPD_FRP
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // Header
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            
            // Left Panel
            this.panelLeft = new System.Windows.Forms.Panel();
            this.brandTabControl = new BrandTabControl();
            this.subTabControl = new SubTabControl();
            this.PanelSPDOneClick = new System.Windows.Forms.Panel();
            
            // Right Panel
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelRightTop = new System.Windows.Forms.Panel();
            this.ComboPort = new System.Windows.Forms.ComboBox();
            this.btnInfo = new System.Windows.Forms.Button();
            this.btnReboot = new System.Windows.Forms.Button();
            this.Logs = new System.Windows.Forms.RichTextBox();
            this.panelRightBottom = new System.Windows.Forms.Panel();
            this.btn_STOP = new System.Windows.Forms.Button();
            
            // Footer
            this.panelFooter = new System.Windows.Forms.Panel();
            this.btnSamsungFirmware = new System.Windows.Forms.Button();
            this.btnTelegram = new System.Windows.Forms.Button();
            this.btnConfig = new System.Windows.Forms.Button();
            this.btnAccount = new System.Windows.Forms.Button();
            
            // Hidden controls (mantidos para compatibilidade)
            this.ListBoxview = new System.Windows.Forms.ListBox();
            this.ListBoxViewSearch = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.CkFDLLoaded = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_resp = new System.Windows.Forms.Label();
            this.comboBoxTimeout = new System.Windows.Forms.ComboBox();
            
            this.panelHeader.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelRightTop.SuspendLayout();
            this.panelRightBottom.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(0x3c, 0x42, 0x4e); // ModernTheme.PanelBackground
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Height = 50;
            this.panelHeader.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.panelHeader.Name = "panelHeader";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(0xd1, 0xd5, 0xdb); // ModernTheme.TextPrimary
            this.lblTitle.Location = new System.Drawing.Point(10, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Text = "iREVERSE UNISPD FRP TOOLS";
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.ForeColor = System.Drawing.Color.FromArgb(0x9c, 0xa3, 0xaf); // ModernTheme.TextSecondary
            this.lblCopyright.Anchor = System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom;
            this.lblCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Text = "© iReverse UniSPD";
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.FromArgb(0x3c, 0x42, 0x4e); // ModernTheme.PanelBackground
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeft.Margin = new System.Windows.Forms.Padding(10);
            this.panelLeft.Padding = new System.Windows.Forms.Padding(10);
            this.panelLeft.Name = "panelLeft";
            // 
            // brandTabControl
            // 
            this.brandTabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.brandTabControl.Height = 45;
            this.brandTabControl.Name = "brandTabControl";
            // 
            // subTabControl
            // 
            this.subTabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.subTabControl.Height = 40;
            this.subTabControl.Name = "subTabControl";
            // 
            // PanelSPDOneClick
            // 
            this.PanelSPDOneClick.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelSPDOneClick.BackColor = System.Drawing.Color.FromArgb(0x3c, 0x42, 0x4e); // ModernTheme.PanelBackground
            this.PanelSPDOneClick.Margin = new System.Windows.Forms.Padding(10);
            this.PanelSPDOneClick.Name = "PanelSPDOneClick";
            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.Color.FromArgb(0x3c, 0x42, 0x4e); // ModernTheme.PanelBackground
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Width = 400;
            this.panelRight.Margin = new System.Windows.Forms.Padding(10);
            this.panelRight.Padding = new System.Windows.Forms.Padding(10);
            this.panelRight.Name = "panelRight";
            // 
            // panelRightTop
            // 
            this.panelRightTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelRightTop.Height = 50;
            this.panelRightTop.Padding = new System.Windows.Forms.Padding(10);
            this.panelRightTop.Name = "panelRightTop";
            // 
            // ComboPort
            // 
            this.ComboPort.BackColor = System.Drawing.Color.FromArgb(0x4a, 0x51, 0x60); // ModernTheme.TabBackground
            this.ComboPort.ForeColor = System.Drawing.Color.FromArgb(0xd1, 0xd5, 0xdb); // ModernTheme.TextPrimary
            this.ComboPort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComboPort.Width = 150;
            this.ComboPort.Height = 25;
            this.ComboPort.Location = new System.Drawing.Point(10, 12);
            this.ComboPort.Name = "ComboPort";
            this.ComboPort.SelectedIndexChanged += new System.EventHandler(this.ComboPort_SelectedIndexChanged);
            // 
            // btnInfo
            // 
            this.btnInfo.Text = "Info";
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Location = new System.Drawing.Point(170, 10);
            this.btnInfo.Size = new System.Drawing.Size(70, 30);
            // ModernTheme.ApplyButtonStyle(this.btnInfo); // Aplicado em Main.cs
            // 
            // btnReboot
            // 
            this.btnReboot.Text = "Reboot";
            this.btnReboot.Name = "btnReboot";
            this.btnReboot.Location = new System.Drawing.Point(250, 10);
            this.btnReboot.Size = new System.Drawing.Size(70, 30);
            // ModernTheme.ApplyButtonStyle(this.btnReboot); // Aplicado em Main.cs
            // 
            // Logs
            // 
            this.Logs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Logs.BackColor = System.Drawing.Color.FromArgb(0x1f, 0x29, 0x37); // ModernTheme.LogBackground
            this.Logs.ForeColor = System.Drawing.Color.FromArgb(0xd1, 0xd5, 0xdb); // ModernTheme.TextPrimary
            this.Logs.Font = new System.Drawing.Font("Consolas", 9F);
            this.Logs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Logs.ReadOnly = true;
            this.Logs.Margin = new System.Windows.Forms.Padding(10);
            this.Logs.Name = "Logs";
            // 
            // panelRightBottom
            // 
            this.panelRightBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelRightBottom.Height = 50;
            this.panelRightBottom.Padding = new System.Windows.Forms.Padding(10);
            this.panelRightBottom.Name = "panelRightBottom";
            // 
            // btn_STOP
            // 
            this.btn_STOP.Text = "STOP";
            this.btn_STOP.Anchor = System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top;
            this.btn_STOP.Location = new System.Drawing.Point(0, 0);
            this.btn_STOP.Size = new System.Drawing.Size(100, 35);
            this.btn_STOP.Name = "btn_STOP";
            // ModernTheme.ApplyButtonStyle(this.btn_STOP, true); // Aplicado em Main.cs
            this.btn_STOP.Click += new System.EventHandler(this.btn_STOP_Click);
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = System.Drawing.Color.FromArgb(0x3c, 0x42, 0x4e); // ModernTheme.PanelBackground
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Height = 40;
            this.panelFooter.Padding = new System.Windows.Forms.Padding(10);
            this.panelFooter.Name = "panelFooter";
            // 
            // btnSamsungFirmware
            // 
            this.btnSamsungFirmware.Text = "Samsung firmware";
            this.btnSamsungFirmware.Name = "btnSamsungFirmware";
            this.btnSamsungFirmware.Location = new System.Drawing.Point(10, 5);
            this.btnSamsungFirmware.Size = new System.Drawing.Size(130, 30);
            // ModernTheme.ApplyButtonStyle(this.btnSamsungFirmware); // Aplicado em Main.cs
            // 
            // btnTelegram
            // 
            this.btnTelegram.Text = "Telegram group";
            this.btnTelegram.Name = "btnTelegram";
            this.btnTelegram.Location = new System.Drawing.Point(150, 5);
            this.btnTelegram.Size = new System.Drawing.Size(130, 30);
            // ModernTheme.ApplyButtonStyle(this.btnTelegram); // Aplicado em Main.cs
            // 
            // btnConfig
            // 
            this.btnConfig.Text = "Config";
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Anchor = System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top;
            this.btnConfig.Location = new System.Drawing.Point(280, 5);
            this.btnConfig.Size = new System.Drawing.Size(80, 30);
            // ModernTheme.ApplyButtonStyle(this.btnConfig); // Aplicado em Main.cs
            // 
            // btnAccount
            // 
            this.btnAccount.Text = "Account";
            this.btnAccount.Name = "btnAccount";
            this.btnAccount.Anchor = System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top;
            this.btnAccount.Location = new System.Drawing.Point(370, 5);
            this.btnAccount.Size = new System.Drawing.Size(80, 30);
            // ModernTheme.ApplyButtonStyle(this.btnAccount); // Aplicado em Main.cs
            // 
            // Hidden controls (compatibilidade)
            // 
            this.ListBoxview.Visible = false;
            this.ListBoxview.Name = "ListBoxview";
            this.ListBoxview.SelectedIndexChanged += new System.EventHandler(this.ListBoxview_SelectedIndexChanged);
            // 
            this.ListBoxViewSearch.Visible = false;
            this.ListBoxViewSearch.Name = "ListBoxViewSearch";
            this.ListBoxViewSearch.TextChanged += new System.EventHandler(this.ListBoxViewSearch_TextChanged);
            // 
            this.progressBar1.Visible = false;
            this.progressBar1.Name = "progressBar1";
            // 
            this.CkFDLLoaded.Visible = false;
            this.CkFDLLoaded.Name = "CkFDLLoaded";
            // 
            this.label1.Visible = false;
            this.label1.Name = "label1";
            // 
            this.lbl_resp.Visible = false;
            this.lbl_resp.Name = "lbl_resp";
            // 
            this.comboBoxTimeout.Visible = false;
            this.comboBoxTimeout.Name = "comboBoxTimeout";
            this.comboBoxTimeout.SelectedIndexChanged += new System.EventHandler(this.comboBoxTimeout_SelectedIndexChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(0x2d, 0x32, 0x3b); // ModernTheme.BackgroundMain
            this.ClientSize = new System.Drawing.Size(1100, 720);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "iReverse UniSPD FRP Tools";
            
            // Adiciona controles ao header (copyright primeiro para anchor funcionar)
            this.panelHeader.Controls.Add(this.lblCopyright);
            this.panelHeader.Controls.Add(this.lblTitle);
            
            // Adiciona controles ao painel esquerdo
            this.panelLeft.Controls.Add(this.PanelSPDOneClick);
            this.panelLeft.Controls.Add(this.subTabControl);
            this.panelLeft.Controls.Add(this.brandTabControl);
            
            // Adiciona controles ao topo do painel direito
            this.panelRightTop.Controls.Add(this.btnReboot);
            this.panelRightTop.Controls.Add(this.btnInfo);
            this.panelRightTop.Controls.Add(this.ComboPort);
            
            // Adiciona controles ao rodapé do painel direito
            this.panelRightBottom.Controls.Add(this.btn_STOP);
            
            // Adiciona controles ao painel direito
            this.panelRight.Controls.Add(this.Logs);
            this.panelRight.Controls.Add(this.panelRightBottom);
            this.panelRight.Controls.Add(this.panelRightTop);
            
            // Adiciona controles ao footer
            this.panelFooter.Controls.Add(this.btnAccount);
            this.panelFooter.Controls.Add(this.btnConfig);
            this.panelFooter.Controls.Add(this.btnTelegram);
            this.panelFooter.Controls.Add(this.btnSamsungFirmware);
            
            // Adiciona painéis principais ao form
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelHeader);
            
            // Hidden controls
            this.Controls.Add(this.ListBoxview);
            this.Controls.Add(this.ListBoxViewSearch);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.CkFDLLoaded);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_resp);
            this.Controls.Add(this.comboBoxTimeout);
            
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelRightTop.ResumeLayout(false);
            this.panelRightBottom.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        // Header
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblCopyright;
        
        // Left Panel
        private System.Windows.Forms.Panel panelLeft;
        private BrandTabControl brandTabControl;
        private SubTabControl subTabControl;
        public System.Windows.Forms.Panel PanelSPDOneClick;
        
        // Right Panel
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelRightTop;
        public System.Windows.Forms.ComboBox ComboPort;
        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.Button btnReboot;
        public System.Windows.Forms.RichTextBox Logs;
        private System.Windows.Forms.Panel panelRightBottom;
        public System.Windows.Forms.Button btn_STOP;
        
        // Footer
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Button btnSamsungFirmware;
        private System.Windows.Forms.Button btnTelegram;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.Button btnAccount;
        
        // Hidden controls (mantidos para compatibilidade)
        public System.Windows.Forms.ListBox ListBoxview;
        internal System.Windows.Forms.TextBox ListBoxViewSearch;
        public System.Windows.Forms.ProgressBar progressBar1;
        public System.Windows.Forms.CheckBox CkFDLLoaded;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label lbl_resp;
        public System.Windows.Forms.ComboBox comboBoxTimeout;
    }
}

