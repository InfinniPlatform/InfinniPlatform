using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.DocumentStorage.MongoDB.Conventions;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Settings;

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Подключение к MongoDB.
    /// </summary>
    internal sealed class MongoConnection
    {
        private static volatile bool _applyConverters;
        private static readonly object ApplyConvertersSync = new object();


        static MongoConnection()
        {
            // Игнорирование null значений в свойствах документов, игнорирование свойств id в классах
            var defaultConventions = new ConventionPack { new IgnoreIfNullConvention(true), new NoIdMemberConvention(), new IgnoreDiscriminatorConvention()};
            ConventionRegistry.Register("IgnoreRules", defaultConventions, t => true);

            // Установка правил сериализации и десериализации внутренних типов данных
            BsonSerializer.RegisterSerializer(MongoDateBsonSerializer.Default);
            BsonSerializer.RegisterSerializer(MongoTimeBsonSerializer.Default);
            BsonSerializer.RegisterSerializer(MongoDynamicWrapperBsonSerializer.Default);
            BsonSerializer.RegisterSerializationProvider(MongoDateBsonSerializationProvider.Default);
            BsonSerializer.RegisterSerializationProvider(MongoTimeBsonSerializationProvider.Default);
            BsonSerializer.RegisterSerializationProvider(MongoDynamicWrapperBsonSerializationProvider.Default);
            BsonSerializer.RegisterSerializationProvider(UnknownTypeBsonSerializationProvider.Default);
        }


        public MongoConnection(IAppEnvironment appEnvironment, MongoConnectionSettings connectionSettings, IEnumerable<IMemberValueConverter> converters = null)
        {
            ApplyConverters(converters);

            _database = new Lazy<IMongoDatabase>(() => CreateMongoDatabase(appEnvironment.Name, connectionSettings));
        }


        private readonly Lazy<IMongoDatabase> _database;


        private static void ApplyConverters(IEnumerable<IMemberValueConverter> converters)
        {
            if (!_applyConverters)
            {
                lock (ApplyConvertersSync)
                {
                    if (!_applyConverters)
                    {
                        var converterList = converters?.ToArray();

                        if (converterList?.Length > 0)
                        {
                            var convertConventions = new ConventionPack { new MongoMemberValueConverterResolver(converterList) };
                            ConventionRegistry.Register("ConvertRules", convertConventions, t => true);

                            _applyConverters = true;
                        }
                    }
                }
            }
        }

        private static IMongoDatabase CreateMongoDatabase(string databaseName, MongoConnectionSettings connectionSettings)
        {
            var mongoClientSettings = new MongoClientSettings();

            if (connectionSettings.Nodes != null && connectionSettings.Nodes.Length > 0)
            {
                var servers = new List<MongoServerAddress>();

                foreach (var server in connectionSettings.Nodes)
                {
                    MongoServerAddress serverAddress;

                    if (MongoServerAddress.TryParse(server, out serverAddress))
                    {
                        servers.Add(serverAddress);
                    }
                }

                if (servers.Count > 0)
                {
                    mongoClientSettings.Servers = servers;
                }
            }

            if (!string.IsNullOrWhiteSpace(databaseName)
                && !string.IsNullOrWhiteSpace(connectionSettings.UserName)
                && !string.IsNullOrWhiteSpace(connectionSettings.Password))
            {
                var mongoCredential = MongoCredential.CreateCredential(databaseName, connectionSettings.UserName, connectionSettings.Password);

                mongoClientSettings.Credentials = new[] { mongoCredential };
            }

            var mongoClient = new MongoClient(mongoClientSettings);

            return mongoClient.GetDatabase(databaseName);
        }


        /// <summary>
        /// Возвращает интерфейс доступа к хранилищу данных.
        /// </summary>
        public IMongoDatabase GetDatabase()
        {
            return _database.Value;
        }


        /// <summary>
        /// Удаляет хранилище данных.
        /// </summary>
        public Task DropDatabaseAsync()
        {
            var databaseName = _database.Value.DatabaseNamespace.DatabaseName;

            return _database.Value.Client.DropDatabaseAsync(databaseName);
        }


        /// <summary>
        /// Возвращает состояние базы данных.
        /// </summary>
        public Task<DynamicWrapper> GetDatabaseStatusAsync()
        {
            var dbStats = new DynamicWrapper { { "dbStats", 1 } };

            return _database.Value.RunCommandAsync(new ObjectCommand<DynamicWrapper>(dbStats));
        }
    }
}