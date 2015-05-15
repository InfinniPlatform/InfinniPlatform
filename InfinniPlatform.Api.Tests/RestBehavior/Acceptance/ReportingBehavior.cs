using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Reporting;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.SearchOptions.Builders;
using InfinniPlatform.Api.TestEnvironment;
using NUnit.Framework;
using Newtonsoft.Json;

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
			var hostingConfig = new HostingConfig()
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

			var response = RestQueryApi.QueryPostUrlEncodedData("SystemConfig","Reporting","GetReport", queryParam);

			Assert.IsNotNull(response);
			Console.WriteLine(response.Content);
		}

		[Test]
		public void ShouldGetPrintViewPdf()
		{

			var filter =
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

			var response = RestQueryApi.QueryPostUrlEncodedData("SystemConfig", "Reporting", "GetPrintView", queryParam);
			Assert.IsNotNull(response);
		}

		private void CreateTestConfig()
		{
			
		}
	}
}
