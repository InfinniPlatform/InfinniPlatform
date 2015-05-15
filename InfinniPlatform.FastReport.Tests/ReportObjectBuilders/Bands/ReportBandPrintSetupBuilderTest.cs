using System;

using FastReport;

using InfinniPlatform.FastReport.ReportObjectBuilders;
using InfinniPlatform.FastReport.ReportObjectBuilders.Bands;
using InfinniPlatform.FastReport.TemplatesFluent.Bands;
using InfinniPlatform.FastReport.Templates.Bands;

using NUnit.Framework;

namespace InfinniPlatform.FastReport.Tests.ReportObjectBuilders.Bands
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ReportBandPrintSetupBuilderTest
	{
		[Test]
		public void ShouldBuildReportBandPrintSetup()
		{
			// Given

			var template = CreateTemplate(ps => ps
				.PrintOn(PrintTargets.FirstPage | PrintTargets.EvenPages)
				.StartNewPage());

			var context = new FrReportObjectBuilderContext();
			var target = new ReportBandPrintSetupBuilder();
			var parent = new ReportTitleBand();

			// When
			target.BuildObject(context, template, parent);

			// Then
			Assert.IsTrue(parent.PrintOn.HasFlag(PrintOn.FirstPage));
			Assert.IsTrue(parent.PrintOn.HasFlag(PrintOn.EvenPages));
			Assert.IsTrue(parent.StartNewPage);
		}

		private static ReportBandPrintSetup CreateTemplate(Action<ReportBandPrintSetupConfig> config)
		{
			var template = new ReportBandPrintSetup();
			config(new ReportBandPrintSetupConfig(template));
			return template;
		}
	}
}