using System;
using System.Collections;
using System.Linq;
using System.Text;
//using System.Windows;
using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.Model.Blocks;
using InfinniPlatform.FlowDocument.Model.Inlines;

//using FrameworkFlowDocument = System.Windows.Documents.FlowDocument;
using FrameworkFlowDocument = InfinniPlatform.FlowDocument.Model.Views.ViewDocument;

namespace InfinniPlatform.FlowDocument.Builders.Factories
{
	static class BuildHelper
	{
		public static readonly Thickness DefaultMargin = new Thickness(0);
		public static readonly Thickness DefaultPadding = new Thickness(0);
		public static readonly FontFamily DefautlFontFamily = new FontFamily("Arial");
		private static readonly BrushConverter BrushConverter = new BrushConverter();


		// TEXT

		/// <summary>
		/// Применяет общие свойства к элементу печатного представления.
		/// </summary>
		public static void ApplyTextProperties(dynamic element, dynamic metadata)
		{
			if (metadata != null)
			{
				ApplyFont(element, metadata.Font);
				ApplyForeground(element, metadata.Foreground);
				ApplyBackground(element, metadata.Background);
			}
		}

		/// <summary>
		/// Применяет общие свойства к элементу печатного представления после того, как он был сформирован.
		/// </summary>
		public static void PostApplyTextProperties(dynamic element, dynamic metadata)
		{
			if (metadata != null)
			{
				ApplyTextCase(element, metadata.TextCase);
			}
		}

		private static void ApplyFont(dynamic element, dynamic font)
		{
			if (font != null)
			{
				ApplyFontFamily(element, font.Family);
				ApplyFontSize(element, font.Size, font.SizeUnit);
				ApplyFontStyle(element, font.Style);
				ApplyFontStretch(element, font.Stretch);
				ApplyFontWeight(element, font.Weight);
				ApplyFontVariant(element, font.Variant);
			}
		}

		private static void ApplyFontFamily(dynamic element, dynamic fontFamily)
		{
			string fontFamilyString;

			if (ConvertHelper.TryToNormString(fontFamily, out fontFamilyString))
			{
				element.FontFamily = new FontFamily(fontFamilyString);
			}
		}

		private static void ApplyFontSize(dynamic element, dynamic fontSize, dynamic fontSizeUnit)
		{
			double fontSizeInPixels;

			if (TryToSizeInPixels(fontSize, fontSizeUnit, out fontSizeInPixels))
			{
				element.FontSize = fontSizeInPixels;
			}
		}

		private static void ApplyFontStyle(dynamic element, dynamic fontStyle)
		{
			string fontStyleString;

			if (ConvertHelper.TryToNormString(fontStyle, out fontStyleString))
			{
				switch (fontStyleString)
				{
					case "normal":
						element.FontStyle = FontStyles.Normal;
						break;
					case "italic":
						element.FontStyle = FontStyles.Italic;
						break;
					case "oblique":
						element.FontStyle = FontStyles.Oblique;
						break;
				}
			}
		}

		private static void ApplyFontStretch(dynamic element, dynamic fontStretch)
		{
			string fontStretchString;

			if (ConvertHelper.TryToNormString(fontStretch, out fontStretchString))
			{
				switch (fontStretchString)
				{
					case "ultracondensed":
						element.FontStretch = FontStretches.UltraCondensed;
						break;
					case "extracondensed":
						element.FontStretch = FontStretches.ExtraCondensed;
						break;
					case "condensed":
						element.FontStretch = FontStretches.Condensed;
						break;
					case "semicondensed":
						element.FontStretch = FontStretches.SemiCondensed;
						break;
					case "normal":
						element.FontStretch = FontStretches.Normal;
						break;
					case "semiexpanded":
						element.FontStretch = FontStretches.SemiExpanded;
						break;
					case "expanded":
						element.FontStretch = FontStretches.Expanded;
						break;
					case "extraexpanded":
						element.FontStretch = FontStretches.ExtraExpanded;
						break;
					case "ultraexpanded":
						element.FontStretch = FontStretches.UltraExpanded;
						break;
				}
			}
		}

