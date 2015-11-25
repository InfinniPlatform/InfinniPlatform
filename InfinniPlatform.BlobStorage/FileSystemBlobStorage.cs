using System;
using System.IO;

using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Logging;
using InfinniPlatform.Sdk.Environment.Binary;
using InfinniPlatform.Sdk.Environment.Log;

namespace InfinniPlatform.BlobStorage
{
    /// <summary>
    /// Реализует сервис для работы хранилищем BLOB (Binary Large OBject) на основе файловой системы.
    /// </summary>
    /// <returns>
    /// Достаточно простая реализация. Хранилище представляет собой каталог с файлами. Для обеспечения
    /// быстрого доступа к файлам - log(N) - каталог организован в виден набора вложенных друг в друга
    /// подкаталогов. Нулевой уровень - корневая папка хранилища, первый уровень - первые 2 символа
    /// идентификатора BLOB, второй уровень - вторые 2 символа идентификатора BLOB, третий уровень -
    /// оставшиеся символы идентификатора BLOB. В каталоге третьего уровня содержат два файла: info
    /// и data. В файле info хранится мета-информация о BLOB, представленная в виде JSON-объекта
    /// (идентификатор, наименование, MIME-тип, размер, дата изменения и т.п.). Наличие файла info
    /// в текущей реализации не обязательно. В файле data хранятся данные BLOB. Наличие файла data
    /// обязательно.
    /// 
    /// Выбор в пользу использования обычной файловой системы был сделан не случайно. Во-первых,
    /// это самый простой и гибкий способ. Во-вторых, некоторые распределенные файловые системы
    /// имеют FUSE (Filesystem in Userspace) адаптеры, поддерживающие POSIX-стандарт, что дает
    /// возможность использовать функции обычной файловой системы при работе, не задумываясь о
    /// том, что на самом деле работа идет с распределенным хранилищем. В-третьих, пока трудно
    /// судить о том, какое распределенное хранилище (из тех, которые не имеют FUSE) подойдет
    /// лучше других.
    /// </returns>
    public sealed class FileSystemBlobStorage : IBlobStorage
    {
        private const string LogComponentName = "FileSystemBlobStorage";


        public FileSystemBlobStorage(string baseDirectory, IPerformanceLog performanceLog)
        {
            _baseDirectory = baseDirectory;
            _performanceLog = performanceLog;

            // TODO: Refactor
            // Получать эти зависимости через конструктор
            _serializer = JsonObjectSerializer.Default;
            _typeProvider = FileExtensionTypeProvider.Default;
        }


        private readonly string _baseDirectory;
        private readonly IPerformanceLog _performanceLog;
        private readonly IObjectSerializer _serializer;
        private readonly FileExtensionTypeProvider _typeProvider;


        public BlobInfo GetBlobInfo(string blobId)
        {
            if (string.IsNullOrEmpty(blobId))
            {
                throw new ArgumentNullException(nameof(blobId));
            }

            var start = DateTime.Now;

            blobId = NormalizeBlobId(blobId);

            var result = ReadBlobInfo(blobId);

            _performanceLog.Log(LogComponentName, "GetBlobInfo", start, null);

            return result;
        }

        public BlobData GetBlobData(string blobId)
        {
            if (string.IsNullOrEmpty(blobId))
            {
                throw new ArgumentNullException(nameof(blobId));
            }

            var start = DateTime.Now;

            blobId = NormalizeBlobId(blobId);

            var blobInfo = ReadBlobInfo(blobId);
            var blobData = ReadBlobData(blobId);

            var result = (blobInfo != null || blobData != null)
                ? new BlobData
                  {
                      Info = blobInfo,
                      Data = blobData
                  }
                : null;

            _performanceLog.Log(LogComponentName, "GetBlobData", start, null);

            return result;
        }

        public string CreateBlob(string blobName, string blobType, byte[] blobData)
        {
            var blobId = GenerateBlobId();

            UpdateBlob(blobId, blobName, blobType, blobData);

            return blobId;
        }

