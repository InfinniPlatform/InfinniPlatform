using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Validation;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DesignControls.PropertyDesigner
{
    public sealed class ObjectProperty : IControlProperty
    {
        public ObjectProperty(Dictionary<string, IControlProperty> simpleProperties,
            Dictionary<string, CollectionProperty> collectionProperties,
            Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> validationRules = null)
        {
            SimpleProperties = simpleProperties ?? new Dictionary<string, IControlProperty>();
            CollectionProperties = collectionProperties ?? new Dictionary<string, CollectionProperty>();
            ValidationRules = validationRules ?? new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>();
            Value = new DynamicWrapper();
        }

        public Dictionary<string, IControlProperty> SimpleProperties { get; set; }
        public Dictionary<string, CollectionProperty> CollectionProperties { get; set; }
        public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> ValidationRules { get; set; }
        public dynamic Value { get; set; }

        public ObjectProperty Clone(bool newItem)
        {
            var cloneProperties = new Dictionary<string, IControlProperty>();
            foreach (var property in SimpleProperties)
            {
                var objectProperty = property.Value as ObjectProperty;
                if (objectProperty != null)
                {
                    cloneProperties.Add(property.Key, objectProperty.Clone(newItem));
                }
                else
                {
                    var simpleProperty = property.Value as SimpleProperty;
                    if (simpleProperty != null)
                    {
                        simpleProperty.Value = simpleProperty.DefaultValue;
                    }
                    cloneProperties.Add(property.Key, property.Value);
                }
            }

            var cloneCollectionProperties = new Dictionary<string, CollectionProperty>();

            foreach (var collectionProperty in CollectionProperties)
            {
                cloneCollectionProperties.Add(collectionProperty.Key, collectionProperty.Value.Clone(newItem));
            }

            var result = new ObjectProperty(cloneProperties, cloneCollectionProperties, ValidationRules);
            return result;
        }
    }
}