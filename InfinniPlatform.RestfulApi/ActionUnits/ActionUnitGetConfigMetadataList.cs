using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.RestfulApi.Utils;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
	/// <summary>
	///  Получить метаданные загруженной конфигурации сервера
	/// </summary>
	public sealed class ActionUnitGetConfigMetadataList
	{
		public void Action(IApplyContext target)
		{
			var paramsDoc = target.Item;
			if (target.Item.Document != null)
			{
				paramsDoc = target.Item.Document;
			}


			var authUtils = new AuthUtils(target.Context.GetComponent<ISecurityComponent>(), target.UserName, null);

			
			target.Result = target.Context.GetComponent<IMetadataComponent>()
								  .GetMetadataList(paramsDoc.Configuration, paramsDoc.Metadata, paramsDoc.MetadataType);

			var result = new List<dynamic>();
			foreach (dynamic o in target.Result)
			{
				ValidationResult validationResult = authUtils.CheckDocumentAccess("SystemConfig",
				                                                                  paramsDoc.MetadataType + "metadata", "getdocument",
				                                                                  o.Id);
				if (validationResult.IsValid)
				{
					result.Add(o);
				}
			}

			target.Result = result;
		}
	}
}
