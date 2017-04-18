using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

using InfinniPlatform.PrintView.Abstractions;
using InfinniPlatform.PrintView.Abstractions.Block;

namespace InfinniPlatform.PrintView.Writers.Html
{
    internal static class HtmlBuilderHelper
    {
        private static readonly HtmlEnumCache EnumCache;


        static HtmlBuilderHelper()
        {
            EnumCache = new HtmlEnumCache();

            EnumCache.AddValue(PrintSizeUnit.Pt, "pt")
                     .AddValue(PrintSizeUnit.Px, "px")
                     .AddValue(PrintSizeUnit.In, "in")
                     .AddValue(PrintSizeUnit.Cm, "cm")
                     .AddValue(PrintSizeUnit.Mm, "mm");

            EnumCache.AddValue(PrintFontStyle.Normal, "normal")
                     .AddValue(PrintFontStyle.Italic, "italic")
                     .AddValue(PrintFontStyle.Oblique, "oblique");

            EnumCache.AddValue(PrintFontStretch.Normal, "normal")
                     .AddValue(PrintFontStretch.UltraCondensed, "ultra-condensed")
                     .AddValue(PrintFontStretch.ExtraCondensed, "extra-condensed")
                     .AddValue(PrintFontStretch.Condensed, "condensed")
                     .AddValue(PrintFontStretch.SemiCondensed, "semi-condensed")
                     .AddValue(PrintFontStretch.SemiExpanded, "semi-expanded")
                     .AddValue(PrintFontStretch.Expanded, "expanded")
                     .AddValue(PrintFontStretch.ExtraExpanded, "extra-expanded")
                     .AddValue(PrintFontStretch.UltraExpanded, "ultra-expanded");

            EnumCache.AddValue(PrintFontWeight.Normal, "400")
                     .AddValue(PrintFontWeight.UltraLight, "100")
                     .AddValue(PrintFontWeight.ExtraLight, "200")
                     .AddValue(PrintFontWeight.Light, "300")
                     .AddValue(PrintFontWeight.Medium, "500")
                     .AddValue(PrintFontWeight.SemiBold, "600")
                     .AddValue(PrintFontWeight.Bold, "700")
                     .AddValue(PrintFontWeight.ExtraBold, "800")
                     .AddValue(PrintFontWeight.UltraBold, "900");

            EnumCache.AddValue(PrintTextAlignment.Left, "left")
                     .AddValue(PrintTextAlignment.Center, "center")
                     .AddValue(PrintTextAlignment.Right, "right")
                     .AddValue(PrintTextAlignment.Justify, "justify");

            EnumCache.AddValue(PrintListMarkerStyle.None, "none")
                     .AddValue(PrintListMarkerStyle.Disc, "disk")
                     .AddValue(PrintListMarkerStyle.Circle, "circle")
                     .AddValue(PrintListMarkerStyle.Square, "square")
                     .AddValue(PrintListMarkerStyle.Box, "box")
                     .AddValue(PrintListMarkerStyle.LowerRoman, "lower-roman")
                     .AddValue(PrintListMarkerStyle.UpperRoman, "upper-roman")
                     .AddValue(PrintListMarkerStyle.LowerLatin, "lower-latin")
                     .AddValue(PrintListMarkerStyle.UpperLatin, "upper-latin")
                     .AddValue(PrintListMarkerStyle.Decimal, "decimal");

            EnumCache.AddValue(PrintTextDecoration.Normal, null)
                     .AddValue(PrintTextDecoration.OverLine, "overline")
                     .AddValue(PrintTextDecoration.Strikethrough, "line-through")
                     .AddValue(PrintTextDecoration.Underline, "underline");
        }


        public static void WriteSizeAttribute(this TextWriter writer, string attribute, double? size, PrintSizeUnit? sizeUnit)
        {
            if (size != null)
            {
                writer.Write(" ");
                writer.Write(attribute);
                writer.Write("=\"");
                writer.WriteInvariant(size);
                writer.WriteEnumValue(sizeUnit);
                writer.Write("\"");
            }
        }

        public static void WriteSizeProperty(this TextWriter writer, string property, double? size, PrintSizeUnit? sizeUnit)
        {
            if (size != null)
            {
                // По умолчанию используется абсолютная единица измерения - pt, поскольку документ создается для печати

                writer.Write(property);
                writer.Write(':');
                writer.WriteInvariant(size);
                writer.WriteEnumValue(sizeUnit ?? PrintSizeUnit.Pt);
                writer.Write(';');
            }
        }

        public static void WriteEnumProperty<TEnum>(this TextWriter writer, string property, TEnum value)
        {
            if (value != null)
            {
                writer.Write(property);
                writer.Write(':');
                writer.WriteEnumValue(value);
                writer.Write(';');
            }
        }

        public static void WriteStringProperty(this TextWriter writer, string property, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                writer.Write(property);
                writer.Write(':');
                writer.Write(value);
                writer.Write(';');
            }
        }

