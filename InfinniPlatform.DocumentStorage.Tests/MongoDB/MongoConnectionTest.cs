using System.Threading.Tasks;

using InfinniPlatform.Dynamic;
using InfinniPlatform.Tests;

using MongoDB.Driver;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class MongoConnectionTest
    {
        [Test]
        public void ShouldGetDatabase()
        {
            // Given
            var connection = MongoTestHelpers.GetConnection();

            // When
            var database = connection.GetDatabase();

            // Then
            Assert.IsNotNull(database);
        }

        [Test]
        public async Task ShouldDropDatabaseAsync()
        {
            // Given
            var connection = MongoTestHelpers.GetConnection();

            // When

            var database = connection.GetDatabase();
            var collection1 = database.GetCollection<DynamicDocument>("DropDatabaseCollection1");
            var collection2 = database.GetCollection<DynamicDocument>("DropDatabaseCollection2");

            collection1.InsertMany(new[] { new DynamicDocument(), new DynamicDocument() });
            collection2.InsertMany(new[] { new DynamicDocument(), new DynamicDocument(), new DynamicDocument() });

            var collectionSizeBeforeDrop1 = collection1.Count(Builders<DynamicDocument>.Filter.Empty);
            var collectionSizeBeforeDrop2 = collection2.Count(Builders<DynamicDocument>.Filter.Empty);

            await connection.DropDatabaseAsync();

            var collectionSizeAfterDrop1 = collection1.Count(Builders<DynamicDocument>.Filter.Empty);
            var collectionSizeAfterDrop2 = collection2.Count(Builders<DynamicDocument>.Filter.Empty);

            // Then
            Assert.AreEqual(2, collectionSizeBeforeDrop1);
            Assert.AreEqual(3, collectionSizeBeforeDrop2);
            Assert.AreEqual(0, collectionSizeAfterDrop1);
            Assert.AreEqual(0, collectionSizeAfterDrop2);
        }

        [Test]
        public async Task ShouldReturnsDatabaseStatus()
        {
            // Given
            var connection = MongoTestHelpers.GetConnection();

            // When
            var status = await connection.GetDatabaseStatusAsync();

            // Then
            Assert.IsNotNull(status);
            Assert.AreEqual(MongoTestHelpers.DatabaseName, status["db"]);
        }
    }
}