using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.ContextComponents;

namespace InfinniPlatform.RestfulApi.Auth
{
	/// <summary>
	///   Предоставление доступа пользователям
	/// </summary>
	public sealed class ActionUnitApplyAccess
	{
		public void Action(IApplyContext target)
		{
			dynamic instance = new DynamicWrapper();
			instance.Configuration = target.Item.Configuration;
			instance.Metadata = target.Item.Metadata;
			instance.RecordId = target.Item.RecordId;
			instance.Action = target.Item.Action;
			instance.UserName = target.Item.UserName;
			instance.Result = target.Item.Result;
			

			if (string.IsNullOrEmpty(target.Item.Configuration) || string.IsNullOrEmpty(target.Item.UserName))
			{
				throw new ArgumentException(Resources.ConfigurationAndUserNameShouldNotBeEmptyForGrantAccess);
			}

			dynamic existingAcl =
				new DocumentApi().GetDocument(AuthorizationStorageExtensions.AuthorizationConfigId,
				                              AuthorizationStorageExtensions.AclStore,
				                              f => f.AddCriteria(cr => cr.Property("UserName").IsEquals(instance.UserName))
				                                    .AddCriteria(cr => cr.Property("Configuration").IsEquals(instance.Configuration))
				                                    .AddCriteria(cr => cr.Property("Metadata").IsEquals(instance.Metadata))
				                                    .AddCriteria(cr => cr.Property("RecordId").IsEquals(instance.RecordId))
				                                    .AddCriteria(cr => cr.Property("Action").IsEquals(instance.Action)),0,1).FirstOrDefault();

			if (existingAcl != null)
			{
				instance.Id = existingAcl.Id;
			}

			new DocumentApi().SetDocument(AuthorizationStorageExtensions.AuthorizationConfigId,
			                              AuthorizationStorageExtensions.AclStore, instance);

			target.Context.GetComponent<CachedSecurityComponent>().UpdateAcl();

		    target.Result = new DynamicWrapper();
		    target.Result.IsValid = true;
		}
	}
}
