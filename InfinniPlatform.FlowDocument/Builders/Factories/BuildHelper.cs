using System;
using System.Collections;
using System.Linq;
using System.Text;

using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.Model.Blocks;
using InfinniPlatform.FlowDocument.Model.Font;
using InfinniPlatform.FlowDocument.Model.Inlines;
using InfinniPlatform.FlowDocument.Model.Views;

namespace InfinniPlatform.FlowDocument.Builders.Factories
{
    static class BuildHelper
    {
        public const string DefautlFontFamily = "Arial";
        public static readonly PrintElementThickness DefaultMargin = new PrintElementThickness(0);
        public static readonly PrintElementThickness DefaultPadding = new PrintElementThickness(0);


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
                var fontValue = new PrintElementFont();

                string fontFamilyString;

                if (ConvertHelper.TryToString(font.Family, out fontFamilyString))
                {
                    fontValue.Family = fontFamilyString;
                }

                double fontSizeInPixels;

                if (TryToSizeInPixels(font.Size, font.SizeUnit, out fontSizeInPixels))
                {
                    fontValue.Size = fontSizeInPixels;
                }

                string fontStyleString;

                if (ConvertHelper.TryToNormString(font.Style, out fontStyleString))
                {
                    switch (fontStyleString)
                    {
                        case "normal":
                            fontValue.Style = PrintElementFontStyle.Normal;
                            break;
                        case "italic":
                            fontValue.Style = PrintElementFontStyle.Italic;
                            break;
                        case "oblique":
                            fontValue.Style = PrintElementFontStyle.Oblique;
                            break;
                    }
                }

                string fontStretchString;

                if (ConvertHelper.TryToNormString(font.Stretch, out fontStretchString))
                {
                    switch (fontStretchString)
                    {
                        case "ultracondensed":
                            fontValue.Stretch = PrintElementFontStretch.UltraCondensed;
                            break;
                        case "extracondensed":
                            fontValue.Stretch = PrintElementFontStretch.ExtraCondensed;
                            break;
                        case "condensed":
                            fontValue.Stretch = PrintElementFontStretch.Condensed;
                            break;
                        case "semicondensed":
                            fontValue.Stretch = PrintElementFontStretch.SemiCondensed;
                            break;
                        case "normal":
                            fontValue.Stretch = PrintElementFontStretch.Normal;
                            break;
                        case "semiexpanded":
                            fontValue.Stretch = PrintElementFontStretch.SemiExpanded;
                            break;
                        case "expanded":
                            fontValue.Stretch = PrintElementFontStretch.Expanded;
                            break;
                        case "extraexpanded":
                            fontValue.Stretch = PrintElementFontStretch.ExtraExpanded;
                            break;
                        case "ultraexpanded":
                            fontValue.Stretch = PrintElementFontStretch.UltraExpanded;
                            break;
                    }
                }

                string fontWeightString;

                if (ConvertHelper.TryToNormString(font.Weight, out fontWeightString))
                {
                    switch (fontWeightString)
                    {
                        case "ultralight":
                            fontValue.Weight = PrintElementFontWeight.UltraLight;
                            break;
                        case "extralight":
                            fontValue.Weight = PrintElementFontWeight.ExtraLight;
                            break;
                        case "light":
                            fontValue.Weight = PrintElementFontWeight.Light;
                            break;
                        case "normal":
                            fontValue.Weight = PrintElementFontWeight.Normal;
                            break;
                        case "medium":
                            fontValue.Weight = PrintElementFontWeight.Medium;
                            break;
                        case "semibold":
                            fontValue.Weight = PrintElementFontWeight.SemiBold;
                            break;
                        case "bold":
                            fontValue.Weight = PrintElementFontWeight.Bold;
                            break;
                        case "extrabold":
                            fontValue.Weight = PrintElementFontWeight.ExtraBold;
                            break;
                        case "ultrabold":
                            fontValue.Weight = PrintElementFontWeight.UltraBold;
                            break;
                    }
                }

                string fontVariantString;

                if (ConvertHelper.TryToNormString(font.Variant, out fontVariantString))
                {
                    switch (fontVariantString)
                    {
                        case "normal":
                            fontValue.Variant = PrintElementFontVariant.Normal;
                            break;
                        case "subscript":
                            fontValue.Variant = PrintElementFontVariant.Subscript;
                            break;
                        case "superscript":
                            fontValue.Variant = PrintElementFontVariant.Superscript;
                            break;
                    }
                }

