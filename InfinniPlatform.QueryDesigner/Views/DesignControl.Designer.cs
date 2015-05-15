namespace InfinniPlatform.QueryDesigner.Views
{
	partial class DesignControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DesignControl));
			this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
			this.ButtonExecuteQuery = new DevExpress.XtraEditors.SimpleButton();
			this.CreateDatabaseButton = new DevExpress.XtraEditors.SimpleButton();
			this.ButtonCreateQuery = new DevExpress.XtraEditors.SimpleButton();
			this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
			this.PanelSectionConstructor = new DevExpress.XtraEditors.PanelControl();
			this.SyntaxTree = new InfinniPlatform.QueryDesigner.Views.QueryConstructorSyntaxTree();
			this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
			this.JsonQueryEditor = new InfinniPlatform.QueryDesigner.Views.QueryJsonEditor();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.panelControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
			this.panelControl2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PanelSectionConstructor)).BeginInit();
			this.SuspendLayout();
			// 
			// panelControl1
			// 
			this.panelControl1.Controls.Add(this.ButtonExecuteQuery);
			this.panelControl1.Controls.Add(this.CreateDatabaseButton);
			this.panelControl1.Controls.Add(this.ButtonCreateQuery);
			this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelControl1.Location = new System.Drawing.Point(0, 678);
			this.panelControl1.LookAndFeel.SkinName = "Office 2013";
			this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(1111, 37);
			this.panelControl1.TabIndex = 1;
			// 
			// ButtonExecuteQuery
			// 
			this.ButtonExecuteQuery.Image = ((System.Drawing.Image)(resources.GetObject("ButtonExecuteQuery.Image")));
			this.ButtonExecuteQuery.Location = new System.Drawing.Point(231, 5);
			this.ButtonExecuteQuery.LookAndFeel.SkinName = "Office 2013";
			this.ButtonExecuteQuery.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ButtonExecuteQuery.Name = "ButtonExecuteQuery";
			this.ButtonExecuteQuery.Size = new System.Drawing.Size(107, 25);
			this.ButtonExecuteQuery.TabIndex = 2;
			this.ButtonExecuteQuery.Text = "Execute Query";
			this.ButtonExecuteQuery.Click += new System.EventHandler(this.ButtonExecuteQueryClick);
			// 
			// CreateDatabaseButton
			// 
			this.CreateDatabaseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.CreateDatabaseButton.Location = new System.Drawing.Point(950, 6);
			this.CreateDatabaseButton.LookAndFeel.SkinName = "Office 2013";
			this.CreateDatabaseButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.CreateDatabaseButton.Name = "CreateDatabaseButton";
			this.CreateDatabaseButton.Size = new System.Drawing.Size(156, 25);
			this.CreateDatabaseButton.TabIndex = 1;
			this.CreateDatabaseButton.Text = "Create test database";
			this.CreateDatabaseButton.Click += new System.EventHandler(this.CreateDatabaseButtonClick);
			// 
			// ButtonCreateQuery
			// 
			this.ButtonCreateQuery.Location = new System.Drawing.Point(5, 5);
			this.ButtonCreateQuery.LookAndFeel.SkinName = "Office 2013";
			this.ButtonCreateQuery.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ButtonCreateQuery.Name = "ButtonCreateQuery";
			this.ButtonCreateQuery.Size = new System.Drawing.Size(99, 25);
			this.ButtonCreateQuery.TabIndex = 0;
			this.ButtonCreateQuery.Text = "Create Query";
			this.ButtonCreateQuery.Click += new System.EventHandler(this.ButtonCreateQueryClick);
			// 
			// panelControl2
			// 
			this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
			this.panelControl2.Controls.Add(this.PanelSectionConstructor);
			this.panelControl2.Controls.Add(this.SyntaxTree);
			this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelControl2.Location = new System.Drawing.Point(0, 0);
			this.panelControl2.LookAndFeel.SkinName = "Office 2013";
			this.panelControl2.LookAndFeel.UseDefaultLookAndFeel = false;
			this.panelControl2.Name = "panelControl2";
			this.panelControl2.Size = new System.Drawing.Size(1111, 395);
			this.panelControl2.TabIndex = 3;
			// 
			// PanelSectionConstructor
			// 
			this.PanelSectionConstructor.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
			this.PanelSectionConstructor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PanelSectionConstructor.Location = new System.Drawing.Point(314, 0);
			this.PanelSectionConstructor.LookAndFeel.SkinName = "Office 2013";
			this.PanelSectionConstructor.LookAndFeel.UseDefaultLookAndFeel = false;
			this.PanelSectionConstructor.Name = "PanelSectionConstructor";
			this.PanelSectionConstructor.Size = new System.Drawing.Size(797, 395);
			this.PanelSectionConstructor.TabIndex = 1;
			// 
			// SyntaxTree
			// 
			this.SyntaxTree.Dock = System.Windows.Forms.DockStyle.Left;
			this.SyntaxTree.Location = new System.Drawing.Point(0, 0);
			this.SyntaxTree.Name = "SyntaxTree";
			this.SyntaxTree.OnAddFromControl = null;
			this.SyntaxTree.OnAddJoinControl = null;
			this.SyntaxTree.OnAddSelectControl = null;
			this.SyntaxTree.OnAddWhereControl = null;
			this.SyntaxTree.OnPressFromSection = null;
			this.SyntaxTree.OnPressJoinAction = null;
			this.SyntaxTree.OnPressSelectSection = null;
			this.SyntaxTree.OnPressWhereAction = null;
			this.SyntaxTree.OnRemoveJoinControl = null;
			this.SyntaxTree.OnRemoveWhereControl = null;
			this.SyntaxTree.Size = new System.Drawing.Size(314, 395);
			this.SyntaxTree.TabIndex = 0;
			// 
			// splitterControl1
			// 
			this.splitterControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitterControl1.Location = new System.Drawing.Point(0, 390);
			this.splitterControl1.Name = "splitterControl1";
			this.splitterControl1.Size = new System.Drawing.Size(1111, 5);
			this.splitterControl1.TabIndex = 0;
			this.splitterControl1.TabStop = false;
			// 
			// JsonQueryEditor
			// 
			this.JsonQueryEditor.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.JsonQueryEditor.Location = new System.Drawing.Point(0, 395);
			this.JsonQueryEditor.Name = "JsonQueryEditor";
			this.JsonQueryEditor.Size = new System.Drawing.Size(1111, 283);
			this.JsonQueryEditor.TabIndex = 2;
			// 
			// DesignControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitterControl1);
			this.Controls.Add(this.panelControl2);
			this.Controls.Add(this.JsonQueryEditor);
			this.Controls.Add(this.panelControl1);
			this.Name = "DesignControl";
			this.Size = new System.Drawing.Size(1111, 715);
			this.Load += new System.EventHandler(this.OnLoadDesigner);
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.panelControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
			this.panelControl2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.PanelSectionConstructor)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.PanelControl panelControl1;
		private DevExpress.XtraEditors.SimpleButton ButtonCreateQuery;
		private QueryJsonEditor JsonQueryEditor;
		private DevExpress.XtraEditors.PanelControl panelControl2;
		private DevExpress.XtraEditors.PanelControl PanelSectionConstructor;
		private QueryConstructorSyntaxTree SyntaxTree;
		private DevExpress.XtraEditors.SimpleButton CreateDatabaseButton;
		private DevExpress.XtraEditors.SplitterControl splitterControl1;
		private DevExpress.XtraEditors.SimpleButton ButtonExecuteQuery;
	}
}
