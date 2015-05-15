namespace InfinniPlatform.DesignControls.ScriptEditor
{
	partial class ScriptEditForm
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
			this.ButtonCancel = new DevExpress.XtraEditors.SimpleButton();
			this.ButtonOK = new DevExpress.XtraEditors.SimpleButton();
			this.ScriptEditor = new InfinniPlatform.DesignControls.ScriptEditor.ScriptEditor();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.panelControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelControl1
			// 
			this.panelControl1.Controls.Add(this.ButtonCancel);
			this.panelControl1.Controls.Add(this.ButtonOK);
			this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelControl1.Location = new System.Drawing.Point(0, 405);
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(691, 38);
			this.panelControl1.TabIndex = 1;
			// 
			// ButtonCancel
			// 
			this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.ButtonCancel.Location = new System.Drawing.Point(605, 10);
			this.ButtonCancel.Name = "ButtonCancel";
			this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
			this.ButtonCancel.TabIndex = 1;
			this.ButtonCancel.Text = "Cancel";
			// 
			// ButtonOK
			// 
			this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.ButtonOK.Location = new System.Drawing.Point(524, 10);
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
			this.ScriptEditor.Script = "";
			this.ScriptEditor.Size = new System.Drawing.Size(691, 405);
			this.ScriptEditor.TabIndex = 2;
			// 
			// ScriptEditForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(691, 443);
			this.Controls.Add(this.ScriptEditor);
			this.Controls.Add(this.panelControl1);
			this.Name = "ScriptEditForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Редактор скриптов";
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.panelControl1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.PanelControl panelControl1;
		private DevExpress.XtraEditors.SimpleButton ButtonCancel;
		private DevExpress.XtraEditors.SimpleButton ButtonOK;
		private ScriptEditor ScriptEditor;
	}
}