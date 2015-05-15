namespace InfinniPlatform.QueryDesigner.Views
{
	partial class QueryConstructorSyntaxTree
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
			this.NavBarSyntax = new DevExpress.XtraNavBar.NavBarControl();
			this.GroupFROM = new DevExpress.XtraNavBar.NavBarGroup();
			this.GroupJOIN = new DevExpress.XtraNavBar.NavBarGroup();
			this.GroupWHERE = new DevExpress.XtraNavBar.NavBarGroup();
			this.GroupSELECT = new DevExpress.XtraNavBar.NavBarGroup();
			this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
			this.JoinButton = new DevExpress.XtraEditors.SimpleButton();
			((System.ComponentModel.ISupportInitialize)(this.NavBarSyntax)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.panelControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// NavBarSyntax
			// 
			this.NavBarSyntax.ActiveGroup = this.GroupFROM;
			this.NavBarSyntax.Dock = System.Windows.Forms.DockStyle.Fill;
			this.NavBarSyntax.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.GroupFROM,
            this.GroupJOIN,
            this.GroupWHERE,
            this.GroupSELECT});
			this.NavBarSyntax.HideGroupCaptions = true;
			this.NavBarSyntax.Location = new System.Drawing.Point(0, 0);
			this.NavBarSyntax.LookAndFeel.SkinName = "Office 2013";
			this.NavBarSyntax.LookAndFeel.UseDefaultLookAndFeel = false;
			this.NavBarSyntax.Name = "NavBarSyntax";
			this.NavBarSyntax.OptionsNavPane.ExpandedWidth = 314;
			this.NavBarSyntax.OptionsNavPane.ShowExpandButton = false;
			this.NavBarSyntax.ShowGroupHint = false;
			this.NavBarSyntax.Size = new System.Drawing.Size(314, 345);
			this.NavBarSyntax.TabIndex = 0;
			this.NavBarSyntax.Text = "NavBarSyntax";
			this.NavBarSyntax.CustomDrawLink += new DevExpress.XtraNavBar.ViewInfo.CustomDrawNavBarElementEventHandler(this.NavBarSyntax_CustomDrawLink);
			this.NavBarSyntax.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.NavBarSyntaxLinkClicked);
			this.NavBarSyntax.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NavBarSyntax_MouseDown);
			// 
			// GroupFROM
			// 
			this.GroupFROM.Caption = "Section FROM";
			this.GroupFROM.Expanded = true;
			this.GroupFROM.Name = "GroupFROM";
			// 
			// GroupJOIN
			// 
			this.GroupJOIN.Caption = "Section JOIN";
			this.GroupJOIN.Name = "GroupJOIN";
			// 
			// GroupWHERE
			// 
			this.GroupWHERE.Caption = "Section WHERE";
			this.GroupWHERE.Name = "GroupWHERE";
			// 
			// GroupSELECT
			// 
			this.GroupSELECT.Caption = "Section SELECT";
			this.GroupSELECT.Name = "GroupSELECT";
			// 
			// panelControl1
			// 
			this.panelControl1.Controls.Add(this.JoinButton);
			this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelControl1.Location = new System.Drawing.Point(0, 345);
			this.panelControl1.LookAndFeel.SkinName = "Office 2013";
			this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(314, 35);
			this.panelControl1.TabIndex = 1;
			// 
			// JoinButton
			// 
			this.JoinButton.Location = new System.Drawing.Point(5, 6);
			this.JoinButton.LookAndFeel.SkinName = "Office 2013";
			this.JoinButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.JoinButton.Name = "JoinButton";
			this.JoinButton.Size = new System.Drawing.Size(75, 24);
			this.JoinButton.TabIndex = 0;
			this.JoinButton.Text = "Add JOIN";
			this.JoinButton.Click += new System.EventHandler(this.ButtonAddJoinClick);
			// 
			// QueryConstructorSyntaxTree
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.NavBarSyntax);
			this.Controls.Add(this.panelControl1);
			this.Name = "QueryConstructorSyntaxTree";
			this.Size = new System.Drawing.Size(314, 380);
			((System.ComponentModel.ISupportInitialize)(this.NavBarSyntax)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.panelControl1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraNavBar.NavBarControl NavBarSyntax;
		private DevExpress.XtraNavBar.NavBarGroup GroupFROM;
		private DevExpress.XtraNavBar.NavBarGroup GroupJOIN;
		private DevExpress.XtraNavBar.NavBarGroup GroupWHERE;
		private DevExpress.XtraNavBar.NavBarGroup GroupSELECT;
		private DevExpress.XtraEditors.PanelControl panelControl1;
		private DevExpress.XtraEditors.SimpleButton JoinButton;
		


	}
}
