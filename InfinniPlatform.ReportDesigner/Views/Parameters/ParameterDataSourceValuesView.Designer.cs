namespace InfinniPlatform.ReportDesigner.Views.Parameters
{
	partial class ParameterDataSourceValuesView
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
			this.LabelPropertyEdit = new System.Windows.Forms.TextBox();
			this.DataSourceEdit = new System.Windows.Forms.ComboBox();
			this.DataSourceLabel = new System.Windows.Forms.Label();
			this.LabelPropertyButton = new System.Windows.Forms.Button();
			this.ValuePropertyLabel = new System.Windows.Forms.Label();
			this.LabelPropertyLabel = new System.Windows.Forms.Label();
			this.ValuePropertyEdit = new System.Windows.Forms.TextBox();
			this.ValuePropertyButton = new System.Windows.Forms.Button();
			this.DataSourcePanel = new System.Windows.Forms.Panel();
			this.LabelPropertyPanel = new System.Windows.Forms.Panel();
			this.ValuePropertyPanel = new System.Windows.Forms.Panel();
			this.DataSourcePanel.SuspendLayout();
			this.LabelPropertyPanel.SuspendLayout();
			this.ValuePropertyPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// LabelPropertyEdit
			// 
			this.LabelPropertyEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LabelPropertyEdit.Location = new System.Drawing.Point(84, 6);
			this.LabelPropertyEdit.Name = "LabelPropertyEdit";
			this.LabelPropertyEdit.ReadOnly = true;
			this.LabelPropertyEdit.Size = new System.Drawing.Size(224, 20);
			this.LabelPropertyEdit.TabIndex = 1;
			// 
			// DataSourceEdit
			// 
			this.DataSourceEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DataSourceEdit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.DataSourceEdit.FormattingEnabled = true;
			this.DataSourceEdit.Location = new System.Drawing.Point(84, 14);
			this.DataSourceEdit.Name = "DataSourceEdit";
			this.DataSourceEdit.Size = new System.Drawing.Size(250, 21);
			this.DataSourceEdit.TabIndex = 1;
			this.DataSourceEdit.SelectedIndexChanged += new System.EventHandler(this.OnDataSourceChanged);
			// 
			// DataSourceLabel
			// 
			this.DataSourceLabel.AutoSize = true;
			this.DataSourceLabel.Location = new System.Drawing.Point(10, 17);
			this.DataSourceLabel.Name = "DataSourceLabel";
			this.DataSourceLabel.Size = new System.Drawing.Size(68, 13);
			this.DataSourceLabel.TabIndex = 0;
			this.DataSourceLabel.Text = "Data source:";
			// 
			// LabelPropertyButton
			// 
			this.LabelPropertyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.LabelPropertyButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.LabelPropertyButton.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Expressions;
			this.LabelPropertyButton.Location = new System.Drawing.Point(314, 6);
			this.LabelPropertyButton.Name = "LabelPropertyButton";
			this.LabelPropertyButton.Size = new System.Drawing.Size(20, 20);
			this.LabelPropertyButton.TabIndex = 2;
			this.LabelPropertyButton.Text = "...";
			this.LabelPropertyButton.UseVisualStyleBackColor = true;
			this.LabelPropertyButton.Click += new System.EventHandler(this.OnLabelPropertyEdit);
			// 
			// ValuePropertyLabel
			// 
			this.ValuePropertyLabel.AutoSize = true;
			this.ValuePropertyLabel.Location = new System.Drawing.Point(10, 9);
			this.ValuePropertyLabel.Name = "ValuePropertyLabel";
			this.ValuePropertyLabel.Size = new System.Drawing.Size(59, 13);
			this.ValuePropertyLabel.TabIndex = 0;
			this.ValuePropertyLabel.Text = "Value field:";
			// 
			// LabelPropertyLabel
			// 
			this.LabelPropertyLabel.AutoSize = true;
			this.LabelPropertyLabel.Location = new System.Drawing.Point(10, 10);
			this.LabelPropertyLabel.Name = "LabelPropertyLabel";
			this.LabelPropertyLabel.Size = new System.Drawing.Size(58, 13);
			this.LabelPropertyLabel.TabIndex = 0;
			this.LabelPropertyLabel.Text = "Label field:";
			// 
			// ValuePropertyEdit
			// 
			this.ValuePropertyEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ValuePropertyEdit.Location = new System.Drawing.Point(84, 6);
			this.ValuePropertyEdit.Name = "ValuePropertyEdit";
			this.ValuePropertyEdit.ReadOnly = true;
			this.ValuePropertyEdit.Size = new System.Drawing.Size(224, 20);
			this.ValuePropertyEdit.TabIndex = 1;
			// 
			// ValuePropertyButton
			// 
			this.ValuePropertyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ValuePropertyButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.ValuePropertyButton.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Expressions;
			this.ValuePropertyButton.Location = new System.Drawing.Point(314, 6);
			this.ValuePropertyButton.Name = "ValuePropertyButton";
			this.ValuePropertyButton.Size = new System.Drawing.Size(20, 20);
			this.ValuePropertyButton.TabIndex = 2;
			this.ValuePropertyButton.Text = "...";
			this.ValuePropertyButton.UseVisualStyleBackColor = true;
			this.ValuePropertyButton.Click += new System.EventHandler(this.OnValuePropertyEdit);
			// 
			// DataSourcePanel
			// 
			this.DataSourcePanel.Controls.Add(this.DataSourceLabel);
			this.DataSourcePanel.Controls.Add(this.DataSourceEdit);
			this.DataSourcePanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.DataSourcePanel.Location = new System.Drawing.Point(0, 0);
			this.DataSourcePanel.Name = "DataSourcePanel";
			this.DataSourcePanel.Size = new System.Drawing.Size(348, 35);
			this.DataSourcePanel.TabIndex = 0;
			// 
			// LabelPropertyPanel
			// 
			this.LabelPropertyPanel.Controls.Add(this.LabelPropertyEdit);
			this.LabelPropertyPanel.Controls.Add(this.LabelPropertyLabel);
			this.LabelPropertyPanel.Controls.Add(this.LabelPropertyButton);
			this.LabelPropertyPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.LabelPropertyPanel.Location = new System.Drawing.Point(0, 35);
			this.LabelPropertyPanel.Name = "LabelPropertyPanel";
			this.LabelPropertyPanel.Size = new System.Drawing.Size(348, 26);
			this.LabelPropertyPanel.TabIndex = 1;
			// 
			// ValuePropertyPanel
			// 
			this.ValuePropertyPanel.Controls.Add(this.ValuePropertyEdit);
			this.ValuePropertyPanel.Controls.Add(this.ValuePropertyButton);
			this.ValuePropertyPanel.Controls.Add(this.ValuePropertyLabel);
			this.ValuePropertyPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.ValuePropertyPanel.Location = new System.Drawing.Point(0, 61);
			this.ValuePropertyPanel.Name = "ValuePropertyPanel";
			this.ValuePropertyPanel.Size = new System.Drawing.Size(348, 26);
			this.ValuePropertyPanel.TabIndex = 2;
			// 
			// ParameterDataSourceValuesView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ValuePropertyPanel);
			this.Controls.Add(this.LabelPropertyPanel);
			this.Controls.Add(this.DataSourcePanel);
			this.Name = "ParameterDataSourceValuesView";
			this.Size = new System.Drawing.Size(348, 87);
			this.DataSourcePanel.ResumeLayout(false);
			this.DataSourcePanel.PerformLayout();
			this.LabelPropertyPanel.ResumeLayout(false);
			this.LabelPropertyPanel.PerformLayout();
			this.ValuePropertyPanel.ResumeLayout(false);
			this.ValuePropertyPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox LabelPropertyEdit;
		private System.Windows.Forms.ComboBox DataSourceEdit;
		private System.Windows.Forms.Label DataSourceLabel;
		private System.Windows.Forms.Button LabelPropertyButton;
		private System.Windows.Forms.Label ValuePropertyLabel;
		private System.Windows.Forms.Label LabelPropertyLabel;
		private System.Windows.Forms.TextBox ValuePropertyEdit;
		private System.Windows.Forms.Button ValuePropertyButton;
		private System.Windows.Forms.Panel DataSourcePanel;
		private System.Windows.Forms.Panel LabelPropertyPanel;
		private System.Windows.Forms.Panel ValuePropertyPanel;
	}
}
