using System;
using System.Linq;

using FastReport;
using FastReport.Utils;

using InfinniPlatform.FastReport.ReportObjectBuilders;
using InfinniPlatform.FastReport.ReportObjectBuilders.Bands;
using InfinniPlatform.FastReport.TemplatesFluent.Bands;
using InfinniPlatform.FastReport.Templates.Bands;
using InfinniPlatform.FastReport.Templates.Elements;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.FastReport.Tests.ReportObjectBuilders.Bands
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ReportBandBuilderTest
	{
		private static readonly ReportBandBuilder Target = new ReportBandBuilder();


		[Test]
		public void ShouldBuildReportBand()
		{
			// Given

			var template = CreateTemplate(b => b
				.Height(10)
				.CanGrow()
				.CanShrink());

			var context = new FrReportObjectBuilderContext();
			var parent = new ReportTitleBand();

			// When
			Target.BuildObject(context, template, parent);

			// Then

			Assert.AreEqual(10 * Units.Millimeters, parent.Height);
			Assert.IsTrue(parent.CanGrow);
			Assert.IsTrue(parent.CanShrink);
		}

		[Test]
		public void ShouldBuildReportBandWithAllProperties()
		{
			// Given

			var context = new FrReportObjectBuilderContext();
			var borderBuilder = new Mock<IReportObjectBuilder<FastReport.Templates.Borders.Border>>();
			context.RegisterBuilder(borderBuilder.Object);
			var printSetupBuilder = new Mock<IReportObjectBuilder<ReportBandPrintSetup>>();
			context.RegisterBuilder(printSetupBuilder.Object);
			var textElementBuilder = new Mock<IReportObjectBuilder<TextElement>>();
			context.RegisterBuilder(textElementBuilder.Object);

			var template = CreateTemplate(b => b
				.Height(10)
				.CanGrow()
				.CanShrink()
				.Border(x => { })
				.PrintSetup(x => { })
				.Elements(x => x.Text(t => { })));

			var parent = new ReportTitleBand();

			// When
			Target.BuildObject(context, template, parent);

			// Then
			borderBuilder.Verify(m => m.BuildObject(context, template.Border, It.IsAny<object>()));
			printSetupBuilder.Verify(m => m.BuildObject(context, template.PrintSetup, It.IsAny<object>()));
			textElementBuilder.Verify(m => m.BuildObject(context, (TextElement)template.Elements.First(), It.IsAny<object>()));
		}

		private static ReportBand CreateTemplate(Action<ReportBandConfig> config)
		{
			var template = new ReportBand();
			config(new ReportBandConfig(template));
			return template;
		}
	}
}