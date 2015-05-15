using System;

using InfinniPlatform.FastReport.ReportObjectBuilders;
using InfinniPlatform.FastReport.ReportObjectBuilders.Print;
using InfinniPlatform.FastReport.TemplatesFluent.Print;
using InfinniPlatform.FastReport.Templates.Print;

using NUnit.Framework;

using FRReport = FastReport.Report;
using FRReportPage = FastReport.ReportPage;
using FRPrintMode = FastReport.PrintMode;

namespace InfinniPlatform.FastReport.Tests.ReportObjectBuilders.Print
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class PrintSetupBuilderTest
	{
		[Test]
		public void ShouldBuildPrinter()
		{
			// Given

			var template = CreateTemplate(ps => ps
				.Printer(p => p
					.PaperWidth(100)
					.PaperHeight(200)
					.PrintMode(PrintMode.Scale)));

			var context = new FrReportObjectBuilderContext();
			var target = new PrintSetupBuilder();
			var parent = new FRReport();
			var page = new FRReportPage();
			parent.Pages.Add(page);

			// When
			target.BuildObject(context, template, parent);

			// Then

			Assert.AreEqual(200, parent.PrintSettings.PrintOnSheetWidth);
			Assert.AreEqual(100, parent.PrintSettings.PrintOnSheetHeight);
			Assert.AreEqual(FRPrintMode.Scale, parent.PrintSettings.PrintMode);
		}

		[Test]
		public void ShouldBuildLandscapePaper()
		{
			// Given

			var template = CreateTemplate(ps => ps
				.Paper(p => p
					.Width(100)
					.Height(200)
					.Landscape()));

			var context = new FrReportObjectBuilderContext();
			var target = new PrintSetupBuilder();
			var parent = new FRReport();
			var page = new FRReportPage();
			parent.Pages.Add(page);

			// When
			target.BuildObject(context, template, parent);

			// Then

			Assert.AreEqual(200, page.PaperWidth);
			Assert.AreEqual(100, page.PaperHeight);
			Assert.IsTrue(page.Landscape);
		}

		[Test]
		public void ShouldBuildPortraitPaper()
		{
			// Given

			var template = CreateTemplate(ps => ps
				.Paper(p => p
					.Width(100)
					.Height(200)
					.Portrait()));

			var context = new FrReportObjectBuilderContext();
			var target = new PrintSetupBuilder();
			var parent = new FRReport();
			var page = new FRReportPage();
			parent.Pages.Add(page);

			// When
			target.BuildObject(context, template, parent);

			// Then

			Assert.AreEqual(100, page.PaperWidth);
			Assert.AreEqual(200, page.PaperHeight);
			Assert.IsFalse(page.Landscape);
		}

		[Test]
		public void ShouldBuildMargin()
		{
			// Given

			var template = CreateTemplate(ps => ps
				.Margin(m => m
					.Left(1).Right(2)
					.Top(3).Bottom(4)
					.MirrorOnEvenPages()));

			var context = new FrReportObjectBuilderContext();
			var target = new PrintSetupBuilder();
			var parent = new FRReport();
			var page = new FRReportPage();
			parent.Pages.Add(page);

			// When
			target.BuildObject(context, template, parent);

			// Then

			Assert.AreEqual(1, page.LeftMargin);
			Assert.AreEqual(2, page.RightMargin);
			Assert.AreEqual(3, page.TopMargin);
			Assert.AreEqual(4, page.BottomMargin);
			Assert.IsTrue(page.MirrorMargins);
		}


		private static PrintSetup CreateTemplate(Action<PrintSetupConfig> config)
		{
			var template = new PrintSetup();
			config(new PrintSetupConfig(template));
			return template;
		}
	}
}