using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;

using ImageSharp;
using ImageSharp.Processing;

using InfinniPlatform.PrintView.Abstractions;
using InfinniPlatform.PrintView.Abstractions.Block;
using InfinniPlatform.PrintView.Abstractions.Format;
using InfinniPlatform.PrintView.Abstractions.Inline;

namespace InfinniPlatform.PrintView.Factories
{
    internal static class FactoryHelper
    {
        public static void ApplyElementProperties(PrintElement element, PrintElement template, PrintStyle style)
        {
            element.Style = template.Style;
            element.Font = template.Font ?? style?.Font;
            element.Foreground = template.Foreground ?? style?.Foreground;
            element.Background = template.Background ?? style?.Background;
            element.TextCase = template.TextCase ?? style?.TextCase;
            element.Visibility = template.Visibility;
            element.Source = template.Source;
            element.Expression = template.Expression;
        }

        public static void ApplyBlockProperties(PrintBlock element, PrintBlock template, PrintStyle style)
        {
            element.Border = template.Border ?? style?.Border;
            element.Margin = template.Margin ?? style?.Margin;
            element.Padding = template.Padding ?? style?.Padding;
            element.TextAlignment = template.TextAlignment ?? style?.TextAlignment;
        }

        public static void ApplyInlineProperties(PrintInline element, PrintInline template, PrintStyle style)
        {
            element.TextDecoration = template.TextDecoration ?? style?.TextDecoration;
        }

        public static void ApplyTextCase(object element, PrintTextCase? textCase)
        {
            if (textCase == null)
            {
                return;
            }

            switch (textCase)
            {
                case PrintTextCase.SentenceCase:
                    ForEachRunElements(element, ApplySentenceCase);
                    break;
                case PrintTextCase.Lowercase:
                    ForEachRunElements(element, ApplyLowercase);
                    break;
                case PrintTextCase.Uppercase:
                    ForEachRunElements(element, ApplyUppercase);
                    break;
                case PrintTextCase.ToggleCase:
                    ForEachRunElements(element, ApplyToggleCase);
                    break;
            }
        }

        private static bool ForEachRunElements(object element, Func<PrintRun, bool> action)
        {
            if (element is PrintDocument)
            {
                foreach (var item in ((PrintDocument)element).Blocks.ToArray())
                {
                    if (!ForEachRunElements(item, action))
                    {
                        return false;
                    }
                }
            }
            else if (element is PrintTable)
            {
                foreach (var item in ((PrintTable)element).Rows.SelectMany(i => i.Cells).Select(i => i.Block).ToArray())
                {
                    if (!ForEachRunElements(item, action))
                    {
                        return false;
                    }
                }
            }
            else if (element is PrintTableRow)
            {
                foreach (var item in ((PrintTableRow)element).Cells.Select(i => i.Block).ToArray())
                {
                    if (!ForEachRunElements(item, action))
                    {
                        return false;
                    }
                }
            }
            else if (element is PrintTableCell)
            {
                if (!ForEachRunElements(((PrintTableCell)element).Block, action))
                {
                    return false;
                }
            }
            else if (element is PrintSection)
            {
                foreach (var item in ((PrintSection)element).Blocks.ToArray())
                {
                    if (!ForEachRunElements(item, action))
                    {
                        return false;
                    }
                }
            }
            else if (element is PrintParagraph)
            {
                foreach (var item in ((PrintParagraph)element).Inlines.ToArray())
                {
                    if (!ForEachRunElements(item, action))
                    {
                        return false;
                    }
                }
            }
            else if (element is PrintSpan)
            {
                foreach (var item in ((PrintSpan)element).Inlines.ToArray())
                {
                    if (!ForEachRunElements(item, action))
                    {
                        return false;
                    }
                }
            }
            else if (element is PrintRun)
            {
                return action((PrintRun)element);
            }

            return true;
        }

        private static bool ApplySentenceCase(PrintRun element)
        {
            if (!string.IsNullOrEmpty(element.Text))
            {
                element.Text = char.ToUpper(element.Text[0]) + element.Text.Substring(1);

                return false;
            }

            return true;
        }

        private static bool ApplyLowercase(PrintRun element)
        {
            if (!string.IsNullOrEmpty(element.Text))
            {
                element.Text = element.Text.ToLower();
            }

            return true;
        }

        private static bool ApplyUppercase(PrintRun element)
        {
            if (!string.IsNullOrEmpty(element.Text))
            {
                element.Text = element.Text.ToUpper();
            }

            return true;
        }

