using System.Collections.Generic;

using FastReport;

using InfinniPlatform.FastReport.Templates.Elements;
using InfinniPlatform.FastReport.Templates.Font;

using DrawingFont = System.Drawing.Font;
using DrawingFontStyle = System.Drawing.FontStyle;
using DrawingGraphicsUnit = System.Drawing.GraphicsUnit;
using DrawingColorTranslator = System.Drawing.ColorTranslator;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Elements
{
	sealed class TextElementStyleTemplateBuilder : IReportObjectTemplateBuilder<TextElementStyle>
	{
		public TextElementStyle BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			var textObject = (TextObject)reportObject;

			return new TextElementStyle
					   {
						   Font = GetFont(textObject.Font),
						   Foreground = DrawingColorTranslator.ToHtml(textObject.TextColor),
						   Background = DrawingColorTranslator.ToHtml(textObject.FillColor),
						   Angle = textObject.Angle,
						   VerticalAlignment = VerticalAlignments[textObject.VertAlign],
						   HorizontalAlignment = HorizontalAlignments[textObject.HorzAlign],
						   WordWrap = textObject.WordWrap
					   };
		}


		private static FontSetup GetFont(DrawingFont font)
		{
			return (font != null)
					   ? new FontSetup
							 {
								 FontFamily = font.FontFamily.Name,
								 FontSize = font.Size,
								 FontSizeUnit = FontSizeUnits[font.Unit],
								 FontStyle = font.Italic ? FontStyle.Italic : FontStyle.Normal,
								 FontWeight = font.Bold ? FontWeight.Bold : FontWeight.Normal,
								 TextDecoration = font.Underline ? TextDecoration.Underline : (font.Strikeout ? TextDecoration.LineThrough : TextDecoration.Normal)
							 }
					   : null;
		}


		private static readonly Dictionary<HorzAlign, HorizontalAlignment> HorizontalAlignments
			= new Dictionary<HorzAlign, HorizontalAlignment>
				  {
					  { HorzAlign.Left, HorizontalAlignment.Left },
					  { HorzAlign.Center, HorizontalAlignment.Center },
					  { HorzAlign.Right, HorizontalAlignment.Right },
					  { HorzAlign.Justify, HorizontalAlignment.Justify }
				  };

		private static readonly Dictionary<VertAlign, VerticalAlignment> VerticalAlignments
			= new Dictionary<VertAlign, VerticalAlignment>
				  {
					  { VertAlign.Top, VerticalAlignment.Top },
					  { VertAlign.Center, VerticalAlignment.Center },
					  { VertAlign.Bottom, VerticalAlignment.Bottom }
				  };

		private static readonly Dictionary<DrawingGraphicsUnit, FontSizeUnit> FontSizeUnits
			= new Dictionary<DrawingGraphicsUnit, FontSizeUnit>
				  {
					  { DrawingGraphicsUnit.World, FontSizeUnit.Pt },
					  { DrawingGraphicsUnit.Display, FontSizeUnit.Px },
					  { DrawingGraphicsUnit.Pixel, FontSizeUnit.Px },
					  { DrawingGraphicsUnit.Point, FontSizeUnit.Pt },
					  { DrawingGraphicsUnit.Inch, FontSizeUnit.In },
					  { DrawingGraphicsUnit.Document, FontSizeUnit.Pt },
					  { DrawingGraphicsUnit.Millimeter, FontSizeUnit.Mm }
				  };
	}
}