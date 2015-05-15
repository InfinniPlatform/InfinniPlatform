namespace InfinniPlatform.ReportDesigner.Views.Bands
{
	partial class ReportBandConfigView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportBandConfigView));
			this.MainPanel = new System.Windows.Forms.Panel();
			this.BandsTree = new System.Windows.Forms.TreeView();
			this.AddButtonMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.DeleteMenuBtn = new System.Windows.Forms.ToolStripMenuItem();
			this.Separator0 = new System.Windows.Forms.ToolStripSeparator();
			this.AddReportTitleBtn = new System.Windows.Forms.ToolStripMenuItem();
			this.AddReportSummaryBtn = new System.Windows.Forms.ToolStripMenuItem();
			this.Separator1 = new System.Windows.Forms.ToolStripSeparator();
			this.AddPageHeaderBtn = new System.Windows.Forms.ToolStripMenuItem();
			this.AddPageFooterBtn = new System.Windows.Forms.ToolStripMenuItem();
			this.Separator2 = new System.Windows.Forms.ToolStripSeparator();
			this.AddGroupHeaderBtn = new System.Windows.Forms.ToolStripMenuItem();
			this.AddGroupFooterBtn = new System.Windows.Forms.ToolStripMenuItem();
			this.AddDataBandBtn = new System.Windows.Forms.ToolStripMenuItem();
			this.BandsTreeImageList = new System.Windows.Forms.ImageList(this.components);
			this.ControlPanel = new System.Windows.Forms.Panel();
			this.MoveDownBtn = new System.Windows.Forms.Button();
			this.MoveUpBtn = new System.Windows.Forms.Button();
			this.DeleteBtn = new System.Windows.Forms.Button();
			this.CloseBtn = new System.Windows.Forms.Button();
			this.AddBtn = new System.Windows.Forms.Button();
			this.MainPanel.SuspendLayout();
			this.AddButtonMenu.SuspendLayout();
			this.ControlPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainPanel
			// 
			this.MainPanel.Controls.Add(this.BandsTree);
			this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainPanel.Location = new System.Drawing.Point(0, 0);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Padding = new System.Windows.Forms.Padding(10, 10, 0, 10);
			this.MainPanel.Size = new System.Drawing.Size(339, 362);
			this.MainPanel.TabIndex = 1;
			// 
			// BandsTree
			// 
			this.BandsTree.ContextMenuStrip = this.AddButtonMenu;
			this.BandsTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BandsTree.HideSelection = false;
			this.BandsTree.ImageIndex = 0;
			this.BandsTree.ImageList = this.BandsTreeImageList;
			this.BandsTree.Location = new System.Drawing.Point(10, 10);
			this.BandsTree.Name = "BandsTree";
			this.BandsTree.SelectedImageIndex = 0;
			this.BandsTree.ShowLines = false;
			this.BandsTree.ShowPlusMinus = false;
			this.BandsTree.Size = new System.Drawing.Size(329, 342);
			this.BandsTree.TabIndex = 0;
			this.BandsTree.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.OnBeforeCollapse);
			this.BandsTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnSelectedNodeChanged);
			this.BandsTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnNodeClick);
			// 
			// AddButtonMenu
			// 
			this.AddButtonMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteMenuBtn,
            this.Separator0,
            this.AddReportTitleBtn,
            this.AddReportSummaryBtn,
            this.Separator1,
            this.AddPageHeaderBtn,
            this.AddPageFooterBtn,
            this.Separator2,
            this.AddGroupHeaderBtn,
            this.AddGroupFooterBtn,
            this.AddDataBandBtn});
			this.AddButtonMenu.Name = "contextMenuStrip1";
			this.AddButtonMenu.Size = new System.Drawing.Size(164, 198);
			// 
			// DeleteMenuBtn
			// 
			this.DeleteMenuBtn.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Delete;
			this.DeleteMenuBtn.Name = "DeleteMenuBtn";
			this.DeleteMenuBtn.Size = new System.Drawing.Size(163, 22);
			this.DeleteMenuBtn.Text = "Delete";
			this.DeleteMenuBtn.Click += new System.EventHandler(this.OnDelete);
			// 
			// Separator0
			// 
			this.Separator0.Name = "Separator0";
			this.Separator0.Size = new System.Drawing.Size(160, 6);
			// 
			// AddReportTitleBtn
			// 
			this.AddReportTitleBtn.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ReportTitleBand;
			this.AddReportTitleBtn.Name = "AddReportTitleBtn";
			this.AddReportTitleBtn.Size = new System.Drawing.Size(163, 22);
			this.AddReportTitleBtn.Text = "Report Title";
			this.AddReportTitleBtn.Click += new System.EventHandler(this.OnAddReportTitle);
			// 
			// AddReportSummaryBtn
			// 
			this.AddReportSummaryBtn.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ReportSummaryBand;
			this.AddReportSummaryBtn.Name = "AddReportSummaryBtn";
			this.AddReportSummaryBtn.Size = new System.Drawing.Size(163, 22);
			this.AddReportSummaryBtn.Text = "Report Summary";
			this.AddReportSummaryBtn.Click += new System.EventHandler(this.OnAddReportSummary);
			// 
			// Separator1
			// 
			this.Separator1.Name = "Separator1";
			this.Separator1.Size = new System.Drawing.Size(160, 6);
			// 
			// AddPageHeaderBtn
			// 
			this.AddPageHeaderBtn.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ReportPageHeaderBand;
			this.AddPageHeaderBtn.Name = "AddPageHeaderBtn";
			this.AddPageHeaderBtn.Size = new System.Drawing.Size(163, 22);
			this.AddPageHeaderBtn.Text = "Page Header";
			this.AddPageHeaderBtn.Click += new System.EventHandler(this.OnAddPageHeader);
			// 
			// AddPageFooterBtn
			// 
			this.AddPageFooterBtn.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ReportPageFooterBand;
			this.AddPageFooterBtn.Name = "AddPageFooterBtn";
			this.AddPageFooterBtn.Size = new System.Drawing.Size(163, 22);
			this.AddPageFooterBtn.Text = "Page Footer";
			this.AddPageFooterBtn.Click += new System.EventHandler(this.OnAddPageFooter);
			// 
			// Separator2
			// 
			this.Separator2.Name = "Separator2";
			this.Separator2.Size = new System.Drawing.Size(160, 6);
			// 
			// AddGroupHeaderBtn
			// 
			this.AddGroupHeaderBtn.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ReportGroupHeaderBand;
			this.AddGroupHeaderBtn.Name = "AddGroupHeaderBtn";
			this.AddGroupHeaderBtn.Size = new System.Drawing.Size(163, 22);
			this.AddGroupHeaderBtn.Text = "Group Header";
			this.AddGroupHeaderBtn.Click += new System.EventHandler(this.OnAddGroupHeader);
			// 
			// AddGroupFooterBtn
			// 
			this.AddGroupFooterBtn.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ReportGroupFooterBand;
			this.AddGroupFooterBtn.Name = "AddGroupFooterBtn";
			this.AddGroupFooterBtn.Size = new System.Drawing.Size(163, 22);
			this.AddGroupFooterBtn.Text = "Group Footer";
			this.AddGroupFooterBtn.Click += new System.EventHandler(this.OnAddGroupFooter);
			// 
			// AddDataBandBtn
			// 
			this.AddDataBandBtn.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ReportDataBand;
			this.AddDataBandBtn.Name = "AddDataBandBtn";
			this.AddDataBandBtn.Size = new System.Drawing.Size(163, 22);
			this.AddDataBandBtn.Text = "Data";
			this.AddDataBandBtn.Click += new System.EventHandler(this.OnAddData);
			// 
			// BandsTreeImageList
			// 
			this.BandsTreeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("BandsTreeImageList.ImageStream")));
			this.BandsTreeImageList.TransparentColor = System.Drawing.Color.Transparent;
			this.BandsTreeImageList.Images.SetKeyName(0, "ReportTitle");
			this.BandsTreeImageList.Images.SetKeyName(1, "ReportSummary");
			this.BandsTreeImageList.Images.SetKeyName(2, "PageHeader");
			this.BandsTreeImageList.Images.SetKeyName(3, "PageFooter");
			this.BandsTreeImageList.Images.SetKeyName(4, "GroupHeader");
			this.BandsTreeImageList.Images.SetKeyName(5, "GroupFooter");
			this.BandsTreeImageList.Images.SetKeyName(6, "Data");
			// 
			// ControlPanel
			// 
			this.ControlPanel.Controls.Add(this.MoveDownBtn);
			this.ControlPanel.Controls.Add(this.MoveUpBtn);
			this.ControlPanel.Controls.Add(this.DeleteBtn);
			this.ControlPanel.Controls.Add(this.CloseBtn);
			this.ControlPanel.Controls.Add(this.AddBtn);
			this.ControlPanel.Dock = System.Windows.Forms.DockStyle.Right;
			this.ControlPanel.Location = new System.Drawing.Point(339, 0);
			this.ControlPanel.Name = "ControlPanel";
			this.ControlPanel.Size = new System.Drawing.Size(95, 362);
			this.ControlPanel.TabIndex = 0;
			// 
			// MoveDownBtn
			// 
			this.MoveDownBtn.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ArrowDown;
			this.MoveDownBtn.Location = new System.Drawing.Point(10, 97);
			this.MoveDownBtn.Name = "MoveDownBtn";
			this.MoveDownBtn.Size = new System.Drawing.Size(23, 23);
			this.MoveDownBtn.TabIndex = 3;
			this.MoveDownBtn.UseVisualStyleBackColor = true;
			this.MoveDownBtn.Click += new System.EventHandler(this.OnMoveDown);
			// 
			// MoveUpBtn
			// 
			this.MoveUpBtn.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ArrowUp;
			this.MoveUpBtn.Location = new System.Drawing.Point(10, 68);
			this.MoveUpBtn.Name = "MoveUpBtn";
			this.MoveUpBtn.Size = new System.Drawing.Size(23, 23);
			this.MoveUpBtn.TabIndex = 2;
			this.MoveUpBtn.UseVisualStyleBackColor = true;
			this.MoveUpBtn.Click += new System.EventHandler(this.OnMoveUp);
			// 
			// DeleteBtn
			// 
			this.DeleteBtn.Location = new System.Drawing.Point(10, 39);
			this.DeleteBtn.Name = "DeleteBtn";
			this.DeleteBtn.Size = new System.Drawing.Size(75, 23);
			this.DeleteBtn.TabIndex = 1;
			this.DeleteBtn.Text = "Delete";
			this.DeleteBtn.UseVisualStyleBackColor = true;
			this.DeleteBtn.Click += new System.EventHandler(this.OnDelete);
			// 
			// CloseBtn
			// 
			this.CloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.CloseBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CloseBtn.Location = new System.Drawing.Point(10, 329);
			this.CloseBtn.Name = "CloseBtn";
			this.CloseBtn.Size = new System.Drawing.Size(75, 23);
			this.CloseBtn.TabIndex = 4;
			this.CloseBtn.Text = "Close";
			this.CloseBtn.UseVisualStyleBackColor = true;
			this.CloseBtn.Click += new System.EventHandler(this.OnClose);
			// 
			// AddBtn
			// 
			this.AddBtn.Location = new System.Drawing.Point(10, 10);
			this.AddBtn.Name = "AddBtn";
			this.AddBtn.Size = new System.Drawing.Size(75, 23);
			this.AddBtn.TabIndex = 0;
			this.AddBtn.Text = "Add";
			this.AddBtn.UseVisualStyleBackColor = true;
			this.AddBtn.Click += new System.EventHandler(this.OnAdd);
			// 
			// ReportBandConfigView
			// 
			this.AcceptButton = this.AddBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.CloseBtn;
			this.ClientSize = new System.Drawing.Size(434, 362);
			this.Controls.Add(this.MainPanel);
			this.Controls.Add(this.ControlPanel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ReportBandConfigView";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Configure Bands";
			this.MainPanel.ResumeLayout(false);
			this.AddButtonMenu.ResumeLayout(false);
			this.ControlPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel MainPanel;
		private System.Windows.Forms.Panel ControlPanel;
		private System.Windows.Forms.Button AddBtn;
		private System.Windows.Forms.ContextMenuStrip AddButtonMenu;
		private System.Windows.Forms.ToolStripMenuItem AddReportTitleBtn;
		private System.Windows.Forms.ToolStripMenuItem AddReportSummaryBtn;
		private System.Windows.Forms.ToolStripMenuItem AddPageHeaderBtn;
		private System.Windows.Forms.Button DeleteBtn;
		private System.Windows.Forms.Button MoveDownBtn;
		private System.Windows.Forms.Button MoveUpBtn;
		private System.Windows.Forms.Button CloseBtn;
		private System.Windows.Forms.ToolStripMenuItem AddPageFooterBtn;
		private System.Windows.Forms.ToolStripMenuItem AddGroupHeaderBtn;
		private System.Windows.Forms.ToolStripMenuItem AddGroupFooterBtn;
		private System.Windows.Forms.ToolStripMenuItem AddDataBandBtn;
		private System.Windows.Forms.ToolStripSeparator Separator1;
		private System.Windows.Forms.ToolStripSeparator Separator2;
		private System.Windows.Forms.TreeView BandsTree;
		private System.Windows.Forms.ImageList BandsTreeImageList;
		private System.Windows.Forms.ToolStripMenuItem DeleteMenuBtn;
		private System.Windows.Forms.ToolStripSeparator Separator0;
	}
}
