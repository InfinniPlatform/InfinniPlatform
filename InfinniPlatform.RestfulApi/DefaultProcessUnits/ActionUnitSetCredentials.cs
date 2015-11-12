﻿using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Global;

namespace InfinniPlatform.RestfulApi.DefaultProcessUnits
{
    public sealed class ActionUnitSetCredentials
    {
        public void Action(IApplyContext target)
        {
            dynamic defaultBusinessProcess = null;

            if (target.Item.Configuration == null)
            {
                return;
            }

            //TODO Игнорировать системные конфигурации при валидации. Пока непонятно, как переделать
            if (target.Item.Configuration.ToLowerInvariant() != "systemconfig" &&
                target.Item.Configuration.ToLowerInvariant() != "update" &&
                target.Item.Configuration.ToLowerInvariant() != "restfulapi")
            {
                //ищем метаданные бизнес-процесса по умолчанию документа 
                defaultBusinessProcess =
                    target.Context.GetComponent<IMetadataComponent>()
                          .GetMetadata(null, target.Item.Configuration, target.Item.Metadata,
                                       MetadataType.Process, "Default");
            }
            else
            {
                return;
            }

            if (defaultBusinessProcess != null && defaultBusinessProcess.Transitions[0].CredentialsType != null)
            {
                dynamic credentialsType = defaultBusinessProcess.Transitions[0].CredentialsType;
                if (credentialsType != null &&
                    credentialsType == AuthorizationStorageExtensions.AnonimousUserCredentials)
                {
                    target.UserName = AuthorizationStorageExtensions.AnonimousUser;
                }
                else if (credentialsType != null && credentialsType == AuthorizationStorageExtensions.CustomCredentials)
                {
                    var scriptArguments = new ApplyContext();
                    scriptArguments.CopyPropertiesFrom(target);
                    scriptArguments.Item = target.Item.Document;
                    scriptArguments.Item.Configuration = target.Item.Configuration;
                    scriptArguments.Item.Metadata = target.Item.Metadata;
                    scriptArguments.Context = target.Context.GetComponent<ICustomServiceGlobalContext>();

                    target.Context.GetComponent<IScriptRunnerComponent>()
                          .GetScriptRunner(null, target.Item.Configuration)
                          .InvokeScript(defaultBusinessProcess.Transitions[0].CredentialsPoint.ScenarioId,
                                        scriptArguments);
                }
            }
        }
    }
}