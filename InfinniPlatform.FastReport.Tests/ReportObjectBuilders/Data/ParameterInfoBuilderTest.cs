using System;

using InfinniPlatform.Core.Schema;
using InfinniPlatform.FastReport.ReportObjectBuilders;
using InfinniPlatform.FastReport.ReportObjectBuilders.Data;
using InfinniPlatform.FastReport.Templates.Data;

using NUnit.Framework;

using FRReport = FastReport.Report;

namespace InfinniPlatform.FastReport.Tests.ReportObjectBuilders.Data
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ParameterInfoBuilderTest
	{
		[Test]
		[TestCase(SchemaDataType.None, typeof(string))]
		[TestCase(SchemaDataType.String, typeof(string))]
		[TestCase(SchemaDataType.Float, typeof(double))]
		[TestCase(SchemaDataType.Integer, typeof(int))]
		[TestCase(SchemaDataType.Boolean, typeof(bool))]
		[TestCase(SchemaDataType.DateTime, typeof(DateTime))]
		public void ShouldBuildParameterInfo(SchemaDataType dataType, Type expectedDataType)
		{
			// Given
			var template = new ParameterInfo { Name = "Parameter1", Caption = "Параметр1", Type = dataType };
			var context = new FrReportObjectBuilderContext();
			var target = new ParameterInfoBuilder();
			var parent = new FRReport();

			// When
			target.BuildObject(context, template, parent);

			// Then

			Assert.IsNotNull(parent.Parameters);
			Assert.AreEqual(1, parent.Parameters.Count);

			Assert.IsNotNull(parent.Parameters[0]);
			Assert.AreEqual("Parameter1", parent.Parameters[0].Name);
			Assert.AreEqual("Параметр1", parent.Parameters[0].Description);
			Assert.AreEqual(expectedDataType, parent.Parameters[0].DataType);
		}
	}
}