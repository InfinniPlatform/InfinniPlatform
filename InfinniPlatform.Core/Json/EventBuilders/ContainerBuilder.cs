using System;
using System.Linq;
using InfinniPlatform.Sdk.Application.Events;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json.EventBuilders
{
    public class ContainerBuilder : IJsonObjectBuilder
    {
        public void BuildJObject(JToken backboneObject, EventDefinition eventDefinition)
        {
            if (backboneObject == null)
            {
                throw new ArgumentException("object to apply event is not defined");
            }

            backboneObject = GetParent(backboneObject, eventDefinition);

            var parentObject = backboneObject is JObject ? backboneObject as JObject : backboneObject.First() as JObject;
            if (parentObject == null)
            {
                throw new ArgumentException("object to add component is not an object.");
            }

            UpdateProperty(GetPropertyName(eventDefinition.Property), parentObject);
        }

        private void UpdateProperty(string propertyName, JObject parentObject)
        {
            var prop = parentObject.Properties().FirstOrDefault(p => p.Name == propertyName);
            if (prop != null)
            {
                prop.Value = new JObject();
            }
            else
            {
                parentObject.Add(propertyName, new JObject());
            }
        }

        private string GetPropertyName(string property)
        {
            return property.Split('.').LastOrDefault();
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