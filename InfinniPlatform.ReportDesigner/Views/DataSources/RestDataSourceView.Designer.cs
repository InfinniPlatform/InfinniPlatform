namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
	partial class RestDataSourceView
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
			this.RequestTimeoutEdit = new System.Windows.Forms.NumericUpDown();
			this.AcceptTypeEdit = new System.Windows.Forms.ComboBox();
			this.ContentTypeEdit = new System.Windows.Forms.ComboBox();
			this.MethodEdit = new System.Windows.Forms.ComboBox();
			this.AcceptTypeLabel = new System.Windows.Forms.Label();
			this.ContentTypeLabel = new System.Windows.Forms.Label();
			this.BodyLabel = new System.Windows.Forms.Label();
			this.MethodLabel = new System.Windows.Forms.Label();
			this.RequestTimeoutUnitLabel = new System.Windows.Forms.Label();
			this.RequestTimeoutLabel = new System.Windows.Forms.Label();
			this.RequestUriLabel = new System.Windows.Forms.Label();
			this.NameLabel = new System.Windows.Forms.Label();
			this.BodyEdit = new System.Windows.Forms.TextBox();
			this.RequestUriEdit = new System.Windows.Forms.TextBox();
			this.NameEdit = new System.Windows.Forms.TextBox();
			this.GeneralTabPageHelp = new System.Windows.Forms.Label();
			this.DataSchemaTabPage = new System.Windows.Forms.TabPage();
			this.DataSchemaEdit = new InfinniPlatform.ReportDesigner.Views.DataSources.DataSourceSchemaView();
			this.DataSchemaTabPageHelp = new System.Windows.Forms.Label();
			this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.MainTabControl.SuspendLayout();
			this.GeneralTabPage.SuspendLayout();
			this.GeneralTabPagePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.RequestTimeoutEdit)).BeginInit();
			this.DataSchemaTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainTabControl
			// 
			this.MainTabControl.Controls.Add(this.GeneralTabPage);
			this.MainTabControl.Controls.Add(this.DataSchemaTabPage);
			this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainTabControl.Location = new System.Drawing.Point(0, 0);
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
			this.GeneralTabPagePanel.Controls.Add(this.RequestTimeoutEdit);
			this.GeneralTabPagePanel.Controls.Add(this.AcceptTypeEdit);
			this.GeneralTabPagePanel.Controls.Add(this.ContentTypeEdit);
			this.GeneralTabPagePanel.Controls.Add(this.MethodEdit);
			this.GeneralTabPagePanel.Controls.Add(this.AcceptTypeLabel);
			this.GeneralTabPagePanel.Controls.Add(this.ContentTypeLabel);
			this.GeneralTabPagePanel.Controls.Add(this.BodyLabel);
			this.GeneralTabPagePanel.Controls.Add(this.MethodLabel);
			this.GeneralTabPagePanel.Controls.Add(this.RequestTimeoutUnitLabel);
			this.GeneralTabPagePanel.Controls.Add(this.RequestTimeoutLabel);
			this.GeneralTabPagePanel.Controls.Add(this.RequestUriLabel);
			this.GeneralTabPagePanel.Controls.Add(this.NameLabel);
			this.GeneralTabPagePanel.Controls.Add(this.BodyEdit);
			this.GeneralTabPagePanel.Controls.Add(this.RequestUriEdit);
			this.GeneralTabPagePanel.Controls.Add(this.NameEdit);
			this.GeneralTabPagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GeneralTabPagePanel.Location = new System.Drawing.Point(3, 43);
			this.GeneralTabPagePanel.Name = "GeneralTabPagePanel";
			this.GeneralTabPagePanel.Size = new System.Drawing.Size(470, 461);
			this.GeneralTabPagePanel.TabIndex = 1;
			// 
			// RequestTimeoutEdit
			// 
			this.RequestTimeoutEdit.Location = new System.Drawing.Point(90, 147);
			this.RequestTimeoutEdit.Maximum = new decimal(new int[] {
            18000,
            0,
            0,
            0});
			this.RequestTimeoutEdit.Name = "RequestTimeoutEdit";
			this.RequestTimeoutEdit.Size = new System.Drawing.Size(120, 20);
			this.RequestTimeoutEdit.TabIndex = 11;
			this.RequestTimeoutEdit.ThousandsSeparator = true;
			this.MainToolTip.SetToolTip(this.RequestTimeoutEdit, "Timeout in seconds before aborting.");
			// 
			// AcceptTypeEdit
			// 
			this.AcceptTypeEdit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.AcceptTypeEdit.Items.AddRange(new object[] {
            "",
            "application/json",
            "application/xml"});
			this.AcceptTypeEdit.Location = new System.Drawing.Point(90, 120);
			this.AcceptTypeEdit.Name = "AcceptTypeEdit";
			this.AcceptTypeEdit.Size = new System.Drawing.Size(150, 21);
			this.AcceptTypeEdit.TabIndex = 9;
			this.MainToolTip.SetToolTip(this.AcceptTypeEdit, "Content-Type that are acceptable.");
			// 
			// ContentTypeEdit
			// 
			this.ContentTypeEdit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ContentTypeEdit.Items.AddRange(new object[] {
            "",
            "application/json",
            "application/xml"});
			this.ContentTypeEdit.Location = new System.Drawing.Point(90, 93);
			this.ContentTypeEdit.Name = "ContentTypeEdit";
			this.ContentTypeEdit.Size = new System.Drawing.Size(150, 21);
			this.ContentTypeEdit.TabIndex = 7;
			this.MainToolTip.SetToolTip(this.ContentTypeEdit, "The mime type of the body of the request (used with POST and PUT requests).");
			// 
			// MethodEdit
			// 
			this.MethodEdit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.MethodEdit.Items.AddRange(new object[] {
            "GET",
            "POST",
            "PUT",
            "DELETE"});
			this.MethodEdit.Location = new System.Drawing.Point(90, 66);
			this.MethodEdit.Name = "MethodEdit";
			this.MethodEdit.Size = new System.Drawing.Size(150, 21);
			this.MethodEdit.TabIndex = 5;
			this.MainToolTip.SetToolTip(this.MethodEdit, "The desired action to be performed on the identified resource.");
			this.MethodEdit.SelectedIndexChanged += new System.EventHandler(this.OnMethodChanged);
			// 
			// AcceptTypeLabel
			// 
			this.AcceptTypeLabel.AutoSize = true;
			this.AcceptTypeLabel.Location = new System.Drawing.Point(10, 123);
			this.AcceptTypeLabel.Name = "AcceptTypeLabel";
			this.AcceptTypeLabel.Size = new System.Drawing.Size(71, 13);
			this.AcceptTypeLabel.TabIndex = 8;
			this.AcceptTypeLabel.Text = "Accept-Type:";
			this.MainToolTip.SetToolTip(this.AcceptTypeLabel, "Content-Type that are acceptable.");
			// 
			// ContentTypeLabel
			// 
			this.ContentTypeLabel.AutoSize = true;
			this.ContentTypeLabel.Location = new System.Drawing.Point(10, 96);
			this.ContentTypeLabel.Name = "ContentTypeLabel";
			this.ContentTypeLabel.Size = new System.Drawing.Size(74, 13);
			this.ContentTypeLabel.TabIndex = 6;
			this.ContentTypeLabel.Text = "Content-Type:";
			this.MainToolTip.SetToolTip(this.ContentTypeLabel, "The mime type of the body of the request (used with POST and PUT requests).");
			// 
			// BodyLabel
			// 
			this.BodyLabel.AutoSize = true;
			this.BodyLabel.Location = new System.Drawing.Point(10, 176);
			this.BodyLabel.Name = "BodyLabel";
			this.BodyLabel.Size = new System.Drawing.Size(34, 13);
			this.BodyLabel.TabIndex = 13;
			this.BodyLabel.Text = "Body:";
			this.MainToolTip.SetToolTip(this.BodyLabel, "Request Body, ex: JSON, XML, etc (corresponds to Content-Type).");
			// 
			// MethodLabel
			// 
			this.MethodLabel.AutoSize = true;
			this.MethodLabel.Location = new System.Drawing.Point(10, 69);
			this.MethodLabel.Name = "MethodLabel";
			this.MethodLabel.Size = new System.Drawing.Size(46, 13);
			this.MethodLabel.TabIndex = 4;
			this.MethodLabel.Text = "Method:";
			this.MainToolTip.SetToolTip(this.MethodLabel, "The desired action to be performed on the identified resource.");
			// 
			// RequestTimeoutUnitLabel
			// 
			this.RequestTimeoutUnitLabel.AutoSize = true;
			this.RequestTimeoutUnitLabel.Location = new System.Drawing.Point(216, 149);
			this.RequestTimeoutUnitLabel.Name = "RequestTimeoutUnitLabel";
			this.RequestTimeoutUnitLabel.Size = new System.Drawing.Size(24, 13);
			this.RequestTimeoutUnitLabel.TabIndex = 12;
			this.RequestTimeoutUnitLabel.Text = "sec";
			this.MainToolTip.SetToolTip(this.RequestTimeoutUnitLabel, "Timeout in seconds before aborting.");
			// 
			// RequestTimeoutLabel
			// 
			this.RequestTimeoutLabel.AutoSize = true;
			this.RequestTimeoutLabel.Location = new System.Drawing.Point(10, 149);
			this.RequestTimeoutLabel.Name = "RequestTimeoutLabel";
			this.RequestTimeoutLabel.Size = new System.Drawing.Size(48, 13);
			this.RequestTimeoutLabel.TabIndex = 10;
			this.RequestTimeoutLabel.Text = "Timeout:";
			this.MainToolTip.SetToolTip(this.RequestTimeoutLabel, "Timeout in seconds before aborting.");
			// 
			// RequestUriLabel
			// 
			this.RequestUriLabel.AutoSize = true;
			this.RequestUriLabel.Location = new System.Drawing.Point(10, 43);
			this.RequestUriLabel.Name = "RequestUriLabel";
			this.RequestUriLabel.Size = new System.Drawing.Size(29, 13);
			this.RequestUriLabel.TabIndex = 2;
			this.RequestUriLabel.Text = "URI:";
			this.MainToolTip.SetToolTip(this.RequestUriLabel, "Request URI (Universal Resource Udentifier).\r\nEx: \'http://www.report.com:9900\'.\r\n" +
        "");
			// 
			// NameLabel
			// 
			this.NameLabel.AutoSize = true;
			this.NameLabel.Location = new System.Drawing.Point(10, 17);
			this.NameLabel.Name = "NameLabel";
			this.NameLabel.Size = new System.Drawing.Size(38, 13);
			this.NameLabel.TabIndex = 0;
			this.NameLabel.Text = "Name:";
			this.MainToolTip.SetToolTip(this.NameLabel, "Data Source name.");
			// 
			// BodyEdit
			// 
			this.BodyEdit.AcceptsReturn = true;
			this.BodyEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.BodyEdit.Location = new System.Drawing.Point(90, 173);
			this.BodyEdit.Multiline = true;
			this.BodyEdit.Name = "BodyEdit";
			this.BodyEdit.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.BodyEdit.Size = new System.Drawing.Size(362, 272);
			this.BodyEdit.TabIndex = 14;
			this.MainToolTip.SetToolTip(this.BodyEdit, "Request Body, ex: JSON, XML, etc (corresponds to Content-Type).");
			// 
			// RequestUriEdit
			// 
			this.RequestUriEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.RequestUriEdit.Location = new System.Drawing.Point(90, 40);
			this.RequestUriEdit.Name = "RequestUriEdit";
			this.RequestUriEdit.Size = new System.Drawing.Size(362, 20);
			this.RequestUriEdit.TabIndex = 3;
			this.MainToolTip.SetToolTip(this.RequestUriEdit, "Request URI (Universal Resource Udentifier).\r\nEx: \'http://www.report.com:9900\'.");
			// 
			// NameEdit
			// 
			this.NameEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.NameEdit.Location = new System.Drawing.Point(90, 14);
			this.NameEdit.MaxLength = 250;
			this.NameEdit.Name = "NameEdit";
			this.NameEdit.Size = new System.Drawing.Size(362, 20);
			this.NameEdit.TabIndex = 1;
			this.MainToolTip.SetToolTip(this.NameEdit, "Data Source name.");
			this.NameEdit.TextChanged += new System.EventHandler(this.OnNameChanged);
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
			this.GeneralTabPageHelp.Text = "Change name, URL, and connection options";
			this.GeneralTabPageHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// DataSchemaTabPage
			// 
			this.DataSchemaTabPage.Controls.Add(this.DataSchemaEdit);
			this.DataSchemaTabPage.Controls.Add(this.DataSchemaTabPageHelp);
			this.DataSchemaTabPage.Location = new System.Drawing.Point(4, 22);
			this.DataSchemaTabPage.Name = "DataSchemaTabPage";
			this.DataSchemaTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.DataSchemaTabPage.Size = new System.Drawing.Size(476, 507);
			this.DataSchemaTabPage.TabIndex = 1;
			this.DataSchemaTabPage.Text = "Data Schema";
			// 
			// DataSchemaEdit
			// 
			this.DataSchemaEdit.AllowEdit = true;
			this.DataSchemaEdit.DataSourceName = null;
			this.DataSchemaEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DataSchemaEdit.Location = new System.Drawing.Point(3, 43);
			this.DataSchemaEdit.Name = "DataSchemaEdit";
			this.DataSchemaEdit.SelectedPropertyPath = null;
			this.DataSchemaEdit.Size = new System.Drawing.Size(470, 461);
			this.DataSchemaEdit.TabIndex = 1;
			this.DataSchemaEdit.OnImportDataSchema += new System.EventHandler(this.OnImportDataSchema);
			// 
			// DataSchemaTabPageHelp
			// 
			this.DataSchemaTabPageHelp.Dock = System.Windows.Forms.DockStyle.Top;
			this.DataSchemaTabPageHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.DataSchemaTabPageHelp.Location = new System.Drawing.Point(3, 3);
			this.DataSchemaTabPageHelp.Name = "DataSchemaTabPageHelp";
			this.DataSchemaTabPageHelp.Padding = new System.Windows.Forms.Padding(10);
			this.DataSchemaTabPageHelp.Size = new System.Drawing.Size(470, 40);
			this.DataSchemaTabPageHelp.TabIndex = 0;
			this.DataSchemaTabPageHelp.Text = "Specify data source schema";
			this.DataSchemaTabPageHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// RestDataSourceView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.MainTabControl);
			this.Name = "RestDataSourceView";
			this.Size = new System.Drawing.Size(484, 533);
			this.MainTabControl.ResumeLayout(false);
			this.GeneralTabPage.ResumeLayout(false);
			this.GeneralTabPagePanel.ResumeLayout(false);
			this.GeneralTabPagePanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.RequestTimeoutEdit)).EndInit();
			this.DataSchemaTabPage.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl MainTabControl;
		private System.Windows.Forms.TabPage GeneralTabPage;
		private System.Windows.Forms.TabPage DataSchemaTabPage;
		private System.Windows.Forms.Label GeneralTabPageHelp;
		private System.Windows.Forms.Panel GeneralTabPagePanel;
		private System.Windows.Forms.ComboBox MethodEdit;
		private System.Windows.Forms.Label MethodLabel;
		private System.Windows.Forms.Label RequestUriLabel;
		private System.Windows.Forms.Label NameLabel;
		private System.Windows.Forms.TextBox RequestUriEdit;
		private System.Windows.Forms.TextBox NameEdit;
		private System.Windows.Forms.Label RequestTimeoutLabel;
		private System.Windows.Forms.NumericUpDown RequestTimeoutEdit;
		private System.Windows.Forms.Label ContentTypeLabel;
		private System.Windows.Forms.Label RequestTimeoutUnitLabel;
		private System.Windows.Forms.ComboBox AcceptTypeEdit;
		private System.Windows.Forms.ComboBox ContentTypeEdit;
		private System.Windows.Forms.Label AcceptTypeLabel;
		private System.Windows.Forms.Label BodyLabel;
		private System.Windows.Forms.TextBox BodyEdit;
		private System.Windows.Forms.ToolTip MainToolTip;
		private System.Windows.Forms.Label DataSchemaTabPageHelp;
		private DataSourceSchemaView DataSchemaEdit;
	}
}