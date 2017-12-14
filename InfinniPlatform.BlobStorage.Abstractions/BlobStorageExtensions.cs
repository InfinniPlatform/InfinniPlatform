using System.IO;

namespace InfinniPlatform.BlobStorage
{
    /// <summary>
    /// Extensions for blob storage.
    /// </summary>
    public static class BlobStorageExtensions
    {
        /// <summary>
        /// Creates BLOB.
        /// </summary>
        /// <param name="blobStorage">BLOB storage.</param>
        /// <param name="blobName">BLOB name.</param>
        /// <param name="blobType">BLOB data type.</param>
        /// <param name="blobData">BLOB data.</param>
        /// <returns>BLOB identifier.</returns>
        public static BlobInfo CreateBlob(this IBlobStorage blobStorage, string blobName, string blobType, byte[] blobData)
        {
            using (var dataStream = new MemoryStream(blobData))
            {
                return blobStorage.CreateBlob(blobName, blobType, dataStream);
            }
        }

        /// <summary>
        /// Updates BLOB.
        /// </summary>
        /// <param name="blobStorage">BLOB storage</param>
        /// <param name="blobId">BLOB identifier.</param>
        /// <param name="blobName">BLOB name.</param>
        /// <param name="blobType">BLOB data type.</param>
        /// <param name="blobData">BLOB data.</param>
        /// <returns>BLOB identifier. </returns>
        public static BlobInfo UpdateBlob(this IBlobStorage blobStorage, string blobId, string blobName, string blobType, byte[] blobData)
        {
            using (var dataStream = new MemoryStream(blobData))
            {
                return blobStorage.UpdateBlob(blobId, blobName, blobType, dataStream);
            }
        }
    }
}