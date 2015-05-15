namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
	partial class RegisterDataSourceView
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
			this.ButtonEditQuery = new System.Windows.Forms.Button();
			this.NameLabel = new System.Windows.Forms.Label();
			this.NameEdit = new System.Windows.Forms.TextBox();
			this.GeneralTabPageHelp = new System.Windows.Forms.Label();
			this.DataSchemaTabPage = new System.Windows.Forms.TabPage();
			this.DataSchemaEdit = new InfinniPlatform.ReportDesigner.Views.DataSources.DataSourceSchemaView();
			this.DataSchemaTabPageHelp = new System.Windows.Forms.Label();
			this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.BodyLabel = new System.Windows.Forms.Label();
			this.BodyEdit = new System.Windows.Forms.TextBox();
			this.MainTabControl.SuspendLayout();
			this.GeneralTabPage.SuspendLayout();
			this.GeneralTabPagePanel.SuspendLayout();
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
			this.MainTabControl.Size = new System.Drawing.Size(484, 524);
			this.MainTabControl.TabIndex = 0;
			// 
			// GeneralTabPage
			// 
			this.GeneralTabPage.Controls.Add(this.GeneralTabPagePanel);
			this.GeneralTabPage.Controls.Add(this.GeneralTabPageHelp);
			this.GeneralTabPage.Location = new System.Drawing.Point(4, 22);
			this.GeneralTabPage.Name = "GeneralTabPage";
			this.GeneralTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.GeneralTabPage.Size = new System.Drawing.Size(476, 498);
			this.GeneralTabPage.TabIndex = 0;
			this.GeneralTabPage.Text = "General";
			// 
			// GeneralTabPagePanel
			// 
			this.GeneralTabPagePanel.Controls.Add(this.ButtonEditQuery);
			this.GeneralTabPagePanel.Controls.Add(this.BodyLabel);
			this.GeneralTabPagePanel.Controls.Add(this.NameLabel);
			this.GeneralTabPagePanel.Controls.Add(this.BodyEdit);
			this.GeneralTabPagePanel.Controls.Add(this.NameEdit);
			this.GeneralTabPagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GeneralTabPagePanel.Location = new System.Drawing.Point(3, 43);
			this.GeneralTabPagePanel.Name = "GeneralTabPagePanel";
			this.GeneralTabPagePanel.Size = new System.Drawing.Size(470, 452);
			this.GeneralTabPagePanel.TabIndex = 1;
			// 
			// ButtonEditQuery
			// 
			this.ButtonEditQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ButtonEditQuery.Location = new System.Drawing.Point(54, 413);
			this.ButtonEditQuery.Name = "ButtonEditQuery";
			this.ButtonEditQuery.Size = new System.Drawing.Size(108, 23);
			this.ButtonEditQuery.TabIndex = 15;
			this.ButtonEditQuery.Text = "Edit Query";
			this.ButtonEditQuery.UseVisualStyleBackColor = true;
			this.ButtonEditQuery.Click += new System.EventHandler(this.ButtonEditQuery_Click);
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
			// NameEdit
			// 
			this.NameEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.NameEdit.Location = new System.Drawing.Point(54, 14);
			this.NameEdit.MaxLength = 250;
			this.NameEdit.Name = "NameEdit";
			this.NameEdit.Size = new System.Drawing.Size(398, 20);
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
			this.GeneralTabPageHelp.Text = "Change name and request body";
			this.GeneralTabPageHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// DataSchemaTabPage
			// 
			this.DataSchemaTabPage.Controls.Add(this.DataSchemaEdit);
			this.DataSchemaTabPage.Controls.Add(this.DataSchemaTabPageHelp);
			this.DataSchemaTabPage.Location = new System.Drawing.Point(4, 22);
			this.DataSchemaTabPage.Name = "DataSchemaTabPage";
			this.DataSchemaTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.DataSchemaTabPage.Size = new System.Drawing.Size(476, 498);
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
			this.DataSchemaEdit.Size = new System.Drawing.Size(470, 452);
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
			// BodyLabel
			// 
			this.BodyLabel.AutoSize = true;
			this.BodyLabel.Location = new System.Drawing.Point(10, 43);
			this.BodyLabel.Name = "BodyLabel";
			this.BodyLabel.Size = new System.Drawing.Size(34, 13);
			this.BodyLabel.TabIndex = 13;
			this.BodyLabel.Text = "Body:";
			this.MainToolTip.SetToolTip(this.BodyLabel, "Request Body");
			// 
			// BodyEdit
			// 
			this.BodyEdit.AcceptsReturn = true;
			this.BodyEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.BodyEdit.Location = new System.Drawing.Point(54, 40);
			this.BodyEdit.Multiline = true;
			this.BodyEdit.Name = "BodyEdit";
			this.BodyEdit.ReadOnly = true;
			this.BodyEdit.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.BodyEdit.Size = new System.Drawing.Size(398, 367);
			this.BodyEdit.TabIndex = 14;
			this.MainToolTip.SetToolTip(this.BodyEdit, "Request Body, ex: JSON, XML, etc (corresponds to Content-Type).");
			// 
			// RegisterDataSourceView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.MainTabControl);
			this.Name = "RegisterDataSourceView";
			this.Size = new System.Drawing.Size(484, 524);
			this.MainTabControl.ResumeLayout(false);
			this.GeneralTabPage.ResumeLayout(false);
			this.GeneralTabPagePanel.ResumeLayout(false);
			this.GeneralTabPagePanel.PerformLayout();
			this.DataSchemaTabPage.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl MainTabControl;
		private System.Windows.Forms.TabPage GeneralTabPage;
		private System.Windows.Forms.TabPage DataSchemaTabPage;
		private System.Windows.Forms.Label GeneralTabPageHelp;
		private System.Windows.Forms.Panel GeneralTabPagePanel;
		private System.Windows.Forms.Label NameLabel;
		private System.Windows.Forms.TextBox NameEdit;
		private System.Windows.Forms.ToolTip MainToolTip;
		private System.Windows.Forms.Label DataSchemaTabPageHelp;
		private DataSourceSchemaView DataSchemaEdit;
		private System.Windows.Forms.Button ButtonEditQuery;
		private System.Windows.Forms.Label BodyLabel;
		private System.Windows.Forms.TextBox BodyEdit;
	}
}