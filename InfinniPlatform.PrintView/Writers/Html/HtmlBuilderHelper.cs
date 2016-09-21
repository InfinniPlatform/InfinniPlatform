using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Blocks;
using InfinniPlatform.PrintView.Model.Font;
using InfinniPlatform.PrintView.Model.Inlines;

namespace InfinniPlatform.PrintView.Writers.Html
{
    internal static class HtmlBuilderHelper
    {
        public static string GetTextAligment(PrintElementTextAlignment align)
        {
            switch (align)
            {
                case PrintElementTextAlignment.Center:
                    return "center";
                case PrintElementTextAlignment.Justify:
                    return "justify";
                case PrintElementTextAlignment.Left:
                    return "left";
                case PrintElementTextAlignment.Right:
                    return "right";
            }

            return null;
        }

        public static string GetFontStyle(PrintElementFontStyle style)
        {
            switch (style)
            {
                case PrintElementFontStyle.Italic:
                    return "italic";
                case PrintElementFontStyle.Normal:
                    return "normal";
                case PrintElementFontStyle.Oblique:
                    return "oblique";
            }

            return null;
        }

        public static string GetFontStretch(PrintElementFontStretch stretch)
        {
            switch (stretch)
            {
                case PrintElementFontStretch.Normal:
                    return "normal";
                case PrintElementFontStretch.Condensed:
                    return "condensed";
                case PrintElementFontStretch.Expanded:
                    return "expanded";
                case PrintElementFontStretch.ExtraCondensed:
                    return "extra-condensed";
                case PrintElementFontStretch.ExtraExpanded:
                    return "extra-expanded";
                case PrintElementFontStretch.SemiCondensed:
                    return "semi-condensed";
                case PrintElementFontStretch.SemiExpanded:
                    return "semi-expanded";
                case PrintElementFontStretch.UltraCondensed:
                    return "ultra-condensed";
                case PrintElementFontStretch.UltraExpanded:
                    return "ultra-expanded";
            }

            return null;
        }

        public static string GetFontWeight(PrintElementFontWeight weight)
        {
            switch (weight)
            {
                case PrintElementFontWeight.UltraLight:
                    return "100";
                case PrintElementFontWeight.ExtraLight:
                    return "200";
                case PrintElementFontWeight.Light:
                    return "300";
                case PrintElementFontWeight.Normal:
                    return "400";
                case PrintElementFontWeight.Medium:
                    return "500";
                case PrintElementFontWeight.SemiBold:
                    return "600";
                case PrintElementFontWeight.Bold:
                    return "700";
                case PrintElementFontWeight.ExtraBold:
                    return "800";
                case PrintElementFontWeight.UltraBold:
                    return "900";
            }

            return null;
        }

        public static string GetTextDecoration(PrintElementTextDecoration decoration)
        {
            switch (decoration)
            {
                case PrintElementTextDecoration.OverLine:
                    return "overline";
                case PrintElementTextDecoration.Strikethrough:
                    return "line-through";
                case PrintElementTextDecoration.Underline:
                    return "underline";
            }

            return null;
        }

