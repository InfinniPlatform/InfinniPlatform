namespace InfinniPlatform.DesignControls.Controls.DataElements
{
    partial class DataGridElement
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
			this.DataGridControl = new DevExpress.XtraGrid.GridControl();
			this.DataGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.ScriptEditor = new InfinniPlatform.DesignControls.ScriptEditor.ScriptEditor();
			((System.ComponentModel.ISupportInitialize)(this.DataGridControl)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.DataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// DataGridControl
			// 
			this.DataGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DataGridControl.Location = new System.Drawing.Point(0, 0);
			this.DataGridControl.MainView = this.DataGridView;
			this.DataGridControl.Name = "DataGridControl";
			this.DataGridControl.Size = new System.Drawing.Size(419, 253);
			this.DataGridControl.TabIndex = 0;
			this.DataGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.DataGridView});
			// 
			// DataGridView
			// 
			this.DataGridView.GridControl = this.DataGridControl;
			this.DataGridView.Name = "DataGridView";
			this.DataGridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
			this.DataGridView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
			this.DataGridView.OptionsBehavior.Editable = false;
			this.DataGridView.OptionsCustomization.AllowColumnMoving = false;
			this.DataGridView.OptionsCustomization.AllowColumnResizing = false;
			this.DataGridView.OptionsCustomization.AllowFilter = false;
			this.DataGridView.OptionsCustomization.AllowGroup = false;
			this.DataGridView.OptionsCustomization.AllowQuickHideColumns = false;
			this.DataGridView.OptionsCustomization.AllowRowSizing = true;
			this.DataGridView.OptionsCustomization.AllowSort = false;
			this.DataGridView.OptionsView.ShowGroupPanel = false;
			// 
			// ScriptEditor
			// 
			this.ScriptEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ScriptEditor.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.ScriptEditor.Location = new System.Drawing.Point(0, 185);
			this.ScriptEditor.Name = "ScriptEditor";
			this.ScriptEditor.Script = "";
			this.ScriptEditor.Size = new System.Drawing.Size(419, 68);
			this.ScriptEditor.TabIndex = 3;
			this.ScriptEditor.ViewMode = true;
			// 
			// DataGridElement
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ScriptEditor);
			this.Controls.Add(this.DataGridControl);
			this.Name = "DataGridElement";
			this.Size = new System.Drawing.Size(419, 253);
			((System.ComponentModel.ISupportInitialize)(this.DataGridControl)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.DataGridView)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl DataGridControl;
		private DevExpress.XtraGrid.Views.Grid.GridView DataGridView;
		private ScriptEditor.ScriptEditor ScriptEditor;
    }
}
