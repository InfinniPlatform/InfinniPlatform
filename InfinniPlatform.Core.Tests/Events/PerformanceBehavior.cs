using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using InfinniPlatform.Api.RestQuery.EventObjects;
using InfinniPlatform.Core.Tests.Events.Builders;
using InfinniPlatform.Json;
using InfinniPlatform.Json.EventBuilders;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Events;
using NUnit.Framework;
using Newtonsoft.Json;

namespace InfinniPlatform.Core.Tests.Events
{
	[TestFixture]
	[Category(TestCategories.PerformanceTest)]
	[Ignore("Тесты не работают из-за наличия циклических ссылок в тестовых данных")]
	public class PerformanceBehavior
	{
		[Test]
		public void JsonSerializationTestForBigJsonObjectCreation_1300Events()
		{
			var journal = FakeVidalMetadata.GetVidalRefFormJournalMetadata();

			var events = journal
				.ToEventListAsObject()
				.Exclude("FormMetadata.ObjectMetadata.FieldCode.FieldMetadata.Parent")
				.Exclude("FormMetadata.ObjectMetadata.FieldName.FieldMetadata.Parent")
                .Exclude("FormMetadata.ControlsMetadata.0.Metadata.GridColumnsMetadata.([0-9]+).DataField.Parent")
				.Exclude("FormMetadata.ControlsMetadata.1.Metadata.FieldsMetadata.([0-9]+).Parent")
				.GetEvents();

			var watch = Stopwatch.StartNew();
			for (int i = 0; i < 60; i++)
			{
				new BackboneBuilderJson().ConstructJsonObject(new DynamicWrapper(), events);
			}
			watch.Stop();
			Console.WriteLine(watch.ElapsedMilliseconds / 1000);
			Assert.GreaterOrEqual(10, watch.ElapsedMilliseconds / 1000);
		}

		[Test]
		public void JsonSerializationTestForBigJsonObjectUpdate_30Events()
		{
			var journal = FakeVidalMetadata.GetVidalRefFormJournalMetadata();

			var events = journal
				.ToEventListAsObject()
				.Exclude("FormMetadata.ObjectMetadata.FieldCode")
				.Exclude("FormMetadata.ObjectMetadata.FieldName")
				.Exclude("FormMetadata.ControlsMetadata.0.Metadata.GridColumnsMetadata.([0-9]+).DataField.Parent")
				.Exclude("FormMetadata.ControlsMetadata.1.Metadata.FieldsMetadata.([0-9]+).Parent")
				.GetEvents();

			var resultObject = new BackboneBuilderJson().ConstructJsonObject(new DynamicWrapper(), events);

			var eventsToUpdate = new List<string>()
				                    {
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldCode\",\"Action\":1}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldCode.Id\",\"Value\":1010,\"Action\":2}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldCode.FieldMetadata\",\"Action\":1}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldCode.FieldMetadata.Id\",\"Value\":10010,\"Action\":2}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldCode.FieldMetadata.IdTree\",\"Value\":false,\"Action\":2}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldCode.FieldMetadata.IdTreeParent\",\"Value\":false,\"Action\":2}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldCode.FieldMetadata.MetadataId\",\"Value\":\"OBJECT_CODE\",\"Action\":2}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldCode.FieldMetadata.MetadataName\",\"Value\":\"Идентификатор объекта\",\"Action\":2}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldCode.FieldMetadata.MetadataDataType\",\"Action\":1}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldCode.FieldMetadata.MetadataDataType.MetadataTypeKind\",\"Value\":\"SimpleType\",\"Action\":2}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldCode.FieldMetadata.MetadataDataType.MetadataIdentifier\",\"Value\":\"string\",\"Action\":2}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldCode.FieldMetadata.MetadataDataType.MetadataValues\",\"Action\":2}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldCode.FieldMetadata.IsEditable\",\"Value\":false,\"Action\":2}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldCode.FieldMetadata.IsFilterable\",\"Value\":false,\"Action\":2}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldCode.FieldMetadata.FilterControlType\",\"Action\":2}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldCode.FieldMetadata.DataFieldName\",\"Action\":2}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldCode.FieldMetadata.IsIdentifier\",\"Value\":false,\"Action\":2}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldCode.FieldMetadata.VisualTemplate\",\"Action\":2}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldCode.Value\",\"Value\":\"REF_VIDAL\",\"Action\":2}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldName\",\"Action\":1}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldName.Id\",\"Value\":1010,\"Action\":2}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldName.FieldMetadata\",\"Action\":1}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldName.FieldMetadata.Id\",\"Value\":10011,\"Action\":2}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldName.FieldMetadata.IdTree\",\"Value\":false,\"Action\":2}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldName.FieldMetadata.IdTreeParent\",\"Value\":false,\"Action\":2}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldName.FieldMetadata.MetadataId\",\"Value\":\"OBJECT_NAME\",\"Action\":2}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldName.FieldMetadata.MetadataName\",\"Value\":\"Наименование объекта\",\"Action\":2}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldName.FieldMetadata.MetadataDataType\",\"Action\":1}",
					                    "{\"Property\":\"FormMetadata.ObjectMetadata.FieldName.FieldMetadata.MetadataDataType.MetadataTypeKind\",\"Value\":\"SimpleType\",\"Action\":2}",

				                    };


			var watch = Stopwatch.StartNew();
			for (int i = 0; i < 600; i++)
			{
				new BackboneBuilderJson().ConstructJsonObject(resultObject, eventsToUpdate.Select(JsonConvert.DeserializeObject<EventDefinition>));
			}
			watch.Stop();
			Console.WriteLine(watch.ElapsedMilliseconds);
			Assert.GreaterOrEqual(1500, watch.ElapsedMilliseconds);
		}

