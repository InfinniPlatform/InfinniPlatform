using System.Collections.Generic;
using System.Drawing;

using InfinniPlatform.FastReport.Templates.Borders;

using FRBorder = FastReport.Border;
using FRBorderLine = FastReport.BorderLine;
using FRBorderLines = FastReport.BorderLines;
using FRLineStyle = FastReport.LineStyle;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Borders
{
	sealed class BorderTemplateBuilder : IReportObjectTemplateBuilder<Border>
	{
		public Border BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			var border = (FRBorder)reportObject;

			return new Border
					   {
						   Left = border.Lines.HasFlag(FRBorderLines.Left) ? GetBorderLine(border.LeftLine) : null,
						   Right = border.Lines.HasFlag(FRBorderLines.Right) ? GetBorderLine(border.RightLine) : null,
						   Top = border.Lines.HasFlag(FRBorderLines.Top) ? GetBorderLine(border.TopLine) : null,
						   Bottom = border.Lines.HasFlag(FRBorderLines.Bottom) ? GetBorderLine(border.BottomLine) : null
					   };
		}

		private static BorderLine GetBorderLine(FRBorderLine reportBorderLine)
		{
			return (reportBorderLine != null)
					   ? new BorderLine
							 {
								 Width = reportBorderLine.Width,
								 Color = ColorTranslator.ToHtml(reportBorderLine.Color),
								 Style = LineStyles[reportBorderLine.Style]
							 }
					   : null;
		}

		private static readonly Dictionary<FRLineStyle, BorderLineStyle> LineStyles
			= new Dictionary<FRLineStyle, BorderLineStyle>
				  {
					  { FRLineStyle.Solid, BorderLineStyle.Solid },
					  { FRLineStyle.Dash, BorderLineStyle.Dash },
					  { FRLineStyle.Dot, BorderLineStyle.Dot },
					  { FRLineStyle.DashDot, BorderLineStyle.DashDot },
					  { FRLineStyle.DashDotDot, BorderLineStyle.DashDotDot },
					  { FRLineStyle.Double, BorderLineStyle.Solid }
				  };
	}
}