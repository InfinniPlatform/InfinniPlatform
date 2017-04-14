using System.Threading.Tasks;

using InfinniPlatform.Sdk.Diagnostics;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.DocumentStorage.MongoDB.Diagnostics
{
    /// <summary>
    /// Предоставляет информацию о состоянии подсистемы хранения документов.
    /// </summary>
    internal class MongoDocumentStorageStatusProvider : ISubsystemStatusProvider
    {
        public MongoDocumentStorageStatusProvider(MongoConnection connection)
        {
            _connection = connection;
        }


        private readonly MongoConnection _connection;


        public string Name => MongoDocumentStorageOptions.SectionName;


        public async Task<object> GetStatus(IHttpRequest request)
        {
            return await _connection.GetDatabaseStatusAsync();
        }
    }
}