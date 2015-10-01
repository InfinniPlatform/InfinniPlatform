using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.DesignControls.Controls.Properties;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.DesignControls.PropertyEditors;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Validations;

namespace InfinniPlatform.DesignControls.Controls.DataSources
{
    public sealed class DocumentDataSource : IPropertiesProvider, ILayoutProvider, IInspectedItem
    {
        private readonly Dictionary<string, IControlProperty> _properties = new Dictionary<string, IControlProperty>();

        public DocumentDataSource()
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

            foreach (var simpleProperty in _properties)
            {
                var objectProperty = simpleProperty.Value as ObjectProperty;
                if (objectProperty != null)
                {
                    instanceLayout[simpleProperty.Key] = objectProperty.Value;
                }
                else
                {
                    instanceLayout[simpleProperty.Key] = simpleProperty.Value.Value;
                }
            }

            return instanceLayout;
        }

        public void SetLayout(dynamic value)
        {
        }

        public string GetPropertyName()
        {
            return "DocumentDataSource";
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
            return new Dictionary<string, Func<IPropertyEditor>>()
                .InheritBaseDataSourceEditors(ObjectInspector)
                .InheritBaseElementPropertyEditors(ObjectInspector);
        }

        public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
        {
            return new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>
            {
                {"ConfigId", Common.CreateNullOrEmptyValidator("DocumentDataSource", "ConfigId")},
                {"DocumentId", Common.CreateNullOrEmptyValidator("DocumentDataSource", "DocumentId")}
            }
                .InheritBaseElementValidators("DocumentDataSource");
        }

        private void InitProperties()
        {
            _properties.InheritBaseDataSourceSimpleProperties();
            _properties.Add("ConfigId", new SimpleProperty(string.Empty));
            _properties.Add("DocumentId", new SimpleProperty(string.Empty));
            _properties.Add("CreateAction", new SimpleProperty("CreateDocument"));
            _properties.Add("ReadAction", new SimpleProperty("GetDocument"));
            _properties.Add("UpdateAction", new SimpleProperty("SetDocument"));
            _properties.Add("DeleteAction", new SimpleProperty("DeleteDocument"));
            _properties.Add("Query", new SimpleProperty(new List<dynamic>()));
        }

        public override string ToString()
        {
            return GetLayout().ToString();
        }
    }
}