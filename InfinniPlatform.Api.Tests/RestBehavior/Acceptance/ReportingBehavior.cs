using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Reporting;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Api.SearchOptions.Builders;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.Sdk.Application.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    [Ignore]
    public sealed class ReportingBehavior
    {
        private IDisposable _server;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            var hostingConfig = new HostingConfig
                {
                    ServerName = "10.0.0.12"
                };

            _server = TestApi.StartServer(c => c.SetHostingConfig(hostingConfig));

            TestApi.InitClientRouting(hostingConfig);
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _server.Dispose();
        }

        private void CreateTestConfig()
        {
        }

        [Test]
        public void ShouldGetPrintViewPdf()
        {
            IEnumerable<object> filter =
                new FilterBuilder().AddCriteria(cr => cr.IsNotContains("123").Property("TestProperty")).GetFilter();

            var queryParam = new
                {
                    ConfigId = "Ehr",
                    DocumentId = "Header",
                    PrintViewId = "TestPrintView",
                    PrintViewType = "ListView",
                    PageNumber = 0,
                    PageSize = 100,
                    Query = filter
                };

            RestQueryResponse response = RestQueryApi.QueryPostUrlEncodedData("SystemConfig", "Reporting",
                                                                              "GetPrintView", queryParam);
            Assert.IsNotNull(response);
        }

        [Test]
        public void ShouldGetReportPdf()
        {
            dynamic instance = new DynamicWrapper();
            instance.Name = "HospitalizationId";
            instance.Value = "4427715e-1f73-4077-a58b-9be70c502287";


            var queryParam = new
                {
                    FileFormat = ReportFileFormat.Pdf,
                    Parameters = new[] {instance},
                    Configuration = "EmergencyRoom",
                    Template = "MedicalHistoryReport"
                };

            RestQueryResponse response = RestQueryApi.QueryPostUrlEncodedData("SystemConfig", "Reporting", "GetReport",
                                                                              queryParam);

            Assert.IsNotNull(response);
            Console.WriteLine(response.Content);
        }
    }
}