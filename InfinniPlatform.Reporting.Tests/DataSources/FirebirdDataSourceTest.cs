using System.Collections.Generic;
using System.IO;
using System.Linq;

using FirebirdSql.Data.FirebirdClient;
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
	public sealed class FirebirdDataSourceTest
	{
		private static readonly string ConnectionString = string.Format("data source=localhost;initial catalog={0};user id=sysdba;password=masterkey;character set=UTF8;dialect=3", Path.GetFullPath("TESTDB.FDB"));


		[TestFixtureSetUp]
		public void SetUp()
		{
			ExecuteCommand("drop table \"Result\";");
			ExecuteCommand("create table \"Result\" (\"Id\" int, \"Value1\" varchar(32), \"Value2\" varchar(32));");
			ExecuteCommand("insert into \"Result\" (\"Id\", \"Value1\", \"Value2\") values (0, 'Value 10', 'Value 20');");
			ExecuteCommand("insert into \"Result\" (\"Id\", \"Value1\", \"Value2\") values (1, 'Value 10', 'Value 21');");
			ExecuteCommand("insert into \"Result\" (\"Id\", \"Value1\", \"Value2\") values (2, 'Value 10', 'Value 22');");
			ExecuteCommand("insert into \"Result\" (\"Id\", \"Value1\", \"Value2\") values (3, 'Value 10', 'Value 23');");
			ExecuteCommand("insert into \"Result\" (\"Id\", \"Value1\", \"Value2\") values (4, 'Value 10', 'Value 24');");
			ExecuteCommand("insert into \"Result\" (\"Id\", \"Value1\", \"Value2\") values (5, 'Value 10', 'Value 25');");
			ExecuteCommand("insert into \"Result\" (\"Id\", \"Value1\", \"Value2\") values (6, 'Value 11', 'Value 26');");
		}

		private static void ExecuteCommand(string commandText)
		{
			using (var connection = new FbConnection(ConnectionString))
			{
				using (var command = connection.CreateCommand())
				{
					connection.Open();

					try
					{
						command.CommandText = commandText;
						command.ExecuteNonQuery();
					}
					catch
					{
					}
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
								.ServerType(SqlServerType.Firebird)
								.ConnectionString(ConnectionString)
								.SelectCommand("select first (@count) \"Id\", \"Value1\", \"Value2\" from \"Result\" where \"Value1\" = @value1")))))
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