using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.DataApi;

namespace InfinniPlatform.SystemConfig.Configurator
{
    /// <summary>
    /// Точка расширения для импорта данных, относящихся к метаданным определенной конфигурации
    /// </summary>
    public sealed class ActionUnitImportDataFromJson
    {
        public void Action(IApplyContext target)
        {
            var documentProvider = new DocumentApi();

            string configuration = target.Item.Configuration;
            string metadata = target.Item.Metadata;

            byte[] zipFileContent = Convert.FromBase64String(target.Item.FileContent);

            using (var archive = new ZipArchive(new MemoryStream(zipFileContent)))
            {
                foreach (var entry in archive.Entries)
                {
                    // Проходим по всем файлам в архиве -
                    // каждый файл содержит определенный набор документов,
                    // которые необходимо добавить в индекс 

                    var entryContent = entry.Open();

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
