using InfinniPlatform.Sdk.Events;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Core.Json
{
    public interface IJsonObjectBuilder
    {
        void BuildJObject(JToken backboneObject, EventDefinition eventDefinition);
    }
}