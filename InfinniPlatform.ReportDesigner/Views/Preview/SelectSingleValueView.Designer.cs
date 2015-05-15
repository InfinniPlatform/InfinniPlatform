namespace InfinniPlatform.ReportDesigner.Views.Preview
{
	partial class SelectSingleValueView
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
			this.ItemsPanel = new System.Windows.Forms.Panel();
			this.ItemsEdit = new System.Windows.Forms.ListBox();
			this.ItemsPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// ItemsPanel
			// 
			this.ItemsPanel.Controls.Add(this.ItemsEdit);
			this.ItemsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ItemsPanel.Location = new System.Drawing.Point(0, 0);
			this.ItemsPanel.Name = "ItemsPanel";
			this.ItemsPanel.Padding = new System.Windows.Forms.Padding(10);
			this.ItemsPanel.Size = new System.Drawing.Size(364, 301);
			this.ItemsPanel.TabIndex = 1;
			// 
			// ItemsEdit
			// 
			this.ItemsEdit.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ItemsEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ItemsEdit.FormattingEnabled = true;
			this.ItemsEdit.Location = new System.Drawing.Point(10, 10);
			this.ItemsEdit.Name = "ItemsEdit";
			this.ItemsEdit.Size = new System.Drawing.Size(344, 281);
			this.ItemsEdit.TabIndex = 0;
			// 
			// SelectValueView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ItemsPanel);
			this.Name = "SelectValueView";
			this.Size = new System.Drawing.Size(364, 301);
			this.ItemsPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel ItemsPanel;
		private System.Windows.Forms.ListBox ItemsEdit;
	}
}
