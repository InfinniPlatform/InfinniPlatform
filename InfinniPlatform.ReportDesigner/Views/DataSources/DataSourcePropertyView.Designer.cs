namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
	partial class DataSourcePropertyView
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
			this.DataTypeEdit = new System.Windows.Forms.ComboBox();
			this.NameEdit = new System.Windows.Forms.TextBox();
			this.DataTypeLabel = new System.Windows.Forms.Label();
			this.NameLabel = new System.Windows.Forms.Label();
			this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// DataTypeEdit
			// 
			this.DataTypeEdit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.DataTypeEdit.Items.AddRange(new object[] {
            "String",
            "Float",
            "Integer",
            "Boolean",
            "DateTime",
            "Object",
            "Array"});
			this.DataTypeEdit.Location = new System.Drawing.Point(74, 40);
			this.DataTypeEdit.Name = "DataTypeEdit";
			this.DataTypeEdit.Size = new System.Drawing.Size(150, 21);
			this.DataTypeEdit.TabIndex = 3;
			this.MainToolTip.SetToolTip(this.DataTypeEdit, "Property data type.");
			// 
			// NameEdit
			// 
			this.NameEdit.Location = new System.Drawing.Point(74, 14);
			this.NameEdit.MaxLength = 250;
			this.NameEdit.Name = "NameEdit";
			this.NameEdit.Size = new System.Drawing.Size(195, 20);
			this.NameEdit.TabIndex = 1;
			this.MainToolTip.SetToolTip(this.NameEdit, "Property name.");
			// 
			// DataTypeLabel
			// 
			this.DataTypeLabel.AutoSize = true;
			this.DataTypeLabel.Location = new System.Drawing.Point(10, 43);
			this.DataTypeLabel.Name = "DataTypeLabel";
			this.DataTypeLabel.Size = new System.Drawing.Size(56, 13);
			this.DataTypeLabel.TabIndex = 2;
			this.DataTypeLabel.Text = "Data type:";
			this.MainToolTip.SetToolTip(this.DataTypeLabel, "Property data type.");
			// 
			// NameLabel
			// 
			this.NameLabel.AutoSize = true;
			this.NameLabel.Location = new System.Drawing.Point(10, 17);
			this.NameLabel.Name = "NameLabel";
			this.NameLabel.Size = new System.Drawing.Size(38, 13);
			this.NameLabel.TabIndex = 0;
			this.NameLabel.Text = "Name:";
			this.MainToolTip.SetToolTip(this.NameLabel, "Property name.");
			// 
			// DataSourcePropertyView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.DataTypeEdit);
			this.Controls.Add(this.NameEdit);
			this.Controls.Add(this.NameLabel);
			this.Controls.Add(this.DataTypeLabel);
			this.Name = "DataSourcePropertyView";
			this.Size = new System.Drawing.Size(284, 74);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox NameEdit;
		private System.Windows.Forms.Label NameLabel;
		private System.Windows.Forms.ComboBox DataTypeEdit;
		private System.Windows.Forms.Label DataTypeLabel;
		private System.Windows.Forms.ToolTip MainToolTip;
	}
}