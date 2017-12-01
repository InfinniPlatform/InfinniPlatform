namespace InfinniPlatform.Http
{
    /// <summary>
    /// Предоставляет метод для определения MIME-типа.
    /// </summary>
    public interface IMimeTypeResolver
    {
        /// <summary>
        /// Возвращает MIME-тип по имени файла.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <returns>MIME-тип файла (если не найден, то 'application/octet-stream').</returns>
        string GetMimeType(string fileName);
    }
}