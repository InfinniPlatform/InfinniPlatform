using System.Collections;

using InfinniPlatform.UserInterface.Services.Metadata;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigSelector
{
	internal sealed class ConfigSelectorElementBuilder : IObjectBuilder
	{
		public ConfigSelectorElementBuilder(string server, int port, string routeVersion)
		{
			_server = server;
			_port = port;
			_routeVersion = routeVersion;
		}

		private readonly int _port;
		private readonly string _routeVersion;
		private readonly string _server;
		private dynamic _version;

		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var element = new ConfigSelectorElement(parent);
			element.ApplyElementMeatadata((object)metadata);
			_version = context.AppView.GetContext().Version;
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

		IEnumerable GetConfigurations()
		{
			return new ConfigurationMetadataService().GetItems();
		}
	}
}