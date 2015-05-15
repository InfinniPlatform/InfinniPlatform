namespace InfinniPlatform.DesignControls.Controls.ActionElements
{
	partial class Button
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
			this.ButtonElement = new DevExpress.XtraEditors.SimpleButton();
			this.SuspendLayout();
			// 
			// ButtonElement
			// 
			this.ButtonElement.Dock = System.Windows.Forms.DockStyle.Top;
			this.ButtonElement.Location = new System.Drawing.Point(5, 5);
			this.ButtonElement.LookAndFeel.SkinName = "Office 2013";
			this.ButtonElement.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ButtonElement.Name = "ButtonElement";
			this.ButtonElement.Size = new System.Drawing.Size(54, 22);
			this.ButtonElement.TabIndex = 0;
			this.ButtonElement.Text = "NewButton";
			// 
			// Button
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ButtonElement);
			this.Name = "Button";
			this.Padding = new System.Windows.Forms.Padding(5);
			this.Size = new System.Drawing.Size(64, 32);
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.SimpleButton ButtonElement;
	}
}
