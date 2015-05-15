namespace InfinniPlatform.ReportDesigner.Views.Designer
{
	public partial class ReportDesignerView
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
			this.ReportDataEdit = new InfinniPlatform.ReportDesigner.Views.Designer.ReportDataView();
			this.ReportLayoutEdit = new InfinniPlatform.ReportDesigner.Views.Designer.ReportLayoutView();
			this.ExportReportDialog = new System.Windows.Forms.SaveFileDialog();
			this.ImportReportDialog = new System.Windows.Forms.OpenFileDialog();
			this.SuspendLayout();
			// 
			// ReportDataEdit
			// 
			this.ReportDataEdit.Location = new System.Drawing.Point(0, 20);
			this.ReportDataEdit.Name = "ReportDataEdit";
			this.ReportDataEdit.Size = new System.Drawing.Size(247, 184);
			this.ReportDataEdit.TabIndex = 1;
			this.ReportDataEdit.DataSourceCreated += new InfinniPlatform.ReportDesigner.Views.Events.CreatedEventHandler<InfinniPlatform.FastReport.Templates.Data.DataSourceInfo>(this.OnDataSourceCreated);
			this.ReportDataEdit.DataSourceChanged += new InfinniPlatform.ReportDesigner.Views.Events.ChangedEventHandler<InfinniPlatform.FastReport.Templates.Data.DataSourceInfo>(this.OnDataSourceChanged);
			this.ReportDataEdit.DataSourceDeleted += new InfinniPlatform.ReportDesigner.Views.Events.DeletedEventHandler<InfinniPlatform.FastReport.Templates.Data.DataSourceInfo>(this.OnDataSourceDeleted);
			this.ReportDataEdit.ParameterCreated += new InfinniPlatform.ReportDesigner.Views.Events.CreatedEventHandler<InfinniPlatform.FastReport.Templates.Data.ParameterInfo>(this.OnParameterCreated);
			this.ReportDataEdit.ParameterChanged += new InfinniPlatform.ReportDesigner.Views.Events.ChangedEventHandler<InfinniPlatform.FastReport.Templates.Data.ParameterInfo>(this.OnParameterChanged);
			this.ReportDataEdit.ParameterDeleted += new InfinniPlatform.ReportDesigner.Views.Events.DeletedEventHandler<InfinniPlatform.FastReport.Templates.Data.ParameterInfo>(this.OnParameterDeleted);
			this.ReportDataEdit.TotalCreated += new InfinniPlatform.ReportDesigner.Views.Events.CreatedEventHandler<InfinniPlatform.FastReport.Templates.Data.TotalInfo>(this.OnTotalCreated);
			this.ReportDataEdit.TotalChanged += new InfinniPlatform.ReportDesigner.Views.Events.ChangedEventHandler<InfinniPlatform.FastReport.Templates.Data.TotalInfo>(this.OnTotalChanged);
			this.ReportDataEdit.TotalDeleted += new InfinniPlatform.ReportDesigner.Views.Events.DeletedEventHandler<InfinniPlatform.FastReport.Templates.Data.TotalInfo>(this.OnTotalDeleted);
			// 
			// ReportLayoutEdit
			// 
			this.ReportLayoutEdit.DataPanel = this.ReportDataEdit;
			this.ReportLayoutEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ReportLayoutEdit.IsModified = false;
			this.ReportLayoutEdit.Location = new System.Drawing.Point(0, 0);
			this.ReportLayoutEdit.Name = "ReportLayoutEdit";
			this.ReportLayoutEdit.Size = new System.Drawing.Size(884, 562);
			this.ReportLayoutEdit.TabIndex = 0;
			this.ReportLayoutEdit.ReportModified += new System.EventHandler(this.OnReportModified);
			this.ReportLayoutEdit.InvokeImportReport += new System.EventHandler(this.OnInvokeImportReport);
			this.ReportLayoutEdit.InvokeExportReport += new System.EventHandler(this.OnInvokeExportReport);
			this.ReportLayoutEdit.InvokePreviewReport += new System.EventHandler(this.OnInvokePreviewReport);
			this.ReportLayoutEdit.InvokeCreateDataSource += new System.EventHandler(this.OnInvokeCreateDataSource);
			this.ReportLayoutEdit.InvokeCreateParameter += new System.EventHandler(this.OnInvokeCreateParameter);
			this.ReportLayoutEdit.InvokeCreateTotal += new System.EventHandler(this.OnInvokeCreateTotal);
			// 
			// ExportReportDialog
			// 
			this.ExportReportDialog.Filter = "Report|*.ipr|JSON|*.json|Text|*.txt|All|*.*";
			this.ExportReportDialog.Title = "Export to File";
			// 
			// ImportReportDialog
			// 
			this.ImportReportDialog.Filter = "Report|*.ipr|JSON|*.json|Text|*.txt|All|*.*";
			this.ImportReportDialog.Title = "Import from File";
			// 
			// ReportDesignerView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ReportLayoutEdit);
			this.Name = "ReportDesignerView";
			this.Size = new System.Drawing.Size(884, 562);
			this.ResumeLayout(false);

		}

		#endregion

		private ReportLayoutView ReportLayoutEdit;
		private ReportDataView ReportDataEdit;
		private System.Windows.Forms.SaveFileDialog ExportReportDialog;
		private System.Windows.Forms.OpenFileDialog ImportReportDialog;
	}
}

