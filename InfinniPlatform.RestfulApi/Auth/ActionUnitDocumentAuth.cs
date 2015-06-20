using System;
using System.Collections.Generic;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.RestfulApi.Utils;
using InfinniPlatform.Sdk.Application.Contracts;
using InfinniPlatform.Sdk.Application.Dynamic;

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
                    new AuthUtils(target.Context.GetComponent<ISecurityComponent>(target.Version), target.UserName,
                                  FilterRoles(target)).CheckDocumentAccess(target.Item.Configuration,
                                                                           target.Item.Metadata, target.Action,
                                                                           target.Item.Document != null
                                                                               ? target.Item.Document.Id
                                                                               : null);
                target.IsValid = result.IsValid;
                target.ValidationMessage = string.Join(",", result.Items);
            }
            else
            {
                target.IsValid = true;
            }
        }

        private IEnumerable<dynamic> FilterRoles(IApplyContext target)
        {
            var roleCheckProcess =
                target.Context.GetComponent<IMetadataComponent>(target.Version)
                      .GetMetadata(target.Version, AuthorizationStorageExtensions.AuthorizationConfigId, "Common",
                                   MetadataType.Process, "FilterRoles");

            if (roleCheckProcess != null && roleCheckProcess.Transitions[0].ActionPoint != null)
            {
                var scriptArguments = new ApplyContext();
                scriptArguments.CopyPropertiesFrom(target);
                scriptArguments.Item.Configuration = target.Item.Configuration;
                scriptArguments.Item.Metadata = target.Item.Metadata;

                try
                {
                    target.Context.GetComponent<IScriptRunnerComponent>(target.Version)
                          .GetScriptRunner(target.Version, AuthorizationStorageExtensions.AuthorizationConfigId)
                          .InvokeScript(roleCheckProcess.Transitions[0].ActionPoint.ScenarioId, scriptArguments);
                }
                catch (ArgumentException e)
                {
                    target.Context.GetComponent<ILogComponent>(target.Version)
                          .GetLog()
                          .Error("Fail to get filtered roles: " + e.Message);
                    return new List<dynamic>();
                }

                //устанавливаем в качестве результата роли, которые вернула точка расширения
                return DynamicWrapperExtensions.ToEnumerable(scriptArguments.Result);
            }

            return null;
        }
    }
}