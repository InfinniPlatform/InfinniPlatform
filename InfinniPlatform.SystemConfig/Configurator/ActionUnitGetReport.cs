using System;
using System.Collections.Generic;

using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Reporting;
using InfinniPlatform.BlobStorage;
using InfinniPlatform.FastReport.Serialization;
using InfinniPlatform.FastReport.Templates.Reports;
using InfinniPlatform.Reporting.Services;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Configurator
{
    public sealed class ActionUnitGetReport
    {
        private static readonly Dictionary<ReportFileFormat, string> Formats
            = new Dictionary<ReportFileFormat, string>
              {
                  { ReportFileFormat.Pdf, ".pdf" }
              };

        public ActionUnitGetReport(IReportService reportService)
        {
            _reportService = reportService;
        }

        private readonly IReportService _reportService;

        public void Action(IUrlEncodedDataContext target)
        {
            ReportTemplate reportTemplate = LoadReportTemplate(target.FormData.Configuration, target.FormData.Template);

            var dict = new Dictionary<string, object>();
            var parameters = target.FormData.Parameters;

            foreach (var parameter in parameters)
            {
                dict.Add(parameter.Name, parameter.Value);
            }

            var data = _reportService.CreateReportFile(reportTemplate, dict, (ReportFileFormat)target.FormData.FileFormat);

            var fileExtensionTypeProvider = new FileExtensionTypeProvider();

            target.Result = new DynamicWrapper();
            target.Result.Data = data;
            target.Result.Info = new DynamicWrapper();
            target.Result.Info.Size = data.Length;
            target.Result.Info.Type = fileExtensionTypeProvider.GetBlobType(Formats[(ReportFileFormat)target.FormData.FileFormat]);
        }

        private static ReportTemplate LoadReportTemplate(string configuration, string report)
        {
            dynamic reportMetadata = new ManagerFactoryConfiguration(configuration).BuildReportMetadataReader().GetItem(report);
            return ReportTemplateSerializer.Instance.Deserialize(Convert.FromBase64String(reportMetadata.Content));
        }
    }
}