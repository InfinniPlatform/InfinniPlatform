namespace InfinniPlatform.QueryDesigner.Views
{
	partial class QueryConstructorJoinConfig
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
			this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
			this.PathConstructor = new InfinniPlatform.QueryDesigner.Views.QueryConstructorControlsContainer();
			this.IndexConfigPart = new InfinniPlatform.QueryDesigner.Views.QueryConstructorIndexConfig();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.panelControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelControl1
			// 
			this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
			this.panelControl1.Controls.Add(this.PathConstructor);
			this.panelControl1.Controls.Add(this.IndexConfigPart);
			this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelControl1.Location = new System.Drawing.Point(0, 0);
			this.panelControl1.LookAndFeel.SkinName = "Office 2013";
			this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(576, 356);
			this.panelControl1.TabIndex = 0;
			// 
			// PathConstructor
			// 
			this.PathConstructor.ControlType = null;
			this.PathConstructor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PathConstructor.Location = new System.Drawing.Point(0, 105);
			this.PathConstructor.Name = "PathConstructor";
			this.PathConstructor.Size = new System.Drawing.Size(576, 251);
			this.PathConstructor.TabIndex = 9;
			// 
			// IndexConfigPart
			// 
			this.IndexConfigPart.DataProvider = null;
			this.IndexConfigPart.Dock = System.Windows.Forms.DockStyle.Top;
			this.IndexConfigPart.Location = new System.Drawing.Point(0, 0);
			this.IndexConfigPart.Name = "IndexConfigPart";
			this.IndexConfigPart.OnConfigurationValueChanged = null;
			this.IndexConfigPart.OnDocumentValueChanged = null;
			this.IndexConfigPart.Size = new System.Drawing.Size(576, 105);
			this.IndexConfigPart.TabIndex = 10;
			// 
			// QueryConstructorJoinConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panelControl1);
			this.Name = "QueryConstructorJoinConfig";
			this.Size = new System.Drawing.Size(576, 356);
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.panelControl1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.PanelControl panelControl1;
		private QueryConstructorControlsContainer PathConstructor;
		private QueryConstructorIndexConfig IndexConfigPart;
	}
}
