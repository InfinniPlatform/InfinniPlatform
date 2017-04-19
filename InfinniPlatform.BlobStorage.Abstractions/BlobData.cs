using System;
using System.IO;

namespace InfinniPlatform.BlobStorage
{
    /// <summary>
    /// Данные BLOB.
    /// </summary>
    public class BlobData
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