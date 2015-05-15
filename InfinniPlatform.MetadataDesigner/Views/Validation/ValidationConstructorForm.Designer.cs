using System;

namespace InfinniPlatform.MetadataDesigner.Views.Validation
{
	partial class ValidationConstructorForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ValidationConstructorForm));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.CancelButton = new DevExpress.XtraEditors.SimpleButton();
            this.ButtonOK = new DevExpress.XtraEditors.SimpleButton();
            this.AddChildButton = new DevExpress.XtraEditors.SimpleButton();
            this.panelBuilderPlaceholder = new DevExpress.XtraEditors.PanelControl();
            this.PreviewMemoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.ValidationTreeView = new System.Windows.Forms.TreeView();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.addMenuItem = new System.Windows.Forms.MenuItem();
            this.menu = new System.Windows.Forms.ContextMenu();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelBuilderPlaceholder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewMemoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.White;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.CancelButton);
            this.panelControl1.Controls.Add(this.ButtonOK);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 469);
            this.panelControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(931, 40);
            this.panelControl1.TabIndex = 1;
            // 
            // CancelButton
            // 
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Image = ((System.Drawing.Image)(resources.GetObject("CancelButton.Image")));
            this.CancelButton.Location = new System.Drawing.Point(847, 12);
            this.CancelButton.LookAndFeel.SkinName = "Office 2013";
            this.CancelButton.LookAndFeel.UseDefaultLookAndFeel = false;
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(79, 23);
            this.CancelButton.TabIndex = 1;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ButtonOK
            // 
            this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonOK.Enabled = false;
            this.ButtonOK.Image = ((System.Drawing.Image)(resources.GetObject("ButtonOK.Image")));
            this.ButtonOK.Location = new System.Drawing.Point(762, 12);
            this.ButtonOK.LookAndFeel.SkinName = "Office 2013";
            this.ButtonOK.LookAndFeel.UseDefaultLookAndFeel = false;
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(79, 23);
            this.ButtonOK.TabIndex = 0;
            this.ButtonOK.Text = "Complete";
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // AddChildButton
            // 
            this.AddChildButton.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.AddChildButton.Location = new System.Drawing.Point(485, 12);
            this.AddChildButton.LookAndFeel.SkinName = "Office 2013";
            this.AddChildButton.LookAndFeel.UseDefaultLookAndFeel = false;
            this.AddChildButton.Name = "AddChildButton";
            this.AddChildButton.Size = new System.Drawing.Size(132, 23);
            this.AddChildButton.TabIndex = 2;
            this.AddChildButton.Text = "Add";
            this.AddChildButton.Click += new System.EventHandler(this.AddChildButton_Click);
            // 
            // panelBuilderPlaceholder
            // 
            this.panelBuilderPlaceholder.Appearance.BackColor = System.Drawing.Color.White;
            this.panelBuilderPlaceholder.Appearance.Options.UseBackColor = true;
            this.panelBuilderPlaceholder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBuilderPlaceholder.Location = new System.Drawing.Point(0, 0);
            this.panelBuilderPlaceholder.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.panelBuilderPlaceholder.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelBuilderPlaceholder.Name = "panelBuilderPlaceholder";
            this.panelBuilderPlaceholder.Size = new System.Drawing.Size(405, 469);
            this.panelBuilderPlaceholder.TabIndex = 2;
            // 
            // PreviewMemoEdit
            // 
            this.PreviewMemoEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this.PreviewMemoEdit.EditValue = "";
            this.PreviewMemoEdit.Location = new System.Drawing.Point(617, 0);
            this.PreviewMemoEdit.Name = "PreviewMemoEdit";
            this.PreviewMemoEdit.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.PreviewMemoEdit.Properties.ReadOnly = true;
            this.PreviewMemoEdit.Properties.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.PreviewMemoEdit.Size = new System.Drawing.Size(314, 469);
            this.PreviewMemoEdit.TabIndex = 3;
            this.PreviewMemoEdit.UseOptimizedRendering = true;
            // 
            // ValidationTreeView
            // 
            this.ValidationTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ValidationTreeView.Location = new System.Drawing.Point(0, 0);
            this.ValidationTreeView.Name = "ValidationTreeView";
            this.ValidationTreeView.Size = new System.Drawing.Size(207, 469);
            this.ValidationTreeView.TabIndex = 4;
            this.ValidationTreeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ValidationTreeView_MouseUp);
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.ValidationTreeView);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.panelBuilderPlaceholder);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(617, 469);
            this.splitContainerControl1.SplitterPosition = 207;
            this.splitContainerControl1.TabIndex = 5;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // addMenuItem
            // 
            this.addMenuItem.Index = 0;
            this.addMenuItem.Text = "Add";
            this.addMenuItem.Click += new System.EventHandler(this.AddNewNodeContextMenuClick);
            // 
            // menu
            // 
            this.menu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.addMenuItem});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(5, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(199, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Use context menu to add validation rules";
            // 
            // ValidationConstructorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(931, 509);
            this.Controls.Add(this.splitContainerControl1);
            this.Controls.Add(this.PreviewMemoEdit);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ValidationConstructorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Validation builder";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelBuilderPlaceholder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewMemoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
		private DevExpress.XtraEditors.SimpleButton ButtonOK;
		private DevExpress.XtraEditors.SimpleButton CancelButton;
        private DevExpress.XtraEditors.SimpleButton AddChildButton;
        private DevExpress.XtraEditors.PanelControl panelBuilderPlaceholder;
        private DevExpress.XtraEditors.MemoEdit PreviewMemoEdit;
        private System.Windows.Forms.TreeView ValidationTreeView;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private System.Windows.Forms.MenuItem addMenuItem;
        private System.Windows.Forms.ContextMenu menu;
        private System.Windows.Forms.Label label1;
	}
}