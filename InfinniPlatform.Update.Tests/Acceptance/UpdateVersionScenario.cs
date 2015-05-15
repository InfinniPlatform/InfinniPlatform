using System.Collections.Generic;
using System.IO;
using System.Net;
using InfinniConfiguration.Update.Tests.Builders;
using InfinniPlatform;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.RestApi;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.Compression;
using InfinniPlatform.Hosting;

using InfinniPlatform.Modules;
using InfinniPlatform.Runtime;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace InfinniConfiguration.Update.Tests.Acceptance
{
	[TestFixture]
	[Category(TestCategories.AcceptanceTest)]
	[Ignore]
	public sealed class UpdateVersionScenario
	{
		private TestRestServer _server;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			AcceptanceTestBuilder.RebuildIntegrationIndexes();
	        AcceptanceTestBuilder.RebuildConfigurationIndexes();

	        _server = TestApi.StartServer(
	            p => p.SetPort(9999)
					  .SetServerName("localhost")
					  .InstallFromJSON(@"TestData\Configurations\Integration.zip", new[] { "InfinniConfiguration.Integration.dll" }));

		}


		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			_server.Stop();
		}


		[Test]
		[Ignore("Тест нуждается в переработке")]
		public void ShouldUpdateVersion()
		{
			//1 Этап. Развертывание первоначальной версии. Отсутствуют развернутые версии в индексе
			var pathToArchive = Path.Combine(Directory.GetCurrentDirectory(), "TestArchive.zip");

            //создаем первоначальную версию конфигурацию
            var packages = CreateUpdate("Integration", pathToArchive, @"TestData\TestAssemblies\InfinniConfiguration.Integration.dll", "version1");

            //устанавливаем пакеты обновления
            InstallUpdate(packages, "Integration");

            //создаем версию конфигурации Rest API для проверки корректности одновременной работы нескольких конфигураций
			packages = CreateUpdate("RestfulApi", pathToArchive, @"TestData\TestAssemblies\InfinniConfiguration.RestfulApi.exe", "version1_api");

            //устанавливаем пакеты обновления
            InstallUpdate(packages, "RestfulApi");


            //создаем организацию
			var events = AcceptanceTestBuilder.RenderValidOrganizationEvents();
			var restClient = new RestClient("http://localhost:9999/Integration/StandardApi/Organization/Publish/");
			var body = new Dictionary<string, object>
				           {
					           {"id", "9AD2B49E-F76D-407C-A47B-AC5E11614CDC"},					           
					           {"events", events},
				           };

			//публикуем организацию
			var restResponse = restClient.Post(new RestRequest { RequestFormat = DataFormat.Json }
															.AddBody(body));

			Assert.AreEqual(HttpStatusCode.OK, restResponse.StatusCode);
			dynamic instance = new DynamicInstance(JObject.Parse(restResponse.Content));
			Assert.IsNotNull(instance.Token);
			//проверяем, что результирующий ответ не содержит поля, которое в данной версии не присутствует
			Assert.IsNull(instance.GetProperty("Version"));
	
			//---------------------------------------------------------
			//2 Этап. Проверка обновления версии конфигурации. Обновление конфигурации на новую версию не должно влиять
            //на существующие объекты 

            //создаем обновление
			var updatePackages = CreateUpdate("Integration", pathToArchive, @"TestData\TestAssemblies\InfinniConfiguration.Integration_changed.dll", "version2");

		    //устанавливаем обновление
            InstallUpdate(updatePackages, "Integration");

            //эмулируем обновление другой одновременно существующей конфигурации 

			updatePackages = CreateUpdate("RestfulApi", pathToArchive, @"TestData\TestAssemblies\InfinniConfiguration.RestfulApi.exe", "version2_api");

            InstallUpdate(updatePackages, "RestfulApi");

		    events = AcceptanceTestBuilder.RenderValidOrganizationEvents();


            //---------------------------------------------------------
            //2.1 Создаем новую организацию. В результате должно быть заполнено поле Version значением NewVersion, в соответствие 
            //с новой версией скриптов
            restClient = new RestClient("http://localhost:9999/Integration/StandardApi/Organization/Publish/");

            body = new Dictionary<string, object>
				           {
					           {"id", "CF75A0A5-D262-48BD-A7D3-F6D6672ED3D0"},					           
					           {"events", events},
				           };

            //публикуем организацию
            restResponse = restClient.Post(new RestRequest { RequestFormat = DataFormat.Json }
                                                            .AddBody(body));

            Assert.AreEqual(HttpStatusCode.OK, restResponse.StatusCode);
            instance = new DynamicInstance(JObject.Parse(restResponse.Content));
            Assert.IsNotNull(instance.Token);
            //проверяем, что в новой версии присутствует поле Version с содержимым "NewVersion"
            Assert.AreEqual("NewVersion", instance.GetProperty("Version"));
            //---------------------------------------------------------

            //---------------------------------------------------------
            //2.2 Обновляем существующую организацию. В результате не должно измениться поведение при сохранении документа (поле Version не должно
            //создаваться, так как документ должен быть обработан с указанной версией скриптовой логики
			restClient = new RestClient("http://localhost:9999/Integration/StandardApi/Organization/Publish/");
			
            //обновляем уже существующую организацию. В результате должны выбрать версию скриптов, соответствующую сохраненному документу
            body = new Dictionary<string, object>
				           {
					           {"id", "9AD2B49E-F76D-407C-A47B-AC5E11614CDC"},					           
					           {"events", events},
				           };

			//публикуем организацию
			restResponse = restClient.Post(new RestRequest { RequestFormat = DataFormat.Json }
															.AddBody(body));

			Assert.AreEqual(HttpStatusCode.OK, restResponse.StatusCode);
			instance = new DynamicInstance(JObject.Parse(restResponse.Content));
			Assert.IsNotNull(instance.Token);
			//проверяем, что в новой версии присутствует поле Version с содержимым "NewVersion"
			Assert.AreEqual(null, instance.GetProperty("Version"));

		}

		private static List<dynamic> CreateUpdate(string configName, string pathToArchive, string assemblyPath, string version)
	    {
            //создаем пакет обновления с измененной скриптовой сборкой
	        //CreateUpdatePackage(configName, pathToArchive,assemblyPath,version);

	        //распаковываем архив, получаем список событий обновления
	        return new GZipDataCompressor().CreateEventsFromArchive(pathToArchive);
	    }

		private static void InstallUpdate(IEnumerable<dynamic> updatePackages, string configMetadata)
	    {
            
            //создаем обработчик обновления
	        var restClient = new RestClient("http://localhost:9999/Update/StandardApi/Package/Install/");

	        foreach (var updatePackage in updatePackages)
	        {
                var body = new Dictionary<string, object>
	            {
	                {"id", null},
	                {"events", updatePackage.GetEvents()},
	            };

                //выполняем обновление метаданных с помощью пакета обновления
                var restResponse = restClient.Post(new RestRequest { RequestFormat = DataFormat.Json, Timeout = 120000}
                                                   .AddBody(body));

                Assert.AreEqual(HttpStatusCode.OK, restResponse.StatusCode);
                //нотифицируем сервер о необходимости обновления версии конфигурации

                restClient = new RestClient("http://localhost:9999/Update/StandardApi/Package/Notify/");

	            var bodyNotify = new Dictionary<string, object>()
	                {
	                    {"metadataConfigurationId", configMetadata}
	                };

                //обновляем версию
                restResponse = restClient.Post(new RestRequest { RequestFormat = DataFormat.Json }.AddBody(bodyNotify));



                Assert.AreEqual(HttpStatusCode.OK, restResponse.StatusCode);	            
	        }


	    }

		//private static void CreateUpdatePackage(string configName, string pathToArchive, string assemblyPath, string version)
		//{
		//	//формируем пакет обновления из сборок
		//	using (
		//		var eventsSerialized = new PackageBuilder(new GZipDataCompressor()).BuildPackageToStream(configName,
		//																								 version,
		//																								 Path.Combine(Directory.GetCurrentDirectory(), assemblyPath)) )
	            
		//	{
		//		if (File.Exists(pathToArchive))
		//		{
		//			File.Delete(pathToArchive);
		//		}
		//		//сохраняем в файл архива
		//		eventsSerialized.WriteAllBytes(pathToArchive);
		//	}
		//}
	}
}
