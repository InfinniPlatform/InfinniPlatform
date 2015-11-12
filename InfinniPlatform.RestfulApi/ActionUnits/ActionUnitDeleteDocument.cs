using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    public sealed class ActionUnitDeleteDocument
    {
        public void Action(IApplyContext target)
        {
            string indexName = string.Format("{0}_{1}", target.Item.Configuration, target.Item.Metadata);

            //получаем провайдер версий документов
            dynamic documentProvider =
                target.Context.GetComponent<InprocessDocumentComponent>()
                      .GetDocumentProvider(null, target.Item.Configuration, target.Item.Metadata,
                                           target.UserName);
            if (documentProvider == null)
            {
                throw new ArgumentException(string.Format(Resources.DocumentProviderTypeNotFound, target.Item.Metadata));
            }


            dynamic criteria = new DynamicWrapper();
            criteria.Property = "Id";
            criteria.Value = target.Item.Id;
            criteria.CriteriaType = CriteriaType.IsEquals;


            IEnumerable<dynamic> doc = documentProvider.GetDocument(new[] {criteria}, 0, 1);

            if (!doc.Any())
            {
                target.Result = new DynamicWrapper();
                target.Result.IsValid = false;
                target.Result.ValidationMessage = string.Format(Resources.DocumentNotFound, target.Item.Id);
                return;
            }

            documentProvider.DeleteDocument(target.Item.Id);

            target.Context.GetComponent<ILogComponent>()
                  .GetLog()
                  .Info(Resources.LogDocumentDeleted, new Dictionary<string, object>
                                                      {
                                                          { "documentId", target.Item.Id },
                                                          { "indexName", indexName },
                                                      });

            target.Result = new DynamicWrapper();
            target.Result.IsValid = true;
            target.Result.ValidationMessage = string.Format(Resources.DocumentDeletedSuccessfully, target.Item.Id);
        }
    }
}