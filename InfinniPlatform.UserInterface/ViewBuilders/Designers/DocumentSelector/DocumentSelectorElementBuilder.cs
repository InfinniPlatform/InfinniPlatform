using System.Collections;
using InfinniPlatform.UserInterface.Services.Metadata;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.DocumentSelector
{
    internal sealed class DocumentSelectorElementBuilder : IObjectBuilder
    {
        private readonly string _server;
        private readonly int _port;
        private readonly string _routeVersion;

        public DocumentSelectorElementBuilder(string server, int port, string routeVersion)
        {
            _server = server;
            _port = port;
            _routeVersion = routeVersion;
        }

        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var element = new DocumentSelectorElement(parent);
            element.ApplyElementMeatadata((object) metadata);
            element.SetDocumentsFunc((version, configId) => GetDocuments(version, configId));

            // Привязка к источнику данных

            IElementDataBinding configIdBinding = context.Build(parent, metadata.ConfigId);

            if (configIdBinding != null)
            {
                configIdBinding.OnPropertyValueChanged += (c, a) => element.SetConfigId(a.Value);
            }

            IElementDataBinding valueBinding = context.Build(parent, metadata.Value);

            if (valueBinding != null)
            {
                valueBinding.OnPropertyValueChanged += (c, a) => element.SetValue(a.Value);
                element.OnValueChanged += (c, a) => valueBinding.SetPropertyValue(a.Value);
            }

            return element;
        }

        private IEnumerable GetDocuments(string version, string configId)
        {
            if (!string.IsNullOrWhiteSpace(configId))
            {
                var documentService = new DocumentMetadataService(version, configId, _server, _port, _routeVersion);
                return documentService.GetItems();
            }

            return null;
        }
    }
}