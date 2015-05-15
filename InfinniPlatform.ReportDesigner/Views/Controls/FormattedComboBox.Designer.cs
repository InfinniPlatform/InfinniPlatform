namespace InfinniPlatform.ReportDesigner.Views.Controls
{
	partial class FormattedComboBox
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
			this.ComboBoxEdit = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// ComboBoxEdit
			// 
			this.ComboBoxEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ComboBoxEdit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ComboBoxEdit.FormattingEnabled = true;
			this.ComboBoxEdit.Location = new System.Drawing.Point(0, 0);
			this.ComboBoxEdit.Name = "ComboBoxEdit";
			this.ComboBoxEdit.Size = new System.Drawing.Size(150, 21);
			this.ComboBoxEdit.TabIndex = 0;
			this.ComboBoxEdit.SelectedIndexChanged += new System.EventHandler(this.OnSelectedIndexChanged);
			// 
			// FormattedComboBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ComboBoxEdit);
			this.Name = "FormattedComboBox";
			this.Size = new System.Drawing.Size(150, 21);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox ComboBoxEdit;
	}
}
