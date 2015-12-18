using System.Collections.Generic;

using InfinniPlatform.Api.Reporting;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.FastReport.Templates.Reports;

namespace InfinniPlatform.ReportDesigner.Services
{
    internal sealed class ReportService
    {
        public string Address { get; set; }

        public IDictionary<string, ParameterValues> GetParameterValues(ReportTemplate template)
        {
            return null;
        }

        public byte[] CreateReportFile(ReportTemplate template, IDictionary<string, object> parameterValues = null, ReportFileFormat fileFormat = ReportFileFormat.Pdf)
        {
            return null;
        }
    }
}