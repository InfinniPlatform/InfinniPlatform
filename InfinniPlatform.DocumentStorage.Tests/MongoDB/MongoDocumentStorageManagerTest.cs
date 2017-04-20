using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.DocumentStorage.Metadata;
using InfinniPlatform.Dynamic;

using MongoDB.Driver;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class MongoDocumentStorageManagerTest
    {
        [Test]
        public async Task ShouldCreateStorageAsync()
        {
            // Given

            const string collectionName = nameof(ShouldCreateStorageAsync);

            var connection = MongoTestHelpers.GetConnection();
            var database = connection.GetDatabase();
            database.DropCollection(collectionName);

            var documentMetadata = new DocumentMetadata
                                   {
                                       Type = collectionName,
                                       Indexes = new List<DocumentIndex>
                                                 {
                                                     new DocumentIndex
                                                     {
                                                         Key = new Dictionary<string, DocumentIndexKeyType>
                                                               {
                                                                   { "Id", DocumentIndexKeyType.Asc },
                                                               }
                                                     },
                                                     new DocumentIndex
                                                     {
                                                         Key = new Dictionary<string, DocumentIndexKeyType>
                                                               {
                                                                   { "Name", DocumentIndexKeyType.Asc },
                                                                   { "Birthday", DocumentIndexKeyType.Desc }
                                                               }
                                                     },
                                                     new DocumentIndex
                                                     {
                                                         Key = new Dictionary<string, DocumentIndexKeyType>
                                                               {
                                                                   { "Description", DocumentIndexKeyType.Text }
                                                               }
                                                     }
                                                 }
                                   };

            // When - Create Collection

            var storageManager = new MongoDocumentStorageManager(connection);
            await storageManager.CreateStorageAsync(documentMetadata);
            await storageManager.CreateStorageAsync(documentMetadata); // No effect

            var collection = connection.GetDatabase().GetCollection<DynamicWrapper>(collectionName);

            // When - Using Indexes

            collection.InsertMany(new[]
                                  {
                                      CreateTestObject(1, "Name1", DateTime.Today.AddHours(1), "Some description 1 for Name1"),
                                      CreateTestObject(2, "Name1", DateTime.Today.AddHours(2), "Some description 2 for Name1"),
                                      CreateTestObject(3, "Name2", DateTime.Today.AddHours(3), "Some description 3 for Name2"),
                                      CreateTestObject(4, "Name2", DateTime.Today.AddHours(4), "Some description 4 for Name2"),
                                      CreateTestObject(5, "Name2", DateTime.Today.AddHours(5), "Some description 5 for Name2"),
                                      CreateTestObject(6, "Name3", DateTime.Today.AddHours(6), "Some description 6 for Name3")
                                  });

            var filterBuilder = Builders<DynamicWrapper>.Filter;
            var resultIndex1 = collection.FindSync(filterBuilder.Eq("Id", 3)).ToList();
            var resultIndex2 = collection.FindSync(filterBuilder.And(filterBuilder.Eq("Name", "Name2"), filterBuilder.Lt("Birthday", DateTime.Today.AddHours(5)))).ToList();
            var resultIndex3 = collection.FindSync(filterBuilder.Text("some name2")).ToList();

            // Then

            // Id: Ascending
            Assert.AreEqual(1, resultIndex1.Count);
            Assert.AreEqual(3, resultIndex1[0]["Id"]);
            Assert.AreEqual("Name2", resultIndex1[0]["Name"]);
            Assert.AreEqual(DateTime.Today.AddHours(3).ToUniversalTime(), resultIndex1[0]["Birthday"]);
            Assert.AreEqual("Some description 3 for Name2", resultIndex1[0]["Description"]);

            // Name: Ascending, Birthday: Descending
            Assert.AreEqual(2, resultIndex2.Count);
            Assert.AreEqual(4, resultIndex2[0]["Id"]);
            Assert.AreEqual("Name2", resultIndex2[0]["Name"]);
            Assert.AreEqual(DateTime.Today.AddHours(4).ToUniversalTime(), resultIndex2[0]["Birthday"]);
            Assert.AreEqual("Some description 4 for Name2", resultIndex2[0]["Description"]);
            Assert.AreEqual(3, resultIndex2[1]["Id"]);
            Assert.AreEqual("Name2", resultIndex2[1]["Name"]);
            Assert.AreEqual(DateTime.Today.AddHours(3).ToUniversalTime(), resultIndex2[1]["Birthday"]);
            Assert.AreEqual("Some description 3 for Name2", resultIndex2[1]["Description"]);

            // Description: Text
            Assert.AreEqual(3, resultIndex3.Count);
            CollectionAssert.AreEquivalent(new[] { 3, 4, 5 }, new[] { resultIndex3[0]["Id"], resultIndex3[1]["Id"], resultIndex3[2]["Id"] });
        }

        [Test]
        public async Task ShouldRenameStorageAsync()
        {
            // Given

            const string collectionName = nameof(ShouldRenameStorageAsync);
            const string collectionNameNew = nameof(ShouldRenameStorageAsync) + "New";

            var connection = MongoTestHelpers.GetConnection();
            var database = connection.GetDatabase();
            database.DropCollection(collectionName);
            database.DropCollection(collectionNameNew);

            var documentMetadata = new DocumentMetadata { Type = collectionName };

            // When

            var storageManager = new MongoDocumentStorageManager(connection);
            await storageManager.CreateStorageAsync(documentMetadata);

            var collection = database.GetCollection<DynamicWrapper>(collectionName);
            var collectionNew = database.GetCollection<DynamicWrapper>(collectionNameNew);
            var collectionContent = new[] { new DynamicWrapper(), new DynamicWrapper(), new DynamicWrapper() };
            collection.InsertMany(collectionContent);

            var collectionCountBeforeRename = collection.Count(Builders<DynamicWrapper>.Filter.Empty);
            var collectionNewCountBeforeRename = collectionNew.Count(Builders<DynamicWrapper>.Filter.Empty);

            await storageManager.RenameStorageAsync(collectionName, collectionNameNew);

            var collectionCountAfterRename = collection.Count(Builders<DynamicWrapper>.Filter.Empty);
            var collectionNewCountAfterRename = collectionNew.Count(Builders<DynamicWrapper>.Filter.Empty);

            // Then
            Assert.AreEqual(collectionContent.Length, collectionCountBeforeRename);
            Assert.AreEqual(0, collectionNewCountBeforeRename);
            Assert.AreEqual(0, collectionCountAfterRename);
            Assert.AreEqual(collectionContent.Length, collectionNewCountAfterRename);
        }

        [Test]
        public async Task ShouldDropStorageAsync()
        {
            // Given

            const string collectionName = nameof(ShouldDropStorageAsync);

            var connection = MongoTestHelpers.GetConnection();
            var database = connection.GetDatabase();
            database.DropCollection(collectionName);

            var documentMetadata = new DocumentMetadata { Type = collectionName };

            // When

            var storageManager = new MongoDocumentStorageManager(connection);
            await storageManager.CreateStorageAsync(documentMetadata);

            var collection = database.GetCollection<DynamicWrapper>(collectionName);
            var collectionContent = new[] { new DynamicWrapper(), new DynamicWrapper(), new DynamicWrapper() };
            collection.InsertMany(collectionContent);

            var collectionCountBeforeDrop = collection.Count(Builders<DynamicWrapper>.Filter.Empty);

            await storageManager.DropStorageAsync(collectionName);

            var collectionCountAfterDrop = collection.Count(Builders<DynamicWrapper>.Filter.Empty);

            // Then
            Assert.AreEqual(collectionContent.Length, collectionCountBeforeDrop);
            Assert.AreEqual(0, collectionCountAfterDrop);
        }

        private static DynamicWrapper CreateTestObject(int id, string name, DateTime birthday, string description)
        {
            return new DynamicWrapper
                   {
                       { "Id", id },
                       { "Name", name },
                       { "Birthday", birthday },
                       { "Description", description }
                   };
        }
    }
}