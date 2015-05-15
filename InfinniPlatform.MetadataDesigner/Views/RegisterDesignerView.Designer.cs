using System.Drawing;
namespace InfinniPlatform.MetadataDesigner.Views
{
    partial class RegisterDesignerView
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
            this.GeneralGroupControl = new DevExpress.XtraEditors.GroupControl();
            this.labelControlName = new DevExpress.XtraEditors.LabelControl();
            this.PeriodComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.RegisterNameTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.labelControlRegisterType = new DevExpress.XtraEditors.LabelControl();
            this.CreateButton = new DevExpress.XtraEditors.SimpleButton();
            this.PeriodLabelControl = new DevExpress.XtraEditors.LabelControl();
            this.AsynchronousCheckBox = new System.Windows.Forms.CheckBox();
            this.RegisterTypeEditor = new DevExpress.XtraEditors.ComboBoxEdit();
            this.PropertiesPanelControl = new DevExpress.XtraEditors.PanelControl();
            this.PropertiesGroupControl = new DevExpress.XtraEditors.GroupControl();
            this.PropertyTypeComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.PropertyTypeLabelControl = new DevExpress.XtraEditors.LabelControl();
            this.DataTypeComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.DataTypeLabelControl = new DevExpress.XtraEditors.LabelControl();
            this.PropertyNameControl = new DevExpress.XtraEditors.LabelControl();
            this.PropertyNameTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.DeletePropertyButton = new DevExpress.XtraEditors.SimpleButton();
            this.AddPropertyButton = new DevExpress.XtraEditors.SimpleButton();
            this.PropertiesListBoxControl = new DevExpress.XtraEditors.ListBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GeneralGroupControl)).BeginInit();
            this.GeneralGroupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PeriodComboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RegisterNameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RegisterTypeEditor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesPanelControl)).BeginInit();
            this.PropertiesPanelControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesGroupControl)).BeginInit();
            this.PropertiesGroupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PropertyTypeComboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataTypeComboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropertyNameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesListBoxControl)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.GeneralGroupControl);
            this.panelControl1.Controls.Add(this.PropertiesPanelControl);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.LookAndFeel.SkinName = "Office 2013";
            this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(576, 509);
            this.panelControl1.TabIndex = 0;
            // 
            // GeneralGroupControl
            // 
            this.GeneralGroupControl.Controls.Add(this.labelControlName);
            this.GeneralGroupControl.Controls.Add(this.PeriodComboBoxEdit);
            this.GeneralGroupControl.Controls.Add(this.RegisterNameTextEdit);
            this.GeneralGroupControl.Controls.Add(this.labelControlRegisterType);
            this.GeneralGroupControl.Controls.Add(this.CreateButton);
            this.GeneralGroupControl.Controls.Add(this.PeriodLabelControl);
            this.GeneralGroupControl.Controls.Add(this.AsynchronousCheckBox);
            this.GeneralGroupControl.Controls.Add(this.RegisterTypeEditor);
            this.GeneralGroupControl.Location = new System.Drawing.Point(5, 5);
            this.GeneralGroupControl.LookAndFeel.SkinName = "Office 2013";
            this.GeneralGroupControl.LookAndFeel.UseDefaultLookAndFeel = false;
            this.GeneralGroupControl.Name = "GeneralGroupControl";
            this.GeneralGroupControl.Size = new System.Drawing.Size(548, 170);
            this.GeneralGroupControl.TabIndex = 12;
            this.GeneralGroupControl.Text = "Common requisites";
            // 
            // labelControlName
            // 
            this.labelControlName.Location = new System.Drawing.Point(21, 24);
            this.labelControlName.LookAndFeel.SkinName = "Office 2013";
            this.labelControlName.LookAndFeel.UseDefaultLookAndFeel = false;
            this.labelControlName.Name = "labelControlName";
            this.labelControlName.Size = new System.Drawing.Size(69, 13);
            this.labelControlName.TabIndex = 1;
            this.labelControlName.Text = "Register name";
            // 
            // PeriodComboBoxEdit
            // 
            this.PeriodComboBoxEdit.Location = new System.Drawing.Point(197, 79);
            this.PeriodComboBoxEdit.Name = "PeriodComboBoxEdit";
            this.PeriodComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.PeriodComboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.PeriodComboBoxEdit.Size = new System.Drawing.Size(217, 20);
            this.PeriodComboBoxEdit.TabIndex = 8;
            this.PeriodComboBoxEdit.SelectedIndexChanged += new System.EventHandler(this.PeriodComboBoxEdit_SelectedIndexChanged);
            // 
            // RegisterNameTextEdit
            // 
            this.RegisterNameTextEdit.Location = new System.Drawing.Point(197, 21);
            this.RegisterNameTextEdit.Name = "RegisterNameTextEdit";
            this.RegisterNameTextEdit.Size = new System.Drawing.Size(217, 20);
            this.RegisterNameTextEdit.TabIndex = 0;
            // 
            // labelControlRegisterType
            // 
            this.labelControlRegisterType.Location = new System.Drawing.Point(21, 52);
            this.labelControlRegisterType.LookAndFeel.SkinName = "Office 2013";
            this.labelControlRegisterType.LookAndFeel.UseDefaultLookAndFeel = false;
            this.labelControlRegisterType.Name = "labelControlRegisterType";
            this.labelControlRegisterType.Size = new System.Drawing.Size(65, 13);
            this.labelControlRegisterType.TabIndex = 3;
            this.labelControlRegisterType.Text = "Register type";
            // 
            // CreateButton
            // 
            this.CreateButton.Location = new System.Drawing.Point(420, 18);
            this.CreateButton.LookAndFeel.SkinName = "Office 2013";
            this.CreateButton.LookAndFeel.UseDefaultLookAndFeel = false;
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(122, 23);
            this.CreateButton.TabIndex = 10;
            this.CreateButton.Text = "Create";
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // PeriodLabelControl
            // 
            this.PeriodLabelControl.Location = new System.Drawing.Point(21, 82);
            this.PeriodLabelControl.LookAndFeel.SkinName = "Office 2013";
            this.PeriodLabelControl.LookAndFeel.UseDefaultLookAndFeel = false;
            this.PeriodLabelControl.Name = "PeriodLabelControl";
            this.PeriodLabelControl.Size = new System.Drawing.Size(108, 13);
            this.PeriodLabelControl.TabIndex = 5;
            this.PeriodLabelControl.Text = "Register writing period";
            // 
            // AsynchronousCheckBox
            // 
            this.AsynchronousCheckBox.AutoSize = true;
            this.AsynchronousCheckBox.Location = new System.Drawing.Point(21, 114);
            this.AsynchronousCheckBox.Name = "AsynchronousCheckBox";
            this.AsynchronousCheckBox.Size = new System.Drawing.Size(126, 17);
            this.AsynchronousCheckBox.TabIndex = 9;
            this.AsynchronousCheckBox.Text = "Asynchronous writing";
            this.AsynchronousCheckBox.UseVisualStyleBackColor = true;
            this.AsynchronousCheckBox.CheckedChanged += new System.EventHandler(this.AsynchronousCheckBox_CheckedChanged);
            // 
            // RegisterTypeEditor
            // 
            this.RegisterTypeEditor.Location = new System.Drawing.Point(197, 49);
            this.RegisterTypeEditor.Name = "RegisterTypeEditor";
            this.RegisterTypeEditor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.RegisterTypeEditor.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.RegisterTypeEditor.Size = new System.Drawing.Size(217, 20);
            this.RegisterTypeEditor.TabIndex = 6;
            this.RegisterTypeEditor.SelectedIndexChanged += new System.EventHandler(this.RegisterTypeEditor_SelectedIndexChanged);
            // 
            // PropertiesPanelControl
            // 
            this.PropertiesPanelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.PropertiesPanelControl.Controls.Add(this.PropertiesGroupControl);
            this.PropertiesPanelControl.Location = new System.Drawing.Point(5, 178);
            this.PropertiesPanelControl.LookAndFeel.SkinName = "Office 2013";
            this.PropertiesPanelControl.LookAndFeel.UseDefaultLookAndFeel = false;
            this.PropertiesPanelControl.Name = "PropertiesPanelControl";
            this.PropertiesPanelControl.Size = new System.Drawing.Size(566, 296);
            this.PropertiesPanelControl.TabIndex = 11;
            // 
            // PropertiesGroupControl
            // 
            this.PropertiesGroupControl.Controls.Add(this.PropertyTypeComboBoxEdit);
            this.PropertiesGroupControl.Controls.Add(this.PropertyTypeLabelControl);
            this.PropertiesGroupControl.Controls.Add(this.DataTypeComboBoxEdit);
            this.PropertiesGroupControl.Controls.Add(this.DataTypeLabelControl);
            this.PropertiesGroupControl.Controls.Add(this.PropertyNameControl);
            this.PropertiesGroupControl.Controls.Add(this.PropertyNameTextEdit);
            this.PropertiesGroupControl.Controls.Add(this.DeletePropertyButton);
            this.PropertiesGroupControl.Controls.Add(this.AddPropertyButton);
            this.PropertiesGroupControl.Controls.Add(this.PropertiesListBoxControl);
            this.PropertiesGroupControl.Location = new System.Drawing.Point(1, 12);
            this.PropertiesGroupControl.LookAndFeel.SkinName = "Office 2013";
            this.PropertiesGroupControl.LookAndFeel.UseDefaultLookAndFeel = false;
            this.PropertiesGroupControl.Name = "PropertiesGroupControl";
            this.PropertiesGroupControl.Size = new System.Drawing.Size(546, 275);
            this.PropertiesGroupControl.TabIndex = 7;
            this.PropertiesGroupControl.Text = "Register properties";
            // 
            // PropertyTypeComboBoxEdit
            // 
            this.PropertyTypeComboBoxEdit.Location = new System.Drawing.Point(196, 81);
            this.PropertyTypeComboBoxEdit.Name = "PropertyTypeComboBoxEdit";
            this.PropertyTypeComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.PropertyTypeComboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.PropertyTypeComboBoxEdit.Size = new System.Drawing.Size(216, 20);
            this.PropertyTypeComboBoxEdit.TabIndex = 12;
            // 
            // PropertyTypeLabelControl
            // 
            this.PropertyTypeLabelControl.Location = new System.Drawing.Point(16, 88);
            this.PropertyTypeLabelControl.LookAndFeel.SkinName = "Office 2013";
            this.PropertyTypeLabelControl.LookAndFeel.UseDefaultLookAndFeel = false;
            this.PropertyTypeLabelControl.Name = "PropertyTypeLabelControl";
            this.PropertyTypeLabelControl.Size = new System.Drawing.Size(67, 13);
            this.PropertyTypeLabelControl.TabIndex = 11;
            this.PropertyTypeLabelControl.Text = "Property type";
            // 
            // DataTypeComboBoxEdit
            // 
            this.DataTypeComboBoxEdit.Location = new System.Drawing.Point(196, 55);
            this.DataTypeComboBoxEdit.Name = "DataTypeComboBoxEdit";
            this.DataTypeComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DataTypeComboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.DataTypeComboBoxEdit.Size = new System.Drawing.Size(216, 20);
            this.DataTypeComboBoxEdit.TabIndex = 10;
            // 
            // DataTypeLabelControl
            // 
            this.DataTypeLabelControl.Location = new System.Drawing.Point(16, 62);
            this.DataTypeLabelControl.LookAndFeel.SkinName = "Office 2013";
            this.DataTypeLabelControl.LookAndFeel.UseDefaultLookAndFeel = false;
            this.DataTypeLabelControl.Name = "DataTypeLabelControl";
            this.DataTypeLabelControl.Size = new System.Drawing.Size(48, 13);
            this.DataTypeLabelControl.TabIndex = 9;
            this.DataTypeLabelControl.Text = "Data type";
            // 
            // PropertyNameControl
            // 
            this.PropertyNameControl.Location = new System.Drawing.Point(16, 36);
            this.PropertyNameControl.LookAndFeel.SkinName = "Office 2013";
            this.PropertyNameControl.LookAndFeel.UseDefaultLookAndFeel = false;
            this.PropertyNameControl.Name = "PropertyNameControl";
            this.PropertyNameControl.Size = new System.Drawing.Size(71, 13);
            this.PropertyNameControl.TabIndex = 8;
            this.PropertyNameControl.Text = "Property name";
            // 
            // PropertyNameTextEdit
            // 
            this.PropertyNameTextEdit.Location = new System.Drawing.Point(196, 29);
            this.PropertyNameTextEdit.Name = "PropertyNameTextEdit";
            this.PropertyNameTextEdit.Size = new System.Drawing.Size(216, 20);
            this.PropertyNameTextEdit.TabIndex = 7;
            // 
            // DeletePropertyButton
            // 
            this.DeletePropertyButton.Location = new System.Drawing.Point(419, 125);
            this.DeletePropertyButton.LookAndFeel.SkinName = "Office 2013";
            this.DeletePropertyButton.LookAndFeel.UseDefaultLookAndFeel = false;
            this.DeletePropertyButton.Name = "DeletePropertyButton";
            this.DeletePropertyButton.Size = new System.Drawing.Size(122, 23);
            this.DeletePropertyButton.TabIndex = 2;
            this.DeletePropertyButton.Text = "Delete property";
            this.DeletePropertyButton.Click += new System.EventHandler(this.DeletePropertyButton_Click);
            // 
            // AddPropertyButton
            // 
            this.AddPropertyButton.Location = new System.Drawing.Point(419, 30);
            this.AddPropertyButton.LookAndFeel.SkinName = "Office 2013";
            this.AddPropertyButton.LookAndFeel.UseDefaultLookAndFeel = false;
            this.AddPropertyButton.Name = "AddPropertyButton";
            this.AddPropertyButton.Size = new System.Drawing.Size(122, 23);
            this.AddPropertyButton.TabIndex = 1;
            this.AddPropertyButton.Text = "Add property";
            this.AddPropertyButton.Click += new System.EventHandler(this.AddPropertyButton_Click);
            // 
            // PropertiesListBoxControl
            // 
            this.PropertiesListBoxControl.Location = new System.Drawing.Point(16, 125);
            this.PropertiesListBoxControl.LookAndFeel.SkinName = "Office 2013";
            this.PropertiesListBoxControl.LookAndFeel.UseDefaultLookAndFeel = false;
            this.PropertiesListBoxControl.Name = "PropertiesListBoxControl";
            this.PropertiesListBoxControl.Size = new System.Drawing.Size(397, 139);
            this.PropertiesListBoxControl.TabIndex = 0;
            // 
            // RegisterDesignerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl1);
            this.Name = "RegisterDesignerView";
            this.Size = new System.Drawing.Size(576, 509);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GeneralGroupControl)).EndInit();
            this.GeneralGroupControl.ResumeLayout(false);
            this.GeneralGroupControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PeriodComboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RegisterNameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RegisterTypeEditor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesPanelControl)).EndInit();
            this.PropertiesPanelControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesGroupControl)).EndInit();
            this.PropertiesGroupControl.ResumeLayout(false);
            this.PropertiesGroupControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PropertyTypeComboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataTypeComboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropertyNameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesListBoxControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.TextEdit RegisterNameTextEdit;
        private DevExpress.XtraEditors.LabelControl labelControlName;
        private DevExpress.XtraEditors.LabelControl PeriodLabelControl;
        private DevExpress.XtraEditors.LabelControl labelControlRegisterType;
        private DevExpress.XtraEditors.ComboBoxEdit RegisterTypeEditor;
        private DevExpress.XtraEditors.GroupControl PropertiesGroupControl;
        private System.Windows.Forms.CheckBox AsynchronousCheckBox;
        private DevExpress.XtraEditors.ListBoxControl PropertiesListBoxControl;
        private DevExpress.XtraEditors.ComboBoxEdit PropertyTypeComboBoxEdit;
        private DevExpress.XtraEditors.LabelControl PropertyTypeLabelControl;
        private DevExpress.XtraEditors.ComboBoxEdit DataTypeComboBoxEdit;
        private DevExpress.XtraEditors.LabelControl DataTypeLabelControl;
        private DevExpress.XtraEditors.LabelControl PropertyNameControl;
        private DevExpress.XtraEditors.TextEdit PropertyNameTextEdit;
        private DevExpress.XtraEditors.SimpleButton DeletePropertyButton;
        private DevExpress.XtraEditors.SimpleButton AddPropertyButton;
        private DevExpress.XtraEditors.SimpleButton CreateButton;
        private DevExpress.XtraEditors.PanelControl PropertiesPanelControl;
        private DevExpress.XtraEditors.ComboBoxEdit PeriodComboBoxEdit;
        private DevExpress.XtraEditors.GroupControl GeneralGroupControl;

    }
}
