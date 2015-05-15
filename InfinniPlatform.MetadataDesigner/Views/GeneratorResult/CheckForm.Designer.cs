namespace InfinniPlatform.MetadataDesigner.Views.GeneratorResult
{
	partial class CheckForm
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
			this.Memo = new DevExpress.XtraEditors.MemoEdit();
			this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
			this.ButtonOK = new DevExpress.XtraEditors.SimpleButton();
			this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
			this.label2 = new System.Windows.Forms.Label();
			this.BodyMemo = new DevExpress.XtraEditors.MemoEdit();
			this.label1 = new System.Windows.Forms.Label();
			this.UrlEdit = new DevExpress.XtraEditors.TextEdit();
			this.label3 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.Memo.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.panelControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
			this.panelControl2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.BodyMemo.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.UrlEdit.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// Memo
			// 
			this.Memo.Location = new System.Drawing.Point(162, 136);
			this.Memo.Name = "Memo";
			this.Memo.Size = new System.Drawing.Size(370, 187);
			this.Memo.TabIndex = 0;
			this.Memo.UseOptimizedRendering = true;
			// 
			// panelControl1
			// 
			this.panelControl1.Controls.Add(this.ButtonOK);
			this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelControl1.Location = new System.Drawing.Point(0, 329);
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(550, 38);
			this.panelControl1.TabIndex = 1;
			// 
			// ButtonOK
			// 
			this.ButtonOK.Location = new System.Drawing.Point(470, 10);
			this.ButtonOK.Name = "ButtonOK";
			this.ButtonOK.Size = new System.Drawing.Size(75, 23);
			this.ButtonOK.TabIndex = 0;
			this.ButtonOK.Text = "ОК";
			this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
			// 
			// panelControl2
			// 
			this.panelControl2.Controls.Add(this.label2);
			this.panelControl2.Controls.Add(this.BodyMemo);
			this.panelControl2.Controls.Add(this.label1);
			this.panelControl2.Controls.Add(this.UrlEdit);
			this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelControl2.Location = new System.Drawing.Point(0, 0);
			this.panelControl2.Name = "panelControl2";
			this.panelControl2.Size = new System.Drawing.Size(550, 136);
			this.panelControl2.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 38);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(31, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Body";
			// 
			// BodyMemo
			// 
			this.BodyMemo.Location = new System.Drawing.Point(162, 35);
			this.BodyMemo.Name = "BodyMemo";
			this.BodyMemo.Size = new System.Drawing.Size(370, 96);
			this.BodyMemo.TabIndex = 2;
			this.BodyMemo.UseOptimizedRendering = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(144, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Результат запроса по URL";
			// 
			// UrlEdit
			// 
			this.UrlEdit.Location = new System.Drawing.Point(162, 9);
			this.UrlEdit.Name = "UrlEdit";
			this.UrlEdit.Size = new System.Drawing.Size(370, 20);
			this.UrlEdit.TabIndex = 0;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 139);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(55, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Response";
			// 
			// CheckForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(550, 367);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.Memo);
			this.Controls.Add(this.panelControl2);
			this.Controls.Add(this.panelControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "CheckForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "CheckForm";
			((System.ComponentModel.ISupportInitialize)(this.Memo.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.panelControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
			this.panelControl2.ResumeLayout(false);
			this.panelControl2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.BodyMemo.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.UrlEdit.Properties)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private DevExpress.XtraEditors.MemoEdit Memo;
		private DevExpress.XtraEditors.PanelControl panelControl1;
		private DevExpress.XtraEditors.SimpleButton ButtonOK;
		private DevExpress.XtraEditors.PanelControl panelControl2;
		private DevExpress.XtraEditors.TextEdit UrlEdit;
		private System.Windows.Forms.Label label2;
		private DevExpress.XtraEditors.MemoEdit BodyMemo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
	}
}