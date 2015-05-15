namespace InfinniPlatform.DesignControls.Controls.LayoutPanels
{
	partial class TabPanel
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
			this.TabControl = new DevExpress.XtraTab.XtraTabControl();
			((System.ComponentModel.ISupportInitialize)(this.TabControl)).BeginInit();
			this.SuspendLayout();
			// 
			// TabControl
			// 
			this.TabControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
			this.TabControl.BorderStylePage = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
			this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TabControl.Location = new System.Drawing.Point(0, 0);
			this.TabControl.LookAndFeel.SkinName = "Office 2013";
			this.TabControl.LookAndFeel.UseDefaultLookAndFeel = false;
			this.TabControl.Name = "TabControl";
			this.TabControl.Size = new System.Drawing.Size(407, 193);
			this.TabControl.TabIndex = 0;
			// 
			// TabPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.TabControl);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "TabPanel";
			this.Size = new System.Drawing.Size(407, 193);
			((System.ComponentModel.ISupportInitialize)(this.TabControl)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraTab.XtraTabControl TabControl;

	}
}
