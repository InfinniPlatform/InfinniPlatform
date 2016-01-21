using System.Collections.Generic;

using InfinniPlatform.Core.Metadata;

namespace InfinniPlatform.SystemConfig.Metadata
{
    internal sealed class MetadataApi : IMetadataApi
    {
        private readonly IConfigurationMetadataProvider _configurationMetadataProvider;

        public MetadataApi(IConfigurationMetadataProvider configurationMetadataProvider)
        {
            _configurationMetadataProvider = configurationMetadataProvider;
        }

        public IEnumerable<string> GetMenuNames(string configuration)
        {
            return _configurationMetadataProvider.GetConfiguration(configuration).GetMenuNames();
        }

        public dynamic GetMenu(string configuration, string menuName)
        {
            return _configurationMetadataProvider.GetConfiguration(configuration).GetMenu(menuName);
        }

        public IEnumerable<string> GetRegisterNames(string configuration)
        {
            return _configurationMetadataProvider.GetConfiguration(configuration).GetRegisterNames();
        }

        public dynamic GetRegister(string configuration, string registerName)
        {
            return _configurationMetadataProvider.GetConfiguration(configuration).GetRegister(registerName);
        }

        public IEnumerable<string> GetDocumentNames(string configuration)
        {
            return _configurationMetadataProvider.GetConfiguration(configuration).GetDocumentNames();
        }

        public dynamic GetDocumentSchema(string configuration, string documentName)
        {
            return _configurationMetadataProvider.GetConfiguration(configuration).GetDocumentSchema(documentName);
        }

        public dynamic GetDocumentEvents(string configuration, string documentName)
        {
            return _configurationMetadataProvider.GetConfiguration(configuration).GetDocumentEvents(documentName);
        }

        public IEnumerable<string> GetActionNames(string configuration, string documentName)
        {
            return _configurationMetadataProvider.GetConfiguration(configuration).GetActionNames(documentName);
        }

        public dynamic GetAction(string configuration, string documentName, string actionName)
        {
            return _configurationMetadataProvider.GetConfiguration(configuration).GetAction(documentName, actionName);
        }

        public IEnumerable<string> GetViewNames(string configuration, string documentName)
        {
            return _configurationMetadataProvider.GetConfiguration(configuration).GetViewNames(documentName);
        }

        public dynamic GetView(string configuration, string documentName, string viewName)
        {
            return _configurationMetadataProvider.GetConfiguration(configuration).GetView(documentName, viewName);
        }

        public IEnumerable<string> GetPrintViewNames(string configuration, string documentName)
        {
            return _configurationMetadataProvider.GetConfiguration(configuration).GetPrintViewNames(documentName);
        }

        public dynamic GetPrintView(string configuration, string documentName, string printViewName)
        {
            return _configurationMetadataProvider.GetConfiguration(configuration).GetPrintView(documentName, printViewName);
        }
    }
}