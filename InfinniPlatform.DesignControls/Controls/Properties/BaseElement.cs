using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.DesignControls.PropertyEditors;

namespace InfinniPlatform.DesignControls.Controls.Properties
{
	public static class BaseElement
	{
		public static Dictionary<string, IControlProperty> InheritBaseElementSimpleProperties(
            this Dictionary<string, IControlProperty> properties)
		{
			properties.Add("Name", new SimpleProperty(string.Empty));
            properties.Add("Text", new SimpleProperty(string.Empty));
            properties.Add("Enabled", new SimpleProperty(true));
            properties.Add("Visible", new SimpleProperty(true));
			properties.Add("VerticalAlignment", new SimpleProperty("Stretch"));
			properties.Add("HorizontalAlignment", new SimpleProperty("Stretch"));
            properties.Add("OnLoaded", new ObjectProperty(new Dictionary<string, IControlProperty>()
				                                             {
					                                             {"Name",new SimpleProperty(string.Empty)}
				                                             }, new Dictionary<string, CollectionProperty>()));
			return properties;
		}

        public static Dictionary<string, IControlProperty> InheritBaseElementValueBinding(this Dictionary<string, IControlProperty> properties)
		{
            var itemsValue = new ObjectProperty(new Dictionary<string, IControlProperty>()
            {
                {
                    "PropertyBinding", new ObjectProperty(new Dictionary<string, IControlProperty>() {
                                                {"DataSource",new SimpleProperty(string.Empty)},
                                                {"Property",new SimpleProperty(string.Empty)},
                                                {"DefaultValue",new SimpleProperty(string.Empty)}
                                    }, new Dictionary<string, CollectionProperty>())
                }
            }, new Dictionary<string, CollectionProperty>());

			properties.Add("Value", itemsValue);
			return properties;
		}


		public static Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> InheritBaseElementValidators(
			this Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> validators, string objectName) 			
		{
			validators.Add("Name", Common.CreateNullOrEmptyValidator(objectName, "Name"));
			validators.Add("Enabled", Common.CreateNullOrEmptyValidator(objectName, "Enabled"));
			validators.Add("Visible", Common.CreateNullOrEmptyValidator(objectName, "Visible"));
			validators.Add("VerticalAlignment", Common.CreateNullOrEmptyValidator(objectName, "VerticalAlignment"));
			validators.Add("HorizontalAlignment", Common.CreateNullOrEmptyValidator(objectName, "HorizontalAlignment"));

			return validators;
		}

		public static Dictionary<string, Func<IPropertyEditor>> InheritBaseElementPropertyEditors(
			this Dictionary<string, Func<IPropertyEditor>> propertyEditors, ObjectInspectorTree inspector)
		{
			propertyEditors.Add("VerticalAlignment",() => new AlignmentEditor());
			propertyEditors.Add("HorizontalAlignment", () => new AlignmentEditor());
			propertyEditors.Add("Enabled", () => new BooleanEditor());
			propertyEditors.Add("Visible", () => new BooleanEditor());
			propertyEditors.Add("OnLoaded.Name", () => new ScriptIdEditor(inspector));
			propertyEditors.Add("OnValueChanged.Name", () => new ScriptIdEditor(inspector));

			return propertyEditors;
		}


	}
}
