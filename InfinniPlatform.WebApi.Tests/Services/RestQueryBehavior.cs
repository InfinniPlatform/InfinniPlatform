using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Hosting;
using InfinniPlatform.Modules;
using InfinniPlatform.WebApi.Factories;
using InfinniPlatform.WebApi.Tests.Builders;
using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.WebApi.Tests.Services
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class RestQueryExecutorBehavior
    {
        private IHostingService _service;


        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            ConfigurationBuilder.InitScriptStorage();
            var factory = new OwinHostingServiceFactory(ModuleExtension.LoadModulesAssemblies("InfinniPlatform.Metadata,InfinniConfiguration.Update,InfinniPlatform.WebApi.Tests"));

            _service = factory.CreateHostingService();

            _service.Start();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _service.Stop();
        }

        [Test]
        public void ShouldMakeGetQueryToService()
        {
            var builder = new RestQueryBuilder("QueryExecutorTest", "patient", "search");

            var response = builder.QueryGet(new dynamic[] {}, 0, 100);

            Assert.AreEqual(response.IsServiceNotRegistered,false);
            Assert.AreEqual(response.IsServerError, false);
            Assert.AreEqual(response.IsBusinessLogicError, false);
            Assert.AreEqual(response.IsRemoteServerNotFound, false);

            dynamic result = new DynamicInstance(JObject.Parse(response.Content));
            Assert.AreEqual(true, result.IsValid);
            Assert.AreEqual("Document provider not registered for metadata: \"patient\"", result.ValidationMessage);
        }

        [Test]
        public void ShouldFailGetQueryToService()
        {
            var builder = new RestQueryBuilder("QueryExecutorTest", "patient", "nonExistingAction");

            var response = builder.QueryGet(new dynamic[] { }, 0, 100);

            Assert.AreEqual(response.IsServiceNotRegistered, true);
            Assert.AreEqual(response.IsServerError, true);
            Assert.AreEqual(response.IsBusinessLogicError, false);
            Assert.AreEqual(response.IsRemoteServerNotFound, false);            
        }

        [Test]
        public void ShouldMakePostQueryToService()
        {
            var builder = new RestQueryBuilder("QueryExecutorTest", "patient", "publish");
            var response = builder.QueryPost(null, new
                {
                    TestProperty = 1
                });

            Assert.AreEqual(response.IsServiceNotRegistered, false);
            Assert.AreEqual(response.IsServerError, false);
            Assert.AreEqual(response.IsBusinessLogicError, false);
            Assert.AreEqual(response.IsRemoteServerNotFound, false); 

            dynamic result = new DynamicInstance(JObject.Parse(response.Content));
            Assert.AreEqual(1, result.TestProperty);
            Assert.AreEqual("version_metadatatests", result.Version);
        }


        [Test]
        public void ShoulFailPostQueryToService()
        {
            var builder = new RestQueryBuilder("QueryExecutorTest", "patient", "publishNonExisting");

            var response = builder.QueryPost(null, new
            {
                TestProperty = 1
            });

            Assert.AreEqual(response.IsServiceNotRegistered, true);
            Assert.AreEqual(response.IsServerError, true);
            Assert.AreEqual(response.IsBusinessLogicError, false);
            Assert.AreEqual(response.IsRemoteServerNotFound, false);
        }
    }
}
