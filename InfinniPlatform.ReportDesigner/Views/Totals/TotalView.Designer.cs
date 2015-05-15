namespace InfinniPlatform.ReportDesigner.Views.Totals
{
	partial class TotalView
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
			this.components = new System.ComponentModel.Container();
			this.NameLabel = new System.Windows.Forms.Label();
			this.NameEdit = new System.Windows.Forms.TextBox();
			this.TotalFuncLabel = new System.Windows.Forms.Label();
			this.ExpressionLabel = new System.Windows.Forms.Label();
			this.DataBandLabel = new System.Windows.Forms.Label();
			this.PrintBandLabel = new System.Windows.Forms.Label();
			this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.TotalFuncEdit = new InfinniPlatform.ReportDesigner.Views.Controls.FormattedComboBox();
			this.PrintBandEdit = new InfinniPlatform.ReportDesigner.Views.Controls.FormattedComboBox();
			this.DataBandEdit = new InfinniPlatform.ReportDesigner.Views.Controls.FormattedComboBox();
			this.ViewHelp = new System.Windows.Forms.Label();
			this.MainPanel = new System.Windows.Forms.Panel();
			this.ExpressionEdit = new InfinniPlatform.ReportDesigner.Views.Controls.ExpressionEditElement();
			this.MainPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// NameLabel
			// 
			this.NameLabel.AutoSize = true;
			this.NameLabel.Location = new System.Drawing.Point(10, 17);
			this.NameLabel.Name = "NameLabel";
			this.NameLabel.Size = new System.Drawing.Size(38, 13);
			this.NameLabel.TabIndex = 0;
			this.NameLabel.Text = "Name:";
			this.MainToolTip.SetToolTip(this.NameLabel, "Total name.");
			// 
			// NameEdit
			// 
			this.NameEdit.Location = new System.Drawing.Point(77, 14);
			this.NameEdit.MaxLength = 250;
			this.NameEdit.Name = "NameEdit";
			this.NameEdit.Size = new System.Drawing.Size(250, 20);
			this.NameEdit.TabIndex = 1;
			this.MainToolTip.SetToolTip(this.NameEdit, "Total name.");
			// 
			// TotalFuncLabel
			// 
			this.TotalFuncLabel.AutoSize = true;
			this.TotalFuncLabel.Location = new System.Drawing.Point(10, 43);
			this.TotalFuncLabel.Name = "TotalFuncLabel";
			this.TotalFuncLabel.Size = new System.Drawing.Size(51, 13);
			this.TotalFuncLabel.TabIndex = 2;
			this.TotalFuncLabel.Text = "Function:";
			this.MainToolTip.SetToolTip(this.TotalFuncLabel, "Aggregate function.");
			// 
			// ExpressionLabel
			// 
			this.ExpressionLabel.AutoSize = true;
			this.ExpressionLabel.Location = new System.Drawing.Point(10, 97);
			this.ExpressionLabel.Name = "ExpressionLabel";
			this.ExpressionLabel.Size = new System.Drawing.Size(61, 13);
			this.ExpressionLabel.TabIndex = 6;
			this.ExpressionLabel.Text = "Expression:";
			this.MainToolTip.SetToolTip(this.ExpressionLabel, "Data property or expression.");
			// 
			// DataBandLabel
			// 
			this.DataBandLabel.AutoSize = true;
			this.DataBandLabel.Location = new System.Drawing.Point(10, 70);
			this.DataBandLabel.Name = "DataBandLabel";
			this.DataBandLabel.Size = new System.Drawing.Size(60, 13);
			this.DataBandLabel.TabIndex = 4;
			this.DataBandLabel.Text = "Data band:";
			this.MainToolTip.SetToolTip(this.DataBandLabel, "Evaluate on each row of the band.");
			// 
			// PrintBandLabel
			// 
			this.PrintBandLabel.AutoSize = true;
			this.PrintBandLabel.Location = new System.Drawing.Point(10, 123);
			this.PrintBandLabel.Name = "PrintBandLabel";
			this.PrintBandLabel.Size = new System.Drawing.Size(58, 13);
			this.PrintBandLabel.TabIndex = 8;
			this.PrintBandLabel.Text = "Print band:";
			this.MainToolTip.SetToolTip(this.PrintBandLabel, "Print on the band.");
			// 
			// TotalFuncEdit
			// 
			this.TotalFuncEdit.ItemFormatString = null;
			this.TotalFuncEdit.Location = new System.Drawing.Point(77, 40);
			this.TotalFuncEdit.Name = "TotalFuncEdit";
			this.TotalFuncEdit.Size = new System.Drawing.Size(250, 21);
			this.TotalFuncEdit.TabIndex = 3;
			this.MainToolTip.SetToolTip(this.TotalFuncEdit, "Aggregate function.");
			this.TotalFuncEdit.SelectedIndexChanged += new System.EventHandler(this.OnFunctionChanged);
			// 
			// PrintBandEdit
			// 
			this.PrintBandEdit.ItemFormatString = "{Name}";
			this.PrintBandEdit.Location = new System.Drawing.Point(77, 120);
			this.PrintBandEdit.Name = "PrintBandEdit";
			this.PrintBandEdit.Size = new System.Drawing.Size(250, 21);
			this.PrintBandEdit.TabIndex = 9;
			this.MainToolTip.SetToolTip(this.PrintBandEdit, "Print on the band.");
			// 
			// DataBandEdit
			// 
			this.DataBandEdit.ItemFormatString = "{Name}: {DataSourceFullName}";
			this.DataBandEdit.Location = new System.Drawing.Point(77, 67);
			this.DataBandEdit.Name = "DataBandEdit";
			this.DataBandEdit.Size = new System.Drawing.Size(250, 21);
			this.DataBandEdit.TabIndex = 5;
			this.MainToolTip.SetToolTip(this.DataBandEdit, "Evaluate on each row of the band.");
			// 
			// ViewHelp
			// 
			this.ViewHelp.Dock = System.Windows.Forms.DockStyle.Top;
			this.ViewHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.ViewHelp.Location = new System.Drawing.Point(0, 0);
			this.ViewHelp.Name = "ViewHelp";
			this.ViewHelp.Padding = new System.Windows.Forms.Padding(10);
			this.ViewHelp.Size = new System.Drawing.Size(340, 40);
			this.ViewHelp.TabIndex = 0;
			this.ViewHelp.Text = "Change name, function, and other options";
			this.ViewHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MainPanel
			// 
			this.MainPanel.Controls.Add(this.ExpressionEdit);
			this.MainPanel.Controls.Add(this.TotalFuncEdit);
			this.MainPanel.Controls.Add(this.PrintBandEdit);
			this.MainPanel.Controls.Add(this.DataBandEdit);
			this.MainPanel.Controls.Add(this.NameLabel);
			this.MainPanel.Controls.Add(this.TotalFuncLabel);
			this.MainPanel.Controls.Add(this.ExpressionLabel);
			this.MainPanel.Controls.Add(this.DataBandLabel);
			this.MainPanel.Controls.Add(this.PrintBandLabel);
			this.MainPanel.Controls.Add(this.NameEdit);
			this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainPanel.Location = new System.Drawing.Point(0, 40);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(340, 155);
			this.MainPanel.TabIndex = 1;
			// 
			// ExpressionEdit
			// 
			this.ExpressionEdit.Location = new System.Drawing.Point(77, 94);
			this.ExpressionEdit.Name = "ExpressionEdit";
			this.ExpressionEdit.Size = new System.Drawing.Size(250, 20);
			this.ExpressionEdit.TabIndex = 7;
			this.MainToolTip.SetToolTip(this.ExpressionEdit, "Data property or expression.");
			// 
			// TotalView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.MainPanel);
			this.Controls.Add(this.ViewHelp);
			this.Name = "TotalView";
			this.Size = new System.Drawing.Size(340, 195);
			this.MainPanel.ResumeLayout(false);
			this.MainPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label NameLabel;
		private System.Windows.Forms.TextBox NameEdit;
		private System.Windows.Forms.Label TotalFuncLabel;
		private System.Windows.Forms.Label ExpressionLabel;
		private System.Windows.Forms.Label DataBandLabel;
		private System.Windows.Forms.Label PrintBandLabel;
		private System.Windows.Forms.ToolTip MainToolTip;
		private System.Windows.Forms.Label ViewHelp;
		private System.Windows.Forms.Panel MainPanel;
		private Controls.FormattedComboBox DataBandEdit;
		private Controls.FormattedComboBox PrintBandEdit;
		private Controls.FormattedComboBox TotalFuncEdit;
		private Controls.ExpressionEditElement ExpressionEdit;
	}
}