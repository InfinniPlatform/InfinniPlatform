using System;
using System.Drawing;

using FastReport;
using FastReport.Table;

using InfinniPlatform.FastReport.ReportObjectBuilders;
using InfinniPlatform.FastReport.ReportObjectBuilders.Borders;
using InfinniPlatform.FastReport.TemplatesFluent.Borders;
using InfinniPlatform.FastReport.Templates.Borders;

using NUnit.Framework;

using Border = InfinniPlatform.FastReport.Templates.Borders.Border;

namespace InfinniPlatform.FastReport.Tests.ReportObjectBuilders.Borders
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class BorderBuilderTest
	{
		public ReportComponentBase[] TestElements
			= new ReportComponentBase[]
				  {
					  new ReportTitleBand(),
					  new ReportSummaryBand(),

					  new PageHeaderBand(),
					  new PageFooterBand(),

					  new GroupHeaderBand(),
					  new GroupFooterBand(),

					  new DataBand(),

					  new TextObject(),
					  new TableCell()
				  };


		[Test]
		[TestCaseSource("TestElements")]
		public void ShouldBuildBorders(ReportComponentBase parent)
		{
			// Given

			var template = CreateTemplate(b => b
				.Left(l => l
					.Width(10)
					.Color("Red")
					.Style(BorderLineStyle.Dot))
				.Right(l => l
					.Width(20)
					.Color("Green")
					.Style(BorderLineStyle.Dash))
				.Top(l => l
					.Width(30)
					.Color("Blue")
					.Style(BorderLineStyle.DashDot))
				.Bottom(l => l
					.Width(40)
					.Color("Yellow")
					.Style(BorderLineStyle.DashDotDot)));

			var context = new FrReportObjectBuilderContext();
			var target = new BorderBuilder();

			// When
			target.BuildObject(context, template, parent);

			// Then

			Assert.IsNotNull(parent.Border);
			Assert.IsTrue(parent.Border.Lines.HasFlag(BorderLines.Left));
			Assert.IsTrue(parent.Border.Lines.HasFlag(BorderLines.Right));
			Assert.IsTrue(parent.Border.Lines.HasFlag(BorderLines.Top));
			Assert.IsTrue(parent.Border.Lines.HasFlag(BorderLines.Bottom));

			Assert.IsNotNull(parent.Border.LeftLine);
			Assert.AreEqual(10, parent.Border.LeftLine.Width);
			Assert.AreEqual(Color.Red, parent.Border.LeftLine.Color);
			Assert.AreEqual(LineStyle.Dot, parent.Border.LeftLine.Style);

			Assert.IsNotNull(parent.Border.RightLine);
			Assert.AreEqual(20, parent.Border.RightLine.Width);
			Assert.AreEqual(Color.Green, parent.Border.RightLine.Color);
			Assert.AreEqual(LineStyle.Dash, parent.Border.RightLine.Style);

			Assert.IsNotNull(parent.Border.TopLine);
			Assert.AreEqual(30, parent.Border.TopLine.Width);
			Assert.AreEqual(Color.Blue, parent.Border.TopLine.Color);
			Assert.AreEqual(LineStyle.DashDot, parent.Border.TopLine.Style);

			Assert.IsNotNull(parent.Border.BottomLine);
			Assert.AreEqual(40, parent.Border.BottomLine.Width);
			Assert.AreEqual(Color.Yellow, parent.Border.BottomLine.Color);
			Assert.AreEqual(LineStyle.DashDotDot, parent.Border.BottomLine.Style);
		}

		[Test]
		[TestCaseSource("TestElements")]
		public void ShouldBuildOnlySpecifiedBorders(ReportComponentBase parent)
		{
			// Given

			var template = CreateTemplate(b => b
				.Left(l => l
					.Width(0)
					.Color("Red")
					.Style(BorderLineStyle.Dot)));

			var context = new FrReportObjectBuilderContext();
			var target = new BorderBuilder();

			// When
			target.BuildObject(context, template, parent);

			// Then

			Assert.IsNotNull(parent.Border);
			Assert.IsFalse(parent.Border.Lines.HasFlag(BorderLines.Left));
			Assert.IsFalse(parent.Border.Lines.HasFlag(BorderLines.Right));
			Assert.IsFalse(parent.Border.Lines.HasFlag(BorderLines.Top));
			Assert.IsFalse(parent.Border.Lines.HasFlag(BorderLines.Bottom));
		}

		private static Border CreateTemplate(Action<BorderConfig> config)
		{
			var template = new Border();
			config(new BorderConfig(template));
			return template;
		}
	}
}