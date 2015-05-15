namespace InfinniPlatform.MetadataDesigner.Views
{
	partial class ServiceDesignerView
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
			this.components = new System.ComponentModel.Container();
			this.panelControl5 = new DevExpress.XtraEditors.PanelControl();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.ComboBoxScenarioId = new DevExpress.XtraEditors.ImageComboBoxEdit();
			this.label15 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.ComboBoxExtensionPoint = new DevExpress.XtraEditors.ImageComboBoxEdit();
			this.ButtonDeleteExtensionPoint = new DevExpress.XtraEditors.SimpleButton();
			this.gridControl3 = new DevExpress.XtraGrid.GridControl();
			this.sourceExtensionPoints = new System.Windows.Forms.BindingSource(this.components);
			this.GridViewExtensionPoints = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridView6 = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.ButtonAddExtensionPoint = new DevExpress.XtraEditors.SimpleButton();
			this.ComboBoxServiceType = new DevExpress.XtraEditors.ImageComboBoxEdit();
			this.ButtonCreateService = new DevExpress.XtraEditors.SimpleButton();
			this.TextEditServiceCaption = new DevExpress.XtraEditors.TextEdit();
			this.label17 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.TextEditServiceName = new DevExpress.XtraEditors.TextEdit();
			this.label19 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.panelControl5)).BeginInit();
			this.panelControl5.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxScenarioId.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxExtensionPoint.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridControl3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.sourceExtensionPoints)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.GridViewExtensionPoints)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxServiceType.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TextEditServiceCaption.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TextEditServiceName.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// panelControl5
			// 
			this.panelControl5.Controls.Add(this.groupBox1);
			this.panelControl5.Controls.Add(this.ComboBoxServiceType);
			this.panelControl5.Controls.Add(this.ButtonCreateService);
			this.panelControl5.Controls.Add(this.TextEditServiceCaption);
			this.panelControl5.Controls.Add(this.label17);
			this.panelControl5.Controls.Add(this.label18);
			this.panelControl5.Controls.Add(this.TextEditServiceName);
			this.panelControl5.Controls.Add(this.label19);
			this.panelControl5.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelControl5.Location = new System.Drawing.Point(0, 0);
			this.panelControl5.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
			this.panelControl5.LookAndFeel.UseDefaultLookAndFeel = false;
			this.panelControl5.Name = "panelControl5";
			this.panelControl5.Size = new System.Drawing.Size(808, 372);
			this.panelControl5.TabIndex = 2;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.ComboBoxScenarioId);
			this.groupBox1.Controls.Add(this.label15);
			this.groupBox1.Controls.Add(this.label14);
			this.groupBox1.Controls.Add(this.ComboBoxExtensionPoint);
			this.groupBox1.Controls.Add(this.ButtonDeleteExtensionPoint);
			this.groupBox1.Controls.Add(this.gridControl3);
			this.groupBox1.Controls.Add(this.ButtonAddExtensionPoint);
			this.groupBox1.Location = new System.Drawing.Point(13, 101);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(599, 266);
			this.groupBox1.TabIndex = 32;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Точки расширения";
			// 
			// ComboBoxScenarioId
			// 
			this.ComboBoxScenarioId.Location = new System.Drawing.Point(155, 51);
			this.ComboBoxScenarioId.Name = "ComboBoxScenarioId";
			this.ComboBoxScenarioId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.ComboBoxScenarioId.Properties.LookAndFeel.SkinName = "Office 2013";
			this.ComboBoxScenarioId.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ComboBoxScenarioId.Size = new System.Drawing.Size(277, 20);
			this.ComboBoxScenarioId.TabIndex = 38;
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(18, 54);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(56, 13);
			this.label15.TabIndex = 37;
			this.label15.Text = "Сценарий";
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(18, 25);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(102, 13);
			this.label14.TabIndex = 36;
			this.label14.Text = "Точка расширения";
			// 
			// ComboBoxExtensionPoint
			// 
			this.ComboBoxExtensionPoint.Location = new System.Drawing.Point(155, 22);
			this.ComboBoxExtensionPoint.Name = "ComboBoxExtensionPoint";
			this.ComboBoxExtensionPoint.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.ComboBoxExtensionPoint.Properties.LookAndFeel.SkinName = "Office 2013";
			this.ComboBoxExtensionPoint.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ComboBoxExtensionPoint.Size = new System.Drawing.Size(277, 20);
			this.ComboBoxExtensionPoint.TabIndex = 34;
			// 
			// ButtonDeleteExtensionPoint
			// 
			this.ButtonDeleteExtensionPoint.Location = new System.Drawing.Point(438, 48);
			this.ButtonDeleteExtensionPoint.LookAndFeel.SkinName = "Office 2013";
			this.ButtonDeleteExtensionPoint.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ButtonDeleteExtensionPoint.Name = "ButtonDeleteExtensionPoint";
			this.ButtonDeleteExtensionPoint.Size = new System.Drawing.Size(155, 23);
			this.ButtonDeleteExtensionPoint.TabIndex = 33;
			this.ButtonDeleteExtensionPoint.Text = "Удалить точку расширения";
			this.ButtonDeleteExtensionPoint.Click += new System.EventHandler(this.ButtonDeleteExtensionPoint_Click);
			// 
			// gridControl3
			// 
			this.gridControl3.DataSource = this.sourceExtensionPoints;
			this.gridControl3.Location = new System.Drawing.Point(6, 87);
			this.gridControl3.LookAndFeel.SkinName = "Office 2013";
			this.gridControl3.LookAndFeel.UseDefaultLookAndFeel = false;
			this.gridControl3.MainView = this.GridViewExtensionPoints;
			this.gridControl3.Name = "gridControl3";
			this.gridControl3.Size = new System.Drawing.Size(587, 173);
			this.gridControl3.TabIndex = 32;
			this.gridControl3.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridViewExtensionPoints,
            this.gridView6});
			// 
			// GridViewExtensionPoints
			// 
			this.GridViewExtensionPoints.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn12,
            this.gridColumn13});
			this.GridViewExtensionPoints.GridControl = this.gridControl3;
			this.GridViewExtensionPoints.Name = "GridViewExtensionPoints";
			this.GridViewExtensionPoints.OptionsBehavior.Editable = false;
			this.GridViewExtensionPoints.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.Hidden;
			this.GridViewExtensionPoints.OptionsView.ShowGroupPanel = false;
			this.GridViewExtensionPoints.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.GridViewExtensionPoints_CustomDrawCell);
			this.GridViewExtensionPoints.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.ExtensionPointFocusedRowChanged);
			// 
			// gridColumn12
			// 
			this.gridColumn12.Caption = "Наименование";
			this.gridColumn12.FieldName = "TypeName";
			this.gridColumn12.Name = "gridColumn12";
			this.gridColumn12.Visible = true;
			this.gridColumn12.VisibleIndex = 0;
			this.gridColumn12.Width = 176;
			// 
			// gridColumn13
			// 
			this.gridColumn13.Caption = "Сценарий";
			this.gridColumn13.FieldName = "ScenarioId";
			this.gridColumn13.Name = "gridColumn13";
			this.gridColumn13.Visible = true;
			this.gridColumn13.VisibleIndex = 1;
			this.gridColumn13.Width = 285;
			// 
			// gridView6
			// 
			this.gridView6.GridControl = this.gridControl3;
			this.gridView6.Name = "gridView6";
			// 
			// ButtonAddExtensionPoint
			// 
			this.ButtonAddExtensionPoint.Location = new System.Drawing.Point(438, 19);
			this.ButtonAddExtensionPoint.LookAndFeel.SkinName = "Office 2013";
			this.ButtonAddExtensionPoint.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ButtonAddExtensionPoint.Name = "ButtonAddExtensionPoint";
			this.ButtonAddExtensionPoint.Size = new System.Drawing.Size(155, 23);
			this.ButtonAddExtensionPoint.TabIndex = 31;
			this.ButtonAddExtensionPoint.Text = "Добавить точку расширения";
			this.ButtonAddExtensionPoint.Click += new System.EventHandler(this.ButtonAddExtensionPoint_Click);
			// 
			// ComboBoxServiceType
			// 
			this.ComboBoxServiceType.Location = new System.Drawing.Point(168, 45);
			this.ComboBoxServiceType.Name = "ComboBoxServiceType";
			this.ComboBoxServiceType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.ComboBoxServiceType.Properties.LookAndFeel.SkinName = "Office 2013";
			this.ComboBoxServiceType.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ComboBoxServiceType.Size = new System.Drawing.Size(277, 20);
			this.ComboBoxServiceType.TabIndex = 28;
			this.ComboBoxServiceType.EditValueChanged += new System.EventHandler(this.ComboBoxServiceType_EditValueChanged);
			// 
			// ButtonCreateService
			// 
			this.ButtonCreateService.Location = new System.Drawing.Point(451, 12);
			this.ButtonCreateService.LookAndFeel.SkinName = "Office 2013";
			this.ButtonCreateService.LookAndFeel.UseDefaultLookAndFeel = false;
			this.ButtonCreateService.Name = "ButtonCreateService";
			this.ButtonCreateService.Size = new System.Drawing.Size(155, 23);
			this.ButtonCreateService.TabIndex = 23;
			this.ButtonCreateService.Text = "Сформировать метаданные";
			this.ButtonCreateService.Click += new System.EventHandler(this.ButtonCreateService_Click);
			// 
			// TextEditServiceCaption
			// 
			this.TextEditServiceCaption.Location = new System.Drawing.Point(168, 75);
			this.TextEditServiceCaption.Name = "TextEditServiceCaption";
			this.TextEditServiceCaption.Size = new System.Drawing.Size(277, 20);
			this.TextEditServiceCaption.TabIndex = 19;
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(16, 78);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(106, 13);
			this.label17.TabIndex = 18;
			this.label17.Text = "Заголовок сервиса";
			// 
			// label18
			// 
			this.label18.AutoSize = true;
			this.label18.Location = new System.Drawing.Point(16, 48);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(71, 13);
			this.label18.TabIndex = 16;
			this.label18.Text = "Тип сервиса";
			// 
			// TextEditServiceName
			// 
			this.TextEditServiceName.Location = new System.Drawing.Point(168, 15);
			this.TextEditServiceName.Name = "TextEditServiceName";
			this.TextEditServiceName.Properties.LookAndFeel.SkinName = "Office 2013";
			this.TextEditServiceName.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
			this.TextEditServiceName.Size = new System.Drawing.Size(277, 20);
			this.TextEditServiceName.TabIndex = 15;
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(16, 18);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(128, 13);
			this.label19.TabIndex = 14;
			this.label19.Text = "Наименование сервиса";
			// 
			// ServiceDesignerView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panelControl5);
			this.Name = "ServiceDesignerView";
			this.Size = new System.Drawing.Size(808, 376);
			((System.ComponentModel.ISupportInitialize)(this.panelControl5)).EndInit();
			this.panelControl5.ResumeLayout(false);
			this.panelControl5.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxScenarioId.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxExtensionPoint.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridControl3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.sourceExtensionPoints)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.GridViewExtensionPoints)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxServiceType.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TextEditServiceCaption.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TextEditServiceName.Properties)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

        private DevExpress.XtraEditors.PanelControl panelControl5;
        private System.Windows.Forms.GroupBox groupBox1;
        public DevExpress.XtraEditors.ImageComboBoxEdit ComboBoxScenarioId;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        public DevExpress.XtraEditors.ImageComboBoxEdit ComboBoxExtensionPoint;
        public DevExpress.XtraEditors.SimpleButton ButtonDeleteExtensionPoint;
        public DevExpress.XtraGrid.GridControl gridControl3;
        public DevExpress.XtraGrid.Views.Grid.GridView GridViewExtensionPoints;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView6;
        public DevExpress.XtraEditors.SimpleButton ButtonAddExtensionPoint;
        public DevExpress.XtraEditors.ImageComboBoxEdit ComboBoxServiceType;
        public DevExpress.XtraEditors.SimpleButton ButtonCreateService;
        public DevExpress.XtraEditors.TextEdit TextEditServiceCaption;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        public DevExpress.XtraEditors.TextEdit TextEditServiceName;
        private System.Windows.Forms.Label label19;
        public System.Windows.Forms.BindingSource sourceExtensionPoints;
	}
}
