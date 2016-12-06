using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace InfinniPlatform.Conventions
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class StructurePlatformConventionsTest
    {
        static StructurePlatformConventionsTest()
        {
            SolutionDir = Path.GetDirectoryName(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath)) ?? ".";
            PackagesDir = ".." + Path.DirectorySeparatorChar + "packages" + Path.DirectorySeparatorChar;

            SolutionProjects = Directory.GetDirectories(SolutionDir, $"{SolutionName}.*").ToArray();
            SolutionCodeProjects = SolutionProjects.Where(p => !p.EndsWith(".Tests")).ToArray();
            SolutionTestProjects = SolutionProjects.Where(p => p.EndsWith(".Tests")).ToArray();

            ProjectNamespace = XNamespace.Get("http://schemas.microsoft.com/developer/msbuild/2003");

            Console.WriteLine(@"SolutionDir={0}", SolutionDir);
        }


        private const string SolutionName = "InfinniPlatform";
        private const string SolutionOutDir = "Assemblies";

        private static readonly string SolutionDir;
        private static readonly string PackagesDir;
        private static readonly string[] SolutionProjects;
        private static readonly string[] SolutionCodeProjects;
        private static readonly string[] SolutionTestProjects;
        private static readonly XNamespace ProjectNamespace;


        [Test]
        [TestCaseSource(nameof(SolutionCodeProjects))]
        [Description(@"Проект должен иметь тестовый проект")]
        public void CodeProjectShouldHaveTestProject(string project)
        {
            // Given
            var expectedTestProject = project + ".Tests";

            // When
            var result = project.EndsWith("InfinniPlatform.ServiceHost")
                         || project.EndsWith("InfinniPlatform.Auth.Adfs")
                         || project.EndsWith("InfinniPlatform.Auth.Cookie")
                         || project.EndsWith("InfinniPlatform.Auth.Facebook")
                         || project.EndsWith("InfinniPlatform.Auth.Google")
                         || project.EndsWith("InfinniPlatform.Auth.Vk")
                         || project.EndsWith("InfinniPlatform.Watcher")
                         || project.EndsWith("Infinni.Agent")
                         || project.EndsWith("InfinniPlatform.Server")
                         || project.EndsWith("InfinniPlatform.Heartbeat")
                         || SolutionTestProjects.Any(testProject => string.Equals(testProject, expectedTestProject, StringComparison.InvariantCultureIgnoreCase));

            // Then
            Assert.IsTrue(result, @"Проект ""{0}"" должен иметь тестовый проект", project);
        }

        [Test]
        [TestCaseSource(nameof(SolutionProjects))]
        [Description(@"Проект должен иметь файл ""packages.config""")]
        public void ProjectShouldHavePackages(string project)
        {
            // When
            var result = File.Exists(Path.Combine(project, "packages.config"));

            // Then
            Assert.IsTrue(result, @"Проект ""{0}"" должен иметь файл ""packages.config""", project);
        }

        [Test]
        [TestCaseSource(nameof(SolutionCodeProjects))]
        [Description(@"Проект должен иметь файл ресурсов ""Properties\Resources""")]
        public void CodeProjectShouldHaveResources(string project)
        {
            // When
            var result = File.Exists(Path.Combine(project, @"Properties" + Path.DirectorySeparatorChar + "Resources.resx"))
                         && File.Exists(Path.Combine(project, @"Properties" + Path.DirectorySeparatorChar + "Resources.Designer.cs"));

            // Then
            Assert.IsTrue(result, @"Проект ""{0}"" должен иметь файл ресурсов ""Properties\Resources""", project);
        }

        [Test]
        [TestCaseSource(nameof(SolutionProjects))]
        [Description(@"Проект должен компилироваться в один каталог (в Debug и Release)")]
        public void ProjectShouldHaveCommonOutputPath(string project)
        {
            // Given
            var coreOutPath = $@"..\{SolutionOutDir}\";

            // When
            var result = LoadProject(project)
                .Elements(ProjectNamespace + "PropertyGroup")
                .Where(i => i.Attribute("Condition") != null)
                .Select(i => ToNormPath(i.Element(ProjectNamespace + "OutputPath")?.Value))
                .All(i => i == coreOutPath);

            // Then
            Assert.IsTrue(result, @"Проект ""{0}"" должен компилироваться в один каталог ""{1}"" (в Debug и Release)", project, coreOutPath);
        }

        [Test]
        [TestCaseSource(nameof(SolutionProjects))]
        [Description(@"Проект может ссылаться только на ""C:\Program Files"" или ""..\packages\""")]
        public void ProjectShouldReferenceOnlyPackages(string project)
        {
            // When
            var result = LoadProject(project)
                .Elements(ProjectNamespace + "ItemGroup")
                .Elements(ProjectNamespace + "Reference")
                .Elements(ProjectNamespace + "HintPath")
                .All(i => i == null
                    || string.IsNullOrWhiteSpace(i.Value)
                    || ToNormPath(i.Value).StartsWith(@"C:\Program Files")
                    || ToNormPath(i.Value).StartsWith(ToNormPath(PackagesDir)));

            // Then
            Assert.IsTrue(result, @"Проект ""{0}"" может ссылаться только на ""C:\Program Files"" или ""..\packages\""", project);
        }

        [Test]
        [TestCaseSource(nameof(SolutionProjects))]
        [Description(@"Проект может ссылаться только на проекты решения")]
        public void ProjectShouldReferenceOnlySolutionProjects(string project)
        {
            // When
            var result = LoadProject(project)
                .Elements(ProjectNamespace + "ItemGroup")
                .Elements(ProjectNamespace + "ProjectReference")
                .Select(i => i.Attribute("Include"))
                .All(i => i != null
                    && string.IsNullOrWhiteSpace(i.Value) == false
                    && i.Value.StartsWith(@"..\")
                    && ToNormPath(i.Value.Replace(@"..\", SolutionDir + @"\"))
                        .StartsWith(ToNormPath(SolutionDir)));

            // Then
            Assert.IsTrue(result, @"Проект ""{0}"" может ссылаться только на проекты решения ""{1}""", project, SolutionName);
        }

        [Test]
        [TestCaseSource(nameof(SolutionProjects))]
        [Description(@"Проект должен иметь файл ""packages.config"" со всеми используемыми пакетами")]
        public void PackageReferencesShouldBeInPackageConfig(string project)
        {
            // Given

            var packageReferences = LoadProject(project)
                .Elements(ProjectNamespace + "ItemGroup")
                .Elements(ProjectNamespace + "Reference")
                .Elements(ProjectNamespace + "HintPath")
                .Where(i => i != null
                            && string.IsNullOrWhiteSpace(i.Value) == false
                            && i.Value.StartsWith(PackagesDir))
                .Select(i => i.Value)
                .ToArray();

            var packageConfig = LoadPackageConfig(project)
                .Elements("package")
                .Select(i => string.Format(PackagesDir + "{0}.{1}" + Path.DirectorySeparatorChar, i.Attribute("id").Value, i.Attribute("version").Value))
                .ToArray();

            // When
            var result = packageReferences.All(pr => packageConfig.Any(pr.StartsWith));

            // Then
            Assert.IsTrue(result, @"Проект ""{0}"" должен иметь файл ""packages.config"" со всеми используемыми пакетами: {1}{2}", project, Environment.NewLine,
                          string.Join(Environment.NewLine, packageReferences.Where(pr => !packageConfig.Any(pr.StartsWith))));
        }

        [Test]
        [TestCaseSource(nameof(SolutionProjects))]
        [Description(@"Проект должен иметь файл ""packages.config"" только с используемыми пакетами")]
        public void PackageConfigShouldContainsOnlyUsedPackageReferences(string project)
        {
            // TODO: Новые пакеты Microsoft, используемые в InfinniPlatform.Expressions, не добавляют references.
            if (project.Contains("InfinniPlatform.PrintView"))
            {
                return;
            }

            // Given
            var packageReferences = LoadProject(project)
                .Elements(ProjectNamespace + "ItemGroup")
                .Elements(ProjectNamespace + "Reference")
                .Elements(ProjectNamespace + "HintPath")
                .Where(i => i != null
                            && string.IsNullOrWhiteSpace(i.Value) == false
                            && i.Value.StartsWith(ToNormPath(PackagesDir)))
                .Select(i => i.Value)
                .ToArray();

            var packageConfig = LoadPackageConfig(project)
                .Elements("package")
                .Where(i => i.Attribute("id").Value.Contains("OwinSelfHost") == false)
                .Select(i => ToNormPath(string.Format(PackagesDir + "{0}.{1}" + Path.DirectorySeparatorChar, i.Attribute("id").Value, i.Attribute("version").Value)))
                .ToArray();

            // When
            var result = packageConfig.All(pc => packageReferences.Any(pr => pr.StartsWith(pc)));

            // Then
            Assert.IsTrue(result, @"Проект ""{0}"" должен иметь файл ""packages.config"" только с используемыми пакетами:{1}{2}", project, Environment.NewLine,
                          string.Join(Environment.NewLine, packageConfig.Where(pc => !packageReferences.Any(pr => pr.StartsWith(pc)))));
        }

        [Test]
        [Description(@"Все проекты должны использовать пакеты одной версии")]
        public void AllProjectsShouldUsedSamePackages()
        {
            // Given

            var usedPackages = SolutionProjects
                .SelectMany(project =>
                            LoadPackageConfig(project)
                                .Elements("package")
                                .Select(i => new
                                {
                                    Project = project,
                                    Package = i.Attribute("id").Value,
                                    Version = i.Attribute("version").Value
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
            var testAssemblies = Directory.GetFiles(Path.Combine(SolutionDir, SolutionOutDir), "*.Tests.dll");

            // When
            var withoutCategory = testAssemblies
                .SelectMany(a => Assembly.LoadFrom(a)
                                         .GetTypes()
                                         .Where(t =>
                                                {
                                                    if (t.IsClass && !t.IsAbstract)
                                                    {
                                                        var ignoreAttr = t.GetCustomAttribute<IgnoreAttribute>();

                                                        if (ignoreAttr != null)
                                                        {
                                                            return false;
                                                        }

                                                        var testFixtureAttr = t.GetCustomAttribute<TestFixtureAttribute>();

                                                        if (testFixtureAttr != null)
                                                        {
                                                            if (!string.IsNullOrEmpty(testFixtureAttr.Category))
                                                            {
                                                                return false;
                                                            }

                                                            var categoryAttr = t.GetCustomAttribute<CategoryAttribute>();

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
            Assert.IsTrue(result.Count == 0, @"Все файлы *.cs должны быть в кодировке UTF-8: {0}", string.Join("; ", result));
        }

        [Test]
        [TestCaseSource(nameof(SolutionTestProjects))]
        [Description(@"Проект должен ссылаться на общий файл конфигурации для тестов")]
        public void TestProjectShouldReferenceOnCommonAppConfig(string project)
        {
            // Given
            const string appConfig = @"..\Files\Config\App.config";

            // When
            var result = LoadProject(project)
                .Elements(ProjectNamespace + "ItemGroup")
                .SelectMany(e => e.Elements(ProjectNamespace + "None"))
                .Where(e => e.Attribute("Include").Value.Contains("App.config"))
                .Select(e => e.Attribute("Include").Value)
                .FirstOrDefault() ?? appConfig;

            // Then
            Assert.AreEqual(ToNormPath(appConfig), ToNormPath(result), @"Проект ""{0}"" должен ссылаться на общий файл конфигурации для тестов ""{1}""", project, appConfig);
        }

        [Test]
        [TestCaseSource(nameof(SolutionCodeProjects))]
        [Description(@"Проект должен ссылаться на общий файл конфигурации для платформы")]
        public void CodeProjectShouldReferenceOnCommonAppConfig(string project)
        {
            if (project.EndsWith("InfinniPlatform.UserInterface")
                || project.EndsWith("InfinniPlatform.QueryDesigner")
                || project.EndsWith("InfinniPlatform.RestfulApi")
                || project.EndsWith("InfinniPlatform.SystemConfig")
                || project.EndsWith("InfinniPlatform.Update")
                || project.EndsWith("InfinniPlatform.ReportDesigner")
                || project.EndsWith("InfinniPlatform.PrintViewDesigner")
                || project.EndsWith("InfinniPlatform.Utils")
                )
            {
                return;
            }

            // Given
            const string appConfig = @"..\Files\Config\App.config";

            // When
            var result = LoadProject(project)
                .Elements(ProjectNamespace + "ItemGroup")
                .SelectMany(e => e.Elements(ProjectNamespace + "None"))
                .Where(e => e.Attribute("Include").Value.Contains("App.config"))
                .Select(e => e.Attribute("Include").Value)
                .FirstOrDefault() ?? appConfig;

            // Then
            Assert.AreEqual(ToNormPath(appConfig), ToNormPath(result), @"Проект ""{0}"" должен ссылаться на общий файл конфигурации для платформы ""{1}""", project, appConfig);
        }


        private static XElement LoadProject(string project)
        {
            return XDocument.Load(Path.Combine(project, $"{Path.GetFileName(project)}.csproj")).Root;
        }

        private static XElement LoadPackageConfig(string project)
        {
            return XDocument.Load(Path.Combine(project, "packages.config")).Root;
        }


        private static string ToNormPath(string path)
        {
            return path?.Replace('/', '\\');
        }
    }
}