using System;
using System.IO;
using System.Threading.Tasks;

using InfinniPlatform.Http;
using InfinniPlatform.Logging;
using InfinniPlatform.Serialization;

namespace InfinniPlatform.BlobStorage
{
    /// <summary>
    /// Реализует сервис для работы хранилищем BLOB (Binary Large OBject) на основе файловой системы.
    /// </summary>
    /// <remarks>
    /// Достаточно простая реализация. Хранилище представляет собой каталог с файлами. Для обеспечения
    /// быстрого доступа к файлам - log(N) - каталог организован в виден набора вложенных друг в друга
    /// подкаталогов. Нулевой уровень - корневая папка хранилища, первый уровень - первые 2 символа
    /// идентификатора BLOB, второй уровень - вторые 2 символа идентификатора BLOB, третий уровень -
    /// оставшиеся символы идентификатора BLOB. В каталоге третьего уровня содержат два файла: info
    /// и data. В файле info хранится мета-информация о BLOB, представленная в виде JSON-объекта
    /// (идентификатор, наименование, MIME-тип, размер, дата изменения и т.п.). Наличие файла info
    /// в текущей реализации не обязательно. В файле data хранятся данные BLOB. Наличие файла data
    /// обязательно.
    /// <br/>
    /// <br/>
    /// Выбор в пользу использования обычной файловой системы был сделан не случайно. Во-первых,
    /// это самый простой и гибкий способ. Во-вторых, некоторые распределенные файловые системы
    /// имеют FUSE (Filesystem in Userspace) адаптеры, поддерживающие POSIX-стандарт, что дает
    /// возможность использовать функции обычной файловой системы при работе, не задумываясь о
    /// том, что на самом деле работа идет с распределенным хранилищем. В-третьих, пока трудно
    /// судить о том, какое распределенное хранилище (из тех, которые не имеют FUSE) подойдет
    /// лучше других.
    /// <br/>
    /// <br/>
    /// ЗАМЕЧАНИЕ. В настоящее время чтение и запись выполняются в режиме <see cref="FileShare.ReadWrite"/>,
    /// что сделано для решения проблемы с блокировки доступа к файлам в многопоточном режиме,
    /// однако появляется вероятность грязного чтения и грязной записи. Если предложенный подход
    /// не решит проблему с блокировками доступа или создаст неудобство с доступом на запись,
    /// следует организовать очередь на запись с возможностью повтора записи при возникновении
    /// ошибок, а доступ к каждому файлу контролировать механизмом, подобным
    /// <see cref="System.Threading.ReaderWriterLockSlim"/>, но более простым.
    /// </remarks>
    [LoggerName(nameof(FileSystemBlobStorage))]
    public class FileSystemBlobStorage : IBlobStorage
    {
        public FileSystemBlobStorage(FileSystemBlobStorageOptions options,
                                     IObjectSerializer objectSerializer,
                                     IMimeTypeResolver mimeTypeResolver,
                                     IPerformanceLogger<FileSystemBlobStorage> perfLogger)
        {
            var baseDirectory = Environment.ExpandEnvironmentVariables(options.BaseDirectory);

            if (string.IsNullOrEmpty(baseDirectory))
            {
                baseDirectory = FileSystemBlobStorageOptions.DefaultBaseDirectory;
            }

            _baseDirectory = baseDirectory;
            _objectSerializer = objectSerializer;
            _mimeTypeResolver = mimeTypeResolver;
            _perfLogger = perfLogger;
        }


        private readonly string _baseDirectory;
        private readonly IObjectSerializer _objectSerializer;
        private readonly IMimeTypeResolver _mimeTypeResolver;
        private readonly IPerformanceLogger _perfLogger;


        public BlobInfo GetBlobInfo(string blobId)
        {
            var start = DateTime.Now;

            try
            {
                if (string.IsNullOrEmpty(blobId))
                {
                    throw new ArgumentNullException(nameof(blobId));
                }

                var result = ReadBlobInfo(blobId);

                _perfLogger.Log("GetBlobInfo", start);

                return result;
            }
            catch (Exception e)
            {
                _perfLogger.Log("GetBlobInfo", start, e);

                throw;
            }
        }

        public BlobData GetBlobData(string blobId)
        {
            var start = DateTime.Now;

            try
            {
                if (string.IsNullOrEmpty(blobId))
                {
                    throw new ArgumentNullException(nameof(blobId));
                }

                var blobInfo = ReadBlobInfo(blobId);
                var blobData = ReadBlobData(blobId);

                var result = (blobInfo != null || blobData != null)
                    ? new BlobData
                    {
                        Info = blobInfo,
                        Data = blobData
                    }
                    : null;

                _perfLogger.Log("GetBlobData", start);

                return result;
            }
            catch (Exception e)
            {
                _perfLogger.Log("GetBlobData", start, e);

                throw;
            }
        }

        public BlobInfo CreateBlob(string blobName, string blobType, Stream blobData)
        {
            var blobId = GenerateBlobId();

            return UpdateBlob(blobId, blobName, blobType, blobData);
        }

        public Task<BlobInfo> CreateBlobAsync(string blobName, string blobType, Stream blobData)
        {
            var blobId = GenerateBlobId();

            return UpdateBlobAsync(blobId, blobName, blobType, blobData);
        }

