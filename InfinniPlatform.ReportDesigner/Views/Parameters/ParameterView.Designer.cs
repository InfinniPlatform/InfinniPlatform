namespace InfinniPlatform.ReportDesigner.Views.Parameters
{
	partial class ParameterView
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
			this.components = new System.ComponentModel.Container();
			this.MainTabControl = new System.Windows.Forms.TabControl();
			this.GeneralTabPage = new System.Windows.Forms.TabPage();
			this.GeneralTabPagePanel = new System.Windows.Forms.Panel();
			this.DataTypeEdit = new System.Windows.Forms.ComboBox();
			this.AllowMultiplyValuesEdit = new System.Windows.Forms.CheckBox();
			this.AllowNullValueEdit = new System.Windows.Forms.CheckBox();
			this.DataTypeLabel = new System.Windows.Forms.Label();
			this.CaptionLabel = new System.Windows.Forms.Label();
			this.NameLabel = new System.Windows.Forms.Label();
			this.CaptionEdit = new System.Windows.Forms.TextBox();
			this.NameEdit = new System.Windows.Forms.TextBox();
			this.GeneralTabPageHelp = new System.Windows.Forms.Label();
			this.AvailableValuesTabPage = new System.Windows.Forms.TabPage();
			this.AvailableValuesEdit = new InfinniPlatform.ReportDesigner.Views.Parameters.ParameterValuesView();
			this.label1 = new System.Windows.Forms.Label();
			this.DefaultValuesTabPage = new System.Windows.Forms.TabPage();
			this.DefaultValuesEdit = new InfinniPlatform.ReportDesigner.Views.Parameters.ParameterValuesView();
			this.label2 = new System.Windows.Forms.Label();
			this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.MainTabControl.SuspendLayout();
			this.GeneralTabPage.SuspendLayout();
			this.GeneralTabPagePanel.SuspendLayout();
			this.AvailableValuesTabPage.SuspendLayout();
			this.DefaultValuesTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainTabControl
			// 
			this.MainTabControl.Controls.Add(this.GeneralTabPage);
			this.MainTabControl.Controls.Add(this.AvailableValuesTabPage);
			this.MainTabControl.Controls.Add(this.DefaultValuesTabPage);
			this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainTabControl.Location = new System.Drawing.Point(0, 0);
			this.MainTabControl.Multiline = true;
			this.MainTabControl.Name = "MainTabControl";
			this.MainTabControl.SelectedIndex = 0;
			this.MainTabControl.Size = new System.Drawing.Size(484, 533);
			this.MainTabControl.TabIndex = 0;
			// 
			// GeneralTabPage
			// 
			this.GeneralTabPage.Controls.Add(this.GeneralTabPagePanel);
			this.GeneralTabPage.Controls.Add(this.GeneralTabPageHelp);
			this.GeneralTabPage.Location = new System.Drawing.Point(4, 22);
			this.GeneralTabPage.Name = "GeneralTabPage";
			this.GeneralTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.GeneralTabPage.Size = new System.Drawing.Size(476, 507);
			this.GeneralTabPage.TabIndex = 0;
			this.GeneralTabPage.Text = "General";
			// 
			// GeneralTabPagePanel
			// 
			this.GeneralTabPagePanel.Controls.Add(this.DataTypeEdit);
			this.GeneralTabPagePanel.Controls.Add(this.AllowMultiplyValuesEdit);
			this.GeneralTabPagePanel.Controls.Add(this.AllowNullValueEdit);
			this.GeneralTabPagePanel.Controls.Add(this.DataTypeLabel);
			this.GeneralTabPagePanel.Controls.Add(this.CaptionLabel);
			this.GeneralTabPagePanel.Controls.Add(this.NameLabel);
			this.GeneralTabPagePanel.Controls.Add(this.CaptionEdit);
			this.GeneralTabPagePanel.Controls.Add(this.NameEdit);
			this.GeneralTabPagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GeneralTabPagePanel.Location = new System.Drawing.Point(3, 43);
			this.GeneralTabPagePanel.Name = "GeneralTabPagePanel";
			this.GeneralTabPagePanel.Size = new System.Drawing.Size(470, 461);
			this.GeneralTabPagePanel.TabIndex = 1;
			// 
			// DataTypeEdit
			// 
			this.DataTypeEdit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.DataTypeEdit.Items.AddRange(new object[] {
            "String",
            "Float",
            "Integer",
            "Boolean",
            "DateTime"});
			this.DataTypeEdit.Location = new System.Drawing.Point(72, 66);
			this.DataTypeEdit.Name = "DataTypeEdit";
			this.DataTypeEdit.Size = new System.Drawing.Size(150, 21);
			this.DataTypeEdit.TabIndex = 5;
			this.MainToolTip.SetToolTip(this.DataTypeEdit, "Parameter data type.");
			// 
			// AllowMultiplyValuesEdit
			// 
			this.AllowMultiplyValuesEdit.AutoSize = true;
			this.AllowMultiplyValuesEdit.Location = new System.Drawing.Point(13, 116);
			this.AllowMultiplyValuesEdit.Name = "AllowMultiplyValuesEdit";
			this.AllowMultiplyValuesEdit.Size = new System.Drawing.Size(123, 17);
			this.AllowMultiplyValuesEdit.TabIndex = 7;
			this.AllowMultiplyValuesEdit.Text = "Allow multiple values";
			this.AllowMultiplyValuesEdit.UseVisualStyleBackColor = true;
			// 
			// AllowNullValueEdit
			// 
			this.AllowNullValueEdit.AutoSize = true;
			this.AllowNullValueEdit.Location = new System.Drawing.Point(13, 93);
			this.AllowNullValueEdit.Name = "AllowNullValueEdit";
			this.AllowNullValueEdit.Size = new System.Drawing.Size(99, 17);
			this.AllowNullValueEdit.TabIndex = 6;
			this.AllowNullValueEdit.Text = "Allow null value";
			this.AllowNullValueEdit.UseVisualStyleBackColor = true;
			// 
			// DataTypeLabel
			// 
			this.DataTypeLabel.AutoSize = true;
			this.DataTypeLabel.Location = new System.Drawing.Point(10, 69);
			this.DataTypeLabel.Name = "DataTypeLabel";
			this.DataTypeLabel.Size = new System.Drawing.Size(56, 13);
			this.DataTypeLabel.TabIndex = 4;
			this.DataTypeLabel.Text = "Data type:";
			this.MainToolTip.SetToolTip(this.DataTypeLabel, "Parameter data type.");
			// 
			// CaptionLabel
			// 
			this.CaptionLabel.AutoSize = true;
			this.CaptionLabel.Location = new System.Drawing.Point(10, 43);
			this.CaptionLabel.Name = "CaptionLabel";
			this.CaptionLabel.Size = new System.Drawing.Size(46, 13);
			this.CaptionLabel.TabIndex = 2;
			this.CaptionLabel.Text = "Caption:";
			this.MainToolTip.SetToolTip(this.CaptionLabel, "Parameter caption.");
			// 
			// NameLabel
			// 
			this.NameLabel.AutoSize = true;
			this.NameLabel.Location = new System.Drawing.Point(10, 17);
			this.NameLabel.Name = "NameLabel";
			this.NameLabel.Size = new System.Drawing.Size(38, 13);
			this.NameLabel.TabIndex = 0;
			this.NameLabel.Text = "Name:";
			this.MainToolTip.SetToolTip(this.NameLabel, "Parameter name.");
			// 
			// CaptionEdit
			// 
			this.CaptionEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.CaptionEdit.Location = new System.Drawing.Point(72, 40);
			this.CaptionEdit.Name = "CaptionEdit";
			this.CaptionEdit.Size = new System.Drawing.Size(380, 20);
			this.CaptionEdit.TabIndex = 3;
			this.MainToolTip.SetToolTip(this.CaptionEdit, "Parameter caption.");
			// 
			// NameEdit
			// 
			this.NameEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.NameEdit.Location = new System.Drawing.Point(72, 14);
			this.NameEdit.Name = "NameEdit";
			this.NameEdit.Size = new System.Drawing.Size(380, 20);
			this.NameEdit.TabIndex = 1;
			this.MainToolTip.SetToolTip(this.NameEdit, "Parameter name.");
			// 
			// GeneralTabPageHelp
			// 
			this.GeneralTabPageHelp.Dock = System.Windows.Forms.DockStyle.Top;
			this.GeneralTabPageHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.GeneralTabPageHelp.Location = new System.Drawing.Point(3, 3);
			this.GeneralTabPageHelp.Name = "GeneralTabPageHelp";
			this.GeneralTabPageHelp.Padding = new System.Windows.Forms.Padding(10);
			this.GeneralTabPageHelp.Size = new System.Drawing.Size(470, 40);
			this.GeneralTabPageHelp.TabIndex = 0;
			this.GeneralTabPageHelp.Text = "Change name, data type, and other options";
			this.GeneralTabPageHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// AvailableValuesTabPage
			// 
			this.AvailableValuesTabPage.Controls.Add(this.AvailableValuesEdit);
			this.AvailableValuesTabPage.Controls.Add(this.label1);
			this.AvailableValuesTabPage.Location = new System.Drawing.Point(4, 22);
			this.AvailableValuesTabPage.Name = "AvailableValuesTabPage";
			this.AvailableValuesTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.AvailableValuesTabPage.Size = new System.Drawing.Size(476, 507);
			this.AvailableValuesTabPage.TabIndex = 1;
			this.AvailableValuesTabPage.Text = "Available Values";
			// 
			// AvailableValuesEdit
			// 
			this.AvailableValuesEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AvailableValuesEdit.Location = new System.Drawing.Point(3, 43);
			this.AvailableValuesEdit.Name = "AvailableValuesEdit";
			this.AvailableValuesEdit.Size = new System.Drawing.Size(470, 461);
			this.AvailableValuesEdit.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(3, 3);
			this.label1.Name = "label1";
			this.label1.Padding = new System.Windows.Forms.Padding(10);
			this.label1.Size = new System.Drawing.Size(470, 40);
			this.label1.TabIndex = 0;
			this.label1.Text = "Choose the available values for this parameter";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// DefaultValuesTabPage
			// 
			this.DefaultValuesTabPage.Controls.Add(this.DefaultValuesEdit);
			this.DefaultValuesTabPage.Controls.Add(this.label2);
			this.DefaultValuesTabPage.Location = new System.Drawing.Point(4, 22);
			this.DefaultValuesTabPage.Name = "DefaultValuesTabPage";
			this.DefaultValuesTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.DefaultValuesTabPage.Size = new System.Drawing.Size(476, 507);
			this.DefaultValuesTabPage.TabIndex = 2;
			this.DefaultValuesTabPage.Text = "Default Values";
			// 
			// DefaultValuesEdit
			// 
			this.DefaultValuesEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DefaultValuesEdit.Location = new System.Drawing.Point(3, 43);
			this.DefaultValuesEdit.Name = "DefaultValuesEdit";
			this.DefaultValuesEdit.ShowLabel = false;
			this.DefaultValuesEdit.Size = new System.Drawing.Size(470, 461);
			this.DefaultValuesEdit.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Top;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label2.Location = new System.Drawing.Point(3, 3);
			this.label2.Name = "label2";
			this.label2.Padding = new System.Windows.Forms.Padding(10);
			this.label2.Size = new System.Drawing.Size(470, 40);
			this.label2.TabIndex = 2;
			this.label2.Text = "Choose the default values for this parameter";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ParameterView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.MainTabControl);
			this.Name = "ParameterView";
			this.Size = new System.Drawing.Size(484, 533);
			this.MainTabControl.ResumeLayout(false);
			this.GeneralTabPage.ResumeLayout(false);
			this.GeneralTabPagePanel.ResumeLayout(false);
			this.GeneralTabPagePanel.PerformLayout();
			this.AvailableValuesTabPage.ResumeLayout(false);
			this.DefaultValuesTabPage.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl MainTabControl;
		private System.Windows.Forms.TabPage GeneralTabPage;
		private System.Windows.Forms.TabPage AvailableValuesTabPage;
		private System.Windows.Forms.TabPage DefaultValuesTabPage;
		private System.Windows.Forms.Label NameLabel;
		private System.Windows.Forms.Label GeneralTabPageHelp;
		private System.Windows.Forms.TextBox NameEdit;
		private System.Windows.Forms.Panel GeneralTabPagePanel;
		private System.Windows.Forms.Label CaptionLabel;
		private System.Windows.Forms.Label DataTypeLabel;
		private System.Windows.Forms.CheckBox AllowMultiplyValuesEdit;
		private System.Windows.Forms.CheckBox AllowNullValueEdit;
		private System.Windows.Forms.ComboBox DataTypeEdit;
		private System.Windows.Forms.TextBox CaptionEdit;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private ParameterValuesView AvailableValuesEdit;
		private ParameterValuesView DefaultValuesEdit;
		private System.Windows.Forms.ToolTip MainToolTip;
	}
}