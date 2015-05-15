namespace InfinniPlatform.ReportDesigner.Views.Preview
{
	partial class PreviewReportView
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
			this.components = new System.ComponentModel.Container();
			this.ParameterListPanel = new System.Windows.Forms.Panel();
			this.ControlPanel = new System.Windows.Forms.Panel();
			this.PreviewBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.MainPanel = new System.Windows.Forms.Panel();
			this.ServicePanel = new System.Windows.Forms.Panel();
			this.RefreshBtn = new System.Windows.Forms.Button();
			this.ServiceUrlEdit = new Controls.EditableComboBox();
			this.FileFormatPanel = new System.Windows.Forms.Panel();
			this.FileFormatLabel = new System.Windows.Forms.Label();
			this.ViewHelp = new System.Windows.Forms.Label();
			this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.ParameterList = new InfinniPlatform.ReportDesigner.Views.Preview.ParameterListView();
			this.FileFormatEdit = new InfinniPlatform.ReportDesigner.Views.Controls.FormattedComboBox();
			this.ParameterListPanel.SuspendLayout();
			this.ControlPanel.SuspendLayout();
			this.MainPanel.SuspendLayout();
			this.ServicePanel.SuspendLayout();
			this.FileFormatPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// ParameterListPanel
			// 
			this.ParameterListPanel.Controls.Add(this.ParameterList);
			this.ParameterListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ParameterListPanel.Location = new System.Drawing.Point(0, 83);
			this.ParameterListPanel.Name = "ParameterListPanel";
			this.ParameterListPanel.Size = new System.Drawing.Size(434, 219);
			this.ParameterListPanel.TabIndex = 3;
			// 
			// ControlPanel
			// 
			this.ControlPanel.Controls.Add(this.PreviewBtn);
			this.ControlPanel.Controls.Add(this.CancelBtn);
			this.ControlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.ControlPanel.Location = new System.Drawing.Point(0, 333);
			this.ControlPanel.Name = "ControlPanel";
			this.ControlPanel.Size = new System.Drawing.Size(434, 29);
			this.ControlPanel.TabIndex = 0;
			// 
			// PreviewBtn
			// 
			this.PreviewBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.PreviewBtn.Location = new System.Drawing.Point(275, 3);
			this.PreviewBtn.Name = "PreviewBtn";
			this.PreviewBtn.Size = new System.Drawing.Size(75, 23);
			this.PreviewBtn.TabIndex = 1;
			this.PreviewBtn.Text = "Preview";
			this.PreviewBtn.UseVisualStyleBackColor = true;
			this.PreviewBtn.Click += new System.EventHandler(this.OnPreview);
			// 
			// CancelBtn
			// 
			this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Location = new System.Drawing.Point(356, 3);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(75, 23);
			this.CancelBtn.TabIndex = 0;
			this.CancelBtn.Text = "Cancel";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.OnCancel);
			// 
			// MainPanel
			// 
			this.MainPanel.Controls.Add(this.ParameterListPanel);
			this.MainPanel.Controls.Add(this.ServicePanel);
			this.MainPanel.Controls.Add(this.FileFormatPanel);
			this.MainPanel.Controls.Add(this.ViewHelp);
			this.MainPanel.Controls.Add(this.ControlPanel);
			this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainPanel.Location = new System.Drawing.Point(0, 0);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(434, 362);
			this.MainPanel.TabIndex = 0;
			// 
			// ServicePanel
			// 
			this.ServicePanel.Controls.Add(this.RefreshBtn);
			this.ServicePanel.Controls.Add(this.ServiceUrlEdit);
			this.ServicePanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.ServicePanel.Location = new System.Drawing.Point(0, 40);
			this.ServicePanel.Name = "ServicePanel";
			this.ServicePanel.Size = new System.Drawing.Size(434, 43);
			this.ServicePanel.TabIndex = 2;
			// 
			// RefreshBtn
			// 
			this.RefreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.RefreshBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.RefreshBtn.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Refresh;
			this.RefreshBtn.Location = new System.Drawing.Point(403, 17);
			this.RefreshBtn.Name = "RefreshBtn";
			this.RefreshBtn.Size = new System.Drawing.Size(21, 21);
			this.RefreshBtn.TabIndex = 1;
			this.MainToolTip.SetToolTip(this.RefreshBtn, "Refresh parameter values");
			this.RefreshBtn.UseVisualStyleBackColor = true;
			this.RefreshBtn.Click += new System.EventHandler(this.OnRefresh);
			// 
			// ServiceUrlEdit
			// 
			this.ServiceUrlEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ServiceUrlEdit.Location = new System.Drawing.Point(10, 17);
			this.ServiceUrlEdit.Name = "ServiceUrlEdit";
			this.ServiceUrlEdit.Size = new System.Drawing.Size(387, 21);
			this.ServiceUrlEdit.TabIndex = 0;
			this.MainToolTip.SetToolTip(this.ServiceUrlEdit, "Report Service URL");
			// 
			// FileFormatPanel
			// 
			this.FileFormatPanel.Controls.Add(this.FileFormatEdit);
			this.FileFormatPanel.Controls.Add(this.FileFormatLabel);
			this.FileFormatPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.FileFormatPanel.Location = new System.Drawing.Point(0, 302);
			this.FileFormatPanel.Name = "FileFormatPanel";
			this.FileFormatPanel.Size = new System.Drawing.Size(434, 31);
			this.FileFormatPanel.TabIndex = 4;
			// 
			// FileFormatLabel
			// 
			this.FileFormatLabel.AutoSize = true;
			this.FileFormatLabel.Location = new System.Drawing.Point(10, 8);
			this.FileFormatLabel.Name = "FileFormatLabel";
			this.FileFormatLabel.Size = new System.Drawing.Size(58, 13);
			this.FileFormatLabel.TabIndex = 0;
			this.FileFormatLabel.Text = "File format:";
			// 
			// ViewHelp
			// 
			this.ViewHelp.Dock = System.Windows.Forms.DockStyle.Top;
			this.ViewHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.ViewHelp.Location = new System.Drawing.Point(0, 0);
			this.ViewHelp.Name = "ViewHelp";
			this.ViewHelp.Padding = new System.Windows.Forms.Padding(10);
			this.ViewHelp.Size = new System.Drawing.Size(434, 40);
			this.ViewHelp.TabIndex = 1;
			this.ViewHelp.Text = "Specify parameter values​​";
			this.ViewHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ParameterList
			// 
			this.ParameterList.AutoScroll = true;
			this.ParameterList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ParameterList.Location = new System.Drawing.Point(0, 0);
			this.ParameterList.Name = "ParameterList";
			this.ParameterList.Size = new System.Drawing.Size(434, 219);
			this.ParameterList.TabIndex = 0;
			// 
			// FileFormatEdit
			// 
			this.FileFormatEdit.ItemFormatString = null;
			this.FileFormatEdit.Location = new System.Drawing.Point(74, 4);
			this.FileFormatEdit.Name = "FileFormatEdit";
			this.FileFormatEdit.Size = new System.Drawing.Size(150, 21);
			this.FileFormatEdit.TabIndex = 1;
			// 
			// PreviewReportView
			// 
			this.AcceptButton = this.PreviewBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(434, 362);
			this.Controls.Add(this.MainPanel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PreviewReportView";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Report Preview";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ParameterListPanel.ResumeLayout(false);
			this.ControlPanel.ResumeLayout(false);
			this.MainPanel.ResumeLayout(false);
			this.ServicePanel.ResumeLayout(false);
			this.FileFormatPanel.ResumeLayout(false);
			this.FileFormatPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel ParameterListPanel;
		private System.Windows.Forms.Panel ControlPanel;
		private System.Windows.Forms.Button PreviewBtn;
		private System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.Panel MainPanel;
		private ParameterListView ParameterList;
		private System.Windows.Forms.Label ViewHelp;
		private System.Windows.Forms.Panel ServicePanel;
		private System.Windows.Forms.Button RefreshBtn;
		private Controls.EditableComboBox ServiceUrlEdit;
		private System.Windows.Forms.Panel FileFormatPanel;
		private Controls.FormattedComboBox FileFormatEdit;
		private System.Windows.Forms.Label FileFormatLabel;
		private System.Windows.Forms.ToolTip MainToolTip;
	}
}
