namespace InfinniPlatform.Api.Metadata.Validation
{
	/// <summary>
	/// Фабрика для создания сервисов проверки корректности схемы объектов метаданных.
	/// </summary>
	public interface IMetadataSchemaValidatorFactory
	{
		/// <summary>
		/// Создает сервис проверки корректности схемы представления <see cref="MetadataType.View"/>.
		/// </summary>
		/// <param name="detailedErrors">Детализированные ошибки.</param>
		IMetadataSchemaValidator CreateViewValidator(bool detailedErrors);

		/// <summary>
		/// Создает сервис проверки корректности схемы печатного представления <see cref="MetadataType.PrintView"/>.
		/// </summary>
		/// <param name="detailedErrors">Детализированные ошибки.</param>
		IMetadataSchemaValidator CreatePrintViewValidator(bool detailedErrors);
	}
}