		//[Test]
		//[Ignore]
		//public void ShouldSavePatient10000TimesInEditAndCreateMode()
		//{
		//	//given
		//	var elasticFactory = new ElasticFactory(new RoutingFactoryBase());
		//	var indexProvider = elasticFactory.BuildIndexStateProvider();
		//	indexProvider.RecreateIndex("patient");
		//	var aggregateProvider = new AggregateProvider();
			
		//	var metadataConfigurationProvider = new MetadataConfigurationProvider(new ServiceRegistrationContainerProvider(
		//		InfinniPlatformHostFactory.CreateDefaultServiceConfiguration(new ServiceTemplateConfiguration())),new ScriptFactoryBuilder(new ElasticFactory(new RoutingFactoryBase()), new DataStorageFactory()));
		//	var configurationObjectBuilder = new ConfigurationObjectBuilder(elasticFactory);

		//	var installer = new IntegrationConfigInstaller();
		//	var config = metadataConfigurationProvider.AddConfiguration(installer.ConfigurationId);
		//	installer.InstallConfiguration(config);
		//	installer.InstallConfigurationServices(config.ServiceRegistrationContainer);

		//	var eventStorageFactory = new DataStorageFactory();
		//	var eventStorage = eventStorageFactory.CreateEventStorageManager();

		//	try
		//	{
		//		eventStorage.DeleteStorage();
		//	}
		//	catch
		//	{
		//		//table not exists
		//	}

		//	eventStorage.CreateStorage();


		//	var query = new ApplyChangesHandler(configurationObjectBuilder, aggregateProvider, metadataConfigurationProvider, new GlobalContext(elasticFactory,eventStorageFactory));
		//	query.ConfigRequestProvider = new ConfigRequestProvider()
		//	{
		//		RequestData = new HttpRouteData(new HttpRoute(), new HttpRouteValueDictionary()
		//																					  {
		//																						  {"configuration","integration"},
		//																						  {"metadata","patient"}
		//																					  })
		//	};

		//	dynamic expando1 = new EventDefinition();
		//	expando1.Property = "FirstName";
		//	expando1.Value = "Иван";
		//	expando1.Action = EventType.CreateProperty;
		//	dynamic expando2 = new EventDefinition();
		//	expando2.Property = "MiddleName";
		//	expando2.Value = "Иванович";
		//	expando2.Action = EventType.CreateProperty;
		//	dynamic expando3 = new EventDefinition();
		//	expando3.Property = "LastName";
		//	expando3.Value = "Иванов";
		//	expando3.Action = EventType.CreateProperty;
		//	dynamic expando4 = new EventDefinition();
		//	expando4.Property = "BirthDate";
		//	expando4.Value = "14.07.2013";
		//	expando4.Action = EventType.CreateProperty;
		//	dynamic expando5 = new EventDefinition();
		//	expando5.Property = "Snils";
		//	expando5.Value = "112-233-445 95";
		//	expando5.Action = EventType.CreateProperty;
		//	dynamic expando6 = new EventDefinition();
		//	expando6.Property = "Inn";
		//	expando6.Value = "7707083893";
		//	expando6.Action = EventType.CreateProperty;
		//	dynamic expando7 = new EventDefinition();
		//	expando7.Property = "Policy";
		//	expando7.Value = "1123";
		//	expando7.Action = EventType.CreateProperty;

		//	var events = new List<EventDefinition>()
		//							 {
		//								expando1,expando2,expando3,expando4,expando5,expando6,expando7
		//							 };
		//	var item = query.ApplyEventsWithMetadata(null, events.ToArray());
		//	indexProvider.Refresh();

		//	var memorySize = GC.GetTotalMemory(true);

		//	var watch = Stopwatch.StartNew();

		//	for (int i = 0; i < 10000; i++)
		//	{
		//		query.ApplyEventsWithMetadata(item.Id, events.ToArray());				
		//	}
		//	watch.Stop();

		//	var memoryAfterSave = GC.GetTotalMemory(false);

		//	var totalMemoryAcquired = memoryAfterSave - memorySize;
			
		//	Console.WriteLine("edit aggregate result");
		//	Console.WriteLine("--------------------------");
		//	Console.WriteLine(watch.ElapsedMilliseconds);
		//	Console.WriteLine(totalMemoryAcquired);

