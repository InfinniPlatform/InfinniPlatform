namespace InfinniPlatform.DesignControls.DesignerSurface
{
    partial class DataSourceSurface
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
			this.PanelListSource = new DevExpress.XtraEditors.PanelControl();
			this.AddObjectDataSourceButton = new DevExpress.XtraEditors.SimpleButton();
			this.SetLayoutButton = new DevExpress.XtraEditors.SimpleButton();
			this.GetLayoutButton = new DevExpress.XtraEditors.SimpleButton();
			this.DeleteDataSourceButton = new DevExpress.XtraEditors.SimpleButton();
			this.AddDocumentDataSourceButton = new DevExpress.XtraEditors.SimpleButton();
			this.GridControlDataSources = new DevExpress.XtraGrid.GridControl();
			this.gridBinding = new System.Windows.Forms.BindingSource(this.components);
			this.GridViewDataSources = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.DataSourceColumn = new DevExpress.XtraGrid.Columns.GridColumn();
			this.repositoryItemButtonEditSource = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
			((System.ComponentModel.ISupportInitialize)(this.PanelListSource)).BeginInit();
			this.PanelListSource.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.GridControlDataSources)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridBinding)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.GridViewDataSources)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEditSource)).BeginInit();
			this.SuspendLayout();
			// 
			// PanelListSource
			// 
			this.PanelListSource.Controls.Add(this.AddObjectDataSourceButton);
			this.PanelListSource.Controls.Add(this.SetLayoutButton);
			this.PanelListSource.Controls.Add(this.GetLayoutButton);
			this.PanelListSource.Controls.Add(this.DeleteDataSourceButton);
			this.PanelListSource.Controls.Add(this.AddDocumentDataSourceButton);
			this.PanelListSource.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.PanelListSource.Location = new System.Drawing.Point(0, 368);
			this.PanelListSource.LookAndFeel.SkinName = "Office 2013";
			this.PanelListSource.LookAndFeel.UseDefaultLookAndFeel = false;
			this.PanelListSource.Name = "PanelListSource";
			this.PanelListSource.Size = new System.Drawing.Size(680, 34);
			this.PanelListSource.TabIndex = 1;
			// 
			// AddObjectDataSourceButton
			// 
			this.AddObjectDataSourceButton.Location = new System.Drawing.Point(158, 6);
			this.AddObjectDataSourceButton.LookAndFeel.SkinName = "Office 2013";
			this.AddObjectDataSourceButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.AddObjectDataSourceButton.Name = "AddObjectDataSourceButton";
			this.AddObjectDataSourceButton.Size = new System.Drawing.Size(147, 23);
			this.AddObjectDataSourceButton.TabIndex = 4;
			this.AddObjectDataSourceButton.Text = "Add ObjectDataSource";
			this.AddObjectDataSourceButton.Click += new System.EventHandler(this.AddObjectDataSourceButton_Click);
			// 
			// SetLayoutButton
			// 
			this.SetLayoutButton.Location = new System.Drawing.Point(602, 6);
			this.SetLayoutButton.LookAndFeel.SkinName = "Office 2013";
			this.SetLayoutButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.SetLayoutButton.Name = "SetLayoutButton";
			this.SetLayoutButton.Size = new System.Drawing.Size(75, 23);
			this.SetLayoutButton.TabIndex = 3;
			this.SetLayoutButton.Text = "Set Layout";
			this.SetLayoutButton.Click += new System.EventHandler(this.SetLayoutButton_Click);
			// 
			// GetLayoutButton
			// 
			this.GetLayoutButton.Location = new System.Drawing.Point(521, 6);
			this.GetLayoutButton.LookAndFeel.SkinName = "Office 2013";
			this.GetLayoutButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.GetLayoutButton.Name = "GetLayoutButton";
			this.GetLayoutButton.Size = new System.Drawing.Size(75, 23);
			this.GetLayoutButton.TabIndex = 2;
			this.GetLayoutButton.Text = "Get Layout";
			this.GetLayoutButton.Click += new System.EventHandler(this.GetLayoutButton_Click);
			// 
			// DeleteDataSourceButton
			// 
			this.DeleteDataSourceButton.Location = new System.Drawing.Point(324, 6);
			this.DeleteDataSourceButton.LookAndFeel.SkinName = "Office 2013";
			this.DeleteDataSourceButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.DeleteDataSourceButton.Name = "DeleteDataSourceButton";
			this.DeleteDataSourceButton.Size = new System.Drawing.Size(112, 23);
			this.DeleteDataSourceButton.TabIndex = 1;
			this.DeleteDataSourceButton.Text = "Delete DataSource";
			this.DeleteDataSourceButton.Click += new System.EventHandler(this.DeleteDataSourceButton_Click);
			// 
			// AddDocumentDataSourceButton
			// 
			this.AddDocumentDataSourceButton.Location = new System.Drawing.Point(5, 6);
			this.AddDocumentDataSourceButton.LookAndFeel.SkinName = "Office 2013";
			this.AddDocumentDataSourceButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.AddDocumentDataSourceButton.Name = "AddDocumentDataSourceButton";
			this.AddDocumentDataSourceButton.Size = new System.Drawing.Size(147, 23);
			this.AddDocumentDataSourceButton.TabIndex = 0;
			this.AddDocumentDataSourceButton.Text = "Add DocumentDataSource";
			this.AddDocumentDataSourceButton.Click += new System.EventHandler(this.AddDocumentDataSourceButton_Click);
			// 
			// GridControlDataSources
			// 
			this.GridControlDataSources.DataSource = this.gridBinding;
			this.GridControlDataSources.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GridControlDataSources.Location = new System.Drawing.Point(0, 0);
			this.GridControlDataSources.MainView = this.GridViewDataSources;
			this.GridControlDataSources.Name = "GridControlDataSources";
			this.GridControlDataSources.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEditSource});
			this.GridControlDataSources.Size = new System.Drawing.Size(680, 368);
			this.GridControlDataSources.TabIndex = 2;
			this.GridControlDataSources.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridViewDataSources});
			// 
			// GridViewDataSources
			// 
			this.GridViewDataSources.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.DataSourceColumn});
			this.GridViewDataSources.GridControl = this.GridControlDataSources;
			this.GridViewDataSources.Name = "GridViewDataSources";
			this.GridViewDataSources.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
			this.GridViewDataSources.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
			this.GridViewDataSources.OptionsCustomization.AllowColumnMoving = false;
			this.GridViewDataSources.OptionsCustomization.AllowColumnResizing = false;
			this.GridViewDataSources.OptionsCustomization.AllowFilter = false;
			this.GridViewDataSources.OptionsCustomization.AllowGroup = false;
			this.GridViewDataSources.OptionsCustomization.AllowQuickHideColumns = false;
			this.GridViewDataSources.OptionsCustomization.AllowSort = false;
			this.GridViewDataSources.OptionsView.ShowGroupPanel = false;
			this.GridViewDataSources.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.GridViewDataSources_CustomColumnDisplayText);
			// 
			// DataSourceColumn
			// 
			this.DataSourceColumn.Caption = "Data Source";
			this.DataSourceColumn.ColumnEdit = this.repositoryItemButtonEditSource;
			this.DataSourceColumn.FieldName = "DataSourceString";
			this.DataSourceColumn.Name = "DataSourceColumn";
			this.DataSourceColumn.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
			this.DataSourceColumn.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
			this.DataSourceColumn.Visible = true;
			this.DataSourceColumn.VisibleIndex = 0;
			// 
			// repositoryItemButtonEditSource
			// 
			this.repositoryItemButtonEditSource.AutoHeight = false;
			this.repositoryItemButtonEditSource.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.repositoryItemButtonEditSource.Name = "repositoryItemButtonEditSource";
			this.repositoryItemButtonEditSource.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repositoryItemButtonEditSource_ButtonClick);
			// 
			// DataSourceSurface
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.GridControlDataSources);
			this.Controls.Add(this.PanelListSource);
			this.Name = "DataSourceSurface";
			this.Size = new System.Drawing.Size(680, 402);
			((System.ComponentModel.ISupportInitialize)(this.PanelListSource)).EndInit();
			this.PanelListSource.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.GridControlDataSources)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridBinding)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.GridViewDataSources)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEditSource)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl PanelListSource;
        private DevExpress.XtraEditors.SimpleButton DeleteDataSourceButton;
        private DevExpress.XtraEditors.SimpleButton AddDocumentDataSourceButton;
        private DevExpress.XtraGrid.GridControl GridControlDataSources;
        private DevExpress.XtraGrid.Views.Grid.GridView GridViewDataSources;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEditSource;
        private System.Windows.Forms.BindingSource gridBinding;
        private DevExpress.XtraEditors.SimpleButton GetLayoutButton;
        private DevExpress.XtraGrid.Columns.GridColumn DataSourceColumn;
        private DevExpress.XtraEditors.SimpleButton SetLayoutButton;
		private DevExpress.XtraEditors.SimpleButton AddObjectDataSourceButton;
    }
}
