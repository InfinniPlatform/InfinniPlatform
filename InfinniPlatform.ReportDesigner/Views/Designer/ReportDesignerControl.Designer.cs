namespace InfinniPlatform.ReportDesigner.Views.Designer
{
	partial class ReportDesignerControl
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
			this.Editor = new InfinniPlatform.ReportDesigner.Views.Designer.ReportDesignerView();
			this.SuspendLayout();
			// 
			// Editor
			// 
			this.Editor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Editor.EditValue = null;
			this.Editor.Location = new System.Drawing.Point(0, 0);
			this.Editor.Name = "Editor";
			this.Editor.Size = new System.Drawing.Size(500, 400);
			this.Editor.TabIndex = 0;
			this.Editor.EditValueChanged += new System.EventHandler(this.OnEditValueChanged);
			// 
			// ReportDesignerControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.Editor);
			this.Name = "ReportDesignerControl";
			this.Size = new System.Drawing.Size(500, 400);
			this.ResumeLayout(false);

		}

		#endregion

		private ReportDesignerView Editor;
	}
}
