using System.IO;

namespace InfinniPlatform.BlobStorage.Abstractions
{
    public static class BlobStorageExtensions
    {
        /// <summary>
        /// Создает BLOB.
        /// </summary>
        /// <param name="blobName">Наименование BLOB.</param>
        /// <param name="blobType">Формат данных BLOB.</param>
        /// <param name="blobData">Данные BLOB.</param>
        /// <returns>Идентификатор BLOB. </returns>
        public static BlobInfo CreateBlob(this IBlobStorage target, string blobName, string blobType, byte[] blobData)
        {
            using (var dataStream = new MemoryStream(blobData))
            {
                return target.CreateBlob(blobName, blobType, dataStream);
            }
        }

        /// <summary>
        /// Обновляет BLOB.
        /// </summary>
        /// <param name="blobId">Идентификатор BLOB.</param>
        /// <param name="blobName">Наименование BLOB.</param>
        /// <param name="blobType">Формат данных BLOB.</param>
        /// <param name="blobData">Данные BLOB.</param>
        /// <returns>Идентификатор BLOB. </returns>
        public static BlobInfo UpdateBlob(this IBlobStorage target, string blobId, string blobName, string blobType, byte[] blobData)
        {
            using (var dataStream = new MemoryStream(blobData))
            {
                return target.UpdateBlob(blobId, blobName, blobType, dataStream);
            }
        }
    }
}