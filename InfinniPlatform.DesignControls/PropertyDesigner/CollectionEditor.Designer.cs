namespace InfinniPlatform.DesignControls.PropertyDesigner
{
    partial class CollectionEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CollectionEditor));
			DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
			this.CollectionPropertiesGrid = new DevExpress.XtraGrid.GridControl();
			this.gridBinding = new System.Windows.Forms.BindingSource();
			this.PropertiesView = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.repositoryItemButtonEdit = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
			this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
			this.ButtonDelete = new DevExpress.XtraEditors.SimpleButton();
			this.ButtonAdd = new DevExpress.XtraEditors.SimpleButton();
			((System.ComponentModel.ISupportInitialize)(this.CollectionPropertiesGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridBinding)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PropertiesView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.panelControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// CollectionPropertiesGrid
			// 
			this.CollectionPropertiesGrid.DataSource = this.gridBinding;
			this.CollectionPropertiesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CollectionPropertiesGrid.EmbeddedNavigator.Buttons.Append.Visible = false;
			this.CollectionPropertiesGrid.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
			this.CollectionPropertiesGrid.EmbeddedNavigator.Buttons.Edit.Visible = false;
			this.CollectionPropertiesGrid.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
			this.CollectionPropertiesGrid.EmbeddedNavigator.Buttons.First.Visible = false;
			this.CollectionPropertiesGrid.EmbeddedNavigator.Buttons.Last.Visible = false;
			this.CollectionPropertiesGrid.EmbeddedNavigator.Buttons.Next.Visible = false;
			this.CollectionPropertiesGrid.EmbeddedNavigator.Buttons.NextPage.Visible = false;
			this.CollectionPropertiesGrid.EmbeddedNavigator.Buttons.Prev.Visible = false;
			this.CollectionPropertiesGrid.EmbeddedNavigator.Buttons.PrevPage.Visible = false;
			this.CollectionPropertiesGrid.Location = new System.Drawing.Point(0, 0);
			this.CollectionPropertiesGrid.MainView = this.PropertiesView;
			this.CollectionPropertiesGrid.Name = "CollectionPropertiesGrid";
			this.CollectionPropertiesGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEdit});
			this.CollectionPropertiesGrid.Size = new System.Drawing.Size(446, 189);
			this.CollectionPropertiesGrid.TabIndex = 1;
			this.CollectionPropertiesGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.PropertiesView});
			// 
			// PropertiesView
			// 
			this.PropertiesView.GridControl = this.CollectionPropertiesGrid;
			this.PropertiesView.Name = "PropertiesView";
			this.PropertiesView.NewItemRowText = "Click to add item";
			this.PropertiesView.OptionsCustomization.AllowColumnMoving = false;
			this.PropertiesView.OptionsCustomization.AllowFilter = false;
			this.PropertiesView.OptionsCustomization.AllowGroup = false;
			this.PropertiesView.OptionsCustomization.AllowQuickHideColumns = false;
			this.PropertiesView.OptionsCustomization.AllowSort = false;
			this.PropertiesView.OptionsView.ShowGroupPanel = false;
			this.PropertiesView.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.PropertiesView_CellValueChanged);
			// 
			// repositoryItemButtonEdit
			// 
			this.repositoryItemButtonEdit.AutoHeight = false;
			this.repositoryItemButtonEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repositoryItemButtonEdit.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true),
            new DevExpress.XtraEditors.Controls.EditorButton(),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
			this.repositoryItemButtonEdit.Name = "repositoryItemButtonEdit";
			this.repositoryItemButtonEdit.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repositoryItemButtonEdit_ButtonClick);
			// 
			// panelControl1
			// 
			this.panelControl1.Controls.Add(this.ButtonDelete);
			this.panelControl1.Controls.Add(this.ButtonAdd);
			this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelControl1.Location = new System.Drawing.Point(0, 189);
			this.panelControl1.LookAndFeel.SkinName = "Office 2013";
			this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(446, 31);
			this.panelControl1.TabIndex = 2;
			// 
			// ButtonDelete
			// 
			this.ButtonDelete.Location = new System.Drawing.Point(86, 3);
			this.ButtonDelete.LookAndFeel.SkinName = "Office 2013";
			this.ButtonDelete.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ButtonDelete.Name = "ButtonDelete";
			this.ButtonDelete.Size = new System.Drawing.Size(75, 23);
			this.ButtonDelete.TabIndex = 1;
			this.ButtonDelete.Text = "Delete";
			this.ButtonDelete.Click += new System.EventHandler(this.ButtonDelete_Click);
			// 
			// ButtonAdd
			// 
			this.ButtonAdd.Location = new System.Drawing.Point(5, 3);
			this.ButtonAdd.LookAndFeel.SkinName = "Office 2013";
			this.ButtonAdd.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ButtonAdd.Name = "ButtonAdd";
			this.ButtonAdd.Size = new System.Drawing.Size(75, 23);
			this.ButtonAdd.TabIndex = 0;
			this.ButtonAdd.Text = "Add";
			this.ButtonAdd.Click += new System.EventHandler(this.ButtonAdd_Click);
			// 
			// CollectionEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.CollectionPropertiesGrid);
			this.Controls.Add(this.panelControl1);
			this.Name = "CollectionEditor";
			this.Size = new System.Drawing.Size(446, 220);
			((System.ComponentModel.ISupportInitialize)(this.CollectionPropertiesGrid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridBinding)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PropertiesView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.panelControl1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl CollectionPropertiesGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView PropertiesView;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton ButtonDelete;
        private DevExpress.XtraEditors.SimpleButton ButtonAdd;
        private System.Windows.Forms.BindingSource gridBinding;
		private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit;
    }
}
