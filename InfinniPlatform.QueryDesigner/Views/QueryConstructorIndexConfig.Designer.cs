namespace InfinniPlatform.QueryDesigner.Views
{
	partial class QueryConstructorIndexConfig
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
			this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
			this.TextEditAlias = new DevExpress.XtraEditors.TextEdit();
			this.LabelAlias = new DevExpress.XtraEditors.LabelControl();
			this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
			this.ComboBoxDocument = new DevExpress.XtraEditors.ImageComboBoxEdit();
			this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
			this.ComboBoxConfiguration = new DevExpress.XtraEditors.ImageComboBoxEdit();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.panelControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.TextEditAlias.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxDocument.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxConfiguration.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// panelControl1
			// 
			this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.panelControl1.Controls.Add(this.TextEditAlias);
			this.panelControl1.Controls.Add(this.LabelAlias);
			this.panelControl1.Controls.Add(this.labelControl2);
			this.panelControl1.Controls.Add(this.ComboBoxDocument);
			this.panelControl1.Controls.Add(this.labelControl1);
			this.panelControl1.Controls.Add(this.ComboBoxConfiguration);
			this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelControl1.Location = new System.Drawing.Point(0, 0);
			this.panelControl1.LookAndFeel.SkinName = "Office 2013";
			this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(566, 111);
			this.panelControl1.TabIndex = 0;
			// 
			// TextEditAlias
			// 
			this.TextEditAlias.Location = new System.Drawing.Point(109, 76);
			this.TextEditAlias.Name = "TextEditAlias";
			this.TextEditAlias.Size = new System.Drawing.Size(384, 20);
			this.TextEditAlias.TabIndex = 13;
			this.TextEditAlias.EditValueChanged += new System.EventHandler(this.TextEditAliasEditValueChanged);
			// 
			// LabelAlias
			// 
			this.LabelAlias.Location = new System.Drawing.Point(15, 79);
			this.LabelAlias.Name = "LabelAlias";
			this.LabelAlias.Size = new System.Drawing.Size(22, 13);
			this.LabelAlias.TabIndex = 12;
			this.LabelAlias.Text = "Alias";
			// 
			// labelControl2
			// 
			this.labelControl2.Location = new System.Drawing.Point(15, 49);
			this.labelControl2.Name = "labelControl2";
			this.labelControl2.Size = new System.Drawing.Size(48, 13);
			this.labelControl2.TabIndex = 11;
			this.labelControl2.Text = "Document";
			// 
			// ComboBoxDocument
			// 
			this.ComboBoxDocument.Location = new System.Drawing.Point(109, 46);
			this.ComboBoxDocument.Name = "ComboBoxDocument";
			this.ComboBoxDocument.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
			this.ComboBoxDocument.Size = new System.Drawing.Size(384, 20);
			this.ComboBoxDocument.TabIndex = 10;
			this.ComboBoxDocument.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.DeleteValueClick);
			this.ComboBoxDocument.EditValueChanged += new System.EventHandler(this.ComboBoxDocumentEditValueChanged);
			// 
			// labelControl1
			// 
			this.labelControl1.Location = new System.Drawing.Point(15, 19);
			this.labelControl1.Name = "labelControl1";
			this.labelControl1.Size = new System.Drawing.Size(65, 13);
			this.labelControl1.TabIndex = 9;
			this.labelControl1.Text = "Configuration";
			// 
			// ComboBoxConfiguration
			// 
			this.ComboBoxConfiguration.Location = new System.Drawing.Point(109, 16);
			this.ComboBoxConfiguration.Name = "ComboBoxConfiguration";
			this.ComboBoxConfiguration.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
			this.ComboBoxConfiguration.Size = new System.Drawing.Size(384, 20);
			this.ComboBoxConfiguration.TabIndex = 8;
			this.ComboBoxConfiguration.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.DeleteValueClick);
			this.ComboBoxConfiguration.EditValueChanged += new System.EventHandler(this.ComboBoxConfigurationEditValueChanged);
			// 
			// QueryConstructorIndexConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panelControl1);
			this.Name = "QueryConstructorIndexConfig";
			this.Size = new System.Drawing.Size(566, 111);
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.panelControl1.ResumeLayout(false);
			this.panelControl1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.TextEditAlias.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxDocument.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxConfiguration.Properties)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.PanelControl panelControl1;
		private DevExpress.XtraEditors.TextEdit TextEditAlias;
		private DevExpress.XtraEditors.LabelControl LabelAlias;
		private DevExpress.XtraEditors.LabelControl labelControl2;
		private DevExpress.XtraEditors.ImageComboBoxEdit ComboBoxDocument;
		private DevExpress.XtraEditors.LabelControl labelControl1;
		private DevExpress.XtraEditors.ImageComboBoxEdit ComboBoxConfiguration;


	}
}
