using System.Linq;
using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Global;

namespace InfinniPlatform.RestfulApi.DefaultProcessUnits
{
    /// <summary>
    ///     Обработчик успешного сохранения документа (при стандартном сохранении документа)
    /// </summary>
    public sealed class ActionUnitSuccessSetDocument
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
                          .GetMetadata(target.Item.Configuration, target.Item.Metadata,
                                       MetadataType.Process, "Default");
            }
            else
            {
                return;
            }


            if (defaultBusinessProcess != null && defaultBusinessProcess.Transitions[0].SuccessPoint != null)
            {
                var scriptArguments = new ApplyContext();
                scriptArguments.CopyPropertiesFrom(target);
                scriptArguments.Item = target.Item.Document;
                scriptArguments.Item.Configuration = target.Item.Configuration;
                scriptArguments.Item.Metadata = target.Item.Metadata;
                scriptArguments.Context = target.Context.GetComponent<ICustomServiceGlobalContext>();

                target.Context.GetComponent<IScriptRunnerComponent>()
                      .GetScriptRunner(null, target.Item.Configuration)
                      .InvokeScript(defaultBusinessProcess.Transitions[0].SuccessPoint.ScenarioId, scriptArguments);

                if (target.Item.Document != null && target.Item.Document.Id != null)
                {
                    var attached = target.Context.GetComponent<ITransactionComponent>()
                        .GetTransactionManager()
                        .GetTransaction(target.TransactionMarker)
                        .GetTransactionItems()
                        .FirstOrDefault(d => d.ContainsInstance(target.Item.Document.Id));

                    if (attached != null)
                    {
                        attached.UpdateDocument(target.Item.Document.Id, scriptArguments.Item);
                    }
                }
            }
        }
    }
}