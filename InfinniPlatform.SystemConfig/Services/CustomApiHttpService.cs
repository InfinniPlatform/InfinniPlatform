using System.Threading.Tasks;

using InfinniPlatform.Core.Contracts;
using InfinniPlatform.Core.Runtime;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.SystemConfig.Services
{
    /// <summary>
    /// Реализует REST-сервис для CustomApi.
    /// </summary>
    internal sealed class CustomApiHttpService : IHttpService
    {
        public CustomApiHttpService(IScriptProcessor scriptProcessor)
        {
            _scriptProcessor = scriptProcessor;
        }

        private readonly IScriptProcessor _scriptProcessor;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.Post["/{configuration}/StandardApi/{documentType}/{actionName}"] = CustomAction;
        }

        private Task<object> CustomAction(IHttpRequest request)
        {
            // TODO: Это наивное предположение, нужно изменить после рефакторинга метаданных сервисов.
            string actionName = $"ActionUnit{request.Parameters.ActionName}";

            dynamic requestForm = request.Form;
            dynamic changesObject = requestForm.changesObject;
            string documentType = changesObject.Metadata;

            changesObject.Documents = changesObject.Documents ?? new object[] { changesObject.Document };

            var context = new ActionContext
            {
                DocumentType = documentType,
                Item = changesObject,
                Result = changesObject
            };

            _scriptProcessor.InvokeScriptByType(actionName, context);

            if (!context.IsValid)
            {
                var error = new DynamicWrapper
                {
                    ["IsValid"] = false,
                    ["ValidationMessage"] = context.ValidationMessage
                };

                return Task.FromResult<object>(new JsonHttpResponse(error) { StatusCode = 400 });
            }

            return Task.FromResult<object>(new JsonHttpResponse(context.Result));
        }
    }
}