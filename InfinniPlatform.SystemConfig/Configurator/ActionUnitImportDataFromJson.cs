using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Configurator
{
    /// <summary>
    ///     Точка расширения для импорта данных, относящихся к метаданным определенной конфигурации
    /// </summary>
    public sealed class ActionUnitImportDataFromJson
    {
        public void Action(IApplyContext target)
        {
            var documentProvider = target.Context.GetComponent<DocumentApi>();

            string configuration = target.Item.Configuration;
            string metadata = target.Item.Metadata;

            byte[] zipFileContent = Convert.FromBase64String(target.Item.FileContent);

            using (var archive = new ZipArchive(new MemoryStream(zipFileContent)))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    // Проходим по всем файлам в архиве -
                    // каждый файл содержит определенный набор документов,
                    // которые необходимо добавить в индекс 

                    Stream entryContent = entry.Open();

                    var buffer = new byte[entry.Length];

                    entryContent.Read(buffer, 0, (int) entry.Length);

                    documentProvider.SetDocuments(
                        configuration,
                        metadata,
                        Encoding.UTF8.GetString(buffer).ToDynamicList(), 1000);
                }
            }
        }
    }
}