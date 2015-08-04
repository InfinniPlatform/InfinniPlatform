using System;
using System.IO;
using System.Text;

using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.Model.Blocks;
using InfinniPlatform.FlowDocument.Model.Font;
using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    static class HtmlBuilderHelper
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

            return "";
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

            return "";
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

            return "";
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

            return "";
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

            return "";
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

            return "";
        }
        public static StringBuilder ApplyBaseStyles(this StringBuilder result, PrintElement element)
        {
            if (element.Font != null)
            {
                result.Append("font-family:");
                result.Append(element.Font.Family);
                result.Append(";");

                if (element.Font.Size != null)
                {
                    result.Append("font-size:");
                    result.Append(element.Font.Size);
                    result.Append("px;");
                }

                if (element.Font.Style != null)
                {
                    result.Append("font-style:");
                    result.Append(GetFontStyle(element.Font.Style.Value));
                    result.Append(";");
                }

                if (element.Font.Stretch != null)
                {
                    result.Append("font-stretch:");
                    result.Append(GetFontStretch(element.Font.Stretch.Value));
                    result.Append(";");
                }

                if (element.Font.Weight != null)
                {
                    result.Append("font-weight:");
                    result.Append(GetFontWeight(element.Font.Weight.Value));
                    result.Append(";");
                }
            }

            if (!string.IsNullOrWhiteSpace(element.Background))
            {
                result.Append("background-color:");
                result.Append(element.Background);
                result.Append(";");
            }

            if (!string.IsNullOrWhiteSpace(element.Foreground))
            {
                result.Append("color:");
                result.Append(element.Foreground);
                result.Append(";");
            }

            return result;
        }

        public static StringBuilder ApplyBlockStyles(this StringBuilder result, PrintElementBlock element)
        {
            if (element.Border != null)
            {
                result.Append("border-top-width:");
                result.Append(element.Border.Thickness.Top);
                result.Append("px;");

                result.Append("border-right-width:");
                result.Append(element.Border.Thickness.Right);
                result.Append("px;");

                result.Append("border-bottom-width:");
                result.Append(element.Border.Thickness.Bottom);
                result.Append("px;");

                result.Append("border-left-width:");
                result.Append(element.Border.Thickness.Left);
                result.Append("px;");

                if (!string.IsNullOrWhiteSpace(element.Border.Color))
                {
                    result.Append("border-style:solid;");

                    result.Append("border-color:");
                    result.Append(element.Border.Color);
                    result.Append(";");
                }

            }

            result.Append("margin-top:");
            result.Append(element.Margin.Top);
            result.Append("px;");

            result.Append("margin-right:");
            result.Append(element.Margin.Right);
            result.Append("px;");

            result.Append("margin-bottom:");
            result.Append(element.Margin.Bottom);
            result.Append("px;");

            result.Append("margin-left:");
            result.Append(element.Margin.Left);
            result.Append("px;");

            result.Append("padding-top:");
            result.Append(element.Padding.Top);
            result.Append("px;");

            result.Append("padding-right:");
            result.Append(element.Padding.Right);
            result.Append("px;");

            result.Append("padding-bottom:");
            result.Append(element.Padding.Bottom);
            result.Append("px;");

            result.Append("padding-left:");
            result.Append(element.Padding.Left);
            result.Append("px;");

            if (element.TextAlignment != null)
            {
                result.Append("text-align:");
                result.Append(GetTextAligment(element.TextAlignment.Value));
                result.Append(";");
            }

            return result;
        }
        public static StringBuilder ApplyInlineStyles(this StringBuilder result, PrintElementInline element)
        {
            if (element.TextDecoration != null)
            {
                result.Append("text-decoration:");
                result.Append(GetTextDecoration(element.TextDecoration.Value));
                result.Append(";");
            }

            return result;
        }

        public static StringBuilder ApplyParagraphStyles(this StringBuilder result, PrintElementParagraph element)
        {
            if (element.IndentSize != null)
            {
                result.Append("text-indent:");
                result.Append(element.IndentSize);
                result.Append("px;");
            }

            return result;
        }

        public static StringBuilder ApplyListStyles(this StringBuilder result, PrintElementList element)
        {
            //startindex todo

            if (element.MarkerStyle != null)
            {
                result.Append("list-style-type:");
                result.Append(GetMarkerStyle(element.MarkerStyle.Value));
                result.Append(";");
            }

            return result;
        }

        public static StringBuilder ApplySubOrSup(this StringBuilder result, PrintElement element)
        {
            if (element.Font != null)
            {
                if (element.Font.Variant != null)
                {
                    switch (element.Font.Variant)
                    {
                        case PrintElementFontVariant.Subscript:
                            result.Append("<sub>");
                            break;
                        case PrintElementFontVariant.Superscript:
                            result.Append("<sup>");
                            break;
                    }
                }
            }

            return result;
        }

        public static StringBuilder ApplySubOrSupSlash(this StringBuilder result, PrintElement element)
        {
            if (element.Font != null)
            {
                if (element.Font.Variant != null)
                {
                    switch (element.Font.Variant)
                    {
                        case PrintElementFontVariant.Subscript:
                            result.Append("</sub>");
                            break;
                        case PrintElementFontVariant.Superscript:
                            result.Append("</sup>");
                            break;
                    }
                }
            }

            return result;
        }

        public static StringBuilder ApplyRowStyles(this StringBuilder result, PrintElementTableRow element)
        {
            if (element.Font != null)
            {
                result.Append("font-family:");
                result.Append(element.Font.Family);
                result.Append(";");

                if (element.Font.Size != null)
                {
                    result.Append("font-size:");
                    result.Append(element.Font.Size);
                    result.Append("px;");
                }

                if (element.Font.Style != null)
                {
                    result.Append("font-style:");
                    result.Append(GetFontStyle(element.Font.Style.Value));
                    result.Append(";");
                }

                if (element.Font.Stretch != null)
                {
                    result.Append("font-stretch:");
                    result.Append(GetFontStretch(element.Font.Stretch.Value));
                    result.Append(";");
                }

                if (element.Font.Weight != null)
                {
                    result.Append("font-weight:");
                    result.Append(GetFontWeight(element.Font.Weight.Value));
                    result.Append(";");
                }
            }

            if (!string.IsNullOrWhiteSpace(element.Background))
            {
                result.Append("background-color:");
                result.Append(element.Background);
                result.Append(";");
            }

            if (!string.IsNullOrWhiteSpace(element.Foreground))
            {
                result.Append("color:");
                result.Append(element.Foreground);
                result.Append(";");
            }

            return result;
        }

        public static StringBuilder ApplyCellStyles(this StringBuilder result, PrintElementTableCell element)
        {
            if (element.Font != null)
            {
                result.Append("font-family:");
                result.Append(element.Font.Family);
                result.Append(";");

                if (element.Font.Size != null)
                {
                    result.Append("font-size:");
                    result.Append(element.Font.Size);
                    result.Append("px;");
                }

                if (element.Font.Style != null)
                {
                    result.Append("font-style:");
                    result.Append(GetFontStyle(element.Font.Style.Value));
                    result.Append(";");
                }

                if (element.Font.Stretch != null)
                {
                    result.Append("font-stretch:");
                    result.Append(GetFontStretch(element.Font.Stretch.Value));
                    result.Append(";");
                }

                if (element.Font.Weight != null)
                {
                    result.Append("font-weight:");
                    result.Append(GetFontWeight(element.Font.Weight.Value));
                    result.Append(";");
                }
            }

            if (!string.IsNullOrWhiteSpace(element.Background))
            {
                result.Append("background-color:");
                result.Append(element.Background);
                result.Append(";");
            }

            if (!string.IsNullOrWhiteSpace(element.Foreground))
            {
                result.Append("color:");
                result.Append(element.Foreground);
                result.Append(";");
            }

            if (element.Border != null)
            {
                result.Append("border-top-width:");
                result.Append(element.Border.Thickness.Top);
                result.Append("px;");

                result.Append("border-right-width:");
                result.Append(element.Border.Thickness.Right);
                result.Append("px;");

                result.Append("border-bottom-width:");
                result.Append(element.Border.Thickness.Bottom);
                result.Append("px;");

                result.Append("border-left-width:");
                result.Append(element.Border.Thickness.Left);
                result.Append("px;");

                if (!string.IsNullOrWhiteSpace(element.Border.Color))
                {
                    result.Append("border-style:solid;");

                    result.Append("border-color:");
                    result.Append(element.Border.Color);
                    result.Append(";");
                }

            }

            result.Append("padding-top:");
            result.Append(element.Padding.Top);
            result.Append("px;");

            result.Append("padding-right:");
            result.Append(element.Padding.Right);
            result.Append("px;");

            result.Append("padding-bottom:");
            result.Append(element.Padding.Bottom);
            result.Append("px;");

            result.Append("padding-left:");
            result.Append(element.Padding.Left);
            result.Append("px;");

            if (element.TextAlignment != null)
            {
                result.Append("text-align:");
                result.Append(GetTextAligment(element.TextAlignment.Value));
                result.Append(";");
            }

            return result;
        }

        public static StringBuilder ApplyCellProperties(this StringBuilder result, PrintElementTableCell element)
        {
            if (element.ColumnSpan != null)
            {
                result.Append("colspan=\"");
                result.Append(element.ColumnSpan);
                result.Append("\" ");
            }

            if (element.RowSpan != null)
            {
                result.Append("rowspan=\"");
                result.Append(element.RowSpan);
                result.Append("\" ");
            }

            return result;
        }

        public static StringBuilder StreamToBase64(this StringBuilder result, Stream stream)
        {
            if (stream != null && stream.CanRead)
            {
                using (var memory = new MemoryStream())
                {
                    stream.CopyTo(memory);

                    result.Append(Convert.ToBase64String(memory.ToArray()));
                }
            }

            return result;
        }

        public static StringBuilder ApplyImageStyles(this StringBuilder result, PrintElementImage element)
        {
            if (element.Size != null)
            {
                if (element.Size.Width != null)
                {
                    result.Append("width:");
                    result.Append(element.Size.Width);
                    result.Append("px;");
                }

                if (element.Size.Height != null)
                {
                    result.Append("height:");
                    result.Append(element.Size.Height);
                    result.Append("px;");
                }
            }

            return result;
        }
    }
}
