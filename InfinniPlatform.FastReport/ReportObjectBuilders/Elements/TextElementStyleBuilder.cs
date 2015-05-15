using System.Collections.Generic;

using FastReport;

using InfinniPlatform.FastReport.Templates.Elements;
using InfinniPlatform.FastReport.Templates.Font;

using DrawingFont = System.Drawing.Font;
using DrawingFontStyle = System.Drawing.FontStyle;
using DrawingGraphicsUnit = System.Drawing.GraphicsUnit;
using DrawingColorTranslator = System.Drawing.ColorTranslator;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Elements
{
	sealed class TextElementStyleBuilder : IReportObjectBuilder<TextElementStyle>
	{
		public void BuildObject(IReportObjectBuilderContext context, TextElementStyle template, object parent)
		{
			var element = (TextObject)parent;

			if (template.Font != null)
			{
				element.Font = CreateDrawingFont(template.Font);
			}

			element.Angle = (int)template.Angle;
			element.WordWrap = template.WordWrap;
			element.HorzAlign = HorizontalAlignments[template.HorizontalAlignment];
			element.VertAlign = VerticalAlignments[template.VerticalAlignment];
			element.TextColor = DrawingColorTranslator.FromHtml(template.Foreground);
			element.FillColor = DrawingColorTranslator.FromHtml(template.Background);
		}


		private static DrawingFont CreateDrawingFont(FontSetup fontSetup)
		{
			var drawingFontStyle = DrawingFontStyle.Regular;

			if (fontSetup.FontStyle != null)
			{
				drawingFontStyle |= FontStyles[fontSetup.FontStyle.Value];
			}

			if (fontSetup.FontWeight != null)
			{
				drawingFontStyle |= FontWeights[fontSetup.FontWeight.Value];
			}

			if (fontSetup.TextDecoration != null)
			{
				drawingFontStyle |= TextDecorations[fontSetup.TextDecoration.Value];
			}

			return new DrawingFont(fontSetup.FontFamily, fontSetup.FontSize, drawingFontStyle, FontSizeUnits[fontSetup.FontSizeUnit]);
		}


		private static readonly Dictionary<HorizontalAlignment, HorzAlign> HorizontalAlignments
			= new Dictionary<HorizontalAlignment, HorzAlign>
				  {
					  { HorizontalAlignment.Left, HorzAlign.Left },
					  { HorizontalAlignment.Center, HorzAlign.Center },
					  { HorizontalAlignment.Right, HorzAlign.Right },
					  { HorizontalAlignment.Justify, HorzAlign.Justify }
				  };

		private static readonly Dictionary<VerticalAlignment, VertAlign> VerticalAlignments
			= new Dictionary<VerticalAlignment, VertAlign>
				  {
					  { VerticalAlignment.Top, VertAlign.Top },
					  { VerticalAlignment.Center, VertAlign.Center },
					  { VerticalAlignment.Bottom, VertAlign.Bottom }
				  };

		private static readonly Dictionary<FontStyle, DrawingFontStyle> FontStyles
			= new Dictionary<FontStyle, DrawingFontStyle>
				  {
					  { FontStyle.Normal, DrawingFontStyle.Regular },
					  { FontStyle.Italic, DrawingFontStyle.Italic },
					  { FontStyle.Oblique, DrawingFontStyle.Italic }
				  };

		private static readonly Dictionary<FontWeight, DrawingFontStyle> FontWeights
			= new Dictionary<FontWeight, DrawingFontStyle>
				  {
					  { FontWeight.UltraLight, DrawingFontStyle.Regular },
					  { FontWeight.ExtraLight, DrawingFontStyle.Regular },
					  { FontWeight.Light, DrawingFontStyle.Regular },
					  { FontWeight.Normal, DrawingFontStyle.Regular },
					  { FontWeight.Medium, DrawingFontStyle.Bold },
					  { FontWeight.SemiBold, DrawingFontStyle.Bold },
					  { FontWeight.Bold, DrawingFontStyle.Bold },
					  { FontWeight.ExtraBold, DrawingFontStyle.Bold },
					  { FontWeight.UltraBold, DrawingFontStyle.Bold }
				  };

		private static readonly Dictionary<TextDecoration, DrawingFontStyle> TextDecorations
			= new Dictionary<TextDecoration, DrawingFontStyle>
				  {
					  { TextDecoration.Normal, DrawingFontStyle.Regular },
					  { TextDecoration.Overline, DrawingFontStyle.Regular },
					  { TextDecoration.LineThrough, DrawingFontStyle.Strikeout },
					  { TextDecoration.Underline, DrawingFontStyle.Underline }
				  };

		private static readonly Dictionary<FontSizeUnit, DrawingGraphicsUnit> FontSizeUnits
			= new Dictionary<FontSizeUnit, DrawingGraphicsUnit>
				  {
					  { FontSizeUnit.Pt, DrawingGraphicsUnit.Point },
					  { FontSizeUnit.Px, DrawingGraphicsUnit.Pixel },
					  { FontSizeUnit.In, DrawingGraphicsUnit.Inch },
					  { FontSizeUnit.Mm, DrawingGraphicsUnit.Millimeter }
				  };
	}
}