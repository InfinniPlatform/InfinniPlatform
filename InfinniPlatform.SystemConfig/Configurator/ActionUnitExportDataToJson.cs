using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.Configurator
{
    /// <summary>
    ///     Точка расширения для сохранения данных, относящихся к метаданным определенной конфигурации
    /// </summary>
    public sealed class ActionUnitExportDataToJson
    {
        public void Action(IApplyContext target)
        {
            string configuration = target.Item.Configuration;
            string metadata = target.Item.Metadata;
            string pathToZip = target.Item.PathToZip;

            if (string.IsNullOrEmpty(configuration))
            {
                throw new ArgumentException("Configuration name should be specified via 'configuration' property");
            }

            if (string.IsNullOrEmpty(metadata))
            {
                throw new ArgumentException("Metadata should be specified via 'metadata' property");
            }

            if (string.IsNullOrEmpty(pathToZip))
            {
                throw new ArgumentException("Path to archive should be specified via 'PathToZip' property");
            }

            var documentProvider = target.Context.GetComponent<DocumentApi>(target.Version);

            int page = 0;

            bool hasDocuments = false;

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    while (true)
                    {
                        var documents = documentProvider.GetDocument(configuration, metadata, null, page, 1000);

                        dynamic[] documentsAsArray = documents as dynamic[] ?? documents.ToArray();
                        if (documents == null || !documentsAsArray.Any())
                        {
                            break;
                        }

                        hasDocuments = true;

                        ZipArchiveEntry archiveFile = archive.CreateEntry(string.Format("part_{0}.json", page));

                        using (Stream entryStream = archiveFile.Open())
                        using (var streamWriter = new StreamWriter(entryStream))
                        {
                            streamWriter.Write('[');

                            for (int index = 0; index < documentsAsArray.Length; index++)
                            {
                                dynamic document = documentsAsArray[index];
                                streamWriter.Write(document.ToString());

                                if (index != (documentsAsArray.Length - 1))
                                {
                                    streamWriter.Write(',');
                                }
                            }


                            streamWriter.Write(']');
                        }

                        page++;
                    }
                }

                string archiveName = Path.Combine(pathToZip, string.Format("{0}_{1}.zip", configuration, metadata));

                if (File.Exists(archiveName))
                {
                    File.Delete(archiveName);
                }

                if (hasDocuments)
                {
                    using (var fileStream = new FileStream(
                        archiveName,
                        FileMode.Create))
                    {
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        memoryStream.CopyTo(fileStream);
                    }
                }
            }
        }
    }
}