		//	memorySize = GC.GetTotalMemory(true);
		//	watch = Stopwatch.StartNew();
		//	for (int i = 0; i < 10000; i++)
		//	{
		//		query.ApplyEventsWithMetadata(null, events.ToArray());
		//	}
		//	watch.Stop();
		//	memoryAfterSave = GC.GetTotalMemory(false);

		//	totalMemoryAcquired = memoryAfterSave - memorySize;
		//	Console.WriteLine("new aggregate insert result");
		//	Console.WriteLine("--------------------------");
		//	Console.WriteLine(watch.ElapsedMilliseconds);
		//	Console.WriteLine(totalMemoryAcquired);
		//}





		//[Test]
		//[Ignore]
		//public void ShouldSavePatient10000TimesInEditAndCreateModeAndMultithread()
		//{
		//	//given
		//	var elasticFactory = new ElasticFactory(new RoutingFactoryBase());
		//	var indexProvider = elasticFactory.BuildIndexStateProvider();
		//	indexProvider.RecreateIndex("patient");
		//	var aggregateProvider = new AggregateProvider();
            
		//	var metadataConfigurationProvider = new MetadataConfigurationProvider(new ServiceRegistrationContainerProvider(MultiCareHostServerExtensions.CreateDefaultServiceConfiguration()));
		//	var installer = new IntegrationConfigInstaller();
		//	var config = metadataConfigurationProvider.AddConfiguration(installer.ConfigurationId);

		//	var configurationObjectBuilder = new ConfigurationObjectBuilder(elasticFactory);

		//	installer.InstallConfiguration(config);
		//	installer.InstallConfigurationServices(config.ServiceRegistrationContainer);


		//	var eventStorageFactory = new DataStorageFactory();
		//	var eventStorage = eventStorageFactory.CreateEventStorageManager();

		//	try
		//	{
		//		eventStorage.DeleteStorage();
		//	}
		//	catch
		//	{
		//		//table not exists
		//	}

		//	eventStorage.CreateStorage();
		//	var query = new ApplyChangesHandler(configurationObjectBuilder, aggregateProvider, metadataConfigurationProvider, new GlobalContext(elasticFactory,eventStorageFactory));

		//	query.ConfigRequestProvider = new ConfigRequestProvider()
		//	{
		//		RequestData = new HttpRouteData(new HttpRoute(), new HttpRouteValueDictionary()
		//																					  {
		//																						  {"configuration","integration"},
		//																						  {"metadata","patient"}
		//																					  })

		//	};

		//	var events = new []
		//						 {
		//							 "{\"Property\":\"FirstName\",\"Value\":\"Иван\",\"Action\":2}",
		//							 "{\"Property\":\"MiddleName\",\"Value\":\"Иванович\",\"Action\":2}",
		//							 "{\"Property\":\"LastName\",\"Value\":\"Иванов\",\"Action\":2}",
		//							 "{\"Property\":\"BirthDate\",\"Value\":\"14.07.2013\",\"Action\":2}",
		//							 "{\"Property\":\"Snils\",\"Value\":\"112-233-445 95\",\"Action\":2}",
		//							 "{\"Property\":\"Inn\",\"Value\":\"7707083893\",\"Action\":2}",
		//							 "{\"Property\":\"Policy\",\"Value\":\"1123\",\"Action\":2}",
		//						 }.Select(JsonConvert.DeserializeObject<EventDefinition>);
		//	var item = query.ApplyEventsWithMetadata(null, events);
		//	indexProvider.Refresh();


			
		//	var memorySize = GC.GetTotalMemory(true);
		//	var watch = Stopwatch.StartNew();

		//	var manualEvent = new ManualResetEvent(false);
		//	var resultEvents = new List<ManualResetEvent>();

		//	var threadCount = 100;
			
		//	for (int i = 0; i < threadCount; i++)
		//	{
		//		var thread = new Thread(() =>
		//													  {
						                                          
		//														  var resultEvent = new ManualResetEvent(false);
		//														  resultEvents.Add(resultEvent);
		//														  manualEvent.WaitOne();
		//														  for (int j = 0; j < 100; j++)
		//														  {
		//															  query.ApplyEventsWithMetadata(item.Id, events.ToArray());
		//														  }
		//														  resultEvent.Set();
		//													  });
		//		thread.IsBackground = true;
		//		thread.Start();
		//	}

		//	while (resultEvents.Count != threadCount)
		//	{
		//		Thread.Sleep(1);
		//	}

		//	manualEvent.Set();

		//	foreach (var manualResetEvent in resultEvents)
		//	{
		//		manualResetEvent.WaitOne();
		//	}

		//	var memoryAfterSave = GC.GetTotalMemory(false);

		//	var totalMemoryAcquired = memoryAfterSave - memorySize;

		//	Console.WriteLine("edit aggregate result");
		//	Console.WriteLine("--------------------------");
		//	Console.WriteLine(watch.ElapsedMilliseconds);
		//	Console.WriteLine(totalMemoryAcquired);

		//}

	}
}
