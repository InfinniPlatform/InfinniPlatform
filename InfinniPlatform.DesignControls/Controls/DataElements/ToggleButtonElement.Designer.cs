namespace InfinniPlatform.DesignControls.Controls.DataElements
{
    partial class ToggleButtonElement
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
            this.ToggleSwitch = new DevExpress.XtraEditors.ToggleSwitch();
            ((System.ComponentModel.ISupportInitialize)(this.ToggleSwitch.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ToggleSwitch
            // 
            this.ToggleSwitch.Location = new System.Drawing.Point(0, 0);
            this.ToggleSwitch.Name = "ToggleSwitch";
            this.ToggleSwitch.Properties.OffText = "OFF";
            this.ToggleSwitch.Properties.OnText = "ON";
            this.ToggleSwitch.Size = new System.Drawing.Size(94, 24);
            this.ToggleSwitch.TabIndex = 0;
            // 
            // ToggleButtonElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.ToggleSwitch);
            this.Name = "ToggleButtonElement";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(96, 26);
            ((System.ComponentModel.ISupportInitialize)(this.ToggleSwitch.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.ToggleSwitch ToggleSwitch;
    }
}
