namespace InfinniPlatform.Sdk.Environment.Binary
{
    /// <summary>
    /// Сервис для работы хранилищем BLOB (Binary Large OBject).
    /// </summary>
    public interface IBlobStorage
    {
        /// <summary>
        /// Возвращает информацию о BLOB.
        /// </summary>
        /// <param name="blobId">Идентификатор BLOB.</param>
        /// <returns>Информация о BLOB.</returns>
        BlobInfo GetBlobInfo(string blobId);

        /// <summary>
        /// Возвращает данные BLOB.
        /// </summary>
        /// <param name="blobId">Идентификатор BLOB.</param>
        /// <returns>Данные BLOB.</returns>
        BlobData GetBlobData(string blobId);

        /// <summary>
        /// Создает BLOB.
        /// </summary>
        /// <param name="blobName">Наименование BLOB.</param>
        /// <param name="blobType">Формат данных BLOB.</param>
        /// <param name="blobData">Данные BLOB.</param>
        /// <returns>Идентификатор BLOB. </returns>
        string CreateBlob(string blobName, string blobType, byte[] blobData);

        /// <summary>
        /// Обновляет BLOB.
        /// </summary>
        /// <param name="blobId">Идентификатор BLOB.</param>
        /// <param name="blobName">Наименование BLOB.</param>
        /// <param name="blobType">Формат данных BLOB.</param>
        /// <param name="blobData">Данные BLOB.</param>
        /// <returns>Идентификатор BLOB. </returns>
        void UpdateBlob(string blobId, string blobName, string blobType, byte[] blobData);

        /// <summary>
        /// Удаляет BLOB.
        /// </summary>
        /// <param name="blobId">Идентификатор BLOB.</param>
        void DeleteBlob(string blobId);
    }
}