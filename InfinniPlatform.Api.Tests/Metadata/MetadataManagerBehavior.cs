using System.Collections;
using System.Collections.Generic;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata.Factories;
using InfinniPlatform.Api.Metadata.MetadataManagers;
using InfinniPlatform.Api.RestApi;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.Api.Tests.Builders;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.Metadata
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class MetadataManagerBehavior
	{
		private TestRestServer _server;
		private const string Server = "localhost";
		private const int Port = 9999;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			
			_server = TestApi.StartServer(
				p => p.SetPort(Port)
					  .SetServerName(Server));
			TestApi.InitClientRouting(Server, Port);

			ConfigurationBuilder.CreateSampleConfigurationTestFixture();
		}

		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
			IndexApi.RebuildIndex("Systemconfig", "metadata");
			IndexApi.RebuildIndex("Systemconfig", "menumetadata");
			_server.Stop();
		}



        [Test]
        public void ShouldGetConfiguration()
        {
			dynamic config = new MetadataFactory().BuildConfigurationMetadataReader().GetItem(ConfigurationBuilder.ConfigurationName);
            Assert.IsNotNull(config);
        }

	}
}
