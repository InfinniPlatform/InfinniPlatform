using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.DesignControls.PropertyDesigner;

namespace InfinniPlatform.DesignControls.Controls.Properties
{
	public static class BaseLinkView
	{
        public static Dictionary<string, IControlProperty> InheritBaseLinkViewSimpleProperties(this Dictionary<string, IControlProperty> properties)
		{
			properties.Add("OpenMode", new SimpleProperty("TabPage"));
			return properties;
		}

		public static Dictionary<string, CollectionProperty> InheritBaseLinkViewCollectionProperties(
			this Dictionary<string, CollectionProperty> collectionProperties)
		{
            collectionProperties.Add("Parameters", new CollectionProperty(new Dictionary<string, IControlProperty>() 
                                                                       {
                                                                           {"Name",new SimpleProperty(string.Empty)},
                                                                           {"Value", new ObjectProperty(new Dictionary<string, IControlProperty>().GetBindings(), new Dictionary<string, CollectionProperty>())}
                                                                       }));
		    return collectionProperties;
		}

		public static Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> InheritBaseLinkViewValidators(
			this Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> validators, string objectName)
		{
			validators.Add("OpenMode", Common.CreateNullOrEmptyValidator(objectName, "OpenMode"));

			return validators;
		}

	}
}
