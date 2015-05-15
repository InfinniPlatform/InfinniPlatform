namespace InfinniPlatform.QueryDesigner.Views
{
	partial class QueryExecutor
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
			this.PanelQueryText = new DevExpress.XtraEditors.PanelControl();
			this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
			this.MemoQuery = new DevExpress.XtraEditors.MemoEdit();
			this.panelBar = new DevExpress.XtraEditors.PanelControl();
			this.DenormalizeCheckEdit = new DevExpress.XtraEditors.CheckEdit();
			this.ButtonExecute = new DevExpress.XtraEditors.SimpleButton();
			this.PanelResults = new DevExpress.XtraEditors.PanelControl();
			this.TabControlResult = new DevExpress.XtraTab.XtraTabControl();
			this.TabPageResultJson = new DevExpress.XtraTab.XtraTabPage();
			this.MemoEditResultJSON = new DevExpress.XtraEditors.MemoEdit();
			this.TabPageResultGrid = new DevExpress.XtraTab.XtraTabPage();
			this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
			this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
			this.ElapsedMillisecondsText = new DevExpress.XtraEditors.TextEdit();
			this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
			((System.ComponentModel.ISupportInitialize)(this.PanelQueryText)).BeginInit();
			this.PanelQueryText.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.panelControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.MemoQuery.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.panelBar)).BeginInit();
			this.panelBar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.DenormalizeCheckEdit.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PanelResults)).BeginInit();
			this.PanelResults.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.TabControlResult)).BeginInit();
			this.TabControlResult.SuspendLayout();
			this.TabPageResultJson.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.MemoEditResultJSON.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
			this.panelControl2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ElapsedMillisecondsText.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// PanelQueryText
			// 
			this.PanelQueryText.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
			this.PanelQueryText.Controls.Add(this.panelControl1);
			this.PanelQueryText.Controls.Add(this.panelBar);
			this.PanelQueryText.Dock = System.Windows.Forms.DockStyle.Top;
			this.PanelQueryText.Location = new System.Drawing.Point(0, 0);
			this.PanelQueryText.LookAndFeel.SkinName = "Office 2013";
			this.PanelQueryText.LookAndFeel.UseDefaultLookAndFeel = false;
			this.PanelQueryText.Name = "PanelQueryText";
			this.PanelQueryText.Size = new System.Drawing.Size(959, 220);
			this.PanelQueryText.TabIndex = 0;
			// 
			// panelControl1
			// 
			this.panelControl1.Controls.Add(this.MemoQuery);
			this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelControl1.Location = new System.Drawing.Point(0, 35);
			this.panelControl1.LookAndFeel.SkinName = "Office 2013";
			this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(959, 185);
			this.panelControl1.TabIndex = 1;
			// 
			// MemoQuery
			// 
			this.MemoQuery.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MemoQuery.Location = new System.Drawing.Point(2, 2);
			this.MemoQuery.Name = "MemoQuery";
			this.MemoQuery.Properties.Appearance.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.MemoQuery.Properties.Appearance.Options.UseFont = true;
			this.MemoQuery.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.MemoQuery.Size = new System.Drawing.Size(955, 181);
			this.MemoQuery.TabIndex = 0;
			this.MemoQuery.UseOptimizedRendering = true;
			// 
			// panelBar
			// 
			this.panelBar.Controls.Add(this.DenormalizeCheckEdit);
			this.panelBar.Controls.Add(this.ButtonExecute);
			this.panelBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelBar.Location = new System.Drawing.Point(0, 0);
			this.panelBar.LookAndFeel.SkinName = "Office 2013";
			this.panelBar.LookAndFeel.UseDefaultLookAndFeel = false;
			this.panelBar.Name = "panelBar";
			this.panelBar.Size = new System.Drawing.Size(959, 35);
			this.panelBar.TabIndex = 0;
			// 
			// DenormalizeCheckEdit
			// 
			this.DenormalizeCheckEdit.EditValue = true;
			this.DenormalizeCheckEdit.Location = new System.Drawing.Point(152, 5);
			this.DenormalizeCheckEdit.Name = "DenormalizeCheckEdit";
			this.DenormalizeCheckEdit.Properties.Caption = "Denormalize query result";
			this.DenormalizeCheckEdit.Size = new System.Drawing.Size(207, 19);
			this.DenormalizeCheckEdit.TabIndex = 2;
			// 
			// ButtonExecute
			// 
			this.ButtonExecute.Image = global::InfinniPlatform.QueryDesigner.Properties.Resources.checkbox_16x16;
			this.ButtonExecute.Location = new System.Drawing.Point(5, 5);
			this.ButtonExecute.LookAndFeel.SkinName = "Office 2013";
			this.ButtonExecute.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ButtonExecute.Name = "ButtonExecute";
			this.ButtonExecute.Size = new System.Drawing.Size(132, 23);
			this.ButtonExecute.TabIndex = 0;
			this.ButtonExecute.Text = "ExecuteQuery";
			this.ButtonExecute.Click += new System.EventHandler(this.ButtonExecuteClick);
			// 
			// PanelResults
			// 
			this.PanelResults.Controls.Add(this.TabControlResult);
			this.PanelResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PanelResults.Location = new System.Drawing.Point(0, 220);
			this.PanelResults.LookAndFeel.SkinName = "Office 2013";
			this.PanelResults.LookAndFeel.UseDefaultLookAndFeel = false;
			this.PanelResults.Name = "PanelResults";
			this.PanelResults.Size = new System.Drawing.Size(959, 329);
			this.PanelResults.TabIndex = 1;
			// 
			// TabControlResult
			// 
			this.TabControlResult.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TabControlResult.Location = new System.Drawing.Point(2, 2);
			this.TabControlResult.Name = "TabControlResult";
			this.TabControlResult.SelectedTabPage = this.TabPageResultJson;
			this.TabControlResult.Size = new System.Drawing.Size(955, 325);
			this.TabControlResult.TabIndex = 0;
			this.TabControlResult.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.TabPageResultJson,
            this.TabPageResultGrid});
			// 
			// TabPageResultJson
			// 
			this.TabPageResultJson.Controls.Add(this.MemoEditResultJSON);
			this.TabPageResultJson.Name = "TabPageResultJson";
			this.TabPageResultJson.Size = new System.Drawing.Size(949, 297);
			this.TabPageResultJson.Text = "JSON";
			// 
			// MemoEditResultJSON
			// 
			this.MemoEditResultJSON.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MemoEditResultJSON.Location = new System.Drawing.Point(0, 0);
			this.MemoEditResultJSON.Name = "MemoEditResultJSON";
			this.MemoEditResultJSON.Properties.Appearance.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.MemoEditResultJSON.Properties.Appearance.Options.UseFont = true;
			this.MemoEditResultJSON.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
			this.MemoEditResultJSON.Size = new System.Drawing.Size(949, 297);
			this.MemoEditResultJSON.TabIndex = 1;
			this.MemoEditResultJSON.UseOptimizedRendering = true;
			// 
			// TabPageResultGrid
			// 
			this.TabPageResultGrid.Name = "TabPageResultGrid";
			this.TabPageResultGrid.Size = new System.Drawing.Size(949, 297);
			this.TabPageResultGrid.Text = "Grid";
			// 
			// panelControl2
			// 
			this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.panelControl2.Controls.Add(this.labelControl1);
			this.panelControl2.Controls.Add(this.ElapsedMillisecondsText);
			this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelControl2.Location = new System.Drawing.Point(0, 549);
			this.panelControl2.LookAndFeel.SkinName = "Office 2013";
			this.panelControl2.LookAndFeel.UseDefaultLookAndFeel = false;
			this.panelControl2.Name = "panelControl2";
			this.panelControl2.Size = new System.Drawing.Size(959, 34);
			this.panelControl2.TabIndex = 2;
			// 
			// labelControl1
			// 
			this.labelControl1.Location = new System.Drawing.Point(91, 9);
			this.labelControl1.Name = "labelControl1";
			this.labelControl1.Size = new System.Drawing.Size(53, 13);
			this.labelControl1.TabIndex = 1;
			this.labelControl1.Text = "Elapsed ms";
			// 
			// ElapsedMillisecondsText
			// 
			this.ElapsedMillisecondsText.Location = new System.Drawing.Point(5, 6);
			this.ElapsedMillisecondsText.Name = "ElapsedMillisecondsText";
			this.ElapsedMillisecondsText.Properties.ReadOnly = true;
			this.ElapsedMillisecondsText.Size = new System.Drawing.Size(80, 20);
			this.ElapsedMillisecondsText.TabIndex = 0;
			// 
			// splitterControl1
			// 
			this.splitterControl1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitterControl1.Location = new System.Drawing.Point(0, 220);
			this.splitterControl1.Name = "splitterControl1";
			this.splitterControl1.Size = new System.Drawing.Size(959, 5);
			this.splitterControl1.TabIndex = 2;
			this.splitterControl1.TabStop = false;
			// 
			// QueryExecutor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitterControl1);
			this.Controls.Add(this.PanelResults);
			this.Controls.Add(this.PanelQueryText);
			this.Controls.Add(this.panelControl2);
			this.Name = "QueryExecutor";
			this.Size = new System.Drawing.Size(959, 583);
			((System.ComponentModel.ISupportInitialize)(this.PanelQueryText)).EndInit();
			this.PanelQueryText.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.panelControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.MemoQuery.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.panelBar)).EndInit();
			this.panelBar.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.DenormalizeCheckEdit.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PanelResults)).EndInit();
			this.PanelResults.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.TabControlResult)).EndInit();
			this.TabControlResult.ResumeLayout(false);
			this.TabPageResultJson.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.MemoEditResultJSON.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
			this.panelControl2.ResumeLayout(false);
			this.panelControl2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ElapsedMillisecondsText.Properties)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.PanelControl PanelQueryText;
		private DevExpress.XtraEditors.PanelControl PanelResults;
		private DevExpress.XtraEditors.PanelControl panelControl1;
		private DevExpress.XtraEditors.PanelControl panelBar;
		private DevExpress.XtraEditors.SimpleButton ButtonExecute;
		private DevExpress.XtraEditors.MemoEdit MemoQuery;
		private DevExpress.XtraTab.XtraTabControl TabControlResult;
		private DevExpress.XtraTab.XtraTabPage TabPageResultJson;
		private DevExpress.XtraTab.XtraTabPage TabPageResultGrid;
		private DevExpress.XtraEditors.MemoEdit MemoEditResultJSON;
		private DevExpress.XtraEditors.PanelControl panelControl2;
		private DevExpress.XtraEditors.TextEdit ElapsedMillisecondsText;
		private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SplitterControl splitterControl1;
        private DevExpress.XtraEditors.CheckEdit DenormalizeCheckEdit;
	}
}
