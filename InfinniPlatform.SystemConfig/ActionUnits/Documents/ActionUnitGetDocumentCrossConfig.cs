using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.SystemConfig.Utils;

namespace InfinniPlatform.SystemConfig.ActionUnits.Documents
{
    public sealed class ActionUnitGetDocumentCrossConfig
    {
        public ActionUnitGetDocumentCrossConfig(DocumentExecutor documentExecutor)
        {
            _documentExecutor = documentExecutor;
        }

        private readonly DocumentExecutor _documentExecutor;

        public void Action(IApplyContext target)
        {
            IEnumerable<object> filter = DynamicWrapperExtensions.ToEnumerable(target.Item.Filter);
            IEnumerable<object> sorting = DynamicWrapperExtensions.ToEnumerable(target.Item.Sorting);
            IEnumerable<object> configs = DynamicWrapperExtensions.ToEnumerable(target.Item.Configurations);
            IEnumerable<object> documents = DynamicWrapperExtensions.ToEnumerable(target.Item.Documents);

            var resultDocuments = new List<dynamic>();

            foreach (string config in configs)
            {
                foreach (string document in documents)
                {
                    IEnumerable<dynamic> completeDocuments = _documentExecutor.GetCompleteDocuments(
                        config,
                        document,
                        Convert.ToInt32(target.Item.PageNumber),
                        Convert.ToInt32(target.Item.PageSize),
                        filter,
                        sorting,
                        target.Item.IgnoreResolve);

                    resultDocuments.AddRange(completeDocuments);
                }
            }

            target.Result = resultDocuments;
        }
    }
}