namespace InfinniPlatform.QueryDesigner.Views
{
	partial class QueryConstructorWhereConfig
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
			this.QueryConstructorWhere = new InfinniPlatform.QueryDesigner.Views.QueryConstructorControlsContainer();
			this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
			this.AddConditionButton = new DevExpress.XtraEditors.SimpleButton();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.panelControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// QueryConstructorWhere
			// 
			this.QueryConstructorWhere.ControlType = null;
			this.QueryConstructorWhere.Dock = System.Windows.Forms.DockStyle.Fill;
			this.QueryConstructorWhere.Location = new System.Drawing.Point(0, 0);
			this.QueryConstructorWhere.Name = "QueryConstructorWhere";
			this.QueryConstructorWhere.OnItemAdded = null;
			this.QueryConstructorWhere.OnItemDeleted = null;
			this.QueryConstructorWhere.Size = new System.Drawing.Size(626, 261);
			this.QueryConstructorWhere.TabIndex = 0;
			// 
			// panelControl1
			// 
			this.panelControl1.Controls.Add(this.AddConditionButton);
			this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelControl1.Location = new System.Drawing.Point(0, 261);
			this.panelControl1.LookAndFeel.SkinName = "Office 2013";
			this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(626, 35);
			this.panelControl1.TabIndex = 1;
			// 
			// AddConditionButton
			// 
			this.AddConditionButton.Location = new System.Drawing.Point(5, 5);
			this.AddConditionButton.LookAndFeel.SkinName = "Office 2013";
			this.AddConditionButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.AddConditionButton.Name = "AddConditionButton";
			this.AddConditionButton.Size = new System.Drawing.Size(100, 23);
			this.AddConditionButton.TabIndex = 0;
			this.AddConditionButton.Text = "Add condition";
			this.AddConditionButton.Click += new System.EventHandler(this.AddConditionButtonClick);
			// 
			// QueryConstructorWhereConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.QueryConstructorWhere);
			this.Controls.Add(this.panelControl1);
			this.Name = "QueryConstructorWhereConfig";
			this.Size = new System.Drawing.Size(626, 296);
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.panelControl1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private QueryConstructorControlsContainer QueryConstructorWhere;
		private DevExpress.XtraEditors.PanelControl panelControl1;
		private DevExpress.XtraEditors.SimpleButton AddConditionButton;

	}
}
