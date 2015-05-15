namespace InfinniPlatform.ReportDesigner.Views.Designer
{
	partial class ReportDesignerForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportDesignerForm));
			this.ReportDesigner = new InfinniPlatform.ReportDesigner.Views.Designer.ReportDesignerView();
			this.SuspendLayout();
			// 
			// ReportDesigner
			// 
			this.ReportDesigner.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ReportDesigner.Location = new System.Drawing.Point(0, 0);
			this.ReportDesigner.Name = "ReportDesigner";
			this.ReportDesigner.Size = new System.Drawing.Size(884, 562);
			this.ReportDesigner.TabIndex = 0;
			// 
			// ReportDesignerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(884, 562);
			this.Controls.Add(this.ReportDesigner);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ReportDesignerForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "InfinniPlatform Report Designer";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.ResumeLayout(false);

		}

		#endregion

		private ReportDesignerView ReportDesigner;
	}
}