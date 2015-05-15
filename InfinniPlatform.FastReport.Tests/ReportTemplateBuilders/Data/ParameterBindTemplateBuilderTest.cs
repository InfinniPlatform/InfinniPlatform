using System;

using FastReport;
using FastReport.Data;

using InfinniPlatform.FastReport.ReportTemplateBuilders;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Data;

using NUnit.Framework;

namespace InfinniPlatform.FastReport.Tests.ReportTemplateBuilders.Data
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ParameterBindTemplateBuilderTest
	{
		private static readonly FrReportObjectTemplateBuilderContext Context
			= new FrReportObjectTemplateBuilderContext();

		[TestFixtureSetUp]
		public void Setup()
		{
			var report = new Report();
			report.Dictionary.Parameters.Add(new Parameter("Parameter1"));

			Context.Report = report;
		}

		[Test]
		public void ShouldBuildParameterBind()
		{
			// Given
			const string parameterName = "[Parameter1]";

			// When
			var target = new ParameterBindTemplateBuilder();
			var result = target.BuildTemplate(Context, parameterName);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual("Parameter1", result.Parameter);
		}

		[Test]
		public void ShouldThrowExceptionWhenUnknownParameter()
		{
			// Given
			const string parameterName = "[UnknownParameter]";

			// When
			TestDelegate test = () =>
				                    {
					                    var target = new ParameterBindTemplateBuilder();
					                    target.BuildTemplate(Context, parameterName);
				                    };

			// Then
			Assert.Throws<InvalidOperationException>(test);
		}
	}
}