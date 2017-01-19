using InfinniPlatform.DocumentStorage.Contract;
using InfinniPlatform.DocumentStorage.Contract.Specifications;
using InfinniPlatform.DocumentStorage.Tests.Storage;
using InfinniPlatform.DocumentStorage.Tests.TestEntities;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.Tests.Specifications
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class DocumentStorageGenericSpecTest
    {
        [Test]
        public void ShouldFindWithSpecification()
        {
            // Given

            var storage = DocumentStorageTestHelpers.GetEmptyStorage<FakeDocument>(nameof(ShouldFindWithSpecification));

            storage.InsertMany(new[]
                               {
                                   new FakeDocument { _id = 1, prop1 = 11 },
                                   new FakeDocument { _id = 2, prop1 = 22 },
                                   new FakeDocument { _id = 3, prop1 = 33 },
                                   new FakeDocument { _id = 4, prop1 = 44 },
                                   new FakeDocument { _id = 5, prop1 = 55 }
                               });

            var specification = new Specification<FakeDocument>(i => i.prop1 > 33);

            // When
            var result = storage.Find(specification).ToList();

            // Then
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(4, result[0]._id);
            Assert.AreEqual(5, result[1]._id);
        }

        [Test]
        public void ShouldFindWithCustomSpecification()
        {
            // Given

            var storage = DocumentStorageTestHelpers.GetEmptyStorage<FakeDocument>(nameof(ShouldFindWithCustomSpecification));

            storage.InsertMany(new[]
                               {
                                   new FakeDocument { _id = 1, prop1 = 11 },
                                   new FakeDocument { _id = 2, prop1 = 22 },
                                   new FakeDocument { _id = 3, prop1 = 33 },
                                   new FakeDocument { _id = 4, prop1 = 44 },
                                   new FakeDocument { _id = 5, prop1 = 55 }
                               });

            var specification = new MySpecification(33);

            // When
            var result = storage.Find(specification).ToList();

            // Then
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(4, result[0]._id);
            Assert.AreEqual(5, result[1]._id);
        }

        [Test]
        public void ShouldSupportMockingSpecification()
        {
            // Given
            var storageMock = new Mock<IDocumentStorage<FakeDocument>>();
            var specification = new Specification<FakeDocument>(i => i.prop1 > 33);

            // When
            storageMock.Object.Find(specification);

            // Then
            storageMock.Verify(i => i.Find(specification), Times.Once);
        }


        class MySpecification : Specification<FakeDocument>
        {
            public MySpecification(int prop1) : base(i => i.prop1 > prop1)
            {
            }
        }
    }
}