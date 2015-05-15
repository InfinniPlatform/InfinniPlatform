namespace InfinniPlatform.QueryDesigner.Views
{
	partial class QueryJsonEditor
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
			this.JsonEdit = new DevExpress.XtraEditors.MemoEdit();
			((System.ComponentModel.ISupportInitialize)(this.JsonEdit.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// JsonEdit
			// 
			this.JsonEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.JsonEdit.Location = new System.Drawing.Point(0, 0);
			this.JsonEdit.Name = "JsonEdit";
			this.JsonEdit.Properties.AcceptsTab = true;
			this.JsonEdit.Properties.Appearance.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.JsonEdit.Properties.Appearance.Options.UseFont = true;
			this.JsonEdit.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.JsonEdit.Properties.LookAndFeel.SkinName = "Office 2013";
			this.JsonEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
			this.JsonEdit.Properties.ReadOnly = true;
			this.JsonEdit.Properties.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.JsonEdit.Properties.WordWrap = false;
			this.JsonEdit.Size = new System.Drawing.Size(537, 380);
			this.JsonEdit.TabIndex = 0;
			this.JsonEdit.UseOptimizedRendering = true;
			// 
			// QueryJsonEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.JsonEdit);
			this.Name = "QueryJsonEditor";
			this.Size = new System.Drawing.Size(537, 380);
			((System.ComponentModel.ISupportInitialize)(this.JsonEdit.Properties)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.MemoEdit JsonEdit;
	}
}
