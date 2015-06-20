using System;
using System.Threading;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
    /// <summary>
    ///     Сервис для работы с метаданными проверки документов на предупреждения.
    /// </summary>
    internal sealed class ValidationWarningMetadataService : BaseMetadataService
    {
        private readonly Lazy<ManagerFactoryDocument> _factory;

        public ValidationWarningMetadataService(string version, string configId, string documentId)
        {
            _factory = new Lazy<ManagerFactoryDocument>(
                () => new ManagerFactoryDocument(version, configId, documentId),
                LazyThreadSafetyMode.ExecutionAndPublication);
        }

        protected override IDataReader CreateDataReader()
        {
            return _factory.Value.BuildValidationWarningsMetadataReader();
        }

        protected override IDataManager CreateDataManager()
        {
            return _factory.Value.BuildValidationWarningsManager();
        }
    }
}