namespace InfinniPlatform.ReportDesigner.Views.Controls
{
	partial class NumericEditor
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
			this.TextBoxControl = new NumericEditor.ClipboardTextBox();
			this.SuspendLayout();
			// 
			// TextBoxControl
			// 
			this.TextBoxControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TextBoxControl.Location = new System.Drawing.Point(0, 0);
			this.TextBoxControl.Name = "TextBoxControl";
			this.TextBoxControl.Size = new System.Drawing.Size(150, 20);
			this.TextBoxControl.TabIndex = 0;
			this.TextBoxControl.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.TextBoxControl.ClipboardPaste += new System.EventHandler<ClipboardEventArgs>(this.OnClipboardPaste);
			this.TextBoxControl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress);
			// 
			// NumericEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.TextBoxControl);
			this.Name = "NumericEditor";
			this.Size = new System.Drawing.Size(150, 20);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private ClipboardTextBox TextBoxControl;
	}
}
