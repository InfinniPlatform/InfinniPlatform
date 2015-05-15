namespace InfinniPlatform.DesignControls.ScriptEditor
{
	partial class ScriptEditor
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
			this.ScriptTextEditor = new DigitalRune.Windows.TextEditor.TextEditorControl();
			this.SuspendLayout();
			// 
			// ScriptTextEditor
			// 
			this.ScriptTextEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ScriptTextEditor.LineViewerStyle = DigitalRune.Windows.TextEditor.Properties.LineViewerStyle.FullRow;
			this.ScriptTextEditor.Location = new System.Drawing.Point(0, 0);
			this.ScriptTextEditor.Name = "ScriptTextEditor";
			this.ScriptTextEditor.Size = new System.Drawing.Size(637, 307);
			this.ScriptTextEditor.TabIndex = 0;
			// 
			// ScriptEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ScriptTextEditor);
			this.Name = "ScriptEditor";
			this.Size = new System.Drawing.Size(637, 307);
			this.ResumeLayout(false);

		}

		#endregion

		private DigitalRune.Windows.TextEditor.TextEditorControl ScriptTextEditor;
	}
}
