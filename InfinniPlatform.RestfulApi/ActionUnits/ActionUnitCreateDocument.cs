using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.Schema;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    internal sealed class ActionUnitCreateDocument
    {
        public void Action(IApplyContext target)
        {
            string configId = target.Item.Configuration;

            string documentId = target.Item.Metadata;

            dynamic schema = target.Context.GetComponent<IMetadataComponent>(target.Version)
                                   .GetMetadataList(target.Version, configId, documentId, MetadataType.Schema)
                                   .FirstOrDefault();

            //если для документа указана схема, то предзаполняем документ
            if (schema != null)
            {
                dynamic prefiledInstance = CreatePrefiledInstance(target.Version,
                                                                  new SchemaProvider(
                                                                      target.Context.GetComponent<IMetadataComponent>(
                                                                          target.Version)), schema);

                dynamic metadataProcess = target.Context.GetComponent<IMetadataComponent>(target.Version)
                                                .GetMetadata(target.Version, configId, documentId, MetadataType.Process,
                                                             "Default");

                if (metadataProcess != null && metadataProcess.Transitions != null &&
                    metadataProcess.Transitions.Count > 0)
                {
                    string schemaPrefillString = metadataProcess.Transitions[0].SchemaPrefill;
                    //выполняем простое предзаполнение
                    if (schemaPrefillString != null)
                    {
                        dynamic schemaPrefill = schemaPrefillString.ToDynamic();

                        var schemaIterator =
                            new SchemaIterator(
                                new SchemaProvider(target.Context.GetComponent<IMetadataComponent>(target.Version)));

                        IEnumerable<string> prefillItems =
                            RestQueryApi.QueryPostJsonRaw("systemconfig", "prefill", "getfillitems", null, null,
                                                          target.Version)
                                        .ToDynamicList()
                                        .Cast<string>();

                        schemaIterator.OnPrimitiveProperty = prefiledField =>
                            {
                                prefiledInstance = CheckPrefillForField(target, prefiledInstance, prefiledField,
                                                                        prefillItems);
                            };
                        schemaIterator.OnObjectProperty = prefiledField =>
                            {
                                prefiledInstance = CheckPrefillForField(target, prefiledInstance, prefiledField,
                                                                        prefillItems);
                            };

                        schemaIterator.ProcessSchema(target.Version, schemaPrefill);
                    }


                    //делаем комплексное предзаполнение
                    if (metadataProcess.Transitions[0].ActionPoint != null)
                    {
                        var scriptArguments = target;
                        scriptArguments.Item = prefiledInstance;
                        target.Context.GetComponent<IScriptRunnerComponent>(target.Version)
                              .GetScriptRunner(target.Version, configId)
                              .InvokeScript(metadataProcess.Transitions[0].ActionPoint.ScenarioId, scriptArguments);
                    }
                }


                target.Result = prefiledInstance;
            }
            else
            {
                target.Result = new DynamicWrapper();
            }
        }

        private dynamic CreatePrefiledInstance(string version, ISchemaProvider schemaProvider, dynamic schema)
        {
            dynamic instance = new DynamicWrapper();

            var schemaIterator = new SchemaIterator(schemaProvider);
            Action<SchemaObject> processAction = schemaObject =>
                {
                    if (!string.IsNullOrEmpty(schemaObject.ParentPath))
                    {
                        dynamic instanceParent = instance[schemaObject.ParentPath];

                        if (instanceParent != null)
                        {
                            instanceParent[schemaObject.Name] = null;
                        }
                    }
                };
            schemaIterator.OnPrimitiveProperty = processAction;
            schemaIterator.OnArrayProperty = processAction;

            schemaIterator.OnObjectProperty = FillObjectProperty(version, schemaProvider, instance);


            schemaIterator.ProcessSchema(version, schema);
            return instance;
        }

        private Action<SchemaObject> FillObjectProperty(string version, ISchemaProvider schemaProvider, dynamic instance)
        {
            return schemaObject =>
                {
                    //если обходим метаданные встроенного объекта,
                    //то создаем умолчательный экземпляр этого объекта и помещаем его в свойство заполняемого экземпляра
                    dynamic prefiledInnerObjectInstance = null;
                    if (schemaObject.ObjectSchema != null && schemaObject.Inline)
                    {
                        prefiledInnerObjectInstance = CreatePrefiledInstance(version, schemaProvider,
                                                                             schemaObject.ObjectSchema);
                    }

                    if (string.IsNullOrEmpty(schemaObject.ParentPath))
                    {
                        instance[schemaObject.Name] = prefiledInnerObjectInstance;
                    }
                    else
                    {
                        dynamic instanceParent = instance[schemaObject.ParentPath];

                        if (instanceParent != null)
                        {
                            instanceParent[schemaObject.Name] = prefiledInnerObjectInstance;
                        }
                    }
                };
        }

        /// <summary>
        ///     Заполнить поля документа указанными значениями
        /// </summary>
        /// <param name="target">Контекст выполнения скрипта</param>
        /// <param name="prefiledInstance">Заполняемая сущность</param>
        /// <param name="prefiledField">Заполняемое поле сущности</param>
        /// <param name="prefillItems">Список существующих модулей предзаполнения</param>
        private dynamic CheckPrefillForField(IApplyContext target, dynamic prefiledInstance, SchemaObject prefiledField,
                                             IEnumerable<string> prefillItems)
        {
            if (prefiledField.Value != null && !string.IsNullOrEmpty(prefiledField.Value.ToString()) &&
                prefiledField.Value.DefaultValue != null &&
                !string.IsNullOrEmpty(prefiledField.Value.DefaultValue.ToString()))
            {
                var scriptArguments = target;
                scriptArguments.Item.FieldName = prefiledField.Name;
                scriptArguments.Item.Document = prefiledInstance;


                if (prefillItems.Any(f => f.Contains(prefiledField.Value.DefaultValue.ToString())))
                {
                    target.Context.GetComponent<IScriptRunnerComponent>(target.Version)
                          .GetScriptRunner(target.Version, "systemconfig")
                          .InvokeScript(
                              prefiledField.Value.DefaultValue.ToString(),
                              scriptArguments);

                    prefiledInstance = scriptArguments.Result ?? scriptArguments.Item.Document;
                }
                else
                {
                    prefiledInstance[prefiledField.Name] = prefiledField.Value.DefaultValue;
                }
            }
            return prefiledInstance;
        }
    }
}