using System;
using InfinniPlatform.BlobStorage;
using InfinniPlatform.Factories;
using InfinniPlatform.FastReport.Serialization;
using InfinniPlatform.FastReport.Templates.Reports;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Binary;

namespace InfinniPlatform.Reporting.Services
{
	/// <summary>
	/// Хранилище шаблонов на базе BLOB-хранилища.
	/// </summary>
	sealed class ReportTemplateBlobStorageRepository : IReportTemplateRepository
	{
		public ReportTemplateBlobStorageRepository(IBlobStorageFactory blobStorageFactory)
		{
			if (blobStorageFactory == null)
			{
				throw new ArgumentNullException("blobStorageFactory");
			}

			_blobStorage = blobStorageFactory.CreateBlobStorage();
			_reportTemplateSerializer = new ReportTemplateSerializer();
		}


		private const string Report = "Report.json";
		private readonly IBlobStorage _blobStorage;
		private readonly ReportTemplateSerializer _reportTemplateSerializer;


		public ReportTemplate GetReportTemplate(string templateId)
		{
			if (string.IsNullOrEmpty(templateId))
			{
				throw new ArgumentNullException("templateId");
			}

			var reportTemplateData = _blobStorage.GetBlobData(new Guid(templateId));

			return _reportTemplateSerializer.Deserialize(reportTemplateData.Data);
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

			_blobStorage.SaveBlob(new Guid(templateId), Report, reportTemplateData);
		}
	}
}