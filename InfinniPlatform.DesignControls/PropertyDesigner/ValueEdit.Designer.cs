namespace InfinniPlatform.DesignControls.PropertyDesigner
{
    partial class ValueEdit
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
			this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
			this.ButtonOK = new DevExpress.XtraEditors.SimpleButton();
			this.ScriptEditor = new InfinniPlatform.DesignControls.ScriptEditor.ScriptEditor();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.panelControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelControl1
			// 
			this.panelControl1.Controls.Add(this.ButtonOK);
			this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelControl1.Location = new System.Drawing.Point(0, 391);
			this.panelControl1.LookAndFeel.SkinName = "Office 2013";
			this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(657, 35);
			this.panelControl1.TabIndex = 1;
			// 
			// ButtonOK
			// 
			this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.ButtonOK.Location = new System.Drawing.Point(570, 6);
			this.ButtonOK.LookAndFeel.SkinName = "Office 2013";
			this.ButtonOK.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ButtonOK.Name = "ButtonOK";
			this.ButtonOK.Size = new System.Drawing.Size(75, 23);
			this.ButtonOK.TabIndex = 0;
			this.ButtonOK.Text = "OK";
			// 
			// ScriptEditor
			// 
			this.ScriptEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ScriptEditor.Location = new System.Drawing.Point(0, 0);
			this.ScriptEditor.Name = "ScriptEditor";
			this.ScriptEditor.ReadOnly = false;
			this.ScriptEditor.Script = "";
			this.ScriptEditor.Size = new System.Drawing.Size(657, 391);
			this.ScriptEditor.TabIndex = 2;
			this.ScriptEditor.ViewMode = false;
			// 
			// ValueEdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(657, 426);
			this.Controls.Add(this.ScriptEditor);
			this.Controls.Add(this.panelControl1);
			this.Name = "ValueEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Property value";
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.panelControl1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

		private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton ButtonOK;
		private ScriptEditor.ScriptEditor ScriptEditor;
    }
}