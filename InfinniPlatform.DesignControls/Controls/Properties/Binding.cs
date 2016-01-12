using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Validation;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.DesignControls.PropertyEditors;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DesignControls.Controls.Properties
{
    public static class Binding
    {
        public static Dictionary<string, IControlProperty> GetBindings(
            this Dictionary<string, IControlProperty> properties)
        {
            properties.Add("ObjectBinding", new ObjectProperty(new Dictionary<string, IControlProperty>
            {
                {"Value", new SimpleProperty(new DynamicWrapper())}
            }, new Dictionary<string, CollectionProperty>(),
                new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>
                {
                    {
                        "Value", Common.CreateNullOrEmptyValidator("ObjectBinding", "Value")
                    }
                }.InheritBaseElementValidators("ObjectBinding")
                ));
            properties.Add("PropertyBinding", new ObjectProperty(new Dictionary<string, IControlProperty>
            {
                {"DataSource", new SimpleProperty(string.Empty)},
                {"Property", new SimpleProperty(string.Empty)},
                {"DefaultValue", new SimpleProperty(null)}
            }, new Dictionary<string, CollectionProperty>(),
                new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>
                {
                    {
                        "DataSource", Common.CreateNullOrEmptyValidator("PropertyBinding", "DataSource")
                    },
                    {
                        "Property", Common.CreateNullOrEmptyValidator("PropertyBinding", "Property")
                    }
                }.InheritBaseElementValidators("PropertyBinding")
                ));
            properties.Add("ParameterBinding", new ObjectProperty(new Dictionary<string, IControlProperty>
            {
                {"Parameter", new SimpleProperty(string.Empty)},
                {"Property", new SimpleProperty(string.Empty)}
            }, new Dictionary<string, CollectionProperty>(),
                new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>
                {
                    {
                        "Parameter", Common.CreateNullOrEmptyValidator("ParameterBinding", "Parameter")
                    },
                    {
                        "Property", Common.CreateNullOrEmptyValidator("ParameterBinding", "Property")
                    }
                }.InheritBaseElementValidators("ParameterBinding")));
            return properties;
        }

        public static Dictionary<string, Func<IPropertyEditor>> InheritBindingPropertyEditors(
            this Dictionary<string, Func<IPropertyEditor>> propertyEditors, ObjectInspectorTree inspector)
        {
            propertyEditors.Add("DataSource", () => new DataSourceEditor(inspector));
            return propertyEditors;
        }
    }
}