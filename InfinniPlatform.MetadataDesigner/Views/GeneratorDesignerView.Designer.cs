namespace InfinniPlatform.MetadataDesigner.Views
{
	partial class GeneratorDesignerView
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
			this.ButtonRefreshConfig = new DevExpress.XtraEditors.SimpleButton();
			this.ButtonCheckGenerator = new DevExpress.XtraEditors.SimpleButton();
			this.ComboBoxSelectGeneratorScenario = new DevExpress.XtraEditors.ImageComboBoxEdit();
			this.label32 = new System.Windows.Forms.Label();
			this.TextEditGeneratorName = new DevExpress.XtraEditors.TextEdit();
			this.label33 = new System.Windows.Forms.Label();
			this.panelControl8 = new DevExpress.XtraEditors.PanelControl();
			this.label1 = new System.Windows.Forms.Label();
			this.ComboBoxSelectViewType = new DevExpress.XtraEditors.ImageComboBoxEdit();
			this.ButtonRefreshScenario = new DevExpress.XtraEditors.SimpleButton();
			this.ButtonCreateGenerator = new DevExpress.XtraEditors.SimpleButton();
			this.sourceAssemblies = new System.Windows.Forms.BindingSource();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxSelectGeneratorScenario.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TextEditGeneratorName.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.panelControl8)).BeginInit();
			this.panelControl8.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxSelectViewType.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.sourceAssemblies)).BeginInit();
			this.SuspendLayout();
			// 
			// ButtonRefreshConfig
			// 
			this.ButtonRefreshConfig.Appearance.BackColor = System.Drawing.Color.LightGreen;
			this.ButtonRefreshConfig.Appearance.Options.UseBackColor = true;
			this.ButtonRefreshConfig.Location = new System.Drawing.Point(3, 101);
			this.ButtonRefreshConfig.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
			this.ButtonRefreshConfig.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ButtonRefreshConfig.Name = "ButtonRefreshConfig";
			this.ButtonRefreshConfig.Size = new System.Drawing.Size(209, 23);
			this.ButtonRefreshConfig.TabIndex = 34;
			this.ButtonRefreshConfig.Text = "Обновить локальную конфигурацию";
			// 
			// ButtonCheckGenerator
			// 
			this.ButtonCheckGenerator.Location = new System.Drawing.Point(381, 42);
			this.ButtonCheckGenerator.LookAndFeel.SkinName = "Office 2013";
			this.ButtonCheckGenerator.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ButtonCheckGenerator.Name = "ButtonCheckGenerator";
			this.ButtonCheckGenerator.Size = new System.Drawing.Size(156, 23);
			this.ButtonCheckGenerator.TabIndex = 33;
			this.ButtonCheckGenerator.Text = "Проверить генератор";
			// 
			// ComboBoxSelectGeneratorScenario
			// 
			this.ComboBoxSelectGeneratorScenario.Location = new System.Drawing.Point(168, 45);
			this.ComboBoxSelectGeneratorScenario.Name = "ComboBoxSelectGeneratorScenario";
			this.ComboBoxSelectGeneratorScenario.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.ComboBoxSelectGeneratorScenario.Size = new System.Drawing.Size(207, 20);
			this.ComboBoxSelectGeneratorScenario.TabIndex = 28;
			// 
			// label32
			// 
			this.label32.AutoSize = true;
			this.label32.Location = new System.Drawing.Point(10, 48);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(56, 13);
			this.label32.TabIndex = 16;
			this.label32.Text = "Сценарий";
			// 
			// TextEditGeneratorName
			// 
			this.TextEditGeneratorName.Location = new System.Drawing.Point(168, 15);
			this.TextEditGeneratorName.Name = "TextEditGeneratorName";
			this.TextEditGeneratorName.Size = new System.Drawing.Size(207, 20);
			this.TextEditGeneratorName.TabIndex = 15;
			// 
			// label33
			// 
			this.label33.AutoSize = true;
			this.label33.Location = new System.Drawing.Point(10, 18);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(144, 13);
			this.label33.TabIndex = 14;
			this.label33.Text = "Наименование генератора";
			// 
			// panelControl8
			// 
			this.panelControl8.Controls.Add(this.label1);
			this.panelControl8.Controls.Add(this.ComboBoxSelectViewType);
			this.panelControl8.Controls.Add(this.ButtonRefreshScenario);
			this.panelControl8.Controls.Add(this.ButtonRefreshConfig);
			this.panelControl8.Controls.Add(this.ButtonCheckGenerator);
			this.panelControl8.Controls.Add(this.ButtonCreateGenerator);
			this.panelControl8.Controls.Add(this.ComboBoxSelectGeneratorScenario);
			this.panelControl8.Controls.Add(this.label32);
			this.panelControl8.Controls.Add(this.TextEditGeneratorName);
			this.panelControl8.Controls.Add(this.label33);
			this.panelControl8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelControl8.Location = new System.Drawing.Point(0, 0);
			this.panelControl8.LookAndFeel.SkinName = "Office 2013";
			this.panelControl8.LookAndFeel.UseDefaultLookAndFeel = false;
			this.panelControl8.Name = "panelControl8";
			this.panelControl8.Size = new System.Drawing.Size(545, 129);
			this.panelControl8.TabIndex = 34;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(10, 77);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(106, 13);
			this.label1.TabIndex = 37;
			this.label1.Text = "Тип представления";
			// 
			// ComboBoxSelectViewType
			// 
			this.ComboBoxSelectViewType.Location = new System.Drawing.Point(168, 74);
			this.ComboBoxSelectViewType.Name = "ComboBoxSelectViewType";
			this.ComboBoxSelectViewType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.ComboBoxSelectViewType.Size = new System.Drawing.Size(207, 20);
			this.ComboBoxSelectViewType.TabIndex = 36;
			// 
			// ButtonRefreshScenario
			// 
			this.ButtonRefreshScenario.Location = new System.Drawing.Point(381, 71);
			this.ButtonRefreshScenario.LookAndFeel.SkinName = "Office 2013";
			this.ButtonRefreshScenario.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ButtonRefreshScenario.Name = "ButtonRefreshScenario";
			this.ButtonRefreshScenario.Size = new System.Drawing.Size(156, 23);
			this.ButtonRefreshScenario.TabIndex = 35;
			this.ButtonRefreshScenario.Text = "Обновить список сценариев";
			this.ButtonRefreshScenario.Click += new System.EventHandler(this.ButtonRefreshScenario_Click);
			// 
			// ButtonCreateGenerator
			// 
			this.ButtonCreateGenerator.Location = new System.Drawing.Point(381, 12);
			this.ButtonCreateGenerator.LookAndFeel.SkinName = "Office 2013";
			this.ButtonCreateGenerator.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ButtonCreateGenerator.Name = "ButtonCreateGenerator";
			this.ButtonCreateGenerator.Size = new System.Drawing.Size(156, 23);
			this.ButtonCreateGenerator.TabIndex = 30;
			this.ButtonCreateGenerator.Text = "Сформировать генератор";
			// 
			// GeneratorDesignerView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panelControl8);
			this.Name = "GeneratorDesignerView";
			this.Size = new System.Drawing.Size(545, 129);
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxSelectGeneratorScenario.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TextEditGeneratorName.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.panelControl8)).EndInit();
			this.panelControl8.ResumeLayout(false);
			this.panelControl8.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxSelectViewType.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.sourceAssemblies)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		public DevExpress.XtraEditors.SimpleButton ButtonRefreshConfig;
		public DevExpress.XtraEditors.SimpleButton ButtonCheckGenerator;
		public DevExpress.XtraEditors.ImageComboBoxEdit ComboBoxSelectGeneratorScenario;
		private System.Windows.Forms.Label label32;
		public DevExpress.XtraEditors.TextEdit TextEditGeneratorName;
		private System.Windows.Forms.Label label33;
		private DevExpress.XtraEditors.PanelControl panelControl8;
		public DevExpress.XtraEditors.SimpleButton ButtonCreateGenerator;
		public System.Windows.Forms.BindingSource sourceAssemblies;
		public DevExpress.XtraEditors.SimpleButton ButtonRefreshScenario;
		private System.Windows.Forms.Label label1;
		public DevExpress.XtraEditors.ImageComboBoxEdit ComboBoxSelectViewType;

	}
}
