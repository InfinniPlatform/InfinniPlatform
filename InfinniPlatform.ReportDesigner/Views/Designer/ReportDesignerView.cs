using System;
using System.Windows.Forms;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.FastReport.Templates.Reports;
using InfinniPlatform.ReportDesigner.Services;
using InfinniPlatform.ReportDesigner.Views.Events;
using InfinniPlatform.ReportDesigner.Views.Preview;

namespace InfinniPlatform.ReportDesigner.Views.Designer
{
    /// <summary>
    ///     Представление редактора отчета.
    /// </summary>
    public sealed partial class ReportDesignerView : UserControl
    {
        private ReportTemplate _editValue;
        private bool _initializing;
        private readonly PreviewReportView _previewDialog = new PreviewReportView();
        private readonly ReportTemplateConverter _templateConverter = new ReportTemplateConverter();
        private readonly ReportTemplateFileStorage _templateStorage = new ReportTemplateFileStorage();

        public ReportDesignerView()
        {
            InitializeComponent();

            ReportDataEdit.GetDataBandsFunc = ReportLayoutEdit.GetDataBands;
            ReportDataEdit.GetPrintBandsFunc = ReportLayoutEdit.GetPrintBands;
        }

        /// <summary>
        ///     Редактируемый отчет.
        /// </summary>
        public ReportTemplate EditValue
        {
            get { return GetReportTemplate(); }
            set
            {
                if (!Equals(_editValue, value))
                {
                    SetReportTemplate(value);
                }
            }
        }

        /// <summary>
        ///     Событие изменения отчета.
        /// </summary>
        public event EventHandler EditValueChanged;

        private ReportTemplate GetReportTemplate()
        {
            if (ReportLayoutEdit.IsModified)
            {
                var template = _templateConverter.ConvertToTemplate(ReportLayoutEdit.Report);
                template.DataSources = ReportDataEdit.DataSources;
                template.Parameters = ReportDataEdit.Parameters;
                template.Totals = ReportDataEdit.Totals;

                _editValue = template;
            }

            return _editValue;
        }

        private void SetReportTemplate(ReportTemplate template)
        {
            _editValue = template;

            var report = _templateConverter.ConvertFromTemplate(template);

            InitializeEditValue(() =>
            {
                ReportLayoutEdit.Report = report;
                ReportDataEdit.Report = report;
                ReportDataEdit.DataSources = template.DataSources;
                ReportDataEdit.Parameters = template.Parameters;
                ReportDataEdit.Totals = template.Totals;
                ReportDataEdit.AllowEdit = true;

                ReportLayoutEdit.IsModified = false;
            });
        }

        private void OnReportModified(object sender, EventArgs e)
        {
            InitializeEditValue(() =>
            {
                var handler = EditValueChanged;

                if (handler != null)
                {
                    handler(sender, e);
                }
            });
        }

        private void InitializeEditValue(Action action)
        {
            if (!_initializing)
            {
                _initializing = true;

                try
                {
                    action();
                }
                finally
                {
                    _initializing = false;
                }
            }
        }

        // Report / Import
        private void OnInvokeImportReport(object sender, EventArgs e)
        {
            if (ImportReportDialog.ShowDialog(this) == DialogResult.OK)
            {
                var fileName = ImportReportDialog.FileName;

                TryExecute(() =>
                {
                    EditValue = _templateStorage.Load(fileName);

                    ReportLayoutEdit.IsModified = true;

                    OnReportModified(sender, e);
                });
            }
        }

        // Report / Export
        private void OnInvokeExportReport(object sender, EventArgs e)
        {
            if (ExportReportDialog.ShowDialog(this) == DialogResult.OK)
            {
                var fileName = ExportReportDialog.FileName;

                TryExecute(() => _templateStorage.Save(fileName, EditValue));
            }
        }

        // Report / Preview
        private void OnInvokePreviewReport(object sender, EventArgs e)
        {
            TryExecute(() =>
            {
                if (ReportDataEdit.ValidateChildren())
                {
                    _previewDialog.ReportTemplate = EditValue;
                    _previewDialog.ShowDialog(this);
                }
            });
        }

        // DATA SOURCES

        private void OnInvokeCreateDataSource(object sender, EventArgs e)
        {
            ReportDataEdit.CreateDataSource();
        }

        private void OnDataSourceCreated(object sender, ValueEventArgs<DataSourceInfo> e)
        {
            ReportLayoutEdit.CreateDataSource(e.Value);
        }

        private void OnDataSourceChanged(object sender, ChangedEventArgs<DataSourceInfo> e)
        {
            ReportLayoutEdit.ChangeDataSource(e.OldValue, e.NewValue);
        }

        private void OnDataSourceDeleted(object sender, ValueEventArgs<DataSourceInfo> e)
        {
            ReportLayoutEdit.DeleteDataSource(e.Value);
        }

        // PARAMETERS

        private void OnInvokeCreateParameter(object sender, EventArgs e)
        {
            ReportDataEdit.CreateParameter();
        }

        private void OnParameterCreated(object sender, ValueEventArgs<ParameterInfo> e)
        {
            ReportLayoutEdit.CreateParameter(e.Value);
        }

        private void OnParameterChanged(object sender, ChangedEventArgs<ParameterInfo> e)
        {
            ReportLayoutEdit.ChangeParameter(e.OldValue, e.NewValue);
        }

        private void OnParameterDeleted(object sender, ValueEventArgs<ParameterInfo> e)
        {
            ReportLayoutEdit.DeleteParameter(e.Value);
        }

        // TOTALS

        private void OnInvokeCreateTotal(object sender, EventArgs e)
        {
            ReportDataEdit.CreateTotal();
        }

        private void OnTotalCreated(object sender, ValueEventArgs<TotalInfo> e)
        {
            ReportLayoutEdit.CreateTotal(e.Value);
        }

        private void OnTotalChanged(object sender, ChangedEventArgs<TotalInfo> e)
        {
            ReportLayoutEdit.ChangeTotal(e.OldValue, e.NewValue);
        }

        private void OnTotalDeleted(object sender, ValueEventArgs<TotalInfo> e)
        {
            ReportLayoutEdit.DeleteTotal(e.Value);
        }

        // HELPERS

        private static void TryExecute(Action action)
        {
            try
            {
                action();
            }
            catch (Exception error)
            {
                error.Message.ShowError();
            }
        }
    }
}