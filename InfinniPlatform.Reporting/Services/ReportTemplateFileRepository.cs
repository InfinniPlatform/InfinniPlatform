using System;
using System.IO;

using InfinniPlatform.FastReport.Serialization;
using InfinniPlatform.FastReport.Templates.Reports;

namespace InfinniPlatform.Reporting.Services
{
	/// <summary>
	/// Хранилище шаблонов отчетов на базе файловой системы.
	/// </summary>
	sealed class ReportTemplateFileRepository : IReportTemplateRepository
	{
		public ReportTemplateFileRepository(string reportBaseDirectory = "")
		{
			_reportBaseDirectory = reportBaseDirectory;
			_reportTemplateSerializer = new ReportTemplateSerializer();
		}


		private readonly string _reportBaseDirectory;
		private readonly ReportTemplateSerializer _reportTemplateSerializer;


		public ReportTemplate GetReportTemplate(string templateId)
		{
			if (string.IsNullOrEmpty(templateId))
			{
				throw new ArgumentNullException("templateId");
			}

			var reportTemplateData = File.ReadAllBytes(Path.Combine(_reportBaseDirectory, templateId));

			return _reportTemplateSerializer.Deserialize(reportTemplateData);
		}

		public void SaveReportTemplate(string templateId, ReportTemplate template)
		{
			if (string.IsNullOrEmpty(templateId))
			{
				throw new ArgumentNullException("templateId");
			}

			if (template == null)
			{
				throw new ArgumentNullException("template");
			}

			var reportTemplateData = _reportTemplateSerializer.Serialize(template);

			File.WriteAllBytes(Path.Combine(_reportBaseDirectory, templateId), reportTemplateData);
		}
	}
}