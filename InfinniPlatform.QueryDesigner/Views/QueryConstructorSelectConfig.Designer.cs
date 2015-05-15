namespace InfinniPlatform.QueryDesigner.Views
{
	partial class QueryConstructorSelectConfig
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
			this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
			this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
			this.SelectPart = new InfinniPlatform.QueryDesigner.Views.QueryConstructorControlsContainer();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.panelControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
			this.panelControl2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelControl1
			// 
			this.panelControl1.Controls.Add(this.SelectPart);
			this.panelControl1.Controls.Add(this.panelControl2);
			this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelControl1.Location = new System.Drawing.Point(0, 0);
			this.panelControl1.LookAndFeel.SkinName = "Office 2013";
			this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(613, 399);
			this.panelControl1.TabIndex = 0;
			// 
			// panelControl2
			// 
			this.panelControl2.Controls.Add(this.simpleButton1);
			this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelControl2.Location = new System.Drawing.Point(2, 360);
			this.panelControl2.LookAndFeel.SkinName = "Office 2013";
			this.panelControl2.LookAndFeel.UseDefaultLookAndFeel = false;
			this.panelControl2.Name = "panelControl2";
			this.panelControl2.Size = new System.Drawing.Size(609, 37);
			this.panelControl2.TabIndex = 1;
			// 
			// simpleButton1
			// 
			this.simpleButton1.Location = new System.Drawing.Point(6, 9);
			this.simpleButton1.LookAndFeel.SkinName = "Office 2013";
			this.simpleButton1.LookAndFeel.UseDefaultLookAndFeel = false;
			this.simpleButton1.Name = "simpleButton1";
			this.simpleButton1.Size = new System.Drawing.Size(95, 23);
			this.simpleButton1.TabIndex = 0;
			this.simpleButton1.Text = "Add select";
			this.simpleButton1.Click += new System.EventHandler(this.AddConditionButtonClick);
			// 
			// SelectPart
			// 
			this.SelectPart.ControlType = null;
			this.SelectPart.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SelectPart.Location = new System.Drawing.Point(2, 2);
			this.SelectPart.Name = "SelectPart";
			this.SelectPart.OnItemAdded = null;
			this.SelectPart.OnItemDeleted = null;
			this.SelectPart.Size = new System.Drawing.Size(609, 358);
			this.SelectPart.TabIndex = 0;
			// 
			// QueryConstructorSelectConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panelControl1);
			this.Name = "QueryConstructorSelectConfig";
			this.Size = new System.Drawing.Size(613, 399);
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.panelControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
			this.panelControl2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.PanelControl panelControl1;
		private QueryConstructorControlsContainer SelectPart;
		private DevExpress.XtraEditors.PanelControl panelControl2;
		private DevExpress.XtraEditors.SimpleButton simpleButton1;
	}
}
