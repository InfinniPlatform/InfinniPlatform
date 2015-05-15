using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Registers;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.SearchOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.ContextComponents;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
	public sealed class ActionUnitDeleteDocument
	{
	    public void Action(IApplyContext target)
	    {
	        var indexName = string.Format("{0}_{1}", target.Item.Configuration, target.Item.Metadata);


	        //получаем провайдер версий документов
		    dynamic documentProvider =
			    target.Context.GetComponent<InprocessDocumentComponent>()
			          .GetDocumentProvider(target.Item.Configuration, target.Item.Metadata, target.UserName);
	        if (documentProvider == null)
	        {
	            throw new ArgumentException(string.Format("document provider for type \"{0}\" not found.",
	                target.Item.Metadata));
	        }


	        dynamic criteria = new DynamicWrapper();
	        criteria.Property = "Id";
	        criteria.Value = target.Item.Id;
	        criteria.CriteriaType = CriteriaType.IsContains;


	        IEnumerable<dynamic> doc = documentProvider.GetDocument(new[] {criteria}, 0, 1);

	        if (!doc.Any())
	        {
	            return;
	        }

	        documentProvider.DeleteDocument(target.Item.Id);

	        target.Result = doc.FirstOrDefault();
	        target.Context.GetComponent<ILogComponent>().GetLog().Info("document \"{0}\" deleted", indexName);
	    }
	}
}
