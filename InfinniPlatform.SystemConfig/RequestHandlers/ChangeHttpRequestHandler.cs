using System;

using InfinniPlatform.Core.Contracts;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.SystemConfig.RequestHandlers
{
    internal sealed class ChangeHttpRequestHandler : IHttpRequestHandler
    {
        public ChangeHttpRequestHandler(Action<IActionContext> action)
        {
            _action = action;
        }

        private readonly Action<IActionContext> _action;

        public object Action(IHttpRequest request)
        {
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

            _action(context);

            if (!context.IsValid)
            {
                return RequestHandlerHelper.BadRequest((string)context.ValidationMessage);
            }

            return new JsonHttpResponse(context.Result);
        }
    }
}