using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Documents;

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
            var criteriaFilters = target.Item.Filter as IEnumerable<dynamic>;
            var criteriaSortings = target.Item.Sorting as IEnumerable<dynamic>;

            var enumerableFilters = criteriaFilters?.Select(o => new FilterCriteria(o.Property, o.Value, (CriteriaType)o.CriteriaType));
            var enumerableSortings = criteriaSortings?.Select(o => new SortingCriteria(o.PropertyName, o.SortingOrder));

            target.Result = _getDocumentExecutor.GetDocument(target.Item.Configuration,
                                                             target.Item.Metadata,
                                                             enumerableFilters,
                                                             Convert.ToInt32(target.Item.PageNumber),
                                                             Convert.ToInt32(target.Item.PageSize),
                                                             enumerableSortings,
                                                             target.Item.IgnoreResolve);
        }
    }
}