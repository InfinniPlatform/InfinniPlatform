using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    public class CreateTestSolutionBehavior
    {
        private IDisposable _server;

        [SetUp]
        public void TestSetup()
        {
            _server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig));

            TestApi.InitClientRouting(TestSettings.DefaultHostingConfig);

        }

        [TearDown]
        public void FixtureTearDown()
        {
            _server.Dispose();
        }

        [Test]
        public void ShouldCreate()
        {
            MetadataManagerSolution managerSolution = ManagerFactorySolution.BuildSolutionManager("1.0.0.0");


            dynamic solution = managerSolution.CreateItem("TestSolution");

            var refConfigs = new List<dynamic>();

            solution.ReferencedConfigurations = refConfigs;

            dynamic refConfig1 = new DynamicWrapper();
            refConfig1.Id = "8eb3ff6a-3340-4529-a9f4-0f468bcc4fa4";
            refConfig1.Name = "TestConfig";
            refConfig1.Version = "1.0.0.0";

            refConfigs.Add(refConfig1);

            managerSolution.DeleteItem(solution);
            managerSolution.InsertItem(solution);
        }

        [Test]
        public void ShouldRegisterAdmin()
        {
            var signInApi = new InfinniSignInApi("localhost", "9900");
            signInApi.SignInInternal("Admin", "Admin", false);
            Console.WriteLine("Зарегистрировались под Admin");

            var customServiceApi = new InfinniCustomServiceApi("localhost", "9900");
            customServiceApi.CookieContainer = signInApi.CookieContainer;
            var result = customServiceApi.ExecuteAction("TestConfig", "common", "Helloworld", new {});

            Console.WriteLine(result.ToString());
        }
    }
}
