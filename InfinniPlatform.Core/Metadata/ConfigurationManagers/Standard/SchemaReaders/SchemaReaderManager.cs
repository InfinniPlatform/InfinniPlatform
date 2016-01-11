using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Core.Schema;

namespace InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.SchemaReaders
{
    public sealed class SchemaReaderManager : ISchemaProvider
    {
        /// <summary>
        ///     Получить схему документа
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="documentId">Идентификатор документа</param>
        /// <returns>Схема документа</returns>
        public dynamic GetSchema(string configId, string documentId)
        {
            return null;
        }
    }
}