		private static void ApplyFontWeight(dynamic element, dynamic fontWeight)
		{
			string fontWeightString;

			if (ConvertHelper.TryToNormString(fontWeight, out fontWeightString))
			{
				switch (fontWeightString)
				{
					case "ultralight":
						element.FontWeight = FontWeights.UltraLight;
						break;
					case "extralight":
						element.FontWeight = FontWeights.ExtraLight;
						break;
					case "light":
						element.FontWeight = FontWeights.Light;
						break;
					case "normal":
						element.FontWeight = FontWeights.Normal;
						break;
					case "medium":
						element.FontWeight = FontWeights.Medium;
						break;
					case "semibold":
						element.FontWeight = FontWeights.SemiBold;
						break;
					case "bold":
						element.FontWeight = FontWeights.Bold;
						break;
					case "extrabold":
						element.FontWeight = FontWeights.ExtraBold;
						break;
					case "ultrabold":
						element.FontWeight = FontWeights.UltraBold;
						break;
				}
			}
		}

		private static void ApplyFontVariant(dynamic element, dynamic fontVariant)
		{
			string fontVariantString;

			if (ConvertHelper.TryToNormString(fontVariant, out fontVariantString))
			{
				switch (fontVariantString)
				{
					case "normal":
						element.Typography.Variants = FontVariants.Normal;
						break;
					case "subscript":
						element.Typography.Variants = FontVariants.Subscript;
						break;
					case "superscript":
						element.Typography.Variants = FontVariants.Superscript;
						break;
				}
			}
		}

		private static void ApplyForeground(dynamic element, dynamic foreground)
		{
			Brush foregroundBrush;

			if (TryToBrush(foreground, out foregroundBrush))
			{
				element.Foreground = foregroundBrush;
			}
		}

		private static void ApplyBackground(dynamic element, dynamic background)
		{
			Brush backgroundBrush;

			if (TryToBrush(background, out backgroundBrush))
			{
				element.Background = backgroundBrush;
			}
		}

		private static void ApplyTextCase(object element, dynamic textCase)
		{
			string textCaseString;

			if (ConvertHelper.TryToNormString(textCase, out textCaseString))
			{
				switch (textCaseString)
				{
					case "sentencecase":
						ForEachRunElements(element, ApplySentenceCase);
						break;
					case "lowercase":
						ForEachRunElements(element, ApplyLowercase);
						break;
					case "uppercase":
						ForEachRunElements(element, ApplyUppercase);
						break;
					case "togglecase":
						ForEachRunElements(element, ApplyToggleCase);
						break;
				}
			}
		}

		private static bool ForEachRunElements(object element, Func<Run, bool> action)
		{
            if (element is FrameworkFlowDocument)
			{
                foreach (var item in ((FrameworkFlowDocument)element).Blocks.ToArray())
				{
					if (!ForEachRunElements(item, action))
					{
						return false;
					}
				}
			}
			else if (element is Table)
			{
				foreach (var item in ((Table)element).Rows.SelectMany(i => i.Cells).SelectMany(i => i.Blocks).ToArray())
				{
					if (!ForEachRunElements(item, action))
					{
						return false;
					}
				}
			}
			else if (element is TableRow)
			{
				foreach (var item in ((TableRow)element).Cells.SelectMany(i => i.Blocks).ToArray())
				{
					if (!ForEachRunElements(item, action))
					{
						return false;
					}
				}
			}
			else if (element is TableCell)
			{
				foreach (var item in ((TableCell)element).Blocks.ToArray())
				{
					if (!ForEachRunElements(item, action))
					{
						return false;
					}
				}
			}
			else if (element is Section)
			{
				foreach (var item in ((Section)element).Blocks.ToArray())
				{
					if (!ForEachRunElements(item, action))
					{
						return false;
					}
				}
			}
			else if (element is Paragraph)
			{
				foreach (var item in ((Paragraph)element).Inlines.ToArray())
				{
					if (!ForEachRunElements(item, action))
					{
						return false;
					}
				}
			}
			else if (element is Span)
			{
				foreach (var item in ((Span)element).Inlines.ToArray())
				{
					if (!ForEachRunElements(item, action))
					{
						return false;
					}
				}
			}
			else if (element is Run)
			{
				return action((Run)element);
			}

			return true;
		}

