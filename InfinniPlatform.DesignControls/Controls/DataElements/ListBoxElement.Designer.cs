namespace InfinniPlatform.DesignControls.Controls.DataElements
{
    partial class ListBoxElement
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
            this.ListBox = new DevExpress.XtraEditors.ListBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.ListBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ListBox
            // 
            this.ListBox.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ListBox.Appearance.Options.UseBackColor = true;
            this.ListBox.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.ListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListBox.Items.AddRange(new object[] {
            "Item1",
            "Item2",
            "Item3"});
            this.ListBox.Location = new System.Drawing.Point(5, 5);
            this.ListBox.Name = "ListBox";
            this.ListBox.Size = new System.Drawing.Size(140, 140);
            this.ListBox.TabIndex = 0;
            // 
            // ListBoxElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ListBox);
            this.Name = "ListBoxElement";
            this.Padding = new System.Windows.Forms.Padding(5);
            ((System.ComponentModel.ISupportInitialize)(this.ListBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.ListBoxControl ListBox;
    }
}
