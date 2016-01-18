using System;

using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Services;
using InfinniPlatform.SystemConfig.ActionUnits.Documents;
using InfinniPlatform.SystemConfig.ActionUnits.Metadata;
using InfinniPlatform.SystemConfig.ActionUnits.Session;
using InfinniPlatform.SystemConfig.RequestHandlers;

namespace InfinniPlatform.SystemConfig.Services
{
    /// <summary>
    /// Сервисы системы.
    /// </summary>
    /// <remarks>
    /// TODO: Сделать декомпозицию этого класса. Каждая подсистема должна самостоятельно регистрировать сервисы, которые она предоставляет.
    /// </remarks>
    internal class SystemHttpService : IHttpService
    {
        public delegate ChangeHttpRequestHandler ChangeHttpRequestHandlerFactory(Action<IActionContext> action);


        public SystemHttpService(IContainerResolver containerResolver,
                                 ChangeHttpRequestHandlerFactory onChangeHandlerFactory,
                                 AttachHttpRequestHandler attachHandler,
                                 DownloadHttpRequestHandler downloadHandler,
                                 ReportHttpRequestHandler reportHandler,
                                 CustomHttpRequestHandler customHandler)
        {
            _containerResolver = containerResolver;
            _onChangeHandlerFactory = onChangeHandlerFactory;
            _attachHandler = attachHandler;
            _downloadHandler = downloadHandler;
            _reportHandler = reportHandler;
            _customHandler = customHandler;
        }


        private readonly IContainerResolver _containerResolver;
        private readonly ChangeHttpRequestHandlerFactory _onChangeHandlerFactory;
        private readonly AttachHttpRequestHandler _attachHandler;
        private readonly DownloadHttpRequestHandler _downloadHandler;
        private readonly ReportHttpRequestHandler _reportHandler;
        private readonly CustomHttpRequestHandler _customHandler;


        public void Load(IHttpServiceBuilder builder)
        {
            // TODO: Навести порядок в путях.

            // Сессия пользователя
            builder.Post
                   .Action("/SystemConfig/StandardApi/authorization/GetSessionData", CreateChangeHandler<ActionUnitGetSessionData>())
                   .Action("/SystemConfig/StandardApi/authorization/SetSessionData", CreateChangeHandler<ActionUnitSetSessionData>())
                   .Action("/SystemConfig/StandardApi/authorization/RemoveSessionData", CreateChangeHandler<ActionUnitRemoveSessionData>());

            // Работа с документами
            builder.Post
                   .Action("/SystemConfig/StandardApi/configuration/GetDocumentById", CreateChangeHandler<ActionUnitGetDocumentById>())
                   .Action("/SystemConfig/StandardApi/configuration/GetDocument", CreateChangeHandler<ActionUnitGetDocument>())
                   .Action("/SystemConfig/StandardApi/configuration/GetNumberOfDocuments", CreateChangeHandler<ActionUnitGetNumberOfDocuments>())
                   .Action("/SystemConfig/StandardApi/configuration/SetDocument", CreateChangeHandler<ActionUnitSetDocument>())
                   .Action("/SystemConfig/StandardApi/configuration/DeleteDocument", CreateChangeHandler<ActionUnitDeleteDocument>());

            // Работа с файлами

            builder.Get
                   .Action("/SystemConfig/UrlEncodedData/configuration/DownloadBinaryContent", _downloadHandler);

            builder.Post
                   .Action("/SystemConfig/Upload/configuration/UploadBinaryContent", _attachHandler);

            // Метаданные представлений
            builder.Post
                   .Action("/SystemConfig/StandardApi/metadata/GetManagedMetadata", CreateChangeHandler<ActionUnitGetManagedMetadata>());

            // Печатные представления
            builder.Post
                   .Action("/SystemConfig/UrlEncodedData/reporting/GetPrintView", _reportHandler);

            // Прикладные сервисы
            builder.Post
                   .Action("/{configuration}/StandardApi/{documentType}/{actionName}", _customHandler);
        }


        private IHttpRequestHandler CreateChangeHandler<TActionUnit>() where TActionUnit : class
        {
            // TODO: Сделать IHttpRequestHandler для каждого действия в отдельности.

            var action = CreateAction<TActionUnit>();
            var handler = _onChangeHandlerFactory(action);
            return handler;
        }

        private Action<IActionContext> CreateAction<TActionUnit>() where TActionUnit : class
        {
            var actionUnit = _containerResolver.Resolve<TActionUnit>();
            var action = (Action<IActionContext>)ReflectionExtensions.GetMemberValue(actionUnit, "Action");
            return action;
        }
    }
}