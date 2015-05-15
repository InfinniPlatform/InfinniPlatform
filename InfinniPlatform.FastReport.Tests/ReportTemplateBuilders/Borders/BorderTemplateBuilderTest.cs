using System.Drawing;

using InfinniPlatform.FastReport.ReportTemplateBuilders;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Borders;
using InfinniPlatform.FastReport.Templates.Borders;

using NUnit.Framework;

using FRBorder = FastReport.Border;
using FRBorderLines = FastReport.BorderLines;
using FRLineStyle = FastReport.LineStyle;

namespace InfinniPlatform.FastReport.Tests.ReportTemplateBuilders.Borders
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class BorderTemplateBuilderTest
	{
		private readonly FrReportObjectTemplateBuilderContext _context = new FrReportObjectTemplateBuilderContext();


		[Test]
		public void ShouldBuildBorder()
		{
			// Given

			var border = new FRBorder
							 {
								 Lines = FRBorderLines.Left | FRBorderLines.Right,

								 LeftLine =
									 {
										 Width = 10,
										 Color = Color.Red,
										 Style = FRLineStyle.Dot
									 },
								 RightLine =
									 {
										 Width = 20,
										 Color = Color.Green,
										 Style = FRLineStyle.Dash
									 },
								 TopLine =
									 {
										 Width = 30,
										 Color = Color.Blue,
										 Style = FRLineStyle.DashDot
									 },
								 BottomLine =
									 {
										 Width = 40,
										 Color = Color.Yellow,
										 Style = FRLineStyle.DashDotDot
									 }
							 };

			// When
			var target = new BorderTemplateBuilder();
			var result = target.BuildTemplate(_context, border);

			// Then

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Left);
			Assert.IsNotNull(result.Right);
			Assert.IsNull(result.Top);
			Assert.IsNull(result.Bottom);

			Assert.AreEqual(10, result.Left.Width);
			Assert.AreEqual("Red", result.Left.Color);
			Assert.AreEqual(BorderLineStyle.Dot, result.Left.Style);

			Assert.AreEqual(20, result.Right.Width);
			Assert.AreEqual("Green", result.Right.Color);
			Assert.AreEqual(BorderLineStyle.Dash, result.Right.Style);
		}
	}
}