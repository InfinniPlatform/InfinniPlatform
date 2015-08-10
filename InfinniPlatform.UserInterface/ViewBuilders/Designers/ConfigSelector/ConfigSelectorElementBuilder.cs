using System.Collections;
using InfinniPlatform.UserInterface.Services.Metadata;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigSelector
{
    internal sealed class ConfigSelectorElementBuilder : IObjectBuilder
    {
        private readonly string _server;
        private readonly int _port;
        private readonly string _routeVersion;

        public ConfigSelectorElementBuilder(string server, int port, string routeVersion)
        {
            _server = server;
            _port = port;
            _routeVersion = routeVersion;
        }

        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var element = new ConfigSelectorElement(parent);
            element.ApplyElementMeatadata((object) metadata);
            element.SetConfigurationsFunc(GetConfigurations(context.AppView.GetContext().Version));

            // Привязка к источнику данных

            IElementDataBinding valueBinding = context.Build(parent, metadata.Value);

            if (valueBinding != null)
            {
                valueBinding.OnPropertyValueChanged += (c, a) => element.SetValue(a.Value);
                element.OnValueChanged += (c, a) => valueBinding.SetPropertyValue(a.Value);
            }

            return element;
        }

        private IEnumerable GetConfigurations(string version)
        {
            return new ConfigurationMetadataService(version, _server, _port, _routeVersion).GetItems();
        }
    }
}