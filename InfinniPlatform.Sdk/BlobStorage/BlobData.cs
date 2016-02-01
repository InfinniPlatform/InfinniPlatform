using System;
using System.IO;

namespace InfinniPlatform.Sdk.BlobStorage
{
    /// <summary>
    /// Данные BLOB.
    /// </summary>
    public sealed class BlobData
    {
        /// <summary>
        /// Информация о BLOB.
        /// </summary>
        public BlobInfo Info { get; set; }

        /// <summary>
        /// Данные BLOB.
        /// </summary>
        public Func<Stream> Data { get; set; }
    }
}