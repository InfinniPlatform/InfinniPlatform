using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using InfinniPlatform.PrintView.Model.Defaults;
using InfinniPlatform.PrintView.Model.Inline;

namespace InfinniPlatform.PrintView.Factories.Inline
{
    internal class PrintImageFactory : PrintElementFactoryBase<PrintImage>
    {
        public override object Create(PrintElementFactoryContext context, PrintImage template)
        {
            var element = CreateImage(context, template);

            FactoryHelper.ApplyElementProperties(element, template, context.ElementStyle);
            FactoryHelper.ApplyInlineProperties(element, template, context.ElementStyle);

            return element;
        }

        private static PrintImage CreateImage(PrintElementFactoryContext context, PrintImage template)
        {
            var element = new PrintImage
            {
                Data = template.Data,
                Size = template.Size,
                Rotation = template.Rotation ?? PrintViewDefaults.Image.Rotation,
                Stretch = template.Stretch ?? PrintViewDefaults.Image.Stretch
            };

            element.Data = GetImageData(context, element.Data);

            if (element.Data != null)
            {
                FactoryHelper.ApplyRotation(element, updateImageSize: false);
            }

            return element;
        }

        private static byte[] GetImageData(PrintElementFactoryContext context, byte[] imageData)
        {
            if (imageData == null)
            {
                imageData = ConvertValueToBytes(context.ElementSourceValue);
            }

            if (imageData == null)
            {
                if (context.IsDesignMode)
                {
                    // В режиме дизайна отображается наименование свойства источника данных или выражение

                    if (!string.IsNullOrEmpty(context.ElementSourceProperty))
                    {
                        imageData = GetImageStubData($"[{context.ElementSourceProperty}]");
                    }
                    else if (!string.IsNullOrEmpty(context.ElementSourceExpression))
                    {
                        imageData = GetImageStubData($"[{context.ElementSourceExpression}]");
                    }
                }
            }

            return imageData;
        }

        private static byte[] GetImageStubData(string imageText)
        {
            using (var image = new Bitmap(200, 200))
            {
                using (var graphics = Graphics.FromImage(image))
                {
                    graphics.Clear(Color.WhiteSmoke);
                    graphics.DrawRectangle(Pens.LightGray, 0, 0, 200, 200);
                    graphics.DrawString(imageText, new Font("Arial", 12), Brushes.Black, 10, 10);
                }

                using (var result = new MemoryStream())
                {
                    image.Save(result, ImageFormat.Png);
                    result.Seek(0, SeekOrigin.Begin);
                    return result.ToArray();
                }
            }
        }

        private static byte[] ConvertValueToBytes(object value)
        {
            var valueAsBytes = value as byte[];

            if (valueAsBytes != null)
            {
                return valueAsBytes;
            }

            var valueAsString = value as string;

            if (valueAsString != null)
            {
                try
                {
                    return Convert.FromBase64String((string)value);
                }
                catch
                {
                }
            }

            return null;
        }
    }
}