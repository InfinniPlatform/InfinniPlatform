using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.ModelRepository;
using InfinniPlatform.Api.ModelRepository.MetadataObjectModel;

using OpenEhr.V1.Its.Xml.AM;
using System;
using System.IO;

namespace InfinniPlatform.OceanInformatics.DataModelLoader
{
    public sealed class TemplateExtractor : ITemplateExtractor
    {
        private readonly ObjectModelExtractor _modelPropertyGroupExtractor;
        private readonly OceanDocumentsLoader _templateLoader;

        public TemplateExtractor(string configId)
        {
            _modelPropertyGroupExtractor = new ObjectModelExtractor(false, configId);
            _templateLoader = new OceanDocumentsLoader();
        }

        public DataSchema Extract(FileStream stream)
        {
            OPERATIONAL_TEMPLATE oceanTemplate;
            string errorMessage;
            if (!_templateLoader.BuildOperationalTemplateFromOptFile(
                stream,
                out errorMessage, 
                out oceanTemplate))
            {
                throw new Exception(string.Format("Can't load template file: {0} due to error {1}", stream, errorMessage));
            }

            return GetDataSchema(oceanTemplate);
        }

        public DataSchema Extract(string oetFilePath, string templatesFolder, string archetypesFolder)
        {
            OPERATIONAL_TEMPLATE oceanTemplate;
            string errorMessage;
            if (!_templateLoader.BuildOperationalTemplateFromOetFile(
                oetFilePath,
                templatesFolder,
                archetypesFolder,
                out errorMessage,
                out oceanTemplate))
            {
                throw new Exception(string.Format("Cann't load template file: {0} due to error {1}", oetFilePath, errorMessage));
            }

            return GetDataSchema(oceanTemplate);
        }

        DataSchema GetDataSchema(OPERATIONAL_TEMPLATE oceanTemplate)
        {
            var description = string.Empty;

            if (oceanTemplate.description != null)
            {
                description = string.Format(
                                            "original_language: {0}, original_author: {1}, purpose: {2}, use: {3}, misuse: {4}, copyright {5}",
                                            oceanTemplate.template_id.value,
                                            oceanTemplate.description.original_author[0].Value,
                                            oceanTemplate.description.details[0].purpose,
                                            oceanTemplate.description.details[0].use,
                                            oceanTemplate.description.details[0].misuse,
                                            oceanTemplate.description.details[0].copyright);
            }

            var document = new DataSchema
            {
                //Name = oceanTemplate.definition.archetype_id.value,
                Caption = oceanTemplate.concept,
                //Id = oceanTemplate.template_id.value,
                Description = description
            };

            var ontologiesPropvider = new OntologiesProvider(oceanTemplate.definition.term_definitions);

            //document.Model.Name = document.Caption.ToTranslit();
            document.Description = document.Description;
            document.Caption = document.Caption;
            document.Properties.Add(_modelPropertyGroupExtractor.Extract(oceanTemplate.definition, ontologiesPropvider));

            if (oceanTemplate.definition.occurrences.upperSpecified && oceanTemplate.definition.occurrences.upper == 1)
                document.Type = DataType.Object.ToString();
            else
            {
                document.Type = DataType.Array.ToString();
                document.Items = new DataSchema
                {
                    Type = DataType.Object.ToString(),
                    Properties = document.Properties
                };
                document.Properties = null;
            }

            return document;
        }
    }
}
