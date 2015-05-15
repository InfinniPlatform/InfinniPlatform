namespace InfinniPlatform.DesignControls.PropertyDesigner
{
    partial class CollectionForm
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
			this.CollectionEditor = new InfinniPlatform.DesignControls.PropertyDesigner.CollectionEditor();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.panelControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelControl1
			// 
			this.panelControl1.Controls.Add(this.ButtonOK);
			this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelControl1.Location = new System.Drawing.Point(0, 309);
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(439, 37);
			this.panelControl1.TabIndex = 1;
			// 
			// ButtonOK
			// 
			this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.ButtonOK.Location = new System.Drawing.Point(353, 6);
			this.ButtonOK.LookAndFeel.SkinName = "Office 2013";
			this.ButtonOK.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ButtonOK.Name = "ButtonOK";
			this.ButtonOK.Size = new System.Drawing.Size(75, 23);
			this.ButtonOK.TabIndex = 0;
			this.ButtonOK.Text = "OK";
			// 
			// CollectionEditor
			// 
			this.CollectionEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CollectionEditor.Location = new System.Drawing.Point(0, 0);
			this.CollectionEditor.Name = "CollectionEditor";
			this.CollectionEditor.PropertyName = null;
			this.CollectionEditor.Size = new System.Drawing.Size(439, 309);
			this.CollectionEditor.TabIndex = 0;
			// 
			// CollectionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(439, 346);
			this.Controls.Add(this.CollectionEditor);
			this.Controls.Add(this.panelControl1);
			this.Name = "CollectionForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "CollectionForm";
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.panelControl1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        public CollectionEditor CollectionEditor;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton ButtonOK;
    }
}