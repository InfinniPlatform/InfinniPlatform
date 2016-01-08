using System;

using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.SystemConfig.Utils;

namespace InfinniPlatform.SystemConfig.ActionUnits.Documents
{
    public sealed class ActionUnitGetDocument
    {
        public ActionUnitGetDocument(DocumentExecutor documentExecutor)
        {
            _documentExecutor = documentExecutor;
        }

        private readonly DocumentExecutor _documentExecutor;

        public void Action(IApplyContext target)
        {
            target.Result = _documentExecutor.GetCompleteDocuments(
                target.Item.Configuration,
                target.Item.Metadata,
                Convert.ToInt32(target.Item.PageNumber),
                Convert.ToInt32(target.Item.PageSize),
                target.Item.Filter,
                target.Item.Sorting,
                target.Item.IgnoreResolve);
        }
    }
}