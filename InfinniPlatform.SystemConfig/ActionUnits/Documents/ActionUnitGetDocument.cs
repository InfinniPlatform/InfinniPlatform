using System;

using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.ActionUnits.Documents
{
    public sealed class ActionUnitGetDocument
    {
        public ActionUnitGetDocument(IGetDocumentExecutor getDocumentExecutor)
        {
            _getDocumentExecutor = getDocumentExecutor;
        }

        private readonly IGetDocumentExecutor _getDocumentExecutor;

        public void Action(IApplyContext target)
        {
            target.Result = _getDocumentExecutor.GetDocument(target.Item.Configuration,
                                                             target.Item.Metadata,
                                                             Convert.ToInt32(target.Item.PageNumber),
                                                             Convert.ToInt32(target.Item.PageSize),
                                                             target.Item.Filter,
                                                             target.Item.Sorting,
                                                             target.Item.IgnoreResolve);
        }
    }
}