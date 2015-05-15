using System.Collections;

using InfinniPlatform.UserInterface.Services.Metadata;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigSelector
{
	sealed class ConfigSelectorElementBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var element = new ConfigSelectorElement(parent);
			element.ApplyElementMeatadata((object)metadata);
			element.SetConfigurationsFunc(GetConfigurations);

			// Привязка к источнику данных

			IElementDataBinding valueBinding = context.Build(parent, metadata.Value);

			if (valueBinding != null)
			{
				valueBinding.OnPropertyValueChanged += (c, a) => element.SetValue(a.Value);
				element.OnValueChanged += (c, a) => valueBinding.SetPropertyValue(a.Value);
			}

			return element;
		}


		private static IEnumerable GetConfigurations()
		{
			return ConfigurationMetadataService.Instance.GetItems();
		}
	}
}