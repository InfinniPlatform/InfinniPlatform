namespace InfinniPlatform.MetadataDesigner.Views
{
	partial class ScenarioDesignerView
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
			this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
			this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
			this.RefreshButton = new DevExpress.XtraEditors.SimpleButton();
			this.ComboBoxScenarioActionType = new DevExpress.XtraEditors.ImageComboBoxEdit();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.CreateScenarioButton = new DevExpress.XtraEditors.SimpleButton();
			this.TextEditScenarioCaption = new DevExpress.XtraEditors.TextEdit();
			this.ComboBoxScenarioIdentifier = new DevExpress.XtraEditors.ImageComboBoxEdit();
			this.label8 = new System.Windows.Forms.Label();
			this.TextEditScenarioDescription = new DevExpress.XtraEditors.TextEdit();
			this.label13 = new System.Windows.Forms.Label();
			this.ComboBoxScenarioContextType = new DevExpress.XtraEditors.ImageComboBoxEdit();
			this.label12 = new System.Windows.Forms.Label();
			this.sourceAssemblies = new System.Windows.Forms.BindingSource();
			this.sourceScenario = new System.Windows.Forms.BindingSource();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.panelControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
			this.groupControl2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxScenarioActionType.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TextEditScenarioCaption.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxScenarioIdentifier.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TextEditScenarioDescription.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxScenarioContextType.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.sourceAssemblies)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.sourceScenario)).BeginInit();
			this.SuspendLayout();
			// 
			// panelControl1
			// 
			this.panelControl1.Controls.Add(this.groupControl2);
			this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelControl1.Location = new System.Drawing.Point(0, 0);
			this.panelControl1.LookAndFeel.SkinName = "Office 2013";
			this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(598, 173);
			this.panelControl1.TabIndex = 0;
			// 
			// groupControl2
			// 
			this.groupControl2.Controls.Add(this.RefreshButton);
			this.groupControl2.Controls.Add(this.ComboBoxScenarioActionType);
			this.groupControl2.Controls.Add(this.label10);
			this.groupControl2.Controls.Add(this.label9);
			this.groupControl2.Controls.Add(this.CreateScenarioButton);
			this.groupControl2.Controls.Add(this.TextEditScenarioCaption);
			this.groupControl2.Controls.Add(this.ComboBoxScenarioIdentifier);
			this.groupControl2.Controls.Add(this.label8);
			this.groupControl2.Controls.Add(this.TextEditScenarioDescription);
			this.groupControl2.Controls.Add(this.label13);
			this.groupControl2.Controls.Add(this.ComboBoxScenarioContextType);
			this.groupControl2.Controls.Add(this.label12);
			this.groupControl2.Location = new System.Drawing.Point(7, 5);
			this.groupControl2.LookAndFeel.SkinName = "Office 2013";
			this.groupControl2.LookAndFeel.UseDefaultLookAndFeel = false;
			this.groupControl2.Name = "groupControl2";
			this.groupControl2.Size = new System.Drawing.Size(583, 157);
			this.groupControl2.TabIndex = 46;
			this.groupControl2.Text = "Реквизиты сценария";
			// 
			// RefreshButton
			// 
			this.RefreshButton.Location = new System.Drawing.Point(404, 50);
			this.RefreshButton.LookAndFeel.SkinName = "Office 2013";
			this.RefreshButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.RefreshButton.Name = "RefreshButton";
			this.RefreshButton.Size = new System.Drawing.Size(162, 24);
			this.RefreshButton.TabIndex = 53;
			this.RefreshButton.Text = "Обновить список сценариев";
			this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
			// 
			// ComboBoxScenarioActionType
			// 
			this.ComboBoxScenarioActionType.Location = new System.Drawing.Point(172, 50);
			this.ComboBoxScenarioActionType.Name = "ComboBoxScenarioActionType";
			this.ComboBoxScenarioActionType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.ComboBoxScenarioActionType.Properties.ReadOnly = true;
			this.ComboBoxScenarioActionType.Size = new System.Drawing.Size(226, 20);
			this.ComboBoxScenarioActionType.TabIndex = 50;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(15, 25);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(56, 13);
			this.label10.TabIndex = 42;
			this.label10.Text = "Сценарий";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(14, 105);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(112, 13);
			this.label9.TabIndex = 43;
			this.label9.Text = "Заголовок сценария";
			// 
			// CreateScenarioButton
			// 
			this.CreateScenarioButton.Location = new System.Drawing.Point(404, 20);
			this.CreateScenarioButton.LookAndFeel.SkinName = "Office 2013";
			this.CreateScenarioButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.CreateScenarioButton.Name = "CreateScenarioButton";
			this.CreateScenarioButton.Size = new System.Drawing.Size(162, 24);
			this.CreateScenarioButton.TabIndex = 52;
			this.CreateScenarioButton.Text = "Сформировать метаданные";
			this.CreateScenarioButton.Click += new System.EventHandler(this.CreateScenarioButton_Click);
			// 
			// TextEditScenarioCaption
			// 
			this.TextEditScenarioCaption.Location = new System.Drawing.Point(172, 102);
			this.TextEditScenarioCaption.Name = "TextEditScenarioCaption";
			this.TextEditScenarioCaption.Size = new System.Drawing.Size(226, 20);
			this.TextEditScenarioCaption.TabIndex = 44;
			// 
			// ComboBoxScenarioIdentifier
			// 
			this.ComboBoxScenarioIdentifier.Location = new System.Drawing.Point(172, 24);
			this.ComboBoxScenarioIdentifier.Name = "ComboBoxScenarioIdentifier";
			this.ComboBoxScenarioIdentifier.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.ComboBoxScenarioIdentifier.Size = new System.Drawing.Size(226, 20);
			this.ComboBoxScenarioIdentifier.TabIndex = 51;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(15, 131);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(57, 13);
			this.label8.TabIndex = 45;
			this.label8.Text = "Описание";
			// 
			// TextEditScenarioDescription
			// 
			this.TextEditScenarioDescription.Location = new System.Drawing.Point(172, 128);
			this.TextEditScenarioDescription.Name = "TextEditScenarioDescription";
			this.TextEditScenarioDescription.Size = new System.Drawing.Size(226, 20);
			this.TextEditScenarioDescription.TabIndex = 46;
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(14, 53);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(91, 13);
			this.label13.TabIndex = 49;
			this.label13.Text = "Тип применения";
			// 
			// ComboBoxScenarioContextType
			// 
			this.ComboBoxScenarioContextType.Location = new System.Drawing.Point(172, 76);
			this.ComboBoxScenarioContextType.Name = "ComboBoxScenarioContextType";
			this.ComboBoxScenarioContextType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.ComboBoxScenarioContextType.Properties.ReadOnly = true;
			this.ComboBoxScenarioContextType.Size = new System.Drawing.Size(226, 20);
			this.ComboBoxScenarioContextType.TabIndex = 47;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(14, 79);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(81, 13);
			this.label12.TabIndex = 48;
			this.label12.Text = "Тип контекста";
			// 
			// ScenarioDesignerView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panelControl1);
			this.Name = "ScenarioDesignerView";
			this.Size = new System.Drawing.Size(598, 173);
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.panelControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
			this.groupControl2.ResumeLayout(false);
			this.groupControl2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxScenarioActionType.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TextEditScenarioCaption.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxScenarioIdentifier.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TextEditScenarioDescription.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ComboBoxScenarioContextType.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.sourceAssemblies)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.sourceScenario)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.PanelControl panelControl1;
		public System.Windows.Forms.BindingSource sourceScenario;
		public System.Windows.Forms.BindingSource sourceAssemblies;
		private DevExpress.XtraEditors.GroupControl groupControl2;
		public DevExpress.XtraEditors.ImageComboBoxEdit ComboBoxScenarioActionType;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label9;
		private DevExpress.XtraEditors.SimpleButton CreateScenarioButton;
		public DevExpress.XtraEditors.TextEdit TextEditScenarioCaption;
		public DevExpress.XtraEditors.ImageComboBoxEdit ComboBoxScenarioIdentifier;
		private System.Windows.Forms.Label label8;
		public DevExpress.XtraEditors.TextEdit TextEditScenarioDescription;
		private System.Windows.Forms.Label label13;
		public DevExpress.XtraEditors.ImageComboBoxEdit ComboBoxScenarioContextType;
		private System.Windows.Forms.Label label12;
		private DevExpress.XtraEditors.SimpleButton RefreshButton;

	}
}
