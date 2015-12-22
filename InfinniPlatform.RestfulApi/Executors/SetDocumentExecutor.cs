using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Transactions;
using InfinniPlatform.Sdk.Environment.Validations;
using InfinniPlatform.Sdk.Global;
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
                dynamic batchResult = ExecuteSetDocument(configuration, documentType, documents);

                if (batchResult?.IsValid == false)
                {
                    throw new ArgumentException(batchResult.ToString());
                }
            }

            dynamic result = new DynamicWrapper();
            result.IsValid = true;
            result.ValidationMessage = Resources.BatchCompletedSuccessfully;

            return result;
        }

        private object ExecuteSetDocument(string configuration, string documentType, IEnumerable<object> documentInstances)
        {

            return ApplyJsonObject(configuration, documentType, documentInstances);
        }

        private static IEnumerable<object>[] GetBatches(IEnumerable<object> items, int batchSize)
        {
            return items.Select((item, index) => new { item, index })
                        .GroupBy(x => x.index / batchSize)
                        .Select(g => g.Select(x => x.item))
                        .ToArray();
        }

        private dynamic ApplyJsonObject(string configuration, string documentType, IEnumerable<object> documentInstances)
        {
            // Идентификатор транзакции
            var transactionId = Guid.NewGuid().ToString();

            // Начало транзакции
            var transaction = _transactionManager.Value.GetTransaction(transactionId);

            dynamic result = new DynamicWrapper();

            if (ValidationUnitSetDocumentError(transactionId, configuration, documentType, documentInstances, result)
                && ActionUnitSetDocument(transactionId, configuration, documentType, documentInstances, result)
                && ActionUnitSuccessSetDocument(transactionId, configuration, documentType, documentInstances, result))
            {
                transaction.CommitTransaction();
            }
            else
            {
                result.InternalServerError = true;
            }

            return result;
        }

        private bool ValidationUnitSetDocumentError(string transactionId, string configuration, string documentType, IEnumerable<object> documentInstances, dynamic result)
        {
            var validationResult = new ValidationResult();

            var defaultBusinessProcess = _metadataComponent.GetMetadata(configuration, documentType, MetadataType.Process, "Default");

            // Скрипт, который выполняется для проверки документов на корректность
            string onValidateAction = defaultBusinessProcess?.Transitions?[0]?.ValidationPointError?.ScenarioId;

            if (onValidateAction != null)
            {
                if (documentInstances != null)
                {
                    foreach (var document in documentInstances)
                    {
                        var actionContext = new ApplyContext();
                        actionContext.Configuration = configuration;
                        actionContext.Metadata = documentType;
                        actionContext.Action = "setdocument";
                        actionContext.Item = document;
                        actionContext.Item.Configuration = configuration;
                        actionContext.Item.Metadata = documentType;
                        actionContext.Context = _globalContext;

                        _scriptRunnerComponent.InvokeScript(onValidateAction, actionContext);

                        if (actionContext.ValidationMessage != null)
                        {
                            if (actionContext.ValidationMessage is IEnumerable && actionContext.ValidationMessage.GetType() != typeof(string))
                            {
                                validationResult.Items.AddRange(actionContext.ValidationMessage);
                            }
                            else
                            {
                                validationResult.Items.Add(actionContext.ValidationMessage);
                            }
                        }

                        validationResult.IsValid &= actionContext.IsValid;
                    }
                }
            }

            result.IsValid = validationResult.IsValid;
            result.ValidationMessage = validationResult.Items;

            return (result.IsValid == true);
        }

        private bool ActionUnitSetDocument(string transactionId, string configuration, string documentType, IEnumerable<object> documentInstances, dynamic result)
        {
            if (documentInstances != null)
            {
                foreach (dynamic document in documentInstances)
                {
                    if (document.Id == null)
                    {
                        document.Id = Guid.NewGuid();
                    }

                    result.Id = document.Id;
                }

                if (!string.IsNullOrEmpty(transactionId))
                {
                    var transaction = _transactionManager.Value.GetTransaction(transactionId);

                    transaction.Attach(configuration, documentType, documentInstances);
                }
            }

            result.IsValid = true;
            result.ValidationMessage = string.Empty;

            return (result.IsValid == true);
        }

        private bool ActionUnitSuccessSetDocument(string transactionId, string configuration, string documentType, IEnumerable<object> documentInstances, dynamic result)
        {
            var defaultBusinessProcess = _metadataComponent.GetMetadata(configuration, documentType, MetadataType.Process, "Default");

            // Скрипт, который выполняется после успешного сохранения документов
            string onSuccessAction = defaultBusinessProcess?.Transitions?[0]?.SuccessPoint?.ScenarioId;

            if (onSuccessAction != null)
            {
                var documents = documentInstances;

                if (documents != null)
                {
                    var transaction = _transactionManager.Value.GetTransaction(transactionId);

                    var transactionItems = transaction.GetTransactionItems();

                    foreach (dynamic document in documents)
                    {
                        var documentId = document.Id;

                        var actionContext = new ApplyContext();
                        actionContext.Configuration = configuration;
                        actionContext.Metadata = documentType;
                        actionContext.Action = "setdocument";
                        actionContext.Item = document;
                        actionContext.Item.Configuration = configuration;
                        actionContext.Item.Metadata = documentType;
                        actionContext.Context = _globalContext;

                        // Выполнение скрипта для очередного документа
                        _scriptRunnerComponent.InvokeScript(onSuccessAction, actionContext);

                        // Обновление ссылки на документ в транзакции
                        var attached = transactionItems.FirstOrDefault(i => i.ContainsInstance(documentId));
                        attached?.UpdateDocument(document.Id, actionContext.Item);
                    }
                }
            }

            result.IsValid = true;
            result.ValidationMessage = string.Empty;

            return (result.IsValid == true);
        }
    }
}