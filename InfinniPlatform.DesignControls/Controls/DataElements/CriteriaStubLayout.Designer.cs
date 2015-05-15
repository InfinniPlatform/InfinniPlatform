namespace InfinniPlatform.DesignControls.Controls.DataElements
{
	partial class CriteriaStubLayout
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
			this.imageComboBoxEdit1 = new DevExpress.XtraEditors.ImageComboBoxEdit();
			this.CaptionLabel = new DevExpress.XtraEditors.LabelControl();
			((System.ComponentModel.ISupportInitialize)(this.imageComboBoxEdit1.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// imageComboBoxEdit1
			// 
			this.imageComboBoxEdit1.Dock = System.Windows.Forms.DockStyle.Right;
			this.imageComboBoxEdit1.Location = new System.Drawing.Point(141, 5);
			this.imageComboBoxEdit1.Name = "imageComboBoxEdit1";
			this.imageComboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.imageComboBoxEdit1.Size = new System.Drawing.Size(94, 20);
			this.imageComboBoxEdit1.TabIndex = 0;
			// 
			// labelControl1
			// 
			this.CaptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.CaptionLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.CaptionLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.CaptionLabel.Location = new System.Drawing.Point(8, 6);
			this.CaptionLabel.Name = "CaptionLabel";
			this.CaptionLabel.Size = new System.Drawing.Size(127, 19);
			this.CaptionLabel.TabIndex = 1;
			this.CaptionLabel.Text = "Caption";
			// 
			// CriteriaStubLayout
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.CaptionLabel);
			this.Controls.Add(this.imageComboBoxEdit1);
			this.Name = "CriteriaStubLayout";
			this.Padding = new System.Windows.Forms.Padding(5);
			this.Size = new System.Drawing.Size(240, 30);
			((System.ComponentModel.ISupportInitialize)(this.imageComboBoxEdit1.Properties)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.ImageComboBoxEdit imageComboBoxEdit1;
		private DevExpress.XtraEditors.LabelControl CaptionLabel;
	}
}
