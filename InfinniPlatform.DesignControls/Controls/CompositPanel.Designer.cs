namespace InfinniPlatform.DesignControls.Controls
{
    partial class CompositPanel
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
			this.PanelContainer = new DevExpress.XtraEditors.PanelControl();
			((System.ComponentModel.ISupportInitialize)(this.PanelContainer)).BeginInit();
			this.SuspendLayout();
			// 
			// PanelContainer
			// 
			this.PanelContainer.Appearance.BorderColor = System.Drawing.Color.DeepSkyBlue;
			this.PanelContainer.Appearance.Options.UseBorderColor = true;
			this.PanelContainer.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.PanelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PanelContainer.Location = new System.Drawing.Point(0, 0);
			this.PanelContainer.LookAndFeel.SkinName = "Office 2013";
			this.PanelContainer.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
			this.PanelContainer.LookAndFeel.UseDefaultLookAndFeel = false;
			this.PanelContainer.Margin = new System.Windows.Forms.Padding(0);
			this.PanelContainer.Name = "PanelContainer";
			this.PanelContainer.Size = new System.Drawing.Size(653, 421);
			this.PanelContainer.TabIndex = 5;
			// 
			// CompositPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.PanelContainer);
			this.Name = "CompositPanel";
			this.Size = new System.Drawing.Size(653, 421);
			((System.ComponentModel.ISupportInitialize)(this.PanelContainer)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

	    internal DevExpress.XtraEditors.PanelControl PanelContainer;
    }
}
