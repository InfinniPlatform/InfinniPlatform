namespace InfinniPlatform.MetadataDesigner.Views.Validation.Controls
{
    partial class ParameterEditControl
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
			this.parameterValueEdit = new DevExpress.XtraEditors.TextEdit();
			this.parameterLabel = new DevExpress.XtraEditors.LabelControl();
			this.dontUseParameter = new DevExpress.XtraEditors.CheckEdit();
			this.CommaPromptLabel = new DevExpress.XtraEditors.LabelControl();
			this.DataTypeLabel = new DevExpress.XtraEditors.LabelControl();
			this.DataTypeComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
			this.MessageMemoEdit = new DevExpress.XtraEditors.MemoEdit();
			this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
			((System.ComponentModel.ISupportInitialize)(this.parameterValueEdit.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dontUseParameter.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.DataTypeComboBox.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.MessageMemoEdit.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.panelControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// parameterValueEdit
			// 
			this.parameterValueEdit.Location = new System.Drawing.Point(131, 2);
			this.parameterValueEdit.Name = "parameterValueEdit";
			this.parameterValueEdit.Size = new System.Drawing.Size(99, 20);
			this.parameterValueEdit.TabIndex = 0;
			// 
			// parameterLabel
			// 
			this.parameterLabel.Location = new System.Drawing.Point(4, 6);
			this.parameterLabel.Name = "parameterLabel";
			this.parameterLabel.Size = new System.Drawing.Size(0, 13);
			this.parameterLabel.TabIndex = 1;
			// 
			// dontUseParameter
			// 
			this.dontUseParameter.Location = new System.Drawing.Point(236, 5);
			this.dontUseParameter.Name = "dontUseParameter";
			this.dontUseParameter.Properties.Caption = "Don\'t use";
			this.dontUseParameter.Size = new System.Drawing.Size(112, 19);
			this.dontUseParameter.TabIndex = 2;
			this.dontUseParameter.CheckedChanged += new System.EventHandler(this.dontUseParameter_CheckedChanged);
			// 
			// CommaPromptLabel
			// 
			this.CommaPromptLabel.Location = new System.Drawing.Point(234, 5);
			this.CommaPromptLabel.Name = "CommaPromptLabel";
			this.CommaPromptLabel.Size = new System.Drawing.Size(100, 13);
			this.CommaPromptLabel.TabIndex = 3;
			this.CommaPromptLabel.Text = "Use \',\' for separation";
			// 
			// DataTypeLabel
			// 
			this.DataTypeLabel.Location = new System.Drawing.Point(131, 29);
			this.DataTypeLabel.Name = "DataTypeLabel";
			this.DataTypeLabel.Size = new System.Drawing.Size(22, 13);
			this.DataTypeLabel.TabIndex = 4;
			this.DataTypeLabel.Text = "Тип:";
			// 
			// DataTypeComboBox
			// 
			this.DataTypeComboBox.EditValue = "String";
			this.DataTypeComboBox.Location = new System.Drawing.Point(159, 26);
			this.DataTypeComboBox.Name = "DataTypeComboBox";
			this.DataTypeComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.DataTypeComboBox.Properties.Items.AddRange(new object[] {
            "String",
            "Integer",
            "Bool",
            "Float",
            "DateTime"});
			this.DataTypeComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
			this.DataTypeComboBox.Size = new System.Drawing.Size(71, 20);
			this.DataTypeComboBox.TabIndex = 5;
			// 
			// MessageMemoEdit
			// 
			this.MessageMemoEdit.Location = new System.Drawing.Point(131, 4);
			this.MessageMemoEdit.Name = "MessageMemoEdit";
			this.MessageMemoEdit.Size = new System.Drawing.Size(257, 55);
			this.MessageMemoEdit.TabIndex = 6;
			this.MessageMemoEdit.UseOptimizedRendering = true;
			this.MessageMemoEdit.Visible = false;
			// 
			// panelControl1
			// 
			this.panelControl1.Appearance.BackColor = System.Drawing.Color.White;
			this.panelControl1.Appearance.Options.UseBackColor = true;
			this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
			this.panelControl1.Controls.Add(this.DataTypeComboBox);
			this.panelControl1.Controls.Add(this.dontUseParameter);
			this.panelControl1.Controls.Add(this.parameterValueEdit);
			this.panelControl1.Controls.Add(this.CommaPromptLabel);
			this.panelControl1.Controls.Add(this.MessageMemoEdit);
			this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelControl1.Location = new System.Drawing.Point(0, 0);
			this.panelControl1.LookAndFeel.SkinName = "Office 2013";
			this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(400, 61);
			this.panelControl1.TabIndex = 7;
			// 
			// ParameterEditControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.DataTypeLabel);
			this.Controls.Add(this.parameterLabel);
			this.Controls.Add(this.panelControl1);
			this.Name = "ParameterEditControl";
			this.Size = new System.Drawing.Size(400, 61);
			((System.ComponentModel.ISupportInitialize)(this.parameterValueEdit.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dontUseParameter.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.DataTypeComboBox.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.MessageMemoEdit.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.panelControl1.ResumeLayout(false);
			this.panelControl1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit parameterValueEdit;
        private DevExpress.XtraEditors.LabelControl parameterLabel;
        private DevExpress.XtraEditors.CheckEdit dontUseParameter;
        private DevExpress.XtraEditors.LabelControl CommaPromptLabel;
        private DevExpress.XtraEditors.LabelControl DataTypeLabel;
        private DevExpress.XtraEditors.ComboBoxEdit DataTypeComboBox;
        private DevExpress.XtraEditors.MemoEdit MessageMemoEdit;
        private DevExpress.XtraEditors.PanelControl panelControl1;
    }
}
