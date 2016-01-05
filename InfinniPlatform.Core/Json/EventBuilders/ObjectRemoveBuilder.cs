using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Logging;
using InfinniPlatform.Sdk.Events;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Core.Json.EventBuilders
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
                Logger.Log.Warn("Object to remove is not found.", new Dictionary<string, object>
                                                                                {
                                                                                    { "object", eventDefinition.Property },
                                                                                });

                return;
            }

            objectToRemove.Remove();
        }
    }
}