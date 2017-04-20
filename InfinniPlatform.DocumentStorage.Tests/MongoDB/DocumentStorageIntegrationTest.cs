using System;
using System.Threading.Tasks;

using InfinniPlatform.Core.Dynamic;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class DocumentStorageIntegrationTest
    {
        [Test]
        public void DocumentStorageCrudTest()
        {
            // Given
            var storage = DocumentStorageTestHelpers.GetEmptyStorage(nameof(DocumentStorageCrudTest));

            // When

            storage.InsertOne(new DynamicWrapper { { "_id", 1 }, { "prop1", 123 }, { "prop2", "abc" } });
            var afterInsert = storage.Find().ToList();

            storage.UpdateOne(u => u.Set("prop3", 456), f => f.Eq("_id", 1));
            var afterUpdate = storage.Find().ToList();

            var doc1 = storage.Find(f => f.Eq("_id", 1)).First();
            doc1["prop1"] = 321;
            doc1["prop2"] = "cba";
            doc1["prop3"] = 654;
            var doc2 = new DynamicWrapper();
            doc2["_id"] = 2;
            doc2["_header"] = null;
            doc2["prop1"] = 789;
            doc2["prop2"] = "xyz";
            doc2["prop3"] = 987;
            storage.ReplaceOne(doc1, f => f.Eq("_id", 1), true);
            storage.ReplaceOne(doc2, f => f.Eq("_id", 2), true);
            var afterReplace = storage.Find().ToList();

            storage.DeleteOne(f => f.Eq("_id", 1));
            var afterDelete = storage.Find().ToList();

            // Then

            Assert.AreEqual(1, afterInsert.Count);
            Assert.AreEqual(1, afterInsert[0]["_id"]);
            Assert.AreEqual(123, afterInsert[0]["prop1"]);
            Assert.AreEqual("abc", afterInsert[0]["prop2"]);
            dynamic headerAfterInsert1 = afterInsert[0]["_header"];
            Assert.IsNotNull(headerAfterInsert1);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeTenant, headerAfterInsert1._tenant);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeUserId, headerAfterInsert1._createUserId);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeUserName, headerAfterInsert1._createUser);
            Assert.Less(Math.Abs((DateTime.UtcNow - (DateTime)headerAfterInsert1._created).TotalMinutes), 1);

            Assert.AreEqual(1, afterUpdate.Count);
            Assert.AreEqual(1, afterUpdate[0]["_id"]);
            Assert.AreEqual(123, afterUpdate[0]["prop1"]);
            Assert.AreEqual("abc", afterUpdate[0]["prop2"]);
            Assert.AreEqual(456, afterUpdate[0]["prop3"]);
            dynamic headerAfterUpdate1 = afterUpdate[0]["_header"];
            Assert.IsNotNull(headerAfterUpdate1);
            Assert.AreEqual(headerAfterInsert1._tenant, headerAfterUpdate1._tenant);
            Assert.AreEqual(headerAfterInsert1._createUserId, headerAfterUpdate1._createUserId);
            Assert.AreEqual(headerAfterInsert1._createUser, headerAfterUpdate1._createUser);
            Assert.Less(Math.Abs(((DateTime)headerAfterInsert1._created - (DateTime)headerAfterUpdate1._created).TotalSeconds), 1);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeUserId, headerAfterUpdate1._updateUserId);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeUserName, headerAfterUpdate1._updateUser);
            Assert.Less(Math.Abs((DateTime.UtcNow - (DateTime)headerAfterUpdate1._updated).TotalMinutes), 1);

            Assert.AreEqual(2, afterReplace.Count);
            Assert.AreEqual(1, afterReplace[0]["_id"]);
            Assert.AreEqual(321, afterReplace[0]["prop1"]);
            Assert.AreEqual("cba", afterReplace[0]["prop2"]);
            Assert.AreEqual(654, afterReplace[0]["prop3"]);
            Assert.AreEqual(2, afterReplace[1]["_id"]);
            Assert.AreEqual(789, afterReplace[1]["prop1"]);
            Assert.AreEqual("xyz", afterReplace[1]["prop2"]);
            Assert.AreEqual(987, afterReplace[1]["prop3"]);
            dynamic headerAfterReplace1 = afterReplace[0]["_header"];
            Assert.IsNotNull(headerAfterReplace1);
            Assert.AreEqual(headerAfterInsert1._tenant, headerAfterReplace1._tenant);
            Assert.AreEqual(headerAfterInsert1._createUserId, headerAfterReplace1._createUserId);
            Assert.AreEqual(headerAfterInsert1._createUser, headerAfterReplace1._createUser);
            Assert.Less(Math.Abs(((DateTime)headerAfterInsert1._created - (DateTime)headerAfterReplace1._created).TotalSeconds), 1);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeUserId, headerAfterReplace1._updateUserId);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeUserName, headerAfterReplace1._updateUser);
            Assert.Less(Math.Abs((DateTime.UtcNow - (DateTime)headerAfterReplace1._updated).TotalMinutes), 1);
            dynamic headerAfterReplace2 = afterReplace[1]["_header"];
            Assert.IsNotNull(headerAfterReplace2);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeTenant, headerAfterReplace1._tenant);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeUserId, headerAfterReplace2._createUserId);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeUserName, headerAfterReplace2._createUser);
            Assert.Less(Math.Abs((DateTime.UtcNow - (DateTime)headerAfterReplace2._created).TotalMinutes), 1);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeUserId, headerAfterReplace2._updateUserId);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeUserName, headerAfterReplace2._updateUser);
            Assert.Less(Math.Abs((DateTime.UtcNow - (DateTime)headerAfterReplace2._updated).TotalMinutes), 1);

            Assert.AreEqual(1, afterDelete.Count);
            Assert.AreEqual(2, afterDelete[0]["_id"]);
        }

        [Test]
        public async Task ShouldNotChangeCreationDate()
        {
            // Given
            var storage = DocumentStorageTestHelpers.GetEmptyStorage(nameof(ShouldNotChangeCreationDate));

            // When

            await storage.SaveOneAsync(new DynamicWrapper { { "_id", 1 } });
            var version1 = await storage.Find(f => f.Eq("_id", 1)).FirstOrDefaultAsync();
            var header1 = (DynamicWrapper)version1["_header"];
            var created1 = (DateTime?)header1["_created"];
            var updated1 = (DateTime?)header1["_updated"];

            await Task.Delay(1000);

            await storage.SaveOneAsync(version1);
            var version2 = await storage.Find(f => f.Eq("_id", 1)).FirstOrDefaultAsync();
            var header2 = (DynamicWrapper)version2["_header"];
            var created2 = (DateTime?)header2["_created"];
            var updated2 = (DateTime?)header2["_updated"];

            // Then

            Assert.IsNotNull(created1);
            Assert.AreEqual(created1, updated1);

            Assert.AreEqual(created1, created2);
            Assert.AreNotEqual(created2, updated2);

            Assert.Less(created2, updated2);
        }
    }
}