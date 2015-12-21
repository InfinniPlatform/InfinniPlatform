using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Global;

namespace InfinniPlatform.RestfulApi.DefaultProcessUnits
{
    /// <summary>
    /// Сложная авторизация при обработке бизнес-процесса
    /// </summary>
    public sealed class ActionUnitComplexAuth
    {
        public ActionUnitComplexAuth(IMetadataComponent metadataComponent,
                                     ICustomServiceGlobalContext customServiceGlobalContext,
                                     IScriptRunnerComponent scriptRunnerComponent)
        {
            _metadataComponent = metadataComponent;
            _customServiceGlobalContext = customServiceGlobalContext;
            _scriptRunnerComponent = scriptRunnerComponent;
        }

        private readonly ICustomServiceGlobalContext _customServiceGlobalContext;
        private readonly IMetadataComponent _metadataComponent;
        private readonly IScriptRunnerComponent _scriptRunnerComponent;

        public void Action(IApplyContext target)
        {
            //TODO Игнорировать системные конфигурации при валидации. Пока непонятно, как переделать
            if (target.Item.Configuration.ToLowerInvariant() != "systemconfig" &&
                target.Item.Configuration.ToLowerInvariant() != "update" &&
                target.Item.Configuration.ToLowerInvariant() != "restfulapi")
            {
                //ищем метаданные бизнес-процесса по умолчанию документа 
                dynamic defaultBusinessProcess = _metadataComponent.GetMetadata(target.Item.Configuration, target.Item.Metadata, MetadataType.Process, "Default");

                if (defaultBusinessProcess != null && defaultBusinessProcess.Transitions[0].ComplexAuthorizationPoint != null)
                {
                    var scriptArguments = new ApplyContext();
                    scriptArguments.CopyPropertiesFrom(target);
                    scriptArguments.Item = target.Item.Document;
                    scriptArguments.Item.Configuration = target.Item.Configuration;
                    scriptArguments.Item.Metadata = target.Item.Metadata;
                    scriptArguments.Context = _customServiceGlobalContext;

                    _scriptRunnerComponent.InvokeScript(defaultBusinessProcess.Transitions[0].ComplexAuthorizationPoint.ScenarioId, scriptArguments);
                }
            }
        }
    }
}