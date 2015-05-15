using InfinniPlatform.DesignControls.ObjectInspector;

namespace InfinniPlatform.DesignControls
{
    partial class DesignerControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DesignerControl));
			this.ObjectInspectorPanel = new DevExpress.XtraEditors.PanelControl();
			this.ObjectInspector = new InfinniPlatform.DesignControls.ObjectInspector.ObjectInspectorTree();
			this.LabelRendered = new DevExpress.XtraEditors.LabelControl();
			this.ClearLayoutButton = new DevExpress.XtraEditors.SimpleButton();
			this.SetSizeButton = new DevExpress.XtraEditors.SimpleButton();
			this.SetLayoutButton = new DevExpress.XtraEditors.SimpleButton();
			this.ButtonGetLayout = new DevExpress.XtraEditors.SimpleButton();
			this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
			this.TextEditHeight = new DevExpress.XtraEditors.TextEdit();
			this.TextEditWidth = new DevExpress.XtraEditors.TextEdit();
			this.ScrollableControl = new DevExpress.XtraEditors.XtraScrollableControl();
			this.PanelContainer = new InfinniPlatform.DesignControls.Controls.LayoutPanels.Panel();
			this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
			this.TabPageViews = new DevExpress.XtraTab.XtraTabPage();
			this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
			this.PanelTools = new DevExpress.XtraEditors.PanelControl();
			this.GenerateViewButton = new DevExpress.XtraEditors.SimpleButton();
			this.TabPageDataSources = new DevExpress.XtraTab.XtraTabPage();
			this.DataSourceSurface = new InfinniPlatform.DesignControls.DesignerSurface.DataSourceSurface();
			this.TabPageScripts = new DevExpress.XtraTab.XtraTabPage();
			this.ScriptSurface = new InfinniPlatform.DesignControls.DesignerSurface.ScriptSurface();
			this.TabPageParameters = new DevExpress.XtraTab.XtraTabPage();
			this.ParametersSurface = new InfinniPlatform.DesignControls.DesignerSurface.ParameterSurface();
			this.TabPageChildViews = new DevExpress.XtraTab.XtraTabPage();
			this.ChildViewSurface = new InfinniPlatform.DesignControls.DesignerSurface.ChildViewSurface();
			((System.ComponentModel.ISupportInitialize)(this.ObjectInspectorPanel)).BeginInit();
			this.ObjectInspectorPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.TextEditHeight.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TextEditWidth.Properties)).BeginInit();
			this.ScrollableControl.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
			this.xtraTabControl1.SuspendLayout();
			this.TabPageViews.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PanelTools)).BeginInit();
			this.PanelTools.SuspendLayout();
			this.TabPageDataSources.SuspendLayout();
			this.TabPageScripts.SuspendLayout();
			this.TabPageParameters.SuspendLayout();
			this.TabPageChildViews.SuspendLayout();
			this.SuspendLayout();
			// 
			// ObjectInspectorPanel
			// 
			this.ObjectInspectorPanel.Controls.Add(this.ObjectInspector);
			this.ObjectInspectorPanel.Dock = System.Windows.Forms.DockStyle.Right;
			this.ObjectInspectorPanel.Location = new System.Drawing.Point(1076, 34);
			this.ObjectInspectorPanel.Name = "ObjectInspectorPanel";
			this.ObjectInspectorPanel.Size = new System.Drawing.Size(239, 812);
			this.ObjectInspectorPanel.TabIndex = 1;
			// 
			// ObjectInspector
			// 
			this.ObjectInspector.ChildViews = null;
			this.ObjectInspector.ControlRepository = null;
			this.ObjectInspector.DataSources = null;
			this.ObjectInspector.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ObjectInspector.FocusedPropertiesNode = null;
			this.ObjectInspector.Location = new System.Drawing.Point(2, 2);
			this.ObjectInspector.Name = "ObjectInspector";
			this.ObjectInspector.ObjectInspectorPopupMenu = null;
			this.ObjectInspector.OnSetFocus = null;
			this.ObjectInspector.PropertiesRootNode = null;
			this.ObjectInspector.Scripts = null;
			this.ObjectInspector.Size = new System.Drawing.Size(235, 808);
			this.ObjectInspector.TabIndex = 0;
			// 
			// LabelRendered
			// 
			this.LabelRendered.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.LabelRendered.Location = new System.Drawing.Point(1014, 12);
			this.LabelRendered.Name = "LabelRendered";
			this.LabelRendered.Size = new System.Drawing.Size(62, 13);
			this.LabelRendered.TabIndex = 0;
			this.LabelRendered.Text = "Rendered in:";
			// 
			// ClearLayoutButton
			// 
			this.ClearLayoutButton.Image = ((System.Drawing.Image)(resources.GetObject("ClearLayoutButton.Image")));
			this.ClearLayoutButton.Location = new System.Drawing.Point(471, 6);
			this.ClearLayoutButton.LookAndFeel.SkinName = "Office 2013";
			this.ClearLayoutButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ClearLayoutButton.Name = "ClearLayoutButton";
			this.ClearLayoutButton.Size = new System.Drawing.Size(74, 23);
			this.ClearLayoutButton.TabIndex = 7;
			this.ClearLayoutButton.Text = "Clear";
			this.ClearLayoutButton.Click += new System.EventHandler(this.ClearLayoutButton_Click);
			// 
			// SetSizeButton
			// 
			this.SetSizeButton.Image = ((System.Drawing.Image)(resources.GetObject("SetSizeButton.Image")));
			this.SetSizeButton.Location = new System.Drawing.Point(211, 6);
			this.SetSizeButton.LookAndFeel.SkinName = "Office 2013";
			this.SetSizeButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.SetSizeButton.Name = "SetSizeButton";
			this.SetSizeButton.Size = new System.Drawing.Size(72, 23);
			this.SetSizeButton.TabIndex = 5;
			this.SetSizeButton.Text = "Set Size";
			this.SetSizeButton.Click += new System.EventHandler(this.SetSizeButton_Click);
			// 
			// SetLayoutButton
			// 
			this.SetLayoutButton.Image = ((System.Drawing.Image)(resources.GetObject("SetLayoutButton.Image")));
			this.SetLayoutButton.Location = new System.Drawing.Point(381, 6);
			this.SetLayoutButton.LookAndFeel.SkinName = "Office 2013";
			this.SetLayoutButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.SetLayoutButton.Name = "SetLayoutButton";
			this.SetLayoutButton.Size = new System.Drawing.Size(84, 23);
			this.SetLayoutButton.TabIndex = 4;
			this.SetLayoutButton.Text = "Set Layout";
			this.SetLayoutButton.Click += new System.EventHandler(this.SetLayoutButton_Click);
			// 
			// ButtonGetLayout
			// 
			this.ButtonGetLayout.Image = ((System.Drawing.Image)(resources.GetObject("ButtonGetLayout.Image")));
			this.ButtonGetLayout.Location = new System.Drawing.Point(289, 6);
			this.ButtonGetLayout.LookAndFeel.SkinName = "Office 2013";
			this.ButtonGetLayout.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ButtonGetLayout.Name = "ButtonGetLayout";
			this.ButtonGetLayout.Size = new System.Drawing.Size(86, 23);
			this.ButtonGetLayout.TabIndex = 3;
			this.ButtonGetLayout.Text = "Get Layout";
			this.ButtonGetLayout.Click += new System.EventHandler(this.ButtonGetLayout_Click);
			// 
			// labelControl2
			// 
			this.labelControl2.Location = new System.Drawing.Point(113, 12);
			this.labelControl2.Name = "labelControl2";
			this.labelControl2.Size = new System.Drawing.Size(31, 13);
			this.labelControl2.TabIndex = 2;
			this.labelControl2.Text = "Height";
			// 
			// labelControl1
			// 
			this.labelControl1.Location = new System.Drawing.Point(5, 12);
			this.labelControl1.Name = "labelControl1";
			this.labelControl1.Size = new System.Drawing.Size(28, 13);
			this.labelControl1.TabIndex = 0;
			this.labelControl1.Text = "Width";
			// 
			// TextEditHeight
			// 
			this.TextEditHeight.EditValue = "768";
			this.TextEditHeight.Location = new System.Drawing.Point(150, 9);
			this.TextEditHeight.Name = "TextEditHeight";
			this.TextEditHeight.Size = new System.Drawing.Size(55, 20);
			this.TextEditHeight.TabIndex = 1;
			// 
			// TextEditWidth
			// 
			this.TextEditWidth.EditValue = "1024";
			this.TextEditWidth.Location = new System.Drawing.Point(39, 9);
			this.TextEditWidth.Name = "TextEditWidth";
			this.TextEditWidth.Size = new System.Drawing.Size(55, 20);
			this.TextEditWidth.TabIndex = 0;
			// 
			// ScrollableControl
			// 
			this.ScrollableControl.Controls.Add(this.PanelContainer);
			this.ScrollableControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ScrollableControl.Location = new System.Drawing.Point(0, 34);
			this.ScrollableControl.Name = "ScrollableControl";
			this.ScrollableControl.Size = new System.Drawing.Size(1076, 812);
			this.ScrollableControl.TabIndex = 2;
			// 
			// PanelContainer
			// 
			this.PanelContainer.Location = new System.Drawing.Point(8, 3);
			this.PanelContainer.Margin = new System.Windows.Forms.Padding(0);
			this.PanelContainer.Name = "PanelContainer";
			this.PanelContainer.ObjectInspector = null;
			this.PanelContainer.Size = new System.Drawing.Size(1016, 765);
			this.PanelContainer.TabIndex = 0;
			this.PanelContainer.Scroll += new System.Windows.Forms.ScrollEventHandler(this.PanelContainer_Scroll);
			// 
			// xtraTabControl1
			// 
			this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
			this.xtraTabControl1.Name = "xtraTabControl1";
			this.xtraTabControl1.SelectedTabPage = this.TabPageViews;
			this.xtraTabControl1.Size = new System.Drawing.Size(1321, 874);
			this.xtraTabControl1.TabIndex = 4;
			this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.TabPageViews,
            this.TabPageDataSources,
            this.TabPageScripts,
            this.TabPageParameters,
            this.TabPageChildViews});
			// 
			// TabPageViews
			// 
			this.TabPageViews.Controls.Add(this.splitterControl1);
			this.TabPageViews.Controls.Add(this.ScrollableControl);
			this.TabPageViews.Controls.Add(this.ObjectInspectorPanel);
			this.TabPageViews.Controls.Add(this.PanelTools);
			this.TabPageViews.Name = "TabPageViews";
			this.TabPageViews.Size = new System.Drawing.Size(1315, 846);
			this.TabPageViews.Text = "View";
			// 
			// splitterControl1
			// 
			this.splitterControl1.Dock = System.Windows.Forms.DockStyle.Right;
			this.splitterControl1.Location = new System.Drawing.Point(1071, 34);
			this.splitterControl1.Name = "splitterControl1";
			this.splitterControl1.Size = new System.Drawing.Size(5, 812);
			this.splitterControl1.TabIndex = 1;
			this.splitterControl1.TabStop = false;
			// 
			// PanelTools
			// 
			this.PanelTools.Appearance.BackColor = System.Drawing.Color.White;
			this.PanelTools.Appearance.Options.UseBackColor = true;
			this.PanelTools.Controls.Add(this.GenerateViewButton);
			this.PanelTools.Controls.Add(this.ClearLayoutButton);
			this.PanelTools.Controls.Add(this.LabelRendered);
			this.PanelTools.Controls.Add(this.labelControl1);
			this.PanelTools.Controls.Add(this.TextEditWidth);
			this.PanelTools.Controls.Add(this.labelControl2);
			this.PanelTools.Controls.Add(this.SetLayoutButton);
			this.PanelTools.Controls.Add(this.SetSizeButton);
			this.PanelTools.Controls.Add(this.ButtonGetLayout);
			this.PanelTools.Controls.Add(this.TextEditHeight);
			this.PanelTools.Dock = System.Windows.Forms.DockStyle.Top;
			this.PanelTools.Location = new System.Drawing.Point(0, 0);
			this.PanelTools.LookAndFeel.SkinName = "Office 2013";
			this.PanelTools.LookAndFeel.UseDefaultLookAndFeel = false;
			this.PanelTools.Name = "PanelTools";
			this.PanelTools.Size = new System.Drawing.Size(1315, 34);
			this.PanelTools.TabIndex = 4;
			// 
			// GenerateViewButton
			// 
			this.GenerateViewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.GenerateViewButton.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.GenerateViewButton.Appearance.Options.UseFont = true;
			this.GenerateViewButton.Image = ((System.Drawing.Image)(resources.GetObject("GenerateViewButton.Image")));
			this.GenerateViewButton.Location = new System.Drawing.Point(1144, 0);
			this.GenerateViewButton.LookAndFeel.SkinName = "Office 2013";
			this.GenerateViewButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.GenerateViewButton.Name = "GenerateViewButton";
			this.GenerateViewButton.Size = new System.Drawing.Size(173, 34);
			this.GenerateViewButton.TabIndex = 8;
			this.GenerateViewButton.Text = "Generate View";
			this.GenerateViewButton.Click += new System.EventHandler(this.GenerateViewButton_Click);
			// 
			// TabPageDataSources
			// 
			this.TabPageDataSources.Controls.Add(this.DataSourceSurface);
			this.TabPageDataSources.Name = "TabPageDataSources";
			this.TabPageDataSources.Size = new System.Drawing.Size(1315, 846);
			this.TabPageDataSources.Text = "Data Sources";
			// 
			// DataSourceSurface
			// 
			this.DataSourceSurface.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DataSourceSurface.Location = new System.Drawing.Point(0, 0);
			this.DataSourceSurface.Name = "DataSourceSurface";
			this.DataSourceSurface.ObjectInspector = null;
			this.DataSourceSurface.Size = new System.Drawing.Size(1315, 846);
			this.DataSourceSurface.TabIndex = 0;
			// 
			// TabPageScripts
			// 
			this.TabPageScripts.Controls.Add(this.ScriptSurface);
			this.TabPageScripts.Name = "TabPageScripts";
			this.TabPageScripts.Size = new System.Drawing.Size(1315, 846);
			this.TabPageScripts.Text = "Scripts";
			// 
			// ScriptSurface
			// 
			this.ScriptSurface.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ScriptSurface.Location = new System.Drawing.Point(0, 0);
			this.ScriptSurface.Name = "ScriptSurface";
			this.ScriptSurface.ObjectInspector = null;
			this.ScriptSurface.Size = new System.Drawing.Size(1315, 846);
			this.ScriptSurface.TabIndex = 0;
			// 
			// TabPageParameters
			// 
			this.TabPageParameters.Controls.Add(this.ParametersSurface);
			this.TabPageParameters.Name = "TabPageParameters";
			this.TabPageParameters.Size = new System.Drawing.Size(1315, 846);
			this.TabPageParameters.Text = "Parameters";
			// 
			// ParametersSurface
			// 
			this.ParametersSurface.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ParametersSurface.Location = new System.Drawing.Point(0, 0);
			this.ParametersSurface.Name = "ParametersSurface";
			this.ParametersSurface.Size = new System.Drawing.Size(1315, 846);
			this.ParametersSurface.TabIndex = 0;
			// 
			// TabPageChildViews
			// 
			this.TabPageChildViews.Controls.Add(this.ChildViewSurface);
			this.TabPageChildViews.Name = "TabPageChildViews";
			this.TabPageChildViews.Size = new System.Drawing.Size(1315, 846);
			this.TabPageChildViews.Text = "Child Views";
			// 
			// ChildViewSurface
			// 
			this.ChildViewSurface.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ChildViewSurface.Location = new System.Drawing.Point(0, 0);
			this.ChildViewSurface.Name = "ChildViewSurface";
			this.ChildViewSurface.ObjectInspector = null;
			this.ChildViewSurface.Size = new System.Drawing.Size(1315, 846);
			this.ChildViewSurface.TabIndex = 0;
			// 
			// DesignerControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.xtraTabControl1);
			this.Name = "DesignerControl";
			this.Size = new System.Drawing.Size(1321, 874);
			((System.ComponentModel.ISupportInitialize)(this.ObjectInspectorPanel)).EndInit();
			this.ObjectInspectorPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.TextEditHeight.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TextEditWidth.Properties)).EndInit();
			this.ScrollableControl.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
			this.xtraTabControl1.ResumeLayout(false);
			this.TabPageViews.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.PanelTools)).EndInit();
			this.PanelTools.ResumeLayout(false);
			this.PanelTools.PerformLayout();
			this.TabPageDataSources.ResumeLayout(false);
			this.TabPageScripts.ResumeLayout(false);
			this.TabPageParameters.ResumeLayout(false);
			this.TabPageChildViews.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

		private DevExpress.XtraEditors.PanelControl ObjectInspectorPanel;
		private DevExpress.XtraEditors.XtraScrollableControl ScrollableControl;
		private DevExpress.XtraEditors.LabelControl labelControl2;
		private DevExpress.XtraEditors.LabelControl labelControl1;
		private DevExpress.XtraEditors.TextEdit TextEditHeight;
		private DevExpress.XtraEditors.TextEdit TextEditWidth;
		private DevExpress.XtraEditors.SimpleButton ButtonGetLayout;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage TabPageViews;
        private DevExpress.XtraTab.XtraTabPage TabPageDataSources;
        private DesignerSurface.DataSourceSurface DataSourceSurface;
        private DevExpress.XtraTab.XtraTabPage TabPageScripts;
        private DesignerSurface.ScriptSurface ScriptSurface;
        private DevExpress.XtraTab.XtraTabPage TabPageParameters;
        private DesignerSurface.ParameterSurface ParametersSurface;
        private DevExpress.XtraEditors.SimpleButton SetLayoutButton;
		private DevExpress.XtraEditors.SimpleButton SetSizeButton;
        private DevExpress.XtraEditors.SimpleButton ClearLayoutButton;
		private DevExpress.XtraEditors.LabelControl LabelRendered;
        private ObjectInspectorTree ObjectInspector;
		private DevExpress.XtraEditors.PanelControl PanelTools;
        private Controls.LayoutPanels.Panel PanelContainer;
		private DevExpress.XtraTab.XtraTabPage TabPageChildViews;
		private DesignerSurface.ChildViewSurface ChildViewSurface;
		private DevExpress.XtraEditors.SplitterControl splitterControl1;
		private DevExpress.XtraEditors.SimpleButton GenerateViewButton;









    }
}