using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

using InfinniPlatform.Core.Schema;
using InfinniPlatform.FastReport.TemplatesFluent.Reports;
using InfinniPlatform.Reporting.DataSources;
using InfinniPlatform.Sdk.Hosting;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

using Newtonsoft.Json;

namespace InfinniPlatform.Reporting.Tests.DataSources
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	[Ignore("Manual")]
	public sealed class RestDataSourceTest
	{
		private static readonly string BaseAddress = string.Format("{0}://{1}:{2}/", HostingConfig.Default.Scheme, HostingConfig.Default.Name, HostingConfig.Default.Port);


		[Test]
		public void ShouldSupportRestDataSource()
		{
			// Given

			const int count = 5;
			const string value1 = "Value 1";
			const string value2 = "Value 2";

			var target = new RestDataSource();

			var reportTemplate = ReportTemplateFluent.Report("ReportName1")
				.Parameters(x => x
					.Register("count", SchemaDataType.Integer)
					.Register("value1", SchemaDataType.String))
				.DataSources(x => x
					.Register("Result", ds => ds
						.Schema(s => s
							.Property("Id", SchemaDataType.Integer)
							.Property("Value1", SchemaDataType.String)
							.Property("Value2", SchemaDataType.String))
						.Provider(p => p
							.Rest(r => r
								.RequestUri(BaseAddress + "api/RestDataSourceTest/?count={count}&value1={value1}")
								.Body(value2)
								.Method("POST")))))
				.BuildTemplate();

			var reportParameterValues = new Dictionary<string, object>
				                            {
					                            { "count", count },
					                            { "value1", value1 }
				                            };

			// When

			Func<JArray> getData = () => target.GetData(reportTemplate.DataSources.First(),
														reportTemplate.Parameters,
														reportParameterValues);

			var data = RestServerHelper.InvokeServer(BaseAddress, getData);

			// Then

			Assert.IsNotNull(data);
			Assert.AreEqual(count, data.Count);

			for (var i = 0; i < count; ++i)
			{
				Assert.AreEqual(i, ((JValue)data[i]["Id"]).Value);
				Assert.AreEqual(value1, ((JValue)data[i]["Value1"]).Value);
				Assert.AreEqual(value2, ((JValue)data[i]["Value2"]).Value);
			}
		}
	}


	public sealed class RestDataSourceTestController : ApiController
	{
		public HttpResponseMessage Post(int count, string value1)
		{
			var value2 = Request.Content.ReadAsStringAsync().Result;

			var result = new List<object>();

			for (var i = 0; i < count; ++i)
			{
				result.Add(new { Id = i, Value1 = value1, Value2 = value2 });
			}

			return new HttpResponseMessage(HttpStatusCode.OK)
					   {
						   Content = new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8)
					   };
		}
	}
}