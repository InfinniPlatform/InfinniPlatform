using System;

using InfinniPlatform.Core.ContextTypes;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.SystemConfig.RequestHandlers
{
    internal sealed class ChangeHttpRequestHandler : IHttpRequestHandler
    {
        public ChangeHttpRequestHandler(Action<IApplyContext> action)
        {
            _action = action;
        }

        private readonly Action<IApplyContext> _action;

        public object Action(IHttpRequest request)
        {
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

            _action(context);

            if (!context.IsValid)
            {
                return RequestHandlerHelper.BadRequest((string)context.ValidationMessage);
            }

            return new JsonHttpResponse(context.Result);
        }
    }
}