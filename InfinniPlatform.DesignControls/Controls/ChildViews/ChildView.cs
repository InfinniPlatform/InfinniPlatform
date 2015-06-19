using System;
using System.Collections.Generic;
using System.Windows.Forms;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.DesignControls.PropertyEditors;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.DesignControls.Controls.ChildViews
{
    public sealed class ChildView : IPropertiesProvider, ILayoutProvider, IInspectedItem
    {
        private readonly Dictionary<string, IControlProperty> _properties = new Dictionary<string, IControlProperty>();

        public ChildView()
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

            try
            {
                if (instanceLayout.LinkView != null)
                {
                    instanceLayout.LinkView = ((object) instanceLayout.LinkView.ToString()).ToDynamic();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Fail to parse child view");
                instanceLayout.LinkView = null;
            }

            return instanceLayout;
        }

        public void SetLayout(dynamic value)
        {
        }

        public string GetPropertyName()
        {
            return "ChildView";
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
                {"LinkView", () => new JsonObjectEditor()}
            };
        }

        public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
        {
            return new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>();
        }

        private void InitProperties()
        {
            _properties.Add("Name", new SimpleProperty(string.Empty));
            _properties.Add("LinkView", new SimpleProperty(null));
        }

        public override string ToString()
        {
            return GetLayout().ToString();
        }
    }
}