﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using InfinniPlatform.Dynamic;
using InfinniPlatform.Serialization;

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Подключение к MongoDB.
    /// </summary>
    internal class MongoConnection
    {
        private static volatile bool _applyConverters;
        private static readonly object ApplyConvertersSync = new object();


        static MongoConnection()
        {
            // Установка соглашений

            var defaultConventions = new ConventionPack
                                     {
                                         // Не сохраняет свойства со значением null
                                         new IgnoreIfNullConvention(true),
                                         // Игнорирует свойства с именем id
                                         new NoIdMemberConvention(),
                                         // Не устанавливает дискриминатор
                                         new MongoIgnoreDiscriminatorConvention(),
                                         // Не сохраняет указанные свойства
                                         new MongoIgnorePropertyConvention(),
                                         // Позволяет определять имена свойств
                                         new MongoPropertyNameConvention(),
                                         // Определяет логику сериализации object
                                         new MongoObjectMemberConverterResolver()
                                     };

            ConventionRegistry.Register("DefaultConventions", defaultConventions, t => true);

            // Установка правил сериализации и десериализации внутренних типов данных
            BsonSerializer.RegisterSerializer(MongoDateBsonSerializer.Default);
            BsonSerializer.RegisterSerializer(MongoTimeBsonSerializer.Default);
            BsonSerializer.RegisterSerializer(MongoDynamicDocumentBsonSerializer.Default);
            BsonSerializer.RegisterSerializer(MongoDecimalBsonSerializer.Default);
            BsonSerializer.RegisterSerializationProvider(MongoDateBsonSerializationProvider.Default);
            BsonSerializer.RegisterSerializationProvider(MongoTimeBsonSerializationProvider.Default);
            BsonSerializer.RegisterSerializationProvider(MongoDynamicDocumentBsonSerializationProvider.Default);
            BsonSerializer.RegisterSerializationProvider(MongoDecimalBsonSerializationProvider.Default);
        }

        public MongoConnection(AppOptions appOptions,
                               MongoDocumentStorageOptions options,
                               IEnumerable<IMemberValueConverter> converters = null,
                               IDocumentKnownTypeSource knownTypeSource = null)
        {
            ApplyConverters(converters);
            RegisterKnownTypes(knownTypeSource);

            _database = new Lazy<IMongoDatabase>(() => CreateMongoDatabase(appOptions.AppName, options));
        }

        private readonly Lazy<IMongoDatabase> _database;

        private static void RegisterKnownTypes(IDocumentKnownTypeSource source)
        {
            if (source == null)
            {
                return;
            }

            foreach (var type in source.KnownTypes)
            {
                var bsonClassMap = new BsonClassMap(type);

                foreach (var property in type.GetTypeInfo().GetProperties().Where(i => i.DeclaringType == type))
                {
                    bsonClassMap.MapProperty(property.Name);
                }

                BsonClassMap.RegisterClassMap(bsonClassMap);
            }
        }

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

        private static IMongoDatabase CreateMongoDatabase(string databaseName, MongoDocumentStorageOptions options)
        {
            var mongoClient = new MongoClient(options.ConnectionString);

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
        public Task<DynamicDocument> GetDatabaseStatusAsync()
        {
            var dbStats = new DynamicDocument { { "dbStats", 1 } };

            return _database.Value.RunCommandAsync(new ObjectCommand<DynamicDocument>(dbStats));
        }
    }
}