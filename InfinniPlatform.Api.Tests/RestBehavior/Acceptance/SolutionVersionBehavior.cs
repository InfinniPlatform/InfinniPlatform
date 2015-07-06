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

        private const string TestConfig1 = "TestConfig1";
        private const string TestConfig2 = "TestConfig2";
        private const string TestConfig3 = "TestConfig3";

        private const string TestConfig1DocumentId = "TestDoc1";
        private const string TestConfig2DocumentId = "TestDoc2";
        private const string TestConfig3DocumentId = "TestDoc3";

        private const string SolutionId = "TestSolution";
        private const string Version = "1.0.0";


        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig));

            TestApi.InitClientRouting(TestSettings.DefaultHostingConfig);
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _server.Dispose();
        }

        [Test]
        public void ShouldUploadEntireSolution()
        {
            ////Given
            //var packages = UpdateTestSolutionThreeTimes(SolutionId, Version,new [] {"1","2","3"});

            //new SolutionUpdateApi(SolutionId, Version).InstallPackages(packages);

            //dynamic testDoc = new
            //{
            //    Name = "TestDocument"
            //};

            //var apiV1 = new DocumentApi("1");
            //var apiV2 = new DocumentApi("2");
            //var apiV3 = new DocumentApi("3");

            ////When
            //apiV1.SetDocument(TestConfig1, TestConfig1DocumentId, testDoc);
            //apiV2.SetDocument(TestConfig2, TestConfig2DocumentId, testDoc);
            //apiV3.SetDocument(TestConfig3, TestConfig3DocumentId, testDoc);

            ////Then
            ////проверяем, что были созданы документы разных версий ActionUnit
            //dynamic checkDoc1 = apiV1.GetDocument(TestConfig1, TestConfig1DocumentId,
            //                                      f =>
            //                                      f.AddCriteria(cr => cr.Property("Name").IsEquals("Name_TestAction")),
            //                                      0, 1).FirstOrDefault();
            //dynamic checkDoc2 = apiV2.GetDocument(TestConfig2, TestConfig2DocumentId,
            //                                      f =>
            //                                      f.AddCriteria(cr => cr.Property("Name").IsEquals("Name_TestAction_v1")),
            //                                      0, 1).FirstOrDefault();
            //dynamic checkDoc3 = apiV3.GetDocument(TestConfig3, TestConfig3DocumentId,
            //                                      f =>
            //                                      f.AddCriteria(cr => cr.Property("Name").IsEquals("Name_TestAction_v2")),
            //                                      0, 1).FirstOrDefault();

            //Assert.IsNotNull(checkDoc1);
            //Assert.IsNotNull(checkDoc2);
            //Assert.IsNotNull(checkDoc3);
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
            UpdateTestSolutionThreeTimes(SolutionId, Version, new[] { "1.01", "2.02","3.04" },
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
            var packages = CreateTestSolutionForSomeVersionsOfOneConfig(SolutionId, Version, new[] { "1.01", "1.02", "1.05" });

            new SolutionUpdateApi(SolutionId, Version).InstallPackages(packages);

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

        private IEnumerable<dynamic> CreateTestSolutionForSomeVersionsOfOneConfig(string solutionId, string version, string[] referencedConfigVersions)
        {
            MetadataManagerSolution managerSolution = ManagerFactorySolution.BuildSolutionManager(version);


            dynamic solution = managerSolution.CreateItem(solutionId);

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

            return packages;
        }


        private void UpdateTestSolutionThreeTimes(string solutionId, string version, string[] referencedConfigVersions, Action onCreateFirstVersionOfSolution, 
            Action onCreateSecondVersionOfSolution, Action onCreateThirdVersionOfSolution, Action assertResults)
        {
            MetadataManagerSolution managerSolution = ManagerFactorySolution.BuildSolutionManager(version);
            var refConfigs = new List<dynamic>();
            dynamic solution = managerSolution.CreateItem(solutionId);
            solution.ReferencedConfigurations = refConfigs;


            var refConfig1 = CreateTestConfig(TestConfig1, TestConfig1DocumentId, referencedConfigVersions[0], "TestAction");
            var packageConfig1 = new PackageBuilder().BuildPackage(TestConfig1, referencedConfigVersions[0], GetType().Assembly.Location);                       
            refConfigs.Add(refConfig1);

            var packages = new List<dynamic>() { packageConfig1};

            managerSolution.DeleteItem(solution);
            managerSolution.InsertItem(solution);

            new SolutionUpdateApi(SolutionId, Version).InstallPackages(packages);

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

            new SolutionUpdateApi(SolutionId, Version).InstallPackages(packages);

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

            new SolutionUpdateApi(SolutionId, Version).InstallPackages(packages);

            onCreateThirdVersionOfSolution();

            assertResults();
        }
    }
}
