namespace InfinniPlatform.Api.Metadata.Validation
{
    /// <summary>
    ///     Сервис проверки корректности схемы объекта метаданных.
    /// </summary>
    public interface IMetadataSchemaValidator
    {
        /// <summary>
        ///     Проверяет корректность схемы объекта метаданных.
        /// </summary>
        /// <param name="metadataValue">Объект метаданных.</param>
        /// <exception cref="MetadataSchemaException"></exception>
        void Validate(object metadataValue);
    }
}