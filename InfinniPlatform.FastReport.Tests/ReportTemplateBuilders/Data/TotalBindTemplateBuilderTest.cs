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
	public sealed class TotalBindTemplateBuilderTest
	{
		private static readonly FrReportObjectTemplateBuilderContext Context
			= new FrReportObjectTemplateBuilderContext();

		[OneTimeSetUp]
		public void Setup()
		{
			var report = new Report();
			report.Dictionary.Totals.Add(new Total { Name = "Total1" });

			Context.Report = report;
		}

		[Test]
		public void ShouldBuildTotalBind()
		{
			// Given
			const string totalName = "[Total1]";

			// When
			var target = new TotalBindTemplateBuilder();
			var result = target.BuildTemplate(Context, totalName);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual("Total1", result.Total);
		}

		[Test]
		public void ShouldThrowExceptionWhenUnknownTotal()
		{
			// Given
			const string totalName = "[UnknownTotal]";

			// When
			TestDelegate test = () =>
			{
				var target = new TotalBindTemplateBuilder();
				target.BuildTemplate(Context, totalName);
			};

			// Then
			Assert.Throws<InvalidOperationException>(test);
		}
	}
}