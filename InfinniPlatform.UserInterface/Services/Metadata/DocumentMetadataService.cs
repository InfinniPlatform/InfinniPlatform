using System;
using System.Threading;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
    /// <summary>
    ///     Сервис для работы с метаданными документов.
    /// </summary>
    internal sealed class DocumentMetadataService : BaseMetadataService
    {
        private readonly Lazy<ManagerFactoryConfiguration> _factory;

        public DocumentMetadataService(string version, string configId)
        {
            _factory = new Lazy<ManagerFactoryConfiguration>(() => new ManagerFactoryConfiguration(version, configId),
                LazyThreadSafetyMode.ExecutionAndPublication);
        }

        protected override IDataReader CreateDataReader()
        {
            return _factory.Value.BuildDocumentMetadataReader();
        }

        protected override IDataManager CreateDataManager()
        {
            return _factory.Value.BuildDocumentManager();
        }
    }
}