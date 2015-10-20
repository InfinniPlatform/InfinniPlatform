using System;
using System.Linq;
using InfinniPlatform.Sdk.Events;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json.EventBuilders
{
    public class PropertyBuilder : IJsonObjectBuilder
    {
        public void BuildJObject(JToken backboneObject, EventDefinition eventDefinition)
        {
            var parent = GetParent(backboneObject, eventDefinition);

            if (parent == null)
            {
                throw new ArgumentException("object to apply event is not defined");
            }

            var ownerProperty = parent is JObject ? parent as JObject : parent.First() as JObject;
            if (ownerProperty == null)
            {
                throw new ArgumentException("specified json object is not an object.");
            }

            UpdateProperty(GetPropertyName(eventDefinition.Property), ownerProperty, eventDefinition.Value);
        }

        private string GetPropertyName(string property)
        {
            return property.Split('.').LastOrDefault();
        }

        private static void UpdateProperty(string propertyName, JObject parentObject, object value)
        {
            var jProperty = parentObject.Property(propertyName);
            var jValue = (value != null) ? ((value is JToken) ? (JToken)value : JToken.FromObject(value)) : null;

            if (jProperty != null)
            {
                jProperty.Value = jValue;
            }
            else
            {
                parentObject.Add(propertyName, jValue);
            }
        }

        private static JToken GetParent(JToken startObject, EventDefinition eventDefinition)
        {
            var path = eventDefinition.Property.Split('.');

            if (path.Count() == 1)
            {
                return startObject;
            }

            return new JsonParser().FindJsonToken(startObject, string.Join(".", path.Take(path.Count() - 1))).FirstOrDefault();
        }
    }
}