using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

using NUnit.Framework;

namespace InfinniPlatform.Tests.Conventions
{
    [TestFixture]
    [Category(TestCategories.BuildTest)]
    public class StructurePlatformConventionsTest
    {
        static StructurePlatformConventionsTest()
        {
            SolutionDir = FindSolutionDirectory();
            SolutionProjects = Directory.GetDirectories(SolutionDir, $"{SolutionName}.*").ToArray();
            SolutionCodeProjects = SolutionProjects.Where(p => !p.EndsWith(".Tests")).ToArray();

            Console.WriteLine(@"SolutionDir={0}", SolutionDir);
        }

        private static string FindSolutionDirectory()
        {
            string solutionDir = null;

            var currentDirectory = Directory.GetCurrentDirectory();

            while (!string.IsNullOrEmpty(currentDirectory) && Directory.Exists(currentDirectory))
            {
                if (File.Exists(Path.Combine(currentDirectory, "InfinniPlatform.sln")))
                {
                    solutionDir = currentDirectory;
                    break;
                }

                currentDirectory = Path.GetDirectoryName(currentDirectory);
            }

            return solutionDir;
        }


        private const string SolutionName = "InfinniPlatform";

        private static readonly string SolutionDir;
        private static readonly string[] SolutionProjects;
        private static readonly string[] SolutionCodeProjects;


        [Test]
        [TestCaseSource(nameof(SolutionCodeProjects))]
        [Description(@"The project should have the resource file")]
        public void CodeProjectShouldHaveResources(string project)
        {
            // When
            var hasResources = File.Exists(Path.Combine(project, @"Properties", "Resources.resx"))
                               && File.Exists(Path.Combine(project, @"Properties", "Resources.Designer.cs"));

            // Then
            AssertCondition(hasResources, @"The project should have the resource file ""Properties/Resources.resx""");
        }

        [Test]
        [TestCaseSource(nameof(SolutionCodeProjects))]
        [Description(@"The project should have <AssemblyName> property")]
        public void ProjectShouldHaveAssemblyNameProperty(string project)
        {
            // Given
            var projectXml = LoadProjectXml(project);

            // When
            var assemblyName = FindChildren(projectXml, "PropertyGroup")
                .SelectMany(e => FindChildren(e, "AssemblyName"))
                .FirstOrDefault();

            // Then
            AssertCondition(!string.IsNullOrWhiteSpace(assemblyName?.Value), @"The project should have <AssemblyName> property");
        }

        [Test]
        [TestCaseSource(nameof(SolutionCodeProjects))]
        [Description(@"The project should have <RootNamespace> property")]
        public void ProjectShouldHaveRootNamespaceProperty(string project)
        {
            // Given
            var projectXml = LoadProjectXml(project);

            // When
            var rootNamespace = FindChildren(projectXml, "PropertyGroup")
                .SelectMany(e => FindChildren(e, "RootNamespace"))
                .FirstOrDefault();

            // Then
            AssertCondition(!string.IsNullOrWhiteSpace(rootNamespace?.Value), @"The project should have <RootNamespace> property");
        }

        [Test]
        [TestCaseSource(nameof(SolutionCodeProjects))]
        [Description(@"The project should have <GenerateAssemblyInfo> property which equals false")]
        public void ProjectShouldHaveGenerateAssemblyInfoProperty(string project)
        {
            // Given
            var projectXml = LoadProjectXml(project);

            // When
            var generateAssemblyInfo = FindChildren(projectXml, "PropertyGroup")
                .SelectMany(e => FindChildren(e, "GenerateAssemblyInfo"))
                .FirstOrDefault();

            // Then
            AssertCondition("false".Equals(generateAssemblyInfo?.Value, StringComparison.OrdinalIgnoreCase), @"The project should have <GenerateAssemblyInfo> property which equals false");
        }

        [Test]
        [TestCaseSource(nameof(SolutionCodeProjects))]
        [Description(@"The project should have reference on GlobalAssemblyInfo.cs")]
        public void ProjectShouldHaveReferenceOnGlobalAssemblyInfo(string project)
        {
            // Given
            var projectXml = LoadProjectXml(project);

            // When
            var globalAssemblyInfo = FindChildren(projectXml, "ItemGroup")
                .SelectMany(e => FindChildren(e, "Compile"))
                .Where(e => @"Properties\GlobalAssemblyInfo.cs".Equals(e.Attribute("Link")?.Value, StringComparison.Ordinal)
                            && @"..\Files\Packaging\GlobalAssemblyInfo.cs".Equals(e.Attribute("Include")?.Value, StringComparison.Ordinal))
                .FirstOrDefault();

            // Then
            AssertCondition(globalAssemblyInfo != null, @"The project should have reference on GlobalAssemblyInfo.cs");
        }

        [Test]
        [TestCaseSource(nameof(SolutionProjects))]
        [Description(@"The project should depend on packages only")]
        public void ProjectShouldDependOnPackagesOnly(string project)
        {
            // Given
            var projectXml = LoadProjectXml(project);

            // When
            var assemblyReferences = GetAssemblyReferences(projectXml);

            // Then
            AssertCondition(assemblyReferences.Count == 0, @"The project should depend on packages only but contains assembly references", assemblyReferences);
        }

        [Test]
        [TestCaseSource(nameof(SolutionProjects))]
        [Description(@"The project should depend on the solution projects only")]
        public void ProjectShouldDependOnSolutionProjectsOnly(string project)
        {
            // Given
            var projectXml = LoadProjectXml(project);

            // When
            var projectReferences = GetProjectReferences(projectXml);
            var externalProjectReferences = projectReferences.Where(i => !GetFullPathFromRelative(project, i).StartsWith(SolutionDir)).ToList();

            // Then
            AssertCondition(externalProjectReferences.Count == 0, @"The project should depend on the solution projects only but contains external references", externalProjectReferences);
        }

