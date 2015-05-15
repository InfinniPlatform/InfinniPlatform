using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.AuthApi;

namespace InfinniPlatform.Api.Security
{
	public sealed class PasswordVerifierComponent : IPasswordVerifierComponent
	{
		private readonly IGlobalContext _globalContext;


		public PasswordVerifierComponent(IGlobalContext globalContext)
		{
			_globalContext = globalContext;
		}

		public bool VerifyPassword(string hashedPassword, string providedPassword)
		{
			var processMetadata =  _globalContext.GetComponent<IMetadataComponent>().GetMetadata(AuthorizationStorageExtensions.AuthorizationConfigId, "Common", MetadataType.Process,
			                               "VerifyPassword");

			
			if (processMetadata != null && processMetadata.Transitions[0].ActionPoint != null)
			{
				var scriptArguments = new ApplyContext();
				scriptArguments.Item = new DynamicWrapper();
				scriptArguments.Item.HashedPassword = hashedPassword;
				scriptArguments.Item.ProvidedPassword = providedPassword;
				scriptArguments.Context = _globalContext;
				_globalContext.GetComponent<IScriptRunnerComponent>().GetScriptRunner(AuthorizationStorageExtensions.AuthorizationConfigId)
					.InvokeScript(processMetadata.Transitions[0].ActionPoint.ScenarioId, scriptArguments);

				//устанавливаем в качестве результата роли, которые вернула точка расширения
				return scriptArguments.IsValid;
			}
			return false;
		}
	}
}
