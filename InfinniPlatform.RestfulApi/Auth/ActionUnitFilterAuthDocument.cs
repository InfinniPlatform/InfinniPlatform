using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.RestfulApi.Utils;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///   Модуль фильтрации документов, на которые у пользователя есть доступ 
    /// </summary>
    public sealed class ActionUnitFilterAuthDocument
    {
        public void Action(IApplyContext target)
        {
			if (target.Item.Secured == null || target.Item.Secured == true)
			{
				var result = DynamicWrapperExtensions.ToEnumerable(target.Result);

				var resultChecked = new List<dynamic>();

				foreach (dynamic document in result)
				{
					var validationResult = new AuthUtils(target.Context.GetComponent<ISecurityComponent>(), target.UserName, null).CheckDocumentAccess(target.Item.Configuration, target.Item.Metadata, "getdocument", document.Id);

					if (validationResult.IsValid)
					{
						resultChecked.Add(document);
					}
				}
				target.Result = resultChecked;
				
			}

        }
    }
}
