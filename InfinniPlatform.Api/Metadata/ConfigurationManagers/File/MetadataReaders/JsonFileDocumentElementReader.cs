using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.File.MetadataReaders
{
    public sealed class JsonFileDocumentElementReader : IDataReader
    {
        private readonly dynamic _document;
        private readonly string _metadataType;

        public JsonFileDocumentElementReader(dynamic document, string metadataType)
        {
            _document = document;
            _metadataType = metadataType;
        }

        public IEnumerable<dynamic> GetItems()
        {
            if (_metadataType == MetadataType.Scenario)
            {
                return _document.Scenarios;
            }
            if (_metadataType == MetadataType.Process)
            {
                return _document.Processes;
            }
            if (_metadataType == MetadataType.Service)
            {
                return _document.Services;
            }
            if (_metadataType == MetadataType.Generator)
            {
                return _document.Generators;
            }
            return null;
        }

        public dynamic GetItem(string metadataName)
        {
            if (_metadataType == MetadataType.Scenario)
            {
                IEnumerable<dynamic> scenarios = _document.Scenarios;
                return scenarios.FirstOrDefault(sc => sc.Name == metadataName);
            }
            if (_metadataType == MetadataType.Process)
            {
                IEnumerable<dynamic> processes = _document.Processes;
                return processes.FirstOrDefault(sc => sc.Name == metadataName);
            }
            if (_metadataType == MetadataType.Service)
            {
                IEnumerable<dynamic> services = _document.Services;
                return services.FirstOrDefault(sc => sc.Name == metadataName);
            }
            if (_metadataType == MetadataType.Generator)
            {
                IEnumerable<dynamic> generators = _document.Generators;
                return generators.FirstOrDefault(sc => sc.Name == metadataName);
            }
            return null;
        }
    }
}