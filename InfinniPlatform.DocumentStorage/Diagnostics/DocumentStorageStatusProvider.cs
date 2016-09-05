using System.Threading.Tasks;

using InfinniPlatform.DocumentStorage.MongoDB;
using InfinniPlatform.Sdk.Diagnostics;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DocumentStorage.Diagnostics
{
    /// <summary>
    /// Предоставляет информацию о состоянии подсистемы хранения документов.
    /// </summary>
    internal sealed class DocumentStorageStatusProvider : ISubsystemStatusProvider
    {
        public DocumentStorageStatusProvider(MongoConnection connection)
        {
            _connection = connection;
        }


        private readonly MongoConnection _connection;


        public string Name => "documentStorage";


        public async Task<object> GetStatus()
        {
            var mongodb = await _connection.GetDatabaseStatusAsync();

            return new DynamicWrapper
                   {
                       { "mongodb", mongodb }
                   };
        }
    }
}