using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Hosting;
using InfinniPlatform.Metadata.Implementation.Handlers;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Hosting;

namespace InfinniPlatform.Metadata.Implementation.HostServerConfiguration
{
    public static class InfinniPlatformHostFactory
    {
        /// <summary>
        ///     Регистрация существующих в платформе шаблонов обработчика запросов
        /// </summary>
        /// <returns></returns>
        public static IServiceTemplateConfiguration CreateDefaultServiceConfiguration(
            this IServiceTemplateConfiguration serviceTemplateConfiguration)
        {
            serviceTemplateConfiguration.RegisterServiceTemplate<ApplyChangesHandler>("ApplyEvents",
                "ApplyJsonObject",
                new ExtensionPointHandlerConfig()
                    .AddExtensionPoint("FilterEvents", ContextTypeKind.ApplyFilter)
                    .AddExtensionPoint("Move", ContextTypeKind.ApplyMove)
                    .AddExtensionPoint("GetResult", ContextTypeKind.ApplyResult)
                    .AsVerb(VerbType.Post));

            serviceTemplateConfiguration.RegisterServiceTemplate<ApplyChangesHandler>("ApplyJson", "ApplyJsonObject",
                new ExtensionPointHandlerConfig()
                    .AddExtensionPoint("FilterEvents", ContextTypeKind.ApplyFilter)
                    .AddExtensionPoint("Move", ContextTypeKind.ApplyMove)
                    .AddExtensionPoint("GetResult", ContextTypeKind.ApplyResult)
                    .AsVerb(VerbType.Post));

            //регистрация обработчика пользовательского сервиса, предназначенного для работы с SDK API
            serviceTemplateConfiguration.RegisterServiceTemplate<CustomServiceHandler>("ApiApplyJson", "ApplyJsonObject",
                new ExtensionPointHandlerConfig()
                    .AddExtensionPoint("FilterEvents", ContextTypeKind.ApplyFilter)
                    .AddExtensionPoint("Move", ContextTypeKind.ApplyMove)
                    .AddExtensionPoint("GetResult", ContextTypeKind.ApplyResult)
                    .AsVerb(VerbType.Post));

            serviceTemplateConfiguration.RegisterServiceTemplate<SystemEventsHandler>("Notify", "Notify",
                new ExtensionPointHandlerConfig().AsVerb(VerbType.Post));

            serviceTemplateConfiguration.RegisterServiceTemplate<SearchHandler>("Search", "GetSearchResult",
                new ExtensionPointHandlerConfig().AsVerb(VerbType.Get)
                    .AddExtensionPoint("ValidateFilter", ContextTypeKind.SearchContext)
                    .AddExtensionPoint("SearchModel", ContextTypeKind.SearchContext)
                );

            serviceTemplateConfiguration.RegisterServiceTemplate<UploadHandler>("Upload", "UploadFile",
                new ExtensionPointHandlerConfig()
                    .AsVerb(VerbType.Upload)
                    .AddExtensionPoint("Upload", ContextTypeKind.Upload));

            serviceTemplateConfiguration.RegisterServiceTemplate<UrlEncodedDataHandler>("UrlEncodedData", "Process",
                new ExtensionPointHandlerConfig()
                    .AsVerb(VerbType.UrlEncodedData)
                    .AddExtensionPoint("ProcessUrlEncodedData", ContextTypeKind.UrlEncodedData));


            //serviceTemplateConfiguration.RegisterServiceTemplate<HelpHandler>("Help", "GetHelp",
            //	new ExtensionPointHandlerConfig()
            //		.AsVerb(VerbType.Get)
            //		.AddExtensionPoint("GetHelp", ContextTypeKind.ApplyResult));


            serviceTemplateConfiguration.RegisterServiceTemplate<SearchDocumentAggregationHandler>("Aggregation",
                "GetAggregationDocumentResult", new ExtensionPointHandlerConfig()
                    .AddExtensionPoint("Join", ContextTypeKind.SearchContext)
                    .AddExtensionPoint("TransformResult", ContextTypeKind.SearchContext)
                    .AsVerb(VerbType.Get));

            return serviceTemplateConfiguration;
        }
    }
}