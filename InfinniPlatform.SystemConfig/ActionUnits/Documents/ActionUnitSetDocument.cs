using System.Collections.Generic;

using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.ActionUnits.Documents
{
    public sealed class ActionUnitSetDocument
    {
        public ActionUnitSetDocument(ISetDocumentExecutor setDocumentExecutor)
        {
            _setDocumentExecutor = setDocumentExecutor;
        }

        private readonly ISetDocumentExecutor _setDocumentExecutor;

        public void Action(IApplyContext target)
        {
            string configuration = target.Item.Configuration;
            string documentType = target.Item.Metadata;
            IEnumerable<dynamic> documents = target.Item.Documents;

            var result = _setDocumentExecutor.SaveDocuments(configuration, documentType, documents);

            target.Result = result;
        }
    }
}