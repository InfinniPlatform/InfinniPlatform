namespace InfinniPlatform.MetadataDesigner.Views.Status
{
	partial class StatusForm
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
			this.progressPanel1 = new DevExpress.XtraWaitForm.ProgressPanel();
			this.LogMemo = new DevExpress.XtraEditors.MemoEdit();
			((System.ComponentModel.ISupportInitialize)(this.LogMemo.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// progressPanel1
			// 
			this.progressPanel1.Appearance.BackColor = System.Drawing.Color.Transparent;
			this.progressPanel1.Appearance.Options.UseBackColor = true;
			this.progressPanel1.AppearanceCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
			this.progressPanel1.AppearanceCaption.Options.UseFont = true;
			this.progressPanel1.AppearanceDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.progressPanel1.AppearanceDescription.Options.UseFont = true;
			this.progressPanel1.Caption = "Выполняется обработка...";
			this.progressPanel1.Description = "Пожалуйста, подождите завершения операции";
			this.progressPanel1.Location = new System.Drawing.Point(92, 12);
			this.progressPanel1.Name = "progressPanel1";
			this.progressPanel1.Size = new System.Drawing.Size(294, 47);
			this.progressPanel1.TabIndex = 0;
			// 
			// LogMemo
			// 
			this.LogMemo.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.LogMemo.Location = new System.Drawing.Point(0, 83);
			this.LogMemo.Name = "LogMemo";
			this.LogMemo.Size = new System.Drawing.Size(491, 410);
			this.LogMemo.TabIndex = 1;
			this.LogMemo.UseOptimizedRendering = true;
			// 
			// StatusForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(491, 493);
			this.ControlBox = false;
			this.Controls.Add(this.LogMemo);
			this.Controls.Add(this.progressPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "StatusForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Пожалуйста, подождите";
			((System.ComponentModel.ISupportInitialize)(this.LogMemo.Properties)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraWaitForm.ProgressPanel progressPanel1;
		public DevExpress.XtraEditors.MemoEdit LogMemo;
	}
}