using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.ActionUnits.Documents
{
    public sealed class ActionUnitDeleteDocument
    {
        public ActionUnitDeleteDocument(ISetDocumentExecutor setDocumentExecutor)
        {
            _setDocumentExecutor = setDocumentExecutor;
        }

        private readonly ISetDocumentExecutor _setDocumentExecutor;

        public void Action(IActionContext target)
        {
            string configuration = target.Item.Configuration;
            string documentType = target.Item.Metadata;
            object documentId = target.Item.Id;

            var result = _setDocumentExecutor.DeleteDocument(configuration, documentType, documentId);

            target.IsValid = result.IsValid;
            target.ValidationMessage = result.ValidationMessage;
            target.Result = result;
        }
    }
}