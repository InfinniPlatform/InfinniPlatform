namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
	partial class DataSourcePropertySelectView
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
			this.DataSchemaEdit = new InfinniPlatform.ReportDesigner.Views.DataSources.DataSourceSchemaView();
			this.SuspendLayout();
			// 
			// DataSchemaEdit
			// 
			this.DataSchemaEdit.DataSourceName = null;
			this.DataSchemaEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DataSchemaEdit.Location = new System.Drawing.Point(0, 0);
			this.DataSchemaEdit.Name = "DataSchemaEdit";
			this.DataSchemaEdit.SelectedPropertyPath = null;
			this.DataSchemaEdit.Size = new System.Drawing.Size(334, 333);
			this.DataSchemaEdit.TabIndex = 0;
			// 
			// DataSourcePropertySelectView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(334, 333);
			this.Controls.Add(this.DataSchemaEdit);
			this.Name = "DataSourcePropertySelectView";
			this.Text = "Select data source property";
			this.ResumeLayout(false);

		}

		#endregion

		private DataSources.DataSourceSchemaView DataSchemaEdit;

	}
}