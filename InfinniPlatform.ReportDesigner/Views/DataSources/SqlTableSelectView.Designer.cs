namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
	partial class SqlTableSelectView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SqlTableSelectView));
			this.ConnectionStringEdit = new System.Windows.Forms.TextBox();
			this.SelectDatabaseBtn = new System.Windows.Forms.Button();
			this.TablesEdit = new System.Windows.Forms.TreeView();
			this.TablesImageList = new System.Windows.Forms.ImageList(this.components);
			this.TableNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.MainPanel = new System.Windows.Forms.Panel();
			this.ViewHelp = new System.Windows.Forms.Label();
			this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.MainPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// ConnectionStringEdit
			// 
			this.ConnectionStringEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ConnectionStringEdit.Location = new System.Drawing.Point(10, 17);
			this.ConnectionStringEdit.Name = "ConnectionStringEdit";
			this.ConnectionStringEdit.ReadOnly = true;
			this.ConnectionStringEdit.Size = new System.Drawing.Size(383, 20);
			this.ConnectionStringEdit.TabIndex = 0;
			this.MainToolTip.SetToolTip(this.ConnectionStringEdit, "Database Connection String");
			// 
			// SelectDatabaseBtn
			// 
			this.SelectDatabaseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.SelectDatabaseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.SelectDatabaseBtn.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.DataSourceSync;
			this.SelectDatabaseBtn.Location = new System.Drawing.Point(399, 17);
			this.SelectDatabaseBtn.Name = "SelectDatabaseBtn";
			this.SelectDatabaseBtn.Size = new System.Drawing.Size(20, 20);
			this.SelectDatabaseBtn.TabIndex = 1;
			this.MainToolTip.SetToolTip(this.SelectDatabaseBtn, "Select Database");
			this.SelectDatabaseBtn.UseVisualStyleBackColor = true;
			this.SelectDatabaseBtn.Click += new System.EventHandler(this.OnSelectDatabase);
			// 
			// TablesEdit
			// 
			this.TablesEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TablesEdit.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.TablesEdit.HideSelection = false;
			this.TablesEdit.ImageIndex = 0;
			this.TablesEdit.ImageList = this.TablesImageList;
			this.TablesEdit.Location = new System.Drawing.Point(10, 43);
			this.TablesEdit.Name = "TablesEdit";
			this.TablesEdit.SelectedImageIndex = 0;
			this.TablesEdit.ShowLines = false;
			this.TablesEdit.ShowPlusMinus = false;
			this.TablesEdit.ShowRootLines = false;
			this.TablesEdit.Size = new System.Drawing.Size(409, 293);
			this.TablesEdit.TabIndex = 2;
			this.TablesEdit.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnNodeMouseClick);
			// 
			// TablesImageList
			// 
			this.TablesImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TablesImageList.ImageStream")));
			this.TablesImageList.TransparentColor = System.Drawing.Color.Transparent;
			this.TablesImageList.Images.SetKeyName(0, "Table");
			// 
			// TableNameColumn
			// 
			this.TableNameColumn.Text = "Name";
			this.TableNameColumn.Width = 0;
			// 
			// MainPanel
			// 
			this.MainPanel.Controls.Add(this.TablesEdit);
			this.MainPanel.Controls.Add(this.SelectDatabaseBtn);
			this.MainPanel.Controls.Add(this.ConnectionStringEdit);
			this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainPanel.Location = new System.Drawing.Point(0, 40);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(430, 350);
			this.MainPanel.TabIndex = 1;
			// 
			// ViewHelp
			// 
			this.ViewHelp.Dock = System.Windows.Forms.DockStyle.Top;
			this.ViewHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.ViewHelp.Location = new System.Drawing.Point(0, 0);
			this.ViewHelp.Name = "ViewHelp";
			this.ViewHelp.Padding = new System.Windows.Forms.Padding(10);
			this.ViewHelp.Size = new System.Drawing.Size(430, 40);
			this.ViewHelp.TabIndex = 0;
			this.ViewHelp.Text = "Select Database Table";
			this.ViewHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SqlTableSelectView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.MainPanel);
			this.Controls.Add(this.ViewHelp);
			this.Name = "SqlTableSelectView";
			this.Size = new System.Drawing.Size(430, 390);
			this.MainPanel.ResumeLayout(false);
			this.MainPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox ConnectionStringEdit;
		private System.Windows.Forms.Button SelectDatabaseBtn;
		private System.Windows.Forms.TreeView TablesEdit;
		private System.Windows.Forms.ImageList TablesImageList;
		private System.Windows.Forms.ColumnHeader TableNameColumn;
		private System.Windows.Forms.Panel MainPanel;
		private System.Windows.Forms.Label ViewHelp;
		private System.Windows.Forms.ToolTip MainToolTip;
	}
}
