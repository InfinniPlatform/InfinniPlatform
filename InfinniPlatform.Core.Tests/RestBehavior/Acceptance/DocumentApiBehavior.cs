using System;
using System.Diagnostics;
using System.Linq;

using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.RestApi;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class DocumentApiBehavior
    {
        private const string DocumentType = "TestDocument";

        private IDisposable _server;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _server = InfinniPlatformInprocessHost.Start();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _server.Dispose();
        }

        [Test]
        //[Ignore("Manual - Simplification SetDocument()")]
        public void SimplificationSetDocument()
        {
            // Given

            var stopwatch = new Stopwatch();
            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);

            var document = new
            {
                Id = Guid.NewGuid().ToString(),
                TestProperty = "TestValue"
            };

            // When

            documentApi.SetDocument(DocumentType, document);

            stopwatch.Restart();
            documentApi.SetDocument(DocumentType, document);
            stopwatch.Stop();
            Console.WriteLine(@"SetDocument: {0}", stopwatch.Elapsed);

            var afterSave = documentApi.GetDocument(DocumentType, f => f.AddCriteria(c => c.Property("Id").IsEquals(document.Id)), 0, 1);

            documentApi.DeleteDocument(DocumentType, document.Id);

            var afterDelete = documentApi.GetDocument(DocumentType, f => f.AddCriteria(c => c.Property("Id").IsEquals(document.Id)), 0, 1);

            // Then

            Assert.IsNotNull(afterSave);
            Assert.IsNotEmpty(afterSave);
            Assert.IsNotNull(afterSave.FirstOrDefault());
            Assert.AreEqual(document.Id, afterSave.FirstOrDefault().Id);

            Assert.IsNotNull(afterDelete);
            Assert.AreEqual(0, afterDelete.Count());
        }

        [Test]
        public void ShouldDeleteDocument()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);

            var document = new
            {
                Id = Guid.NewGuid().ToString(),
                TestProperty = "TestValue"
            };

            // When

            documentApi.SetDocument(DocumentType, document);

            var afterSave = documentApi.GetDocument(DocumentType, f => f.AddCriteria(c => c.Property("Id").IsEquals(document.Id)), 0, 1);

            documentApi.DeleteDocument(DocumentType, document.Id);

            var afterDelete = documentApi.GetDocument(DocumentType, f => f.AddCriteria(c => c.Property("Id").IsEquals(document.Id)), 0, 1);

            // Then

            Assert.IsNotNull(afterSave);
            Assert.IsNotEmpty(afterSave);
            Assert.IsNotNull(afterSave.FirstOrDefault());
            Assert.AreEqual(document.Id, afterSave.FirstOrDefault().Id);

            Assert.IsNotNull(afterDelete);
            Assert.AreEqual(0, afterDelete.Count());
        }

        [Test]
        public void ShouldGetDocuments()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);

            var testProperty = Guid.NewGuid().ToString();

            var document = new
            {
                Id = Guid.NewGuid().ToString(),
                TestProperty = testProperty
            };

            // When

            documentApi.SetDocument(DocumentType, document);

            var items = documentApi.GetDocument(DocumentType, filter => filter.AddCriteria(cr => cr.Property("TestProperty").IsEquals(testProperty)), 0, 10);

            // Then

            Assert.IsNotNull(items);
            Assert.AreEqual(1, items.Count());

            var document1 = items.ElementAt(0);
            Assert.IsNotNull(document1);
            Assert.AreEqual(document.Id, document1.Id);
            Assert.AreEqual(document.TestProperty, document1.TestProperty);
        }

        [Test]
        public void ShouldGetNumberOfDocuments()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);

            // When

            var testProperty1 = Guid.NewGuid().ToString();

            for (var i = 0; i < 3; ++i)
            {
                var document = new
                {
                    Id = Guid.NewGuid().ToString(),
                    TestProperty = testProperty1
                };

                documentApi.SetDocument(DocumentType, document);
            }

            var testProperty2 = Guid.NewGuid().ToString();

            for (var i = 0; i < 7; ++i)
            {
                var document = new
                {
                    Id = Guid.NewGuid().ToString(),
                    TestProperty = testProperty2
                };

                documentApi.SetDocument(DocumentType, document);
            }

            var count1 = documentApi.GetNumberOfDocuments(DocumentType, filter => filter.AddCriteria(cr => cr.Property("TestProperty").IsEquals(testProperty1)));
            var count2 = documentApi.GetNumberOfDocuments(DocumentType, filter => filter.AddCriteria(cr => cr.Property("TestProperty").IsEquals(testProperty2)));

            // Then

            Assert.AreEqual(3, count1);
            Assert.AreEqual(7, count2);
        }

        [Test]
        public void ShouldReturnCorrectMessageAfterSetDocumentWithIncorrectSchema()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);

            var rightDocument1 = new
            {
                Id = Guid.NewGuid().ToString(),
                TestProperty = "StringValue1",
                ComplexArray = new[] { new { ValidProperty = 11 } }
            };

            var rightDocument2 = new
            {
                Id = Guid.NewGuid().ToString(),
                TestProperty = "StringValue2",
                ComplexArray = new[] { new { ValidProperty = 11 } }
            };

            var badDocument = new
            {
                Id = Guid.NewGuid().ToString(),
                TestProperty = 2,
                ComplexObject = DateTime.Now
            };

            // When

            documentApi.SetDocument(DocumentType, rightDocument1);
            documentApi.SetDocument(DocumentType, rightDocument2);
            var error = Assert.Catch(() => documentApi.SetDocument(DocumentType, badDocument));

            // Then

            Assert.IsTrue(error.Message.Contains("Cannot complete transaction"));
        }

        [Test]
        public void ShouldSaveDocumentWithArrayInlineObjects()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);

            var testProperty = Guid.NewGuid().ToString();

            var documents = new[]
                            {
                                new
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    TestProperty = testProperty,
                                    ComplexArray = new[] { new { ValidProperty = 11 } }
                                },
                                new
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    TestProperty = testProperty,
                                    ComplexArray = new[] { new { ValidProperty = 21 } }
                                }
                            };

            // When

            documentApi.SetDocuments(DocumentType, documents);

            var items = documentApi.GetDocument(DocumentType, f => f.AddCriteria(c => c.Property("TestProperty").IsEquals(testProperty)), 0, 10);

            // Then

            Assert.IsNotNull(items);
            Assert.AreEqual(2, items.Count());

            var document1 = items.FirstOrDefault(i => i.Id == documents[0].Id);
            Assert.IsNotNull(document1);
            Assert.AreEqual(documents[0].TestProperty, document1.TestProperty);
            Assert.IsNotNull(document1.ComplexArray);
            Assert.AreEqual(1, Enumerable.Count(document1.ComplexArray));
            Assert.IsNotNull(Enumerable.ElementAt(document1.ComplexArray, 0));
            Assert.AreEqual(documents[0].ComplexArray[0].ValidProperty, Enumerable.ElementAt(document1.ComplexArray, 0).ValidProperty);

            var document2 = items.FirstOrDefault(i => i.Id == documents[1].Id);
            Assert.IsNotNull(document2);
            Assert.AreEqual(documents[1].TestProperty, document2.TestProperty);
            Assert.IsNotNull(document2.ComplexArray);
            Assert.AreEqual(1, Enumerable.Count(document2.ComplexArray));
            Assert.IsNotNull(Enumerable.ElementAt(document2.ComplexArray, 0));
            Assert.AreEqual(documents[1].ComplexArray[0].ValidProperty, Enumerable.ElementAt(document2.ComplexArray, 0).ValidProperty);
        }

        [Test]
        public void ShouldSaveDocumentWithInlineObject()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);

            var testProperty = Guid.NewGuid().ToString();

            var documents = new[]
                            {
                                new
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    TestProperty = testProperty,
                                    ComplexObject = new { ValidProperty = 11 }
                                },
                                new
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    TestProperty = testProperty,
                                    ComplexObject = new { ValidProperty = 21 }
                                }
                            };

            // When

            documentApi.SetDocuments(DocumentType, documents);

            var items = documentApi.GetDocument(DocumentType, f => f.AddCriteria(c => c.Property("TestProperty").IsEquals(testProperty)), 0, 10);

            // Then

            Assert.IsNotNull(items);
            Assert.AreEqual(2, items.Count());

            var document1 = items.FirstOrDefault(i => i.Id == documents[0].Id);
            Assert.IsNotNull(document1);
            Assert.AreEqual(documents[0].TestProperty, document1.TestProperty);
            Assert.IsNotNull(document1.ComplexObject);
            Assert.AreEqual(documents[0].ComplexObject.ValidProperty, document1.ComplexObject.ValidProperty);

            var document2 = items.FirstOrDefault(i => i.Id == documents[1].Id);
            Assert.IsNotNull(document2);
            Assert.AreEqual(documents[1].Id, document2.Id);
            Assert.AreEqual(documents[1].TestProperty, document2.TestProperty);
            Assert.IsNotNull(document2.ComplexObject);
            Assert.AreEqual(documents[1].ComplexObject.ValidProperty, document2.ComplexObject.ValidProperty);
        }

        [Test]
        public void ShouldSetDocuments()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);

            var testProperty = Guid.NewGuid().ToString();

            var documents = new[]
                            {
                                new
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    TestProperty = testProperty
                                },
                                new
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    TestProperty = testProperty
                                },
                                new
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    TestProperty = testProperty
                                },
                                new
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    TestProperty = testProperty
                                },
                                new
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    TestProperty = testProperty
                                }
                            };

            // When

            documentApi.SetDocuments(DocumentType, documents);

            var items = documentApi.GetDocument(DocumentType, filter => filter.AddCriteria(cr => cr.Property("TestProperty").IsEquals(testProperty)), 0, 10);

            // Then

            Assert.IsNotNull(items);
            Assert.AreEqual(documents.Length, items.Count());
        }
    }
}