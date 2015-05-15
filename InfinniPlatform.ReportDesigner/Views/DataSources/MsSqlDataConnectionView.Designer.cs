namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
	partial class MsSqlDataConnectionView
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
			this.ServerNameLabel = new System.Windows.Forms.Label();
			this.LogOnGroupBox = new System.Windows.Forms.GroupBox();
			this.PasswordEdit = new System.Windows.Forms.TextBox();
			this.PasswordLabel = new System.Windows.Forms.Label();
			this.UserNameLabel = new System.Windows.Forms.Label();
			this.UseSqlServerAuthenticationBtn = new System.Windows.Forms.RadioButton();
			this.UserNameEdit = new System.Windows.Forms.TextBox();
			this.UseWindowsAuthenticationBtn = new System.Windows.Forms.RadioButton();
			this.ServerNameEdit = new System.Windows.Forms.ComboBox();
			this.DatabaseGroupBox = new System.Windows.Forms.GroupBox();
			this.DatabaseFileBtn = new System.Windows.Forms.Button();
			this.DatabaseFileEdit = new System.Windows.Forms.TextBox();
			this.DatabaseNameEdit = new System.Windows.Forms.ComboBox();
			this.DatabaseNameBtn = new System.Windows.Forms.RadioButton();
			this.DatabaseLogicalNameEdit = new System.Windows.Forms.TextBox();
			this.AttachDatabaseFileBtn = new System.Windows.Forms.RadioButton();
			this.RefreshServerNamesBtn = new System.Windows.Forms.Button();
			this.DatabaseFileOpenDialog = new System.Windows.Forms.OpenFileDialog();
			this.LogOnGroupBox.SuspendLayout();
			this.DatabaseGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// ServerNameLabel
			// 
			this.ServerNameLabel.AutoSize = true;
			this.ServerNameLabel.Location = new System.Drawing.Point(10, 17);
			this.ServerNameLabel.Name = "ServerNameLabel";
			this.ServerNameLabel.Size = new System.Drawing.Size(70, 13);
			this.ServerNameLabel.TabIndex = 0;
			this.ServerNameLabel.Text = "Server name:";
			// 
			// LogOnGroupBox
			// 
			this.LogOnGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LogOnGroupBox.Controls.Add(this.PasswordEdit);
			this.LogOnGroupBox.Controls.Add(this.PasswordLabel);
			this.LogOnGroupBox.Controls.Add(this.UserNameLabel);
			this.LogOnGroupBox.Controls.Add(this.UseSqlServerAuthenticationBtn);
			this.LogOnGroupBox.Controls.Add(this.UserNameEdit);
			this.LogOnGroupBox.Controls.Add(this.UseWindowsAuthenticationBtn);
			this.LogOnGroupBox.Location = new System.Drawing.Point(13, 40);
			this.LogOnGroupBox.Name = "LogOnGroupBox";
			this.LogOnGroupBox.Size = new System.Drawing.Size(373, 128);
			this.LogOnGroupBox.TabIndex = 3;
			this.LogOnGroupBox.TabStop = false;
			this.LogOnGroupBox.Text = "Log on to the server";
			// 
			// PasswordEdit
			// 
			this.PasswordEdit.Enabled = false;
			this.PasswordEdit.Location = new System.Drawing.Point(93, 97);
			this.PasswordEdit.Name = "PasswordEdit";
			this.PasswordEdit.PasswordChar = '●';
			this.PasswordEdit.Size = new System.Drawing.Size(150, 20);
			this.PasswordEdit.TabIndex = 5;
			// 
			// PasswordLabel
			// 
			this.PasswordLabel.AutoSize = true;
			this.PasswordLabel.Enabled = false;
			this.PasswordLabel.Location = new System.Drawing.Point(26, 100);
			this.PasswordLabel.Name = "PasswordLabel";
			this.PasswordLabel.Size = new System.Drawing.Size(56, 13);
			this.PasswordLabel.TabIndex = 4;
			this.PasswordLabel.Text = "Password:";
			// 
			// UserNameLabel
			// 
			this.UserNameLabel.AutoSize = true;
			this.UserNameLabel.Enabled = false;
			this.UserNameLabel.Location = new System.Drawing.Point(26, 74);
			this.UserNameLabel.Name = "UserNameLabel";
			this.UserNameLabel.Size = new System.Drawing.Size(61, 13);
			this.UserNameLabel.TabIndex = 2;
			this.UserNameLabel.Text = "User name:";
			// 
			// UseSqlServerAuthenticationBtn
			// 
			this.UseSqlServerAuthenticationBtn.AutoSize = true;
			this.UseSqlServerAuthenticationBtn.Location = new System.Drawing.Point(10, 48);
			this.UseSqlServerAuthenticationBtn.Name = "UseSqlServerAuthenticationBtn";
			this.UseSqlServerAuthenticationBtn.Size = new System.Drawing.Size(173, 17);
			this.UseSqlServerAuthenticationBtn.TabIndex = 1;
			this.UseSqlServerAuthenticationBtn.Text = "Use SQL Server Authentication";
			this.UseSqlServerAuthenticationBtn.UseVisualStyleBackColor = true;
			this.UseSqlServerAuthenticationBtn.CheckedChanged += new System.EventHandler(this.OnSelectLogOnMode);
			// 
			// UserNameEdit
			// 
			this.UserNameEdit.Enabled = false;
			this.UserNameEdit.Location = new System.Drawing.Point(93, 71);
			this.UserNameEdit.Name = "UserNameEdit";
			this.UserNameEdit.Size = new System.Drawing.Size(150, 20);
			this.UserNameEdit.TabIndex = 3;
			// 
			// UseWindowsAuthenticationBtn
			// 
			this.UseWindowsAuthenticationBtn.AutoSize = true;
			this.UseWindowsAuthenticationBtn.Checked = true;
			this.UseWindowsAuthenticationBtn.Location = new System.Drawing.Point(10, 25);
			this.UseWindowsAuthenticationBtn.Name = "UseWindowsAuthenticationBtn";
			this.UseWindowsAuthenticationBtn.Size = new System.Drawing.Size(162, 17);
			this.UseWindowsAuthenticationBtn.TabIndex = 0;
			this.UseWindowsAuthenticationBtn.TabStop = true;
			this.UseWindowsAuthenticationBtn.Text = "Use Windows Authentication";
			this.UseWindowsAuthenticationBtn.UseVisualStyleBackColor = true;
			this.UseWindowsAuthenticationBtn.CheckedChanged += new System.EventHandler(this.OnSelectLogOnMode);
			// 
			// ServerNameEdit
			// 
			this.ServerNameEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ServerNameEdit.FormattingEnabled = true;
			this.ServerNameEdit.Location = new System.Drawing.Point(86, 14);
			this.ServerNameEdit.Name = "ServerNameEdit";
			this.ServerNameEdit.Size = new System.Drawing.Size(274, 21);
			this.ServerNameEdit.TabIndex = 1;
			this.ServerNameEdit.Text = "localhost";
			this.ServerNameEdit.DropDown += new System.EventHandler(this.OnDropDownServerNames);
			// 
			// DatabaseGroupBox
			// 
			this.DatabaseGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DatabaseGroupBox.Controls.Add(this.DatabaseFileBtn);
			this.DatabaseGroupBox.Controls.Add(this.DatabaseFileEdit);
			this.DatabaseGroupBox.Controls.Add(this.DatabaseNameEdit);
			this.DatabaseGroupBox.Controls.Add(this.DatabaseNameBtn);
			this.DatabaseGroupBox.Controls.Add(this.DatabaseLogicalNameEdit);
			this.DatabaseGroupBox.Controls.Add(this.AttachDatabaseFileBtn);
			this.DatabaseGroupBox.Location = new System.Drawing.Point(13, 174);
			this.DatabaseGroupBox.Name = "DatabaseGroupBox";
			this.DatabaseGroupBox.Size = new System.Drawing.Size(373, 155);
			this.DatabaseGroupBox.TabIndex = 4;
			this.DatabaseGroupBox.TabStop = false;
			this.DatabaseGroupBox.Text = "Connect to a database";
			// 
			// DatabaseFileBtn
			// 
			this.DatabaseFileBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.DatabaseFileBtn.Enabled = false;
			this.DatabaseFileBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.DatabaseFileBtn.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.FolderOpen;
			this.DatabaseFileBtn.Location = new System.Drawing.Point(339, 98);
			this.DatabaseFileBtn.Name = "DatabaseFileBtn";
			this.DatabaseFileBtn.Size = new System.Drawing.Size(20, 20);
			this.DatabaseFileBtn.TabIndex = 4;
			this.DatabaseFileBtn.UseVisualStyleBackColor = true;
			this.DatabaseFileBtn.Click += new System.EventHandler(this.OnSelectDatabaseFile);
			// 
			// DatabaseFileEdit
			// 
			this.DatabaseFileEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DatabaseFileEdit.Enabled = false;
			this.DatabaseFileEdit.Location = new System.Drawing.Point(29, 98);
			this.DatabaseFileEdit.Name = "DatabaseFileEdit";
			this.DatabaseFileEdit.ReadOnly = true;
			this.DatabaseFileEdit.Size = new System.Drawing.Size(304, 20);
			this.DatabaseFileEdit.TabIndex = 3;
			// 
			// DatabaseNameEdit
			// 
			this.DatabaseNameEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DatabaseNameEdit.FormattingEnabled = true;
			this.DatabaseNameEdit.Location = new System.Drawing.Point(29, 48);
			this.DatabaseNameEdit.Name = "DatabaseNameEdit";
			this.DatabaseNameEdit.Size = new System.Drawing.Size(330, 21);
			this.DatabaseNameEdit.TabIndex = 1;
			this.DatabaseNameEdit.DropDown += new System.EventHandler(this.OnDropDownDatabaseNames);
			// 
			// DatabaseNameBtn
			// 
			this.DatabaseNameBtn.AutoSize = true;
			this.DatabaseNameBtn.Checked = true;
			this.DatabaseNameBtn.Location = new System.Drawing.Point(10, 25);
			this.DatabaseNameBtn.Name = "DatabaseNameBtn";
			this.DatabaseNameBtn.Size = new System.Drawing.Size(182, 17);
			this.DatabaseNameBtn.TabIndex = 0;
			this.DatabaseNameBtn.TabStop = true;
			this.DatabaseNameBtn.Text = "Select or enter a database name:";
			this.DatabaseNameBtn.UseVisualStyleBackColor = true;
			this.DatabaseNameBtn.CheckedChanged += new System.EventHandler(this.OnSelectDatabase);
			// 
			// DatabaseLogicalNameEdit
			// 
			this.DatabaseLogicalNameEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DatabaseLogicalNameEdit.Enabled = false;
			this.DatabaseLogicalNameEdit.Location = new System.Drawing.Point(29, 124);
			this.DatabaseLogicalNameEdit.Name = "DatabaseLogicalNameEdit";
			this.DatabaseLogicalNameEdit.Size = new System.Drawing.Size(330, 20);
			this.DatabaseLogicalNameEdit.TabIndex = 5;
			// 
			// AttachDatabaseFileBtn
			// 
			this.AttachDatabaseFileBtn.AutoSize = true;
			this.AttachDatabaseFileBtn.Location = new System.Drawing.Point(10, 75);
			this.AttachDatabaseFileBtn.Name = "AttachDatabaseFileBtn";
			this.AttachDatabaseFileBtn.Size = new System.Drawing.Size(131, 17);
			this.AttachDatabaseFileBtn.TabIndex = 2;
			this.AttachDatabaseFileBtn.Text = "Attach a database file:";
			this.AttachDatabaseFileBtn.UseVisualStyleBackColor = true;
			this.AttachDatabaseFileBtn.CheckedChanged += new System.EventHandler(this.OnSelectDatabase);
			// 
			// RefreshServerNamesBtn
			// 
			this.RefreshServerNamesBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.RefreshServerNamesBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.RefreshServerNamesBtn.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Refresh;
			this.RefreshServerNamesBtn.Location = new System.Drawing.Point(366, 15);
			this.RefreshServerNamesBtn.Name = "RefreshServerNamesBtn";
			this.RefreshServerNamesBtn.Size = new System.Drawing.Size(20, 20);
			this.RefreshServerNamesBtn.TabIndex = 2;
			this.RefreshServerNamesBtn.UseVisualStyleBackColor = true;
			this.RefreshServerNamesBtn.Click += new System.EventHandler(this.OnRefreshServerNames);
			// 
			// DatabaseFileOpenDialog
			// 
			this.DatabaseFileOpenDialog.Filter = "Microsoft SQL Server Database|*.mdf|All Files|*.*";
			this.DatabaseFileOpenDialog.Title = "Open Database File";
			// 
			// MsSqlDataConnectionView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.RefreshServerNamesBtn);
			this.Controls.Add(this.DatabaseGroupBox);
			this.Controls.Add(this.ServerNameEdit);
			this.Controls.Add(this.LogOnGroupBox);
			this.Controls.Add(this.ServerNameLabel);
			this.Name = "MsSqlDataConnectionView";
			this.Size = new System.Drawing.Size(399, 342);
			this.LogOnGroupBox.ResumeLayout(false);
			this.LogOnGroupBox.PerformLayout();
			this.DatabaseGroupBox.ResumeLayout(false);
			this.DatabaseGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label ServerNameLabel;
		private System.Windows.Forms.GroupBox LogOnGroupBox;
		private System.Windows.Forms.RadioButton UseSqlServerAuthenticationBtn;
		private System.Windows.Forms.RadioButton UseWindowsAuthenticationBtn;
		private System.Windows.Forms.ComboBox ServerNameEdit;
		private System.Windows.Forms.TextBox PasswordEdit;
		private System.Windows.Forms.Label PasswordLabel;
		private System.Windows.Forms.Label UserNameLabel;
		private System.Windows.Forms.TextBox UserNameEdit;
		private System.Windows.Forms.GroupBox DatabaseGroupBox;
		private System.Windows.Forms.ComboBox DatabaseNameEdit;
		private System.Windows.Forms.RadioButton DatabaseNameBtn;
		private System.Windows.Forms.RadioButton AttachDatabaseFileBtn;
		private System.Windows.Forms.Button DatabaseFileBtn;
		private System.Windows.Forms.TextBox DatabaseFileEdit;
		private System.Windows.Forms.TextBox DatabaseLogicalNameEdit;
		private System.Windows.Forms.Button RefreshServerNamesBtn;
		private System.Windows.Forms.OpenFileDialog DatabaseFileOpenDialog;
	}
}