        public BlobInfo UpdateBlob(string blobId, string blobName, string blobType, Stream blobData)
        {
            var start = DateTime.Now;

            try
            {
                if (string.IsNullOrEmpty(blobId))
                {
                    throw new ArgumentNullException(nameof(blobId));
                }

                if (blobData == null)
                {
                    throw new ArgumentNullException(nameof(blobData));
                }

                if (string.IsNullOrEmpty(blobType))
                {
                    blobType = _mimeTypeResolver.GetMimeType(blobName);
                }

                var blobInfo = new BlobInfo
                {
                    Id = blobId,
                    Name = blobName,
                    Type = blobType,
                    Size = blobData.Length,
                    Time = DateTime.UtcNow
                };

                WriteBlobInfo(blobId, blobInfo);
                WriteBlobData(blobId, blobData);

                _perfLogger.Log("UpdateBlob", start);

                return blobInfo;
            }
            catch (Exception e)
            {
                _perfLogger.Log("UpdateBlob", start, e);

                throw;
            }
        }

        public async Task<BlobInfo> UpdateBlobAsync(string blobId, string blobName, string blobType, Stream blobData)
        {
            var start = DateTime.Now;

            try
            {
                if (string.IsNullOrEmpty(blobId))
                {
                    throw new ArgumentNullException(nameof(blobId));
                }

                if (blobData == null)
                {
                    throw new ArgumentNullException(nameof(blobData));
                }

                if (string.IsNullOrEmpty(blobType))
                {
                    blobType = _mimeTypeResolver.GetMimeType(blobName);
                }

                var blobInfo = new BlobInfo
                {
                    Id = blobId,
                    Name = blobName,
                    Type = blobType,
                    Size = blobData.Length,
                    Time = DateTime.UtcNow
                };

                WriteBlobInfo(blobId, blobInfo);
                await WriteBlobDataAsync(blobId, blobData);

                _perfLogger.Log("UpdateBlob", start);

                return blobInfo;
            }
            catch (Exception e)
            {
                _perfLogger.Log("UpdateBlob", start, e);

                throw;
            }
        }

        public void DeleteBlob(string blobId)
        {
            var start = DateTime.Now;

            try
            {
                if (string.IsNullOrEmpty(blobId))
                {
                    throw new ArgumentNullException(nameof(blobId));
                }

                TryDeleteDirectory(blobId);

                _perfLogger.Log("DeleteBlob", start);
            }
            catch (Exception e)
            {
                _perfLogger.Log("DeleteBlob", start, e);

                throw;
            }
        }

        // BlobInfo

        private BlobInfo ReadBlobInfo(string blobId)
        {
            BlobInfo blobInfo = null;

            var infoFilePath = GetBlobInfoFilePath(blobId);

            // Если файл с мета-информацией существует
            if (IsFileExists(infoFilePath))
            {
                using (var infoFile = OpenReadFileStream(infoFilePath))
                {
                    blobInfo = _objectSerializer.Deserialize(infoFile, typeof(BlobInfo)) as BlobInfo;
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
                        Type = _mimeTypeResolver.GetMimeType(blobId),
                        Size = dataFileInfo.Length,
                        Time = dataFileInfo.LastWriteTimeUtc
                    };
                }
            }

            return blobInfo;
        }

        private void WriteBlobInfo(string blobId, BlobInfo blobInfo)
        {
            TryCreateDirectory(blobId);

            var infoFilePath = GetBlobInfoFilePath(blobId);

            using (var infoFile = OpenWriteFileStream(infoFilePath))
            {
                _objectSerializer.Serialize(infoFile, blobInfo);
            }
        }

        // BlobData

        private Func<Stream> ReadBlobData(string blobId)
        {
            var dataFilePath = GetBlobDataFilePath(blobId);

            if (IsFileExists(dataFilePath))
            {
                return () => OpenReadFileStream(dataFilePath);
            }

            return null;
        }

        private void WriteBlobData(string blobId, Stream blobData)
        {
            TryCreateDirectory(blobId);

            var dataFilePath = GetBlobDataFilePath(blobId);

            using (var dataFile = OpenWriteFileStream(dataFilePath))
            {
                blobData.CopyTo(dataFile);
                dataFile.Flush();
            }
        }

        private async Task WriteBlobDataAsync(string blobId, Stream blobData)
        {
            TryCreateDirectory(blobId);

            var dataFilePath = GetBlobDataFilePath(blobId);

            using (var dataFile = OpenWriteFileStream(dataFilePath))
            {
                await blobData.CopyToAsync(dataFile);
                await dataFile.FlushAsync();
            }
        }

        // File paths

        private static string GenerateBlobId()
        {
            return Guid.NewGuid().ToString("N");
        }

        private string GetBlobInfoFilePath(string blobId)
        {
            return Path.Combine(GetBlobDirectoryPath(blobId), "info");
        }

        private string GetBlobDataFilePath(string blobId)
        {
            return Path.Combine(GetBlobDirectoryPath(blobId), "data");
        }

        private string GetBlobDirectoryPath(string blobId)
        {
            var level1 = blobId.Substring(0, 2);
            var level2 = blobId.Substring(2, 2);
            var level3 = blobId.Substring(4);

            return Path.Combine(_baseDirectory, level1, level2, level3);
        }

        // File system

        private void TryCreateDirectory(string blobId)
        {
            var blobDir = GetBlobDirectoryPath(blobId);

            if (!Directory.Exists(blobDir))
            {
                Directory.CreateDirectory(blobDir);
            }
        }

        private void TryDeleteDirectory(string blobId)
        {
            var blobDir = GetBlobDirectoryPath(blobId);

            if (Directory.Exists(blobDir))
            {
                Directory.Delete(blobDir, true);
            }
        }

        private static bool IsFileExists(string path)
        {
            return File.Exists(path);
        }

        private static Stream OpenWriteFileStream(string path)
        {
            return new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        }

        private static Stream OpenReadFileStream(string path)
        {
            return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }
    }
}