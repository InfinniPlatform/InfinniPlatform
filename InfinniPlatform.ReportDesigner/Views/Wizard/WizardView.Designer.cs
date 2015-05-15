namespace InfinniPlatform.ReportDesigner.Views.Wizard
{
	partial class WizardView
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
			this.ControlPanel = new System.Windows.Forms.Panel();
			this.ResetBtn = new System.Windows.Forms.Button();
			this.BackBtn = new System.Windows.Forms.Button();
			this.NextBtn = new System.Windows.Forms.Button();
			this.FinishBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.MainPanel = new System.Windows.Forms.Panel();
			this.ControlPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// ControlPanel
			// 
			this.ControlPanel.Controls.Add(this.ResetBtn);
			this.ControlPanel.Controls.Add(this.BackBtn);
			this.ControlPanel.Controls.Add(this.NextBtn);
			this.ControlPanel.Controls.Add(this.FinishBtn);
			this.ControlPanel.Controls.Add(this.CancelBtn);
			this.ControlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.ControlPanel.Location = new System.Drawing.Point(0, 504);
			this.ControlPanel.Name = "ControlPanel";
			this.ControlPanel.Size = new System.Drawing.Size(484, 29);
			this.ControlPanel.TabIndex = 0;
			// 
			// ResetBtn
			// 
			this.ResetBtn.Location = new System.Drawing.Point(3, 3);
			this.ResetBtn.Name = "ResetBtn";
			this.ResetBtn.Size = new System.Drawing.Size(75, 23);
			this.ResetBtn.TabIndex = 4;
			this.ResetBtn.Text = "&Defaults";
			this.ResetBtn.UseVisualStyleBackColor = true;
			this.ResetBtn.Click += new System.EventHandler(this.OnReset);
			// 
			// BackBtn
			// 
			this.BackBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BackBtn.Location = new System.Drawing.Point(163, 3);
			this.BackBtn.Name = "BackBtn";
			this.BackBtn.Size = new System.Drawing.Size(75, 23);
			this.BackBtn.TabIndex = 0;
			this.BackBtn.Text = "< &Back";
			this.BackBtn.UseVisualStyleBackColor = true;
			this.BackBtn.Click += new System.EventHandler(this.OnBack);
			// 
			// NextBtn
			// 
			this.NextBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.NextBtn.Location = new System.Drawing.Point(244, 3);
			this.NextBtn.Name = "NextBtn";
			this.NextBtn.Size = new System.Drawing.Size(75, 23);
			this.NextBtn.TabIndex = 1;
			this.NextBtn.Text = "&Next >";
			this.NextBtn.UseVisualStyleBackColor = true;
			this.NextBtn.Click += new System.EventHandler(this.OnNext);
			// 
			// FinishBtn
			// 
			this.FinishBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.FinishBtn.Location = new System.Drawing.Point(325, 3);
			this.FinishBtn.Name = "FinishBtn";
			this.FinishBtn.Size = new System.Drawing.Size(75, 23);
			this.FinishBtn.TabIndex = 2;
			this.FinishBtn.Text = "&Finish";
			this.FinishBtn.UseVisualStyleBackColor = true;
			this.FinishBtn.Click += new System.EventHandler(this.OnFinish);
			// 
			// CancelBtn
			// 
			this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Location = new System.Drawing.Point(406, 3);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(75, 23);
			this.CancelBtn.TabIndex = 3;
			this.CancelBtn.Text = "Cancel";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.OnCancel);
			// 
			// MainPanel
			// 
			this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainPanel.Location = new System.Drawing.Point(0, 0);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(484, 504);
			this.MainPanel.TabIndex = 0;
			// 
			// WizardView
			// 
			this.AcceptButton = this.FinishBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(484, 533);
			this.Controls.Add(this.MainPanel);
			this.Controls.Add(this.ControlPanel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "WizardView";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.ControlPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel ControlPanel;
		private System.Windows.Forms.Button FinishBtn;
		private System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.Button BackBtn;
		private System.Windows.Forms.Button NextBtn;
		private System.Windows.Forms.Panel MainPanel;
		private System.Windows.Forms.Button ResetBtn;
	}
}