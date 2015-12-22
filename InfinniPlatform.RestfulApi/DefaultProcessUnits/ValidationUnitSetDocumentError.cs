using System;
using System.Collections;

using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Validations;
using InfinniPlatform.Sdk.Global;

namespace InfinniPlatform.RestfulApi.DefaultProcessUnits
{
    public sealed class ValidationUnitSetDocumentError
    {
        public ValidationUnitSetDocumentError(IMetadataComponent metadataComponent, IScriptRunnerComponent scriptRunnerComponent, ICustomServiceGlobalContext customServiceGlobalContext)
        {
            _metadataComponent = metadataComponent;
            _scriptRunnerComponent = scriptRunnerComponent;
            _customServiceGlobalContext = customServiceGlobalContext;
        }


        private readonly ICustomServiceGlobalContext _customServiceGlobalContext;
        private readonly IMetadataComponent _metadataComponent;
        private readonly IScriptRunnerComponent _scriptRunnerComponent;


        public void Action(IApplyContext target)
        {
            string configuration = target.Item.Configuration;
            string documentType = target.Item.Metadata;

            var validationResult = new ValidationResult();

            // TODO: Что это за проверка?
            if (!string.Equals(configuration, "restfulapi", StringComparison.OrdinalIgnoreCase) && !string.Equals(configuration, "systemconfig", StringComparison.OrdinalIgnoreCase))
            {
                var defaultBusinessProcess = _metadataComponent.GetMetadata(configuration, documentType, MetadataType.Process, "Default");

                // Скрипт, который выполняется для проверки документов на корректность
                string onValidateAction = defaultBusinessProcess?.Transitions?[0]?.ValidationPointError?.ScenarioId;

                if (onValidateAction != null)
                {
                    var documents = target.Item.Documents;

                    if (documents != null)
                    {
                        foreach (var document in documents)
                        {
                            var actionContext = new ApplyContext();
                            actionContext.CopyPropertiesFrom(target);
                            actionContext.Item = document;
                            actionContext.Item.Configuration = configuration;
                            actionContext.Item.Metadata = documentType;
                            actionContext.Context = _customServiceGlobalContext;

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
            }

            target.Item.IgnoreWarnings = null;
            target.Item.ValidationWarnings = null;
            target.Item.ValidationErrors = null;

            target.ValidationMessage = validationResult.Items;
            target.IsValid = validationResult.IsValid;
        }
    }
}