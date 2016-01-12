using InfinniPlatform.Core.Hosting;

namespace InfinniPlatform.SystemConfig.RequestHandlers
{
    /// <summary>
    /// Регистратор стандартных шаблонов обработчиков запроса
    /// </summary>
    internal sealed class RequestHandlerInstaller : IRequestHandlerInstaller
    {
        public RequestHandlerInstaller(IServiceTemplateConfiguration serviceTemplateConfiguration)
        {
            _serviceTemplateConfiguration = serviceTemplateConfiguration;
        }

        private readonly IServiceTemplateConfiguration _serviceTemplateConfiguration;

        /// <summary>
        /// Зарегистрировать шаблоны в конфигурации
        /// </summary>
        public void RegisterTemplates()
        {
            _serviceTemplateConfiguration.RegisterServiceTemplate<ApplyChangesHandler>("ApplyEvents",
                "ApplyJsonObject",
                new ExtensionPointHandlerConfig()
                    .AddExtensionPoint("FilterEvents", ContextTypeKind.ApplyFilter)
                    .AddExtensionPoint("Move", ContextTypeKind.ApplyMove)
                    .AddExtensionPoint("GetResult", ContextTypeKind.ApplyResult)
                    .AsVerb(VerbType.Post));

            _serviceTemplateConfiguration.RegisterServiceTemplate<ApplyChangesHandler>("ApplyJson", "ApplyJsonObject",
                new ExtensionPointHandlerConfig()
                    .AddExtensionPoint("FilterEvents", ContextTypeKind.ApplyFilter)
                    .AddExtensionPoint("Move", ContextTypeKind.ApplyMove)
                    .AddExtensionPoint("GetResult", ContextTypeKind.ApplyResult)
                    .AsVerb(VerbType.Post));

            _serviceTemplateConfiguration.RegisterServiceTemplate<SearchHandler>("Search", "GetSearchResult",
                new ExtensionPointHandlerConfig().AsVerb(VerbType.Get)
                                                 .AddExtensionPoint("ValidateFilter", ContextTypeKind.SearchContext)
                                                 .AddExtensionPoint("SearchModel", ContextTypeKind.SearchContext)
                );

            _serviceTemplateConfiguration.RegisterServiceTemplate<UploadHandler>("Upload", "UploadFile",
                new ExtensionPointHandlerConfig()
                    .AsVerb(VerbType.Upload)
                    .AddExtensionPoint("Upload", ContextTypeKind.Upload));

            _serviceTemplateConfiguration.RegisterServiceTemplate<UrlEncodedDataHandler>("UrlEncodedData", "Process",
                new ExtensionPointHandlerConfig()
                    .AsVerb(VerbType.UrlEncodedData)
                    .AddExtensionPoint("ProcessUrlEncodedData", ContextTypeKind.UrlEncodedData));

            _serviceTemplateConfiguration.RegisterServiceTemplate<SearchDocumentAggregationHandler>("Aggregation",
                "GetAggregationDocumentResult", new ExtensionPointHandlerConfig()
                    .AddExtensionPoint("Join", ContextTypeKind.SearchContext)
                    .AddExtensionPoint("TransformResult", ContextTypeKind.SearchContext)
                    .AsVerb(VerbType.Get));
        }
    }
}