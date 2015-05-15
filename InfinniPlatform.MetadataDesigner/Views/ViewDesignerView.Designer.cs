namespace InfinniPlatform.MetadataDesigner.Views
{
	partial class ViewDesignerView
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
			this.ComboBoxSelectViewType = new DevExpress.XtraEditors.ImageComboBoxEdit();
			this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
			this.DescriptionEditor = new DevExpress.XtraEditors.TextEdit();
			this.CaptionEditor = new DevExpress.XtraEditors.TextEdit();
			this.NameEditor = new DevExpress.XtraEditors.TextEdit();
			this.TabControlViewDesigner = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.GroupControlGenerator = new DevExpress.XtraEditors.GroupControl();
			this.MetadataGeneratorSelect = new DevExpress.XtraEditors.ImageComboBoxEdit();
			this.ButtonCheckGenerator = new DevExpress.XtraEditors.SimpleButton();
			this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
			this.TabPageDesigner = new System.Windows.Forms.TabPage();
			((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
			this.groupControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxSelectViewType.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.DescriptionEditor.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.CaptionEditor.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.NameEditor.Properties)).BeginInit();
			this.TabControlViewDesigner.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.GroupControlGenerator)).BeginInit();
			this.GroupControlGenerator.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.MetadataGeneratorSelect.Properties)).BeginInit();
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
			this.CreateButton.Text = "Create";
			this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
			// 
			// groupControl1
			// 
			this.groupControl1.Controls.Add(this.ComboBoxSelectViewType);
			this.groupControl1.Controls.Add(this.CreateButton);
			this.groupControl1.Controls.Add(this.labelControl5);
			this.groupControl1.Controls.Add(this.labelControl4);
			this.groupControl1.Controls.Add(this.labelControl3);
			this.groupControl1.Controls.Add(this.labelControl2);
			this.groupControl1.Controls.Add(this.DescriptionEditor);
			this.groupControl1.Controls.Add(this.CaptionEditor);
			this.groupControl1.Controls.Add(this.NameEditor);
			this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupControl1.Location = new System.Drawing.Point(3, 3);
			this.groupControl1.LookAndFeel.SkinName = "Office 2013";
			this.groupControl1.LookAndFeel.UseDefaultLookAndFeel = false;
			this.groupControl1.Name = "groupControl1";
			this.groupControl1.Size = new System.Drawing.Size(873, 133);
			this.groupControl1.TabIndex = 3;
			this.groupControl1.Text = "General properties";
			// 
			// ComboBoxSelectViewType
			// 
			this.ComboBoxSelectViewType.Location = new System.Drawing.Point(119, 102);
			this.ComboBoxSelectViewType.Name = "ComboBoxSelectViewType";
			this.ComboBoxSelectViewType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.ComboBoxSelectViewType.Size = new System.Drawing.Size(244, 20);
			this.ComboBoxSelectViewType.TabIndex = 37;
			// 
			// labelControl5
			// 
			this.labelControl5.Location = new System.Drawing.Point(19, 105);
			this.labelControl5.Name = "labelControl5";
			this.labelControl5.Size = new System.Drawing.Size(49, 13);
			this.labelControl5.TabIndex = 6;
			this.labelControl5.Text = "Form type";
			// 
			// labelControl4
			// 
			this.labelControl4.Location = new System.Drawing.Point(19, 79);
			this.labelControl4.Name = "labelControl4";
			this.labelControl4.Size = new System.Drawing.Size(53, 13);
			this.labelControl4.TabIndex = 5;
			this.labelControl4.Text = "Description";
			// 
			// labelControl3
			// 
			this.labelControl3.Location = new System.Drawing.Point(19, 53);
			this.labelControl3.Name = "labelControl3";
			this.labelControl3.Size = new System.Drawing.Size(37, 13);
			this.labelControl3.TabIndex = 4;
			this.labelControl3.Text = "Caption";
			// 
			// labelControl2
			// 
			this.labelControl2.Location = new System.Drawing.Point(19, 27);
			this.labelControl2.Name = "labelControl2";
			this.labelControl2.Size = new System.Drawing.Size(27, 13);
			this.labelControl2.TabIndex = 3;
			this.labelControl2.Text = "Name";
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
			// TabControlViewDesigner
			// 
			this.TabControlViewDesigner.Controls.Add(this.tabPage1);
			this.TabControlViewDesigner.Controls.Add(this.TabPageDesigner);
			this.TabControlViewDesigner.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TabControlViewDesigner.Location = new System.Drawing.Point(0, 0);
			this.TabControlViewDesigner.Name = "TabControlViewDesigner";
			this.TabControlViewDesigner.SelectedIndex = 0;
			this.TabControlViewDesigner.Size = new System.Drawing.Size(887, 624);
			this.TabControlViewDesigner.TabIndex = 4;
			this.TabControlViewDesigner.SelectedIndexChanged += new System.EventHandler(this.TabControlViewDesigner_SelectedIndexChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.GroupControlGenerator);
			this.tabPage1.Controls.Add(this.groupControl1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(879, 598);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "General";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// GroupControlGenerator
			// 
			this.GroupControlGenerator.Controls.Add(this.MetadataGeneratorSelect);
			this.GroupControlGenerator.Controls.Add(this.ButtonCheckGenerator);
			this.GroupControlGenerator.Controls.Add(this.labelControl1);
			this.GroupControlGenerator.Dock = System.Windows.Forms.DockStyle.Top;
			this.GroupControlGenerator.Location = new System.Drawing.Point(3, 136);
			this.GroupControlGenerator.LookAndFeel.SkinName = "Office 2013";
			this.GroupControlGenerator.LookAndFeel.UseDefaultLookAndFeel = false;
			this.GroupControlGenerator.Name = "GroupControlGenerator";
			this.GroupControlGenerator.Size = new System.Drawing.Size(873, 61);
			this.GroupControlGenerator.TabIndex = 4;
			this.GroupControlGenerator.Text = "Metadata";
			// 
			// MetadataGeneratorSelect
			// 
			this.MetadataGeneratorSelect.Location = new System.Drawing.Point(120, 29);
			this.MetadataGeneratorSelect.Name = "MetadataGeneratorSelect";
			this.MetadataGeneratorSelect.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.MetadataGeneratorSelect.Size = new System.Drawing.Size(243, 20);
			this.MetadataGeneratorSelect.TabIndex = 35;
			// 
			// ButtonCheckGenerator
			// 
			this.ButtonCheckGenerator.Location = new System.Drawing.Point(369, 27);
			this.ButtonCheckGenerator.LookAndFeel.SkinName = "Office 2013";
			this.ButtonCheckGenerator.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ButtonCheckGenerator.Name = "ButtonCheckGenerator";
			this.ButtonCheckGenerator.Size = new System.Drawing.Size(140, 23);
			this.ButtonCheckGenerator.TabIndex = 34;
			this.ButtonCheckGenerator.Text = "Check metadata";
			this.ButtonCheckGenerator.Click += new System.EventHandler(this.ButtonCheckGenerator_Click);
			// 
			// labelControl1
			// 
			this.labelControl1.Location = new System.Drawing.Point(19, 33);
			this.labelControl1.Name = "labelControl1";
			this.labelControl1.Size = new System.Drawing.Size(74, 13);
			this.labelControl1.TabIndex = 0;
			this.labelControl1.Text = "Select template";
			// 
			// TabPageDesigner
			// 
			this.TabPageDesigner.Location = new System.Drawing.Point(4, 22);
			this.TabPageDesigner.Name = "TabPageDesigner";
			this.TabPageDesigner.Padding = new System.Windows.Forms.Padding(3);
			this.TabPageDesigner.Size = new System.Drawing.Size(879, 598);
			this.TabPageDesigner.TabIndex = 1;
			this.TabPageDesigner.Text = "Designer";
			this.TabPageDesigner.UseVisualStyleBackColor = true;
			// 
			// ViewDesignerView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.TabControlViewDesigner);
			this.Name = "ViewDesignerView";
			this.Size = new System.Drawing.Size(887, 624);
			((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
			this.groupControl1.ResumeLayout(false);
			this.groupControl1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxSelectViewType.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.DescriptionEditor.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.CaptionEditor.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.NameEditor.Properties)).EndInit();
			this.TabControlViewDesigner.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.GroupControlGenerator)).EndInit();
			this.GroupControlGenerator.ResumeLayout(false);
			this.GroupControlGenerator.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.MetadataGeneratorSelect.Properties)).EndInit();
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
		private DevExpress.XtraEditors.LabelControl labelControl5;
		public DevExpress.XtraEditors.ImageComboBoxEdit ComboBoxSelectViewType;
		private DevExpress.XtraEditors.TextEdit NameEditor;
		private System.Windows.Forms.TabControl TabControlViewDesigner;
		private System.Windows.Forms.TabPage tabPage1;
		private DevExpress.XtraEditors.GroupControl GroupControlGenerator;
		private DevExpress.XtraEditors.ImageComboBoxEdit MetadataGeneratorSelect;
		public DevExpress.XtraEditors.SimpleButton ButtonCheckGenerator;
		private DevExpress.XtraEditors.LabelControl labelControl1;
		private System.Windows.Forms.TabPage TabPageDesigner;
	}
}
