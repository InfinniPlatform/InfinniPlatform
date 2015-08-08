using System.Collections.Generic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.ModelRepository;
using InfinniPlatform.Api.ModelRepository.MetadataObjectModel;

using OpenEhr.V1.Its.Xml.AM;

using System;
using System.Linq;

namespace InfinniPlatform.OceanInformatics.DataModelLoader
{
    public sealed class ArchetypeExtractor : IArchetypeExtractor
    {
        private readonly ObjectModelExtractor _modelObjectExtractor;

        public ArchetypeExtractor(string configId)
        {
            _modelObjectExtractor = new ObjectModelExtractor(true, configId);
        }

        /// <summary>
        /// Для извлечения метаданных архитипа происходит загрузка архетипа из файла средствами
        /// библиотеки от Ocean, после чего модель архетипа Ocean конвертируется в модель данных InfinniPlatform
        /// </summary>        
        public DataSchema Extract(string architypePath)
        {
            var archetype = new DataSchema();

            ARCHETYPE oceanArchetype;
            string errorMessage;

            if (!OceanDocumentsLoader.BuildFromAdlFile(architypePath, out errorMessage, out oceanArchetype))
            {
                throw new Exception(string.Format("Cann't load archetype file: {0} due to error {1}", architypePath,
                    errorMessage));
            }

            var termDefinition =
                (oceanArchetype.ontology.term_definitions.FirstOrDefault(d => d.language == "ru") ??
                 oceanArchetype.ontology.term_definitions.FirstOrDefault(d => d.language == "en")) ??
                oceanArchetype.ontology.term_definitions.FirstOrDefault();

            OntologiesProvider ontologiesProvider = null;
            if (termDefinition != null)
            {
                ontologiesProvider = new OntologiesProvider(termDefinition.items);
                archetype.Caption = ontologiesProvider.GetText(oceanArchetype.concept);
            }

            archetype.Type = DataType.Object.ToString();
            archetype.Caption = oceanArchetype.archetype_id.value;

            archetype.Description = string.Format(
                "original_language: {0}, original_author: {1}, purpose: {2}, use: {3}, misuse: {4}, copyright {5}",
                oceanArchetype.original_language.terminology_id.value,
                oceanArchetype.description.original_author[0].Value,
                oceanArchetype.description.details[0].purpose,
                oceanArchetype.description.details[0].use,
                oceanArchetype.description.details[0].misuse,
                oceanArchetype.description.details[0].copyright);

            var extractedData = _modelObjectExtractor.Extract(oceanArchetype.definition, ontologiesProvider);
            if (!archetype.Properties.ContainsKey(extractedData.Key))
                archetype.Properties.Add(extractedData);

            if (!oceanArchetype.definition.occurrences.upperSpecified ||
                (oceanArchetype.definition.occurrences.upperSpecified &&
                oceanArchetype.definition.occurrences.upper != 1))
            {
                // Обрабатываем как массив
                archetype.Type = DataType.Array.ToString();
                archetype.Items = new DataSchema
                {
                    Type = DataType.Object.ToString(),
                    Properties = archetype.Properties
                };
                archetype.Properties = null;
            }

            // После загрузки архетипа выполняем проход по всем элементами для
            // разрешения внутренних ссылок
            // (например, свойства с именами ARCHETYPE_INTERNAL_REF необходимо заменить  
            // элементами, на которые они ссылаются)

            if (archetype.Properties != null)
                ResolveArchetypeInternalRefs(archetype.Properties, archetype);

            return archetype;
        }

        private void ResolveArchetypeInternalRefs(IDictionary<string, DataSchema> properties, DataSchema archetype)
        {
            var resolvedProperties = new Dictionary<string, DataSchema>();
            var propertiesToRemove = new List<string>();

            foreach (var property in properties)
            {
                if (property.Key.StartsWith("ARCHETYPE_INTERNAL_REF"))
                {
                    propertiesToRemove.Add(property.Key);

                    var linkPath = ((DocumentLink) property.Value.TypeInfo["DocumentLink"]).DocumentId;

                    var pathParts = linkPath.Replace("]", "").Split(new[] {"/items["}, StringSplitOptions.RemoveEmptyEntries);

                    // Выполняем поиск свойства по полному пути вида: /items[at0.34.1]/items[at0.10]
                    var currentItemUnderSearch = archetype.Properties.First().Value.Properties;

                    var searchResult = archetype.Properties.FirstOrDefault();

                    foreach (var pathPart in pathParts)
                    {
                        foreach (var dataSchema in currentItemUnderSearch)
                        {
                            if (dataSchema.Value.TypeInfo != null &&
                                dataSchema.Value.TypeInfo.ContainsKey("NodeId") &&
                                dataSchema.Value.TypeInfo["NodeId"].ToString() == pathPart)
                            {
                                searchResult = dataSchema;
                                break;
                            }
                        }

                        if (searchResult.Value.Type == DataType.Object.ToString())
                            currentItemUnderSearch = searchResult.Value.Properties;
                        else if (searchResult.Value.Type == DataType.Array.ToString())
                            currentItemUnderSearch = searchResult.Value.Items.Properties;
                    }

                    resolvedProperties.Add(searchResult.Key, searchResult.Value);
                }

                if (property.Value.Properties != null &&
                    property.Value.Properties.Count > 0)
                    ResolveArchetypeInternalRefs(property.Value.Properties, archetype);
            }

            foreach (var propsToRemove in propertiesToRemove)
            {
                properties.Remove(propsToRemove);
            }

            foreach (var resolvedProps in resolvedProperties)
            {
                properties.Add(resolvedProps);
            }
        }
    }
}
