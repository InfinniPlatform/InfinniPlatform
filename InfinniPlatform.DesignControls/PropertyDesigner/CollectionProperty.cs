using System.Collections.Generic;

namespace InfinniPlatform.DesignControls.PropertyDesigner
{
    public sealed class CollectionProperty : IControlProperty
    {
        private readonly Dictionary<string, IControlProperty> _properties;

        public CollectionProperty(Dictionary<string, IControlProperty> properties)
        {
            _properties = properties;
            Items = new List<dynamic>();
        }

        public List<dynamic> Items { get; set; }

        public Dictionary<string, IControlProperty> Properties
        {
            get { return _properties; }
        }

        public dynamic Value
        {
            get { return Items; }
            set { Items = value; }
        }

        public CollectionProperty Clone(bool newItem)
        {
            var cloneProperties = new Dictionary<string, IControlProperty>();

            foreach (var property in Properties)
            {
                if (property.Value is ObjectProperty)
                {
                    var objectProperty = property.Value as ObjectProperty;
                    cloneProperties.Add(property.Key, objectProperty.Clone(newItem));
                }
                else if (property.Value is CollectionProperty)
                {
                    var collectionProperty = property.Value as CollectionProperty;
                    cloneProperties.Add(property.Key, collectionProperty.Clone(newItem));
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

            var cloneProperty = new CollectionProperty(cloneProperties);

            cloneProperty.Items = Items;
            return cloneProperty;
        }
    }
}