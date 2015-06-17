using System;
using System.Collections.Generic;

using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Reporting;
using InfinniPlatform.BlobStorage;
using InfinniPlatform.FastReport.Serialization;
using InfinniPlatform.FastReport.Templates.Reports;
using InfinniPlatform.Reporting.Services;

namespace InfinniPlatform.SystemConfig.Configurator
{
	public sealed class ActionUnitGetReport
	{
		//TODO Добавить остальные форматы
		private static readonly Dictionary<ReportFileFormat, string> Formats
			= new Dictionary<ReportFileFormat, string>
			  {
				  { ReportFileFormat.Pdf, ".pdf" },
			  };

		public void Action(IUrlEncodedDataContext target)
		{
			// необходимо избавиться от ссылок на FastReport сборку
			ReportTemplate reportTemplate = LoadReportTemplate(target.FormData.Version, target.FormData.Configuration, target.FormData.Template);

			var dict = new Dictionary<string, object>();
			var parameters = target.FormData.Parameters;

			foreach (var parameter in parameters)
			{
				dict.Add(parameter.Name, parameter.Value);
			}

			byte[] data = new ReportServiceFactory().CreateReportService().CreateReportFile(reportTemplate, dict, (ReportFileFormat)target.FormData.FileFormat);

			var fileExtensionTypeProvider = new FileExtensionTypeProvider();

			target.Result = new DynamicWrapper();
			target.Result.Data = data;
			target.Result.Info = new DynamicWrapper();
			target.Result.Info.Size = data.Length;
			target.Result.Info.Type = fileExtensionTypeProvider.GetBlobType(Formats[(ReportFileFormat)target.FormData.FileFormat]);
		}

		private static ReportTemplate LoadReportTemplate(string version, string configuration, string report)
		{
			var reportMetadata = new ManagerFactoryConfiguration(version, configuration).BuildReportMetadataReader().GetItem(report);
			return ReportTemplateSerializer.Instance.Deserialize(Convert.FromBase64String(reportMetadata.Content));
		}
	}
}