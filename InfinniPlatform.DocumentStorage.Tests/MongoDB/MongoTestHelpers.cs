﻿using System.Collections.Generic;

using InfinniPlatform.DocumentStorage.Metadata;
using InfinniPlatform.Serialization;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    internal static class MongoTestHelpers
    {
        public const string DatabaseName = "MongoTest";


        public static MongoConnection GetConnection(IEnumerable<IMemberValueConverter> converters = null, IDocumentKnownTypeSource source = null)
        {
            var appOptions = new AppOptions { AppName = DatabaseName };

            return new MongoConnection(appOptions, MongoDocumentStorageOptions.Default, converters, source);
        }


        public static MongoDocumentStorageProvider GetStorageProvider(string documentType = null, IEnumerable<IMemberValueConverter> converters = null, IDocumentKnownTypeSource source = null)
        {
            return new MongoDocumentStorageProvider(GetConnection(converters, source), documentType);
        }

        public static MongoDocumentStorageProvider<TDocument> GetStorageProvider<TDocument>(string documentType = null, IEnumerable<IMemberValueConverter> converters = null, IDocumentKnownTypeSource source = null)
        {
            return new MongoDocumentStorageProvider<TDocument>(GetConnection(converters, source), documentType);
        }


        public static MongoDocumentStorageProvider GetEmptyStorageProvider(string documentType, params DocumentIndex[] indexes)
        {
            var connection = CreateEmptyStorage(documentType, null, indexes);
            return new MongoDocumentStorageProvider(connection, documentType);
        }

        public static MongoDocumentStorageProvider GetEmptyStorageProvider(string documentType, IEnumerable<IMemberValueConverter> converters = null, IDocumentKnownTypeSource source = null, params DocumentIndex[] indexes)
        {
            var connection = CreateEmptyStorage(documentType, converters, indexes);
            return new MongoDocumentStorageProvider(connection, documentType);
        }


        public static MongoDocumentStorageProvider<TDocument> GetEmptyStorageProvider<TDocument>(string documentType, params DocumentIndex[] indexes)
        {
            var connection = CreateEmptyStorage(documentType, null, indexes);
            return new MongoDocumentStorageProvider<TDocument>(connection, documentType);
        }

        public static MongoDocumentStorageProvider<TDocument> GetEmptyStorageProvider<TDocument>(string documentType, IEnumerable<IMemberValueConverter> converters = null, IDocumentKnownTypeSource source = null, params DocumentIndex[] indexes)
        {
            var connection = CreateEmptyStorage(documentType, converters, indexes);
            return new MongoDocumentStorageProvider<TDocument>(connection, documentType);
        }


        private static MongoConnection CreateEmptyStorage(string documentType, IEnumerable<IMemberValueConverter> converters = null, params DocumentIndex[] indexes)
        {
            var connection = GetConnection(converters);
            var database = connection.GetDatabase();
            database.DropCollection(documentType);

            if (indexes != null && indexes.Length > 0)
            {
                var storageManager = new MongoDocumentStorageManager(connection);
                storageManager.CreateStorageAsync(new DocumentMetadata { Type = documentType, Indexes = indexes }).Wait();
            }

            return connection;
        }
    }
}