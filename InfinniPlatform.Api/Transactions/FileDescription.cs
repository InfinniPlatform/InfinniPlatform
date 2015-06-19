namespace InfinniPlatform.Api.Transactions
{
    /// <summary>
    ///     Описание связанного с документом файла
    /// </summary>
    public sealed class FileDescription
    {
        /// <summary>
        ///     Поле ссылки в документе, связанное с файлом
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        ///     Файловый поток
        /// </summary>
        public byte[] Bytes { get; set; }
    }
}