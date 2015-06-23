using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.DesignControls.Controls.Properties;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.DesignControls.PropertyEditors;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DesignControls.Controls.Scripts
{
    public sealed class Script : IPropertiesProvider, ILayoutProvider
    {
        private readonly Dictionary<string, IControlProperty> _properties = new Dictionary<string, IControlProperty>();

        public Script()
        {
            InitProperties();
        }

        public string Name
        {
            get { return _properties["Name"].Value.ToString(); }
        }

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
            return "Script";
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
                {"Body", () => new JsonObjectEditor()}
            };
        }

        public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
        {
            return new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>
            {
                {"Name", Common.CreateNullOrEmptyValidator("Script", "Name")}
            };
        }

        private void InitProperties()
        {
            _properties.Add("Name", new SimpleProperty(string.Empty));
            _properties.Add("Body", new SimpleProperty(string.Empty));
        }
    }
}