using System;
using System.Threading.Tasks;

using InfinniPlatform.Core.Abstractions.Dynamic;
using InfinniPlatform.DocumentStorage.Abstractions;
using InfinniPlatform.DocumentStorage.MongoDB;
using InfinniPlatform.DocumentStorage.MongoDB.Transactions;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.Tests.MongoDB.Transactions
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class UnitOfWorkTest
    {
        [Test]
        public void ShouldBeginChildrenWork()
        {
            // Given
            var parentUnit = CreateUnitOfWork();

            // When
            var work1 = parentUnit.Begin();
            var work2 = parentUnit.Begin();
            var work3 = parentUnit.Begin();

            // Then
            Assert.AreEqual(parentUnit, work1);
            Assert.AreNotEqual(parentUnit, work2);
            Assert.AreNotEqual(parentUnit, work3);
            Assert.AreNotEqual(work2, work3);
        }

        [Test]
        public void ShouldBeginNewWorkAfterDispose()
        {
            // Given
            var parentUnit = CreateUnitOfWork();

            // When

            var work11 = parentUnit.Begin();
            var work12 = parentUnit.Begin();
            var work13 = parentUnit.Begin();

            parentUnit.Dispose();

            var work21 = parentUnit.Begin();
            var work22 = parentUnit.Begin();
            var work23 = parentUnit.Begin();

            // Then

            Assert.AreEqual(parentUnit, work11);
            Assert.AreNotEqual(parentUnit, work12);
            Assert.AreNotEqual(parentUnit, work13);
            Assert.AreNotEqual(work12, work13);

            Assert.AreEqual(parentUnit, work21);
            Assert.AreNotEqual(parentUnit, work22);
            Assert.AreNotEqual(parentUnit, work23);
            Assert.AreNotEqual(work22, work23);
        }

        [Test]
        public void ShouldCommit()
        {
            // Given

            Mock<IDocumentBulkBuilder> bulkBuilder;
            var parentUnit = CreateUnitOfWork(out bulkBuilder);

            var document1 = new DynamicWrapper();
            var document2 = new DynamicWrapper();
            var document3 = new DynamicWrapper();
            var document4 = new DynamicWrapper();

            // When

            parentUnit.Begin(); // begin parent work
            var childWork1 = parentUnit.Begin(); // begin child work 1
            var childWork2 = parentUnit.Begin(); // begin child work 2

            childWork1.InsertOne("documentType1", document1);
            childWork1.InsertOne("documentType2", document2);

            childWork2.InsertOne("documentType1", document3);
            childWork2.InsertOne("documentType2", document4);

            childWork1.Commit(); // commit to parent
            childWork2.Commit(); // commit to parent

            bulkBuilder.Verify(i => i.InsertOne(It.IsAny<DynamicWrapper>()), Times.Never());

            parentUnit.Commit(); // commit to storage

            // Then

            bulkBuilder.Verify(i => i.InsertOne(It.Is<DynamicWrapper>(it => ReferenceEquals(it, document1))), Times.Once);
            bulkBuilder.Verify(i => i.InsertOne(It.Is<DynamicWrapper>(it => ReferenceEquals(it, document2))), Times.Once);
            bulkBuilder.Verify(i => i.InsertOne(It.Is<DynamicWrapper>(it => ReferenceEquals(it, document3))), Times.Once);
            bulkBuilder.Verify(i => i.InsertOne(It.Is<DynamicWrapper>(it => ReferenceEquals(it, document4))), Times.Once);
        }

        [Test]
        public async Task ShouldCommitAsync()
        {
            // Given

            Mock<IDocumentBulkBuilder> bulkBuilder;
            var parentUnit = CreateUnitOfWork(out bulkBuilder);

            var document1 = new DynamicWrapper();
            var document2 = new DynamicWrapper();
            var document3 = new DynamicWrapper();
            var document4 = new DynamicWrapper();

            // When

            parentUnit.Begin(); // begin parent work
            var childWork1 = parentUnit.Begin(); // begin child work 1
            var childWork2 = parentUnit.Begin(); // begin child work 2

            childWork1.InsertOne("documentType1", document1);
            childWork1.InsertOne("documentType2", document2);

            childWork2.InsertOne("documentType1", document3);
            childWork2.InsertOne("documentType2", document4);

            childWork1.Commit(); // commit to parent
            childWork2.Commit(); // commit to parent

            bulkBuilder.Verify(i => i.InsertOne(It.IsAny<DynamicWrapper>()), Times.Never());

            await parentUnit.CommitAsync(); // commit to storage

            // Then

            bulkBuilder.Verify(i => i.InsertOne(It.Is<DynamicWrapper>(it => ReferenceEquals(it, document1))), Times.Once);
            bulkBuilder.Verify(i => i.InsertOne(It.Is<DynamicWrapper>(it => ReferenceEquals(it, document2))), Times.Once);
            bulkBuilder.Verify(i => i.InsertOne(It.Is<DynamicWrapper>(it => ReferenceEquals(it, document3))), Times.Once);
            bulkBuilder.Verify(i => i.InsertOne(It.Is<DynamicWrapper>(it => ReferenceEquals(it, document4))), Times.Once);
        }


        private static UnitOfWork CreateUnitOfWork()
        {
            Mock<IDocumentBulkBuilder> bulkBuilder;
            return new UnitOfWork(CreateStorageFactory(out bulkBuilder));
        }

        private static UnitOfWork CreateUnitOfWork(out Mock<IDocumentBulkBuilder> bulkBuilder)
        {
            return new UnitOfWork(CreateStorageFactory(out bulkBuilder));
        }


        private static IDocumentStorageFactory CreateStorageFactory(out Mock<IDocumentBulkBuilder> bulkBuilder)
        {
            var storageFactory = new Mock<IDocumentStorageFactory>();

            var storage = new Mock<IDocumentStorage>();
            var bulkExecutor = storage.As<IDocumentStorageBulkExecutor>();
            var bulkBuilderResult = new Mock<IDocumentBulkBuilder>();

            storageFactory.Setup(i => i.GetStorage(It.IsAny<string>()))
                          .Returns(storage.Object);

            bulkExecutor.Setup(i => i.Bulk(It.IsAny<Action<object>>(), It.IsAny<bool>()))
                        .Callback((Action<object> bulk, bool isOrdered) => bulk(bulkBuilderResult.Object))
                        .Returns(DocumentBulkResult.Empty);

            bulkExecutor.Setup(i => i.BulkAsync(It.IsAny<Action<object>>(), It.IsAny<bool>()))
                        .Callback((Action<object> bulk, bool isOrdered) => bulk(bulkBuilderResult.Object))
                        .Returns(Task.FromResult(DocumentBulkResult.Empty));

            bulkBuilder = bulkBuilderResult;

            return storageFactory.Object;
        }
    }
}