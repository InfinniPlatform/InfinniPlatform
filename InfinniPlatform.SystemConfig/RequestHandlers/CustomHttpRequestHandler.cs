using InfinniPlatform.Core.Contracts;
using InfinniPlatform.Core.Runtime;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.SystemConfig.RequestHandlers
{
    internal sealed class CustomHttpRequestHandler : IHttpRequestHandler
    {
        public CustomHttpRequestHandler(IScriptProcessor scriptProcessor)
        {
            _scriptProcessor = scriptProcessor;
        }

        private readonly IScriptProcessor _scriptProcessor;

        public object Action(IHttpRequest request)
        {
            // TODO: Это наивное предположение, нужно изменить после рефакторинга метаданных сервисов.
            string actionName = $"ActionUnit{request.Parameters.ActionName}";

            dynamic requestForm = request.Form;
            dynamic changesObject = requestForm.changesObject;
            string configuration = changesObject.Configuration;
            string documentType = changesObject.Metadata;

            changesObject.Documents = changesObject.Documents ?? new object[] { changesObject.Document };

            var context = new ActionContext
            {
                Configuration = configuration,
                Metadata = documentType,
                Item = changesObject,
                Result = changesObject
            };

            _scriptProcessor.InvokeScriptByType(actionName, context);

            if (!context.IsValid)
            {
                return RequestHandlerHelper.BadRequest((string)context.ValidationMessage);
            }

            return new JsonHttpResponse(context.Result);
        }
    }
}