using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Core.Metadata.ConfigurationManagers.File
{
    public class JsonFileRegisterReader : IDataReader
    {
        private readonly dynamic _configObject;

        public JsonFileRegisterReader(dynamic configObject)
        {
            _configObject = configObject;
        }

        public IEnumerable<dynamic> GetItems()
        {
            return _configObject.Registers;
        }

        public dynamic GetItem(string metadataName)
        {
            IEnumerable<dynamic> regs = _configObject.Registers;
            return regs.FirstOrDefault(d => d.Name == metadataName);
        }
    }
}