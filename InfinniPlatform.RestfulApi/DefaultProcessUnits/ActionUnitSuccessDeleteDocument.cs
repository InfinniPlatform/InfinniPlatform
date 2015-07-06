using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.RestfulApi.DefaultProcessUnits
{
    /// <summary>
    ///     Обработчик успешного удалении документа (при стандартном сохранении документа)
    /// </summary>
    public sealed class ActionUnitSuccessDeleteDocument
    {
        public void Action(IApplyContext target)
        {
            dynamic defaultBusinessProcess = null;

            //TODO Игнорировать системные конфигурации при валидации. Пока непонятно, как переделать
            if (target.Item.Configuration.ToLowerInvariant() != "systemconfig" &&
                target.Item.Configuration.ToLowerInvariant() != "update" &&
                target.Item.Configuration.ToLowerInvariant() != "restfulapi")
            {
                //ищем метаданные бизнес-процесса по умолчанию документа 
                defaultBusinessProcess =
                    target.Context.GetComponent<IMetadataComponent>()
                          .GetMetadata(target.Context.GetVersion(target.Item.Configuration, target.UserName), target.Item.Configuration, target.Item.Metadata,
                                       MetadataType.Process, "Default");
            }
            else
            {
                return;
            }


            if (defaultBusinessProcess != null && defaultBusinessProcess.Transitions[0].DeletePoint != null)
            {
                var scriptArguments = new ApplyContext();
                scriptArguments.CopyPropertiesFrom(target);
                scriptArguments.Item = new DynamicWrapper();
                scriptArguments.Item.Id = target.Item.Id;
                scriptArguments.Item.Configuration = target.Item.Configuration;
                scriptArguments.Item.Metadata = target.Item.Metadata;
                scriptArguments.Item.Document = target.Result;

                target.Context.GetComponent<IScriptRunnerComponent>()
                      .GetScriptRunner(target.Context.GetVersion(target.Item.Configuration, target.UserName), target.Item.Configuration)
                      .InvokeScript(defaultBusinessProcess.Transitions[0].DeletePoint.ScenarioId, scriptArguments);
            }
        }
    }
}