using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.DesignControls.Controls.Properties;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.DesignControls.PropertyEditors;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DesignControls.Controls.DataSources
{
    public sealed class ObjectDataSource : IPropertiesProvider, ILayoutProvider, IInspectedItem
    {
        private readonly Dictionary<string, IControlProperty> _properties = new Dictionary<string, IControlProperty>();

        public ObjectDataSource()
        {
            InitProperties();
        }

        public string Name
        {
            get { return _properties["Name"].Value.ToString(); }
        }

        public ObjectInspectorTree ObjectInspector { get; set; }

        public dynamic GetLayout()
        {
            dynamic instanceLayout = new DynamicWrapper();

            instanceLayout.Items = DynamicWrapperExtensions.ToDynamicList(_properties["Items"].Value.ToString());

            foreach (var simpleProperty in _properties)
            {
                var objectProperty = simpleProperty.Value as ObjectProperty;

                instanceLayout[simpleProperty.Key] =
                    objectProperty != null ? objectProperty.Value : simpleProperty.Value.Value;
            }


            return instanceLayout;
        }

        public void SetLayout(dynamic value)
        {
        }

        public string GetPropertyName()
        {
            return "ObjectDataSource";
        }

        public void ApplySimpleProperties()
        {
        }

        public void ApplyCollections()
        {
        }

        public Dictionary<string, IControlProperty> GetSimpleProperties()
        {
            return _properties;
        }

        public Dictionary<string, CollectionProperty> GetCollections()
        {
            return new Dictionary<string, CollectionProperty>();
        }

        public void LoadProperties(dynamic value)
        {
            DesignerExtensions.SetSimplePropertiesFromInstance(_properties, value);
        }

        public Dictionary<string, Func<IPropertyEditor>> GetPropertyEditors()
        {
            return new Dictionary<string, Func<IPropertyEditor>>
            {
                {"Items", () => new JsonObjectEditor()}
            }
                .InheritBaseDataSourceEditors(ObjectInspector)
                .InheritBaseElementPropertyEditors(ObjectInspector);
        }

        public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
        {
            return new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>()
                .InheritBaseElementValidators("ObjectDataSource");
        }

        private void InitProperties()
        {
            _properties.InheritBaseDataSourceSimpleProperties();
            _properties.Add("Items", new SimpleProperty(new List<dynamic>()));
        }

        public override string ToString()
        {
            return GetLayout().ToString();
        }
    }
}