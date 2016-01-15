﻿using InfinniPlatform.Core.ContextTypes;
using InfinniPlatform.Core.Runtime;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.SystemConfig.RequestHandlers
{
    internal sealed class CustomHttpRequestHandler : SimpleHttpRequestHandler
    {
        public CustomHttpRequestHandler(IScriptProcessor scriptProcessor)
        {
            _scriptProcessor = scriptProcessor;
        }

        private readonly IScriptProcessor _scriptProcessor;

        protected override object ActionResult(IHttpRequest request)
        {
            // TODO: Это наивное предположение, нужно изменить после рефакторинга метаданных сервисов.
            string actionName = $"ActionUnit{request.Parameters.ActionName}";

            dynamic requestForm = request.Form;
            dynamic changesObject = requestForm.changesObject;
            string configuration = changesObject.Configuration;
            string documentType = changesObject.Metadata;

            changesObject.Documents = changesObject.Documents ?? new object[] { changesObject.Document };

            var context = new ApplyContext
                          {
                              Item = changesObject,
                              Result = changesObject,
                              Configuration = configuration,
                              Metadata = documentType,
                              Type = documentType
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