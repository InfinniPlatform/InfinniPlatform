﻿using System.Threading.Tasks;

using InfinniPlatform.Diagnostics;
using InfinniPlatform.Http;
using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.DocumentStorage.Diagnostics
{
    /// <summary>
    /// Предоставляет информацию о состоянии подсистемы хранения документов.
    /// </summary>
    internal class MongoDocumentStorageStatusProvider : ISubsystemStatusProvider
    {
        public MongoDocumentStorageStatusProvider(MongoConnection connection, MongoDocumentStorageOptions mongoDocumentStorageOptions)
        {
            _connection = connection;
            _mongoDocumentStorageOptions = mongoDocumentStorageOptions;
        }


        private readonly MongoConnection _connection;
        private readonly MongoDocumentStorageOptions _mongoDocumentStorageOptions;


        public string Name => _mongoDocumentStorageOptions.SectionName;


        public async Task<object> GetStatus(HttpRequest request)
        {
            return await _connection.GetDatabaseStatusAsync();
        }
    }
}