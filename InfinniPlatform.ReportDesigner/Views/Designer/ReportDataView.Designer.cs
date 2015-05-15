namespace InfinniPlatform.ReportDesigner.Views.Designer
{
	partial class ReportDataView
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
			System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Data Sources");
			System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Parameters");
			System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Totals");
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportDataView));
			this.DataSourceMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.CreateDataSourceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.EditDataSourceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.DeleteDataSourceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ParameterMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.CreateParameterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.EditParameterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.DeleteParameterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.TotalMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.CreateTotalMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.EditTotalMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.DeleteTotalMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.DataTree = new System.Windows.Forms.TreeView();
			this.DataTreeImageList = new System.Windows.Forms.ImageList(this.components);
			this.ControlPanel = new System.Windows.Forms.ToolStrip();
			this.ActionsButton = new System.Windows.Forms.ToolStripSplitButton();
			this.CreateDataSourceButton = new System.Windows.Forms.ToolStripMenuItem();
			this.CreateParameterButton = new System.Windows.Forms.ToolStripMenuItem();
			this.CreateTotalButton = new System.Windows.Forms.ToolStripMenuItem();
			this.EditButton = new System.Windows.Forms.ToolStripButton();
			this.DeleteButton = new System.Windows.Forms.ToolStripButton();
			this.Separator1 = new System.Windows.Forms.ToolStripSeparator();
			this.MoveUpButton = new System.Windows.Forms.ToolStripButton();
			this.MoveDownButton = new System.Windows.Forms.ToolStripButton();
			this.DataSourceMenu.SuspendLayout();
			this.ParameterMenu.SuspendLayout();
			this.TotalMenu.SuspendLayout();
			this.ControlPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// DataSourceMenu
			// 
			this.DataSourceMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateDataSourceMenuItem,
            this.EditDataSourceMenuItem,
            this.DeleteDataSourceMenuItem});
			this.DataSourceMenu.Name = "DataSourceMenu";
			this.DataSourceMenu.Size = new System.Drawing.Size(174, 70);
			this.DataSourceMenu.Opening += new System.ComponentModel.CancelEventHandler(this.OnMenuOpening);
			// 
			// CreateDataSourceMenuItem
			// 
			this.CreateDataSourceMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.DataSourceNew;
			this.CreateDataSourceMenuItem.Name = "CreateDataSourceMenuItem";
			this.CreateDataSourceMenuItem.Size = new System.Drawing.Size(173, 22);
			this.CreateDataSourceMenuItem.Text = "New Data Source...";
			this.CreateDataSourceMenuItem.Click += new System.EventHandler(this.OnCreateDataSource);
			// 
			// EditDataSourceMenuItem
			// 
			this.EditDataSourceMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Edit;
			this.EditDataSourceMenuItem.Name = "EditDataSourceMenuItem";
			this.EditDataSourceMenuItem.Size = new System.Drawing.Size(173, 22);
			this.EditDataSourceMenuItem.Text = "Edit Data Source...";
			this.EditDataSourceMenuItem.Click += new System.EventHandler(this.OnEditDataSource);
			// 
			// DeleteDataSourceMenuItem
			// 
			this.DeleteDataSourceMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Delete;
			this.DeleteDataSourceMenuItem.Name = "DeleteDataSourceMenuItem";
			this.DeleteDataSourceMenuItem.Size = new System.Drawing.Size(173, 22);
			this.DeleteDataSourceMenuItem.Text = "Delete Data Source";
			this.DeleteDataSourceMenuItem.Click += new System.EventHandler(this.OnDeleteDataSource);
			// 
			// ParameterMenu
			// 
			this.ParameterMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateParameterMenuItem,
            this.EditParameterMenuItem,
            this.DeleteParameterMenuItem});
			this.ParameterMenu.Name = "ParameterMenu";
			this.ParameterMenu.Size = new System.Drawing.Size(165, 70);
			this.ParameterMenu.Opening += new System.ComponentModel.CancelEventHandler(this.OnMenuOpening);
			// 
			// CreateParameterMenuItem
			// 
			this.CreateParameterMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ParameterNew;
			this.CreateParameterMenuItem.Name = "CreateParameterMenuItem";
			this.CreateParameterMenuItem.Size = new System.Drawing.Size(164, 22);
			this.CreateParameterMenuItem.Text = "New Parameter...";
			this.CreateParameterMenuItem.Click += new System.EventHandler(this.OnCreateParameter);
			// 
			// EditParameterMenuItem
			// 
			this.EditParameterMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Edit;
			this.EditParameterMenuItem.Name = "EditParameterMenuItem";
			this.EditParameterMenuItem.Size = new System.Drawing.Size(164, 22);
			this.EditParameterMenuItem.Text = "Edit Parameter...";
			this.EditParameterMenuItem.Click += new System.EventHandler(this.OnEditParameter);
			// 
			// DeleteParameterMenuItem
			// 
			this.DeleteParameterMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Delete;
			this.DeleteParameterMenuItem.Name = "DeleteParameterMenuItem";
			this.DeleteParameterMenuItem.Size = new System.Drawing.Size(164, 22);
			this.DeleteParameterMenuItem.Text = "Delete Parameter";
			this.DeleteParameterMenuItem.Click += new System.EventHandler(this.OnDeleteParameter);
			// 
			// TotalMenu
			// 
			this.TotalMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateTotalMenuItem,
            this.EditTotalMenuItem,
            this.DeleteTotalMenuItem});
			this.TotalMenu.Name = "TotalMenu";
			this.TotalMenu.Size = new System.Drawing.Size(138, 70);
			this.TotalMenu.Opening += new System.ComponentModel.CancelEventHandler(this.OnMenuOpening);
			// 
			// CreateTotalMenuItem
			// 
			this.CreateTotalMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.TotalNew;
			this.CreateTotalMenuItem.Name = "CreateTotalMenuItem";
			this.CreateTotalMenuItem.Size = new System.Drawing.Size(137, 22);
			this.CreateTotalMenuItem.Text = "New Total...";
			this.CreateTotalMenuItem.Click += new System.EventHandler(this.OnCreateTotal);
			// 
			// EditTotalMenuItem
			// 
			this.EditTotalMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Edit;
			this.EditTotalMenuItem.Name = "EditTotalMenuItem";
			this.EditTotalMenuItem.Size = new System.Drawing.Size(137, 22);
			this.EditTotalMenuItem.Text = "Edit Total...";
			this.EditTotalMenuItem.Click += new System.EventHandler(this.OnEditTotal);
			// 
			// DeleteTotalMenuItem
			// 
			this.DeleteTotalMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Delete;
			this.DeleteTotalMenuItem.Name = "DeleteTotalMenuItem";
			this.DeleteTotalMenuItem.Size = new System.Drawing.Size(137, 22);
			this.DeleteTotalMenuItem.Text = "Delete Total";
			this.DeleteTotalMenuItem.Click += new System.EventHandler(this.OnDeleteTotal);
			// 
			// DataTree
			// 
			this.DataTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.DataTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DataTree.ImageIndex = 0;
			this.DataTree.ImageList = this.DataTreeImageList;
			this.DataTree.Location = new System.Drawing.Point(0, 25);
			this.DataTree.Name = "DataTree";
			treeNode4.ContextMenuStrip = this.DataSourceMenu;
			treeNode4.ImageKey = "DataSources";
			treeNode4.Name = "DataSources";
			treeNode4.SelectedImageKey = "DataSources";
			treeNode4.Text = "Data Sources";
			treeNode5.ContextMenuStrip = this.ParameterMenu;
			treeNode5.ImageKey = "Parameters";
			treeNode5.Name = "Parameters";
			treeNode5.SelectedImageKey = "Parameters";
			treeNode5.Text = "Parameters";
			treeNode6.ContextMenuStrip = this.TotalMenu;
			treeNode6.ImageKey = "Totals";
			treeNode6.Name = "Totals";
			treeNode6.SelectedImageKey = "Totals";
			treeNode6.Text = "Totals";
			this.DataTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5,
            treeNode6});
			this.DataTree.SelectedImageIndex = 0;
			this.DataTree.Size = new System.Drawing.Size(250, 325);
			this.DataTree.TabIndex = 0;
			this.DataTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnSelectedNodeChanged);
			this.DataTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnNodeClick);
			// 
			// DataTreeImageList
			// 
			this.DataTreeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("DataTreeImageList.ImageStream")));
			this.DataTreeImageList.TransparentColor = System.Drawing.Color.Transparent;
			this.DataTreeImageList.Images.SetKeyName(0, "DataSources");
			this.DataTreeImageList.Images.SetKeyName(1, "DataSource");
			this.DataTreeImageList.Images.SetKeyName(2, "Parameters");
			this.DataTreeImageList.Images.SetKeyName(3, "Parameter");
			this.DataTreeImageList.Images.SetKeyName(4, "Totals");
			this.DataTreeImageList.Images.SetKeyName(5, "Total");
			this.DataTreeImageList.Images.SetKeyName(6, "None");
			this.DataTreeImageList.Images.SetKeyName(7, "String");
			this.DataTreeImageList.Images.SetKeyName(8, "Integer");
			this.DataTreeImageList.Images.SetKeyName(9, "Float");
			this.DataTreeImageList.Images.SetKeyName(10, "Boolean");
			this.DataTreeImageList.Images.SetKeyName(11, "DateTime");
			this.DataTreeImageList.Images.SetKeyName(12, "Object");
			this.DataTreeImageList.Images.SetKeyName(13, "Array");
			// 
			// ControlPanel
			// 
			this.ControlPanel.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.ControlPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ActionsButton,
            this.EditButton,
            this.DeleteButton,
            this.Separator1,
            this.MoveUpButton,
            this.MoveDownButton});
			this.ControlPanel.Location = new System.Drawing.Point(0, 0);
			this.ControlPanel.Name = "ControlPanel";
			this.ControlPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.ControlPanel.Size = new System.Drawing.Size(250, 25);
			this.ControlPanel.TabIndex = 3;
			this.ControlPanel.Text = "toolStrip1";
			// 
			// ActionsButton
			// 
			this.ActionsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.ActionsButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateDataSourceButton,
            this.CreateParameterButton,
            this.CreateTotalButton});
			this.ActionsButton.Image = ((System.Drawing.Image)(resources.GetObject("ActionsButton.Image")));
			this.ActionsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ActionsButton.Name = "ActionsButton";
			this.ActionsButton.Size = new System.Drawing.Size(63, 22);
			this.ActionsButton.Text = "Actions";
			this.ActionsButton.ButtonClick += new System.EventHandler(this.OnActions);
			// 
			// CreateDataSourceButton
			// 
			this.CreateDataSourceButton.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.DataSourceNew;
			this.CreateDataSourceButton.Name = "CreateDataSourceButton";
			this.CreateDataSourceButton.Size = new System.Drawing.Size(173, 22);
			this.CreateDataSourceButton.Text = "New Data Source...";
			this.CreateDataSourceButton.Click += new System.EventHandler(this.OnCreateDataSource);
			// 
			// CreateParameterButton
			// 
			this.CreateParameterButton.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ParameterNew;
			this.CreateParameterButton.Name = "CreateParameterButton";
			this.CreateParameterButton.Size = new System.Drawing.Size(173, 22);
			this.CreateParameterButton.Text = "New Parameter...";
			this.CreateParameterButton.Click += new System.EventHandler(this.OnCreateParameter);
			// 
			// CreateTotalButton
			// 
			this.CreateTotalButton.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.TotalNew;
			this.CreateTotalButton.Name = "CreateTotalButton";
			this.CreateTotalButton.Size = new System.Drawing.Size(173, 22);
			this.CreateTotalButton.Text = "New Total...";
			this.CreateTotalButton.Click += new System.EventHandler(this.OnCreateTotal);
			// 
			// EditButton
			// 
			this.EditButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.EditButton.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Edit;
			this.EditButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.EditButton.Name = "EditButton";
			this.EditButton.Size = new System.Drawing.Size(23, 22);
			this.EditButton.Text = "Edit";
			this.EditButton.Click += new System.EventHandler(this.OnEditClick);
			// 
			// DeleteButton
			// 
			this.DeleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.DeleteButton.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Delete;
			this.DeleteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.DeleteButton.Name = "DeleteButton";
			this.DeleteButton.Size = new System.Drawing.Size(23, 22);
			this.DeleteButton.Text = "Delete";
			this.DeleteButton.Click += new System.EventHandler(this.OnDeleteClick);
			// 
			// Separator1
			// 
			this.Separator1.Name = "Separator1";
			this.Separator1.Size = new System.Drawing.Size(6, 25);
			// 
			// MoveUpButton
			// 
			this.MoveUpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.MoveUpButton.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ArrowUp;
			this.MoveUpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.MoveUpButton.Name = "MoveUpButton";
			this.MoveUpButton.Size = new System.Drawing.Size(23, 22);
			this.MoveUpButton.Text = "Move Up";
			this.MoveUpButton.Click += new System.EventHandler(this.OnMoveUp);
			// 
			// MoveDownButton
			// 
			this.MoveDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.MoveDownButton.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ArrowDown;
			this.MoveDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.MoveDownButton.Name = "MoveDownButton";
			this.MoveDownButton.Size = new System.Drawing.Size(23, 22);
			this.MoveDownButton.Text = "Move Down";
			this.MoveDownButton.Click += new System.EventHandler(this.OnMoveDown);
			// 
			// ReportDataView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.DataTree);
			this.Controls.Add(this.ControlPanel);
			this.Name = "ReportDataView";
			this.Size = new System.Drawing.Size(250, 350);
			this.DataSourceMenu.ResumeLayout(false);
			this.ParameterMenu.ResumeLayout(false);
			this.TotalMenu.ResumeLayout(false);
			this.ControlPanel.ResumeLayout(false);
			this.ControlPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TreeView DataTree;
		private System.Windows.Forms.ImageList DataTreeImageList;
		private System.Windows.Forms.ContextMenuStrip DataSourceMenu;
		private System.Windows.Forms.ToolStripMenuItem CreateDataSourceMenuItem;
		private System.Windows.Forms.ToolStripMenuItem EditDataSourceMenuItem;
		private System.Windows.Forms.ToolStripMenuItem DeleteDataSourceMenuItem;
		private System.Windows.Forms.ContextMenuStrip ParameterMenu;
		private System.Windows.Forms.ContextMenuStrip TotalMenu;
		private System.Windows.Forms.ToolStripMenuItem CreateParameterMenuItem;
		private System.Windows.Forms.ToolStripMenuItem EditParameterMenuItem;
		private System.Windows.Forms.ToolStripMenuItem DeleteParameterMenuItem;
		private System.Windows.Forms.ToolStripMenuItem CreateTotalMenuItem;
		private System.Windows.Forms.ToolStripMenuItem EditTotalMenuItem;
		private System.Windows.Forms.ToolStripMenuItem DeleteTotalMenuItem;
		private System.Windows.Forms.ToolStrip ControlPanel;
		private System.Windows.Forms.ToolStripSplitButton ActionsButton;
		private System.Windows.Forms.ToolStripMenuItem CreateDataSourceButton;
		private System.Windows.Forms.ToolStripMenuItem CreateParameterButton;
		private System.Windows.Forms.ToolStripMenuItem CreateTotalButton;
		private System.Windows.Forms.ToolStripButton EditButton;
		private System.Windows.Forms.ToolStripButton DeleteButton;
		private System.Windows.Forms.ToolStripSeparator Separator1;
		private System.Windows.Forms.ToolStripButton MoveUpButton;
		private System.Windows.Forms.ToolStripButton MoveDownButton;
	}
}
