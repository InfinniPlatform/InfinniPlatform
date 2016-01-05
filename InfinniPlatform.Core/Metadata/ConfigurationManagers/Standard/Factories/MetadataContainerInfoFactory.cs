using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Metadata.MetadataContainers;
using InfinniPlatform.Core.Properties;

namespace InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.Factories
{
    /// <summary>
    ///     Фабрика контейнеров метаданных
    /// </summary>
    public sealed class MetadataContainerInfoFactory
    {
        private readonly Dictionary<string, IMetadataContainerInfo> _metadataContainerInfoList =
            new Dictionary<string, IMetadataContainerInfo>();

        public MetadataContainerInfoFactory()
        {
            _metadataContainerInfoList.Add(MetadataType.View, new MetadataContainerView());
            _metadataContainerInfoList.Add(MetadataType.Document, new MetadataContainerDocument());
            _metadataContainerInfoList.Add(MetadataType.Report, new MetadataContainerReport());
            _metadataContainerInfoList.Add(MetadataType.Service, new MetadataContainerService());
            _metadataContainerInfoList.Add(MetadataType.Process, new MetadataContainerProcess());
            _metadataContainerInfoList.Add(MetadataType.Scenario, new MetadataContainerScenario());
            _metadataContainerInfoList.Add(MetadataType.Menu, new MetadataContainerMenu());
            _metadataContainerInfoList.Add(MetadataType.Generator, new MetadataContainerGenerator());
            _metadataContainerInfoList.Add(MetadataType.Assembly, new MetadataContainerAssembly());
            _metadataContainerInfoList.Add(MetadataType.ValidationError, new MetadataContainerValidationErrors());
            _metadataContainerInfoList.Add(MetadataType.ValidationWarning, new MetadataContainerValidationWarnings());
            _metadataContainerInfoList.Add(MetadataType.Status, new MetadataContainerStatus());
            _metadataContainerInfoList.Add(MetadataType.PrintView, new MetadataContainerPrintView());
        }

        public IMetadataContainerInfo BuildMetadataContainerInfo(string metadataType)
        {
            if (_metadataContainerInfoList.ContainsKey(metadataType))
            {
                return _metadataContainerInfoList[metadataType];
            }
            throw new ArgumentException(Resources.MetadataContainerForSpecifiedTypeNotFound);
        }
    }
}