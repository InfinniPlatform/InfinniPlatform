using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.RestApi.AuthApi;
using InfinniPlatform.Api.Transactions;
using InfinniPlatform.Metadata;

namespace InfinniPlatform.RestfulApi.Utils
{
	public static class TransactionUtils
	{
		public static void ApplyTransactionMarker(IApplyContext target)
		{
			if (!string.IsNullOrEmpty(target.TransactionMarker))
			{
				if (target.Item.Document != null || target.Item.Documents != null)
				{
					target.Context.GetComponent<ITransactionComponent>().GetTransactionManager().Attach(target.TransactionMarker, CreateAttachedInstance(target));
				}
			}

		}

		private static dynamic CreateAttachedInstance(IApplyContext target)
		{
			var attachedInstance = new AttachedInstance();
			attachedInstance.Version = target.Version;
			attachedInstance.Instance = target.Item;
			attachedInstance.TenantId = target.Context.GetComponent<ISecurityComponent>()
			                                 .GetClaim(AuthorizationStorageExtensions.OrganizationClaim, target.UserName);
			
			return attachedInstance;
		}
	}
}
