using System;
using System.IO;
using System.Linq;
using System.Text;

using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.TestEnvironment;

using NUnit.Framework;

namespace InfinniPlatform.RestfulApi.Tests
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public class ControllerBehavior
    {
		private IDisposable _server;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig));
		}

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
			_server.Dispose();
        }


        [Test]
        public void ShouldRebuildIndex()
        {
            var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "index", "rebuildindex",null, new
                {
                    Configuration = "integration",
                    Metadata = "document"
                });

            Assert.AreEqual(true, response.IsAllOk);
        }

		[Test]
		public void ShouldGetIndexStatus()
		{
			var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "index", "indexexists",null, new
			{
				Configuration = "integration",
				Metadata = "document"
			});

			Assert.AreEqual(true, response.IsAllOk);
		}
        
        [Test]
        public void ShouldGetFromIndex()
        {
            var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "index", "getfromindex",null, new
                {
                    Configuration = "integration",
                    Metadata = "document",
                    Id = Guid.NewGuid().ToString()
                });

            Assert.AreEqual(true, response.IsAllOk);
        }


        [Test]
        public void ShouldInsertIndex()
        {
            var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "index", "insertindex",null, new
                {
                    Configuration = "integration",
                    Metadata = "document",
                    Item = new
                        {
                            Id = Guid.NewGuid().ToString(),
                            TestProperty = "1",
                            TestInnerObject = new
                                {
                                    TestPropertyInner = "2"
                                }
                        }
                });

            Assert.AreEqual(true, response.IsAllOk);
        }


        [Test]
        public void ShouldInsertIndexWithTimestamp()
        {
            var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "index", "insertindexwithtimestamp", null, new
                {
                    Configuration = "integration",
                    Metadata = "document",
                    TimeStamp = DateTime.Now,
                    Item = new
                        {
                            Id = Guid.NewGuid().ToString(),
                            TestProperty = "1",
                            TestInnerObject = new
                                {
                                    TestPropertyInner = "2"
                                }
                        }
                });
            
            Assert.AreEqual(true, response.IsAllOk);            
        }

        [Test]
        public void ShouldGetDocument()
        {
            var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "getdocument",null, new
            {
                Configuration = "update",
                Metadata = "package",
                Filter = new object[]{},
                PageNumber = 0,
                PageSize = 1
            });

            Assert.AreEqual(true, response.IsAllOk);

            var result = response.ToDynamicList();
            Assert.AreEqual(1,result.Count());
        }

        [Test]
        public void ShouldSetDocument()
        {
            var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "setdocument", null, new
            {
                Configuration = "update",
				Metadata = "package",
                Item = new
                    {
                        Id = Guid.NewGuid().ToString(),
                        TestProperty = 1
                    }
            });

            Assert.AreEqual(true, response.IsAllOk);
        }


        static public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = Encoding.UTF8.GetBytes(toEncode);
            return Convert.ToBase64String(toEncodeAsBytes);
        }

        [Test]
        public void ShouldSetDocuments()
        {
            var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "setdocument",null, new
            {
                Configuration = "update",
                Metadata = "package",
                Documents = new object[]
                {
                    new
                        {
                            Id = Guid.NewGuid().ToString(),
                            TestProperty = 1                            
                        },
                    new
                        {
                            Id = Guid.NewGuid().ToString(),
                            TestProeprty = 2
                        }
                }
            });


            Assert.AreEqual(true, response.IsAllOk);
        }



		[Test]
		public void ShouldStartServer()
		{
            var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "status", null, null);

			Assert.AreEqual(true, response.IsAllOk);
		}

        [Test]
        public void ShouldInsertIndexFromDynamic()
        {

            dynamic DynamicWrapper = new DynamicWrapper();
            
            DynamicWrapper.Configuration = "integration";
            DynamicWrapper.Metadata = "document";
            DynamicWrapper.Item = new DynamicWrapper();
            DynamicWrapper.Item.Id = Guid.NewGuid().ToString();
            DynamicWrapper.Item.IInnerDocument = new DynamicWrapper();
            DynamicWrapper.Item.IInnerDocument.TestProperty = "1";
            var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "index", "insertindex",null, DynamicWrapper);

            Assert.AreEqual(true, response.IsAllOk);
        }

		[Test]
		public void ShoudlGetDocumentByQuery()
		{
            

			var queryText = File.ReadAllText(Path.Combine("TestData", "Query", "Query.txt"));

			dynamic bodyQuery = new DynamicWrapper();
			bodyQuery.QueryText = queryText;

            RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "getbyquery" ,null, bodyQuery);
		}
    }
}