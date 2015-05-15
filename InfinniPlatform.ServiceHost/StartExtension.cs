using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.BlobStorage;
using InfinniPlatform.Cassandra.Client;
using InfinniPlatform.EventStorage;
using InfinniPlatform.Factories;

namespace InfinniPlatform.ServiceHost
{
    internal static class StartExtension
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
            }
            catch
            {
            }
        }

        internal static void CreateEventStorage()
        {
            var manager = GetEventStorageFactory().CreateEventStorageManager();

            try
            {
                manager.CreateStorage();
            }
            catch
            {
            }
        }
    }
}
