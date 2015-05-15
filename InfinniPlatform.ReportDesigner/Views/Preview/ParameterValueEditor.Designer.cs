namespace InfinniPlatform.ReportDesigner.Views.Preview
{
	partial class ParameterValueEditor
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
			this.CaptionLabel = new System.Windows.Forms.Label();
			this.EditorPanel = new System.Windows.Forms.Panel();
			this.NullEdit = new System.Windows.Forms.CheckBox();
			this.CaptionPanel = new System.Windows.Forms.Panel();
			this.NullPanel = new System.Windows.Forms.Panel();
			this.SelectPanel = new System.Windows.Forms.Panel();
			this.SelectBtn = new System.Windows.Forms.Button();
			this.CaptionPanel.SuspendLayout();
			this.NullPanel.SuspendLayout();
			this.SelectPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// CaptionLabel
			// 
			this.CaptionLabel.AutoEllipsis = true;
			this.CaptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CaptionLabel.Location = new System.Drawing.Point(0, 0);
			this.CaptionLabel.Name = "CaptionLabel";
			this.CaptionLabel.Size = new System.Drawing.Size(130, 22);
			this.CaptionLabel.TabIndex = 0;
			this.CaptionLabel.Text = "Parameter:";
			this.CaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// EditorPanel
			// 
			this.EditorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.EditorPanel.Location = new System.Drawing.Point(140, 2);
			this.EditorPanel.Name = "EditorPanel";
			this.EditorPanel.Size = new System.Drawing.Size(179, 22);
			this.EditorPanel.TabIndex = 1;
			// 
			// NullEdit
			// 
			this.NullEdit.AutoSize = true;
			this.NullEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.NullEdit.Location = new System.Drawing.Point(3, 0);
			this.NullEdit.Name = "NullEdit";
			this.NullEdit.Size = new System.Drawing.Size(54, 22);
			this.NullEdit.TabIndex = 2;
			this.NullEdit.Text = "NULL";
			this.NullEdit.UseVisualStyleBackColor = true;
			this.NullEdit.CheckedChanged += new System.EventHandler(this.OnNullValue);
			// 
			// CaptionPanel
			// 
			this.CaptionPanel.Controls.Add(this.CaptionLabel);
			this.CaptionPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.CaptionPanel.Location = new System.Drawing.Point(10, 2);
			this.CaptionPanel.Name = "CaptionPanel";
			this.CaptionPanel.Size = new System.Drawing.Size(130, 22);
			this.CaptionPanel.TabIndex = 3;
			// 
			// NullPanel
			// 
			this.NullPanel.Controls.Add(this.NullEdit);
			this.NullPanel.Dock = System.Windows.Forms.DockStyle.Right;
			this.NullPanel.Location = new System.Drawing.Point(343, 2);
			this.NullPanel.Name = "NullPanel";
			this.NullPanel.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.NullPanel.Size = new System.Drawing.Size(57, 22);
			this.NullPanel.TabIndex = 1;
			// 
			// SelectPanel
			// 
			this.SelectPanel.Controls.Add(this.SelectBtn);
			this.SelectPanel.Dock = System.Windows.Forms.DockStyle.Right;
			this.SelectPanel.Location = new System.Drawing.Point(319, 2);
			this.SelectPanel.Name = "SelectPanel";
			this.SelectPanel.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.SelectPanel.Size = new System.Drawing.Size(24, 22);
			this.SelectPanel.TabIndex = 4;
			// 
			// SelectBtn
			// 
			this.SelectBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.SelectBtn.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Select;
			this.SelectBtn.Location = new System.Drawing.Point(3, 0);
			this.SelectBtn.Name = "SelectBtn";
			this.SelectBtn.Size = new System.Drawing.Size(20, 20);
			this.SelectBtn.TabIndex = 0;
			this.SelectBtn.UseVisualStyleBackColor = true;
			this.SelectBtn.Click += new System.EventHandler(this.OnSelectValue);
			// 
			// ParameterValueEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.EditorPanel);
			this.Controls.Add(this.SelectPanel);
			this.Controls.Add(this.CaptionPanel);
			this.Controls.Add(this.NullPanel);
			this.Name = "ParameterValueEditor";
			this.Padding = new System.Windows.Forms.Padding(10, 2, 10, 2);
			this.Size = new System.Drawing.Size(410, 26);
			this.CaptionPanel.ResumeLayout(false);
			this.NullPanel.ResumeLayout(false);
			this.NullPanel.PerformLayout();
			this.SelectPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label CaptionLabel;
		private System.Windows.Forms.Panel EditorPanel;
		private System.Windows.Forms.CheckBox NullEdit;
		private System.Windows.Forms.Panel CaptionPanel;
		private System.Windows.Forms.Panel NullPanel;
		private System.Windows.Forms.Panel SelectPanel;
		private System.Windows.Forms.Button SelectBtn;
	}
}
