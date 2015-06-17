using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.ModelRepository;
using InfinniPlatform.Api.ModelRepository.MetadataObjectModel;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.OceanInformatics.DataModelLoader;

using NUnit.Framework;

namespace InfinniPlatform.ModelRepository.Tests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public sealed class SimiTemplatesConvertationBehavior
    {
		private IDisposable _server;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig)
			                                    .InstallFromJson("BasicTemplates.zip", new string[0]));
		}

        [TestFixtureTearDown]
        public void TearDownFixture()
        {
            _server.Dispose();
        }

        [Test]
        [Ignore(
            "Ручной тест - необходим только для разовой генерации конфигурации с набором документов, сгенерированных из архетипов СИМИ"
            )]
        public void CreateConfigurationWithSimiArchetypes()
        {
            IArchetypeExtractor archetypeExtractor = new ArchetypeExtractor("BasicTemplates");
            IComplexTypeExtractor complexTypeExtractor = new ComplexTypeExtractor("BasicTemplates");

            var managerDocument = new ManagerFactoryConfiguration(null, "basictemplates").BuildDocumentManager();

            // Сложные типы добавляем в конфигурацию как отдельные документы
            var complexTypes = complexTypeExtractor.ExtractComplexTypeModels();

            foreach (var complexType in complexTypes)
            {
                dynamic documentModel = new
                {
                    Id = complexType.Key,
                    Name = complexType.Key,
                    Caption = complexType.Value.Caption,
                    Description = "Структура сложного типа OpenEHR",
                    SearchAbility = 0,
                    Versioning = 2,
                    MetadataIndex = complexType.Key,
                    Services = new object[0],
                    Processes = new object[0],
                    Scenarios = new object[0],
                    Generators = new object[0],
                    Views = new object[0],
                    ValidationWarnings = new object[0],
                    ValidationErrors = new object[0],
                    DocumentStatuses = new[]
                    {
                        new
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = "Invalid",
                            Caption = "Invalid"
                        },
                        new
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = "Deleted",
                            Caption = "Deleted"
                        }
                    }
                }.ToDynamic();

                documentModel.Schema = ProcessSchema(complexType.Value);

                managerDocument.MergeItem(documentModel);
            }

            var notLoadedArchetypes = new List<string>();

            //const string folder = @"C:\Users\a.popov.INFINNITY\Documents\MultiCare docs\OpenEHR\Архетипы для тестов";
            const string folder = @"C:\Users\a.popov.INFINNITY\Documents\MultiCare docs\OpenEHR\Архетипы СИМИ";

            // Generating archetypes
            foreach (var archetypeFolder in Directory.GetDirectories(folder))
            {
                foreach (var archetypeFile in Directory
                    .GetFiles(archetypeFolder)
                    .Where(archetypeFile => archetypeFile.EndsWith("adl")))
                {
                    DataSchema archetype;

                    try
                    {
                        archetype = archetypeExtractor.Extract(archetypeFile);
                    }
                    catch (Exception e)
                    {
                        notLoadedArchetypes.Add(e.Message + ": " + archetypeFile);
                        continue;
                    }

                    var docName = archetype.Caption
                        .Replace(".", "_")
                        .Replace("-", "_")
                        .Replace("/", "_slash_");

                    dynamic documentModel = new
                    {
                        Id = docName,
                        Name = docName,
                        Caption = archetype.Properties.First().Value.Caption,
                        Description = "Структура архетипа OpenEHR из проекта СИМИ",
                        SearchAbility = 0,
                        Versioning = 2,
                        MetadataIndex = docName,
                        Services = new object[0],
                        Processes = new object[0],
                        Scenarios = new object[0],
                        Generators = new object[0],
                        Views = new object[0],
                        ValidationWarnings = new object[0],
                        ValidationErrors = new object[0],
                        DocumentStatuses = new[]
                        {
                            new
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "Invalid",
                                Caption = "Invalid"
                            },
                            new
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "Deleted",
                                Caption = "Deleted"
                            }
                        }
                    }.ToDynamic();

                    documentModel.Schema = ProcessSchema(archetype.Properties.First().Value);

                    managerDocument.MergeItem(documentModel);
                }
            }

            // Обновляем сведения о метаданных конфигураций после добавления всех документов в run-time 
            RestQueryApi.QueryPostNotify(null, "basictemplates");
        }

        /// <summary>
        /// Выполняет постобработку схемы:
        /// 1. Замена ссылок на сложные типы (DV_BOOLEAN, DV_TEXT и др) на примитивные типы
        /// 2. Вырезание свойства Tree
        /// </summary>
        /// <param name="sourceSchema"></param>
        /// <returns></returns>
        private dynamic ProcessSchema(DataSchema sourceSchema)
        {
            dynamic updatedSchema = new 
            {
                sourceSchema.Caption, 
                sourceSchema.Description, 
                sourceSchema.Type
            }.ToDynamic();

            if (sourceSchema.TypeInfo != null)
            {
                updatedSchema.TypeInfo = new DynamicWrapper();

                foreach (var property in sourceSchema.TypeInfo)
                {
                    updatedSchema.TypeInfo[RemoveInvalidChars(property.Key)] = property.Value;
                }
            }

            if (sourceSchema.Items != null)
            {
                updatedSchema.Items = ProcessSchema(sourceSchema.Items);
            }


            if (sourceSchema.TypeInfo != null && 
                sourceSchema.Type == DataType.Object.ToString() &&
                sourceSchema.Properties.Count == 0 &&
                sourceSchema.TypeInfo.ContainsKey("DocumentLink"))
            {
                string docName = sourceSchema.TypeInfo["DocumentLink"].ToDynamic().DocumentId.ToString();

                switch (docName)
                {
                    case "DV_BOOLEAN":
                        updatedSchema.Type = DataType.Bool.ToString();
                        updatedSchema.TypeInfo.DocumentLink = null;
                        break;
                    case "DV_DATE":
                    case "DV_TIME":
                    case "DV_DATE_TIME":
                        updatedSchema.Type = DataType.DateTime.ToString();
                        updatedSchema.TypeInfo.DocumentLink = null;
                        break;
                    case "DV_COUNT":
                        updatedSchema.Type = DataType.Integer.ToString();
                        updatedSchema.TypeInfoDocumentLink = null;
                        break;
                    case "DV_TEXT":
                        updatedSchema.Type = DataType.String.ToString();
                        updatedSchema.TypeInfo.DocumentLink = null;
                        break;
                }
            }

            if (sourceSchema.Properties != null)
            {
                updatedSchema.Properties = new DynamicWrapper();

                if (sourceSchema.Properties.Count == 1 &&
                    (sourceSchema.Properties.First().Key == "Tree" ||
                     sourceSchema.Properties.First().Key == "Derevo"))
                {
                    foreach (var property in sourceSchema.Properties.First().Value.Properties)
                    {
                        updatedSchema.Properties[RemoveInvalidChars(property.Key)] = ProcessSchema(property.Value);
                    }
                }
                else
                {
                    foreach (var property in sourceSchema.Properties)
                    {
                        updatedSchema.Properties[RemoveInvalidChars(property.Key)] = ProcessSchema(property.Value);
                    }
                }
            }

            return updatedSchema;
        }

        readonly string _invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars()) + ".";

        private string RemoveInvalidChars(string sourceString)
        {
            return _invalidChars.Aggregate(sourceString, (current, c) => current.Replace(c.ToString(), ""));
        }
    }
}
