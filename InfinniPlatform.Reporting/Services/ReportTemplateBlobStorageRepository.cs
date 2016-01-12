using System;

using InfinniPlatform.FastReport.Serialization;
using InfinniPlatform.FastReport.Templates.Reports;
using InfinniPlatform.Sdk.BlobStorage;

namespace InfinniPlatform.Reporting.Services
{
    /// <summary>
    /// Хранилище шаблонов на базе BLOB-хранилища.
    /// </summary>
    internal sealed class ReportTemplateBlobStorageRepository : IReportTemplateRepository
    {
        private const string Report = "Report.json";

        public ReportTemplateBlobStorageRepository(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
            _reportTemplateSerializer = new ReportTemplateSerializer();
        }

        private readonly IBlobStorage _blobStorage;
        private readonly ReportTemplateSerializer _reportTemplateSerializer;

        public ReportTemplate GetReportTemplate(string templateId)
        {
            if (string.IsNullOrEmpty(templateId))
            {
                throw new ArgumentNullException("templateId");
            }

            var reportTemplateData = _blobStorage.GetBlobData(templateId);

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

            _blobStorage.UpdateBlob(templateId, Report, string.Empty, reportTemplateData);
        }
    }
}