                element.Font = fontValue;
            }
        }

        private static void ApplyForeground(dynamic element, dynamic foreground)
        {
            string foregroundBrush;

            if (ConvertHelper.TryToString(foreground, out foregroundBrush))
            {
                element.Foreground = foregroundBrush;
            }
        }

        private static void ApplyBackground(dynamic element, dynamic background)
        {
            string backgroundBrush;

            if (ConvertHelper.TryToString(background, out backgroundBrush))
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

        private static bool ForEachRunElements(object element, Func<PrintElementRun, bool> action)
        {
            if (element is PrintViewDocument)
            {
                foreach (var item in ((PrintViewDocument)element).Blocks.ToArray())
                {
                    if (!ForEachRunElements(item, action))
                    {
                        return false;
                    }
                }
            }
            else if (element is PrintElementTable)
            {
                foreach (var item in ((PrintElementTable)element).Rows.SelectMany(i => i.Cells).Select(i => i.Block).ToArray())
                {
                    if (!ForEachRunElements(item, action))
                    {
                        return false;
                    }
                }
            }
            else if (element is PrintElementTableRow)
            {
                foreach (var item in ((PrintElementTableRow)element).Cells.Select(i => i.Block).ToArray())
                {
                    if (!ForEachRunElements(item, action))
                    {
                        return false;
                    }
                }
            }
            else if (element is PrintElementTableCell)
            {
                if (!ForEachRunElements(((PrintElementTableCell)element).Block, action))
                {
                    return false;
                }
            }
            else if (element is PrintElementSection)
            {
                foreach (var item in ((PrintElementSection)element).Blocks.ToArray())
                {
                    if (!ForEachRunElements(item, action))
                    {
                        return false;
                    }
                }
            }
            else if (element is PrintElementParagraph)
            {
                foreach (var item in ((PrintElementParagraph)element).Inlines.ToArray())
                {
                    if (!ForEachRunElements(item, action))
                    {
                        return false;
                    }
                }
            }
            else if (element is PrintElementSpan)
            {
                foreach (var item in ((PrintElementSpan)element).Inlines.ToArray())
                {
                    if (!ForEachRunElements(item, action))
                    {
                        return false;
                    }
                }
            }
            else if (element is PrintElementRun)
            {
                return action((PrintElementRun)element);
            }

            return true;
        }

        private static bool ApplySentenceCase(PrintElementRun element)
        {
            if (!string.IsNullOrEmpty(element.Text))
            {
                element.Text = char.ToUpper(element.Text[0]) + element.Text.Substring(1);
                return false;
            }

            return true;
        }

        private static bool ApplyLowercase(PrintElementRun element)
        {
            if (!string.IsNullOrEmpty(element.Text))
            {
                element.Text = element.Text.ToLower();
            }

            return true;
        }

        private static bool ApplyUppercase(PrintElementRun element)
        {
            if (!string.IsNullOrEmpty(element.Text))
            {
                element.Text = element.Text.ToUpper();
            }

            return true;
        }

        private static bool ApplyToggleCase(PrintElementRun element)
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
                PrintElementThickness borderThickness;

                if (TryToThickness(border.Thickness, out borderThickness))
                {
                    element.Border.Thickness = borderThickness;
                }

                string borderColor;

                if (ConvertHelper.TryToNormString(border.Color, out borderColor))
                {
                    element.Border.Color = borderColor;
                }
            }
        }

        private static void ApplyMargin(dynamic element, dynamic margin)
        {
            PrintElementThickness marginThickness;

            if (TryToThickness(margin, out marginThickness))
            {
                element.Margin = marginThickness;
            }
        }

        private static void ApplyPadding(dynamic element, dynamic padding)
        {
            PrintElementThickness paddingThickness;

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
                        element.TextAlignment = PrintElementTextAlignment.Left;
                        break;
                    case "center":
                        element.TextAlignment = PrintElementTextAlignment.Center;
                        break;
                    case "right":
                        element.TextAlignment = PrintElementTextAlignment.Right;
                        break;
                    case "justify":
                        element.TextAlignment = PrintElementTextAlignment.Justify;
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
                        element.TextDecoration = null;
                        break;
                    case "overline":
                        element.TextDecoration = PrintElementTextDecoration.OverLine;
                        break;
                    case "strikethrough":
                        element.TextDecoration = PrintElementTextDecoration.Strikethrough;
                        break;
                    case "underline":
                        element.TextDecoration = PrintElementTextDecoration.Underline;
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

        public static bool TryToThickness(dynamic value, out PrintElementThickness result)
        {
            if (value != null)
            {
                double all;

                if (TryToSizeInPixels(value.All, value.SizeUnit, out all))
                {
                    result = new PrintElementThickness(all);
                }
                else
                {
                    double left, top, right, bottom;
                    TryToSizeInPixels(value.Left, value.SizeUnit, out left);
                    TryToSizeInPixels(value.Top, value.SizeUnit, out top);
                    TryToSizeInPixels(value.Right, value.SizeUnit, out right);
                    TryToSizeInPixels(value.Bottom, value.SizeUnit, out bottom);

                    result = new PrintElementThickness(left, top, right, bottom);
                }

                return true;
            }

            result = default(PrintElementThickness);

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

        public static double CalcContentWidth(double elementWidth, params PrintElementThickness[] thicknesses)
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

        private static double ZeroIfNaN(double? value)
        {
            if (value == null || double.IsNaN((double)value))
            {
                return 0;
            }

            return value.Value;
        }
    }
}