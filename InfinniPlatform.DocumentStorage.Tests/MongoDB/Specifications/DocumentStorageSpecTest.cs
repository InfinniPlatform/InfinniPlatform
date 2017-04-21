using InfinniPlatform.DocumentStorage.Specifications;
using InfinniPlatform.Dynamic;
using InfinniPlatform.Tests;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.MongoDB.Specifications
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class DocumentStorageSpecTest
    {
        [Test]
        public void ShouldFindWithSpecification()
        {
            // Given

            var storage = DocumentStorageTestHelpers.GetEmptyStorage(nameof(ShouldFindWithSpecification));

            storage.InsertMany(new[]
                               {
                                   new DynamicWrapper { { "_id", 1 }, { "prop1", 11 } },
                                   new DynamicWrapper { { "_id", 2 }, { "prop1", 22 } },
                                   new DynamicWrapper { { "_id", 3 }, { "prop1", 33 } },
                                   new DynamicWrapper { { "_id", 4 }, { "prop1", 44 } },
                                   new DynamicWrapper { { "_id", 5 }, { "prop1", 55 } }
                               });

            var specification = new Specification(f => f.Gt("prop1", 33));

            // When
            var result = storage.Find(specification).ToList();

            // Then
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(4, result[0]["_id"]);
            Assert.AreEqual(5, result[1]["_id"]);
        }

        [Test]
        public void ShouldFindWithCustomSpecification()
        {
            // Given

            var storage = DocumentStorageTestHelpers.GetEmptyStorage(nameof(ShouldFindWithCustomSpecification));

            storage.InsertMany(new[]
                               {
                                   new DynamicWrapper { { "_id", 1 }, { "prop1", 11 } },
                                   new DynamicWrapper { { "_id", 2 }, { "prop1", 22 } },
                                   new DynamicWrapper { { "_id", 3 }, { "prop1", 33 } },
                                   new DynamicWrapper { { "_id", 4 }, { "prop1", 44 } },
                                   new DynamicWrapper { { "_id", 5 }, { "prop1", 55 } }
                               });

            var specification = new MySpecification(33);

            // When
            var result = storage.Find(specification).ToList();

            // Then
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(4, result[0]["_id"]);
            Assert.AreEqual(5, result[1]["_id"]);
        }

        [Test]
        public void ShouldSupportMockingSpecification()
        {
            // Given
            var storageMock = new Mock<IDocumentStorage>();
            var specification = new Specification(f => f.Gt("prop1", 33));

            // When
            storageMock.Object.Find(specification);

            // Then
            storageMock.Verify(i => i.Find(specification), Times.Once);
        }


        class MySpecification : Specification
        {
            public MySpecification(int prop1) : base(f => f.Gt("prop1", prop1))
            {
            }
        }
    }
}