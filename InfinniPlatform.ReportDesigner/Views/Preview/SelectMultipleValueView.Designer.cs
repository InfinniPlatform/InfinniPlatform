namespace InfinniPlatform.ReportDesigner.Views.Preview
{
	partial class SelectMultipleValueView
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
			this.EditorPanel = new System.Windows.Forms.Panel();
			this.ItemsPanel = new System.Windows.Forms.Panel();
			this.ItemsEdit = new System.Windows.Forms.CheckedListBox();
			this.MainMenu = new System.Windows.Forms.ToolStrip();
			this.AddBtn = new System.Windows.Forms.ToolStripButton();
			this.DeleteBtn = new System.Windows.Forms.ToolStripButton();
			this.ControlPanel = new System.Windows.Forms.Panel();
			this.ItemsPanel.SuspendLayout();
			this.MainMenu.SuspendLayout();
			this.ControlPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// EditorPanel
			// 
			this.EditorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.EditorPanel.Location = new System.Drawing.Point(0, 25);
			this.EditorPanel.Name = "EditorPanel";
			this.EditorPanel.Padding = new System.Windows.Forms.Padding(10, 5, 10, 0);
			this.EditorPanel.Size = new System.Drawing.Size(364, 27);
			this.EditorPanel.TabIndex = 0;
			// 
			// ItemsPanel
			// 
			this.ItemsPanel.Controls.Add(this.ItemsEdit);
			this.ItemsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ItemsPanel.Location = new System.Drawing.Point(0, 52);
			this.ItemsPanel.Name = "ItemsPanel";
			this.ItemsPanel.Padding = new System.Windows.Forms.Padding(10);
			this.ItemsPanel.Size = new System.Drawing.Size(364, 249);
			this.ItemsPanel.TabIndex = 1;
			// 
			// ItemsEdit
			// 
			this.ItemsEdit.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ItemsEdit.CheckOnClick = true;
			this.ItemsEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ItemsEdit.FormattingEnabled = true;
			this.ItemsEdit.Location = new System.Drawing.Point(10, 10);
			this.ItemsEdit.Name = "ItemsEdit";
			this.ItemsEdit.Size = new System.Drawing.Size(344, 229);
			this.ItemsEdit.TabIndex = 0;
			// 
			// MainMenu
			// 
			this.MainMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddBtn,
            this.DeleteBtn});
			this.MainMenu.Location = new System.Drawing.Point(0, 0);
			this.MainMenu.Name = "MainMenu";
			this.MainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.MainMenu.Size = new System.Drawing.Size(364, 25);
			this.MainMenu.TabIndex = 1;
			this.MainMenu.Text = "toolStrip1";
			// 
			// AddBtn
			// 
			this.AddBtn.AutoSize = false;
			this.AddBtn.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.New;
			this.AddBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.AddBtn.Name = "AddBtn";
			this.AddBtn.Size = new System.Drawing.Size(60, 22);
			this.AddBtn.Text = "Add";
			this.AddBtn.Click += new System.EventHandler(this.OnAdd);
			// 
			// DeleteBtn
			// 
			this.DeleteBtn.AutoSize = false;
			this.DeleteBtn.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Delete;
			this.DeleteBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.DeleteBtn.Name = "DeleteBtn";
			this.DeleteBtn.Size = new System.Drawing.Size(60, 22);
			this.DeleteBtn.Text = "Delete";
			this.DeleteBtn.Click += new System.EventHandler(this.OnDelete);
			// 
			// ControlPanel
			// 
			this.ControlPanel.Controls.Add(this.EditorPanel);
			this.ControlPanel.Controls.Add(this.MainMenu);
			this.ControlPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.ControlPanel.Location = new System.Drawing.Point(0, 0);
			this.ControlPanel.Name = "ControlPanel";
			this.ControlPanel.Size = new System.Drawing.Size(364, 52);
			this.ControlPanel.TabIndex = 0;
			// 
			// SelectMultipleValueView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ItemsPanel);
			this.Controls.Add(this.ControlPanel);
			this.Name = "SelectMultipleValueView";
			this.Size = new System.Drawing.Size(364, 301);
			this.ItemsPanel.ResumeLayout(false);
			this.MainMenu.ResumeLayout(false);
			this.MainMenu.PerformLayout();
			this.ControlPanel.ResumeLayout(false);
			this.ControlPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel EditorPanel;
		private System.Windows.Forms.Panel ItemsPanel;
		private System.Windows.Forms.CheckedListBox ItemsEdit;
		private System.Windows.Forms.ToolStrip MainMenu;
		private System.Windows.Forms.ToolStripButton AddBtn;
		private System.Windows.Forms.ToolStripButton DeleteBtn;
		private System.Windows.Forms.Panel ControlPanel;
	}
}
