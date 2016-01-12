using System;
using System.Collections.Generic;

using InfinniPlatform.Core.ContextTypes;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Core.Runtime;
using InfinniPlatform.Core.Transactions;

namespace InfinniPlatform.SystemConfig.Executors
{
    internal class SetDocumentExecutor : ISetDocumentExecutor
    {
        public SetDocumentExecutor(IDocumentTransactionScopeProvider transactionScopeProvider, IMetadataComponent metadataComponent, IScriptProcessor scriptProcessor)
        {
            _transactionScopeProvider = transactionScopeProvider;
            _metadataComponent = metadataComponent;
            _scriptProcessor = scriptProcessor;
        }


        private readonly IDocumentTransactionScopeProvider _transactionScopeProvider;
        private readonly IMetadataComponent _metadataComponent;
        private readonly IScriptProcessor _scriptProcessor;


        public DocumentExecutorResult SaveDocument(string configuration, string documentType, object documentInstance)
        {
            return ExecuteSaveDocuments(configuration, documentType, new[] { documentInstance });
        }

        public DocumentExecutorResult SaveDocuments(string configuration, string documentType, IEnumerable<object> documentInstances)
        {
            return ExecuteSaveDocuments(configuration, documentType, documentInstances);
        }


        public DocumentExecutorResult DeleteDocument(string configuration, string documentType, object documentId)
        {
            return ExecuteDeleteDocuments(configuration, documentType, new[] { documentId });
        }

        public DocumentExecutorResult DeleteDocuments(string configuration, string documentType, IEnumerable<object> documentIds)
        {
            return ExecuteDeleteDocuments(configuration, documentType, documentIds);
        }


        private DocumentExecutorResult ExecuteSaveDocuments(string configuration, string documentType, IEnumerable<object> documentInstances)
        {
            var result = new DocumentExecutorResult { IsValid = true };

            var transactionScope = _transactionScopeProvider.GetTransactionScope();

            // TODO: Разрешить вызов данной операции только для REST-запросов с соответствующим параметром
            transactionScope.Synchronous();

            // Бизнес-процесс сохранения документа
            var businessProcess = _metadataComponent.GetMetadata(configuration, documentType, MetadataType.Process, "Default");

            // Скрипт, который выполняется для проверки возможности сохранения документа
            string onValidateAction = businessProcess?.Transitions?[0]?.ValidationPointError?.ScenarioId;

            // Скрипт, который выполняется после успешного сохранения документа
            string onSuccessAction = businessProcess?.Transitions?[0]?.SuccessPoint?.ScenarioId;

            foreach (dynamic documentInstance in documentInstances)
            {
                if (documentInstance.Id == null)
                {
                    documentInstance.Id = Guid.NewGuid().ToString("D");
                }

                result.Id = documentInstance.Id;

                if (!string.IsNullOrEmpty(onValidateAction))
                {
                    // Вызов прикладного скрипта для проверки документа
                    ApplyContext actionContext = CreateActionContext(configuration, documentType, documentInstance);
                    _scriptProcessor.InvokeScript(onValidateAction, actionContext);

                    if (!CheckActionResult(actionContext, result))
                    {
                        break;
                    }
                }

                var documentToSave = documentInstance;

                if (!string.IsNullOrEmpty(onSuccessAction))
                {
                    // Вызов скрипта для пост-обработки документа перед его сохранением
                    ApplyContext actionContext = CreateActionContext(configuration, documentType, documentInstance);
                    _scriptProcessor.InvokeScript(onSuccessAction, actionContext);

                    // Предполагается, что скрипт может заменить оригинальный документ
                    documentToSave = actionContext.Item;

                    if (!CheckActionResult(actionContext, result))
                    {
                        break;
                    }
                }

                // Регистрация сохранения в транзакции
                transactionScope.SaveDocument(configuration, documentType, documentInstance.Id, documentToSave);
            }

            return result;
        }

        private DocumentExecutorResult ExecuteDeleteDocuments(string configuration, string documentType, IEnumerable<object> documentIds)
        {
            var result = new DocumentExecutorResult { IsValid = true };

            var transactionScope = _transactionScopeProvider.GetTransactionScope();

            // TODO: Разрешить вызов данной операции только для REST-запросов с соответствующим параметром
            transactionScope.Synchronous();

            // Бизнес-процесс сохранения документа
            var businessProcess = _metadataComponent.GetMetadata(configuration, documentType, MetadataType.Process, "Default");

            // Скрипт, который выполняется для проверки возможности удаления документа
            string onValidateAction = businessProcess?.Transitions?[0]?.DeletingDocumentValidationPoint?.ScenarioId;

            // Скрипт, который выполняется после успешного удаления документа
            string onSuccessAction = businessProcess?.Transitions?[0]?.DeletePoint?.ScenarioId;

            foreach (var documentId in documentIds)
            {
                if (!string.IsNullOrEmpty(onValidateAction))
                {
                    // Вызов прикладного скрипта для проверки документа
                    ApplyContext actionContext = CreateActionContext(configuration, documentType, documentId);
                    _scriptProcessor.InvokeScript(onValidateAction, actionContext);

                    if (!CheckActionResult(actionContext, result))
                    {
                        break;
                    }
                }

                // Регистрация удаления в транзакции
                transactionScope.DeleteDocument(configuration, documentType, documentId);

                if (!string.IsNullOrEmpty(onSuccessAction))
                {
                    // Вызов скрипта для пост-обработки документа перед его сохранением
                    ApplyContext actionContext = CreateActionContext(configuration, documentType, documentId);
                    _scriptProcessor.InvokeScript(onSuccessAction, actionContext);

                    if (!CheckActionResult(actionContext, result))
                    {
                        break;
                    }
                }
            }

            return result;
        }


        private ApplyContext CreateActionContext(string configuration, string documentType, object actionItem)
        {
            var actionContext = new ApplyContext { Item = actionItem, IsValid = true };

            // TODO: Скорей всего, эта информация не нужна
            actionContext.Configuration = configuration;
            actionContext.Metadata = documentType;

            return actionContext;
        }

        private static bool CheckActionResult(ApplyContext actionContext, DocumentExecutorResult setDocumentResult)
        {
            setDocumentResult.IsValid = actionContext.IsValid;
            setDocumentResult.ValidationMessage = actionContext.ValidationMessage;
            setDocumentResult.IsInternalServerError = actionContext.IsValid ? (bool?)null : true;

            return actionContext.IsValid;
        }
    }
}