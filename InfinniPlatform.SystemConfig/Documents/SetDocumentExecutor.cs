using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Contracts;
using InfinniPlatform.Core.Documents;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Runtime;
using InfinniPlatform.Core.Transactions;

namespace InfinniPlatform.SystemConfig.Documents
{
    internal class SetDocumentExecutor : ISetDocumentExecutor
    {
        public SetDocumentExecutor(IDocumentTransactionScopeProvider transactionScopeProvider, IMetadataApi metadataComponent, IScriptProcessor scriptProcessor)
        {
            _transactionScopeProvider = transactionScopeProvider;
            _metadataComponent = metadataComponent;
            _scriptProcessor = scriptProcessor;
        }


        private readonly IDocumentTransactionScopeProvider _transactionScopeProvider;
        private readonly IMetadataApi _metadataComponent;
        private readonly IScriptProcessor _scriptProcessor;


        public DocumentExecutorResult SaveDocument(string documentType, object documentInstance)
        {
            return ExecuteSaveDocuments(documentType, new[] { documentInstance });
        }

        public DocumentExecutorResult SaveDocuments(string documentType, IEnumerable<object> documentInstances)
        {
            return ExecuteSaveDocuments(documentType, documentInstances);
        }


        public DocumentExecutorResult DeleteDocument(string documentType, object documentId)
        {
            return ExecuteDeleteDocuments(documentType, new[] { documentId });
        }

        public DocumentExecutorResult DeleteDocuments(string documentType, IEnumerable<object> documentIds)
        {
            return ExecuteDeleteDocuments(documentType, documentIds);
        }


        private DocumentExecutorResult ExecuteSaveDocuments(string documentType, IEnumerable<object> documentInstances)
        {
            var result = new DocumentExecutorResult { IsValid = true };

            var transactionScope = _transactionScopeProvider.GetTransactionScope();

            // События документа
            var documentEvents = _metadataComponent.GetDocumentEvents(documentType);

            // Скрипт, который выполняется для проверки возможности сохранения документа
            string onValidateAction = documentEvents != null && documentEvents.ValidationPointError != null ? documentEvents.ValidationPointError.ScenarioId : null;

            // Скрипт, который выполняется после успешного сохранения документа
            string onSuccessAction = documentEvents != null && documentEvents.SuccessPoint != null ? documentEvents.SuccessPoint.ScenarioId : null;

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
                    ActionContext actionContext = CreateActionContext(documentType, documentInstance);
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
                    ActionContext actionContext = CreateActionContext(documentType, documentInstance);
                    _scriptProcessor.InvokeScript(onSuccessAction, actionContext);

                    // Предполагается, что скрипт может заменить оригинальный документ
                    documentToSave = actionContext.Item;

                    if (!CheckActionResult(actionContext, result))
                    {
                        break;
                    }
                }

                // Регистрация сохранения в транзакции
                transactionScope.SaveDocument(documentType, documentInstance.Id, documentToSave);
            }

            return result;
        }

        private DocumentExecutorResult ExecuteDeleteDocuments(string documentType, IEnumerable<object> documentIds)
        {
            var result = new DocumentExecutorResult { IsValid = true };

            var transactionScope = _transactionScopeProvider.GetTransactionScope();

            // События документа
            var documentEvents = _metadataComponent.GetDocumentEvents(documentType);

            // Скрипт, который выполняется для проверки возможности удаления документа
            string onValidateAction = documentEvents != null && (documentEvents.DeletingDocumentValidationPoint != null) ? documentEvents.DeletingDocumentValidationPoint.ScenarioId : null;

            // Скрипт, который выполняется после успешного удаления документа
            string onSuccessAction = documentEvents != null && (documentEvents.DeletePoint != null) ? documentEvents.DeletePoint.ScenarioId : null;

            foreach (var documentId in documentIds)
            {
                if (!string.IsNullOrEmpty(onValidateAction))
                {
                    // Вызов прикладного скрипта для проверки документа
                    ActionContext actionContext = CreateActionContext(documentType, documentId);
                    _scriptProcessor.InvokeScript(onValidateAction, actionContext);

                    if (!CheckActionResult(actionContext, result))
                    {
                        break;
                    }
                }

                // Регистрация удаления в транзакции
                transactionScope.DeleteDocument(documentType, documentId);

                if (!string.IsNullOrEmpty(onSuccessAction))
                {
                    // Вызов скрипта для пост-обработки документа перед его сохранением
                    ActionContext actionContext = CreateActionContext(documentType, documentId);
                    _scriptProcessor.InvokeScript(onSuccessAction, actionContext);

                    if (!CheckActionResult(actionContext, result))
                    {
                        break;
                    }
                }
            }

            return result;
        }


        private static ActionContext CreateActionContext(string documentType, object actionItem)
        {
            var actionContext = new ActionContext
                                {
                                    DocumentType = documentType,
                                    Item = actionItem
                                };

            return actionContext;
        }

        private static bool CheckActionResult(ActionContext actionContext, DocumentExecutorResult setDocumentResult)
        {
            setDocumentResult.IsValid = actionContext.IsValid;
            setDocumentResult.ValidationMessage = actionContext.ValidationMessage;
            setDocumentResult.Result = actionContext.Result;

            return actionContext.IsValid;
        }
    }
}