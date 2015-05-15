namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
	partial class DataSourceSchemaView
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
			System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("");
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataSourceSchemaView));
			this.DataSourceTreeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.CreatePropertyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.EditPropertyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.DeletePropertyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.Separator1MenuItem = new System.Windows.Forms.ToolStripSeparator();
			this.CutPropertyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CopyPropertyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PastePropertyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.Separator2MenuItem = new System.Windows.Forms.ToolStripSeparator();
			this.SortPropertiesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MoveUpPropertyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MoveDownPropertyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ControlPanel = new System.Windows.Forms.ToolStrip();
			this.ImportDataSchemaButton = new System.Windows.Forms.ToolStripButton();
			this.Separator0Button = new System.Windows.Forms.ToolStripSeparator();
			this.CreatePropertyButton = new System.Windows.Forms.ToolStripButton();
			this.EditPropertyButton = new System.Windows.Forms.ToolStripButton();
			this.DeletePropertyButton = new System.Windows.Forms.ToolStripButton();
			this.Separator1Button = new System.Windows.Forms.ToolStripSeparator();
			this.CutPropertyButton = new System.Windows.Forms.ToolStripButton();
			this.CopyPropertyButton = new System.Windows.Forms.ToolStripButton();
			this.PastePropertyButton = new System.Windows.Forms.ToolStripButton();
			this.Separator2Button = new System.Windows.Forms.ToolStripSeparator();
			this.SortPropertiesButton = new System.Windows.Forms.ToolStripButton();
			this.MoveUpPropertyButton = new System.Windows.Forms.ToolStripButton();
			this.MoveDownPropertyButton = new System.Windows.Forms.ToolStripButton();
			this.DataSourceTree = new System.Windows.Forms.TreeView();
			this.DataSourceTreeImageList = new System.Windows.Forms.ImageList(this.components);
			this.DataSourceTreeMenu.SuspendLayout();
			this.ControlPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// DataSourceTreeMenu
			// 
			this.DataSourceTreeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreatePropertyMenuItem,
            this.EditPropertyMenuItem,
            this.DeletePropertyMenuItem,
            this.Separator1MenuItem,
            this.CutPropertyMenuItem,
            this.CopyPropertyMenuItem,
            this.PastePropertyMenuItem,
            this.Separator2MenuItem,
            this.SortPropertiesMenuItem,
            this.MoveUpPropertyMenuItem,
            this.MoveDownPropertyMenuItem});
			this.DataSourceTreeMenu.Name = "DataSourceTreeMenu";
			this.DataSourceTreeMenu.Size = new System.Drawing.Size(200, 214);
			// 
			// CreatePropertyMenuItem
			// 
			this.CreatePropertyMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.New;
			this.CreatePropertyMenuItem.Name = "CreatePropertyMenuItem";
			this.CreatePropertyMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.CreatePropertyMenuItem.Size = new System.Drawing.Size(199, 22);
			this.CreatePropertyMenuItem.Text = "Create Property";
			this.CreatePropertyMenuItem.Click += new System.EventHandler(this.OnCreatePropertyMenuClick);
			// 
			// EditPropertyMenuItem
			// 
			this.EditPropertyMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Edit;
			this.EditPropertyMenuItem.Name = "EditPropertyMenuItem";
			this.EditPropertyMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.EditPropertyMenuItem.Size = new System.Drawing.Size(199, 22);
			this.EditPropertyMenuItem.Text = "Edit Property";
			this.EditPropertyMenuItem.Click += new System.EventHandler(this.OnEditPropertyMenuClick);
			// 
			// DeletePropertyMenuItem
			// 
			this.DeletePropertyMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Delete;
			this.DeletePropertyMenuItem.Name = "DeletePropertyMenuItem";
			this.DeletePropertyMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.DeletePropertyMenuItem.Size = new System.Drawing.Size(199, 22);
			this.DeletePropertyMenuItem.Text = "Delete Property";
			this.DeletePropertyMenuItem.Click += new System.EventHandler(this.OnDeletePropertyMenuClick);
			// 
			// Separator1MenuItem
			// 
			this.Separator1MenuItem.Name = "Separator1MenuItem";
			this.Separator1MenuItem.Size = new System.Drawing.Size(196, 6);
			// 
			// CutPropertyMenuItem
			// 
			this.CutPropertyMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Cut;
			this.CutPropertyMenuItem.Name = "CutPropertyMenuItem";
			this.CutPropertyMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.CutPropertyMenuItem.Size = new System.Drawing.Size(199, 22);
			this.CutPropertyMenuItem.Text = "Cut";
			this.CutPropertyMenuItem.Click += new System.EventHandler(this.OnCutPropertyMenuClick);
			// 
			// CopyPropertyMenuItem
			// 
			this.CopyPropertyMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Copy;
			this.CopyPropertyMenuItem.Name = "CopyPropertyMenuItem";
			this.CopyPropertyMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.CopyPropertyMenuItem.Size = new System.Drawing.Size(199, 22);
			this.CopyPropertyMenuItem.Text = "Copy";
			this.CopyPropertyMenuItem.Click += new System.EventHandler(this.OnCopyPropertyMenuClick);
			// 
			// PastePropertyMenuItem
			// 
			this.PastePropertyMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Paste;
			this.PastePropertyMenuItem.Name = "PastePropertyMenuItem";
			this.PastePropertyMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.PastePropertyMenuItem.Size = new System.Drawing.Size(199, 22);
			this.PastePropertyMenuItem.Text = "Paste";
			this.PastePropertyMenuItem.Click += new System.EventHandler(this.OnPastePropertyMenuClick);
			// 
			// Separator2MenuItem
			// 
			this.Separator2MenuItem.Name = "Separator2MenuItem";
			this.Separator2MenuItem.Size = new System.Drawing.Size(196, 6);
			// 
			// SortPropertiesMenuItem
			// 
			this.SortPropertiesMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.OrderByAsc;
			this.SortPropertiesMenuItem.Name = "SortPropertiesMenuItem";
			this.SortPropertiesMenuItem.Size = new System.Drawing.Size(199, 22);
			this.SortPropertiesMenuItem.Text = "Sort Properties";
			this.SortPropertiesMenuItem.Click += new System.EventHandler(this.OnSortPropertiesMenuClick);
			// 
			// MoveUpPropertyMenuItem
			// 
			this.MoveUpPropertyMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ArrowUp;
			this.MoveUpPropertyMenuItem.Name = "MoveUpPropertyMenuItem";
			this.MoveUpPropertyMenuItem.Size = new System.Drawing.Size(199, 22);
			this.MoveUpPropertyMenuItem.Text = "Move Up";
			this.MoveUpPropertyMenuItem.Click += new System.EventHandler(this.OnMoveUpPropertyMenuClick);
			// 
			// MoveDownPropertyMenuItem
			// 
			this.MoveDownPropertyMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ArrowDown;
			this.MoveDownPropertyMenuItem.Name = "MoveDownPropertyMenuItem";
			this.MoveDownPropertyMenuItem.Size = new System.Drawing.Size(199, 22);
			this.MoveDownPropertyMenuItem.Text = "Move Down";
			this.MoveDownPropertyMenuItem.Click += new System.EventHandler(this.OnMoveDownPropertyMenuClick);
			// 
			// ControlPanel
			// 
			this.ControlPanel.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.ControlPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ImportDataSchemaButton,
            this.Separator0Button,
            this.CreatePropertyButton,
            this.EditPropertyButton,
            this.DeletePropertyButton,
            this.Separator1Button,
            this.CutPropertyButton,
            this.CopyPropertyButton,
            this.PastePropertyButton,
            this.Separator2Button,
            this.SortPropertiesButton,
            this.MoveUpPropertyButton,
            this.MoveDownPropertyButton});
			this.ControlPanel.Location = new System.Drawing.Point(0, 0);
			this.ControlPanel.Name = "ControlPanel";
			this.ControlPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.ControlPanel.Size = new System.Drawing.Size(280, 25);
			this.ControlPanel.TabIndex = 1;
			this.ControlPanel.Text = "Control Panel";
			// 
			// ImportDataSchemaButton
			// 
			this.ImportDataSchemaButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ImportDataSchemaButton.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.DataSourceSync;
			this.ImportDataSchemaButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ImportDataSchemaButton.Name = "ImportDataSchemaButton";
			this.ImportDataSchemaButton.Size = new System.Drawing.Size(23, 22);
			this.ImportDataSchemaButton.Text = "Import Data Schema";
			this.ImportDataSchemaButton.Visible = false;
			this.ImportDataSchemaButton.Click += new System.EventHandler(this.OnImportDataSchemaButtonClick);
			// 
			// Separator0Button
			// 
			this.Separator0Button.Name = "Separator0Button";
			this.Separator0Button.Size = new System.Drawing.Size(6, 25);
			this.Separator0Button.Visible = false;
			// 
			// CreatePropertyButton
			// 
			this.CreatePropertyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.CreatePropertyButton.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.New;
			this.CreatePropertyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.CreatePropertyButton.Name = "CreatePropertyButton";
			this.CreatePropertyButton.Size = new System.Drawing.Size(23, 22);
			this.CreatePropertyButton.Text = "Create Property";
			this.CreatePropertyButton.ToolTipText = "Create Property";
			this.CreatePropertyButton.Click += new System.EventHandler(this.OnCreatePropertyButtonClick);
			// 
			// EditPropertyButton
			// 
			this.EditPropertyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.EditPropertyButton.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Edit;
			this.EditPropertyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.EditPropertyButton.Name = "EditPropertyButton";
			this.EditPropertyButton.Size = new System.Drawing.Size(23, 22);
			this.EditPropertyButton.Text = "Edit Property";
			this.EditPropertyButton.Click += new System.EventHandler(this.OnEditPropertyButtonClick);
			// 
			// DeletePropertyButton
			// 
			this.DeletePropertyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.DeletePropertyButton.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Delete;
			this.DeletePropertyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.DeletePropertyButton.Name = "DeletePropertyButton";
			this.DeletePropertyButton.Size = new System.Drawing.Size(23, 22);
			this.DeletePropertyButton.Text = "Delete Property";
			this.DeletePropertyButton.Click += new System.EventHandler(this.OnDeletePropertyButtonClick);
			// 
			// Separator1Button
			// 
			this.Separator1Button.Name = "Separator1Button";
			this.Separator1Button.Size = new System.Drawing.Size(6, 25);
			// 
			// CutPropertyButton
			// 
			this.CutPropertyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.CutPropertyButton.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Cut;
			this.CutPropertyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.CutPropertyButton.Name = "CutPropertyButton";
			this.CutPropertyButton.Size = new System.Drawing.Size(23, 22);
			this.CutPropertyButton.Text = "Cut Property";
			this.CutPropertyButton.Click += new System.EventHandler(this.OnCutPropertyButtonClick);
			// 
			// CopyPropertyButton
			// 
			this.CopyPropertyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.CopyPropertyButton.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Copy;
			this.CopyPropertyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.CopyPropertyButton.Name = "CopyPropertyButton";
			this.CopyPropertyButton.Size = new System.Drawing.Size(23, 22);
			this.CopyPropertyButton.Text = "Copy Property";
			this.CopyPropertyButton.Click += new System.EventHandler(this.OnCopyPropertyButtonClick);
			// 
			// PastePropertyButton
			// 
			this.PastePropertyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.PastePropertyButton.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Paste;
			this.PastePropertyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.PastePropertyButton.Name = "PastePropertyButton";
			this.PastePropertyButton.Size = new System.Drawing.Size(23, 22);
			this.PastePropertyButton.Text = "Paste Property";
			this.PastePropertyButton.Click += new System.EventHandler(this.OnPastePropertyButtonClick);
			// 
			// Separator2Button
			// 
			this.Separator2Button.Name = "Separator2Button";
			this.Separator2Button.Size = new System.Drawing.Size(6, 25);
			// 
			// SortPropertiesButton
			// 
			this.SortPropertiesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.SortPropertiesButton.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.OrderByAsc;
			this.SortPropertiesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.SortPropertiesButton.Name = "SortPropertiesButton";
			this.SortPropertiesButton.Size = new System.Drawing.Size(23, 22);
			this.SortPropertiesButton.Text = "Sort Properties";
			this.SortPropertiesButton.Click += new System.EventHandler(this.OnSortPropertiesButtonClick);
			// 
			// MoveUpPropertyButton
			// 
			this.MoveUpPropertyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.MoveUpPropertyButton.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ArrowUp;
			this.MoveUpPropertyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.MoveUpPropertyButton.Name = "MoveUpPropertyButton";
			this.MoveUpPropertyButton.Size = new System.Drawing.Size(23, 22);
			this.MoveUpPropertyButton.Text = "Move Up";
			this.MoveUpPropertyButton.ToolTipText = "Move Up";
			this.MoveUpPropertyButton.Click += new System.EventHandler(this.OnMoveUpPropertyButtonClick);
			// 
			// MoveDownPropertyButton
			// 
			this.MoveDownPropertyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.MoveDownPropertyButton.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ArrowDown;
			this.MoveDownPropertyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.MoveDownPropertyButton.Name = "MoveDownPropertyButton";
			this.MoveDownPropertyButton.Size = new System.Drawing.Size(23, 22);
			this.MoveDownPropertyButton.Text = "Move Down";
			this.MoveDownPropertyButton.Click += new System.EventHandler(this.OnMoveDownPropertyButtonClick);
			// 
			// DataSourceTree
			// 
			this.DataSourceTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.DataSourceTree.ContextMenuStrip = this.DataSourceTreeMenu;
			this.DataSourceTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DataSourceTree.HideSelection = false;
			this.DataSourceTree.ImageIndex = 0;
			this.DataSourceTree.ImageList = this.DataSourceTreeImageList;
			this.DataSourceTree.Location = new System.Drawing.Point(0, 25);
			this.DataSourceTree.Name = "DataSourceTree";
			treeNode2.ImageKey = "Array";
			treeNode2.Name = "DataSourceName";
			treeNode2.SelectedImageKey = "Array";
			treeNode2.Text = "";
			this.DataSourceTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
			this.DataSourceTree.SelectedImageIndex = 0;
			this.DataSourceTree.Size = new System.Drawing.Size(280, 225);
			this.DataSourceTree.TabIndex = 2;
			this.DataSourceTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnSelectedNodeChanged);
			this.DataSourceTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnNodeClick);
			// 
			// DataSourceTreeImageList
			// 
			this.DataSourceTreeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("DataSourceTreeImageList.ImageStream")));
			this.DataSourceTreeImageList.TransparentColor = System.Drawing.Color.Transparent;
			this.DataSourceTreeImageList.Images.SetKeyName(0, "None");
			this.DataSourceTreeImageList.Images.SetKeyName(1, "String");
			this.DataSourceTreeImageList.Images.SetKeyName(2, "Float");
			this.DataSourceTreeImageList.Images.SetKeyName(3, "Integer");
			this.DataSourceTreeImageList.Images.SetKeyName(4, "Boolean");
			this.DataSourceTreeImageList.Images.SetKeyName(5, "DateTime");
			this.DataSourceTreeImageList.Images.SetKeyName(6, "Object");
			this.DataSourceTreeImageList.Images.SetKeyName(7, "Array");
			// 
			// DataSourceSchemaView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.DataSourceTree);
			this.Controls.Add(this.ControlPanel);
			this.Name = "DataSourceSchemaView";
			this.Size = new System.Drawing.Size(280, 250);
			this.DataSourceTreeMenu.ResumeLayout(false);
			this.ControlPanel.ResumeLayout(false);
			this.ControlPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip ControlPanel;
		private System.Windows.Forms.ToolStripButton CreatePropertyButton;
		private System.Windows.Forms.ToolStripButton EditPropertyButton;
		private System.Windows.Forms.ToolStripButton DeletePropertyButton;
		private System.Windows.Forms.TreeView DataSourceTree;
		private System.Windows.Forms.ImageList DataSourceTreeImageList;
		private System.Windows.Forms.ContextMenuStrip DataSourceTreeMenu;
		private System.Windows.Forms.ToolStripMenuItem CreatePropertyMenuItem;
		private System.Windows.Forms.ToolStripMenuItem EditPropertyMenuItem;
		private System.Windows.Forms.ToolStripMenuItem DeletePropertyMenuItem;
		private System.Windows.Forms.ToolStripSeparator Separator1MenuItem;
		private System.Windows.Forms.ToolStripMenuItem CutPropertyMenuItem;
		private System.Windows.Forms.ToolStripMenuItem CopyPropertyMenuItem;
		private System.Windows.Forms.ToolStripMenuItem PastePropertyMenuItem;
		private System.Windows.Forms.ToolStripSeparator Separator1Button;
		private System.Windows.Forms.ToolStripButton CutPropertyButton;
		private System.Windows.Forms.ToolStripButton CopyPropertyButton;
		private System.Windows.Forms.ToolStripButton PastePropertyButton;
		private System.Windows.Forms.ToolStripSeparator Separator2Button;
		private System.Windows.Forms.ToolStripButton MoveUpPropertyButton;
		private System.Windows.Forms.ToolStripButton MoveDownPropertyButton;
		private System.Windows.Forms.ToolStripButton SortPropertiesButton;
		private System.Windows.Forms.ToolStripSeparator Separator2MenuItem;
		private System.Windows.Forms.ToolStripMenuItem SortPropertiesMenuItem;
		private System.Windows.Forms.ToolStripMenuItem MoveUpPropertyMenuItem;
		private System.Windows.Forms.ToolStripMenuItem MoveDownPropertyMenuItem;
		private System.Windows.Forms.ToolStripButton ImportDataSchemaButton;
		private System.Windows.Forms.ToolStripSeparator Separator0Button;
	}
}
