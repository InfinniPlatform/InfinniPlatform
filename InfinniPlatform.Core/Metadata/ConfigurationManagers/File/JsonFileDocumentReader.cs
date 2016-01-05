using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Core.Metadata.ConfigurationManagers.File
{
    public class JsonFileDocumentReader : IDataReader
    {
        private readonly dynamic _configObject;

        public JsonFileDocumentReader(dynamic configObject)
        {
            _configObject = configObject;
        }

        public IEnumerable<dynamic> GetItems()
        {
            return _configObject.Documents;
        }

        public dynamic GetItem(string metadataName)
        {
            IEnumerable<dynamic> docs = _configObject.Documents;
            return docs.FirstOrDefault(d => d.Name == metadataName);
        }
    }
}