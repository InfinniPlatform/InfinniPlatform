namespace InfinniPlatform.DesignControls.Controls.DataElements
{
	partial class SearchPanelElement
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchPanelElement));
			this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
			this.SearchButton = new DevExpress.XtraEditors.SimpleButton();
			((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// textEdit1
			// 
			this.textEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textEdit1.Location = new System.Drawing.Point(28, 5);
			this.textEdit1.Name = "textEdit1";
			this.textEdit1.Properties.ReadOnly = true;
			this.textEdit1.Size = new System.Drawing.Size(98, 20);
			this.textEdit1.TabIndex = 0;
			// 
			// SearchButton
			// 
			this.SearchButton.Dock = System.Windows.Forms.DockStyle.Left;
			this.SearchButton.Image = ((System.Drawing.Image)(resources.GetObject("SearchButton.Image")));
			this.SearchButton.Location = new System.Drawing.Point(5, 5);
			this.SearchButton.Name = "SearchButton";
			this.SearchButton.Size = new System.Drawing.Size(23, 21);
			this.SearchButton.TabIndex = 1;
			// 
			// SearchPanelElement
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.textEdit1);
			this.Controls.Add(this.SearchButton);
			this.Name = "SearchPanelElement";
			this.Padding = new System.Windows.Forms.Padding(5);
			this.Size = new System.Drawing.Size(131, 31);
			((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.TextEdit textEdit1;
		private DevExpress.XtraEditors.SimpleButton SearchButton;
	}
}
