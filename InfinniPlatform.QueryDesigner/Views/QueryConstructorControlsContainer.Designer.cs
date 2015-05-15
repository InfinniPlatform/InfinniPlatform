namespace InfinniPlatform.QueryDesigner.Views
{
	partial class QueryConstructorControlsContainer
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
			this.ScrollControl = new DevExpress.XtraEditors.XtraScrollableControl();
			this.SuspendLayout();
			// 
			// ScrollControl
			// 
			this.ScrollControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ScrollControl.FireScrollEventOnMouseWheel = true;
			this.ScrollControl.Location = new System.Drawing.Point(0, 0);
			this.ScrollControl.LookAndFeel.SkinName = "Office 2013";
			this.ScrollControl.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ScrollControl.Name = "ScrollControl";
			this.ScrollControl.Size = new System.Drawing.Size(631, 221);
			this.ScrollControl.TabIndex = 0;
			// 
			// QueryConstructorControlsContainer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ScrollControl);
			this.Name = "QueryConstructorControlsContainer";
			this.Size = new System.Drawing.Size(631, 221);
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.XtraScrollableControl ScrollControl;
	}
}
