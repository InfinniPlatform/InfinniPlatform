using System.Collections.Generic;

using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.Tests.MongoDB
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    [Ignore("Tests need to run in order with context reset.")]
    public class SerializationTest
    {
        private static readonly DynamicWrapper DynamicDoc = new DynamicWrapper
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

        private static readonly Doc StrongTypeDoc = new Doc
                                                    {
                                                        _id = Id2,
                                                        Property1 = new A
                                                                    {
                                                                        List = new List<Base>
                                                                               {
                                                                                   new C1(),
                                                                                   new C2()
                                                                               }
                                                                    }
                                                    };

        private const string Id1 = "1bef63c4-bc1b-46b2-b2f2-14efd51a05e9";
        private const string Id2 = "b7751ed8-5f5a-4b7d-8ddf-6b2bc7effbe1";

        [Test]
        public void Find()
        {
            var sourceMock = new Mock<IDocumentKnownTypeSource>();
            sourceMock.Setup(source => source.KnownTypes)
                      .Returns(new[] { typeof(C1), typeof(C2) });

            var storage = MongoTestHelpers.GetStorageProvider(nameof(SerializationTest), source: sourceMock.Object);

            Assert.DoesNotThrow(() => storage.Find(f => f.Eq("_id", Id1)).ToList());
            Assert.DoesNotThrow(() => storage.Find(f => f.Eq("_id", Id2)).ToList());
        }

        [Test]
        public void FindGeneric()
        {
            var sourceMock = new Mock<IDocumentKnownTypeSource>();
            sourceMock.Setup(source => source.KnownTypes)
                      .Returns(new[] { typeof(C1), typeof(C2) });

            var storageGeneric = MongoTestHelpers.GetStorageProvider<Doc>(nameof(SerializationTest), source: sourceMock.Object);

            Assert.DoesNotThrow(() => storageGeneric.Find(d => d._id == (object)Id1).ToList());
            Assert.DoesNotThrow(() => storageGeneric.Find(d => d._id == (object)Id2).ToList());
        }

        [Test]
        public void Insert()
        {
            var sourceMock = new Mock<IDocumentKnownTypeSource>();
            sourceMock.Setup(source => source.KnownTypes).Returns(new[] { typeof(C1), typeof(C2) });

            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(SerializationTest), source: sourceMock.Object);
            var storageGeneric = MongoTestHelpers.GetEmptyStorageProvider<Doc>(nameof(SerializationTest), source: sourceMock.Object);

            storage.InsertOne(DynamicDoc);

            storageGeneric.InsertOne(StrongTypeDoc);

            Assert.DoesNotThrow(() => { var list = storage.Find(builder => builder.Eq("_id", Id1)).ToList(); });
            Assert.DoesNotThrow(() => { var list = storage.Find(builder => builder.Eq("_id", Id2)).ToList(); });
            Assert.AreNotEqual(0, storage.Find(builder => builder.Eq("_id", Id1)).Count());
            Assert.AreNotEqual(0, storage.Find(builder => builder.Eq("_id", Id2)).Count());
        }

        [Test]
        public void TestreformagkhBug()
        {
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(SerializationTest));
            var cursor = storage.Find(f => f.Eq("_id", "5746ba30803e124264bbab3e"));
        }
    }
}