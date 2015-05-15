using InfinniPlatform.Api.ModelRepository.MetadataObjectModel;
using InfinniPlatform.ModelRepository.OpenEhrDataConverter;
using InfinniPlatform.OceanInformatics.DataModelLoader;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NUnit.Framework;

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace InfinniPlatform.ModelRepository.Tests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public sealed class OpenEhrDataConvertionTest
    {
        [Test]
        public void ConvertForm025AndForm066Test()
        {
            string errorMessage;
            JObject result;

            var complexTypeExtractor = new ComplexTypeExtractor("C74887E5-D039-4215-977C-05BC827B6585");
            var complexTypes = complexTypeExtractor.ExtractComplexTypeModels();

            var form25V2 =
                JToken.ReadFrom(new JsonTextReader(new StreamReader("OpenEhrTestData/TemplateForm025.txt")))
                      .ToObject<DataSchema>();
            //var form66V2 =
            //    JToken.ReadFrom(new JsonTextReader(new StreamReader("OpenEhrTestData/TemplateForm066.txt")))
            //          .ToObject<DataSchema>();

            var dataConverter = new OceanOpenEhrDataConverter();

            Assert.IsTrue(dataConverter.ConvertData(
                new FileStream("OpenEhrTestData/Form025.xml", FileMode.Open),
                form25V2,
                complexTypes,
                out errorMessage,
                out result));

            //File.WriteAllText("Form 025 data.txt", result.ToString());

            //Assert.IsTrue(dataConverter.ConvertData(
            //    new FileStream("OpenEhrTestData/Form066.xml", FileMode.Open),
            //    form66V2,
            //    complexTypes,
            //    out errorMessage,
            //    out result));

            //File.WriteAllText("Form 066 data.txt", result.ToString());

            //File.WriteAllText("complex types.txt", JArray.FromObject(complexTypes).ToString());
        }

        [Test]
        public void ConvertForm025Test()
        {
            string errorCollection;

            var factory = new OceanOpenEhrFactory();

            var archetypeExtractor = factory.BuildArchetypeExtractor(); 
            var complexTypeExtractor = factory.BuildComplexTypeExtractor(); 
            var templateExtractor = factory.BuildTemplateExtractor();
            var dataExtractor = factory.BuildDataConverter(); 

            const string dataFileName1 = "OpenEhrTestData/025_1.xml";
            const string dataFileName2 = "OpenEhrTestData/025_2.xml";
            const string oetFileName = "OpenEhrTestData/template025.oet";
            
            // Generating archetypes
            foreach (var archetypeFile in Directory.GetFiles("OpenEhrTestData").Where(archetypeFile => archetypeFile.EndsWith("adl")))
            {
                var archetype = archetypeExtractor.Extract(archetypeFile);
                Assert.IsNotNull(archetype);
            }

            // Generating template
            var template = templateExtractor.Extract(oetFileName, "OpenEhrTestData", "OpenEhrTestData");
            Assert.IsNotNull(template);

            var serializedTemplate = JsonConvert.SerializeObject(template);

            //File.WriteAllText("TemplateForm025.txt", JObject.FromObject(template).ToString());

            template = JsonConvert.DeserializeObject<DataSchema>(serializedTemplate);

            // Extract complex types
            var complexTypes = complexTypeExtractor.ExtractComplexTypeModels();
            foreach (var complexTypeModel in complexTypes)
            {
                Assert.IsNotNull(complexTypeModel);
            }

            // Extract data
            JObject result;

            var sWatch = new Stopwatch();

            // Один xml документ может содержать несколько секций типа COMPOSITION.
            // Каждая такая секция представляет собой отдельный документ, который необходимо выделить.
            // Публиковать в ИЭМК нужно именно элементы типа COMPOSITION
            // Следующий блок кода выделяет из документа секции с COMPOSITION

            // Загрузку из файла заменить на загрузку документа из строки!
            var sourceFileContent = XDocument.Load(new FileStream(dataFileName1, FileMode.Open)).Root;
            IList<XElement> compositions = new List<XElement>();

            if (sourceFileContent != null)
            {
                var ns = sourceFileContent.GetNamespaceOfPrefix("xsi");
                if (ns != null)
                {
                    foreach (var childElement in sourceFileContent.Elements())
                    {
                        var attibute = childElement.Attribute(XName.Get("type", ns.NamespaceName));
                        if (attibute != null && attibute.Value == "COMPOSITION")
                        {
                            compositions.Add(childElement);
                        }
                    }
                }
            }

            sWatch.Restart();

            // Convert first document 
            Assert.IsTrue(
                dataExtractor.ConvertData(
                new MemoryStream(Encoding.UTF8.GetBytes(compositions[0].ToString())), 
                template, 
                complexTypes, 
                out errorCollection, 
                out result));

            sWatch.Stop();

            sourceFileContent = XDocument.Load(new FileStream(dataFileName2, FileMode.Open)).Root;
            var sourceData = sourceFileContent.Element(XName.Get("data", sourceFileContent.GetDefaultNamespace().NamespaceName));

            // Convert second document 
            Assert.IsTrue(
                dataExtractor.ConvertData(
                new MemoryStream(Encoding.UTF8.GetBytes(sourceData.ToString())), 
                template,
                complexTypes,
                out errorCollection,
                out result));
        }
    }
}
