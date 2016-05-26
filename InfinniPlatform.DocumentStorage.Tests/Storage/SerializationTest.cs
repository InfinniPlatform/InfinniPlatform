using System.Collections.Generic;

using FakeNamespace.DontChange;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;

using MongoDB.Bson.Serialization;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.Tests.Storage
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class SerializationTest
    {
        private const string Id1 = "1bef63c4-bc1b-46b2-b2f2-14efd51a05e9";
        private const string Id2 = "86937120-1550-49a0-bb4b-85a020a29305";
        private const string Id3 = "b7751ed8-5f5a-4b7d-8ddf-6b2bc7effbe1";

        [Test]
        public void Find()
        {
            var storage = DocumentStorageTestHelpers.GetStorage(nameof(SerializationTest));

            Assert.DoesNotThrow(() => storage.Find(f => f.Eq("_id", Id1)).ToList());
            Assert.DoesNotThrow(() => storage.Find(f => f.Eq("_id", Id2)).ToList());
        }

        [Test]
        public void FindAfterClassMapCreation()
        {
            BsonClassMap.RegisterClassMap<Doc>();
            BsonClassMap.RegisterClassMap<C1>();
            BsonClassMap.RegisterClassMap<C2>();
            BsonClassMap.RegisterClassMap<A>();

            var storage = DocumentStorageTestHelpers.GetStorage(nameof(SerializationTest));

            Assert.DoesNotThrow(() => storage.Find(f => f.Eq("_id", Id1)).ToList());
            Assert.DoesNotThrow(() => storage.Find(f => f.Eq("_id", Id2)).ToList());
        }

        [Test]
        public void FindGeneric()
        {
            var storageGeneric = DocumentStorageTestHelpers.GetStorage<Doc>(nameof(SerializationTest));

            Assert.DoesNotThrow(() => storageGeneric.Find(d => d._id == (object)Id3).ToList());
        }

        [Test]
        public void FindGenericAfterClassMapCreation()
        {
//            BsonClassMap.RegisterClassMap<Doc>();
            BsonClassMap.RegisterClassMap<C1>(cmi=>cmi.SetDiscriminatorIsRequired(true));
            BsonClassMap.RegisterClassMap<C2>(cmi => cmi.SetDiscriminatorIsRequired(true));
//            BsonClassMap.RegisterClassMap<A>();

            var storageGeneric = DocumentStorageTestHelpers.GetStorage<Doc>(nameof(SerializationTest));

            Assert.DoesNotThrow(() => { var list = storageGeneric.Find(d => d._id == (object)Id3).ToList(); });
        }

        [Test]
        public void Insert()
        {
            var documentStorageImpl = DocumentStorageTestHelpers.GetEmptyStorage(nameof(SerializationTest));
            var count = documentStorageImpl.Count();

            Assert.AreEqual(0, count, "Не удалось очистить базу перед тестом.");

            var storage = DocumentStorageTestHelpers.GetStorage(nameof(SerializationTest));
            var storageGeneric = DocumentStorageTestHelpers.GetStorage<Doc>(nameof(SerializationTest));

            var dynamicWrapper = new DynamicWrapper
                                 {
                                     ["Property1"] = new A
                                                     {
                                                         List = new List<Base>
                                                                {
                                                                    new C1(),
                                                                    new C2()
                                                                }
                                                     },
                                     ["_id"] = Id1
                                 };
            storage.InsertOne(dynamicWrapper);

            var convertToDynamic = JsonObjectSerializer.Default.ConvertToDynamic(dynamicWrapper);
            var dynamicWrapper1 = JsonObjectSerializer.Default.ConvertFromDynamic<DynamicWrapper>(convertToDynamic);
            dynamicWrapper1["_id"] = Id2;
            storage.InsertOne(dynamicWrapper1);

            var doc = new Doc
                      {
                          _id = Id3,
                          Property1 = new A
                                      {
                                          List = new List<Base>
                                                 {
                                                     new C1(),
                                                     new C2()
                                                 }
                                      }
                      };

            storageGeneric.InsertOne(doc);

            Assert.DoesNotThrow(() => documentStorageImpl.Find(builder => builder.Eq("_id", Id1)).ToList());
            Assert.DoesNotThrow(() => documentStorageImpl.Find(builder => builder.Eq("_id", Id2)).ToList());
            Assert.DoesNotThrow(() => documentStorageImpl.Find(builder => builder.Eq("_id", Id3)).ToList());
            Assert.AreNotEqual(0, documentStorageImpl.Find(builder => builder.Eq("_id", Id1)).Count());
            Assert.AreNotEqual(0, documentStorageImpl.Find(builder => builder.Eq("_id", Id2)).Count());
            Assert.AreNotEqual(0, documentStorageImpl.Find(builder => builder.Eq("_id", Id3)).Count());
        }

        [Test]
        public void Test()
        {
            var documentStorageImpl = DocumentStorageTestHelpers.GetStorage(nameof(SerializationTest));
            var documentFindCursor = documentStorageImpl.Find(f => f.Eq("_id", "5746ba30803e124264bbab3e"));
        }
    }
}