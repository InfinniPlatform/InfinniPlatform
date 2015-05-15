using InfinniPlatform.BlobStorage;
using InfinniPlatform.Cassandra.Client;
using InfinniPlatform.EventStorage;
using InfinniPlatform.Factories;
using InfinniPlatform.Logging;
using InfinniPlatform.RestfulApi.Properties;

namespace InfinniPlatform.RestfulApi.Extensions
{
    internal static class FastStorageExtension
    {
        internal static IBlobStorageFactory GetBlobStorageFactory()
        {
            var cassandraFactory = new CassandraDatabaseFactory();
            var blobStorageFactory = new CassandraBlobStorageFactory(cassandraFactory);
            return blobStorageFactory;
        }

        internal static IEventStorageFactory GetEventStorageFactory()
        {
            var cassandraFactory = new CassandraDatabaseFactory();
            var eventStorageFactory = new CassandraEventStorageFactory(cassandraFactory);
            return eventStorageFactory;
        }

        internal static void CreateBlobStorage()
        {
            var manager = GetBlobStorageFactory().CreateBlobStorageManager();

            try
            {
                manager.CreateStorage();
                Logger.Log.Info(Resources.BlobStorageCreated);
            }
            catch
            {
                Logger.Log.Info(Resources.BlobStorageAlreadyCreated);
            }
        }

        internal static void CreateEventStorage()
        {
            var manager = GetEventStorageFactory().CreateEventStorageManager();

            try
            {
                manager.CreateStorage();
                Logger.Log.Info(Resources.EventStorageCreated);
            }
            catch
            {
                Logger.Log.Info(Resources.EventStorageAlreadyCreated);
            }
        }
    }
}
