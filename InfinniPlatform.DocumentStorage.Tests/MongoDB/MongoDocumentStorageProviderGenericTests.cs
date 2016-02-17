using InfinniPlatform.DocumentStorage.Tests.TestEntities;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.Tests.MongoDB
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class MongoDocumentStorageProviderGenericTests
    {
        [Test]
        public void ShouldCount()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldCount));

            // When
            storage.InsertMany(new[] { new SimpleEntity { _id = 1 }, new SimpleEntity { _id = 2 }, new SimpleEntity { _id = 3 } });
            var count = storage.Count();

            // Then
            Assert.AreEqual(3, count);
        }

        [Test]
        public async void ShouldCountAsync()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldCountAsync));

            // When
            storage.InsertMany(new[] { new SimpleEntity { _id = 1 }, new SimpleEntity { _id = 2 }, new SimpleEntity { _id = 3 } });
            var count = await storage.CountAsync();

            // Then
            Assert.AreEqual(3, count);
        }
    }
}