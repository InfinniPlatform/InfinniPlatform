namespace InfinniPlatform.MetadataDesigner.Views.Validation.Controls
{
    partial class CollectionPredicateBuilderControl
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
            this.parametersPanel = new DevExpress.XtraEditors.PanelControl();
            this.operationComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.operationLabel = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.parametersPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.operationComboBox.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // parametersPanel
            // 
            this.parametersPanel.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.parametersPanel.Appearance.Options.UseBackColor = true;
            this.parametersPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.parametersPanel.Location = new System.Drawing.Point(13, 40);
            this.parametersPanel.LookAndFeel.SkinName = "Office 2013";
            this.parametersPanel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.parametersPanel.Name = "parametersPanel";
            this.parametersPanel.Size = new System.Drawing.Size(401, 210);
            this.parametersPanel.TabIndex = 5;
            // 
            // operationComboBox
            // 
            this.operationComboBox.Location = new System.Drawing.Point(145, 12);
            this.operationComboBox.Name = "operationComboBox";
            this.operationComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.operationComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.operationComboBox.Size = new System.Drawing.Size(150, 20);
            this.operationComboBox.TabIndex = 4;
            this.operationComboBox.SelectedIndexChanged += new System.EventHandler(this.operationComboBox_SelectedIndexChanged);
            // 
            // operationLabel
            // 
            this.operationLabel.Location = new System.Drawing.Point(15, 15);
            this.operationLabel.Name = "operationLabel";
            this.operationLabel.Size = new System.Drawing.Size(82, 13);
            this.operationLabel.TabIndex = 3;
            this.operationLabel.Text = "Select operation:";
            // 
            // CollectionPredicateBuilderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.parametersPanel);
            this.Controls.Add(this.operationComboBox);
            this.Controls.Add(this.operationLabel);
            this.Name = "CollectionPredicateBuilderControl";
            this.Size = new System.Drawing.Size(464, 267);
            ((System.ComponentModel.ISupportInitialize)(this.parametersPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.operationComboBox.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl parametersPanel;
        private DevExpress.XtraEditors.ComboBoxEdit operationComboBox;
        private DevExpress.XtraEditors.LabelControl operationLabel;
    }
}