        private static bool ApplyToggleCase(PrintRun element)
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

        public static void ApplyRotation(PrintImage imageElement, bool updateImageSize)
        {
            if (imageElement.Rotation == null)
            {
                imageElement.Rotation = PrintImageRotation.Rotate0;
            }

            var rotateType = RotateType.None;

            // Определение способа поворота изображения
            switch (imageElement.Rotation.Value)
            {
                case PrintImageRotation.Rotate0:
                    rotateType = RotateType.None;
                    break;
                case PrintImageRotation.Rotate90:
                    rotateType = RotateType.Rotate90;
                    break;
                case PrintImageRotation.Rotate180:
                    rotateType = RotateType.Rotate180;
                    break;
                case PrintImageRotation.Rotate270:
                    rotateType = RotateType.Rotate270;
                    break;
            }

            // Если нужно определить размер изображения или повернуть его
            if (updateImageSize || rotateType != RotateType.None)
            {
                try
                {
                    // Чтение изображение из потока байт

                    using (var imageStream = new MemoryStream(imageElement.Data))
                    using (var image = new Image(imageStream))
                    {
                        Image<Color> rotatedImage = null;

                        // Если изображение нужно повернуть
                        if (rotateType != RotateType.None)
                        {
                            // Поворот изображения и сброс угла поворота в ноль
                            rotatedImage = image.Rotate(rotateType);
                            imageElement.Rotation = PrintImageRotation.Rotate0;
                        }

                        // Если нужно определить размер изображения
                        if (updateImageSize)
                        {
                            imageElement.Size = (rotatedImage == null)
                                ? new PrintSize(image.Width, image.Height, PrintSizeUnit.Px)
                                : new PrintSize(rotatedImage.Width, rotatedImage.Height, PrintSizeUnit.Px);
                        }

                        // Если изображение было повернуто
                        if (rotatedImage != null)
                        {
                            // Обновление данных изображения
                            imageElement.Data = GetBitmapBytes(rotatedImage);
                        }
                    }
                }
                catch
                {
                }
            }
        }

        public static byte[] GetBitmapBytes(Image<Color> image)
        {
            using (var imageStream = new MemoryStream())
            {
                image.SaveAsPng(imageStream);
                imageStream.Seek(0, SeekOrigin.Begin);
                return imageStream.ToArray();
            }
        }

        public static string FormatValue(PrintElementFactoryContext context, string text, ValueFormat sourceFormat)
        {
            // Если текст не задан, значение берется из источника
            if (string.IsNullOrEmpty(text))
            {
                var sourceValue = context.ElementSourceValue;

                if (sourceValue != null)
                {
                    // Определение функции для форматирования значения
                    var formatFunc = (Func<object, string>)context.Factory.BuildElement(context, sourceFormat) ?? (v => v?.ToString());

                    // Форматирование значения (или значений для коллекции)
                    text = ConvertHelper.ObjectIsCollection(sourceValue)
                        ? string.Join("; ", ((IEnumerable)sourceValue).Cast<object>().Select(formatFunc))
                        : formatFunc(sourceValue);
                }
                else if (context.IsDesignMode)
                {
                    // В режиме дизайна отображается наименование свойства источника данных или выражение

                    if (!string.IsNullOrEmpty(context.ElementSourceProperty))
                    {
                        text = $"[{context.ElementSourceProperty}]";
                    }
                    else if (!string.IsNullOrEmpty(context.ElementSourceExpression))
                    {
                        text = $"[{context.ElementSourceExpression}]";
                    }
                }
            }

            return text;
        }

        public static PrintElementFactoryContext CreateContentContext(this PrintElementFactoryContext context, params PrintThickness[] thicknesses)
        {
            var contentWidth = CalcContentWidth(context.ElementWidth, thicknesses);

            return context.Create(contentWidth);
        }

        public static double CalcContentWidth(double elementWidth, params PrintThickness[] thicknesses)
        {
            var width = elementWidth;

            if (thicknesses != null)
            {
                foreach (var thickness in thicknesses)
                {
                    if (thickness != null)
                    {
                        var left = PrintSizeUnitConverter.ToUnifiedSize(thickness.Left, thickness.SizeUnit);
                        var right = PrintSizeUnitConverter.ToUnifiedSize(thickness.Right, thickness.SizeUnit);

                        width -= (left + right);
                    }
                }
            }

            return Math.Max(width, 0);
        }
    }
}