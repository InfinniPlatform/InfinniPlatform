using System.Threading.Tasks;

using InfinniPlatform.Core.Diagnostics;
using InfinniPlatform.DocumentStorage.MongoDB;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DocumentStorage.Diagnostics
{
    /// <summary>
    /// Предоставляет информацию о состоянии MongoDB.
    /// </summary>
    internal sealed class MongoStatusProvider : ISubsystemStatusProvider
    {
        public MongoStatusProvider(MongoConnection connection)
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