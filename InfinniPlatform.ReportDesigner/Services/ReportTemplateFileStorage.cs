using System.IO;
using InfinniPlatform.FastReport.Serialization;
using InfinniPlatform.FastReport.Templates.Reports;

namespace InfinniPlatform.ReportDesigner.Services
{
    internal sealed class ReportTemplateFileStorage
    {
        public ReportTemplate Load(string fileName)
        {
            var data = File.ReadAllBytes(fileName);

            return ReportTemplateSerializer.Instance.Deserialize(data);
        }

        public void Save(string fileName, ReportTemplate template)
        {
            var data = ReportTemplateSerializer.Instance.Serialize(template);

            using (var writer = File.OpenWrite(fileName))
            {
                writer.Write(data, 0, data.Length);
                writer.Flush();
            }
        }
    }
}