using System;
using System.Linq;
using InfinniPlatform.Logging;
using InfinniPlatform.Sdk.Events;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json.EventBuilders
{
    public class ObjectRemoveBuilder : IJsonObjectBuilder
    {
        public void BuildJObject(JToken backboneObject, EventDefinition eventDefinition)
        {
            if (backboneObject == null)
            {
                throw new ArgumentException("object to apply event is not defined");
            }

            var objectToRemove =
                new JsonParser().FindJsonToken(backboneObject, eventDefinition.Property).FirstOrDefault();

            if (objectToRemove == null)
            {
                Logger.Log.Error("object to remove not found: \"{0}\"", eventDefinition.Property);
                return;
            }

            objectToRemove.Remove();
        }
    }
}