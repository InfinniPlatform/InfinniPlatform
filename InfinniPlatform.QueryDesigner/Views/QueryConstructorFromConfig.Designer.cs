namespace InfinniPlatform.QueryDesigner.Views
{
	partial class QueryConstructorFromConfig
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
			this.IndexConfigPart = new InfinniPlatform.QueryDesigner.Views.QueryConstructorIndexConfig();
			this.SuspendLayout();
			// 
			// IndexConfigPart
			// 
			this.IndexConfigPart.DataProvider = null;
			this.IndexConfigPart.Dock = System.Windows.Forms.DockStyle.Fill;
			this.IndexConfigPart.Location = new System.Drawing.Point(0, 0);
			this.IndexConfigPart.Name = "IndexConfigPart";
			this.IndexConfigPart.OnConfigurationValueChanged = null;
			this.IndexConfigPart.OnDocumentValueChanged = null;
			this.IndexConfigPart.Size = new System.Drawing.Size(507, 112);
			this.IndexConfigPart.TabIndex = 0;
			// 
			// QueryConstructorFromConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.IndexConfigPart);
			this.Name = "QueryConstructorFromConfig";
			this.Size = new System.Drawing.Size(507, 112);
			this.ResumeLayout(false);

		}

		#endregion

		private QueryConstructorIndexConfig IndexConfigPart;
	}
}
