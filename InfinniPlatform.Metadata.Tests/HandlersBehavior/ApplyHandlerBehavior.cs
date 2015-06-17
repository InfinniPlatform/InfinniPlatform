using System;
using System.Collections.Generic;

using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.TestEnvironment;

using NUnit.Framework;

namespace InfinniPlatform.Metadata.Tests.HandlersBehavior
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	[Ignore]
	public sealed class ApplyHandlerBehavior
	{
		private IDisposable _server;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig)
												.AddConfigurationFromAssembly("InfinniPlatform.Metadata.Tests"));

            new UpdateApi(null).ForceReload("Handlers");
            new UpdateApi(null).UpdateStore("Handlers");
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			_server.Dispose();
		}


		[Test]
		public void ShouldApplyEventsInsertAndUpdate()
		{

			//проверяем вставку нового объекта, основанную на применении событий
			var body = new
				{
					TestField = 1,
					TestItems = new List<object>
                        {
                            new
                                {
                                    TestInner = 2
                                },
                            new
                                {
                                    TestInner = 3
                                }
                        }
				};

			dynamic result = RestQueryApi.QueryPostRaw("Handlers", "patienttest", "checkevents", null, body).ToDynamic();
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.TestField);
			Assert.AreEqual(2, result.TestItems.Count());

			//проверяем апдейт объекта
			var bodyUpdate = new
			{
				TestField1 = 1,
				TestField = 2
			};
			result = RestQueryApi.QueryPostRaw("Handlers", "patienttest", "checkevents", result.Id, bodyUpdate).ToDynamic();
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.TestField1);
			Assert.AreEqual(2, result.TestField);
			Assert.AreEqual(2, result.TestItems.Count());

			//проверяем замену объекта
			var bodyReplace = new
			{
				TestField1 = 1,
				TestField = 2
			};
			result = RestQueryApi.QueryPostRaw("Handlers", "patienttest", "checkevents", result.Id, bodyReplace,null, true).ToDynamic();
			Assert.IsNotNull(result);
			Assert.IsNull(result.TestItems);
			Assert.AreEqual(1, result.TestField1);
			Assert.AreEqual(2, result.TestField);
		}

		[Test]
		public void ShouldApplyJsonInsertAndReplace()
		{
			//проверяем вставку нового объекта, основанного на применении JSON-объекта
			var body = new
			{
				TestField = 1,
				TestItems = new List<object>
                        {
                            new
                                {
                                    TestInner = 2
                                },
                            new
                                {
                                    TestInner = 3
                                }
                        }
			};

			dynamic result = RestQueryApi.QueryPostJsonRaw("Handlers", "patienttest", "checkjson", null, body).ToDynamic();
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.TestField);
			Assert.AreEqual(2, result.TestItems.Count());

			//проверяем апдейт объекта
			var bodyUpdate = new
				{
					TestField1 = 1,
					TestField = 2
				};
			result = RestQueryApi.QueryPostJsonRaw("Handlers", "patienttest", "checkjson", result.Id, bodyUpdate).ToDynamic();
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.TestField1);
			Assert.AreEqual(2, result.TestField);
			Assert.AreEqual(2, result.TestItems.Count());

			//проверяем замену объекта
			var bodyReplace = new
			{
				TestField1 = 1,
				TestField = 2
			};
			result = RestQueryApi.QueryPostJsonRaw("Handlers", "patienttest", "checkjson", result.Id, bodyReplace, null, true).ToDynamic();
			Assert.IsNotNull(result);
			Assert.IsNull(result.TestItems);
			Assert.AreEqual(1, result.TestField1);
			Assert.AreEqual(2, result.TestField);



		}



	}
}
