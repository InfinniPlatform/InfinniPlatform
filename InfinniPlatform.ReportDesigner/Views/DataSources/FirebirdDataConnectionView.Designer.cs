namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
	partial class FirebirdDataConnectionView
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
			this.DatabaseFileLabel = new System.Windows.Forms.Label();
			this.DatabaseFileEdit = new System.Windows.Forms.TextBox();
			this.DatabaseFileOpenDialog = new System.Windows.Forms.OpenFileDialog();
			this.DatabaseFileBtn = new System.Windows.Forms.Button();
			this.UserNameLabel = new System.Windows.Forms.Label();
			this.PasswordLabel = new System.Windows.Forms.Label();
			this.UserNameEdit = new System.Windows.Forms.TextBox();
			this.PasswordEdit = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// DatabaseFileLabel
			// 
			this.DatabaseFileLabel.AutoSize = true;
			this.DatabaseFileLabel.Location = new System.Drawing.Point(10, 17);
			this.DatabaseFileLabel.Name = "DatabaseFileLabel";
			this.DatabaseFileLabel.Size = new System.Drawing.Size(72, 13);
			this.DatabaseFileLabel.TabIndex = 0;
			this.DatabaseFileLabel.Text = "Database file:";
			// 
			// DatabaseFileEdit
			// 
			this.DatabaseFileEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DatabaseFileEdit.Location = new System.Drawing.Point(91, 14);
			this.DatabaseFileEdit.Name = "DatabaseFileEdit";
			this.DatabaseFileEdit.ReadOnly = true;
			this.DatabaseFileEdit.Size = new System.Drawing.Size(200, 20);
			this.DatabaseFileEdit.TabIndex = 1;
			// 
			// DatabaseFileOpenDialog
			// 
			this.DatabaseFileOpenDialog.Filter = "Firebird Database|*.fdb|All Files|*.*";
			this.DatabaseFileOpenDialog.Title = "Open Database File";
			// 
			// DatabaseFileBtn
			// 
			this.DatabaseFileBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.DatabaseFileBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.DatabaseFileBtn.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.FolderOpen;
			this.DatabaseFileBtn.Location = new System.Drawing.Point(297, 14);
			this.DatabaseFileBtn.Name = "DatabaseFileBtn";
			this.DatabaseFileBtn.Size = new System.Drawing.Size(20, 20);
			this.DatabaseFileBtn.TabIndex = 2;
			this.DatabaseFileBtn.UseVisualStyleBackColor = true;
			this.DatabaseFileBtn.Click += new System.EventHandler(this.OnSelectDatabaseFile);
			// 
			// UserNameLabel
			// 
			this.UserNameLabel.AutoSize = true;
			this.UserNameLabel.Location = new System.Drawing.Point(10, 43);
			this.UserNameLabel.Name = "UserNameLabel";
			this.UserNameLabel.Size = new System.Drawing.Size(61, 13);
			this.UserNameLabel.TabIndex = 3;
			this.UserNameLabel.Text = "User name:";
			// 
			// PasswordLabel
			// 
			this.PasswordLabel.AutoSize = true;
			this.PasswordLabel.Location = new System.Drawing.Point(10, 69);
			this.PasswordLabel.Name = "PasswordLabel";
			this.PasswordLabel.Size = new System.Drawing.Size(56, 13);
			this.PasswordLabel.TabIndex = 5;
			this.PasswordLabel.Text = "Password:";
			// 
			// UserNameEdit
			// 
			this.UserNameEdit.Location = new System.Drawing.Point(91, 40);
			this.UserNameEdit.Name = "UserNameEdit";
			this.UserNameEdit.Size = new System.Drawing.Size(150, 20);
			this.UserNameEdit.TabIndex = 4;
			// 
			// PasswordEdit
			// 
			this.PasswordEdit.Location = new System.Drawing.Point(91, 66);
			this.PasswordEdit.Name = "PasswordEdit";
			this.PasswordEdit.PasswordChar = '●';
			this.PasswordEdit.Size = new System.Drawing.Size(150, 20);
			this.PasswordEdit.TabIndex = 6;
			// 
			// FirebirdDataConnectionView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.DatabaseFileBtn);
			this.Controls.Add(this.PasswordEdit);
			this.Controls.Add(this.UserNameEdit);
			this.Controls.Add(this.DatabaseFileEdit);
			this.Controls.Add(this.PasswordLabel);
			this.Controls.Add(this.UserNameLabel);
			this.Controls.Add(this.DatabaseFileLabel);
			this.Name = "FirebirdDataConnectionView";
			this.Size = new System.Drawing.Size(327, 96);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label DatabaseFileLabel;
		private System.Windows.Forms.TextBox DatabaseFileEdit;
		private System.Windows.Forms.OpenFileDialog DatabaseFileOpenDialog;
		private System.Windows.Forms.Button DatabaseFileBtn;
		private System.Windows.Forms.Label UserNameLabel;
		private System.Windows.Forms.Label PasswordLabel;
		private System.Windows.Forms.TextBox UserNameEdit;
		private System.Windows.Forms.TextBox PasswordEdit;
	}
}
