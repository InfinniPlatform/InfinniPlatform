namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
	partial class SqlDataSourceView
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SqlDataSourceView));
			this.MainTabControl = new System.Windows.Forms.TabControl();
			this.GeneralTabPage = new System.Windows.Forms.TabPage();
			this.GeneralTabPagePanel = new System.Windows.Forms.Panel();
			this.CommandTimeoutEdit = new System.Windows.Forms.NumericUpDown();
			this.SelectCommandLabel = new System.Windows.Forms.Label();
			this.CommandTimeoutUnitLabel = new System.Windows.Forms.Label();
			this.CommandTimeoutLabel = new System.Windows.Forms.Label();
			this.ConnectionStringLabel = new System.Windows.Forms.Label();
			this.NameLabel = new System.Windows.Forms.Label();
			this.SelectCommandEdit = new System.Windows.Forms.TextBox();
			this.ConnectionStringEdit = new System.Windows.Forms.TextBox();
			this.NameEdit = new System.Windows.Forms.TextBox();
			this.GeneralTabPageHelp = new System.Windows.Forms.Label();
			this.DataSchemaTabPage = new System.Windows.Forms.TabPage();
			this.DataSchemaEdit = new InfinniPlatform.ReportDesigner.Views.DataSources.DataSourceSchemaView();
			this.DataSchemaTabPageHelp = new System.Windows.Forms.Label();
			this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.MainTabControl.SuspendLayout();
			this.GeneralTabPage.SuspendLayout();
			this.GeneralTabPagePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.CommandTimeoutEdit)).BeginInit();
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
			this.MainTabControl.Size = new System.Drawing.Size(484, 455);
			this.MainTabControl.TabIndex = 0;
			// 
			// GeneralTabPage
			// 
			this.GeneralTabPage.Controls.Add(this.GeneralTabPagePanel);
			this.GeneralTabPage.Controls.Add(this.GeneralTabPageHelp);
			this.GeneralTabPage.Location = new System.Drawing.Point(4, 22);
			this.GeneralTabPage.Name = "GeneralTabPage";
			this.GeneralTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.GeneralTabPage.Size = new System.Drawing.Size(476, 429);
			this.GeneralTabPage.TabIndex = 0;
			this.GeneralTabPage.Text = "General";
			// 
			// GeneralTabPagePanel
			// 
			this.GeneralTabPagePanel.Controls.Add(this.CommandTimeoutEdit);
			this.GeneralTabPagePanel.Controls.Add(this.SelectCommandLabel);
			this.GeneralTabPagePanel.Controls.Add(this.CommandTimeoutUnitLabel);
			this.GeneralTabPagePanel.Controls.Add(this.CommandTimeoutLabel);
			this.GeneralTabPagePanel.Controls.Add(this.ConnectionStringLabel);
			this.GeneralTabPagePanel.Controls.Add(this.NameLabel);
			this.GeneralTabPagePanel.Controls.Add(this.SelectCommandEdit);
			this.GeneralTabPagePanel.Controls.Add(this.ConnectionStringEdit);
			this.GeneralTabPagePanel.Controls.Add(this.NameEdit);
			this.GeneralTabPagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GeneralTabPagePanel.Location = new System.Drawing.Point(3, 43);
			this.GeneralTabPagePanel.Name = "GeneralTabPagePanel";
			this.GeneralTabPagePanel.Size = new System.Drawing.Size(470, 383);
			this.GeneralTabPagePanel.TabIndex = 1;
			// 
			// CommandTimeoutEdit
			// 
			this.CommandTimeoutEdit.Location = new System.Drawing.Point(110, 66);
			this.CommandTimeoutEdit.Maximum = new decimal(new int[] {
            18000,
            0,
            0,
            0});
			this.CommandTimeoutEdit.Name = "CommandTimeoutEdit";
			this.CommandTimeoutEdit.Size = new System.Drawing.Size(120, 20);
			this.CommandTimeoutEdit.TabIndex = 5;
			this.CommandTimeoutEdit.ThousandsSeparator = true;
			this.MainToolTip.SetToolTip(this.CommandTimeoutEdit, "The wait time before terminating the attempt to execute a command and generating " +
        "an error.");
			// 
			// SelectCommandLabel
			// 
			this.SelectCommandLabel.AutoSize = true;
			this.SelectCommandLabel.Location = new System.Drawing.Point(10, 95);
			this.SelectCommandLabel.Name = "SelectCommandLabel";
			this.SelectCommandLabel.Size = new System.Drawing.Size(86, 13);
			this.SelectCommandLabel.TabIndex = 7;
			this.SelectCommandLabel.Text = "Command query:";
			this.MainToolTip.SetToolTip(this.SelectCommandLabel, "SQL statement to execute at the data source.");
			// 
			// CommandTimeoutUnitLabel
			// 
			this.CommandTimeoutUnitLabel.AutoSize = true;
			this.CommandTimeoutUnitLabel.Location = new System.Drawing.Point(234, 68);
			this.CommandTimeoutUnitLabel.Name = "CommandTimeoutUnitLabel";
			this.CommandTimeoutUnitLabel.Size = new System.Drawing.Size(24, 13);
			this.CommandTimeoutUnitLabel.TabIndex = 6;
			this.CommandTimeoutUnitLabel.Text = "sec";
			this.MainToolTip.SetToolTip(this.CommandTimeoutUnitLabel, "The wait time before terminating the attempt to execute a command and generating " +
        "an error.");
			// 
			// CommandTimeoutLabel
			// 
			this.CommandTimeoutLabel.AutoSize = true;
			this.CommandTimeoutLabel.Location = new System.Drawing.Point(10, 68);
			this.CommandTimeoutLabel.Name = "CommandTimeoutLabel";
			this.CommandTimeoutLabel.Size = new System.Drawing.Size(94, 13);
			this.CommandTimeoutLabel.TabIndex = 4;
			this.CommandTimeoutLabel.Text = "Command timeout:";
			this.MainToolTip.SetToolTip(this.CommandTimeoutLabel, "The wait time before terminating the attempt to execute a command and generating " +
        "an error.");
			// 
			// ConnectionStringLabel
			// 
			this.ConnectionStringLabel.AutoSize = true;
			this.ConnectionStringLabel.Location = new System.Drawing.Point(10, 43);
			this.ConnectionStringLabel.Name = "ConnectionStringLabel";
			this.ConnectionStringLabel.Size = new System.Drawing.Size(92, 13);
			this.ConnectionStringLabel.TabIndex = 2;
			this.ConnectionStringLabel.Text = "Connection string:";
			this.MainToolTip.SetToolTip(this.ConnectionStringLabel, resources.GetString("ConnectionStringLabel.ToolTip"));
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
			// SelectCommandEdit
			// 
			this.SelectCommandEdit.AcceptsReturn = true;
			this.SelectCommandEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SelectCommandEdit.Location = new System.Drawing.Point(110, 92);
			this.SelectCommandEdit.Multiline = true;
			this.SelectCommandEdit.Name = "SelectCommandEdit";
			this.SelectCommandEdit.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.SelectCommandEdit.Size = new System.Drawing.Size(342, 272);
			this.SelectCommandEdit.TabIndex = 8;
			this.MainToolTip.SetToolTip(this.SelectCommandEdit, "SQL statement to execute at the data source.");
			// 
			// ConnectionStringEdit
			// 
			this.ConnectionStringEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ConnectionStringEdit.Location = new System.Drawing.Point(108, 40);
			this.ConnectionStringEdit.Name = "ConnectionStringEdit";
			this.ConnectionStringEdit.Size = new System.Drawing.Size(344, 20);
			this.ConnectionStringEdit.TabIndex = 3;
			this.MainToolTip.SetToolTip(this.ConnectionStringEdit, resources.GetString("ConnectionStringEdit.ToolTip"));
			// 
			// NameEdit
			// 
			this.NameEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.NameEdit.Location = new System.Drawing.Point(108, 14);
			this.NameEdit.MaxLength = 250;
			this.NameEdit.Name = "NameEdit";
			this.NameEdit.Size = new System.Drawing.Size(344, 20);
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
			this.GeneralTabPageHelp.Text = "Change name and create query";
			this.GeneralTabPageHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// DataSchemaTabPage
			// 
			this.DataSchemaTabPage.Controls.Add(this.DataSchemaEdit);
			this.DataSchemaTabPage.Controls.Add(this.DataSchemaTabPageHelp);
			this.DataSchemaTabPage.Location = new System.Drawing.Point(4, 22);
			this.DataSchemaTabPage.Name = "DataSchemaTabPage";
			this.DataSchemaTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.DataSchemaTabPage.Size = new System.Drawing.Size(476, 429);
			this.DataSchemaTabPage.TabIndex = 1;
			this.DataSchemaTabPage.Text = "Data Schema";
			// 
			// DataSchemaEdit
			// 
			this.DataSchemaEdit.AllowEdit = true;
			this.DataSchemaEdit.DataNestingLevel = 1;
			this.DataSchemaEdit.DataSourceName = null;
			this.DataSchemaEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DataSchemaEdit.Location = new System.Drawing.Point(3, 43);
			this.DataSchemaEdit.Name = "DataSchemaEdit";
			this.DataSchemaEdit.SelectedPropertyPath = null;
			this.DataSchemaEdit.Size = new System.Drawing.Size(470, 383);
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
			// SqlDataSourceView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.MainTabControl);
			this.Name = "SqlDataSourceView";
			this.Size = new System.Drawing.Size(484, 455);
			this.MainTabControl.ResumeLayout(false);
			this.GeneralTabPage.ResumeLayout(false);
			this.GeneralTabPagePanel.ResumeLayout(false);
			this.GeneralTabPagePanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.CommandTimeoutEdit)).EndInit();
			this.DataSchemaTabPage.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl MainTabControl;
		private System.Windows.Forms.TabPage GeneralTabPage;
		private System.Windows.Forms.Panel GeneralTabPagePanel;
		private System.Windows.Forms.NumericUpDown CommandTimeoutEdit;
		private System.Windows.Forms.Label SelectCommandLabel;
		private System.Windows.Forms.Label CommandTimeoutUnitLabel;
		private System.Windows.Forms.Label CommandTimeoutLabel;
		private System.Windows.Forms.Label ConnectionStringLabel;
		private System.Windows.Forms.Label NameLabel;
		private System.Windows.Forms.TextBox SelectCommandEdit;
		private System.Windows.Forms.TextBox ConnectionStringEdit;
		private System.Windows.Forms.TextBox NameEdit;
		private System.Windows.Forms.Label GeneralTabPageHelp;
		private System.Windows.Forms.TabPage DataSchemaTabPage;
		private DataSourceSchemaView DataSchemaEdit;
		private System.Windows.Forms.Label DataSchemaTabPageHelp;
		private System.Windows.Forms.ToolTip MainToolTip;
	}
}
