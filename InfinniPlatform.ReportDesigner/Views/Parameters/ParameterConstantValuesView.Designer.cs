namespace InfinniPlatform.ReportDesigner.Views.Parameters
{
	partial class ParameterConstantValuesView
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
			this.MainPanel = new System.Windows.Forms.Panel();
			this.ValueListView = new System.Windows.Forms.DataGridView();
			this.LabelPropertyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ValuePropertyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.WarningPanel = new System.Windows.Forms.Panel();
			this.WarningMessage = new System.Windows.Forms.Label();
			this.WarningImage = new System.Windows.Forms.PictureBox();
			this.MainMenu = new System.Windows.Forms.ToolStrip();
			this.AddBtn = new System.Windows.Forms.ToolStripButton();
			this.DeleteBtn = new System.Windows.Forms.ToolStripButton();
			this.Separator1 = new System.Windows.Forms.ToolStripSeparator();
			this.MoveUpBtn = new System.Windows.Forms.ToolStripButton();
			this.MoveDownBtn = new System.Windows.Forms.ToolStripButton();
			this.MainPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ValueListView)).BeginInit();
			this.WarningPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.WarningImage)).BeginInit();
			this.MainMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainPanel
			// 
			this.MainPanel.Controls.Add(this.ValueListView);
			this.MainPanel.Controls.Add(this.WarningPanel);
			this.MainPanel.Controls.Add(this.MainMenu);
			this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainPanel.Location = new System.Drawing.Point(0, 0);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(450, 300);
			this.MainPanel.TabIndex = 1;
			// 
			// ValueListView
			// 
			this.ValueListView.AllowUserToAddRows = false;
			this.ValueListView.AllowUserToDeleteRows = false;
			this.ValueListView.AllowUserToResizeColumns = false;
			this.ValueListView.AllowUserToResizeRows = false;
			this.ValueListView.BackgroundColor = System.Drawing.SystemColors.Control;
			this.ValueListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ValueListView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ValueListView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LabelPropertyColumn,
            this.ValuePropertyColumn});
			this.ValueListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ValueListView.Location = new System.Drawing.Point(0, 55);
			this.ValueListView.MultiSelect = false;
			this.ValueListView.Name = "ValueListView";
			this.ValueListView.RowHeadersVisible = false;
			this.ValueListView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.ValueListView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.ValueListView.Size = new System.Drawing.Size(450, 245);
			this.ValueListView.TabIndex = 1;
			this.ValueListView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.OnCellValidating);
			// 
			// LabelPropertyColumn
			// 
			this.LabelPropertyColumn.HeaderText = "Label";
			this.LabelPropertyColumn.Name = "LabelPropertyColumn";
			this.LabelPropertyColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.LabelPropertyColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.LabelPropertyColumn.Width = 200;
			// 
			// ValuePropertyColumn
			// 
			this.ValuePropertyColumn.HeaderText = "Value";
			this.ValuePropertyColumn.Name = "ValuePropertyColumn";
			this.ValuePropertyColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.ValuePropertyColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.ValuePropertyColumn.Width = 200;
			// 
			// WarningPanel
			// 
			this.WarningPanel.Controls.Add(this.WarningMessage);
			this.WarningPanel.Controls.Add(this.WarningImage);
			this.WarningPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.WarningPanel.Location = new System.Drawing.Point(0, 25);
			this.WarningPanel.Name = "WarningPanel";
			this.WarningPanel.Size = new System.Drawing.Size(450, 30);
			this.WarningPanel.TabIndex = 2;
			this.WarningPanel.Visible = false;
			// 
			// WarningMessage
			// 
			this.WarningMessage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.WarningMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.WarningMessage.Location = new System.Drawing.Point(30, 0);
			this.WarningMessage.Name = "WarningMessage";
			this.WarningMessage.Size = new System.Drawing.Size(420, 30);
			this.WarningMessage.TabIndex = 5;
			this.WarningMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// WarningImage
			// 
			this.WarningImage.Dock = System.Windows.Forms.DockStyle.Left;
			this.WarningImage.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.MessageError;
			this.WarningImage.Location = new System.Drawing.Point(0, 0);
			this.WarningImage.Name = "WarningImage";
			this.WarningImage.Size = new System.Drawing.Size(30, 30);
			this.WarningImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.WarningImage.TabIndex = 3;
			this.WarningImage.TabStop = false;
			// 
			// MainMenu
			// 
			this.MainMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddBtn,
            this.DeleteBtn,
            this.Separator1,
            this.MoveUpBtn,
            this.MoveDownBtn});
			this.MainMenu.Location = new System.Drawing.Point(0, 0);
			this.MainMenu.Name = "MainMenu";
			this.MainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.MainMenu.Size = new System.Drawing.Size(450, 25);
			this.MainMenu.TabIndex = 3;
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
			this.AddBtn.Click += new System.EventHandler(this.OnAddClick);
			// 
			// DeleteBtn
			// 
			this.DeleteBtn.AutoSize = false;
			this.DeleteBtn.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.Delete;
			this.DeleteBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.DeleteBtn.Name = "DeleteBtn";
			this.DeleteBtn.Size = new System.Drawing.Size(60, 22);
			this.DeleteBtn.Text = "Delete";
			this.DeleteBtn.Click += new System.EventHandler(this.OnDeleteClick);
			// 
			// Separator1
			// 
			this.Separator1.Name = "Separator1";
			this.Separator1.Size = new System.Drawing.Size(6, 25);
			// 
			// MoveUpBtn
			// 
			this.MoveUpBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.MoveUpBtn.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ArrowUp;
			this.MoveUpBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.MoveUpBtn.Name = "MoveUpBtn";
			this.MoveUpBtn.Size = new System.Drawing.Size(23, 22);
			this.MoveUpBtn.Text = "Move Up";
			this.MoveUpBtn.Click += new System.EventHandler(this.OnUpClick);
			// 
			// MoveDownBtn
			// 
			this.MoveDownBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.MoveDownBtn.Image = global::InfinniPlatform.ReportDesigner.Properties.Resources.ArrowDown;
			this.MoveDownBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.MoveDownBtn.Name = "MoveDownBtn";
			this.MoveDownBtn.Size = new System.Drawing.Size(23, 22);
			this.MoveDownBtn.Text = "Move Down";
			this.MoveDownBtn.Click += new System.EventHandler(this.OnDownClick);
			// 
			// ParameterConstantValuesView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.MainPanel);
			this.Name = "ParameterConstantValuesView";
			this.Size = new System.Drawing.Size(450, 300);
			this.MainPanel.ResumeLayout(false);
			this.MainPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ValueListView)).EndInit();
			this.WarningPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.WarningImage)).EndInit();
			this.MainMenu.ResumeLayout(false);
			this.MainMenu.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel MainPanel;
		private System.Windows.Forms.DataGridView ValueListView;
		private System.Windows.Forms.DataGridViewTextBoxColumn LabelPropertyColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn ValuePropertyColumn;
		private System.Windows.Forms.Panel WarningPanel;
		private System.Windows.Forms.Label WarningMessage;
		private System.Windows.Forms.PictureBox WarningImage;
		private System.Windows.Forms.ToolStrip MainMenu;
		private System.Windows.Forms.ToolStripButton AddBtn;
		private System.Windows.Forms.ToolStripButton DeleteBtn;
		private System.Windows.Forms.ToolStripSeparator Separator1;
		private System.Windows.Forms.ToolStripButton MoveUpBtn;
		private System.Windows.Forms.ToolStripButton MoveDownBtn;
	}
}