        public static void WriteColorProperty(this TextWriter writer, string property, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var colorMatch = Regex.Match(value, @"#(?<a>[0-9a-fA-F]{2}){0,1}(?<r>[0-9a-fA-F]{2}){1}(?<g>[0-9a-fA-F]{2}){1}(?<b>[0-9a-fA-F]{2}){1}", RegexOptions.Compiled);

                if (colorMatch.Success && colorMatch.Groups["a"].Success)
                {
                    byte a, r, g, b;

                    if (byte.TryParse(colorMatch.Groups["a"].Value, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out a)
                        && byte.TryParse(colorMatch.Groups["r"].Value, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out r)
                        && byte.TryParse(colorMatch.Groups["g"].Value, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out g)
                        && byte.TryParse(colorMatch.Groups["b"].Value, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out b))
                    {
                        value = string.Format(CultureInfo.InvariantCulture, "rgba({0},{1},{2},{3})", r, g, b, a / 255.0);
                    }
                }

                writer.WriteStringProperty(property, value);
            }
        }

        private static void WriteEnumValue<TEnum>(this TextWriter writer, TEnum value)
        {
            if (value != null)
            {
                var valueString = EnumCache.GetValue(value);

                if (valueString != null)
                {
                    writer.Write(valueString);
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


        public static void ApplyElementStyles(this TextWriter writer, PrintElement element)
        {
            writer.WriteFont(element.Font);
            writer.WriteForeground(element.Foreground);
            writer.WriteBackground(element.Background);
        }

        public static void ApplyBlockStyles(this TextWriter writer, PrintBlock element)
        {
            writer.WriteBorder(element.Border);
            writer.WriteMargin(element.Margin);
            writer.WritePadding(element.Padding);
            writer.WriteTextAlignment(element.TextAlignment);
        }

        public static void ApplyInlineStyles(this TextWriter result, PrintInline element)
        {
            result.WriteEnumProperty("text-decoration", element.TextDecoration);
        }


        public static void ApplySubOrSup(this TextWriter writer, PrintElement element)
        {
            if (element.Font?.Variant != null)
            {
                switch (element.Font.Variant.Value)
                {
                    case PrintFontVariant.Subscript:
                        writer.Write("<sub>");
                        break;
                    case PrintFontVariant.Superscript:
                        writer.Write("<sup>");
                        break;
                }
            }
        }

        public static void ApplySubOrSupSlash(this TextWriter writer, PrintElement element)
        {
            if (element.Font?.Variant != null)
            {
                switch (element.Font.Variant.Value)
                {
                    case PrintFontVariant.Subscript:
                        writer.Write("</sub>");
                        break;
                    case PrintFontVariant.Superscript:
                        writer.Write("</sup>");
                        break;
                }
            }
        }


        public static void WriteFont(this TextWriter writer, PrintFont font)
        {
            if (font != null)
            {
                writer.WriteStringProperty("font-family", font.Family);
                writer.WriteSizeProperty("font-size", font.Size, font.SizeUnit);
                writer.WriteEnumProperty("font-style", font.Style);
                writer.WriteEnumProperty("font-stretch", font.Stretch);
                writer.WriteEnumProperty("font-weight", font.Weight);
            }
        }

        public static void WriteForeground(this TextWriter writer, string foreground)
        {
            writer.WriteColorProperty("color", foreground);
        }

        public static void WriteBackground(this TextWriter writer, string background)
        {
            writer.WriteColorProperty("background-color", background);
        }

        public static void WriteBorder(this TextWriter writer, PrintBorder border)
        {
            if (border != null)
            {
                var borderThickness = border.Thickness;

                if (borderThickness != null)
                {
                    var borderSizeUnit = borderThickness.SizeUnit;

                    writer.WriteSizeProperty("border-left-width", borderThickness.Left, borderSizeUnit);
                    writer.WriteSizeProperty("border-top-width", borderThickness.Top, borderSizeUnit);
                    writer.WriteSizeProperty("border-right-width", borderThickness.Right, borderSizeUnit);
                    writer.WriteSizeProperty("border-bottom-width", borderThickness.Bottom, borderSizeUnit);
                }

                var borderColor = border.Color;

                if (!string.IsNullOrWhiteSpace(borderColor))
                {
                    writer.Write("border-style:solid;");
                    writer.WriteColorProperty("border-color", borderColor);
                }
            }
        }

        public static void WriteMargin(this TextWriter writer, PrintThickness margin)
        {
            if (margin != null)
            {
                var marginSizeUnit = margin.SizeUnit;

                writer.WriteSizeProperty("margin-left", margin.Left, marginSizeUnit);
                writer.WriteSizeProperty("margin-top", margin.Top, marginSizeUnit);
                writer.WriteSizeProperty("margin-right", margin.Right, marginSizeUnit);
                writer.WriteSizeProperty("margin-bottom", margin.Bottom, marginSizeUnit);
            }
        }

        public static void WritePadding(this TextWriter writer, PrintThickness padding)
        {
            if (padding != null)
            {
                var paddingSizeUnit = padding.SizeUnit;

                writer.WriteSizeProperty("padding-left", padding.Left, paddingSizeUnit);
                writer.WriteSizeProperty("padding-top", padding.Top, paddingSizeUnit);
                writer.WriteSizeProperty("padding-right", padding.Right, paddingSizeUnit);
                writer.WriteSizeProperty("padding-bottom", padding.Bottom, paddingSizeUnit);
            }
        }

        public static void WriteTextAlignment(this TextWriter writer, PrintTextAlignment? textAlignment)
        {
            writer.WriteEnumProperty("text-align", textAlignment);
        }
    }
}