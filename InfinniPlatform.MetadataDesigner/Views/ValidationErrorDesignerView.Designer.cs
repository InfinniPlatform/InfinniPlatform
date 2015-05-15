namespace InfinniPlatform.MetadataDesigner.Views
{
	partial class ValidationErrorDesignerView
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
			this.CreateButton = new DevExpress.XtraEditors.SimpleButton();
			this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
			this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
			this.DescriptionEditor = new DevExpress.XtraEditors.TextEdit();
			this.CaptionEditor = new DevExpress.XtraEditors.TextEdit();
			this.NameEditor = new DevExpress.XtraEditors.TextEdit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
			this.groupControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.DescriptionEditor.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.CaptionEditor.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.NameEditor.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// CreateButton
			// 
			this.CreateButton.Location = new System.Drawing.Point(369, 24);
			this.CreateButton.LookAndFeel.SkinName = "Office 2013";
			this.CreateButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.CreateButton.Name = "CreateButton";
			this.CreateButton.Size = new System.Drawing.Size(140, 23);
			this.CreateButton.TabIndex = 2;
			this.CreateButton.Text = "Сформировать";
			this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
			// 
			// groupControl1
			// 
			this.groupControl1.Controls.Add(this.CreateButton);
			this.groupControl1.Controls.Add(this.labelControl4);
			this.groupControl1.Controls.Add(this.labelControl3);
			this.groupControl1.Controls.Add(this.labelControl2);
			this.groupControl1.Controls.Add(this.DescriptionEditor);
			this.groupControl1.Controls.Add(this.CaptionEditor);
			this.groupControl1.Controls.Add(this.NameEditor);
			this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupControl1.Location = new System.Drawing.Point(0, 0);
			this.groupControl1.LookAndFeel.SkinName = "Office 2013";
			this.groupControl1.LookAndFeel.UseDefaultLookAndFeel = false;
			this.groupControl1.Name = "groupControl1";
			this.groupControl1.Size = new System.Drawing.Size(521, 105);
			this.groupControl1.TabIndex = 3;
			this.groupControl1.Text = "Общие реквизиты";
			// 
			// labelControl4
			// 
			this.labelControl4.Location = new System.Drawing.Point(19, 79);
			this.labelControl4.Name = "labelControl4";
			this.labelControl4.Size = new System.Drawing.Size(49, 13);
			this.labelControl4.TabIndex = 5;
			this.labelControl4.Text = "Описание";
			// 
			// labelControl3
			// 
			this.labelControl3.Location = new System.Drawing.Point(19, 53);
			this.labelControl3.Name = "labelControl3";
			this.labelControl3.Size = new System.Drawing.Size(53, 13);
			this.labelControl3.TabIndex = 4;
			this.labelControl3.Text = "Заголовок";
			// 
			// labelControl2
			// 
			this.labelControl2.Location = new System.Drawing.Point(19, 27);
			this.labelControl2.Name = "labelControl2";
			this.labelControl2.Size = new System.Drawing.Size(73, 13);
			this.labelControl2.TabIndex = 3;
			this.labelControl2.Text = "Наименование";
			// 
			// DescriptionEditor
			// 
			this.DescriptionEditor.Location = new System.Drawing.Point(119, 76);
			this.DescriptionEditor.Name = "DescriptionEditor";
			this.DescriptionEditor.Size = new System.Drawing.Size(244, 20);
			this.DescriptionEditor.TabIndex = 2;
			// 
			// CaptionEditor
			// 
			this.CaptionEditor.Location = new System.Drawing.Point(119, 50);
			this.CaptionEditor.Name = "CaptionEditor";
			this.CaptionEditor.Size = new System.Drawing.Size(244, 20);
			this.CaptionEditor.TabIndex = 1;
			// 
			// NameEditor
			// 
			this.NameEditor.Location = new System.Drawing.Point(119, 24);
			this.NameEditor.Name = "NameEditor";
			this.NameEditor.Size = new System.Drawing.Size(244, 20);
			this.NameEditor.TabIndex = 0;
			// 
			// ValidationWarningsDesignerView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupControl1);
			this.Name = "ValidationWarningsDesignerView";
			this.Size = new System.Drawing.Size(521, 108);
			((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
			this.groupControl1.ResumeLayout(false);
			this.groupControl1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.DescriptionEditor.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.CaptionEditor.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.NameEditor.Properties)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.SimpleButton CreateButton;
		private DevExpress.XtraEditors.GroupControl groupControl1;
		private DevExpress.XtraEditors.LabelControl labelControl4;
		private DevExpress.XtraEditors.LabelControl labelControl3;
		private DevExpress.XtraEditors.LabelControl labelControl2;
		private DevExpress.XtraEditors.TextEdit DescriptionEditor;
		private DevExpress.XtraEditors.TextEdit CaptionEditor;
		private DevExpress.XtraEditors.TextEdit NameEditor;
	}
}
