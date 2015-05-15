namespace InfinniPlatform.DesignControls.ObjectInspector
{
	partial class ObjectInspectorTree
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjectInspectorTree));
			this.barManager = new DevExpress.XtraBars.BarManager();
			this.bar = new DevExpress.XtraBars.Bar();
			this.SettingsButton = new DevExpress.XtraBars.BarButtonItem();
			this.ButtonCopy = new DevExpress.XtraBars.BarButtonItem();
			this.ButtonCut = new DevExpress.XtraBars.BarButtonItem();
			this.ButtonPaste = new DevExpress.XtraBars.BarButtonItem();
			this.ButtonMoveUp = new DevExpress.XtraBars.BarButtonItem();
			this.ButtonMoveDown = new DevExpress.XtraBars.BarButtonItem();
			this.ButtonDelete = new DevExpress.XtraBars.BarButtonItem();
			this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
			this.ControlsTree = new DevExpress.XtraTreeList.TreeList();
			this.ControlColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
			((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ControlsTree)).BeginInit();
			this.SuspendLayout();
			// 
			// barManager
			// 
			this.barManager.AllowCustomization = false;
			this.barManager.AllowMoveBarOnToolbar = false;
			this.barManager.AllowQuickCustomization = false;
			this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar});
			this.barManager.DockControls.Add(this.barDockControlTop);
			this.barManager.DockControls.Add(this.barDockControlBottom);
			this.barManager.DockControls.Add(this.barDockControlLeft);
			this.barManager.DockControls.Add(this.barDockControlRight);
			this.barManager.Form = this;
			this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.SettingsButton,
            this.ButtonCut,
            this.ButtonPaste,
            this.ButtonMoveUp,
            this.ButtonMoveDown,
            this.ButtonDelete,
            this.ButtonCopy});
			this.barManager.MaxItemId = 8;
			// 
			// bar
			// 
			this.bar.BarName = "Tools";
			this.bar.DockCol = 0;
			this.bar.DockRow = 0;
			this.bar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
			this.bar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.SettingsButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.ButtonCopy, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.ButtonCut),
            new DevExpress.XtraBars.LinkPersistInfo(this.ButtonPaste),
            new DevExpress.XtraBars.LinkPersistInfo(this.ButtonMoveUp, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.ButtonMoveDown),
            new DevExpress.XtraBars.LinkPersistInfo(this.ButtonDelete, true)});
			this.bar.OptionsBar.AllowQuickCustomization = false;
			this.bar.OptionsBar.DisableClose = true;
			this.bar.OptionsBar.DisableCustomization = true;
			this.bar.OptionsBar.DrawBorder = false;
			this.bar.OptionsBar.DrawDragBorder = false;
			this.bar.OptionsBar.UseWholeRow = true;
			this.bar.Text = "Tools";
			// 
			// SettingsButton
			// 
			this.SettingsButton.Caption = "Properties";
			this.SettingsButton.Glyph = ((System.Drawing.Image)(resources.GetObject("SettingsButton.Glyph")));
			this.SettingsButton.Id = 1;
			this.SettingsButton.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("SettingsButton.LargeGlyph")));
			this.SettingsButton.Name = "SettingsButton";
			this.SettingsButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.SettingsButton_ItemClick);
			// 
			// ButtonCopy
			// 
			this.ButtonCopy.Caption = "Copy";
			this.ButtonCopy.Glyph = ((System.Drawing.Image)(resources.GetObject("ButtonCopy.Glyph")));
			this.ButtonCopy.Id = 7;
			this.ButtonCopy.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("ButtonCopy.LargeGlyph")));
			this.ButtonCopy.Name = "ButtonCopy";
			this.ButtonCopy.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonCopy_ItemClick);
			// 
			// ButtonCut
			// 
			this.ButtonCut.Caption = "Cut";
			this.ButtonCut.Glyph = ((System.Drawing.Image)(resources.GetObject("ButtonCut.Glyph")));
			this.ButtonCut.Id = 2;
			this.ButtonCut.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("ButtonCut.LargeGlyph")));
			this.ButtonCut.Name = "ButtonCut";
			this.ButtonCut.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonCut_ItemClick);
			// 
			// ButtonPaste
			// 
			this.ButtonPaste.Caption = "Paste";
			this.ButtonPaste.Enabled = false;
			this.ButtonPaste.Glyph = ((System.Drawing.Image)(resources.GetObject("ButtonPaste.Glyph")));
			this.ButtonPaste.Id = 3;
			this.ButtonPaste.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("ButtonPaste.LargeGlyph")));
			this.ButtonPaste.Name = "ButtonPaste";
			this.ButtonPaste.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonPaste_ItemClick);
			// 
			// ButtonMoveUp
			// 
			this.ButtonMoveUp.Caption = "barButtonItem1";
			this.ButtonMoveUp.Glyph = ((System.Drawing.Image)(resources.GetObject("ButtonMoveUp.Glyph")));
			this.ButtonMoveUp.Id = 4;
			this.ButtonMoveUp.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("ButtonMoveUp.LargeGlyph")));
			this.ButtonMoveUp.Name = "ButtonMoveUp";
			// 
			// ButtonMoveDown
			// 
			this.ButtonMoveDown.Caption = "barButtonItem1";
			this.ButtonMoveDown.Glyph = ((System.Drawing.Image)(resources.GetObject("ButtonMoveDown.Glyph")));
			this.ButtonMoveDown.Id = 5;
			this.ButtonMoveDown.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("ButtonMoveDown.LargeGlyph")));
			this.ButtonMoveDown.Name = "ButtonMoveDown";
			// 
			// ButtonDelete
			// 
			this.ButtonDelete.Caption = "Delete";
			this.ButtonDelete.Glyph = ((System.Drawing.Image)(resources.GetObject("ButtonDelete.Glyph")));
			this.ButtonDelete.Id = 6;
			this.ButtonDelete.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("ButtonDelete.LargeGlyph")));
			this.ButtonDelete.Name = "ButtonDelete";
			this.ButtonDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonDelete_ItemClick);
			// 
			// barDockControlTop
			// 
			this.barDockControlTop.CausesValidation = false;
			this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
			this.barDockControlTop.Size = new System.Drawing.Size(298, 31);
			// 
			// barDockControlBottom
			// 
			this.barDockControlBottom.CausesValidation = false;
			this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.barDockControlBottom.Location = new System.Drawing.Point(0, 518);
			this.barDockControlBottom.Size = new System.Drawing.Size(298, 0);
			// 
			// barDockControlLeft
			// 
			this.barDockControlLeft.CausesValidation = false;
			this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.barDockControlLeft.Location = new System.Drawing.Point(0, 31);
			this.barDockControlLeft.Size = new System.Drawing.Size(0, 487);
			// 
			// barDockControlRight
			// 
			this.barDockControlRight.CausesValidation = false;
			this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.barDockControlRight.Location = new System.Drawing.Point(298, 31);
			this.barDockControlRight.Size = new System.Drawing.Size(0, 487);
			// 
			// ControlsTree
			// 
			this.ControlsTree.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.ControlColumn});
			this.ControlsTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ControlsTree.Location = new System.Drawing.Point(0, 31);
			this.ControlsTree.Name = "ControlsTree";
			this.ControlsTree.BeginUnboundLoad();
			this.ControlsTree.AppendNode(new object[] {
            "Form"}, -1);
			this.ControlsTree.EndUnboundLoad();
			this.ControlsTree.OptionsBehavior.Editable = false;
			this.ControlsTree.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.ControlsTree.OptionsView.ShowIndicator = false;
			this.ControlsTree.Size = new System.Drawing.Size(298, 487);
			this.ControlsTree.TabIndex = 4;
			this.ControlsTree.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.ControlsTree_focusedNodeChanged);
			this.ControlsTree.Click += new System.EventHandler(this.ControlsTree_Click);
			this.ControlsTree.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ControlsTree_mouseUp);
			// 
			// ControlColumn
			// 
			this.ControlColumn.Caption = "Control name";
			this.ControlColumn.FieldName = "ControlName";
			this.ControlColumn.MinWidth = 34;
			this.ControlColumn.Name = "ControlColumn";
			this.ControlColumn.Visible = true;
			this.ControlColumn.VisibleIndex = 0;
			this.ControlColumn.Width = 93;
			// 
			// ObjectInspectorTree
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ControlsTree);
			this.Controls.Add(this.barDockControlLeft);
			this.Controls.Add(this.barDockControlRight);
			this.Controls.Add(this.barDockControlBottom);
			this.Controls.Add(this.barDockControlTop);
			this.Name = "ObjectInspectorTree";
			this.Size = new System.Drawing.Size(298, 518);
			((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ControlsTree)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraBars.BarManager barManager;
		private DevExpress.XtraBars.Bar bar;
		private DevExpress.XtraBars.BarDockControl barDockControlTop;
		private DevExpress.XtraBars.BarDockControl barDockControlBottom;
		private DevExpress.XtraBars.BarDockControl barDockControlLeft;
		private DevExpress.XtraBars.BarDockControl barDockControlRight;
		private DevExpress.XtraBars.BarButtonItem SettingsButton;
		private DevExpress.XtraBars.BarButtonItem ButtonCut;
		private DevExpress.XtraBars.BarButtonItem ButtonPaste;
		private DevExpress.XtraBars.BarButtonItem ButtonMoveUp;
		private DevExpress.XtraBars.BarButtonItem ButtonMoveDown;
		private DevExpress.XtraTreeList.TreeList ControlsTree;
		private DevExpress.XtraTreeList.Columns.TreeListColumn ControlColumn;
		private DevExpress.XtraBars.BarButtonItem ButtonDelete;
		private DevExpress.XtraBars.BarButtonItem ButtonCopy;
	}
}