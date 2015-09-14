using System;
using System.Windows.Forms;
using InfinniPlatform.Api.Reporting;
using InfinniPlatform.FastReport.Templates.Reports;
using InfinniPlatform.ReportDesigner.Properties;
using InfinniPlatform.ReportDesigner.Services;
using InfinniPlatform.ReportDesigner.Settings;

namespace InfinniPlatform.ReportDesigner.Views.Preview
{
    /// <summary>
    ///     Представление для предварительного просмотра отчета.
    /// </summary>
    sealed partial class PreviewReportView : Form
    {
        private readonly ReportService _reportService = new ReportService();

        public PreviewReportView()
        {
            InitializeComponent();
            SetServiceAddresses();

            FileFormatEdit.Items = Enum.GetValues(typeof (ReportFileFormat));
            FileFormatEdit.SelectedItem = ReportFileFormat.Pdf;
        }

        /// <summary>
        ///     Шаблон отчета.
        /// </summary>
        public ReportTemplate ReportTemplate { get; set; }

        // EVENTS

        private void OnLoad(object sender, EventArgs e)
        {
            RebuildParameterList();
            ResetParameterValues();

            PreviewBtn.Enabled = (ReportTemplate != null);
            RefreshBtn.Enabled = (ReportTemplate != null);
        }

        private void OnRefresh(object sender, EventArgs e)
        {
            ResetParameterValues();
        }

        private void OnPreview(object sender, EventArgs e)
        {
            if (ReportTemplate != null && ParameterList.ValidateChildren())
            {
                var parameterValues = ParameterList.GetSelectedParameterValues();
                var fileFormat = (ReportFileFormat) FileFormatEdit.SelectedItem;

                InvokeService(() => _reportService.CreateReportFile(ReportTemplate, parameterValues, fileFormat));
            }
        }

        private void OnCancel(object sender, EventArgs e)
        {
            Close();
        }

        // HELPERS

        private void RebuildParameterList()
        {
            SuspendLayout();

            try
            {
                ParameterList.Parameters = (ReportTemplate != null) ? ReportTemplate.Parameters : null;
            }
            finally
            {
                ResumeLayout();
            }
        }

        private void ResetParameterValues()
        {
            if (ReportTemplate != null)
            {
                InvokeService(() => _reportService.GetParameterValues(ReportTemplate),
                    parameterValues => ParameterList.SetDefaultParameterValues(parameterValues),
                    () => ParameterList.SetDefaultParameterValues(null));
            }
            else
            {
                ParameterList.SetDefaultParameterValues(null);
            }
        }

        private void InvokeService<TResult>(Func<TResult> work, Action<TResult> success = null, Action error = null)
        {
            var address = ServiceUrlEdit.SelectedValue;

            if (string.IsNullOrWhiteSpace(address))
            {
                Resources.ServiceUrlCannotBeNullOrWhiteSpace.ShowError();
                ServiceUrlEdit.Focus();
            }
            else
            {
                _reportService.Address = address;

                MainPanel.AsyncAction(work, success, error);
            }
        }

        private void SetServiceAddresses()
        {
            if (DesignMode == false && DesignerConfigSection.Instance != null)
            {
                foreach (ValueConfigElement reportService in DesignerConfigSection.Instance.ReportServices)
                {
                    ServiceUrlEdit.AddItem(reportService.Key, reportService.Value);
                }
            }
        }
    }
}