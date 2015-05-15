namespace InfinniPlatform.DesignControls.DesignerSurface
{
    partial class ParameterSurface
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
			this.GridControlParameterrs = new DevExpress.XtraGrid.GridControl();
			this.gridBinding = new System.Windows.Forms.BindingSource(this.components);
			this.GridViewParameters = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.ParameterColumn = new DevExpress.XtraGrid.Columns.GridColumn();
			this.repositoryItemButtonEditSource = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
			this.PanelButtons = new DevExpress.XtraEditors.PanelControl();
			this.GEtLayoutButton = new DevExpress.XtraEditors.SimpleButton();
			this.DeleteScriptButton = new DevExpress.XtraEditors.SimpleButton();
			this.AddScriptButton = new DevExpress.XtraEditors.SimpleButton();
			((System.ComponentModel.ISupportInitialize)(this.GridControlParameterrs)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridBinding)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.GridViewParameters)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEditSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PanelButtons)).BeginInit();
			this.PanelButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// GridControlParameterrs
			// 
			this.GridControlParameterrs.DataSource = this.gridBinding;
			this.GridControlParameterrs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GridControlParameterrs.Location = new System.Drawing.Point(0, 0);
			this.GridControlParameterrs.MainView = this.GridViewParameters;
			this.GridControlParameterrs.Name = "GridControlParameterrs";
			this.GridControlParameterrs.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEditSource});
			this.GridControlParameterrs.Size = new System.Drawing.Size(610, 313);
			this.GridControlParameterrs.TabIndex = 5;
			this.GridControlParameterrs.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridViewParameters});
			// 
			// GridViewParameters
			// 
			this.GridViewParameters.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ParameterColumn});
			this.GridViewParameters.GridControl = this.GridControlParameterrs;
			this.GridViewParameters.Name = "GridViewParameters";
			this.GridViewParameters.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
			this.GridViewParameters.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
			this.GridViewParameters.OptionsCustomization.AllowColumnMoving = false;
			this.GridViewParameters.OptionsCustomization.AllowColumnResizing = false;
			this.GridViewParameters.OptionsCustomization.AllowFilter = false;
			this.GridViewParameters.OptionsCustomization.AllowGroup = false;
			this.GridViewParameters.OptionsCustomization.AllowQuickHideColumns = false;
			this.GridViewParameters.OptionsCustomization.AllowSort = false;
			this.GridViewParameters.OptionsView.ShowGroupPanel = false;
			this.GridViewParameters.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.GridViewScripts_CustomColumnDisplayText);
			// 
			// ParameterColumn
			// 
			this.ParameterColumn.Caption = "Parameter";
			this.ParameterColumn.ColumnEdit = this.repositoryItemButtonEditSource;
			this.ParameterColumn.FieldName = "ParameterName";
			this.ParameterColumn.Name = "ParameterColumn";
			this.ParameterColumn.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
			this.ParameterColumn.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
			this.ParameterColumn.Visible = true;
			this.ParameterColumn.VisibleIndex = 0;
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
			// PanelButtons
			// 
			this.PanelButtons.Controls.Add(this.GEtLayoutButton);
			this.PanelButtons.Controls.Add(this.DeleteScriptButton);
			this.PanelButtons.Controls.Add(this.AddScriptButton);
			this.PanelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.PanelButtons.Location = new System.Drawing.Point(0, 313);
			this.PanelButtons.LookAndFeel.SkinName = "Office 2013";
			this.PanelButtons.LookAndFeel.UseDefaultLookAndFeel = false;
			this.PanelButtons.Name = "PanelButtons";
			this.PanelButtons.Size = new System.Drawing.Size(610, 34);
			this.PanelButtons.TabIndex = 4;
			// 
			// GEtLayoutButton
			// 
			this.GEtLayoutButton.Location = new System.Drawing.Point(246, 6);
			this.GEtLayoutButton.LookAndFeel.SkinName = "Office 2013";
			this.GEtLayoutButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.GEtLayoutButton.Name = "GEtLayoutButton";
			this.GEtLayoutButton.Size = new System.Drawing.Size(109, 23);
			this.GEtLayoutButton.TabIndex = 2;
			this.GEtLayoutButton.Text = "Get Layout";
			this.GEtLayoutButton.Click += new System.EventHandler(this.GEtLayoutButton_Click);
			// 
			// DeleteScriptButton
			// 
			this.DeleteScriptButton.Location = new System.Drawing.Point(120, 6);
			this.DeleteScriptButton.LookAndFeel.SkinName = "Office 2013";
			this.DeleteScriptButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.DeleteScriptButton.Name = "DeleteScriptButton";
			this.DeleteScriptButton.Size = new System.Drawing.Size(109, 23);
			this.DeleteScriptButton.TabIndex = 1;
			this.DeleteScriptButton.Text = "Delete Parameter";
			this.DeleteScriptButton.Click += new System.EventHandler(this.DeleteScriptButton_Click);
			// 
			// AddScriptButton
			// 
			this.AddScriptButton.Location = new System.Drawing.Point(5, 6);
			this.AddScriptButton.LookAndFeel.SkinName = "Office 2013";
			this.AddScriptButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.AddScriptButton.Name = "AddScriptButton";
			this.AddScriptButton.Size = new System.Drawing.Size(109, 23);
			this.AddScriptButton.TabIndex = 0;
			this.AddScriptButton.Text = "Add Parameter";
			this.AddScriptButton.Click += new System.EventHandler(this.AddScriptButton_Click);
			// 
			// ParameterSurface
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.GridControlParameterrs);
			this.Controls.Add(this.PanelButtons);
			this.Name = "ParameterSurface";
			this.Size = new System.Drawing.Size(610, 347);
			((System.ComponentModel.ISupportInitialize)(this.GridControlParameterrs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridBinding)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.GridViewParameters)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEditSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PanelButtons)).EndInit();
			this.PanelButtons.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl GridControlParameterrs;
        private DevExpress.XtraGrid.Views.Grid.GridView GridViewParameters;
        private DevExpress.XtraGrid.Columns.GridColumn ParameterColumn;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEditSource;
        private DevExpress.XtraEditors.PanelControl PanelButtons;
        private DevExpress.XtraEditors.SimpleButton GEtLayoutButton;
        private DevExpress.XtraEditors.SimpleButton DeleteScriptButton;
        private DevExpress.XtraEditors.SimpleButton AddScriptButton;
        private System.Windows.Forms.BindingSource gridBinding;
    }
}
