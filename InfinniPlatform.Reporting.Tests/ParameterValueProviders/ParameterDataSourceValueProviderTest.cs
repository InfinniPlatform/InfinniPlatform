using System.Collections.Generic;
using InfinniPlatform.Api.Schema;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.Reporting.ParameterValueProviders;
using InfinniPlatform.Reporting.Tests.DataSources;

using Moq;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace InfinniPlatform.Reporting.Tests.ParameterValueProviders
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ParameterDataSourceValueProviderTest
	{
		[Test]
		public void ShouldGetParameterValues()
		{
			// Given

			var dataSource = new DataSourceStub(JArray.Parse(@"
				[
					{
						'MyLabelProperty': 'Label1',
						'MyValueProperty': 'Value1'
					},
					{
						'MyLabelProperty': 'Label2',
						'MyValueProperty': 'Value2'
					},
					{
						'MyLabelProperty': 'Label3',
						'MyValueProperty': 'Value3'
					}
				]
			"));

			var dataSourceInfo = new DataSourceInfo
									 {
										 Name = "MyDataSource",

										 Provider = new Mock<IDataProviderInfo>().Object,

										 Schema = new DataSchema
													  {
														  Type = SchemaDataType.Object,

														  Properties = new Dictionary<string, DataSchema>
								                                           {
									                                           { "MyLabelProperty", new DataSchema { Type = SchemaDataType.String } },
									                                           { "MyValueProperty", new DataSchema { Type = SchemaDataType.String } }
								                                           }
													  }
									 };

			var valueProviderInfo = new ParameterDataSourceValueProviderInfo
										{
											DataSource = "MyDataSource",
											LabelProperty = "MyLabelProperty",
											ValueProperty = "MyValueProperty"
										};

			// When
			var target = new ParameterDataSourceValueProvider(dataSource);
			var result = target.GetParameterValues(valueProviderInfo, new[] { dataSourceInfo });

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Count);
			Assert.AreEqual("Value1", ((JValue)result["Label1"]).Value);
			Assert.AreEqual("Value2", ((JValue)result["Label2"]).Value);
			Assert.AreEqual("Value3", ((JValue)result["Label3"]).Value);
		}
	}
}