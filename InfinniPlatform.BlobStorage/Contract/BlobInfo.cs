using System;

namespace InfinniPlatform.BlobStorage.Contract
{
    /// <summary>
    /// Информация о BLOB.
    /// </summary>
    public sealed class BlobInfo
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Наименование.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Формат данных.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Размер в байтах.
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Дата и время создания.
        /// </summary>
        public DateTime Time { get; set; }
    }
}