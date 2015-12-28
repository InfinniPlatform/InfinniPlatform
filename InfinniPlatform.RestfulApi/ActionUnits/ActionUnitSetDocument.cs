using System;
using System.Collections;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    [Obsolete]
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
            IEnumerable documents = target.Item.Documents;

            var result = _setDocumentExecutor.SaveDocument(configuration, documentType, documents);

            target.Result = result;
        }
    }
}