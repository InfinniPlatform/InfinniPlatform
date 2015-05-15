using System.Collections.Generic;
using System.IO;
using InfinniPlatform.Runtime.Implementation.ProjectConverter;
using NUnit.Framework;

namespace InfinniPlatform.Runtime.Tests.ProjectConverter
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    class ProjectConverterToBehavior
    {
        [Test]
        public void ShouldAddProjectProperty()
        {
            //Arrange
            var _config = new ProjectConfig
            {
                DebugOutputPath = @"bin\Debug\",
                ReleaseOutputPath = @"bin\Release\",
                TargetFrameworkVersion = "v4.5"
            };
            var _converter = new ProjectConverterTo(_config);

            var _projectObject = new Project();
            var projectProperty = new ProjectProperty
            {
                OutputType = "Library",
                ProjectGuid = "{21AF539B-437E-4450-96B3-436B104E8EFC}",
                RootNamespace = "Test",
                AssemblyName = "Test",
                ProjectTypeGuids = "{E53F8FEA-EAE0-44A6-8774-FFD645390401};{fae04ec0-301f-11d3-bf4b-00c04f79efbc}"
            };
            _projectObject.ProjectProperty = projectProperty;

            //Act
            var projectStream = _converter.Convert(_projectObject);
            string resultProjectText;
            projectStream.Position = 0;
            using (var reader = new StreamReader(projectStream))
            {
                resultProjectText = reader.ReadToEnd();
            }

            //Assert
            StringAssert.Contains(@"<ProjectGuid>{21AF539B-437E-4450-96B3-436B104E8EFC}</ProjectGuid>", resultProjectText);
            StringAssert.Contains(@"<ProjectTypeGuids>{E53F8FEA-EAE0-44A6-8774-FFD645390401};{fae04ec0-301f-11d3-bf4b-00c04f79efbc}</ProjectTypeGuids>", resultProjectText);
            StringAssert.Contains(@"<OutputType>Library</OutputType>", resultProjectText);
            StringAssert.Contains(@"<RootNamespace>Test</RootNamespace>", resultProjectText);
            StringAssert.Contains(@"<AssemblyName>Test</AssemblyName>", resultProjectText);
        }

        [Test]
        public void ShouldUseConfig()
        {
            //Arrange
            var _config = new ProjectConfig
            {
                DebugOutputPath = @"bin\Debug\",
                ReleaseOutputPath = @"bin\Release\",
                TargetFrameworkVersion = "v4.5"
            };
            var _converter = new ProjectConverterTo(_config);

            var _projectObject = new Project();
            var projectProperty = new ProjectProperty
            {
                OutputType = "Library",
                ProjectGuid = "{21AF539B-437E-4450-96B3-436B104E8EFC}",
                RootNamespace = "Test",
                AssemblyName = "Test"
            };
            _projectObject.ProjectProperty = projectProperty;

            //Act
            var projectStream = _converter.Convert(_projectObject);
            string resultProjectText;
            projectStream.Position = 0;
            using (var reader = new StreamReader(projectStream))
            {
                resultProjectText = reader.ReadToEnd();
            }

            //Assert
            StringAssert.Contains("<TargetFrameworkVersion>" + _config.TargetFrameworkVersion + "</TargetFrameworkVersion>", resultProjectText);
            StringAssert.Contains("<OutputPath>" + _config.DebugOutputPath + "</OutputPath>", resultProjectText);
            StringAssert.Contains("<OutputPath>" + _config.ReleaseOutputPath + "</OutputPath>", resultProjectText);
        }

        [Test]
        public void ShouldAddModules()
        {
            //Arrange
            var _config = new ProjectConfig
            {
                DebugOutputPath = @"bin\Debug\",
                ReleaseOutputPath = @"bin\Release\",
                TargetFrameworkVersion = "v4.5"
            };
            var _converter = new ProjectConverterTo(_config);

            var _projectObject = new Project();
            var projectProperty = new ProjectProperty
            {
                OutputType = "Library",
                ProjectGuid = "{21AF539B-437E-4450-96B3-436B104E8EFC}",
                RootNamespace = "Test",
                AssemblyName = "Test"
            };
            _projectObject.ProjectProperty = projectProperty;

            var modules = new List<Module>{
            new Module{Include = @"ActionUnits\ActionUnitDeleteDocument.cs"},
            new Module{Include = @"..\Files\VersionInfo.cs", Link=@"Properties\VersionInfo.cs"},
            new Module{Include = @"Properties\Resources.Designer.cs", AutoGen="True", DesignTime="True", DependentUpon="Resources.resx"}
            };
            _projectObject.Modules = modules;

            //Act
            var projectStream = _converter.Convert(_projectObject);
            string resultProjectText;
            projectStream.Position = 0;
            using (var reader = new StreamReader(projectStream))
            {
                resultProjectText = reader.ReadToEnd();
            }

            //Assert
            const string expectation0 = @"<Compile Include=""ActionUnits\ActionUnitDeleteDocument.cs"" />";
            const string expectation1 = @"<Compile Include=""..\Files\VersionInfo.cs"">" +
                                        "<Link>Properties\\VersionInfo.cs</Link>" +
                                        "</Compile>";
            const string expectation2 = @"<Compile Include=""Properties\Resources.Designer.cs"">" +
                                        "<AutoGen>True</AutoGen>" +
                                        "<DesignTime>True</DesignTime>" +
                                        "<DependentUpon>Resources.resx</DependentUpon>" +
                                        "</Compile>";

            StringAssert.Contains(expectation0, resultProjectText);
            StringAssert.Contains(expectation1, resultProjectText);
            StringAssert.Contains(expectation2, resultProjectText);
        }

        [Test]
        public void ShouldAddReferences()
        {
            //Arrange
            var _config = new ProjectConfig
            {
                DebugOutputPath = @"bin\Debug\",
                ReleaseOutputPath = @"bin\Release\",
                TargetFrameworkVersion = "v4.5"
            };
            var _converter = new ProjectConverterTo(_config);

            var _projectObject = new Project();
            var projectProperty = new ProjectProperty
            {
                OutputType = "Library",
                ProjectGuid = "{21AF539B-437E-4450-96B3-436B104E8EFC}",
                RootNamespace = "Test",
                AssemblyName = "Test"
            };
            _projectObject.ProjectProperty = projectProperty;

            var references = new List<Reference>{
            new Reference
                {
                    Include = @"System.Configuration",
                    RequiredTargetFramework = "3.5"
                },
            new Reference
                {
                    Include = @"Autofac.Configuration",
                    HintPath=@"..\packages\Autofac.3.0.2\lib\net40\Autofac.Configuration.dll",
                    Private = "True"
                },
            new Reference
                {
                    Include = @"Nest, Version=0.9.21.0, Culture=neutral, processorArchitecture=MSIL", 
                    SpecificVersion="False", 
                    HintPath=@"..\packages\NEST.0.9.21.0\lib\NET4\Nest.dll"
                }
            };
            _projectObject.References = references;

            //Act
            var projectStream = _converter.Convert(_projectObject);
            string resultProjectText;
            projectStream.Position = 0;
            using (var reader = new StreamReader(projectStream))
            {
                resultProjectText = reader.ReadToEnd();
            }

            //Assert
            const string expectation0 = @"<Reference Include=""System.Configuration"">" +
                                        "<RequiredTargetFramework>3.5</RequiredTargetFramework>" +
                                        "</Reference>";
            const string expectation1 = @"<Reference Include=""Autofac.Configuration"">" +
                                        @"<HintPath>..\packages\Autofac.3.0.2\lib\net40\Autofac.Configuration.dll</HintPath>" +
                                        "<Private>True</Private>" +
                                        "</Reference>";
            const string expectation2 = @"<Reference Include=""Nest, Version=0.9.21.0, Culture=neutral, processorArchitecture=MSIL"">" +
                                        "<SpecificVersion>False</SpecificVersion>" +
                                        @"<HintPath>..\packages\NEST.0.9.21.0\lib\NET4\Nest.dll</HintPath>" +
                                        "</Reference>";

            StringAssert.Contains(expectation0, resultProjectText);
            StringAssert.Contains(expectation1, resultProjectText);
            StringAssert.Contains(expectation2, resultProjectText);
        }

        [Test]
        public void ShouldAddProjectReferences()
        {
            //Arrange
            var _config = new ProjectConfig
            {
                DebugOutputPath = @"bin\Debug\",
                ReleaseOutputPath = @"bin\Release\",
                TargetFrameworkVersion = "v4.5"
            };
            var _converter = new ProjectConverterTo(_config);

            var _projectObject = new Project();
            var projectProperty = new ProjectProperty
            {
                OutputType = "Library",
                ProjectGuid = "{21AF539B-437E-4450-96B3-436B104E8EFC}",
                RootNamespace = "Test",
                AssemblyName = "Test"
            };
            _projectObject.ProjectProperty = projectProperty;

            var projectReferences = new List<ProjectReference>{
            new ProjectReference
                {
                    Include = @"..\MultiCare.API\MultiCare.API.csproj", 
                    ProjectGuid="{B5011B31-7822-4A08-9DE0-4BA1D43F77BA}", 
                    Name="MultiCare.API"
                }
            };
            _projectObject.ProjectReferences = projectReferences;

            //Act
            var projectStream = _converter.Convert(_projectObject);
            string resultProjectText;
            projectStream.Position = 0;
            using (var reader = new StreamReader(projectStream))
            {
                resultProjectText = reader.ReadToEnd();
            }

            //Assert
            const string expectation = @"<ProjectReference Include=""..\MultiCare.API\MultiCare.API.csproj"">" +
                                       "<Project>{B5011B31-7822-4A08-9DE0-4BA1D43F77BA}</Project>" +
                                       "<Name>MultiCare.API</Name>" +
                                       "</ProjectReference>";

            StringAssert.Contains(expectation, resultProjectText);
        }

        [Test]
        public void ShouldAddEmbeddedResource()
        {
            //Arrange
            var _config = new ProjectConfig
            {
                DebugOutputPath = @"bin\Debug\",
                ReleaseOutputPath = @"bin\Release\",
                TargetFrameworkVersion = "v4.5"
            };
            var _converter = new ProjectConverterTo(_config);

            var _projectObject = new Project();
            var projectProperty = new ProjectProperty
            {
                OutputType = "Library",
                ProjectGuid = "{21AF539B-437E-4450-96B3-436B104E8EFC}",
                RootNamespace = "Test",
                AssemblyName = "Test"
            };
            _projectObject.ProjectProperty = projectProperty;

            var embeddedResources = new List<EmbeddedResource>{
            new EmbeddedResource
                {
                    Include = @"Properties\Resources.resx", 
                    Generator="PublicResXFileCodeGenerator", 
                    LastGenOutput="Resources.Designer.cs",
                    SubType="Designer"
                }
            };
            _projectObject.EmbeddedResources = embeddedResources;

            //Act
            var projectStream = _converter.Convert(_projectObject);
            string resultProjectText;
            projectStream.Position = 0;
            using (var reader = new StreamReader(projectStream))
            {
                resultProjectText = reader.ReadToEnd();
            }

            //Assert
            const string expectation = @"<EmbeddedResource Include=""Properties\Resources.resx"">" +
                                       "<Generator>PublicResXFileCodeGenerator</Generator>" +
                                       "<LastGenOutput>Resources.Designer.cs</LastGenOutput>" +
                                       "<SubType>Designer</SubType>" +
                                       "</EmbeddedResource>";

            StringAssert.Contains(expectation, resultProjectText);
        }

        [Test]
        public void ShouldAddContent()
        {
            //Arrange
            var _config = new ProjectConfig
            {
                DebugOutputPath = @"bin\Debug\",
                ReleaseOutputPath = @"bin\Release\",
                TargetFrameworkVersion = "v4.5"
            };
            var _converter = new ProjectConverterTo(_config);

            var _projectObject = new Project();
            var projectProperty = new ProjectProperty
            {
                OutputType = "Library",
                ProjectGuid = "{21AF539B-437E-4450-96B3-436B104E8EFC}",
                RootNamespace = "Test",
                AssemblyName = "Test"
            };
            _projectObject.ProjectProperty = projectProperty;

            var contents = new List<Content>{
            new Content{Include = @"Modules.txt"}
            };
            _projectObject.Contents = contents;

            //Act
            var projectStream = _converter.Convert(_projectObject);
            string resultProjectText;
            projectStream.Position = 0;
            using (var reader = new StreamReader(projectStream))
            {
                resultProjectText = reader.ReadToEnd();
            }

            //Assert
            const string expectation = @"<Content Include=""Modules.txt"" />";

            StringAssert.Contains(expectation, resultProjectText);
        }

        [Test]
        public void ShouldAddImport()
        {
            //Arrange
            var _config = new ProjectConfig
            {
                DebugOutputPath = @"bin\Debug\",
                ReleaseOutputPath = @"bin\Release\",
                TargetFrameworkVersion = "v4.5"
            };
            var _converter = new ProjectConverterTo(_config);

            var _projectObject = new Project();
            var projectProperty = new ProjectProperty
            {
                OutputType = "Library",
                ProjectGuid = "{21AF539B-437E-4450-96B3-436B104E8EFC}",
                RootNamespace = "Test",
                AssemblyName = "Test"
            };
            _projectObject.ProjectProperty = projectProperty;

            var imports = new List<Import>{
            new Import
                {
                    Project = @"$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props",
                    Condition = @"Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"
                },
            new Import{Project = @"$(MSBuildToolsPath)\Microsoft.CSharp.targets"},
            };
            _projectObject.Imports = imports;

            //Act
            var projectStream = _converter.Convert(_projectObject);
            string resultProjectText;
            projectStream.Position = 0;
            using (var reader = new StreamReader(projectStream))
            {
                resultProjectText = reader.ReadToEnd();
            }

            //Assert
            const string expectation1 = @"<Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"" Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"" />";
            const string expectation2 = @"<Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />";

            StringAssert.Contains(expectation1, resultProjectText);
            StringAssert.Contains(expectation2, resultProjectText);
        }
    }
}
