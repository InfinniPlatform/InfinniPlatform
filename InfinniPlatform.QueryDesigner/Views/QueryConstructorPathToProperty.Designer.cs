namespace InfinniPlatform.QueryDesigner.Views
{
	partial class QueryConstructorPathToProperty
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
			this.PanelControl = new DevExpress.XtraEditors.PanelControl();
			this.CaptionLabel = new DevExpress.XtraEditors.LabelControl();
			this.ComboBoxPath = new DevExpress.XtraEditors.ImageComboBoxEdit();
			((System.ComponentModel.ISupportInitialize)(this.PanelControl)).BeginInit();
			this.PanelControl.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxPath.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// PanelControl
			// 
			this.PanelControl.Appearance.BackColor = System.Drawing.Color.White;
			this.PanelControl.Appearance.BackColor2 = System.Drawing.Color.White;
			this.PanelControl.Appearance.BorderColor = System.Drawing.Color.White;
			this.PanelControl.Appearance.ForeColor = System.Drawing.Color.Black;
			this.PanelControl.Appearance.Options.UseBackColor = true;
			this.PanelControl.Appearance.Options.UseBorderColor = true;
			this.PanelControl.Appearance.Options.UseForeColor = true;
			this.PanelControl.Controls.Add(this.CaptionLabel);
			this.PanelControl.Controls.Add(this.ComboBoxPath);
			this.PanelControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PanelControl.Location = new System.Drawing.Point(0, 0);
			this.PanelControl.LookAndFeel.SkinName = "Office 2013";
			this.PanelControl.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
			this.PanelControl.LookAndFeel.UseDefaultLookAndFeel = false;
			this.PanelControl.Name = "PanelControl";
			this.PanelControl.Size = new System.Drawing.Size(415, 33);
			this.PanelControl.TabIndex = 0;
			// 
			// CaptionLabel
			// 
			this.CaptionLabel.Location = new System.Drawing.Point(15, 9);
			this.CaptionLabel.Name = "CaptionLabel";
			this.CaptionLabel.Size = new System.Drawing.Size(71, 13);
			this.CaptionLabel.TabIndex = 14;
			this.CaptionLabel.Text = "Document field";
			// 
			// ComboBoxPath
			// 
			this.ComboBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ComboBoxPath.Location = new System.Drawing.Point(170, 7);
			this.ComboBoxPath.Name = "ComboBoxPath";
			this.ComboBoxPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
			this.ComboBoxPath.Size = new System.Drawing.Size(235, 20);
			this.ComboBoxPath.TabIndex = 13;
			// 
			// QueryConstructorPathToProperty
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.PanelControl);
			this.Name = "QueryConstructorPathToProperty";
			this.Size = new System.Drawing.Size(415, 33);
			((System.ComponentModel.ISupportInitialize)(this.PanelControl)).EndInit();
			this.PanelControl.ResumeLayout(false);
			this.PanelControl.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxPath.Properties)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.PanelControl PanelControl;
		private DevExpress.XtraEditors.LabelControl CaptionLabel;
		private DevExpress.XtraEditors.ImageComboBoxEdit ComboBoxPath;
	}
}
