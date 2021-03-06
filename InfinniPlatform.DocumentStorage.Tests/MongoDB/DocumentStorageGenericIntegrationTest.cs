﻿using System;
using System.Threading.Tasks;

using InfinniPlatform.DocumentStorage.TestEntities;
using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class DocumentStorageGenericIntegrationTest
    {
        [Test]
        public void DocumentStorageGenericCrudTest()
        {
            // Given
            var storage = DocumentStorageTestHelpers.GetEmptyStorage<FakeDocument>(nameof(DocumentStorageGenericCrudTest));

            // When

            storage.InsertOne(new FakeDocument { _id = 1, prop1 = 123, prop2 = "abc" });
            var afterInsert = storage.Find().ToList();

            storage.UpdateOne(u => u.Set(i => i.prop3, 456), i => i._id == (object)1);
            var afterUpdate = storage.Find().ToList();

            var doc1 = storage.Find(i => i._id == (object)1).First();
            doc1.prop1 = 321;
            doc1.prop2 = "cba";
            doc1.prop3 = 654;
            var doc2 = new FakeDocument();
            doc2._id = 2;
            doc2.prop1 = 789;
            doc2.prop2 = "xyz";
            doc2.prop3 = 987;
            storage.ReplaceOne(doc1, i => i._id == (object)1, true);
            storage.ReplaceOne(doc2, i => i._id == (object)2, true);
            var afterReplace = storage.Find().ToList();

            storage.DeleteOne(i => i._id == (object)1);
            var afterDelete = storage.Find().ToList();

            // Then

            Assert.AreEqual(1, afterInsert.Count);
            Assert.AreEqual(1, afterInsert[0]._id);
            Assert.AreEqual(123, afterInsert[0].prop1);
            Assert.AreEqual("abc", afterInsert[0].prop2);
            var headerAfterInsert1 = afterInsert[0]._header;
            Assert.IsNotNull(headerAfterInsert1);
            Assert.IsNotNull(headerAfterInsert1._created);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeTenant, headerAfterInsert1._tenant);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeUserId, headerAfterInsert1._createUserId);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeUserName, headerAfterInsert1._createUser);
            Assert.Less(Math.Abs((DateTime.UtcNow - headerAfterInsert1._created.Value).TotalMinutes), 1);

            Assert.AreEqual(1, afterUpdate.Count);
            Assert.AreEqual(1, afterUpdate[0]._id);
            Assert.AreEqual(123, afterUpdate[0].prop1);
            Assert.AreEqual("abc", afterUpdate[0].prop2);
            Assert.AreEqual(456, afterUpdate[0].prop3);
            var headerAfterUpdate1 = afterUpdate[0]._header;
            Assert.IsNotNull(headerAfterUpdate1);
            Assert.IsNotNull(headerAfterUpdate1._created);
            Assert.IsNotNull(headerAfterUpdate1._updated);
            Assert.AreEqual(headerAfterInsert1._tenant, headerAfterUpdate1._tenant);
            Assert.AreEqual(headerAfterInsert1._createUserId, headerAfterUpdate1._createUserId);
            Assert.AreEqual(headerAfterInsert1._createUser, headerAfterUpdate1._createUser);
            Assert.Less(Math.Abs(((DateTime)headerAfterInsert1._created - headerAfterUpdate1._created.Value).TotalSeconds), 1);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeUserId, headerAfterUpdate1._updateUserId);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeUserName, headerAfterUpdate1._updateUser);
            Assert.Less(Math.Abs((DateTime.UtcNow - headerAfterUpdate1._updated.Value).TotalMinutes), 1);

            Assert.AreEqual(2, afterReplace.Count);
            Assert.AreEqual(1, afterReplace[0]._id);
            Assert.AreEqual(321, afterReplace[0].prop1);
            Assert.AreEqual("cba", afterReplace[0].prop2);
            Assert.AreEqual(654, afterReplace[0].prop3);
            Assert.AreEqual(2, afterReplace[1]._id);
            Assert.AreEqual(789, afterReplace[1].prop1);
            Assert.AreEqual("xyz", afterReplace[1].prop2);
            Assert.AreEqual(987, afterReplace[1].prop3);
            var headerAfterReplace1 = afterReplace[0]._header;
            Assert.IsNotNull(headerAfterReplace1);
            Assert.IsNotNull(headerAfterReplace1._created);
            Assert.IsNotNull(headerAfterReplace1._updated);
            Assert.AreEqual(headerAfterInsert1._tenant, headerAfterReplace1._tenant);
            Assert.AreEqual(headerAfterInsert1._createUserId, headerAfterReplace1._createUserId);
            Assert.AreEqual(headerAfterInsert1._createUser, headerAfterReplace1._createUser);
            Assert.Less(Math.Abs(((DateTime)headerAfterInsert1._created - headerAfterReplace1._created.Value).TotalSeconds), 1);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeUserId, headerAfterReplace1._updateUserId);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeUserName, headerAfterReplace1._updateUser);
            Assert.Less(Math.Abs((DateTime.UtcNow - headerAfterReplace1._updated.Value).TotalMinutes), 1);
            var headerAfterReplace2 = afterReplace[1]._header;
            Assert.IsNotNull(headerAfterReplace2);
            Assert.IsNotNull(headerAfterReplace2._created);
            Assert.IsNotNull(headerAfterReplace2._updated);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeTenant, headerAfterReplace1._tenant);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeUserId, headerAfterReplace2._createUserId);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeUserName, headerAfterReplace2._createUser);
            Assert.Less(Math.Abs((DateTime.UtcNow - headerAfterReplace2._created.Value).TotalMinutes), 1);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeUserId, headerAfterReplace2._updateUserId);
            Assert.AreEqual(DocumentStorageTestHelpers.FakeUserName, headerAfterReplace2._updateUser);
            Assert.Less(Math.Abs((DateTime.UtcNow - headerAfterReplace2._updated.Value).TotalMinutes), 1);

            Assert.AreEqual(1, afterDelete.Count);
            Assert.AreEqual(2, afterDelete[0]._id);
        }

        [Test]
        public void ShouldUseDocumentTypeAttribute()
        {
            // Given

            var value1 = Guid.NewGuid().ToString();
            var value2 = Guid.NewGuid().ToString();

            var storageWithSpecifiedName = DocumentStorageTestHelpers.GetEmptyStorage<FakeDocument>("FakeDocumentCollection");
            var storageWithDefaultName = DocumentStorageTestHelpers.GetStorage<FakeDocument>();

            // When

            storageWithSpecifiedName.InsertOne(new FakeDocument { _id = 1, prop2 = value1 });
            storageWithDefaultName.InsertOne(new FakeDocument { _id = 2, prop2 = value2 });

            var storageWithSpecifiedNameDocuments = storageWithDefaultName.Find().ToList();
            var storageWithDefaultNameDocuments = storageWithDefaultName.Find().ToList();

            // Then

            Assert.AreEqual(2, storageWithSpecifiedNameDocuments.Count);
            Assert.AreEqual(1, storageWithSpecifiedNameDocuments[0]._id);
            Assert.AreEqual(value1, storageWithSpecifiedNameDocuments[0].prop2);
            Assert.AreEqual(2, storageWithSpecifiedNameDocuments[1]._id);
            Assert.AreEqual(value2, storageWithSpecifiedNameDocuments[1].prop2);

            Assert.AreEqual(2, storageWithDefaultNameDocuments.Count);
            Assert.AreEqual(1, storageWithDefaultNameDocuments[0]._id);
            Assert.AreEqual(value1, storageWithDefaultNameDocuments[0].prop2);
            Assert.AreEqual(2, storageWithDefaultNameDocuments[1]._id);
            Assert.AreEqual(value2, storageWithDefaultNameDocuments[1].prop2);
        }

        [Test]
        public async Task ShouldNotChangeCreationDate()
        {
            // Given
            var storage = DocumentStorageTestHelpers.GetEmptyStorage<FakeDocument>(nameof(DocumentStorageGenericCrudTest));

            // When

            await storage.SaveOneAsync(new FakeDocument { _id = 1 });
            var version1 = await storage.Find(i => i._id == (object)1).FirstOrDefaultAsync();
            var header1 = version1._header;
            var created1 = header1._created;
            var updated1 = header1._updated;

            await Task.Delay(1000);

            await storage.SaveOneAsync(version1);
            var version2 = await storage.Find(i => i._id == (object)1).FirstOrDefaultAsync();
            var header2 = version2._header;
            var created2 = header2._created;
            var updated2 = header2._updated;

            // Then

            Assert.IsNotNull(created1);
            Assert.AreEqual(created1, updated1);

            Assert.AreEqual(created1, created2);
            Assert.AreNotEqual(created2, updated2);

            Assert.Less(created2, updated2);
        }
    }
}