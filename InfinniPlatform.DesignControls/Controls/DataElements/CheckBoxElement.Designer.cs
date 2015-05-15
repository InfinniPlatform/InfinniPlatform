namespace InfinniPlatform.DesignControls.Controls.DataElements
{
    partial class CheckBoxElement
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
			this.CheckBox = new DevExpress.XtraEditors.CheckEdit();
			((System.ComponentModel.ISupportInitialize)(this.CheckBox.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// CheckBox
			// 
			this.CheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CheckBox.Location = new System.Drawing.Point(0, 0);
			this.CheckBox.Name = "CheckBox";
			this.CheckBox.Properties.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.CheckBox.Properties.Appearance.Options.UseBackColor = true;
			this.CheckBox.Properties.Caption = "CheckBox";
			this.CheckBox.Size = new System.Drawing.Size(150, 19);
			this.CheckBox.TabIndex = 0;
			// 
			// CheckBoxElement
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.CheckBox);
			this.Name = "CheckBoxElement";
			this.Size = new System.Drawing.Size(150, 24);
			((System.ComponentModel.ISupportInitialize)(this.CheckBox.Properties)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.CheckEdit CheckBox;
    }
}