        public void UpdateBlob(string blobId, string blobName, string blobType, byte[] blobData)
        {
            if (string.IsNullOrEmpty(blobId))
            {
                throw new ArgumentNullException(nameof(blobId));
            }

            if (blobData == null)
            {
                throw new ArgumentNullException(nameof(blobData));
            }

            var start = DateTime.Now;

            blobId = NormalizeBlobId(blobId);

            if (string.IsNullOrEmpty(blobType))
            {
                blobType = _typeProvider.GetBlobType(blobName);
            }

            var blobInfo = new BlobInfo
                           {
                               Id = blobId,
                               Name = blobName,
                               Type = blobType,
                               Size = blobData.LongLength,
                               Time = DateTime.UtcNow
                           };

            WriteBlobInfo(blobId, blobInfo);
            WriteBlobData(blobId, blobData);

            _performanceLog.Log(LogComponentName, "UpdateBlob", start, null);
        }

        public void DeleteBlob(string blobId)
        {
            if (string.IsNullOrEmpty(blobId))
            {
                throw new ArgumentNullException(nameof(blobId));
            }

            var start = DateTime.Now;

            blobId = NormalizeBlobId(blobId);

            DeleteBlobData(blobId);

            _performanceLog.Log(LogComponentName, "DeleteBlob", start, null);
        }


        private BlobInfo ReadBlobInfo(string blobId)
        {
            BlobInfo blobInfo = null;

            var infoFilePath = GetBlobInfoFilePath(blobId);

            // Если файл с мета-информацией существует
            if (File.Exists(infoFilePath))
            {
                using (var infoFile = File.OpenRead(infoFilePath))
                {
                    blobInfo = _serializer.Deserialize(infoFile, typeof(BlobInfo)) as BlobInfo;
                }
            }
            // Если файл с мета-информацией не существует
            else
            {
                var dataFilePath = GetBlobDataFilePath(blobId);
                var dataFileInfo = new FileInfo(dataFilePath);

                if (dataFileInfo.Exists)
                {
                    blobInfo = new BlobInfo
                               {
                                   Id = blobId,
                                   Name = blobId,
                                   Type = _typeProvider.GetBlobType(blobId),
                                   Size = dataFileInfo.Length,
                                   Time = dataFileInfo.LastWriteTime
                               };
                }
            }

            return blobInfo;
        }

        private void WriteBlobInfo(string blobId, BlobInfo blobInfo)
        {
            var blobDir = GetBlobDirectoryPath(blobId);

            if (!Directory.Exists(blobDir))
            {
                Directory.CreateDirectory(blobDir);
            }

            var infoFilePath = GetBlobInfoFilePath(blobId);

            using (var infoFile = File.Create(infoFilePath))
            {
                _serializer.Serialize(infoFile, blobInfo);
            }
        }


        private byte[] ReadBlobData(string blobId)
        {
            var dataFilePath = GetBlobDataFilePath(blobId);

            if (File.Exists(dataFilePath))
            {
                return File.ReadAllBytes(dataFilePath);
            }

            return null;
        }

        private void WriteBlobData(string blobId, byte[] blobData)
        {
            var blobDir = GetBlobDirectoryPath(blobId);

            if (!Directory.Exists(blobDir))
            {
                Directory.CreateDirectory(blobDir);
            }

            var dataFilePath = GetBlobDataFilePath(blobId);

            File.WriteAllBytes(dataFilePath, blobData);
        }

        private void DeleteBlobData(string blobId)
        {
            var blobDir = GetBlobDirectoryPath(blobId);

            if (Directory.Exists(blobDir))
            {
                Directory.Delete(blobDir, true);
            }
        }


        private string GetBlobDirectoryPath(string blobId)
        {
            var level1 = blobId.Substring(0, 2);
            var level2 = blobId.Substring(2, 2);
            var level3 = blobId.Substring(4);

            return Path.Combine(_baseDirectory, level1, level2, level3);
        }

        private string GetBlobInfoFilePath(string blobId)
        {
            return Path.Combine(GetBlobDirectoryPath(blobId), "info");
        }

        private string GetBlobDataFilePath(string blobId)
        {
            return Path.Combine(GetBlobDirectoryPath(blobId), "data");
        }


        private static string GenerateBlobId()
        {
            return Guid.NewGuid().ToString("N");
        }

        private static string NormalizeBlobId(string blobId)
        {
            Guid blobGuid;

            if (Guid.TryParse(blobId, out blobGuid))
            {
                // TODO: Refactor
                // Ниже осуществляется переформатирование ссылки на файл.
                // Есть старые документы, которые хранят ссылки на файлы в ином формате.
                // Нужно сделать миграцию этих данных, чтобы отказаться от данного кода.
                // Код был добавлен при переносе файлов с Cassandra в файловую систему.

                blobId = blobGuid.ToString("N");
            }

            return blobId;
        }
    }
}