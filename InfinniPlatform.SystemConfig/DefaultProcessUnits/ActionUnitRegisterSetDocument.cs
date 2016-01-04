using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Transactions;

namespace InfinniPlatform.SystemConfig.DefaultProcessUnits
{
    /// <summary>
    /// Обработчик занесения данных документа в регистр после успешного сохранения документа (при стандартном сохранении
    /// документа)
    /// </summary>
    public sealed class ActionUnitRegisterSetDocument
    {
        public ActionUnitRegisterSetDocument(IMetadataComponent metadataComponent, IDocumentTransactionScopeProvider transactionScopeProvider, IScriptRunnerComponent scriptRunnerComponent, IGlobalContext globalContext)
        {
            _metadataComponent = metadataComponent;
            _transactionScopeProvider = transactionScopeProvider;
            _scriptRunnerComponent = scriptRunnerComponent;
            _globalContext = globalContext;
        }

        private readonly IGlobalContext _globalContext;
        private readonly IMetadataComponent _metadataComponent;
        private readonly IScriptRunnerComponent _scriptRunnerComponent;
        private readonly IDocumentTransactionScopeProvider _transactionScopeProvider;

        public void Action(IApplyContext target)
        {
            string configuration = target.Item.Configuration;
            string documentType = target.Item.Metadata;

            var defaultBusinessProcess = _metadataComponent.GetMetadata(configuration, documentType, MetadataType.Process, "Default");

            var registerPoint = defaultBusinessProcess?.Transitions?[0]?.RegisterPoint;

            if (registerPoint != null)
            {
                var scriptArguments = new ApplyContext();
                scriptArguments.CopyPropertiesFrom(target);
                scriptArguments.Item = target.Item.Document;
                scriptArguments.Item.Configuration = configuration;
                scriptArguments.Item.Metadata = documentType;
                scriptArguments.Context = _globalContext;

                _scriptRunnerComponent.InvokeScript(registerPoint.ScenarioId, scriptArguments);

                if (target.Item.Document != null && target.Item.Document.Id != null)
                {
                    var transactionScope = _transactionScopeProvider.GetTransactionScope();

                    transactionScope.SaveDocument(configuration, documentType, target.Item.Document.Id, scriptArguments.Item);
                }
            }
        }
    }
}