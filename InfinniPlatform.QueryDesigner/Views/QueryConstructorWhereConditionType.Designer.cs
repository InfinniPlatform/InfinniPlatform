namespace InfinniPlatform.QueryDesigner.Views
{
	partial class QueryConstructorWhereConditionType
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
			this.ComboBoxConditionType = new DevExpress.XtraEditors.ImageComboBoxEdit();
			((System.ComponentModel.ISupportInitialize)(this.PanelControl)).BeginInit();
			this.PanelControl.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxConditionType.Properties)).BeginInit();
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
			this.PanelControl.Controls.Add(this.ComboBoxConditionType);
			this.PanelControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PanelControl.Location = new System.Drawing.Point(0, 0);
			this.PanelControl.LookAndFeel.SkinName = "Office 2013";
			this.PanelControl.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
			this.PanelControl.LookAndFeel.UseDefaultLookAndFeel = false;
			this.PanelControl.Name = "PanelControl";
			this.PanelControl.Size = new System.Drawing.Size(246, 33);
			this.PanelControl.TabIndex = 1;
			// 
			// ComboBoxConditionType
			// 
			this.ComboBoxConditionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ComboBoxConditionType.Location = new System.Drawing.Point(6, 7);
			this.ComboBoxConditionType.Name = "ComboBoxConditionType";
			this.ComboBoxConditionType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
			this.ComboBoxConditionType.Size = new System.Drawing.Size(234, 20);
			this.ComboBoxConditionType.TabIndex = 13;
			// 
			// QueryConstructorWhereConditionType
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.PanelControl);
			this.Name = "QueryConstructorWhereConditionType";
			this.Size = new System.Drawing.Size(246, 33);
			((System.ComponentModel.ISupportInitialize)(this.PanelControl)).EndInit();
			this.PanelControl.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxConditionType.Properties)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.PanelControl PanelControl;
		private DevExpress.XtraEditors.ImageComboBoxEdit ComboBoxConditionType;

	}
}
