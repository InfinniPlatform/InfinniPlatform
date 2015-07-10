using System.IO;
using System.Linq;
using System.Reflection;
using InfinniPlatform.Runtime.Implementation.ProjectConverter;
using NUnit.Framework;

namespace InfinniPlatform.Runtime.Tests.ProjectConverter
{
    [TestFixture]
	[Ignore]
    [Category(TestCategories.UnitTest)]
    public sealed class ProjectConverterFromBehavior
    {
        private ProjectConverterFrom _converter;
        private Project _project;

        [SetUp]
        public void InitializationProjectAnalyzer()
        {
            _converter = new ProjectConverterFrom();
            var projXml = Assembly.GetExecutingAssembly().GetManifestResourceStream("InfinniPlatform.Runtime.Tests.Implementation.ProjectConverter.projExamples.example1.xml");

            _project = _converter.Convert(projXml);
        }

        [Test]
        public void ShouldSetProjectProperty()
        {
            var projectProperty = _project.ProjectProperty;

            Assert.AreEqual(projectProperty.ProjectGuid, "{E4FA5829-B1C4-40EA-9FCF-F877D0C34E86}");
            Assert.AreEqual(projectProperty.ProjectTypeGuids, "{E53F8FEA-EAE0-44A6-8774-FFD645390401}");
            Assert.AreEqual(projectProperty.OutputType, "Library");
            Assert.AreEqual(projectProperty.RootNamespace, "InfinniConfiguration.Integration");
            Assert.AreEqual(projectProperty.AssemblyName, "InfinniConfiguration.Integration");
        }

        [Test]
        public void ShouldRegisterAllModules()
        {
            var module = _project.Modules.ToList();

			Assert.NotNull(module.Find(m => m.Include == Path.Combine("ActionUnits", "ActionUnitDeleteDocument.cs")));
			Assert.NotNull(module.Find(m => (m.Include == Path.Combine("..", "Files", "VersionInfo.cs") &&
                                             m.Link == Path.Combine("Properties", "VersionInfo.cs"))));
            Assert.NotNull(module.Find(m => (m.Include == Path.Combine("Properties", "Resources.Designer.cs") &&
                                                      m.AutoGen == "True" &&
                                                      m.DesignTime == "True" &&
                                                      m.DependentUpon == "Resources.resx")));
        }

        [Test]
        public void ShouldRegisterAllReferences()
        {
            var references = _project.References.ToList();

            Assert.NotNull(references.Find(r => r.Include == "System"));
            Assert.NotNull(references.Find(r => r.Include == "System.ComponentModel.DataAnnotations" &&
                                                r.RequiredTargetFramework == "3.5" &&
                                                r.Private == "True"));
            Assert.NotNull(references.Find(r =>
                (r.Include == "Nest, Version=0.9.21.0, Culture=neutral, processorArchitecture=MSIL") &&
                 r.SpecificVersion == "False" &&
                 r.HintPath == @"..\packages\NEST.0.9.21.0\lib\NET4\Nest.dll"
                ));
        }

        [Test]
        public void ShouldRegisterAllProjectReferences()
        {
            var projectReferences = _project.ProjectReferences.ToList();

            Assert.NotNull(projectReferences.Find(p => (p.Include == @"..\InfinniPlatform.Api\InfinniPlatform.Api.csproj" &&
                                                        p.Name == "InfinniPlatform.Api" &&
                                                        p.ProjectGuid == "{7ca0f96c-7ff4-44b2-9d44-851984df4532}")));
        }

        [Test]
        public void ShouldRegisterAllEmbeddedResources()
        {
            var embeddedResources = _project.EmbeddedResources.ToList();

            Assert.NotNull(embeddedResources.Find(e => (e.Include == @"Properties\Resources.resx" &&
                                                        e.Generator == "ResXFileCodeGenerator" &&
                                                        e.LastGenOutput == "Resources.Designer.cs" &&
                                                        e.SubType == "Designer")));
        }

        [Test]
        public void ShouldRegisterAllContents()
        {
            var contents = _project.Contents.ToList();

            Assert.NotNull(contents.Find(c => c.Include == @"Modules.txt"));
        }

        [Test]
        public void ShouldRegisterAllImports()
        {
            var imports = _project.Imports.ToList();

            Assert.NotNull(imports.Find(i => i.Project == @"$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props" &&
                                                i.Condition == @"Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"));
            Assert.NotNull(imports.Find(i => i.Project == @"$(MSBuildToolsPath)\Microsoft.CSharp.targets"));
        }
    }
}
