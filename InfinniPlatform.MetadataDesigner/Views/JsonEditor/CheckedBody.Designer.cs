namespace InfinniPlatform.MetadataDesigner.Views.JsonEditor
{
	partial class CheckedBody
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
			this.MemoJSONBody = new DevExpress.XtraEditors.MemoEdit();
			this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
			this.CancelButton = new DevExpress.XtraEditors.SimpleButton();
			this.ButtonOK = new DevExpress.XtraEditors.SimpleButton();
			((System.ComponentModel.ISupportInitialize)(this.MemoJSONBody.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.panelControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// MemoJSONBody
			// 
			this.MemoJSONBody.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MemoJSONBody.EditValue = "{\r\n\r\n}";
			this.MemoJSONBody.Location = new System.Drawing.Point(0, 0);
			this.MemoJSONBody.Name = "MemoJSONBody";
			this.MemoJSONBody.Size = new System.Drawing.Size(418, 193);
			this.MemoJSONBody.TabIndex = 0;
			this.MemoJSONBody.UseOptimizedRendering = true;
			// 
			// panelControl1
			// 
			this.panelControl1.Controls.Add(this.CancelButton);
			this.panelControl1.Controls.Add(this.ButtonOK);
			this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelControl1.Location = new System.Drawing.Point(0, 193);
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(418, 40);
			this.panelControl1.TabIndex = 1;
			// 
			// CancelButton
			// 
			this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelButton.Location = new System.Drawing.Point(312, 6);
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(89, 30);
			this.CancelButton.TabIndex = 1;
			this.CancelButton.Text = "Отмена";
			// 
			// ButtonOK
			// 
			this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.ButtonOK.Location = new System.Drawing.Point(217, 6);
			this.ButtonOK.Name = "ButtonOK";
			this.ButtonOK.Size = new System.Drawing.Size(89, 30);
			this.ButtonOK.TabIndex = 0;
			this.ButtonOK.Text = "ОК";
			// 
			// CheckedBody
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(418, 233);
			this.Controls.Add(this.MemoJSONBody);
			this.Controls.Add(this.panelControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "CheckedBody";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Добавьте тело запроса JSON";
			((System.ComponentModel.ISupportInitialize)(this.MemoJSONBody.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.panelControl1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.MemoEdit MemoJSONBody;
		private DevExpress.XtraEditors.PanelControl panelControl1;
		private DevExpress.XtraEditors.SimpleButton ButtonOK;
		private DevExpress.XtraEditors.SimpleButton CancelButton;
	}
}