        [Test]
        [Description(@"Projects should use the same versions of packages")]
        public void ProjectsShouldUseTheSameVersionsOfPackages()
        {
            // Given
            var packages = SolutionProjects.SelectMany(project => GetPackageReferences(LoadProjectXml(project))).ToList();

            // When
            var unconsolidatedPackages = packages.GroupBy(p => p.Item1, (p, g) => new { Name = p, Versions = g.Select(i => i.Item2).Distinct().ToList() })
                .Where(p => p.Versions.Count > 1).Select(p => $"{p.Name} {string.Join(", ", p.Versions.OrderBy(i => i))}").ToList();

            // Then
            AssertCondition(unconsolidatedPackages.Count == 0, @"Projects should use the same versions of packages but there are some unconsolidated packages", unconsolidatedPackages);
        }

        [Test]
        [Description(@"All tests should have a category")]
        public void AllTestsShouldHaveCategory()
        {
            // Given
            var testAssemblies = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "*.Tests.dll");

            // When

            var assemblyLoadContext = new ReflectionOnlyAssemblyLoadContext();

            var withoutCategory = testAssemblies
                .SelectMany(a => assemblyLoadContext.LoadFromAssemblyPath(a)
                                                    .GetTypes()
                                                    .Where(t =>
                                                           {
                                                               var ti = t.GetTypeInfo();

                                                               if (ti.IsClass && !ti.IsAbstract)
                                                               {
                                                                   var ignoreAttr = ti.GetCustomAttribute<IgnoreAttribute>();

                                                                   if (ignoreAttr != null)
                                                                   {
                                                                       return false;
                                                                   }

                                                                   var testFixtureAttr = ti.GetCustomAttribute<TestFixtureAttribute>();

                                                                   if (testFixtureAttr != null)
                                                                   {
                                                                       if (!string.IsNullOrEmpty(testFixtureAttr.Category))
                                                                       {
                                                                           return false;
                                                                       }

                                                                       var categoryAttr = ti.GetCustomAttribute<CategoryAttribute>();

                                                                       if (!string.IsNullOrEmpty(categoryAttr?.Name))
                                                                       {
                                                                           return false;
                                                                       }

                                                                       return true;
                                                                   }
                                                               }

                                                               return false;
                                                           })
                                                    .Select(t => t.FullName))
                .ToList();

            // Then
            AssertCondition(withoutCategory.Count == 0, @"All tests should have a category but there are some tests without category", withoutCategory);
        }

        [Test]
        [Description(@"All *.cs files should be UTF-8 with BOM")]
        public void AllFilesShouldBeEncodedInUtf8()
        {
            // Given
            var objPath = Path.DirectorySeparatorChar + "obj" + Path.DirectorySeparatorChar;
            var codeFiles = Directory.GetFiles(SolutionDir, "*.cs", SearchOption.AllDirectories).Where(f => !f.Contains(objPath));

            // When

            var nonUtf8Files = new List<string>();

            foreach (var codeFile in codeFiles)
            {
                using (var codeFileStream = new FileStream(codeFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var byteOrderMark = new byte[3];
                    codeFileStream.Read(byteOrderMark, 0, 3);

                    // Determines UTF-8 BOM
                    if ((byteOrderMark[0] == 0xEF && byteOrderMark[1] == 0xBB && byteOrderMark[2] == 0xBF) == false)
                    {
                        nonUtf8Files.Add(codeFile);
                    }
                }
            }

            // Then
            AssertCondition(nonUtf8Files.Count == 0, @"All *.cs files should be UTF-8 with BOM", nonUtf8Files);
        }


        private static IList<Tuple<string, string>> GetPackageReferences(XElement projectXml)
        {
            return FindChildren(projectXml, "ItemGroup")
                .SelectMany(e => FindChildren(e, "PackageReference"))
                .Select(e => Tuple.Create(e.Attribute("Include").Value, e.Attribute("Version").Value))
                .ToList();
        }

        private static IList<string> GetProjectReferences(XElement projectXml)
        {
            return FindChildren(projectXml, "ItemGroup")
                .SelectMany(e => FindChildren(e, "ProjectReference"))
                .Select(e => e.Attribute("Include").Value)
                .ToList();
        }

        private static IList<string> GetAssemblyReferences(XElement projectXml)
        {
            return FindChildren(projectXml, "ItemGroup")
                .SelectMany(e => FindChildren(e, "Reference"))
                .SelectMany(e => FindChildren(e, "HintPath"))
                .Select(e => e.Value)
                .ToList();
        }


        private static XElement LoadProjectXml(string project)
        {
            return XDocument.Load(Path.Combine(project, $"{Path.GetFileName(project)}.csproj")).Root;
        }

        private static IEnumerable<XElement> FindChildren(XElement parent, string childName)
        {
            return parent.Elements().Where(i => string.Equals(i.Name.LocalName, childName, StringComparison.Ordinal));
        }


        private static string GetFullPathFromRelative(string basePath, string relativePath)
        {
            var absolutePath = Path.Combine(basePath, relativePath);

            return Path.GetFullPath(new Uri(absolutePath).LocalPath);
        }


        private static void AssertCondition(bool expected, string message, IEnumerable<string> messageItems = null)
        {
            if (!expected)
            {
                Assert.Fail(message + (messageItems.Any() ? $": {Environment.NewLine}{string.Join("; " + Environment.NewLine, messageItems)}" : ""));
            }
        }
    }
}