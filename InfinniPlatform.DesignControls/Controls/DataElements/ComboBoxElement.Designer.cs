namespace InfinniPlatform.DesignControls.Controls.DataElements
{
    partial class ComboBoxElement
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
            this.ComboBoxEdit = new DevExpress.XtraEditors.ImageComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.ComboBoxEdit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ComboBoxEdit
            // 
            this.ComboBoxEdit.Location = new System.Drawing.Point(5, 5);
            this.ComboBoxEdit.Name = "ComboBoxEdit";
            this.ComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
            this.ComboBoxEdit.Size = new System.Drawing.Size(275, 20);
            this.ComboBoxEdit.TabIndex = 0;
            // 
            // ComboBoxElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ComboBoxEdit);
            this.Name = "ComboBoxElement";
            this.Size = new System.Drawing.Size(285, 28);
            ((System.ComponentModel.ISupportInitialize)(this.ComboBoxEdit.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.ImageComboBoxEdit ComboBoxEdit;
    }
}
