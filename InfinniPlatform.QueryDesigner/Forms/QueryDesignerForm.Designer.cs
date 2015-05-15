namespace InfinniPlatform.QueryDesigner.Forms
{
	partial class QueryDesignerForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryDesignerForm));
			this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
			this.OKButton = new DevExpress.XtraEditors.SimpleButton();
			this.designControl1 = new InfinniPlatform.QueryDesigner.Views.DesignControl();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.panelControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelControl1
			// 
			this.panelControl1.Controls.Add(this.OKButton);
			this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelControl1.Location = new System.Drawing.Point(0, 643);
			this.panelControl1.LookAndFeel.SkinName = "Office 2013";
			this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(1126, 37);
			this.panelControl1.TabIndex = 1;
			// 
			// OKButton
			// 
			this.OKButton.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
			this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OKButton.Image = ((System.Drawing.Image)(resources.GetObject("OKButton.Image")));
			this.OKButton.Location = new System.Drawing.Point(12, 9);
			this.OKButton.LookAndFeel.SkinName = "Office 2013";
			this.OKButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.OKButton.Name = "OKButton";
			this.OKButton.Size = new System.Drawing.Size(101, 23);
			this.OKButton.TabIndex = 2;
			this.OKButton.Text = "OK";
			// 
			// designControl1
			// 
			this.designControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.designControl1.Location = new System.Drawing.Point(0, 0);
			this.designControl1.Name = "designControl1";
			this.designControl1.Size = new System.Drawing.Size(1126, 643);
			this.designControl1.TabIndex = 0;
			// 
			// QueryDesignerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1126, 680);
			this.Controls.Add(this.designControl1);
			this.Controls.Add(this.panelControl1);
			this.Name = "QueryDesignerForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.panelControl1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Views.DesignControl designControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton OKButton;


	}
}

