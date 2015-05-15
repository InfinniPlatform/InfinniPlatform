namespace InfinniPlatform.DesignControls.Controls.ActionElements
{
    partial class MenuBar
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
			this.ButtonSelectMenu = new DevExpress.XtraEditors.ButtonEdit();
			this.TreeListMenu = new DevExpress.XtraTreeList.TreeList();
			((System.ComponentModel.ISupportInitialize)(this.ButtonSelectMenu.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TreeListMenu)).BeginInit();
			this.SuspendLayout();
			// 
			// ButtonSelectMenu
			// 
			this.ButtonSelectMenu.Dock = System.Windows.Forms.DockStyle.Top;
			this.ButtonSelectMenu.Location = new System.Drawing.Point(5, 5);
			this.ButtonSelectMenu.Name = "ButtonSelectMenu";
			this.ButtonSelectMenu.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown)});
			this.ButtonSelectMenu.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
			this.ButtonSelectMenu.Size = new System.Drawing.Size(282, 20);
			this.ButtonSelectMenu.TabIndex = 0;
			// 
			// TreeListMenu
			// 
			this.TreeListMenu.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TreeListMenu.Location = new System.Drawing.Point(5, 25);
			this.TreeListMenu.Name = "TreeListMenu";
			this.TreeListMenu.OptionsLayout.AddNewColumns = false;
			this.TreeListMenu.OptionsView.ShowColumns = false;
			this.TreeListMenu.Padding = new System.Windows.Forms.Padding(5);
			this.TreeListMenu.Size = new System.Drawing.Size(282, 211);
			this.TreeListMenu.TabIndex = 1;
			// 
			// MenuBar
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.TreeListMenu);
			this.Controls.Add(this.ButtonSelectMenu);
			this.Name = "MenuBar";
			this.Padding = new System.Windows.Forms.Padding(5);
			this.Size = new System.Drawing.Size(292, 241);
			((System.ComponentModel.ISupportInitialize)(this.ButtonSelectMenu.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TreeListMenu)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.ButtonEdit ButtonSelectMenu;
		private DevExpress.XtraTreeList.TreeList TreeListMenu;
    }
}
