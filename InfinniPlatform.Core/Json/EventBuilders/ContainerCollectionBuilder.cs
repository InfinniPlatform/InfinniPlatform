using System;
using System.Linq;

using InfinniPlatform.Sdk.Events;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Core.Json.EventBuilders
{
    public class ContainerCollectionBuilder : IJsonObjectBuilder
    {
        public void BuildJObject(JToken backboneObject, EventDefinition eventDefinition)
        {
            backboneObject = GetParent(backboneObject, eventDefinition);

            if (backboneObject == null)
            {
                throw new ArgumentException("object to apply event is not defined");
            }

            var parentObject = backboneObject is JObject ? backboneObject as JObject : backboneObject.First() as JObject;
            if (parentObject == null)
            {
                throw new ArgumentException("object to add an array property is not an object.");
            }

            UpdateProperty(GetPropertyName(eventDefinition.Property), parentObject);
        }

        private string GetPropertyName(string property)
        {
            return property.Split('.').LastOrDefault();
        }

        private void UpdateProperty(string propertyName, JObject parentObject)
        {
            var prop = parentObject.Properties().FirstOrDefault(p => p.Name == propertyName);

            if (prop == null)
            {
                parentObject.Add(propertyName, new JArray());
            }
        }

        private JToken GetParent(JToken startObject, dynamic eventDefinition)
        {
            var path = ((string) eventDefinition.Property).Split('.');
            if (path.Count() == 1)
            {
                return startObject;
            }
            return
                new JsonParser().FindJsonToken(startObject, string.Join(".", path.Take(path.Count() - 1)))
                    .FirstOrDefault();
        }
    }
}