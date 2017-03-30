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
            var currentDirectory = Directory.GetCurrentDirectory();

            var solutionDirIndex = currentDirectory.IndexOf(Path.DirectorySeparatorChar + SolutionName + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);

            SolutionDir = currentDirectory.Substring(0, solutionDirIndex + SolutionName.Length + 2);
            SolutionProjects = Directory.GetDirectories(SolutionDir, $"{SolutionName}.*").ToArray();
            SolutionCodeProjects = SolutionProjects.Where(p => !p.EndsWith(".Tests")).ToArray();

            Console.WriteLine(@"SolutionDir={0}", SolutionDir);
        }


        private const string SolutionName = "InfinniPlatform";

        private static readonly string SolutionDir;
        private static readonly string[] SolutionProjects;
        private static readonly string[] SolutionCodeProjects;


        [Test]
        [TestCaseSource(nameof(SolutionCodeProjects))]
        [Description(@"Проект должен иметь файл ресурсов ""Properties/Resources.resx""")]
        public void CodeProjectShouldHaveResources(string project)
        {
            // When
            var hasResources = File.Exists(Path.Combine(project, @"Properties" + Path.DirectorySeparatorChar + "Resources.resx"))
                               && File.Exists(Path.Combine(project, @"Properties" + Path.DirectorySeparatorChar + "Resources.Designer.cs"));

            // Then

            if (!hasResources)
            {
                Assert.Fail($@"Проект ""{project}"" должен иметь файл ресурсов ""Properties/Resources.resx""");
            }
        }

        [Test]
        [TestCaseSource(nameof(SolutionProjects))]
        [Description(@"Проект может ссылаться только на NuGet-пакеты")]
        public void ProjectShouldReferenceOnlyPackages(string project)
        {
            // Given
            var projectXml = LoadProjectXml(project);

            // When
            var assemblyReferences = FindChildren(projectXml, "ItemGroup")
                .SelectMany(e => FindChildren(e, "Reference"))
                .SelectMany(e => FindChildren(e, "HintPath"))
                .Where(e => !e.Value.StartsWith(@"..\packages\"))
                .ToArray();

            // Then

            if (assemblyReferences.Length > 0)
            {
                Assert.Fail($@"Проект ""{project}"" может ссылаться только на NuGet-пакеты");
            }
        }

        [Test]
        [TestCaseSource(nameof(SolutionProjects))]
        [Description(@"Проект может ссылаться только на проекты решения")]
        public void ProjectShouldReferenceOnlySolutionProjects(string project)
        {
            // Given
            var projectXml = LoadProjectXml(project);

            // When

            var projectReferences = FindChildren(projectXml, "ItemGroup")
                .SelectMany(e => FindChildren(e, "ProjectReference"))
                .Select(i => i.Attribute("Include").Value);

            var externalProjectReferences = projectReferences
                .Where(i => !GetFullPathFromRelative(project, i).StartsWith(SolutionDir))
                .ToArray();

            // Then

            if (externalProjectReferences.Length > 0)
            {
                Assert.Fail($@"Проект ""{project}"" может ссылаться только на проекты решения ""{SolutionName}""");
            }
        }

        [Test]
        [Description(@"Все проекты должны использовать пакеты одной версии")]
        public void AllProjectsShouldUsedSamePackages()
        {
            // Given

            var usedPackages = SolutionProjects
                .SelectMany(project => GetPackageReferences(LoadProjectXml(project))
                                .Select(i => new
                                             {
                                                 Project = project,
                                                 Package = i.Item1,
                                                 Version = i.Item2
                                             }))
                .ToArray();

            // When

            var packageVersions = usedPackages
                .GroupBy(p => p.Package,
                         (p, groups) => new
                                        {
                                            Package = p,
                                            Versions = groups.Select(i => i.Version).Distinct().ToArray()
                                        })
                .Where(p => p.Versions.Length > 1)
                .ToArray();

            // Then

            if (packageVersions.Length > 0)
            {
                var errorMessage = string.Join(Environment.NewLine,
                                               packageVersions.Select(p => string.Format("{0}:{1}{2}{1}", p.Package, Environment.NewLine,
                                                                                         string.Join(Environment.NewLine,
                                                                                                     usedPackages.Where(i => i.Package == p.Package)
                                                                                                                 .OrderBy(i => i.Version)
                                                                                                                 .Select(i => $"   {i.Package}.{i.Version} - {i.Project}")))));

                Assert.Fail(@"Все проекты должны использовать пакеты одной версии:{0}{1}", Environment.NewLine, errorMessage);
            }
        }

        [Test]
        [Description(@"Все тесты должны иметь категорию")]
        public void AllTestsShouldHaveCategory()
        {
            // Given
            var testAssemblies = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.Tests.dll");

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
                .ToArray();

            // Then

            if (withoutCategory.Length > 0)
            {
                Assert.Fail(@"Все тесты должны иметь категорию:{0}{1}", Environment.NewLine, string.Join(Environment.NewLine, withoutCategory));
            }
        }

        [Test]
        [Description(@"Все файлы *.cs должны быть в кодировке UTF-8")]
        public void AllFilesShouldBeEncodedInUtf8()
        {
            // Given
            var result = new List<string>();
            var objPath = Path.DirectorySeparatorChar + "obj" + Path.DirectorySeparatorChar;
            var codeFiles = Directory.GetFiles(SolutionDir, "*.cs", SearchOption.AllDirectories).Where(f => !f.Contains(objPath));

            // When

            foreach (var codeFile in codeFiles)
            {
                using (var codeFileStream = new FileStream(codeFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var byteOrderMark = new byte[4];
                    codeFileStream.Read(byteOrderMark, 0, 4);

                    // Если файл представлен не UTF-8
                    if ((byteOrderMark[0] == 0xEF && byteOrderMark[1] == 0xBB && byteOrderMark[2] == 0xBF) == false)
                    {
                        result.Add(codeFile);
                    }
                }
            }

            // Then

            if (result.Count != 0)
            {
                Assert.Fail($@"Все файлы *.cs должны быть в кодировке UTF-8: {string.Join("; ", result)}");
            }
        }


        private static XElement LoadProjectXml(string project)
        {
            return XDocument.Load(Path.Combine(project, $"{Path.GetFileName(project)}.csproj")).Root;
        }

        private static IEnumerable<XElement> FindChildren(XElement parent, string childName)
        {
            return parent.Elements().Where(i => string.Equals(i.Name.LocalName, childName, StringComparison.Ordinal));
        }

        private static IEnumerable<Tuple<string, string>> GetPackageReferences(XElement projectXml)
        {
            var packages = FindChildren(projectXml, "ItemGroup")
                .SelectMany(e => FindChildren(e, "PackageReference"))
                .Select(e => Tuple.Create(e.Attribute("Include").Value, e.Attribute("Version").Value))
                .ToArray();

            return packages;
        }

        private static string GetFullPathFromRelative(string basePath, string relativePath)
        {
            var absolutePath = Path.Combine(basePath, relativePath);

            return Path.GetFullPath(new Uri(absolutePath).LocalPath);
        }
    }
}