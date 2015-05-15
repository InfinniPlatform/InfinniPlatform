namespace InfinniPlatform.ReportDesigner.Views.Designer
{
	partial class ReportLayoutView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportLayoutView));
			this.MainMenu = new System.Windows.Forms.MenuStrip();
			this.ReportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ImportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ExportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.FileMenuItemSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.PageSetupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PreviewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.EditMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.UndoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.RedoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.EditMenuItemSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.CutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CopyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PasteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.DeleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.InsertMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.InsertTextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.InsertTableMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.LayoutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ReportTitleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ReportSummaryMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PageHeaderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PageFooterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ReportMenuItemSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.ConfigureBandsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.DataMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.NewDataSourceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.NewParameterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.NewTotalMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MainPanel = new System.Windows.Forms.Panel();
			this.MainMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainMenu
			// 
			this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ReportMenuItem,
            this.EditMenuItem,
            this.InsertMenuItem,
            this.LayoutMenuItem,
            this.DataMenuItem});
			this.MainMenu.Location = new System.Drawing.Point(0, 0);
			this.MainMenu.Name = "MainMenu";
			this.MainMenu.Size = new System.Drawing.Size(300, 24);
			this.MainMenu.TabIndex = 5;
			this.MainMenu.Text = "menuStrip1";
			// 
			// ReportMenuItem
			// 
			this.ReportMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ImportMenuItem,
            this.ExportMenuItem,
            this.FileMenuItemSeparator1,
            this.PageSetupMenuItem,
            this.PreviewMenuItem});
			this.ReportMenuItem.Name = "ReportMenuItem";
			this.ReportMenuItem.Size = new System.Drawing.Size(54, 20);
			this.ReportMenuItem.Text = "&Report";
			// 
			// ImportMenuItem
			// 
			this.ImportMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Import;
			this.ImportMenuItem.Name = "ImportMenuItem";
			this.ImportMenuItem.Size = new System.Drawing.Size(165, 22);
			this.ImportMenuItem.Text = "Import";
			this.ImportMenuItem.Click += new System.EventHandler(this.OnImportReport);
			// 
			// ExportMenuItem
			// 
			this.ExportMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Export;
			this.ExportMenuItem.Name = "ExportMenuItem";
			this.ExportMenuItem.Size = new System.Drawing.Size(165, 22);
			this.ExportMenuItem.Text = "Export";
			this.ExportMenuItem.Click += new System.EventHandler(this.OnExportReport);
			// 
			// FileMenuItemSeparator1
			// 
			this.FileMenuItemSeparator1.Name = "FileMenuItemSeparator1";
			this.FileMenuItemSeparator1.Size = new System.Drawing.Size(162, 6);
			// 
			// PageSetupMenuItem
			// 
			this.PageSetupMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("PageSetupMenuItem.Image")));
			this.PageSetupMenuItem.Name = "PageSetupMenuItem";
			this.PageSetupMenuItem.Size = new System.Drawing.Size(165, 22);
			this.PageSetupMenuItem.Text = "Page Setup...";
			this.PageSetupMenuItem.Click += new System.EventHandler(this.OnPageSetup);
			// 
			// PreviewMenuItem
			// 
			this.PreviewMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Preview;
			this.PreviewMenuItem.Name = "PreviewMenuItem";
			this.PreviewMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
			this.PreviewMenuItem.Size = new System.Drawing.Size(165, 22);
			this.PreviewMenuItem.Text = "Pre&view...";
			this.PreviewMenuItem.Click += new System.EventHandler(this.OnPreviewReport);
			// 
			// EditMenuItem
			// 
			this.EditMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UndoMenuItem,
            this.RedoMenuItem,
            this.EditMenuItemSeparator1,
            this.CutMenuItem,
            this.CopyMenuItem,
            this.PasteMenuItem,
            this.DeleteMenuItem});
			this.EditMenuItem.Name = "EditMenuItem";
			this.EditMenuItem.Size = new System.Drawing.Size(39, 20);
			this.EditMenuItem.Text = "&Edit";
			this.EditMenuItem.DropDownOpening += new System.EventHandler(this.OnEditMenuOpening);
			// 
			// UndoMenuItem
			// 
			this.UndoMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.EditUndo;
			this.UndoMenuItem.Name = "UndoMenuItem";
			this.UndoMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.UndoMenuItem.Size = new System.Drawing.Size(144, 22);
			this.UndoMenuItem.Text = "&Undo";
			this.UndoMenuItem.Click += new System.EventHandler(this.OnEditUndo);
			// 
			// RedoMenuItem
			// 
			this.RedoMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.EditRedo;
			this.RedoMenuItem.Name = "RedoMenuItem";
			this.RedoMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
			this.RedoMenuItem.Size = new System.Drawing.Size(144, 22);
			this.RedoMenuItem.Text = "&Redo";
			this.RedoMenuItem.Click += new System.EventHandler(this.OnEditRedo);
			// 
			// EditMenuItemSeparator1
			// 
			this.EditMenuItemSeparator1.Name = "EditMenuItemSeparator1";
			this.EditMenuItemSeparator1.Size = new System.Drawing.Size(141, 6);
			// 
			// CutMenuItem
			// 
			this.CutMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Cut;
			this.CutMenuItem.Name = "CutMenuItem";
			this.CutMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.CutMenuItem.Size = new System.Drawing.Size(144, 22);
			this.CutMenuItem.Text = "Cu&t";
			this.CutMenuItem.Click += new System.EventHandler(this.OnEditCut);
			// 
			// CopyMenuItem
			// 
			this.CopyMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Copy;
			this.CopyMenuItem.Name = "CopyMenuItem";
			this.CopyMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.CopyMenuItem.Size = new System.Drawing.Size(144, 22);
			this.CopyMenuItem.Text = "&Copy";
			this.CopyMenuItem.Click += new System.EventHandler(this.OnEditCopy);
			// 
			// PasteMenuItem
			// 
			this.PasteMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Paste;
			this.PasteMenuItem.Name = "PasteMenuItem";
			this.PasteMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.PasteMenuItem.Size = new System.Drawing.Size(144, 22);
			this.PasteMenuItem.Text = "&Paste";
			this.PasteMenuItem.Click += new System.EventHandler(this.OnEditPaste);
			// 
			// DeleteMenuItem
			// 
			this.DeleteMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Delete;
			this.DeleteMenuItem.Name = "DeleteMenuItem";
			this.DeleteMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.DeleteMenuItem.Size = new System.Drawing.Size(144, 22);
			this.DeleteMenuItem.Text = "&Delete";
			this.DeleteMenuItem.Click += new System.EventHandler(this.OnEditDelete);
			// 
			// InsertMenuItem
			// 
			this.InsertMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.InsertTextMenuItem,
            this.InsertTableMenuItem});
			this.InsertMenuItem.Name = "InsertMenuItem";
			this.InsertMenuItem.Size = new System.Drawing.Size(48, 20);
			this.InsertMenuItem.Text = "&Insert";
			// 
			// InsertTextMenuItem
			// 
			this.InsertTextMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ElementText;
			this.InsertTextMenuItem.Name = "InsertTextMenuItem";
			this.InsertTextMenuItem.Size = new System.Drawing.Size(103, 22);
			this.InsertTextMenuItem.Text = "Text";
			this.InsertTextMenuItem.Click += new System.EventHandler(this.OnInsertTextElement);
			// 
			// InsertTableMenuItem
			// 
			this.InsertTableMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ElementTable;
			this.InsertTableMenuItem.Name = "InsertTableMenuItem";
			this.InsertTableMenuItem.Size = new System.Drawing.Size(103, 22);
			this.InsertTableMenuItem.Text = "Table";
			this.InsertTableMenuItem.Click += new System.EventHandler(this.OnInsertTableElement);
			// 
			// LayoutMenuItem
			// 
			this.LayoutMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ReportTitleMenuItem,
            this.ReportSummaryMenuItem,
            this.PageHeaderMenuItem,
            this.PageFooterMenuItem,
            this.ReportMenuItemSeparator1,
            this.ConfigureBandsMenuItem});
			this.LayoutMenuItem.Name = "LayoutMenuItem";
			this.LayoutMenuItem.Size = new System.Drawing.Size(55, 20);
			this.LayoutMenuItem.Text = "&Layout";
			this.LayoutMenuItem.DropDownOpening += new System.EventHandler(this.OnReportMenuOpening);
			// 
			// ReportTitleMenuItem
			// 
			this.ReportTitleMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ReportTitleBand;
			this.ReportTitleMenuItem.Name = "ReportTitleMenuItem";
			this.ReportTitleMenuItem.Size = new System.Drawing.Size(171, 22);
			this.ReportTitleMenuItem.Text = "Report &Title";
			this.ReportTitleMenuItem.Click += new System.EventHandler(this.OnSetReportTitleBand);
			// 
			// ReportSummaryMenuItem
			// 
			this.ReportSummaryMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ReportSummaryBand;
			this.ReportSummaryMenuItem.Name = "ReportSummaryMenuItem";
			this.ReportSummaryMenuItem.Size = new System.Drawing.Size(171, 22);
			this.ReportSummaryMenuItem.Text = "Report &Summary";
			this.ReportSummaryMenuItem.Click += new System.EventHandler(this.OnSetReportSummaryBand);
			// 
			// PageHeaderMenuItem
			// 
			this.PageHeaderMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ReportPageHeaderBand;
			this.PageHeaderMenuItem.Name = "PageHeaderMenuItem";
			this.PageHeaderMenuItem.Size = new System.Drawing.Size(171, 22);
			this.PageHeaderMenuItem.Text = "Page &Header";
			this.PageHeaderMenuItem.Click += new System.EventHandler(this.OnSetReportPageHeaderBand);
			// 
			// PageFooterMenuItem
			// 
			this.PageFooterMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ReportPageFooterBand;
			this.PageFooterMenuItem.Name = "PageFooterMenuItem";
			this.PageFooterMenuItem.Size = new System.Drawing.Size(171, 22);
			this.PageFooterMenuItem.Text = "Page &Footer";
			this.PageFooterMenuItem.Click += new System.EventHandler(this.OnReportPageFooterBand);
			// 
			// ReportMenuItemSeparator1
			// 
			this.ReportMenuItemSeparator1.Name = "ReportMenuItemSeparator1";
			this.ReportMenuItemSeparator1.Size = new System.Drawing.Size(168, 6);
			// 
			// ConfigureBandsMenuItem
			// 
			this.ConfigureBandsMenuItem.Name = "ConfigureBandsMenuItem";
			this.ConfigureBandsMenuItem.Size = new System.Drawing.Size(171, 22);
			this.ConfigureBandsMenuItem.Text = "&Configure Bands...";
			this.ConfigureBandsMenuItem.Click += new System.EventHandler(this.OnConfigureBands);
			// 
			// DataMenuItem
			// 
			this.DataMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewDataSourceMenuItem,
            this.NewParameterMenuItem,
            this.NewTotalMenuItem});
			this.DataMenuItem.Name = "DataMenuItem";
			this.DataMenuItem.Size = new System.Drawing.Size(43, 20);
			this.DataMenuItem.Text = "&Data";
			// 
			// NewDataSourceMenuItem
			// 
			this.NewDataSourceMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.DataSourceNew;
			this.NewDataSourceMenuItem.Name = "NewDataSourceMenuItem";
			this.NewDataSourceMenuItem.Size = new System.Drawing.Size(173, 22);
			this.NewDataSourceMenuItem.Text = "New &Data Source...";
			this.NewDataSourceMenuItem.Click += new System.EventHandler(this.OnCreateDataSource);
			// 
			// NewParameterMenuItem
			// 
			this.NewParameterMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ParameterNew;
			this.NewParameterMenuItem.Name = "NewParameterMenuItem";
			this.NewParameterMenuItem.Size = new System.Drawing.Size(173, 22);
			this.NewParameterMenuItem.Text = "New &Parameter...";
			this.NewParameterMenuItem.Click += new System.EventHandler(this.OnCreateParameter);
			// 
			// NewTotalMenuItem
			// 
			this.NewTotalMenuItem.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.TotalNew;
			this.NewTotalMenuItem.Name = "NewTotalMenuItem";
			this.NewTotalMenuItem.Size = new System.Drawing.Size(173, 22);
			this.NewTotalMenuItem.Text = "New &Total...";
			this.NewTotalMenuItem.Click += new System.EventHandler(this.OnCreateTotal);
			// 
			// MainPanel
			// 
			this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainPanel.Location = new System.Drawing.Point(0, 24);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(300, 126);
			this.MainPanel.TabIndex = 6;
			// 
			// ReportLayoutView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.MainPanel);
			this.Controls.Add(this.MainMenu);
			this.Name = "ReportLayoutView";
			this.Size = new System.Drawing.Size(300, 150);
			this.MainMenu.ResumeLayout(false);
			this.MainMenu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip MainMenu;
		private System.Windows.Forms.ToolStripMenuItem ReportMenuItem;
		private System.Windows.Forms.ToolStripSeparator FileMenuItemSeparator1;
		private System.Windows.Forms.ToolStripMenuItem PreviewMenuItem;
		private System.Windows.Forms.ToolStripMenuItem EditMenuItem;
		private System.Windows.Forms.ToolStripMenuItem UndoMenuItem;
		private System.Windows.Forms.ToolStripMenuItem RedoMenuItem;
		private System.Windows.Forms.ToolStripSeparator EditMenuItemSeparator1;
		private System.Windows.Forms.ToolStripMenuItem CutMenuItem;
		private System.Windows.Forms.ToolStripMenuItem CopyMenuItem;
		private System.Windows.Forms.ToolStripMenuItem PasteMenuItem;
		private System.Windows.Forms.ToolStripMenuItem DeleteMenuItem;
		private System.Windows.Forms.ToolStripMenuItem InsertMenuItem;
		private System.Windows.Forms.ToolStripMenuItem InsertTextMenuItem;
		private System.Windows.Forms.ToolStripMenuItem InsertTableMenuItem;
		private System.Windows.Forms.ToolStripMenuItem LayoutMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ReportTitleMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ReportSummaryMenuItem;
		private System.Windows.Forms.ToolStripMenuItem PageHeaderMenuItem;
		private System.Windows.Forms.ToolStripMenuItem PageFooterMenuItem;
		private System.Windows.Forms.ToolStripSeparator ReportMenuItemSeparator1;
		private System.Windows.Forms.ToolStripMenuItem ConfigureBandsMenuItem;
		private System.Windows.Forms.ToolStripMenuItem DataMenuItem;
		private System.Windows.Forms.ToolStripMenuItem NewDataSourceMenuItem;
		private System.Windows.Forms.ToolStripMenuItem NewParameterMenuItem;
		private System.Windows.Forms.ToolStripMenuItem NewTotalMenuItem;
		private System.Windows.Forms.Panel MainPanel;
		private System.Windows.Forms.ToolStripMenuItem PageSetupMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ImportMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ExportMenuItem;
	}
}
