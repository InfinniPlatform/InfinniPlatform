using System;
using System.Collections.Generic;

using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.ActionUnits.Documents
{
    public sealed class ActionUnitGetDocumentCrossConfig
    {
        public ActionUnitGetDocumentCrossConfig(IGetDocumentExecutor getDocumentExecutor)
        {
            _getDocumentExecutor = getDocumentExecutor;
        }

        private readonly IGetDocumentExecutor _getDocumentExecutor;

        public void Action(IApplyContext target)
        {
            IEnumerable<object> configs = DynamicWrapperExtensions.ToEnumerable(target.Item.Configurations);
            IEnumerable<object> documents = DynamicWrapperExtensions.ToEnumerable(target.Item.Documents);

            var resultDocuments = new List<dynamic>();

            foreach (string config in configs)
            {
                foreach (string document in documents)
                {
                    IEnumerable<dynamic> completeDocuments = _getDocumentExecutor.GetDocument(config,
                                                                                              document,
                                                                                              Convert.ToInt32(target.Item.PageNumber),
                                                                                              Convert.ToInt32(target.Item.PageSize),
                                                                                              target.Item.Filter,
                                                                                              target.Item.Sorting,
                                                                                              target.Item.IgnoreResolve);

                    resultDocuments.AddRange(completeDocuments);
                }
            }

            target.Result = resultDocuments;
        }
    }
}