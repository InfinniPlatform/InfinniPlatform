namespace InfinniPlatform.ReportDesigner.Views.Parameters
{
	partial class ParameterValuesView
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
			this.ValuesPanel = new System.Windows.Forms.Panel();
			this.ValuesTypePanel = new System.Windows.Forms.Panel();
			this.ValueProviderTypeLabel = new System.Windows.Forms.Label();
			this.DataSourceValuesRadioButton = new System.Windows.Forms.RadioButton();
			this.ConstantValuesRadioButton = new System.Windows.Forms.RadioButton();
			this.NoneValuesRadioButton = new System.Windows.Forms.RadioButton();
			this.ValuesTypePanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// ValuesPanel
			// 
			this.ValuesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ValuesPanel.Location = new System.Drawing.Point(0, 110);
			this.ValuesPanel.Name = "ValuesPanel";
			this.ValuesPanel.Size = new System.Drawing.Size(450, 190);
			this.ValuesPanel.TabIndex = 1;
			// 
			// ValuesTypePanel
			// 
			this.ValuesTypePanel.Controls.Add(this.ValueProviderTypeLabel);
			this.ValuesTypePanel.Controls.Add(this.DataSourceValuesRadioButton);
			this.ValuesTypePanel.Controls.Add(this.ConstantValuesRadioButton);
			this.ValuesTypePanel.Controls.Add(this.NoneValuesRadioButton);
			this.ValuesTypePanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.ValuesTypePanel.Location = new System.Drawing.Point(0, 0);
			this.ValuesTypePanel.Name = "ValuesTypePanel";
			this.ValuesTypePanel.Size = new System.Drawing.Size(450, 110);
			this.ValuesTypePanel.TabIndex = 0;
			// 
			// ValueProviderTypeLabel
			// 
			this.ValueProviderTypeLabel.AutoSize = true;
			this.ValueProviderTypeLabel.Location = new System.Drawing.Point(10, 17);
			this.ValueProviderTypeLabel.Name = "ValueProviderTypeLabel";
			this.ValueProviderTypeLabel.Size = new System.Drawing.Size(195, 13);
			this.ValueProviderTypeLabel.TabIndex = 0;
			this.ValueProviderTypeLabel.Text = "Select from one of the following options:";
			// 
			// DataSourceValuesRadioButton
			// 
			this.DataSourceValuesRadioButton.AutoSize = true;
			this.DataSourceValuesRadioButton.Location = new System.Drawing.Point(13, 79);
			this.DataSourceValuesRadioButton.Name = "DataSourceValuesRadioButton";
			this.DataSourceValuesRadioButton.Size = new System.Drawing.Size(137, 17);
			this.DataSourceValuesRadioButton.TabIndex = 3;
			this.DataSourceValuesRadioButton.Text = "Get values from a query";
			this.DataSourceValuesRadioButton.UseVisualStyleBackColor = true;
			this.DataSourceValuesRadioButton.CheckedChanged += new System.EventHandler(this.OnSelectParameterValues);
			// 
			// ConstantValuesRadioButton
			// 
			this.ConstantValuesRadioButton.AutoSize = true;
			this.ConstantValuesRadioButton.Location = new System.Drawing.Point(13, 56);
			this.ConstantValuesRadioButton.Name = "ConstantValuesRadioButton";
			this.ConstantValuesRadioButton.Size = new System.Drawing.Size(94, 17);
			this.ConstantValuesRadioButton.TabIndex = 2;
			this.ConstantValuesRadioButton.Text = "Specify values";
			this.ConstantValuesRadioButton.UseVisualStyleBackColor = true;
			this.ConstantValuesRadioButton.CheckedChanged += new System.EventHandler(this.OnSelectParameterValues);
			// 
			// NoneValuesRadioButton
			// 
			this.NoneValuesRadioButton.AutoSize = true;
			this.NoneValuesRadioButton.Checked = true;
			this.NoneValuesRadioButton.Location = new System.Drawing.Point(13, 33);
			this.NoneValuesRadioButton.Name = "NoneValuesRadioButton";
			this.NoneValuesRadioButton.Size = new System.Drawing.Size(51, 17);
			this.NoneValuesRadioButton.TabIndex = 1;
			this.NoneValuesRadioButton.TabStop = true;
			this.NoneValuesRadioButton.Text = "None";
			this.NoneValuesRadioButton.UseVisualStyleBackColor = true;
			this.NoneValuesRadioButton.CheckedChanged += new System.EventHandler(this.OnSelectParameterValues);
			// 
			// ParameterValuesView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ValuesPanel);
			this.Controls.Add(this.ValuesTypePanel);
			this.Name = "ParameterValuesView";
			this.Size = new System.Drawing.Size(450, 300);
			this.ValuesTypePanel.ResumeLayout(false);
			this.ValuesTypePanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel ValuesPanel;
		private System.Windows.Forms.Panel ValuesTypePanel;
		private System.Windows.Forms.RadioButton DataSourceValuesRadioButton;
		private System.Windows.Forms.RadioButton ConstantValuesRadioButton;
		private System.Windows.Forms.RadioButton NoneValuesRadioButton;
		private System.Windows.Forms.Label ValueProviderTypeLabel;
	}
}