        public static string GetMarkerStyle(PrintElementListMarkerStyle style)
        {
            switch (style)
            {
                case PrintElementListMarkerStyle.Box:
                    return "box";
                case PrintElementListMarkerStyle.Circle:
                    return "circle";
                case PrintElementListMarkerStyle.Decimal:
                    return "decimal";
                case PrintElementListMarkerStyle.Disc:
                    return "disk";
                case PrintElementListMarkerStyle.LowerLatin:
                    return "lower-latin";
                case PrintElementListMarkerStyle.LowerRoman:
                    return "lower-roman";
                case PrintElementListMarkerStyle.None:
                    return "none";
                case PrintElementListMarkerStyle.Square:
                    return "square";
                case PrintElementListMarkerStyle.UpperLatin:
                    return "upper-latin";
                case PrintElementListMarkerStyle.UpperRoman:
                    return "upper-roman";
            }

            return null;
        }
        public static void ApplyBaseStyles(this TextWriter result, PrintElement element)
        {
            if (element.Font != null)
            {
                if (!string.IsNullOrWhiteSpace(element.Font.Family))
                {
                    result.Write("font-family:");
                    result.Write(element.Font.Family);
                    result.Write(";");
                }

                if (element.Font.Size != null)
                {
                    result.Write("font-size:");
                    result.WriteInvariant(element.Font.Size.Value);
                    result.Write("px;");
                }

                if (element.Font.Style != null)
                {
                    result.Write("font-style:");
                    result.Write(GetFontStyle(element.Font.Style.Value));
                    result.Write(";");
                }

                if (element.Font.Stretch != null)
                {
                    result.Write("font-stretch:");
                    result.Write(GetFontStretch(element.Font.Stretch.Value));
                    result.Write(";");
                }

                if (element.Font.Weight != null)
                {
                    result.Write("font-weight:");
                    result.Write(GetFontWeight(element.Font.Weight.Value));
                    result.Write(";");
                }
            }

            if (!string.IsNullOrWhiteSpace(element.Background))
            {
                result.Write("background-color:");
                result.Write(element.Background.TryToRgba());
                result.Write(";");
            }

            if (!string.IsNullOrWhiteSpace(element.Foreground))
            {
                result.Write("color:");
                result.Write(element.Foreground.TryToRgba());
                result.Write(";");
            }
        }

        public static void ApplyBlockStyles(this TextWriter result, PrintElementBlock element)
        {
            if (element.Border != null)
            {
                result.Write("border-top-width:");
                result.WriteInvariant(element.Border.Thickness.Top);
                result.Write("px;");

                result.Write("border-right-width:");
                result.WriteInvariant(element.Border.Thickness.Right);
                result.Write("px;");

                result.Write("border-bottom-width:");
                result.WriteInvariant(element.Border.Thickness.Bottom);
                result.Write("px;");

                result.Write("border-left-width:");
                result.WriteInvariant(element.Border.Thickness.Left);
                result.Write("px;");

                if (!string.IsNullOrWhiteSpace(element.Border.Color))
                {
                    result.Write("border-style:solid;");

                    result.Write("border-color:");
                    result.Write(element.Border.Color.TryToRgba());
                    result.Write(";");
                }

            }

            result.Write("margin-top:");
            result.WriteInvariant(element.Margin.Top);
            result.Write("px;");

            result.Write("margin-right:");
            result.WriteInvariant(element.Margin.Right);
            result.Write("px;");

            result.Write("margin-bottom:");
            result.WriteInvariant(element.Margin.Bottom);
            result.Write("px;");

            result.Write("margin-left:");
            result.WriteInvariant(element.Margin.Left);
            result.Write("px;");

            result.Write("padding-top:");
            result.WriteInvariant(element.Padding.Top);
            result.Write("px;");

            result.Write("padding-right:");
            result.WriteInvariant(element.Padding.Right);
            result.Write("px;");

            result.Write("padding-bottom:");
            result.WriteInvariant(element.Padding.Bottom);
            result.Write("px;");

            result.Write("padding-left:");
            result.WriteInvariant(element.Padding.Left);
            result.Write("px;");

            if (element.TextAlignment != null)
            {
                result.Write("text-align:");
                result.Write(GetTextAligment(element.TextAlignment.Value));
                result.Write(";");
            }
        }
        public static void ApplyInlineStyles(this TextWriter result, PrintElementInline element)
        {
            if (element.TextDecoration != null)
            {
                result.Write("text-decoration:");
                result.Write(GetTextDecoration(element.TextDecoration.Value));
                result.Write(";");
            }
        }

