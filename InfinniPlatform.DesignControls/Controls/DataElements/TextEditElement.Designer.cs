namespace InfinniPlatform.DesignControls.Controls.DataElements
{
    partial class TextEditElement
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
			this.TextEdit = new DevExpress.XtraEditors.TextEdit();
			((System.ComponentModel.ISupportInitialize)(this.TextEdit.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// TextEdit
			// 
			this.TextEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TextEdit.Location = new System.Drawing.Point(5, 6);
			this.TextEdit.Margin = new System.Windows.Forms.Padding(0);
			this.TextEdit.Name = "TextEdit";
			this.TextEdit.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window;
			this.TextEdit.Properties.Appearance.Options.UseBackColor = true;
			this.TextEdit.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.TextEdit.Properties.LookAndFeel.SkinName = "Office 2013";
			this.TextEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
			this.TextEdit.Properties.ReadOnly = true;
			this.TextEdit.Size = new System.Drawing.Size(41, 20);
			this.TextEdit.TabIndex = 0;
			// 
			// TextEditElement
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.TextEdit);
			this.Name = "TextEditElement";
			this.Padding = new System.Windows.Forms.Padding(5);
			this.Size = new System.Drawing.Size(51, 31);
			((System.ComponentModel.ISupportInitialize)(this.TextEdit.Properties)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit TextEdit;
    }
}
