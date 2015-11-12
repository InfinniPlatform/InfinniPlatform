using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.Logging;
using InfinniPlatform.RestfulApi.Utils;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Log;
using InfinniPlatform.Sdk.Environment.Validations;
using InfinniPlatform.Sdk.Global;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///     Модуль авторизации стандартного процесса обработки документов
    /// </summary>
    public sealed class ActionUnitDocumentAuth
    {
        public void Action(IApplyContext target)
        {
            if (target.Item.Secured == null || target.Item.Secured == true)
            {
                ValidationResult result =
                    new AuthUtils(target.Context.GetComponent<ISecurityComponent>(), target.UserName,
                        FilterRoles(target)).CheckDocumentAccess(target.Item.Configuration,
                            target.Item.Metadata, target.Action,
                            target.Item.Document != null
                                ? target.Item.Document.Id
                                : null);
                target.IsValid = result.IsValid;
                target.ValidationMessage = string.Join(",", (result.Items != null) ? result.Items.Select(i => (string)i.ToString()) : Enumerable.Empty<string>());
            }
            else
            {
                target.IsValid = true;
            }
        }

        private IEnumerable<dynamic> FilterRoles(IApplyContext target)
        {
            var roleCheckProcess =
                target.Context.GetComponent<IMetadataComponent>()
                      .GetMetadata(null, AuthorizationStorageExtensions.AuthorizationConfigId, "Common",
                          MetadataType.Process, "FilterRoles");

            if (roleCheckProcess != null && roleCheckProcess.Transitions[0].ActionPoint != null)
            {
                var scriptArguments = new ApplyContext();
                scriptArguments.CopyPropertiesFrom(target);
                scriptArguments.Item.Configuration = target.Item.Configuration;
                scriptArguments.Item.Metadata = target.Item.Metadata;
                scriptArguments.Context = target.Context.GetComponent<ICustomServiceGlobalContext>();
                try
                {
                    target.Context.GetComponent<IScriptRunnerComponent>()
                          .GetScriptRunner(null, AuthorizationStorageExtensions.AuthorizationConfigId)
                          .InvokeScript(roleCheckProcess.Transitions[0].ActionPoint.ScenarioId, scriptArguments);
                }
                catch (ArgumentException e)
                {
                    var log = target.Context.GetComponent<ILogComponent>().GetLog();
                    log.Warn("Failed to get filtered roles.", null, e);

                    return new List<dynamic>();
                }

                //устанавливаем в качестве результата роли, которые вернула точка расширения
                return DynamicWrapperExtensions.ToEnumerable(scriptArguments.Result);
            }

            return null;
        }
    }
}