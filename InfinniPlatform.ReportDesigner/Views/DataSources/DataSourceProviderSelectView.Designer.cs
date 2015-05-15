namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
	partial class DataSourceProviderSelectView
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
			this.DataProviderEdit = new System.Windows.Forms.ListBox();
			this.ViewHelp = new System.Windows.Forms.Label();
			this.MainPanel = new System.Windows.Forms.Panel();
			this.MainPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// DataProviderEdit
			// 
			this.DataProviderEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DataProviderEdit.FormattingEnabled = true;
			this.DataProviderEdit.Location = new System.Drawing.Point(10, 17);
			this.DataProviderEdit.Name = "DataProviderEdit";
			this.DataProviderEdit.Size = new System.Drawing.Size(419, 241);
			this.DataProviderEdit.TabIndex = 0;
			// 
			// ViewHelp
			// 
			this.ViewHelp.Dock = System.Windows.Forms.DockStyle.Top;
			this.ViewHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.ViewHelp.Location = new System.Drawing.Point(0, 0);
			this.ViewHelp.Name = "ViewHelp";
			this.ViewHelp.Padding = new System.Windows.Forms.Padding(10);
			this.ViewHelp.Size = new System.Drawing.Size(439, 40);
			this.ViewHelp.TabIndex = 1;
			this.ViewHelp.Text = "Select data source provider";
			this.ViewHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MainPanel
			// 
			this.MainPanel.Controls.Add(this.DataProviderEdit);
			this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainPanel.Location = new System.Drawing.Point(0, 40);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Padding = new System.Windows.Forms.Padding(10, 17, 10, 10);
			this.MainPanel.Size = new System.Drawing.Size(439, 268);
			this.MainPanel.TabIndex = 2;
			// 
			// DataSourceProviderSelectView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.MainPanel);
			this.Controls.Add(this.ViewHelp);
			this.Name = "DataSourceProviderSelectView";
			this.Size = new System.Drawing.Size(439, 308);
			this.MainPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox DataProviderEdit;
		private System.Windows.Forms.Label ViewHelp;
		private System.Windows.Forms.Panel MainPanel;
	}
}
