using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.FlowDocument.Builders.Factories.Inlines
{
    sealed class PrintElementImageFactory : IPrintElementFactory
    {
        public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            var element = CreateImage(buildContext, elementMetadata);

            BuildHelper.ApplyTextProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyTextProperties(element, elementMetadata);

            BuildHelper.ApplyInlineProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyInlineProperties(element, elementMetadata);

            return element;
        }

        private static PrintElementImage CreateImage(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            var imageStream = GetImageDataStream(buildContext, elementMetadata.Data);
            if (imageStream != null)
            {
                return ApplyImageData(imageStream, elementMetadata);
            }
            return null;
        }

        private static PrintElementImage ApplyImageData(Stream imageStream, dynamic elementMetadata)
        {
            try
            {
                imageStream = ApplyRotation(imageStream, elementMetadata.Rotation);
                var image = new PrintElementImage(new Bitmap(imageStream));
                ApplySize(image, elementMetadata.Size);
                ApplyStretch(image, elementMetadata.Stretch);
                return image;
            }
            catch
            {
            }
            return null;
        }

        private static Stream GetImageDataStream(PrintElementBuildContext buildContext, dynamic imageData)
        {
            Stream result = null;

            byte[] imageDataBytes;

            // Если данные не заданы, они берутся из источника
            if (!ConvertHelper.TryToBytes(imageData, out imageDataBytes) || imageDataBytes == null)
            {
                if (!ConvertHelper.TryToBytes(buildContext.ElementSourceValue, out imageDataBytes) || imageDataBytes == null)
                {
                    if (buildContext.IsDesignMode)
                    {
                        // В режиме дизайна отображается наименование свойства источника данных или выражение

                        if (!string.IsNullOrEmpty(buildContext.ElementSourceProperty))
                        {
                            result = GetImageDataStreamStub($"[{buildContext.ElementSourceProperty}]");
                        }
                        else if (!string.IsNullOrEmpty(buildContext.ElementSourceExpression))
                        {
                            result = GetImageDataStreamStub(string.Format("[{0}]", buildContext.ElementSourceExpression));
                        }
                    }
                }
                else
                {
                    result = new MemoryStream(imageDataBytes);
                }
            }
            else
            {
                result = new MemoryStream(imageDataBytes);
            }

            return result;
        }

        private static Stream GetImageDataStreamStub(string imageText)
        {
            var result = new MemoryStream();

            using (var image = new Bitmap(200, 200))
            {
                using (var graphics = Graphics.FromImage(image))
                {
                    graphics.Clear(Color.WhiteSmoke);
                    graphics.DrawRectangle(Pens.LightGray, 0, 0, 200, 200);
                    graphics.DrawString(imageText, new Font("Arial", 12), Brushes.Black, 10, 10);
                }

                image.Save(result, ImageFormat.Png);
                result.Seek(0, SeekOrigin.Begin);
            }

            return result;
        }

        private static Stream ApplyRotation(Stream bitmap, dynamic rotation)
        {
            string rotationString;

            if (ConvertHelper.TryToNormString(rotation, out rotationString))
            {
                switch (rotationString)
                {
                    case "rotate90":
                        return RotateImage(bitmap, RotateFlipType.Rotate90FlipNone);
                    case "rotate180":
                        return RotateImage(bitmap, RotateFlipType.Rotate180FlipNone);
                    case "rotate270":
                        return RotateImage(bitmap, RotateFlipType.Rotate270FlipNone);
                }
            }

            return bitmap;
        }

        private static Stream RotateImage(Stream image, RotateFlipType rotation)
        {
            try
            {
                using (var bitmap = new Bitmap(image))
                {
                    bitmap.RotateFlip(rotation);

                    var result = new MemoryStream();
                    bitmap.Save(result, ImageFormat.Png);
                    result.Seek(0, SeekOrigin.Begin);

                    return result;
                }
            }
            catch
            {
                return image;
            }
        }
        private static void ApplySize(PrintElementImage image, dynamic size)
        {
            if (size != null)
            {
                PrintElementSize imageSize = null;

                double width;

                if (BuildHelper.TryToSizeInPixels(size.Width, size.SizeUnit, out width))
                {
                    imageSize = imageSize ?? new PrintElementSize();
                    imageSize.Width = width;
                }

                double height;

                if (BuildHelper.TryToSizeInPixels(size.Height, size.SizeUnit, out height))
                {
                    imageSize = imageSize ?? new PrintElementSize();
                    imageSize.Height = height;
                }

                image.Size = imageSize;
            }
        }

        private static void ApplyStretch(PrintElementImage image, dynamic stretch)
        {
            string stretchString;

            if (ConvertHelper.TryToNormString(stretch, out stretchString))
            {
                switch (stretchString)
                {
                    case "none":
                        image.Stretch = PrintElementStretch.None;
                        break;
                    case "fill":
                        image.Stretch = PrintElementStretch.Fill;
                        break;
                    case "uniform":
                        image.Stretch = PrintElementStretch.Uniform;
                        break;
                }
            }
        }
    }
}