		private static bool ApplySentenceCase(Run element)
		{
			if (!string.IsNullOrEmpty(element.Text))
			{
				element.Text = char.ToUpper(element.Text[0]) + element.Text.Substring(1);
				return false;
			}

			return true;
		}

		private static bool ApplyLowercase(Run element)
		{
			if (!string.IsNullOrEmpty(element.Text))
			{
				element.Text = element.Text.ToLower();
			}

			return true;
		}

		private static bool ApplyUppercase(Run element)
		{
			if (!string.IsNullOrEmpty(element.Text))
			{
				element.Text = element.Text.ToUpper();
			}

			return true;
		}

		private static bool ApplyToggleCase(Run element)
		{
			if (!string.IsNullOrEmpty(element.Text))
			{
				var text = new StringBuilder(element.Text.Length);

				foreach (var c in element.Text)
				{
					text.Append(char.IsLower(c) ? char.ToUpper(c) : char.ToLower(c));
				}

				element.Text = text.ToString();
			}

			return true;
		}


		// BLOCK

		/// <summary>
		/// Применяет общие свойства блочных элементов печатного представления.
		/// </summary>
		public static void ApplyBlockProperties(dynamic element, dynamic metadata)
		{
			if (metadata != null)
			{
				ApplyBorder(element, metadata.Border);
				ApplyMargin(element, metadata.Margin);
				ApplyPadding(element, metadata.Padding);
				ApplyTextAlignment(element, metadata.TextAlignment);
			}
		}

		private static void ApplyBorder(dynamic element, dynamic border)
		{
			if (border != null)
			{
				Thickness borderThickness;

				if (TryToThickness(border.Thickness, out borderThickness))
				{
					element.BorderThickness = borderThickness;
				}

				Brush borderBrush;

				if (TryToBrush(border.Color, out borderBrush))
				{
					element.BorderBrush = borderBrush;
				}
			}
		}

		private static void ApplyMargin(dynamic element, dynamic margin)
		{
			Thickness marginThickness;

			if (TryToThickness(margin, out marginThickness))
			{
				element.Margin = marginThickness;
			}
		}

		private static void ApplyPadding(dynamic element, dynamic padding)
		{
			Thickness paddingThickness;

			if (TryToThickness(padding, out paddingThickness))
			{
				element.Padding = paddingThickness;
			}
		}

		private static void ApplyTextAlignment(dynamic element, dynamic textAlignment)
		{
			string textAlignmentString;

			if (ConvertHelper.TryToNormString(textAlignment, out textAlignmentString))
			{
				switch (textAlignmentString)
				{
					case "left":
						element.TextAlignment = TextAlignment.Left;
						break;
					case "center":
						element.TextAlignment = TextAlignment.Center;
						break;
					case "right":
						element.TextAlignment = TextAlignment.Right;
						break;
					case "justify":
						element.TextAlignment = TextAlignment.Justify;
						break;
				}
			}
		}


		// INLINE

		/// <summary>
		/// Применяет общие свойства однострочных элементов печатного представления.
		/// </summary>
		public static void ApplyInlineProperties(dynamic element, dynamic metadata)
		{
			if (metadata != null)
			{
				ApplyTextDecoration(element, metadata.TextDecoration);
			}
		}

		private static void ApplyTextDecoration(dynamic element, dynamic textDecoration)
		{
			string textDecorationString;

			if (ConvertHelper.TryToNormString(textDecoration, out textDecorationString))
			{
				switch (textDecorationString)
				{
					case "normal":
						element.TextDecorations = null;
						break;
					case "overline":
						element.TextDecorations = TextDecorations.OverLine;
						break;
					case "strikethrough":
						element.TextDecorations = TextDecorations.Strikethrough;
						break;
					case "underline":
						element.TextDecorations = TextDecorations.Underline;
						break;
				}
			}
		}


		// TABLE

