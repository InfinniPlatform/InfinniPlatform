using System;
using System.IO;

using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Sdk.Environment.Binary;

namespace InfinniPlatform.BlobStorage
{
    /// <summary>
    /// Реализует сервис для работы хранилищем BLOB (Binary Large OBject) на основе файловой системы.
    /// </summary>
    /// <returns>
    /// Достаточно простая реализация. Хранилище представляет собой каталог с файлами. Файлы храняться
    /// в подкаталогах. Имена подкаталогов совпадают с уникальными идентификторами файлов. Подкаталоги
    /// содержат два файла: info и data. В файле info хранится метаинформация о файле, представленная
    /// в виде JSON-объекта (идентификтор, наименование, MIME-тип, размер, дата измения и т.п.).
    /// В файле data хранятся данные самого файла (собственно BLOB).
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
        public FileSystemBlobStorage(string baseDirectory)
        {
            _baseDirectory = baseDirectory;

            // TODO: Refactor
            // Получать эти зависимости через конструктор
            _serializer = JsonObjectSerializer.Default;
            _typeProvider = FileExtensionTypeProvider.Default;
        }


        private readonly string _baseDirectory;
        private readonly IObjectSerializer _serializer;
        private readonly FileExtensionTypeProvider _typeProvider;


        public BlobInfo GetBlobInfo(string blobId)
        {
            if (string.IsNullOrEmpty(blobId))
            {
                throw new ArgumentNullException(nameof(blobId));
            }

            blobId = NormalizeBlobId(blobId);

            var blobInfo = ReadBlobInfo(blobId);

            return blobInfo;
        }

        public BlobData GetBlobData(string blobId)
        {
            if (string.IsNullOrEmpty(blobId))
            {
                throw new ArgumentNullException(nameof(blobId));
            }

            blobId = NormalizeBlobId(blobId);

            var blobInfo = ReadBlobInfo(blobId);
            var blobData = ReadBlobData(blobId);

            return (blobInfo != null || blobData != null)
                ? new BlobData
                  {
                      Info = blobInfo,
                      Data = blobData
                  }
                : null;
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
        }

        public void DeleteBlob(string blobId)
        {
            if (string.IsNullOrEmpty(blobId))
            {
                throw new ArgumentNullException(nameof(blobId));
            }

            blobId = NormalizeBlobId(blobId);

            DeleteBlobData(blobId);
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
            return Path.Combine(_baseDirectory, blobId);
        }

        private string GetBlobInfoFilePath(string blobId)
        {
            return Path.Combine(_baseDirectory, blobId, "info");
        }

        private string GetBlobDataFilePath(string blobId)
        {
            return Path.Combine(_baseDirectory, blobId, "data");
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
                // Ниже осущесвляется переформатирование ссылки на файл.
                // Есть старые документы, которые хранят ссылки на файлы в ином формате.
                // Нужно сделать миграцию этих данных, чтобы отказаться от данного кода.
                // Код был добавлен при переносе файлов с Cassandra в файловую систему.

                blobId = blobGuid.ToString("N");
            }

            return blobId;
        }
    }
}