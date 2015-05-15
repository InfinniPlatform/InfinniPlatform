namespace InfinniPlatform.DesignControls.DesignerSurface
{
	partial class ChildViewSurface
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
			this.GridControlChildView = new DevExpress.XtraGrid.GridControl();
			this.gridBinding = new System.Windows.Forms.BindingSource(this.components);
			this.GridViewChildViews = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.ChildViewColumn = new DevExpress.XtraGrid.Columns.GridColumn();
			this.repositoryItemButtonEditSource = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
			this.PanelListSource = new DevExpress.XtraEditors.PanelControl();
			this.SetLayoutButton = new DevExpress.XtraEditors.SimpleButton();
			this.GetLayoutButton = new DevExpress.XtraEditors.SimpleButton();
			this.DeleteDataSourceButton = new DevExpress.XtraEditors.SimpleButton();
			this.AddDocumentDataSourceButton = new DevExpress.XtraEditors.SimpleButton();
			((System.ComponentModel.ISupportInitialize)(this.GridControlChildView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridBinding)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.GridViewChildViews)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEditSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PanelListSource)).BeginInit();
			this.PanelListSource.SuspendLayout();
			this.SuspendLayout();
			// 
			// GridControlChildView
			// 
			this.GridControlChildView.DataSource = this.gridBinding;
			this.GridControlChildView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GridControlChildView.Location = new System.Drawing.Point(0, 0);
			this.GridControlChildView.MainView = this.GridViewChildViews;
			this.GridControlChildView.Name = "GridControlChildView";
			this.GridControlChildView.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEditSource});
			this.GridControlChildView.Size = new System.Drawing.Size(610, 310);
			this.GridControlChildView.TabIndex = 3;
			this.GridControlChildView.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridViewChildViews});
			// 
			// GridViewChildViews
			// 
			this.GridViewChildViews.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ChildViewColumn});
			this.GridViewChildViews.GridControl = this.GridControlChildView;
			this.GridViewChildViews.Name = "GridViewChildViews";
			this.GridViewChildViews.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
			this.GridViewChildViews.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
			this.GridViewChildViews.OptionsCustomization.AllowColumnMoving = false;
			this.GridViewChildViews.OptionsCustomization.AllowColumnResizing = false;
			this.GridViewChildViews.OptionsCustomization.AllowFilter = false;
			this.GridViewChildViews.OptionsCustomization.AllowGroup = false;
			this.GridViewChildViews.OptionsCustomization.AllowQuickHideColumns = false;
			this.GridViewChildViews.OptionsCustomization.AllowRowSizing = true;
			this.GridViewChildViews.OptionsView.ShowGroupPanel = false;
			this.GridViewChildViews.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.GridViewCustomColumnDisplayText);
			// 
			// ChildViewColumn
			// 
			this.ChildViewColumn.Caption = "Child View";
			this.ChildViewColumn.ColumnEdit = this.repositoryItemButtonEditSource;
			this.ChildViewColumn.FieldName = "ChildViewString";
			this.ChildViewColumn.Name = "ChildViewColumn";
			this.ChildViewColumn.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
			this.ChildViewColumn.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
			this.ChildViewColumn.Visible = true;
			this.ChildViewColumn.VisibleIndex = 0;
			// 
			// repositoryItemButtonEditSource
			// 
			this.repositoryItemButtonEditSource.AutoHeight = false;
			this.repositoryItemButtonEditSource.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.repositoryItemButtonEditSource.LookAndFeel.SkinName = "Office 2013";
			this.repositoryItemButtonEditSource.LookAndFeel.UseDefaultLookAndFeel = false;
			this.repositoryItemButtonEditSource.Name = "repositoryItemButtonEditSource";
			this.repositoryItemButtonEditSource.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repositoryItemButtonEditSource_ButtonClick);
			// 
			// PanelListSource
			// 
			this.PanelListSource.Controls.Add(this.SetLayoutButton);
			this.PanelListSource.Controls.Add(this.GetLayoutButton);
			this.PanelListSource.Controls.Add(this.DeleteDataSourceButton);
			this.PanelListSource.Controls.Add(this.AddDocumentDataSourceButton);
			this.PanelListSource.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.PanelListSource.Location = new System.Drawing.Point(0, 310);
			this.PanelListSource.LookAndFeel.SkinName = "Office 2013";
			this.PanelListSource.LookAndFeel.UseDefaultLookAndFeel = false;
			this.PanelListSource.Name = "PanelListSource";
			this.PanelListSource.Size = new System.Drawing.Size(610, 34);
			this.PanelListSource.TabIndex = 4;
			// 
			// SetLayoutButton
			// 
			this.SetLayoutButton.Location = new System.Drawing.Point(376, 6);
			this.SetLayoutButton.LookAndFeel.SkinName = "Office 2013";
			this.SetLayoutButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.SetLayoutButton.Name = "SetLayoutButton";
			this.SetLayoutButton.Size = new System.Drawing.Size(75, 23);
			this.SetLayoutButton.TabIndex = 3;
			this.SetLayoutButton.Text = "Set Layout";
			this.SetLayoutButton.Click += new System.EventHandler(this.SetLayoutButtonClick);
			// 
			// GetLayoutButton
			// 
			this.GetLayoutButton.Location = new System.Drawing.Point(295, 6);
			this.GetLayoutButton.LookAndFeel.SkinName = "Office 2013";
			this.GetLayoutButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.GetLayoutButton.Name = "GetLayoutButton";
			this.GetLayoutButton.Size = new System.Drawing.Size(75, 23);
			this.GetLayoutButton.TabIndex = 2;
			this.GetLayoutButton.Text = "Get Layout";
			this.GetLayoutButton.Click += new System.EventHandler(this.GetLayoutButtonClick);
			// 
			// DeleteDataSourceButton
			// 
			this.DeleteDataSourceButton.Location = new System.Drawing.Point(158, 6);
			this.DeleteDataSourceButton.LookAndFeel.SkinName = "Office 2013";
			this.DeleteDataSourceButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.DeleteDataSourceButton.Name = "DeleteDataSourceButton";
			this.DeleteDataSourceButton.Size = new System.Drawing.Size(112, 23);
			this.DeleteDataSourceButton.TabIndex = 1;
			this.DeleteDataSourceButton.Text = "Delete";
			this.DeleteDataSourceButton.Click += new System.EventHandler(this.DeleteChildViewButtonClick);
			// 
			// AddDocumentDataSourceButton
			// 
			this.AddDocumentDataSourceButton.Location = new System.Drawing.Point(5, 6);
			this.AddDocumentDataSourceButton.LookAndFeel.SkinName = "Office 2013";
			this.AddDocumentDataSourceButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.AddDocumentDataSourceButton.Name = "AddDocumentDataSourceButton";
			this.AddDocumentDataSourceButton.Size = new System.Drawing.Size(147, 23);
			this.AddDocumentDataSourceButton.TabIndex = 0;
			this.AddDocumentDataSourceButton.Text = "Add ChildView";
			this.AddDocumentDataSourceButton.Click += new System.EventHandler(this.AddChildViewButtonClick);
			// 
			// ChildViewSurface
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.GridControlChildView);
			this.Controls.Add(this.PanelListSource);
			this.Name = "ChildViewSurface";
			this.Size = new System.Drawing.Size(610, 344);
			((System.ComponentModel.ISupportInitialize)(this.GridControlChildView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridBinding)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.GridViewChildViews)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEditSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PanelListSource)).EndInit();
			this.PanelListSource.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraGrid.GridControl GridControlChildView;
		private DevExpress.XtraGrid.Views.Grid.GridView GridViewChildViews;
		private DevExpress.XtraGrid.Columns.GridColumn ChildViewColumn;
		private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEditSource;
		private DevExpress.XtraEditors.PanelControl PanelListSource;
		private DevExpress.XtraEditors.SimpleButton SetLayoutButton;
		private DevExpress.XtraEditors.SimpleButton GetLayoutButton;
		private DevExpress.XtraEditors.SimpleButton DeleteDataSourceButton;
		private DevExpress.XtraEditors.SimpleButton AddDocumentDataSourceButton;
		private System.Windows.Forms.BindingSource gridBinding;
	}
}
