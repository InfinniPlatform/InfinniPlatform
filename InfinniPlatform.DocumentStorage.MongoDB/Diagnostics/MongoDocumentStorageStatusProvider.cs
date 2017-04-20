using System.Threading.Tasks;

using InfinniPlatform.Core.Diagnostics;
using InfinniPlatform.Core.Http;

namespace InfinniPlatform.DocumentStorage.Diagnostics
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