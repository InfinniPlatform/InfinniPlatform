using InfinniPlatform.Actions;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Factories;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.BlobStorage;
using InfinniPlatform.Cassandra.Client;
using InfinniPlatform.EventStorage;
using InfinniPlatform.Factories;
using InfinniPlatform.Hosting;
using InfinniPlatform.Index;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Logging;
using InfinniPlatform.Metadata;
using InfinniPlatform.MigrationsAndVerifications.Migrations;
using InfinniPlatform.Reporting.PrintView;
using InfinniPlatform.SystemConfig.RoutingFactory;
using InfinniPlatform.Transactions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace InfinniPlatform.MigrationsAndVerifications.Tests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public sealed class DocumentSchemaMigrationBehavior
    {
        private IIndexStateProvider _indexProvider;

        private const string ContainerId = "TestMetadata";
        private const string ContainerIdWithInline = "TestMetadataWithInline";
        private const string MetadataConfigurationId = "TestConfig";


		private IDisposable _server;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig));
		}

        [TestFixtureTearDown]
        public void TearDownFixture()
        {
            _server.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            _indexProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();
            _indexProvider.DeleteIndexType(MetadataConfigurationId, ContainerId);
        }

        [Test]
        public void ShouldCreateConfigurationIndexes()
        {
            var configProvider = new MetadataConfigurationProviderMock();
            dynamic containerSchema = new
            {
                Type = "Object",
                Caption = "InitialDocument",
                Properties = new
                {
                    Id = new
                    {
                        Type = "Uuid",
                        Caption = "Идетификатор"
                    },
                    FirstName = new
                    {
                        Type = "String",
                        Caption = "Имя"
                    }
                }
            }.ToDynamic();
            configProvider.GetMetadataConfiguration("").SetSchemaVersion(ContainerId, containerSchema);
            
            var containerSchemaWithInline = new
            {
                Type = "Object",
                Caption = "InitialDocument",
                Properties = new
                {
                    Id = new
                    {
                        Type = "Uuid",
                        Caption = "Идетификатор"
                    },
                    Link = new
                    {
                        Type = "Object",
                        TypeInfo = new
                        {
                            DocumentLink = new
                            {
                                ConfigId = MetadataConfigurationId,
                                DocumentId = ContainerId,
                                Inline = true
                            }
                        }
                    }
                }
            }.ToDynamic();
            configProvider.GetMetadataConfiguration("").SetSchemaVersion(ContainerIdWithInline, containerSchemaWithInline);
            
            //When
            var storeInstaller = new UpdateStoreMigration();
            string message;
            
            storeInstaller.AssignActiveConfiguration(MetadataConfigurationId, new GlobalContext(
                new CassandraEventStorageFactory(new CassandraDatabaseFactory()),
                new CassandraBlobStorageFactory(new CassandraDatabaseFactory()),				
				new FlowDocumentPrintViewBuilderFactory(),
				new ElasticFactory(new RoutingFactoryBase()),
                null,
                configProvider,
                new TransactionManager(new ElasticFactory(new RoutingFactoryBase())), 
				null,
				new Log4NetLogFactory()));

            storeInstaller.Up(out message, new object[] {});

            //Then
            var stateIndex = _indexProvider.GetIndexStatus(MetadataConfigurationId, ContainerId);
            
            Assert.AreEqual(true, stateIndex == IndexStatus.Exists);
        }

        [Test]
        public void ShouldUpdateConfigurationIndexWhenInlineDocumentsChanged()
        {
            //Given
            var configProvider = new MetadataConfigurationProviderMock();
            dynamic containerSchema = new
            {
                Type = "Object",
                Caption = "InitialDocument",
                Properties = new
                {
                    Id = new
                    {
                        Type = "Uuid",
                        Caption = "Идетификатор"
                    },
                    FirstName = new
                    {
                        Type = "String",
                        Caption = "Имя"
                    }
                }
            }.ToDynamic();
            configProvider.GetMetadataConfiguration("").SetSchemaVersion(ContainerId, containerSchema);

            var containerSchemaWithInline = new
            {
                Type = "Object",
                Caption = "InitialDocument",
                Properties = new
                {
                    Id = new
                    {
                        Type = "Uuid",
                        Caption = "Идетификатор"
                    },
                    Link = new
                    {
                        Type = "Object",
                        TypeInfo = new
                        {
                            DocumentLink = new
                            {
                                ConfigId = MetadataConfigurationId,
                                DocumentId = ContainerId,
                                Inline = true
                            }
                        }
                    }
                }
            }.ToDynamic();
            configProvider.GetMetadataConfiguration("").SetSchemaVersion(ContainerIdWithInline, containerSchemaWithInline);
            //When

            var storeInstaller = new UpdateStoreMigration();
            string message;
            storeInstaller.AssignActiveConfiguration(MetadataConfigurationId, new GlobalContext(
                new CassandraEventStorageFactory(new CassandraDatabaseFactory()),
                new CassandraBlobStorageFactory(new CassandraDatabaseFactory()),
				new FlowDocumentPrintViewBuilderFactory(),
				new ElasticFactory(new RoutingFactoryBase()),
                null,
                configProvider,
				new TransactionManager(new ElasticFactory(new RoutingFactoryBase())), 
				null,
				new Log4NetLogFactory()));
            
            storeInstaller.Up(out message, new object[] {});

            var stateIndex = _indexProvider.GetIndexStatus(MetadataConfigurationId, ContainerId);

            Assert.AreEqual(true, stateIndex == IndexStatus.Exists);

            // Изменяем схему данных исходного документа

            var schema = configProvider.GetMetadataConfiguration("").GetSchemaVersion(ContainerId);
            schema.Properties.FirstName.Type = "Object";
            configProvider.GetMetadataConfiguration("").SetSchemaVersion(ContainerId, schema);
            
            storeInstaller.Up(out message, new object[] {});

            Assert.AreEqual(IndexStatus.Exists, _indexProvider.GetIndexStatus(MetadataConfigurationId, ContainerIdWithInline));
        }


        private sealed class MetadataConfigurationMock : IMetadataConfiguration
        {
            private readonly Dictionary<string, dynamic> _schemas = new Dictionary<string, dynamic>();

            public string ConfigurationId { get; set; }

            public IScriptConfiguration ScriptConfiguration
            {
                get { throw new NotImplementedException(); }
            }

            public string ActualVersion
            {
                get { return "version"; }
            }

            public IEnumerable<string> Containers
            {
                get { return new[] { ContainerId, ContainerIdWithInline }; }
            }

            public bool IsEmbeddedConfiguration
            {
                get { return false; }
            }
            public dynamic MoveWorkflow(string containerId, string workflowId, dynamic target, object state = null)
            {
                throw new NotImplementedException();
            }

            public string GetExtensionPointValue(string metadataId, string actionInstanceName, string extensionPointTypeName)
            {
                throw new NotImplementedException();
            }
            
            public SearchAbilityType GetSearchAbilityType(string containerId)
            {
                return SearchAbilityType.KeywordBasedSearch;
            }

            public void SetSearchAbilityType(string containerId, SearchAbilityType searchAbility)
            {
                throw new NotImplementedException();
            }

            public void RegisterWorkflow(string containerId, string workflowId, Action<IStateWorkflowStartingPointConfig> actionConfiguration)
            {
                throw new NotImplementedException();
            }
            public void RegisterMenu(IEnumerable<dynamic> menuList)
            {
                throw new NotImplementedException();
            }
            public IEnumerable<dynamic> GetMenuList()
            {
                throw new NotImplementedException();
            }

            public string GetMetadataIndexType(string containerId)
            {
                return containerId;
            }

            public void SetMetadataIndexType(string containerId, string indexType)
            {
                throw new NotImplementedException();
            }

            public void SetSchemaVersion(string containerId, dynamic schema)
            {
                _schemas[containerId] = schema;
            }

            public dynamic GetSchemaVersion(string containerId)
            {
                return _schemas[containerId];
            }

            public void RegisterProcess(string containerId, dynamic process)
            {
                throw new NotImplementedException();
            }
            public void RegisterService(string containerId, dynamic service)
            {
                throw new NotImplementedException();
            }
            public void RegisterScenario(string containerId, dynamic scenario)
            {
                throw new NotImplementedException();
            }

            public void RegisterValidationError(string containerId, dynamic error)
            {
                throw new NotImplementedException();
            }

            public void RegisterStatus(string containerId, dynamic status)
            {
                throw new NotImplementedException();
            }

            public void RegisterRegister(dynamic register)
            {
                throw new NotImplementedException();
            }

            public void UnregisterProcess(string containerId, string process)
            {
                throw new NotImplementedException();
            }

            public void UnregisterService(string containerId, string service)
            {
                throw new NotImplementedException();
            }

            public void UnregisterScenario(string containerId, string scenario)
            {
                throw new NotImplementedException();
            }

            public void UnregisterGenerator(string containerId, string generator)
            {
                throw new NotImplementedException();
            }

            public void UnregisterView(string containerId, string view)
            {
                throw new NotImplementedException();
            }

            public void UnregisterPrintView(string containerId, string printView)
            {
                throw new NotImplementedException();
            }

            public void UnregisterValidationWarning(string containerId, string warning)
            {
                throw new NotImplementedException();
            }

            public void UnregisterValidationError(string containerId, string error)
            {
                throw new NotImplementedException();
            }

            public void UnregisterStatus(string containerId, string status)
            {
                throw new NotImplementedException();
            }

            public void UnregisterRegister(string register)
            {
                throw new NotImplementedException();
            }

            public dynamic GetProcess(string containerId, string processName)
            {
                throw new NotImplementedException();
            }
            public dynamic GetService(string containerId, string serviceName)
            {
                throw new NotImplementedException();
            }

            public dynamic GetValidationWarning(string containerId, string warningName)
            {
                throw new NotImplementedException();
            }

            public dynamic GetStatus(string containerId, string statusName)
            {
                throw new NotImplementedException();
            }

            public dynamic GetScenario(string containerId, string scenarioName)
            {
                throw new NotImplementedException();
            }

            public dynamic GetValidationError(string containerId, string errorName)
            {
                throw new NotImplementedException();
            }

            public dynamic GetGenerator(string documentId, Func<dynamic, bool> generatorSelector)
	        {
		        throw new NotImplementedException();
	        }

	        public dynamic GetView(string containerId, Func<dynamic, bool> viewSelector)
	        {
		        throw new NotImplementedException();
	        }

	        public dynamic GetPrintView(string containerId, Func<dynamic, bool> selector)
	        {
		        throw new NotImplementedException();
	        }

	        public dynamic GetRegister(string registerName)
            {
                throw new NotImplementedException();
            }

	        public IEnumerable<dynamic> GetViews(string containerId)
	        {
		        throw new NotImplementedException();
	        }

	        public IEnumerable<dynamic> GetPrintViews(string containerId)
	        {
		        throw new NotImplementedException();
	        }

	        public IEnumerable<dynamic> GetGenerators(string containerId)
	        {
		        throw new NotImplementedException();
	        }

	        public IEnumerable<dynamic> GetScenarios(string containerId)
	        {
		        throw new NotImplementedException();
	        }

	        public IEnumerable<dynamic> GetProcesses(string containerId)
	        {
		        throw new NotImplementedException();
	        }

	        public IEnumerable<dynamic> GetServices(string containerId)
	        {
		        throw new NotImplementedException();
	        }

	        public IEnumerable<dynamic> GetValidationErrors(string containerId)
	        {
		        throw new NotImplementedException();
	        }

	        public IEnumerable<dynamic> GetValidationWarnings(string containerId)
	        {
		        throw new NotImplementedException();
	        }

	        public void RegisterGenerator(string containerId, dynamic generator)
	        {
		        throw new NotImplementedException();
	        }

	        public void RegisterView(string containerId, dynamic view)
	        {
		        throw new NotImplementedException();
	        }

	        public void RegisterPrintView(string containerId, dynamic printView)
	        {
		        throw new NotImplementedException();
	        }

            public void RegisterValidationWarning(string containerId, dynamic warning)
            {
                throw new NotImplementedException();
            }

            public IServiceRegistrationContainer ServiceRegistrationContainer
            {
                get { throw new NotImplementedException(); }
            }
            public IServiceTemplateConfiguration ServiceTemplateConfiguration
            {
                get { throw new NotImplementedException(); }
            }
        }

        private sealed class MetadataConfigurationProviderMock : IMetadataConfigurationProvider
        {
            private readonly IMetadataConfiguration _configurationMock = new MetadataConfigurationMock
            {
                ConfigurationId = MetadataConfigurationId
            };

            public IMetadataConfiguration GetMetadataConfiguration(string metadataConfigurationId)
            {
                return _configurationMock;
            }

            public IMetadataConfiguration AddConfiguration(string metadataConfigurationId,
                IScriptConfiguration actionConfiguration, bool isEmbeddedConfiguration)
            {
                return _configurationMock;
            }

            public IEnumerable<IMetadataConfiguration> Configurations
            {
                get { return new[] { _configurationMock }; }
            }

            public void RemoveConfiguration(string metadataConfigurationId)
            {
            }
        }

    }
}
