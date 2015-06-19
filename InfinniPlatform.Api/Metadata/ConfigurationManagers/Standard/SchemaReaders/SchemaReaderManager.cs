using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Schema;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.SchemaReaders
{
    public sealed class SchemaReaderManager : ISchemaProvider
    {
        /// <summary>
        ///     Получить схему документа
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="documentId">Идентификатор документа</param>
        /// <returns>Схема документа</returns>
        public dynamic GetSchema(string version, string configId, string documentId)
        {
            return new MetadataApi(version).GetDocumentSchema(configId, documentId);
        }
    }
}