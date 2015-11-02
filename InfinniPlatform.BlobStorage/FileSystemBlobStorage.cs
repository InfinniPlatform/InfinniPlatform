using System;
using System.IO;

using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Sdk.Environment.Binary;

namespace InfinniPlatform.BlobStorage
{
    public sealed class FileSystemBlobStorage : IBlobStorage, IBlobStorageManager
    {
        public FileSystemBlobStorage(string baseDirectory)
        {
            _baseDirectory = baseDirectory;
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

            var blobInfo = ReadBlobInfo(blobId);

            return blobInfo;
        }

        public BlobData GetBlobData(string blobId)
        {
            if (string.IsNullOrEmpty(blobId))
            {
                throw new ArgumentNullException(nameof(blobId));
            }

            var blobInfo = ReadBlobInfo(blobId);
            var blobData = ReadBlobData(blobId);

            return new BlobData
                   {
                       Info = blobInfo,
                       Data = blobData
                   };
        }

        public void SaveBlob(string blobId, string blobName, byte[] blobData)
        {
            var blobType = _typeProvider.GetBlobType(blobName);

            SaveBlob(blobId, blobName, blobType, blobData);
        }

        public void SaveBlob(string blobId, string blobName, string blobType, byte[] blobData)
        {
            if (string.IsNullOrEmpty(blobId))
            {
                throw new ArgumentNullException(nameof(blobId));
            }

            if (blobData == null)
            {
                throw new ArgumentNullException(nameof(blobData));
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

            DeleteBlobData(blobId);
        }

        public void CreateStorage()
        {
            if (!Directory.Exists(_baseDirectory))
            {
                Directory.CreateDirectory(_baseDirectory);
            }
        }

        public void DeleteStorage()
        {
            if (Directory.Exists(_baseDirectory))
            {
                Directory.Delete(_baseDirectory, true);
            }
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
    }
}