using System;
using System.Linq;

using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Transactions;
using InfinniPlatform.Sdk.Global;

namespace InfinniPlatform.RestfulApi.DefaultProcessUnits
{
    public sealed class ActionUnitSuccessSetDocument
    {
        public ActionUnitSuccessSetDocument(IMetadataComponent metadataComponent, ITransactionComponent transactionComponent, IScriptRunnerComponent scriptRunnerComponent, ICustomServiceGlobalContext customServiceGlobalContext)
        {
            _metadataComponent = metadataComponent;
            _transactionManager = transactionComponent.GetTransactionManager();
            _scriptRunnerComponent = scriptRunnerComponent;
            _customServiceGlobalContext = customServiceGlobalContext;
        }


        private readonly IMetadataComponent _metadataComponent;
        private readonly ITransactionManager _transactionManager;
        private readonly IScriptRunnerComponent _scriptRunnerComponent;
        private readonly ICustomServiceGlobalContext _customServiceGlobalContext;


        public void Action(IApplyContext target)
        {
            string configuration = target.Item.Configuration;
            string documentType = target.Item.Metadata;

            // TODO: Что это за проверка?
            if (!string.Equals(configuration, "restfulapi", StringComparison.OrdinalIgnoreCase) && !string.Equals(configuration, "systemconfig", StringComparison.OrdinalIgnoreCase))
            {
                var defaultBusinessProcess = _metadataComponent.GetMetadata(configuration, documentType, MetadataType.Process, "Default");

                // Скрипт, который выполняется после успешного сохранения документов
                string onSuccessAction = defaultBusinessProcess?.Transitions?[0]?.SuccessPoint?.ScenarioId;

                if (onSuccessAction != null)
                {
                    var documents = target.Item.Documents;

                    if (documents != null)
                    {
                        var transaction = _transactionManager.GetTransaction(target.TransactionMarker);

                        var transactionItems = transaction.GetTransactionItems();

                        foreach (var document in documents)
                        {
                            var documentId = document.Id;

                            var actionContext = new ApplyContext();
                            actionContext.CopyPropertiesFrom(target);
                            actionContext.Item = document;
                            actionContext.Item.Configuration = configuration;
                            actionContext.Item.Metadata = documentType;
                            actionContext.Context = _customServiceGlobalContext;

                            // Выполнение скрипта для очередного документа
                            _scriptRunnerComponent.InvokeScript(onSuccessAction, actionContext);

                            // Обновление ссылки на документ в транзакции
                            var attached = transactionItems.FirstOrDefault(i => i.ContainsInstance(documentId));
                            attached?.UpdateDocument(document.Id, actionContext.Item);
                        }
                    }
                }
            }
        }
    }
}