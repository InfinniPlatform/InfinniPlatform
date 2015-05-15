namespace InfinniPlatform.DesignControls.Controls.DataElements
{
	partial class LabelElement
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
            this.Label = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // Label
            // 
            this.Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Label.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.Label.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.Label.Location = new System.Drawing.Point(0, 0);
            this.Label.LookAndFeel.SkinName = "Office 2013";
            this.Label.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Label.Name = "Label";
            this.Label.Size = new System.Drawing.Size(126, 17);
            this.Label.TabIndex = 0;
            this.Label.Text = "Label";
            // 
            // LabelElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Label);
            this.Name = "LabelElement";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(126, 17);
            this.ResumeLayout(false);

		}

		#endregion

        private DevExpress.XtraEditors.LabelControl Label;
	}
}
