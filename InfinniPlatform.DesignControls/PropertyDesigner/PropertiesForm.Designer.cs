namespace InfinniPlatform.DesignControls.PropertyDesigner
{
	partial class PropertiesForm
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
			DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.pageSimpleProperties = new System.Windows.Forms.TabPage();
			this.SimplePropertiesGrid = new DevExpress.XtraVerticalGrid.VGridControl();
			this.repositoryItemButtonEdit = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
			this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
			this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
			this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
			this.CancelButton = new DevExpress.XtraEditors.SimpleButton();
			this.ButtonOK = new DevExpress.XtraEditors.SimpleButton();
			this.tabControl1.SuspendLayout();
			this.pageSimpleProperties.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.SimplePropertiesGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.panelControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.pageSimpleProperties);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(712, 411);
			this.tabControl1.TabIndex = 0;
			// 
			// pageSimpleProperties
			// 
			this.pageSimpleProperties.Controls.Add(this.SimplePropertiesGrid);
			this.pageSimpleProperties.Location = new System.Drawing.Point(4, 22);
			this.pageSimpleProperties.Name = "pageSimpleProperties";
			this.pageSimpleProperties.Padding = new System.Windows.Forms.Padding(3);
			this.pageSimpleProperties.Size = new System.Drawing.Size(704, 385);
			this.pageSimpleProperties.TabIndex = 0;
			this.pageSimpleProperties.Text = "Simple Properties";
			this.pageSimpleProperties.UseVisualStyleBackColor = true;
			// 
			// SimplePropertiesGrid
			// 
			this.SimplePropertiesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SimplePropertiesGrid.Location = new System.Drawing.Point(3, 3);
			this.SimplePropertiesGrid.LookAndFeel.SkinName = "Office 2013";
			this.SimplePropertiesGrid.LookAndFeel.UseDefaultLookAndFeel = false;
			this.SimplePropertiesGrid.Name = "SimplePropertiesGrid";
			this.SimplePropertiesGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEdit,
            this.repositoryItemLookUpEdit1,
            this.repositoryItemButtonEdit1});
			this.SimplePropertiesGrid.Size = new System.Drawing.Size(698, 379);
			this.SimplePropertiesGrid.TabIndex = 0;
			this.SimplePropertiesGrid.CustomDrawRowValueCell += new DevExpress.XtraVerticalGrid.Events.CustomDrawRowValueCellEventHandler(this.SimplePropertiesGrid_CustomDrawRowValueCell);
			// 
			// repositoryItemButtonEdit
			// 
			this.repositoryItemButtonEdit.AutoHeight = false;
			this.repositoryItemButtonEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::InfinniPlatform.DesignControls.Properties.Resources.zoom_16x16, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true),
            new DevExpress.XtraEditors.Controls.EditorButton(),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::InfinniPlatform.DesignControls.Properties.Resources.close_16x16, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", "RemoveProperty", null, true)});
			this.repositoryItemButtonEdit.LookAndFeel.SkinName = "Office 2013";
			this.repositoryItemButtonEdit.LookAndFeel.UseDefaultLookAndFeel = false;
			this.repositoryItemButtonEdit.Name = "repositoryItemButtonEdit";
			this.repositoryItemButtonEdit.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repositoryItemButtonEdit_ButtonClick);
			// 
			// repositoryItemLookUpEdit1
			// 
			this.repositoryItemLookUpEdit1.AutoHeight = false;
			this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::InfinniPlatform.DesignControls.Properties.Resources.close_16x16, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", "RemoveProperty", null, true)});
			this.repositoryItemLookUpEdit1.LookAndFeel.SkinName = "Office 2013";
			this.repositoryItemLookUpEdit1.LookAndFeel.UseDefaultLookAndFeel = false;
			this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
			// 
			// repositoryItemButtonEdit1
			// 
			this.repositoryItemButtonEdit1.AutoHeight = false;
			this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::InfinniPlatform.DesignControls.Properties.Resources.close_16x16, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject4, "", "RemoveProperty", null, true)});
			this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
			this.repositoryItemButtonEdit1.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repositoryItemButtonEdit_RemoveClick);
			// 
			// panelControl1
			// 
			this.panelControl1.Controls.Add(this.CancelButton);
			this.panelControl1.Controls.Add(this.ButtonOK);
			this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelControl1.Location = new System.Drawing.Point(0, 411);
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(712, 35);
			this.panelControl1.TabIndex = 2;
			// 
			// CancelButton
			// 
			this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelButton.Location = new System.Drawing.Point(625, 7);
			this.CancelButton.LookAndFeel.SkinName = "Office 2013";
			this.CancelButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(75, 23);
			this.CancelButton.TabIndex = 1;
			this.CancelButton.Text = "Cancel";
			// 
			// ButtonOK
			// 
			this.ButtonOK.Location = new System.Drawing.Point(540, 7);
			this.ButtonOK.LookAndFeel.SkinName = "Office 2013";
			this.ButtonOK.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ButtonOK.Name = "ButtonOK";
			this.ButtonOK.Size = new System.Drawing.Size(75, 23);
			this.ButtonOK.TabIndex = 0;
			this.ButtonOK.Text = "OK";
			this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
			// 
			// PropertiesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(712, 446);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.panelControl1);
			this.Name = "PropertiesForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Properties editor";
			this.tabControl1.ResumeLayout(false);
			this.pageSimpleProperties.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.SimplePropertiesGrid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.panelControl1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage pageSimpleProperties;
		private DevExpress.XtraVerticalGrid.VGridControl SimplePropertiesGrid;
		private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton ButtonOK;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit;
		private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
		private DevExpress.XtraEditors.SimpleButton CancelButton;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;

	}
}
