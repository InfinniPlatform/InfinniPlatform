namespace InfinniPlatform.DesignControls.PropertyDesigner
{
	partial class PropertiesControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertiesControl));
			this.PanelDataControl = new DevExpress.XtraEditors.PanelControl();
			this.ControlNameLabel = new DevExpress.XtraEditors.LabelControl();
			this.ButtonProperties = new DevExpress.XtraEditors.SimpleButton();
			((System.ComponentModel.ISupportInitialize)(this.PanelDataControl)).BeginInit();
			this.PanelDataControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// PanelDataControl
			// 
			this.PanelDataControl.Appearance.BackColor = System.Drawing.Color.White;
			this.PanelDataControl.Appearance.BorderColor = System.Drawing.Color.White;
			this.PanelDataControl.Appearance.Options.UseBackColor = true;
			this.PanelDataControl.Appearance.Options.UseBorderColor = true;
			this.PanelDataControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
			this.PanelDataControl.Controls.Add(this.ControlNameLabel);
			this.PanelDataControl.Controls.Add(this.ButtonProperties);
			this.PanelDataControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PanelDataControl.Location = new System.Drawing.Point(0, 0);
			this.PanelDataControl.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Style3D;
			this.PanelDataControl.LookAndFeel.UseDefaultLookAndFeel = false;
			this.PanelDataControl.Margin = new System.Windows.Forms.Padding(0);
			this.PanelDataControl.Name = "PanelDataControl";
			this.PanelDataControl.Padding = new System.Windows.Forms.Padding(5);
			this.PanelDataControl.Size = new System.Drawing.Size(543, 364);
			this.PanelDataControl.TabIndex = 0;
			// 
			// ControlNameLabel
			// 
			this.ControlNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ControlNameLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.ControlNameLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.ControlNameLabel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.ControlNameLabel.Location = new System.Drawing.Point(398, 338);
			this.ControlNameLabel.Name = "ControlNameLabel";
			this.ControlNameLabel.Size = new System.Drawing.Size(104, 18);
			this.ControlNameLabel.TabIndex = 2;
			this.ControlNameLabel.Text = "labelControl1";
			// 
			// ButtonProperties
			// 
			this.ButtonProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonProperties.Image = ((System.Drawing.Image)(resources.GetObject("ButtonProperties.Image")));
			this.ButtonProperties.Location = new System.Drawing.Point(508, 333);
			this.ButtonProperties.LookAndFeel.SkinName = "Office 2013";
			this.ButtonProperties.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ButtonProperties.Name = "ButtonProperties";
			this.ButtonProperties.Size = new System.Drawing.Size(27, 23);
			this.ButtonProperties.TabIndex = 1;
			this.ButtonProperties.Click += new System.EventHandler(this.ButtonProperties_Click);
			// 
			// PropertiesControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.PanelDataControl);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "PropertiesControl";
			this.Size = new System.Drawing.Size(543, 364);
			((System.ComponentModel.ISupportInitialize)(this.PanelDataControl)).EndInit();
			this.PanelDataControl.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.PanelControl PanelDataControl;
		private DevExpress.XtraEditors.SimpleButton ButtonProperties;
		private DevExpress.XtraEditors.LabelControl ControlNameLabel;
	}
}
