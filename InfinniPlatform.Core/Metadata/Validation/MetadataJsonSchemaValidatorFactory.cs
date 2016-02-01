namespace InfinniPlatform.Core.Metadata.Validation
{
    /// <summary>
    /// Фабрика для создания сервисов проверки корректности JSON-схемы объектов метаданных.
    /// </summary>
    public sealed class MetadataJsonSchemaValidatorFactory : IMetadataSchemaValidatorFactory
    {
        /// <summary>
        /// Создает сервис проверки корректности схемы представления <see cref="MetadataType.View" />.
        /// </summary>
        /// <param name="detailedErrors">Детализированные ошибки.</param>
        public IMetadataSchemaValidator CreateViewValidator(bool detailedErrors)
        {
            return new MetadataJsonSchemaValidator(MetadataType.View, detailedErrors);
        }

        /// <summary>
        /// Создает сервис проверки корректности схемы печатного представления <see cref="MetadataType.PrintView" />.
        /// </summary>
        /// <param name="detailedErrors">Детализированные ошибки.</param>
        public IMetadataSchemaValidator CreatePrintViewValidator(bool detailedErrors)
        {
            return new MetadataJsonSchemaValidator(MetadataType.PrintView, detailedErrors);
        }
    }
}