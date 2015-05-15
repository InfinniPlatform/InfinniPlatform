using System.Collections.Generic;
using System.Drawing;

using InfinniPlatform.FastReport.Templates.Borders;

using FRBorderLine = FastReport.BorderLine;
using FRBorderLines = FastReport.BorderLines;
using FRLineStyle = FastReport.LineStyle;
using FRElement = FastReport.ReportComponentBase;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Borders
{
	sealed class BorderBuilder : IReportObjectBuilder<Border>
	{
		public void BuildObject(IReportObjectBuilderContext context, Border template, object parent)
		{
			var element = (FRElement)parent;

			var elementBorder = element.Border;

			if (template.Left != null && template.Left.Width > 0)
			{
				elementBorder.Lines |= FRBorderLines.Left;
				SetBorderLine(elementBorder.LeftLine, template.Left);
			}

			if (template.Right != null && template.Right.Width > 0)
			{
				elementBorder.Lines |= FRBorderLines.Right;
				SetBorderLine(elementBorder.RightLine, template.Right);
			}

			if (template.Top != null && template.Top.Width > 0)
			{
				elementBorder.Lines |= FRBorderLines.Top;
				SetBorderLine(elementBorder.TopLine, template.Top);
			}

			if (template.Bottom != null && template.Bottom.Width > 0)
			{
				elementBorder.Lines |= FRBorderLines.Bottom;
				SetBorderLine(elementBorder.BottomLine, template.Bottom);
			}
		}

		private static void SetBorderLine(FRBorderLine reportBorderLine, BorderLine templateBorderLine)
		{
			// Тут толщина линии выражается в пикселях, но есть малая вероятность, что понадобятся миллиметры (как везде)

			reportBorderLine.Width = templateBorderLine.Width;
			reportBorderLine.Color = ColorTranslator.FromHtml(templateBorderLine.Color);
			reportBorderLine.Style = LineStyles[templateBorderLine.Style];
		}

		private static readonly Dictionary<BorderLineStyle, FRLineStyle> LineStyles
			= new Dictionary<BorderLineStyle, FRLineStyle>
				  {
					  { BorderLineStyle.Solid, FRLineStyle.Solid },
					  { BorderLineStyle.Double, FRLineStyle.Double },
					  { BorderLineStyle.Dot, FRLineStyle.Dot },
					  { BorderLineStyle.Dash, FRLineStyle.Dash },
					  { BorderLineStyle.DashDot, FRLineStyle.DashDot },
					  { BorderLineStyle.DashDotDot, FRLineStyle.DashDotDot }
				  };
	}
}