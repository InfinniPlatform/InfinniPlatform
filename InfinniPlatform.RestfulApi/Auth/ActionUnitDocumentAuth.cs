﻿using System;
using System.Collections.Generic;
using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.RestfulApi.Utils;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment;
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
                target.Context.GetComponent<IMetadataComponent>()
                      .GetMetadata(target.Context.GetVersion(AuthorizationStorageExtensions.AuthorizationConfigId, target.UserName), AuthorizationStorageExtensions.AuthorizationConfigId, "Common",
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
                          .GetScriptRunner(target.Context.GetVersion(AuthorizationStorageExtensions.AuthorizationConfigId,target.UserName), AuthorizationStorageExtensions.AuthorizationConfigId)
                          .InvokeScript(roleCheckProcess.Transitions[0].ActionPoint.ScenarioId, scriptArguments);
                }
                catch (ArgumentException e)
                {
                    target.Context.GetComponent<ILogComponent>()
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