using System;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;

namespace InfinniPlatform.Api.Tests.RestBehavior.ConfiguratorApiBehavior
{
    public class CrudSettings
    {
        public const string ConfigurationFirstId = "TestConfigurationCRUD";

        public const string ConfigurationSecondId = "TestConfigurationCRUD1";

        public const string ConfigurationDescription = "Тестовая конфигурация";
        private readonly string _metadataType;

        public CrudSettings(string metadataType)
        {
            _metadataType = metadataType;
        }

        public string MetadataType
        {
            get { return _metadataType; }
        }

        public string FirstMetadataName { get; set; }

        public string SecondMetadataName { get; set; }

        public string FirstMetadataId { get; set; }

        public string SecondMetadataId { get; set; }

        public Func<string, string, dynamic> BuildInstanceAction { get; set; }

        public Action<CrudSettings> AdditionalOperationCheck { get; set; }

        public dynamic FirstInstance
        {
            get { return BuildInstanceAction(FirstMetadataId, FirstMetadataName); }
        }

        public dynamic SecondInstance
        {
            get { return BuildInstanceAction(SecondMetadataId, SecondMetadataName); }
        }

        public Func<IDataReader> Reader { get; set; }

        public Func<IDataManager> Manager { get; set; }

        public Action InitTest { get; set; }

        public override string ToString()
        {
            return _metadataType;
        }


        public ManagerFactoryDocument MetadataFactoryDocument(string documentId)
        {
            return new ManagerFactoryDocument(null, ConfigurationFirstId, documentId);
        }

        public void CheckAdditionalMetadataOperations()
        {
            if (AdditionalOperationCheck != null)
            {
                AdditionalOperationCheck(this);
            }
        }

        public static dynamic BuildTestConfig(string configUid, string configName)
        {
            MetadataManagerConfiguration managerConfig = ManagerFactoryConfiguration.BuildConfigurationManager(null);
            dynamic existConfig = managerConfig.CreateItem(configName);
            existConfig.Id = configUid;
            existConfig.Name = configName;

            managerConfig.DeleteItem(existConfig);
            managerConfig.MergeItem(existConfig);

            return existConfig;
        }
    }
}