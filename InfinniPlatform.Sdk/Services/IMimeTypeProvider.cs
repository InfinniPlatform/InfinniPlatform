namespace InfinniPlatform.Sdk.Services
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
        /// <returns>MIME-тип файла.</returns>
        string GetMimeType(string fileName);
    }
}