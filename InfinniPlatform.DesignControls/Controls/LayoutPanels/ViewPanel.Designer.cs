namespace InfinniPlatform.DesignControls.Controls.LayoutPanels
{
    partial class ViewPanel
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
			this.ScriptEditor = new InfinniPlatform.DesignControls.ScriptEditor.ScriptEditor();
			this.SuspendLayout();
			// 
			// ScriptEditor
			// 
			this.ScriptEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ScriptEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ScriptEditor.Location = new System.Drawing.Point(0, 0);
			this.ScriptEditor.Name = "ScriptEditor";
			this.ScriptEditor.ReadOnly = true;
			this.ScriptEditor.Script = "";
			this.ScriptEditor.Size = new System.Drawing.Size(233, 118);
			this.ScriptEditor.TabIndex = 0;
			this.ScriptEditor.ViewMode = true;
			// 
			// ViewPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ScriptEditor);
			this.Name = "ViewPanel";
			this.Size = new System.Drawing.Size(233, 118);
			this.ResumeLayout(false);

        }

        #endregion

		private ScriptEditor.ScriptEditor ScriptEditor;

	}
}
