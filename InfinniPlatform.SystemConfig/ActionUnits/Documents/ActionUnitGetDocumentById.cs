using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.SystemConfig.ActionUnits.Documents
{
    /// <summary>
    /// Модуль для получения документа по указанному идентификатору
    /// </summary>
    public sealed class ActionUnitGetDocumentById
    {
        public ActionUnitGetDocumentById(IDocumentApi documentApi)
        {
            _documentApi = documentApi;
        }

        private readonly IDocumentApi _documentApi;

        public void Action(IActionContext target)
        {
            string configuration = target.Item.ConfigId;
            string documentType = target.Item.DocumentId;
            string documentId = target.Item.Id;

            target.Result = _documentApi.GetDocumentById(configuration, documentType, documentId);
        }
    }
}