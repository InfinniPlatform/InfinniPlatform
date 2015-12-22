using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Transactions;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.RestfulApi.Executors
{
    internal class SetDocumentExecutor : ISetDocumentExecutor
    {
        public SetDocumentExecutor(IGlobalContext globalContext, Func<IContainerResolver> containerResolver, IMetadataComponent metadataComponent, IScriptRunnerComponent scriptRunnerComponent)
        {
            _globalContext = globalContext;
            _metadataComponent = metadataComponent;
            _scriptRunnerComponent = scriptRunnerComponent;

            // TODO: Сделано именно так, потому что есть циклическая ссылка "SetDocumentExecutor - TransactionManager - BinaryManager - SetDocumentExecutor"
            _transactionManager = new Lazy<ITransactionManager>(() => containerResolver().Resolve<ITransactionComponent>().GetTransactionManager());
        }


        private readonly IGlobalContext _globalContext;
        private readonly IMetadataComponent _metadataComponent;
        private readonly IScriptRunnerComponent _scriptRunnerComponent;
        private readonly Lazy<ITransactionManager> _transactionManager;


        public dynamic SetDocument(string configuration, string documentType, object documentInstance)
        {
            return ExecuteSetDocument(configuration, documentType, new[] { documentInstance });
        }

        public dynamic SetDocuments(string configuration, string documentType, IEnumerable<object> documentInstances)
        {
            var documentBatches = GetBatches(documentInstances, 100);

            foreach (var documents in documentBatches)
            {
                var batchResult = ExecuteSetDocument(configuration, documentType, documents);

                if (batchResult.IsValid == false)
                {
                    throw new InvalidOperationException(batchResult.ValidationMessage?.ToString());
                }
            }

            var result = new SetDocumentResult { IsValid = true };

            return result;
        }

        private SetDocumentResult ExecuteSetDocument(string configuration, string documentType, IEnumerable<object> documentInstances)
        {
            var result = new SetDocumentResult { IsValid = true, ValidationMessage = null };

            // Бизнес-процесс сохранения документа
            var businessProcess = _metadataComponent.GetMetadata(configuration, documentType, MetadataType.Process, "Default");

            // Скрипт, который выполняется для проверки документов на корректность
            string onValidateAction = businessProcess?.Transitions?[0]?.ValidationPointError?.ScenarioId;

            // Скрипт, который выполняется после успешного сохранения документов
            string onSuccessAction = businessProcess?.Transitions?[0]?.SuccessPoint?.ScenarioId;

            // TODO: Следует заменить на UnitOfWork со стратегией PerRequest
            var transactionId = Guid.NewGuid().ToString();
            var transaction = _transactionManager.Value.GetTransaction(transactionId);

            foreach (dynamic documentInstance in documentInstances)
            {
                if (documentInstance.Id == null)
                {
                    documentInstance.Id = Guid.NewGuid();
                }

                if (!string.IsNullOrEmpty(onValidateAction))
                {

                    // Вызов прикладного скрипта для проверки документа
                    ApplyContext actionContext = CreateActionContext(configuration, documentType, documentInstance);
                    _scriptRunnerComponent.InvokeScript(onValidateAction, actionContext);

                    if (!CheckActionResult(actionContext, result))
                    {
                        break;
                    }
                }

                // Добавление документа в транзакцию
                var transactionEntry = transaction.Attach(configuration, documentType, new[] { documentInstance });

                if (!string.IsNullOrEmpty(onSuccessAction))
                {
                    // Вызов скрипта для пост-обработки документа перед его сохранением
                    ApplyContext actionContext = CreateActionContext(configuration, documentType, documentInstance);
                    _scriptRunnerComponent.InvokeScript(onSuccessAction, actionContext);

                    // Предполагается, что скрипт может заменить оригинальный документ
                    transactionEntry.UpdateDocument(documentInstance.Id, actionContext.Item);

                    if (!CheckActionResult(actionContext, result))
                    {
                        break;
                    }
                }
            }

            if (result.IsValid)
            {
                // Физическое сохранение
                transaction.CommitTransaction();
            }

            return result;
        }

        private ApplyContext CreateActionContext(string configuration, string documentType, object documentInstance)
        {
            var actionContext = new ApplyContext { Item = documentInstance, IsValid = true };

            // TODO: Скорей всего, эта информация не нужна
            actionContext.Configuration = configuration;
            actionContext.Metadata = documentType;

            // TODO: Эти действия вообще необъяснимы
            actionContext.Item.Configuration = configuration;
            actionContext.Item.Metadata = documentType;

            // TODO: Заменить на IoC
            actionContext.Context = _globalContext;

            return actionContext;
        }

        private static bool CheckActionResult(ApplyContext actionContext, SetDocumentResult setDocumentResult)
        {
            setDocumentResult.IsValid = actionContext.IsValid;
            setDocumentResult.ValidationMessage = actionContext.ValidationMessage;
            setDocumentResult.InternalServerError = actionContext.IsValid ? null : "true";

            return actionContext.IsValid;
        }

        private static IEnumerable<object>[] GetBatches(IEnumerable<object> items, int batchSize)
        {
            return items.Select((item, index) => new { item, index })
                        .GroupBy(x => x.index / batchSize)
                        .Select(g => g.Select(x => x.item))
                        .ToArray();
        }


        private class SetDocumentResult
        {
            public bool IsValid { get; set; }

            public object ValidationMessage { get; set; }

            public object InternalServerError { get; set; }
        }
    }
}