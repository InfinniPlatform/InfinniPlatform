using System;
using System.IO;

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
        public static void ApplyBaseStyles(this TextWriter result, PrintElement element)
        {
            if (element.Font != null)
            {
                result.Write("font-family:");
                result.Write(element.Font.Family);
                result.Write(";");

                if (element.Font.Size != null)
                {
                    result.Write("font-size:");
                    result.Write(element.Font.Size);
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
                result.Write(element.Background);
                result.Write(";");
            }

            if (!string.IsNullOrWhiteSpace(element.Foreground))
            {
                result.Write("color:");
                result.Write(element.Foreground);
                result.Write(";");
            }
        }

        public static void ApplyBlockStyles(this TextWriter result, PrintElementBlock element)
        {
            if (element.Border != null)
            {
                result.Write("border-top-width:");
                result.Write(element.Border.Thickness.Top);
                result.Write("px;");
                       
                result.Write("border-right-width:");
                result.Write(element.Border.Thickness.Right);
                result.Write("px;");
                       
                result.Write("border-bottom-width:");
                result.Write(element.Border.Thickness.Bottom);
                result.Write("px;");
                       
                result.Write("border-left-width:");
                result.Write(element.Border.Thickness.Left);
                result.Write("px;");

                if (!string.IsNullOrWhiteSpace(element.Border.Color))
                {
                    result.Write("border-style:solid;");
                           
                    result.Write("border-color:");
                    result.Write(element.Border.Color);
                    result.Write(";");
                }

            }

            result.Write("margin-top:");
            result.Write(element.Margin.Top);
            result.Write("px;");
                   
            result.Write("margin-right:");
            result.Write(element.Margin.Right);
            result.Write("px;");
                   
            result.Write("margin-bottom:");
            result.Write(element.Margin.Bottom);
            result.Write("px;");
                   
            result.Write("margin-left:");
            result.Write(element.Margin.Left);
            result.Write("px;");
                   
            result.Write("padding-top:");
            result.Write(element.Padding.Top);
            result.Write("px;");
                   
            result.Write("padding-right:");
            result.Write(element.Padding.Right);
            result.Write("px;");
                   
            result.Write("padding-bottom:");
            result.Write(element.Padding.Bottom);
            result.Write("px;");
                   
            result.Write("padding-left:");
            result.Write(element.Padding.Left);
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
                result.Write(element.IndentSize);
                result.Write("px;");
            }
        }

        public static void ApplyListStyles(this TextWriter result, PrintElementList element)
        {
            //startindex todo

            if (element.MarkerStyle != null)
            {
                result.Write("list-style-type:");
                result.Write(GetMarkerStyle(element.MarkerStyle.Value));
                result.Write(";");
            }
        }

        public static void ApplySubOrSup(this TextWriter result, PrintElement element)
        {
            if (element.Font != null)
            {
                if (element.Font.Variant != null)
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
        }

        public static void ApplySubOrSupSlash(this TextWriter result, PrintElement element)
        {
            if (element.Font != null)
            {
                if (element.Font.Variant != null)
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
        }

        public static void ApplyRowStyles(this TextWriter result, PrintElementTableRow element)
        {
            if (element.Font != null)
            {
                result.Write("font-family:");
                result.Write(element.Font.Family);
                result.Write(";");

                if (element.Font.Size != null)
                {
                    result.Write("font-size:");
                    result.Write(element.Font.Size);
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
                result.Write(element.Background);
                result.Write(";");
            }

            if (!string.IsNullOrWhiteSpace(element.Foreground))
            {
                result.Write("color:");
                result.Write(element.Foreground);
                result.Write(";");
            }
        }

        public static void ApplyCellStyles(this TextWriter result, PrintElementTableCell element)
        {
            if (element.Font != null)
            {
                result.Write("font-family:");
                result.Write(element.Font.Family);
                result.Write(";");

                if (element.Font.Size != null)
                {
                    result.Write("font-size:");
                    result.Write(element.Font.Size);
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
                result.Write(element.Background);
                result.Write(";");
            }

            if (!string.IsNullOrWhiteSpace(element.Foreground))
            {
                result.Write("color:");
                result.Write(element.Foreground);
                result.Write(";");
            }

            if (element.Border != null)
            {
                result.Write("border-top-width:");
                result.Write(element.Border.Thickness.Top);
                result.Write("px;");
                       
                result.Write("border-right-width:");
                result.Write(element.Border.Thickness.Right);
                result.Write("px;");
                       
                result.Write("border-bottom-width:");
                result.Write(element.Border.Thickness.Bottom);
                result.Write("px;");
                       
                result.Write("border-left-width:");
                result.Write(element.Border.Thickness.Left);
                result.Write("px;");

                if (!string.IsNullOrWhiteSpace(element.Border.Color))
                {
                    result.Write("border-style:solid;");
                           
                    result.Write("border-color:");
                    result.Write(element.Border.Color);
                    result.Write(";");
                }

            }

            result.Write("padding-top:");
            result.Write(element.Padding.Top);
            result.Write("px;");
                   
            result.Write("padding-right:");
            result.Write(element.Padding.Right);
            result.Write("px;");
                   
            result.Write("padding-bottom:");
            result.Write(element.Padding.Bottom);
            result.Write("px;");
                   
            result.Write("padding-left:");
            result.Write(element.Padding.Left);
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

        public static void StreamToBase64(this TextWriter result, Stream stream)
        {
            if (stream != null && stream.CanRead)
            {
                using (var memory = new MemoryStream())
                {
                    stream.CopyTo(memory);

                    result.Write(Convert.ToBase64String(memory.ToArray()));
                }
            }
        }

        public static void ApplyImageStyles(this TextWriter result, PrintElementImage element)
        {
            if (element.Size != null)
            {
                if (element.Size.Width != null)
                {
                    result.Write("width:");
                    result.Write(element.Size.Width);
                    result.Write("px;");
                }

                if (element.Size.Height != null)
                {
                    result.Write("height:");
                    result.Write(element.Size.Height);
                    result.Write("px;");
                }
            }
        }
    }
}
