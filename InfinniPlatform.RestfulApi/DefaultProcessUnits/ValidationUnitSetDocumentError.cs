using System.Collections;
using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.Logging;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.Sdk.Environment.Validations;
using InfinniPlatform.Sdk.Global;

namespace InfinniPlatform.RestfulApi.DefaultProcessUnits
{
    /// <summary>
    ///     Валидатор для применения правил проверки документа, описанных в его метаданных
    /// </summary>
    public sealed class ValidationUnitSetDocumentError
    {
        public void Validate(IApplyContext target)
        {
            dynamic validationMessage = target.ValidationMessage;

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

            if (defaultBusinessProcess == null || defaultBusinessProcess.Transitions.Count == 0)
            {
                target.ValidationMessage = validationMessage;
                return;
            }

            var validationResult = new ValidationResult();
            //выполняем валидацию на error
            if (defaultBusinessProcess.Transitions[0].ValidationRuleError != null)
            {
                var validationOperator =
                    target.Context.GetComponent<IMetadataComponent>()
                          .GetMetadata(target.Context.GetVersion(target.Item.Configuration, target.UserName), target.Item.Configuration, target.Item.Metadata,
                                       MetadataType.ValidationError,
                                       (string) defaultBusinessProcess.Transitions[0].ValidationRuleError)
                          .ValidationOperator;
                IValidationOperator op = ValidationExtensions.CreateValidatorFromConfigValidator(validationOperator);
                op.Validate(target.Item.Document, validationResult);
            }


            if (defaultBusinessProcess.Transitions[0].ValidationPointError != null)
            {
                //получаем конструктор метаданных конфигураций
                var configBuilder =
                    target.Context.GetComponent<IConfigurationMediatorComponent>().ConfigurationBuilder;

                IConfigurationObject configurationObject = configBuilder.GetConfigurationObject(target.Context.GetVersion(target.Item.Configuration, target.UserName),
                                                                                                target.Item
                                                                                                      .Configuration);

                if (configurationObject == null)
                {
                    Logger.Log.Error(string.Format(Resources.ConfigurationReferencedInObjectNotFound,
                                                   target.Item.Configuration,
                                                   "DefaultBusinessProcess"));
                }
                else
                {
                    if (defaultBusinessProcess.Transitions[0].ValidationPointError.ScenarioId != null)
                    {
                        IValidationOperator validator = configurationObject.MetadataConfiguration.ScriptConfiguration
                                                                           .GetValidator(
                                                                               defaultBusinessProcess.Transitions[0]
                                                                                   .ValidationPointError.ScenarioId);


                        var context = new ApplyContext();

                        context.CopyPropertiesFrom(target);
                        context.Item = target.Item.Document ?? target.Item;
                        context.Context = context.Context.GetComponent<ICustomServiceGlobalContext>();

                        validator.Validate(context, new ValidationResult());

                        if (context.ValidationMessage != null)
                        {
                            if (context.ValidationMessage is IEnumerable &&
                                context.ValidationMessage.GetType() != typeof (string))
                            {
                                validationResult.Items.AddRange(context.ValidationMessage);
                            }
                            else
                            {
                                validationResult.Items.Add(context.ValidationMessage);
                            }
                        }
                        validationResult.IsValid &= context.IsValid;
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