        public static void ApplyParagraphStyles(this TextWriter result, PrintElementParagraph element)
        {
            if (element.IndentSize != null)
            {
                result.Write("text-indent:");
                result.WriteInvariant(element.IndentSize);
                result.Write("px;");
            }
        }

        public static void ApplyListStyles(this TextWriter result, PrintElementList element)
        {
            if (element.MarkerStyle != null)
            {
                result.Write("list-style-type:");
                result.Write(GetMarkerStyle(element.MarkerStyle.Value));
                result.Write(";");
            }
        }

        public static void ApplySubOrSup(this TextWriter result, PrintElement element)
        {
            if (element.Font != null && element.Font.Variant != null)
            {
                switch (element.Font.Variant)
                {
                    case PrintElementFontVariant.Subscript:
                        result.Write("<sub>");
                        break;
                    case PrintElementFontVariant.Superscript:
                        result.Write("<sup>");
                        break;
                }
            }
        }

        public static void ApplySubOrSupSlash(this TextWriter result, PrintElement element)
        {
            if (element.Font != null && element.Font.Variant != null)
            {
                switch (element.Font.Variant)
                {
                    case PrintElementFontVariant.Subscript:
                        result.Write("</sub>");
                        break;
                    case PrintElementFontVariant.Superscript:
                        result.Write("</sup>");
                        break;
                }
            }
        }

        public static void ApplyRowStyles(this TextWriter result, PrintElementTableRow element)
        {
            if (element.Font != null)
            {
                if (!string.IsNullOrWhiteSpace(element.Font.Family))
                {
                    result.Write("font-family:");
                    result.Write(element.Font.Family);
                    result.Write(";");
                }

                if (element.Font.Size != null)
                {
                    result.Write("font-size:");
                    result.WriteInvariant(element.Font.Size);
                    result.Write("px;");
                }

                if (element.Font.Style != null)
                {
                    result.Write("font-style:");
                    result.Write(GetFontStyle(element.Font.Style.Value));
                    result.Write(";");
                }

                if (element.Font.Stretch != null)
                {
                    result.Write("font-stretch:");
                    result.Write(GetFontStretch(element.Font.Stretch.Value));
                    result.Write(";");
                }

                if (element.Font.Weight != null)
                {
                    result.Write("font-weight:");
                    result.Write(GetFontWeight(element.Font.Weight.Value));
                    result.Write(";");
                }
            }

            if (!string.IsNullOrWhiteSpace(element.Background))
            {
                result.Write("background-color:");
                result.Write(element.Background.TryToRgba());
                result.Write(";");
            }

            if (!string.IsNullOrWhiteSpace(element.Foreground))
            {
                result.Write("color:");
                result.Write(element.Foreground.TryToRgba());
                result.Write(";");
            }
        }

