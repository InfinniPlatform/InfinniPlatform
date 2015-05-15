namespace InfinniPlatform.Api.Metadata.MetadataContainers
{
	sealed class MetadataContainerScenario : IMetadataContainerInfo
	{
		/// <summary>
		///   Получить наименование контейнера метаданных
		/// </summary>
		/// <returns>Наименование контейнера метаданных</returns>
		public string GetMetadataContainerName()
		{
			return MetadataType.ScenarioContainer;
		}

		/// <summary>
		///   Получить наименование типа метаданных
		/// </summary>
		/// <returns>Наименование типа метаданных</returns>
		public string GetMetadataTypeName()
		{
			return MetadataType.Scenario;
		}
	}
}
