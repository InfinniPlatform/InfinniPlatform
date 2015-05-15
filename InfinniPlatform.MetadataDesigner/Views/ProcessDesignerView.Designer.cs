namespace InfinniPlatform.MetadataDesigner.Views
{
	partial class ProcessDesignerView
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
			this.PanelBusinessProcessTemplate = new DevExpress.XtraEditors.PanelControl();
			this.PanelLabel = new DevExpress.XtraEditors.PanelControl();
			this.ProcessTemplateEditor = new DevExpress.XtraEditors.ComboBoxEdit();
			this.LabelControlType = new DevExpress.XtraEditors.LabelControl();
			((System.ComponentModel.ISupportInitialize)(this.PanelBusinessProcessTemplate)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PanelLabel)).BeginInit();
			this.PanelLabel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ProcessTemplateEditor.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// PanelBusinessProcessTemplate
			// 
			this.PanelBusinessProcessTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PanelBusinessProcessTemplate.Location = new System.Drawing.Point(0, 51);
			this.PanelBusinessProcessTemplate.LookAndFeel.SkinName = "Office 2013";
			this.PanelBusinessProcessTemplate.LookAndFeel.UseDefaultLookAndFeel = false;
			this.PanelBusinessProcessTemplate.Name = "PanelBusinessProcessTemplate";
			this.PanelBusinessProcessTemplate.Size = new System.Drawing.Size(806, 492);
			this.PanelBusinessProcessTemplate.TabIndex = 2;
			// 
			// PanelLabel
			// 
			this.PanelLabel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.PanelLabel.Controls.Add(this.ProcessTemplateEditor);
			this.PanelLabel.Controls.Add(this.LabelControlType);
			this.PanelLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.PanelLabel.Location = new System.Drawing.Point(0, 0);
			this.PanelLabel.LookAndFeel.SkinName = "Office 2013";
			this.PanelLabel.LookAndFeel.UseDefaultLookAndFeel = false;
			this.PanelLabel.Name = "PanelLabel";
			this.PanelLabel.Size = new System.Drawing.Size(806, 51);
			this.PanelLabel.TabIndex = 1;
			// 
			// ProcessTemplateEditor
			// 
			this.ProcessTemplateEditor.Location = new System.Drawing.Point(183, 17);
			this.ProcessTemplateEditor.Name = "ProcessTemplateEditor";
			this.ProcessTemplateEditor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.ProcessTemplateEditor.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
			this.ProcessTemplateEditor.Size = new System.Drawing.Size(233, 20);
			this.ProcessTemplateEditor.TabIndex = 1;
			this.ProcessTemplateEditor.EditValueChanged += new System.EventHandler(this.ProcessTemplateEditorEditValueChanged);
			// 
			// LabelControlType
			// 
			this.LabelControlType.Location = new System.Drawing.Point(19, 20);
			this.LabelControlType.Name = "LabelControlType";
			this.LabelControlType.Size = new System.Drawing.Size(126, 13);
			this.LabelControlType.TabIndex = 0;
			this.LabelControlType.Text = "Business process template";
			// 
			// ProcessDesignerView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.PanelBusinessProcessTemplate);
			this.Controls.Add(this.PanelLabel);
			this.Name = "ProcessDesignerView";
			this.Size = new System.Drawing.Size(806, 543);
			((System.ComponentModel.ISupportInitialize)(this.PanelBusinessProcessTemplate)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PanelLabel)).EndInit();
			this.PanelLabel.ResumeLayout(false);
			this.PanelLabel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ProcessTemplateEditor.Properties)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.PanelControl PanelBusinessProcessTemplate;
		private DevExpress.XtraEditors.PanelControl PanelLabel;
		private DevExpress.XtraEditors.LabelControl LabelControlType;
		private DevExpress.XtraEditors.ComboBoxEdit ProcessTemplateEditor;
	}
}