        public static void ApplyCellStyles(this TextWriter result, PrintElementTableCell element)
        {
            if (element == null)
            {
                return;
            }

            if (element.Font != null)
            {
                if (!string.IsNullOrWhiteSpace(element.Font.Family))
                {
                    result.Write("font-family:");
                    result.Write(element.Font.Family);
                    result.Write(";");
                }

                if (element.Font.Size != null)
                {
                    result.Write("font-size:");
                    result.WriteInvariant(element.Font.Size);
                    result.Write("px;");
                }

                if (element.Font.Style != null)
                {
                    result.Write("font-style:");
                    result.Write(GetFontStyle(element.Font.Style.Value));
                    result.Write(";");
                }

                if (element.Font.Stretch != null)
                {
                    result.Write("font-stretch:");
                    result.Write(GetFontStretch(element.Font.Stretch.Value));
                    result.Write(";");
                }

                if (element.Font.Weight != null)
                {
                    result.Write("font-weight:");
                    result.Write(GetFontWeight(element.Font.Weight.Value));
                    result.Write(";");
                }
            }

            if (!string.IsNullOrWhiteSpace(element.Background))
            {
                result.Write("background-color:");
                result.Write(element.Background.TryToRgba());
                result.Write(";");
            }

            if (!string.IsNullOrWhiteSpace(element.Foreground))
            {
                result.Write("color:");
                result.Write(element.Foreground.TryToRgba());
                result.Write(";");
            }

            if (element.Border != null)
            {
                result.Write("border-top-width:");
                result.WriteInvariant(element.Border.Thickness.Top);
                result.Write("px;");

                result.Write("border-right-width:");
                result.WriteInvariant(element.Border.Thickness.Right);
                result.Write("px;");

                result.Write("border-bottom-width:");
                result.WriteInvariant(element.Border.Thickness.Bottom);
                result.Write("px;");

                result.Write("border-left-width:");
                result.WriteInvariant(element.Border.Thickness.Left);
                result.Write("px;");

                if (!string.IsNullOrWhiteSpace(element.Border.Color))
                {
                    result.Write("border-style:solid;");

                    result.Write("border-color:");
                    result.Write(element.Border.Color.TryToRgba());
                    result.Write(";");
                }

            }

            result.Write("padding-top:");
            result.WriteInvariant(element.Padding.Top);
            result.Write("px;");

            result.Write("padding-right:");
            result.WriteInvariant(element.Padding.Right);
            result.Write("px;");

            result.Write("padding-bottom:");
            result.WriteInvariant(element.Padding.Bottom);
            result.Write("px;");

            result.Write("padding-left:");
            result.WriteInvariant(element.Padding.Left);
            result.Write("px;");

            if (element.TextAlignment != null)
            {
                result.Write("text-align:");
                result.Write(GetTextAligment(element.TextAlignment.Value));
                result.Write(";");
            }
        }

        public static void ApplyCellProperties(this TextWriter result, PrintElementTableCell element)
        {
            if (element == null)
            {
                return;
            }

            if (element.ColumnSpan != null)
            {
                result.Write("colspan=\"");
                result.Write(element.ColumnSpan);
                result.Write("\" ");
            }

            if (element.RowSpan != null)
            {
                result.Write("rowspan=\"");
                result.Write(element.RowSpan);
                result.Write("\" ");
            }
        }

        public static void ApplyImageStyles(this TextWriter result, PrintElementImage element)
        {
            if (element.Size != null)
            {
                if (element.Size.Width != null)
                {
                    result.Write("width:");
                    result.WriteInvariant(element.Size.Width);
                    result.Write("px;");
                }

                if (element.Size.Height != null)
                {
                    result.Write("height:");
                    result.WriteInvariant(element.Size.Height);
                    result.Write("px;");
                }
            }
        }

        public static void WriteInvariant(this TextWriter writer, object value)
        {
            if (value != null)
            {
                writer.Write(Convert.ToString(value, CultureInfo.InvariantCulture));
            }
        }

        public static string TryToRgba(this string color)
        {
            if (!string.IsNullOrWhiteSpace(color))
            {
                var colorMatch = Regex.Match(color, @"#(?<a>[0-9a-fA-F]{2}){0,1}(?<r>[0-9a-fA-F]{2}){1}(?<g>[0-9a-fA-F]{2}){1}(?<b>[0-9a-fA-F]{2}){1}", RegexOptions.Compiled);

                if (colorMatch.Success && colorMatch.Groups["a"].Success)
                {
                    byte a, r, g, b;

                    if (byte.TryParse(colorMatch.Groups["a"].Value, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out a)
                        && byte.TryParse(colorMatch.Groups["r"].Value, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out r)
                        && byte.TryParse(colorMatch.Groups["g"].Value, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out g)
                        && byte.TryParse(colorMatch.Groups["b"].Value, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out b))
                    {
                        return string.Format(CultureInfo.InvariantCulture, "rgba({0},{1},{2},{3})", r, g, b, a / 255.0);
                    }
                }
            }

            return color;
        }
    }
}