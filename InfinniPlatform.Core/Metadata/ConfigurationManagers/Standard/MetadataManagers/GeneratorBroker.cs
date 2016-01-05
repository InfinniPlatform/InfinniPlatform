namespace InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.MetadataManagers
{
    /// <summary>
    /// API для действий с генераторами.
    /// Необходим для упрощения работы с генераторами, так как действия с метаданными генераторов
    /// охватывают несколько типов метаданных элементов (сценарии, бизнес-процессы, сервисы)
    /// </summary>
    public sealed class GeneratorBroker
    {
        public GeneratorBroker(string configurationId, string documentId)
        {
            _configurationId = configurationId;
            _documentId = documentId;
        }

        private readonly string _configurationId;
        private readonly string _documentId;

        public void CreateGenerator(dynamic generatorObject)
        {
        }

        public void DeleteGenerator(string generatorName)
        {
        }
    }
}