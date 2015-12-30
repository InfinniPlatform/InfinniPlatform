using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    public sealed class ActionUnitDeleteDocument
    {
        public ActionUnitDeleteDocument(ISetDocumentExecutor setDocumentExecutor)
        {
            _setDocumentExecutor = setDocumentExecutor;
        }

        private readonly ISetDocumentExecutor _setDocumentExecutor;

        public void Action(IApplyContext target)
        {
            string configuration = target.Item.Configuration;
            string documentType = target.Item.Metadata;
            object documentId = target.Item.Id;

            var result = _setDocumentExecutor.DeleteDocument(configuration, documentType, documentId);

            target.Result = result;
        }
    }
}