using InfinniPlatform.DesignControls.Controls.LayoutPanels.GridPanels;

namespace InfinniPlatform.DesignControls.Controls.DataElements
{
	partial class FilterPanelElement
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterPanelElement));
			this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
			this.FilterButton = new DevExpress.XtraEditors.SimpleButton();
			this.GridPanelAdditional = new InfinniPlatform.DesignControls.Controls.LayoutPanels.GridPanels.GridPanel();
			this.LabelAdditional = new InfinniPlatform.DesignControls.Controls.DataElements.LabelElement();
			this.GridPanelGeneral = new InfinniPlatform.DesignControls.Controls.LayoutPanels.GridPanels.GridPanel();
			this.LabelGeneral = new InfinniPlatform.DesignControls.Controls.DataElements.LabelElement();
			((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
			this.panelControl2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelControl2
			// 
			this.panelControl2.Appearance.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.panelControl2.Appearance.Options.UseBackColor = true;
			this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
			this.panelControl2.Controls.Add(this.FilterButton);
			this.panelControl2.Dock = System.Windows.Forms.DockStyle.Left;
			this.panelControl2.Location = new System.Drawing.Point(0, 0);
			this.panelControl2.Name = "panelControl2";
			this.panelControl2.Size = new System.Drawing.Size(106, 116);
			this.panelControl2.TabIndex = 1;
			// 
			// FilterButton
			// 
			this.FilterButton.Image = ((System.Drawing.Image)(resources.GetObject("FilterButton.Image")));
			this.FilterButton.Location = new System.Drawing.Point(5, 5);
			this.FilterButton.LookAndFeel.SkinName = "Office 2013";
			this.FilterButton.LookAndFeel.UseDefaultLookAndFeel = false;
			this.FilterButton.Name = "FilterButton";
			this.FilterButton.Size = new System.Drawing.Size(86, 23);
			this.FilterButton.TabIndex = 1;
			this.FilterButton.Text = "Set Filter";
			// 
			// GridPanelAdditional
			// 
			this.GridPanelAdditional.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.GridPanelAdditional.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.GridPanelAdditional.Dock = System.Windows.Forms.DockStyle.Top;
			this.GridPanelAdditional.Location = new System.Drawing.Point(106, 81);
			this.GridPanelAdditional.Margin = new System.Windows.Forms.Padding(0);
			this.GridPanelAdditional.Name = "GridPanelAdditional";
			this.GridPanelAdditional.ObjectInspector = null;
			this.GridPanelAdditional.Size = new System.Drawing.Size(462, 37);
			this.GridPanelAdditional.TabIndex = 5;
			// 
			// LabelAdditional
			// 
			this.LabelAdditional.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.LabelAdditional.Dock = System.Windows.Forms.DockStyle.Top;
			this.LabelAdditional.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.LabelAdditional.LabelText = "Additional Properties";
			this.LabelAdditional.Location = new System.Drawing.Point(106, 60);
			this.LabelAdditional.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.LabelAdditional.Name = "LabelAdditional";
			this.LabelAdditional.ObjectInspector = null;
			this.LabelAdditional.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
			this.LabelAdditional.Size = new System.Drawing.Size(462, 21);
			this.LabelAdditional.TabIndex = 4;
			// 
			// GridPanelGeneral
			// 
			this.GridPanelGeneral.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.GridPanelGeneral.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.GridPanelGeneral.Dock = System.Windows.Forms.DockStyle.Top;
			this.GridPanelGeneral.Location = new System.Drawing.Point(106, 21);
			this.GridPanelGeneral.Margin = new System.Windows.Forms.Padding(0);
			this.GridPanelGeneral.Name = "GridPanelGeneral";
			this.GridPanelGeneral.ObjectInspector = null;
			this.GridPanelGeneral.Size = new System.Drawing.Size(462, 39);
			this.GridPanelGeneral.TabIndex = 3;
			// 
			// LabelGeneral
			// 
			this.LabelGeneral.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.LabelGeneral.Dock = System.Windows.Forms.DockStyle.Top;
			this.LabelGeneral.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.LabelGeneral.LabelText = "General Properties";
			this.LabelGeneral.Location = new System.Drawing.Point(106, 0);
			this.LabelGeneral.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.LabelGeneral.Name = "LabelGeneral";
			this.LabelGeneral.ObjectInspector = null;
			this.LabelGeneral.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
			this.LabelGeneral.Size = new System.Drawing.Size(462, 21);
			this.LabelGeneral.TabIndex = 2;
			// 
			// FilterPanelElement
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.GridPanelAdditional);
			this.Controls.Add(this.LabelAdditional);
			this.Controls.Add(this.GridPanelGeneral);
			this.Controls.Add(this.LabelGeneral);
			this.Controls.Add(this.panelControl2);
			this.Name = "FilterPanelElement";
			this.Size = new System.Drawing.Size(568, 116);
			((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
			this.panelControl2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.PanelControl panelControl2;
		private DevExpress.XtraEditors.SimpleButton FilterButton;
		private LabelElement LabelGeneral;
		private GridPanel GridPanelGeneral;
		private LabelElement LabelAdditional;
		private GridPanel GridPanelAdditional;

	}
}
