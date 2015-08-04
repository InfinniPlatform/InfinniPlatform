using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Hosting;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    class SolutionVersionBehavior
    {
        private IDisposable _server;

        private const string TestConfig1 = "TestConfigVersion1";
        private const string TestConfig2 = "TestConfigVersion2";
        private const string TestConfig3 = "TestConfigVersion3";

        private const string TestConfig1DocumentId = "TestDoc1";
        private const string TestConfig2DocumentId = "TestDoc2";
        private const string TestConfig3DocumentId = "TestDoc3";

        private const string SolutionId1 = "TestSolution1";
        private const string SolutionId2 = "TestSolution2";
        private const string SolutionId3 = "TestSolution3";
        private const string Version = "1.0.0.0";


        [SetUp]
        public void TestSetup()
        {
            _server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig));

            TestApi.InitClientRouting(TestSettings.DefaultHostingConfig);

            DeleteConfigVersion();

            _server.Dispose();

            _server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig));
        }

        [TearDown]
        public void FixtureTearDown()
        {
            _server.Dispose();
        }

        /// <summary>
        /// При обновлении решения должны обновляться все связанные с ним версии конфигураций
        /// Обновляем приложение с тремя конфигурациями и проверяем сохранение документов, 
        /// связанных с каждой из указанных в решении конфигураций 
        /// </summary>
        [Test]
        public void ShouldUploadEntireSolution()
        {
            //Given
            

            new IndexApi().RebuildIndex(TestConfig1, TestConfig1DocumentId);
            new IndexApi().RebuildIndex(TestConfig2, TestConfig2DocumentId);
            new IndexApi().RebuildIndex(TestConfig3, TestConfig3DocumentId);

            CreateTestSolutionWithThreeConfigs(new[]{"1.1.1.1","1.1.1.2","1.1.1.3"});

            dynamic testDoc = new
            {
                Name = "TestDocument"
            };

            var api = new DocumentApi();

            //When
            api.SetDocument(TestConfig1, TestConfig1DocumentId, testDoc);
            api.SetDocument(TestConfig2, TestConfig2DocumentId, testDoc);
            api.SetDocument(TestConfig3, TestConfig3DocumentId, testDoc);

            //Then
            //проверяем, что были созданы документы разных версий ActionUnit
            dynamic checkDoc1 = api.GetDocument(TestConfig1, TestConfig1DocumentId,
                                                  f =>
                                                  f.AddCriteria(cr => cr.Property("Name").IsEquals("Name_TestAction")),
                                                  0, 1).FirstOrDefault();
            dynamic checkDoc2 = api.GetDocument(TestConfig2, TestConfig2DocumentId,
                                                  f =>
                                                  f.AddCriteria(cr => cr.Property("Name").IsEquals("Name_TestAction_v1")),
                                                  0, 1).FirstOrDefault();
            dynamic checkDoc3 = api.GetDocument(TestConfig3, TestConfig3DocumentId,
                                                  f =>
                                                  f.AddCriteria(cr => cr.Property("Name").IsEquals("Name_TestAction_v2")),
                                                  0, 1).FirstOrDefault();

            Assert.IsNotNull(checkDoc1);
            Assert.IsNotNull(checkDoc2);
            Assert.IsNotNull(checkDoc3);
        }

        /// <summary>
        ///  При существовании трех разных версий конфигурации должен использовать 
        ///  ту мажорную версию, в которую первый раз залогинился
        /// </summary>
        [Test]
        public void ShouldSelectAccordingMinorVersion()
        {
            //Given
            dynamic testDoc = new
            {
                Name = "TestDocument"
            };

            var api = new DocumentApi();

            new IndexApi().RebuildIndex(TestConfig1, TestConfig1DocumentId);

            new SignInApi().SignInInternal("Admin", "Admin", false);

            //When
            CreateAndUpdateTestSolutionThreeTimes(Version, new[] {"1.0.1.0", "2.0.2.0", "3.0.4.0"},
                                                  () =>
                                                      {
                                                          api.SetDocument(TestConfig1, TestConfig1DocumentId, testDoc);
                                                      },
                                                  () => api.SetDocument(TestConfig1, TestConfig1DocumentId, testDoc),
                                                  () => api.SetDocument(TestConfig1, TestConfig1DocumentId, testDoc),
                                                  () =>
                                                      {
                                                          //Then
                                                          //проверяем, что была выбрана соответствующая минорная версия конфигурации при сохранении документов
                                                          //при создании документов должна была быть использована конфигурация с версей 1.0.1, как первая
                                                          //в которую залогинился пользователь
                                                          dynamic checkDoc1 = api.GetDocument(TestConfig1, TestConfig1DocumentId,
                                                                                    f =>
                                                                                    f.AddCriteria(cr => cr.Property("Name").IsEquals("Name_TestAction")),
                                                                                    0, 3).ToList();

                                                          dynamic checkDoc2 = api.GetDocument(TestConfig1, TestConfig1DocumentId,
                                                                                                f =>
                                                                                                f.AddCriteria(cr => cr.Property("Name").IsEquals("Name_TestAction_v1")),
                                                                                                0, 1).FirstOrDefault();
                                                          dynamic checkDoc3 = api.GetDocument(TestConfig1, TestConfig1DocumentId,
                                                                                                f =>
                                                                                                f.AddCriteria(cr => cr.Property("Name").IsEquals("Name_TestAction_v2")),
                                                                                                0, 1).FirstOrDefault();
                                                          
                                                          

                                                          Assert.AreEqual(checkDoc1.Count,3);
                                                          Assert.IsNull(checkDoc2);
                                                          Assert.IsNull(checkDoc3);

                                                          var irrelevantVersions =
                                                              RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata",
                                                                                            "getirrelevantversions",
                                                                                            null, new
                                                                                            {
                                                                                                UserName = "Admin"
                                                                                            }).ToDynamicList();

                                                          Assert.AreEqual(1, irrelevantVersions.Count());
                                                          Assert.AreEqual(irrelevantVersions.First().ConfigurationId, TestConfig1);
                                                          Assert.AreEqual(irrelevantVersions.First().Version, "1.0.1.0");
                                                          Assert.AreEqual(irrelevantVersions.First().ActualVersion, "3.0.0.0");

                                                          RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata",
                                                                                        "setrelevantversion", null, new
                                                                                            {
                                                                                                ConfigurationId =
                                                                                                                        TestConfig1,
                                                                                                Version = "3.0.0.0",
                                                                                                UserName = "Admin"
                                                                                            });

                                                          irrelevantVersions = RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata",
                                                                                          "getirrelevantversions",
                                                                                          null, null).ToDynamicList();

                                                          Assert.AreEqual(0, irrelevantVersions.Count());

                                                          //сохраняем еще один документ 
                                                          api.SetDocument(TestConfig1, TestConfig1DocumentId, testDoc);

                                                          //убеждаемся, что была использована конфигурация версии 3.0.0.0
                                                          checkDoc3 = api.GetDocument(TestConfig1, TestConfig1DocumentId,
                                                              f =>
                                                              f.AddCriteria(cr => cr.Property("Name").IsEquals("Name_TestAction_v2")),
                                                              0, 1).FirstOrDefault();

                                                          Assert.IsNotNull(checkDoc3);
                                                      });            
        }

        /// <summary>
        ///   Создание трех одинаковых конфигураций разных версий
        ///  Последовательно 3 раза обновляем Solution 
        ///  и после этого каждый раз сохраняем документ. 
        ///  Должно создаться 3 разных версии документов
        /// </summary>
        [Test]
        public void ShouldGetMinorVersionByMain()
        {
            //Given

            dynamic testDoc = new
            {
                Name = "TestDocument"
            };

            var api = new DocumentApi();

            new IndexApi().RebuildIndex(TestConfig1, TestConfig1DocumentId);

            //When
            CreateAndUpdateTestSolutionThreeTimes(Version, new[] { "1.0.1.0", "2.0.2.0","3.0.4.0" },
                () => api.SetDocument(TestConfig1, TestConfig1DocumentId, testDoc),
                () => api.SetDocument(TestConfig1, TestConfig1DocumentId, testDoc),
                () => api.SetDocument(TestConfig1, TestConfig1DocumentId, testDoc),
                () =>
                    {
                        //Then
                        //проверяем, что была выбрана соответствующая минорная версия конфигурации при сохранении документов
                        dynamic checkDoc1 = api.GetDocument(TestConfig1, TestConfig1DocumentId,
                                                  f =>
                                                  f.AddCriteria(cr => cr.Property("Name").IsEquals("Name_TestAction")),
                                                  0, 1).FirstOrDefault();

                        dynamic checkDoc2 = api.GetDocument(TestConfig1, TestConfig1DocumentId,
                                                              f =>
                                                              f.AddCriteria(cr => cr.Property("Name").IsEquals("Name_TestAction_v1")),
                                                              0, 1).FirstOrDefault();
                        dynamic checkDoc3 = api.GetDocument(TestConfig1, TestConfig1DocumentId,
                                                              f =>
                                                              f.AddCriteria(cr => cr.Property("Name").IsEquals("Name_TestAction_v2")),
                                                              0, 1).FirstOrDefault();

                        Assert.IsNotNull(checkDoc1);
                        Assert.IsNotNull(checkDoc2);
                        Assert.IsNotNull(checkDoc3);
                    }
                );                                
        }

        /// <summary>
        ///  Три раза обновляем конфигурацию, после чего сохраняем документ
        ///  Должен создаться документ, соответствующий третьей версии конфигурации
        /// </summary>
        [Test]
        public void ShouldGetLastVersionOfConfiguration()
        {
            //Given

            CreateTestSolutionForSomeVersionsOfOneConfig(Version, new[] { "1.0.1.0", "1.0.2.0", "1.0.5.0" });
            
            new IndexApi().RebuildIndex(TestConfig1, TestConfig1DocumentId);

            dynamic testDoc = new
            {
                Name = "TestDocument"
            };

            var apiV1 = new DocumentApi();

            //When
            apiV1.SetDocument(TestConfig1, TestConfig1DocumentId, testDoc);
            //Then

            //проверяем, что была выбрана соответствующая минорная версия конфигурации при сохранении документов
            dynamic checkDoc1 = apiV1.GetDocument(TestConfig1, TestConfig1DocumentId,
                                                  f =>
                                                  f.AddCriteria(cr => cr.Property("Name").IsEquals("Name_TestAction_v2")),
                                                  0, 1).FirstOrDefault();

            Assert.IsNotNull(checkDoc1);
        }


        private dynamic CreateTestConfig(string configurationId, string documentId, string version, string scenarioId)
        {
            MetadataManagerConfiguration managerConfiguration =
                ManagerFactoryConfiguration.BuildConfigurationManager(version); //указана версия конфигурации

            dynamic config = managerConfiguration.CreateItem(configurationId);
            managerConfiguration.DeleteItem(config);
            managerConfiguration.MergeItem(config);

            MetadataManagerDocument managerDocument =
                new ManagerFactoryConfiguration(version, configurationId).BuildDocumentManager();
            dynamic documentMetadata1 = managerDocument.CreateItem(documentId);

            documentMetadata1.Schema = new DynamicWrapper();
            documentMetadata1.Schema.Type = documentId;
            documentMetadata1.Schema.Caption = documentId;
            documentMetadata1.Schema.Properties = new DynamicWrapper();
            documentMetadata1.Schema.Properties.Name = new DynamicWrapper();
            documentMetadata1.Schema.Properties.Name.Type = "String";
            documentMetadata1.Schema.Properties.Name.Caption = "Patient name";

            managerDocument.MergeItem(documentMetadata1);

            //добавляем бизнес-процесс по умолчанию
            MetadataManagerElement processManager =
                new ManagerFactoryDocument(version, configurationId, documentId).BuildProcessManager();
            dynamic defaultProcess = processManager.CreateItem("Default");

            dynamic instance = new DynamicWrapper();
            instance.Name = "Default transition";
            defaultProcess.Type = WorkflowTypes.WithoutState;
            defaultProcess.Transitions = new List<dynamic>();
            defaultProcess.Transitions.Add(instance);

            instance.SuccessPoint = new DynamicWrapper();
            instance.SuccessPoint.TypeName = "Action";
            instance.SuccessPoint.ScenarioId = scenarioId;

            processManager.MergeItem(defaultProcess);

            MetadataManagerElement scenarioManager =
                new ManagerFactoryDocument(version, configurationId, documentId).BuildScenarioManager();

            dynamic scenarioItem = scenarioManager.CreateItem(scenarioId);
            scenarioItem.ScenarioId = scenarioId;
            scenarioItem.Id = scenarioId;
            scenarioItem.Name = scenarioId;
            scenarioItem.ScriptUnitType = ScriptUnitType.Action;
            scenarioItem.ContextType = ContextTypeKind.ApplyMove;
            scenarioManager.MergeItem(scenarioItem);

            MetadataManagerElement assemblyManager =
                new ManagerFactoryConfiguration(version, configurationId).BuildAssemblyManager();
            dynamic assemblyItem = assemblyManager.CreateItem("InfinniPlatform.Api.Tests");
            assemblyManager.MergeItem(assemblyItem);

            return config;
        }

        private void CreateTestSolutionWithThreeConfigs(string[] referencedConfigVersions)
        {
            MetadataManagerSolution managerSolution = ManagerFactorySolution.BuildSolutionManager(Version);
            var refConfigs = new List<dynamic>();
            var packages = new List<dynamic>();
            dynamic solution = managerSolution.CreateItem(SolutionId1);
            solution.ReferencedConfigurations = refConfigs;

            var refConfig1 = CreateTestConfig(TestConfig1, TestConfig1DocumentId, referencedConfigVersions[0], "TestAction");
            var packageConfig1 = new PackageBuilder().BuildPackage(TestConfig1, referencedConfigVersions[0], GetType().Assembly.Location);

            refConfigs.Add(refConfig1);
            packages.Add(packageConfig1);

            var refConfig2 = CreateTestConfig(TestConfig2, TestConfig2DocumentId, referencedConfigVersions[1], "TestAction_v1");
            var packageConfig2 = new PackageBuilder().BuildPackage(TestConfig2, referencedConfigVersions[1], GetType().Assembly.Location);

            refConfigs.Add(refConfig2);
            packages.Add(packageConfig2);

            var refConfig3 = CreateTestConfig(TestConfig3, TestConfig3DocumentId, referencedConfigVersions[2], "TestAction_v2");
            var packageConfig3 = new PackageBuilder().BuildPackage(TestConfig3, referencedConfigVersions[2], GetType().Assembly.Location);

            refConfigs.Add(refConfig3);
            packages.Add(packageConfig3);

            managerSolution.DeleteItem(solution);
            managerSolution.InsertItem(solution);

            new SolutionUpdateApi(SolutionId1, Version).InstallPackages(packages);

        }

        private void DeleteConfigVersion()
        {
            var reader = ManagerFactoryConfiguration.BuildConfigurationMetadataReader(null,true);
            foreach (var item in reader.GetItems())
            {
                if (((string) item.Name).Contains("TestConfig"))
                {
                    MetadataManagerConfiguration manager =
                        ManagerFactoryConfiguration.BuildConfigurationManager(item.Version);
                    manager.DeleteItem(item);
                }
            }

            new SignInApi().SignInInternal("Admin", "Admin", false);
            var docApi = new DocumentApi();
            var items = docApi.GetDocument(AuthorizationStorageExtensions.AuthorizationConfigId,
                                                      AuthorizationStorageExtensions.VersionStore, null, 0, 1000);
            foreach (var item in items)
            {
                docApi.DeleteDocument(AuthorizationStorageExtensions.AuthorizationConfigId,
                                      AuthorizationStorageExtensions.VersionStore, item.Id);
            }
            new SignInApi().SignOutInternal();
        }

        private void CreateTestSolutionForSomeVersionsOfOneConfig(string version, string[] referencedConfigVersions)
        {
            MetadataManagerSolution managerSolution = ManagerFactorySolution.BuildSolutionManager(version);


            dynamic solution = managerSolution.CreateItem(SolutionId3);

            var refConfig1 = CreateTestConfig(TestConfig1, TestConfig1DocumentId, referencedConfigVersions[0], "TestAction");
            var packageConfig1 = new PackageBuilder().BuildPackage(TestConfig1, referencedConfigVersions[0], GetType().Assembly.Location);

            var refConfig2 = CreateTestConfig(TestConfig1, TestConfig1DocumentId, referencedConfigVersions[1], "TestAction_v1");
            var packageConfig2 = new PackageBuilder().BuildPackage(TestConfig1, referencedConfigVersions[1], GetType().Assembly.Location);

            var refConfig3 = CreateTestConfig(TestConfig1, TestConfig1DocumentId, referencedConfigVersions[2], "TestAction_v2");
            var packageConfig3 = new PackageBuilder().BuildPackage(TestConfig1, referencedConfigVersions[2], GetType().Assembly.Location);

            var packages = new[] { packageConfig1, packageConfig2, packageConfig3 };

            var refConfigs = new List<dynamic>();

            solution.ReferencedConfigurations = refConfigs;
            refConfigs.Add(refConfig1);
            refConfigs.Add(refConfig2);
            refConfigs.Add(refConfig3);

            managerSolution.DeleteItem(solution);
            managerSolution.InsertItem(solution);

            new SolutionUpdateApi(SolutionId3, Version).InstallPackages(packages);
        }


        private void CreateAndUpdateTestSolutionThreeTimes(string version, string[] referencedConfigVersions, Action onCreateFirstVersionOfSolution, 
            Action onCreateSecondVersionOfSolution, Action onCreateThirdVersionOfSolution, Action assertResults)
        {
            MetadataManagerSolution managerSolution = ManagerFactorySolution.BuildSolutionManager(version);
            var refConfigs = new List<dynamic>();
            dynamic solution = managerSolution.CreateItem(SolutionId2);
            solution.ReferencedConfigurations = refConfigs;


            var refConfig1 = CreateTestConfig(TestConfig1, TestConfig1DocumentId, referencedConfigVersions[0], "TestAction");
            var packageConfig1 = new PackageBuilder().BuildPackage(TestConfig1, referencedConfigVersions[0], GetType().Assembly.Location);                       
            refConfigs.Add(refConfig1);

            var packages = new List<dynamic>() { packageConfig1};

            managerSolution.DeleteItem(solution);
            managerSolution.InsertItem(solution);

            new SolutionUpdateApi(SolutionId2, Version).InstallPackages(packages);

            onCreateFirstVersionOfSolution();

            refConfigs.Clear();
            packages.Clear();

            var refConfig2 = CreateTestConfig(TestConfig1, TestConfig1DocumentId, referencedConfigVersions[1], "TestAction_v1");
            var packageConfig2 = new PackageBuilder().BuildPackage(TestConfig1, referencedConfigVersions[1], GetType().Assembly.Location);

            refConfigs.Add(refConfig2);
            packages.Add(packageConfig2);
            solution.ReferencedConfigurations = refConfigs;

            managerSolution.DeleteItem(solution);
            managerSolution.InsertItem(solution);

            new SolutionUpdateApi(SolutionId2, Version).InstallPackages(packages);

            onCreateSecondVersionOfSolution();

            refConfigs.Clear();
            packages.Clear();

            var refConfig3 = CreateTestConfig(TestConfig1, TestConfig1DocumentId, referencedConfigVersions[2], "TestAction_v2");
            var packageConfig3 = new PackageBuilder().BuildPackage(TestConfig1, referencedConfigVersions[2], GetType().Assembly.Location);

            refConfigs.Add(refConfig3);
            packages.Add(packageConfig3);

            solution.ReferencedConfigurations = refConfigs;

            managerSolution.DeleteItem(solution);
            managerSolution.InsertItem(solution);

            new SolutionUpdateApi(SolutionId2, Version).InstallPackages(packages);

            onCreateThirdVersionOfSolution();

            assertResults();
        }
    }
}
