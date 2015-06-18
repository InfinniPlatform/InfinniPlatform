namespace InfinniPlatform.MetadataDesigner.Views
{
	partial class ConfigDeployDesignerView
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigDeployDesignerView));
            this.panelControl7 = new DevExpress.XtraEditors.PanelControl();
            this.ButtonUpdateConfigurationAppliedAssemblies = new DevExpress.XtraEditors.SimpleButton();
            this.toggleDirectory = new DevExpress.XtraEditors.ToggleSwitch();
            this.ImportButton = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.TextEditServerPort = new DevExpress.XtraEditors.TextEdit();
            this.TextEditServerName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.ButtonExportConfig = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl7)).BeginInit();
            this.panelControl7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toggleDirectory.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEditServerPort.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEditServerName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl7
            // 
            this.panelControl7.Controls.Add(this.ButtonUpdateConfigurationAppliedAssemblies);
            this.panelControl7.Controls.Add(this.toggleDirectory);
            this.panelControl7.Controls.Add(this.ImportButton);
            this.panelControl7.Controls.Add(this.labelControl4);
            this.panelControl7.Controls.Add(this.TextEditServerPort);
            this.panelControl7.Controls.Add(this.TextEditServerName);
            this.panelControl7.Controls.Add(this.labelControl2);
            this.panelControl7.Controls.Add(this.ButtonExportConfig);
            this.panelControl7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl7.Location = new System.Drawing.Point(0, 0);
            this.panelControl7.LookAndFeel.SkinName = "Office 2013";
            this.panelControl7.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl7.Name = "panelControl7";
            this.panelControl7.Size = new System.Drawing.Size(731, 439);
            this.panelControl7.TabIndex = 14;
            // 
            // ButtonUpdateConfigurationAppliedAssemblies
            // 
            this.ButtonUpdateConfigurationAppliedAssemblies.Location = new System.Drawing.Point(19, 115);
            this.ButtonUpdateConfigurationAppliedAssemblies.LookAndFeel.SkinName = "Office 2013";
            this.ButtonUpdateConfigurationAppliedAssemblies.LookAndFeel.UseDefaultLookAndFeel = false;
            this.ButtonUpdateConfigurationAppliedAssemblies.Name = "ButtonUpdateConfigurationAppliedAssemblies";
            this.ButtonUpdateConfigurationAppliedAssemblies.Size = new System.Drawing.Size(250, 23);
            this.ButtonUpdateConfigurationAppliedAssemblies.TabIndex = 14;
            this.ButtonUpdateConfigurationAppliedAssemblies.Text = "Update configuration applied assemblies";
            this.ButtonUpdateConfigurationAppliedAssemblies.Click += new System.EventHandler(this.ButtonUpdateConfigurationAppliedAssemblies_Click);
            // 
            // toggleDirectory
            // 
            this.toggleDirectory.EditValue = true;
            this.toggleDirectory.Location = new System.Drawing.Point(18, 153);
            this.toggleDirectory.Name = "toggleDirectory";
            this.toggleDirectory.Properties.OffText = "Use file";
            this.toggleDirectory.Properties.OnText = "Use directory";
            this.toggleDirectory.Size = new System.Drawing.Size(252, 24);
            this.toggleDirectory.TabIndex = 13;
            // 
            // ImportButton
            // 
            this.ImportButton.Image = ((System.Drawing.Image)(resources.GetObject("ImportButton.Image")));
            this.ImportButton.Location = new System.Drawing.Point(20, 46);
            this.ImportButton.LookAndFeel.SkinName = "Office 2013";
            this.ImportButton.LookAndFeel.UseDefaultLookAndFeel = false;
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(250, 23);
            this.ImportButton.TabIndex = 12;
            this.ImportButton.Text = "Import configuration metadata";
            this.ImportButton.Click += new System.EventHandler(this.ImportButtonClick);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(549, 19);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(55, 13);
            this.labelControl4.TabIndex = 11;
            this.labelControl4.Text = "Server port";
            // 
            // TextEditServerPort
            // 
            this.TextEditServerPort.Location = new System.Drawing.Point(610, 16);
            this.TextEditServerPort.Name = "TextEditServerPort";
            this.TextEditServerPort.Size = new System.Drawing.Size(100, 20);
            this.TextEditServerPort.TabIndex = 10;
            // 
            // TextEditServerName
            // 
            this.TextEditServerName.Location = new System.Drawing.Point(419, 16);
            this.TextEditServerName.Name = "TextEditServerName";
            this.TextEditServerName.Size = new System.Drawing.Size(100, 20);
            this.TextEditServerName.TabIndex = 9;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(302, 19);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(73, 13);
            this.labelControl2.TabIndex = 6;
            this.labelControl2.Text = "Server address";
            // 
            // ButtonExportConfig
            // 
            this.ButtonExportConfig.Image = ((System.Drawing.Image)(resources.GetObject("ButtonExportConfig.Image")));
            this.ButtonExportConfig.Location = new System.Drawing.Point(19, 13);
            this.ButtonExportConfig.LookAndFeel.SkinName = "Office 2013";
            this.ButtonExportConfig.LookAndFeel.UseDefaultLookAndFeel = false;
            this.ButtonExportConfig.Name = "ButtonExportConfig";
            this.ButtonExportConfig.Size = new System.Drawing.Size(250, 23);
            this.ButtonExportConfig.TabIndex = 1;
            this.ButtonExportConfig.Text = "Export configuration file";
            this.ButtonExportConfig.Click += new System.EventHandler(this.ButtonExportConfigClick);
            // 
            // ConfigDeployDesignerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl7);
            this.Name = "ConfigDeployDesignerView";
            this.Size = new System.Drawing.Size(731, 439);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl7)).EndInit();
            this.panelControl7.ResumeLayout(false);
            this.panelControl7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toggleDirectory.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEditServerPort.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEditServerName.Properties)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

        private DevExpress.XtraEditors.PanelControl panelControl7;
		private DevExpress.XtraEditors.LabelControl labelControl4;
		public DevExpress.XtraEditors.TextEdit TextEditServerPort;
        public DevExpress.XtraEditors.TextEdit TextEditServerName;
        private DevExpress.XtraEditors.LabelControl labelControl2;
		public DevExpress.XtraEditors.SimpleButton ButtonExportConfig;
		public DevExpress.XtraEditors.SimpleButton ImportButton;
        private DevExpress.XtraEditors.ToggleSwitch toggleDirectory;
        public DevExpress.XtraEditors.SimpleButton ButtonUpdateConfigurationAppliedAssemblies;

	}
}
