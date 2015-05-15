using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using InfinniPlatform.Api.Schema;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.FastReport.TemplatesFluent.Reports;
using InfinniPlatform.Reporting.DataSources;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace InfinniPlatform.Reporting.Tests.DataSources
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	[Ignore("Manual")]
	public sealed class SqlDataSourceTest
	{
		private const string ConnectionString = "Data Source=.;Initial Catalog=Test;Integrated Security=SSPI";


		[TestFixtureSetUp]
		public void SetUp()
		{
			using (var connection = new SqlConnection("Data Source=.;Initial Catalog=master;Integrated Security=SSPI"))
			{
				using (var command = connection.CreateCommand())
				{
					connection.Open();

					command.CommandText = @"
						if (exists(select * from sys.databases where name = 'Test')) begin
							drop database Test
						end

						create database Test";

					command.ExecuteNonQuery();

					command.CommandText = @"
						use Test

						create table Result (
							Id int,
							Value1 nvarchar(32),
							Value2 nvarchar(32)
						)

						insert into Result (Id, Value1, Value2) values (0, 'Value 10', 'Value 20')
						insert into Result (Id, Value1, Value2) values (1, 'Value 10', 'Value 21')
						insert into Result (Id, Value1, Value2) values (2, 'Value 10', 'Value 22')
						insert into Result (Id, Value1, Value2) values (3, 'Value 10', 'Value 23')
						insert into Result (Id, Value1, Value2) values (4, 'Value 10', 'Value 24')
						insert into Result (Id, Value1, Value2) values (5, 'Value 10', 'Value 25')
						insert into Result (Id, Value1, Value2) values (6, 'Value 11', 'Value 26')";

					command.ExecuteNonQuery();
				}
			}
		}

		[Test]
		public void ShouldSupportRestDataSource()
		{
			// Given

			const int count = 3;
			const string value1 = "Value 10";

			var target = new SqlDataSource();

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
							.Sql(r => r
								.ServerType(SqlServerType.MsSqlServer)
								.ConnectionString(ConnectionString)
								.SelectCommand("select top (@count) Id, Value1, Value2 from Result where Value1 = @value1")))))
				.BuildTemplate();

			var reportParameterValues = new Dictionary<string, object>
				                            {
					                            { "count", count },
					                            { "value1", value1 }
				                            };

			// When

			var data = target.GetData(reportTemplate.DataSources.First(),
									  reportTemplate.Parameters,
									  reportParameterValues);

			// Then

			Assert.IsNotNull(data);
			Assert.AreEqual(count, data.Count);

			for (var i = 0; i < count; ++i)
			{
				var jObject = (JObject)data[i];
				Assert.AreEqual(i, ((JValue)jObject["Id"]).Value);
				Assert.AreEqual(value1, ((JValue)jObject["Value1"]).Value);
			}
		}
	}
}