		/// <summary>
		/// Применяет общие свойства к ячейке таблицы печатного представления.
		/// </summary>
		public static void ApplyTableCellProperties(dynamic element, dynamic metadata)
		{
			if (metadata != null)
			{
				ApplyBorder(element, metadata.Border);
				ApplyPadding(element, metadata.Padding);
				ApplyTextAlignment(element, metadata.TextAlignment);
			}
		}


		// HELPERS

		public static bool TryToSizeInPixels(dynamic originalSize, dynamic originalSizeUnit, out double sizeInPixels)
		{
			if (ConvertHelper.TryToDouble(originalSize, out sizeInPixels) && sizeInPixels > 0)
			{
				string sizeUnitString;
				ConvertHelper.TryToNormString(originalSizeUnit, out sizeUnitString);

				switch (sizeUnitString)
				{
					case "px":
						sizeInPixels *= SizeUnits.Px;
						break;
					case "in":
						sizeInPixels *= SizeUnits.In;
						break;
					case "cm":
						sizeInPixels *= SizeUnits.Cm;
						break;
					case "mm":
						sizeInPixels *= SizeUnits.Mm;
						break;
					default:
						sizeInPixels *= SizeUnits.Pt;
						break;
				}

				return true;
			}

			return false;
		}

		public static bool TryToThickness(dynamic value, out Thickness result)
		{
			if (value != null)
			{
				double all;

				if (TryToSizeInPixels(value.All, value.SizeUnit, out all))
				{
					result = new Thickness(all);
				}
				else
				{
					double left, top, right, bottom;
					TryToSizeInPixels(value.Left, value.SizeUnit, out left);
					TryToSizeInPixels(value.Top, value.SizeUnit, out top);
					TryToSizeInPixels(value.Right, value.SizeUnit, out right);
					TryToSizeInPixels(value.Bottom, value.SizeUnit, out bottom);

					result = new Thickness(left, top, right, bottom);
				}

				return true;
			}

			result = default(Thickness);
			return false;
		}

		private static bool TryToBrush(dynamic value, out Brush result)
		{
			result = null;

			string valueString;

			if (ConvertHelper.TryToNormString(value, out valueString))
			{
				try
				{
					result = (Brush)BrushConverter.ConvertFromString(valueString);
					return true;
				}
				catch
				{
				}
			}

			return false;
		}


		public static string FormatValue(PrintElementBuildContext buildContext, dynamic text, dynamic format)
		{
			string result;

			// Если текст не задан, значение берется из источника
			if (!ConvertHelper.TryToString(text, out result))
			{
				var sourceValue = buildContext.ElementSourceValue;

				if (sourceValue != null)
				{
					// Определение функции для форматирования значения

					Func<object, string> formatFunc = buildContext.ElementBuilder.BuildElement(buildContext, format);

					if (formatFunc == null)
					{
						formatFunc = v => v.ToString();
					}

					// Форматирование значения (или значений для коллекции)
					result = ConvertHelper.ObjectIsCollection(sourceValue)
						? string.Join("; ", ((IEnumerable)sourceValue).Cast<object>().Select(formatFunc))
						: formatFunc(sourceValue);
				}
				else if (buildContext.IsDesignMode)
				{
					// В режиме дизайна отображается наименование свойства источника данных или выражение

					if (!string.IsNullOrEmpty(buildContext.ElementSourceProperty))
					{
						result = string.Format("[{0}]", buildContext.ElementSourceProperty);
					}
					else if (!string.IsNullOrEmpty(buildContext.ElementSourceExpression))
					{
						result = string.Format("[{0}]", buildContext.ElementSourceExpression);
					}
				}
			}

			return result;
		}

		public static double CalcContentWidth(double elementWidth, params Thickness[] thicknesses)
		{
			var result = elementWidth;

			if (thicknesses != null)
			{
				foreach (var thickness in thicknesses)
				{
					result -= (ZeroIfNaN(thickness.Left) + ZeroIfNaN(thickness.Right));
				}
			}

			return Math.Max(result, 0);
		}

		private static double ZeroIfNaN(double value)
		{
			if (double.IsNaN(value))
			{
				return 0;
			}

			return value;
		}
	}
}