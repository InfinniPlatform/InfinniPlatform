namespace InfinniPlatform.ReportDesigner.Views.Controls
{
	partial class ExpressionEditElement
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
			this.ExpressionBtn = new System.Windows.Forms.Button();
			this.ExpressionEdit = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// ExpressionBtn
			// 
			this.ExpressionBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ExpressionBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.ExpressionBtn.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Expressions;
			this.ExpressionBtn.Location = new System.Drawing.Point(130, 0);
			this.ExpressionBtn.Name = "ExpressionBtn";
			this.ExpressionBtn.Size = new System.Drawing.Size(20, 20);
			this.ExpressionBtn.TabIndex = 1;
			this.ExpressionBtn.UseVisualStyleBackColor = true;
			this.ExpressionBtn.Click += new System.EventHandler(this.OnExpressionButton);
			// 
			// ExpressionEdit
			// 
			this.ExpressionEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ExpressionEdit.Location = new System.Drawing.Point(0, 0);
			this.ExpressionEdit.Name = "ExpressionEdit";
			this.ExpressionEdit.Size = new System.Drawing.Size(124, 20);
			this.ExpressionEdit.TabIndex = 0;
			// 
			// ExpressionEditElement
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ExpressionBtn);
			this.Controls.Add(this.ExpressionEdit);
			this.Name = "ExpressionEditElement";
			this.Size = new System.Drawing.Size(150, 20);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button ExpressionBtn;
		private System.Windows.Forms.TextBox ExpressionEdit;
	}
}
