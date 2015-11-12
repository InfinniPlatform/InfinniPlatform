using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.Api.Validation.ValidationBuilders;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Validations;
using InfinniPlatform.SystemConfig.Properties;

namespace InfinniPlatform.SystemConfig.Configurator
{
    /// <summary>
    ///     Получение метаданных на высоком уровне абстракции (без указания запроса IQL)
    /// </summary>
    public class ActionUnitGetManagedMetadata
    {
        public void Action(IApplyContext target)
        {
            IValidationOperator validator = ValidationBuilder.ForObject(builder => builder.And(rules => rules
                                                                                                            .IsNotNullOrEmpty
                                                                                                            ("Configuration",
                                                                                                             Resources
                                                                                                                 .ConfigurationIdNotFound)
                                                                                                            .IsNotNullOrEmpty
                                                                                                            ("MetadataObject",
                                                                                                             Resources
                                                                                                                 .ErrorMetadataNotSpecified)));

            Func<dynamic, bool> predicate = null;
            //если не указано конкретное наименование метаданных (наименование конкретной формы, например "SomeForm1", то ищем первую попавшуюся форму указанного типа MetadataType)
            if (string.IsNullOrEmpty(target.Item.MetadataName))
            {
                //если не указан тип и наименование, берем первое попавшееся view
                if (string.IsNullOrEmpty(target.Item.MetadataType))
                {
                    predicate = view => true;
                }
                else
                {
                    predicate = view => view.MetadataType == target.Item.MetadataType;
                }
            }
                //иначе ищем метаданные с указанным наименованием MetadataName и указанным типом MetadataType
            else
            {
                predicate = view => view.Name == target.Item.MetadataName;
            }

            if (validator.Validate(target.Item))
            {
                dynamic itemMetadata = null;

                itemMetadata =
                    target.Context.GetComponent<IMetadataComponent>()
                          .GetMetadataItem(target.Item.Configuration, target.Item.MetadataObject,
                                           MetadataType.View, predicate);

                //если нашли существующие метаданные - возвращаем их
                if (itemMetadata != null)
                {
                    target.Result = itemMetadata;
                    target.Result.RequestParameters = CreateParametersArray(target.Item.Parameters);
                }
                    //иначе пытаемся найти одноименный генератор метаданных
                else
                {
                    var generatorDocument = target.Item.MetadataObject;

                    dynamic generatorMetadataItem =
                        target.Context.GetComponent<IMetadataComponent>()
                              .GetMetadataItem(target.Item.Configuration, target.Item.MetadataObject,
                                               MetadataType.Generator, predicate);
                    if (generatorMetadataItem == null)
                    {
                        generatorDocument = "Common";
                        generatorMetadataItem =
                            target.Context.GetComponent<IMetadataComponent>()
                                  .GetMetadataItem(target.Item.Configuration, generatorDocument,
                                                   MetadataType.Generator, predicate);
                    }

                    if (generatorMetadataItem == null)
                    {
                        target.Result = new DynamicWrapper();

                        target.Result.Parameters = CreateParametersArray(target.Item.Parameters);
                        target.ValidationMessage = string.Format(Resources.GeneratorMetadataNotRegistered,
                                                                 target.Item.MetadataType);
                        return;
                    }

                    if (generatorMetadataItem.MetadataType != null &&
                        generatorMetadataItem.MetadataType.ToLowerInvariant() ==
                        target.Item.MetadataType.ToLowerInvariant())
                    {
                        var body = new
                            {
                                Metadata = generatorDocument,
                                GeneratorName = generatorMetadataItem.Name,
                                target.Item.Configuration,
                                target.Item.Parameters
                            };

                        target.Result =
                            RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "generatemetadata", null, body).ToDynamic();
                        target.Result.Parameters = CreateParametersArray(target.Item.Parameters);
                        target.ValidationMessage = string.Format("Metadata generated for \"{0}\" successfully",
                                                                 generatorMetadataItem.Name);
                    }
                }
            }
        }

        private dynamic CreateParametersArray(dynamic parameters)
        {
            if (parameters == null)
            {
                return new List<dynamic>();
            }
            var result = new List<dynamic>();
            foreach (dynamic parameter in parameters)
            {
                dynamic item = new DynamicWrapper();
                item.Name = parameter.Key;
                item.Value = parameter.Value;
                result.Add(item);
            }
            return result;
        }
    }
}