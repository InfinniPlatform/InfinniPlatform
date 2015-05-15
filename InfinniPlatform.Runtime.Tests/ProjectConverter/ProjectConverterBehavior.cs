using System.IO;
using System.Reflection;
using InfinniPlatform.Runtime.Implementation.ProjectConverter;
using NUnit.Framework;

namespace InfinniPlatform.Runtime.Tests.ProjectConverter
{
    [TestFixture]
	[Ignore]
    [Category(TestCategories.UnitTest)]
    class ProjectConverterBehavior
    {
        //Чтобы проверить работу классов ProjectAnalyze и ProjectConvertorTo,
        //в примерах элементы расположенны в порядке, используемом в ProjectConvertorTo
        //Заголовок - Ссылки на сборки - Модули - Ссылки на проекты - Внедряемые ресурсы -
        //Дополнительно подключаемые ресурсы - Сборки, пространства имен которых должены быть импортированы

        [Test]
        [TestCase("example1.xml")]
        [TestCase("example2.xml")]
        [TestCase("example3.xml")]
        public void CompareInitialXmlWithResult(string file)
        {
            //Arrange
            var projectConverterFrom = new ProjectConverterFrom();

            var config = new ProjectConfig
            {
                DebugOutputPath = @"bin\Debug\",
                ReleaseOutputPath = @"bin\Release\",
                TargetFrameworkVersion = "v4.5"
            };
            var projStream = "InfinniPlatform.Runtime.Tests.Implementation.ProjectConverter.projExamples." + file;

            //Act
            var projectConverterTo = new ProjectConverterTo(config);
            var initialProject = Assembly.GetExecutingAssembly().GetManifestResourceStream(projStream);
            if (initialProject == null) Assert.Fail();

            var projectObject = projectConverterFrom.Convert(initialProject);
            var resultProject = projectConverterTo.Convert(projectObject);
            if (resultProject == null) Assert.Fail();

            string initialProjectText, resultProjectText;
            // поскольку в полученном тексте csproj  отсутствую переносы и не всегда совпадают пробелы
            // при сравнении proj файлов переносы и пробелы не учитываются

            //Assert
            initialProject.Position = 0;
            using (var reader1 = new StreamReader(initialProject))
            {
                initialProjectText = reader1.ReadToEnd()
                                            .Replace("\r\n", string.Empty)
                                            .Replace(" ", string.Empty);
            }

            resultProject.Position = 0;
            using (var reader2 = new StreamReader(resultProject))
            {
                resultProjectText = reader2.ReadToEnd()
                                            .Replace(" ", string.Empty);
            }

            StringAssert.AreEqualIgnoringCase(initialProjectText, resultProjectText);
        }
    }
}
