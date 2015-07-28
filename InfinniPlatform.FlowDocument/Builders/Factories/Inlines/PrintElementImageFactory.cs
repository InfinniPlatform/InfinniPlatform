using System.Drawing;
using System.Globalization;
using System.IO;
using Image = InfinniPlatform.FlowDocument.Model.Inlines.Image;

//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;

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

        private static Image CreateImage(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            var imageData = GetImageDataStream(buildContext, elementMetadata.Data);
            if (imageData != null)
            {
                imageData = ApplyImageData(imageData, buildContext, elementMetadata);
            }
            return null;
        }

        private static Stream ApplyImageData(Stream imageStream, PrintElementBuildContext buildContext, dynamic elementMetadata)
        {

            try
            {
                // var b = new Bitmap(element);
                // b.Rotate();
                // b.Resize();
                // b.Stretch();
                // b.Save();


                imageStream = ApplyRotation(imageStream, elementMetadata.Rotation);
                ApplySize(imageStream, elementMetadata.Size);
                ApplyStretch(imageStream, elementMetadata.Stretch);
            }
            catch
            {
            }
            return imageStream;
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
                            result = GetImageDataStreamStub(string.Format("[{0}]", buildContext.ElementSourceProperty));
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
            var formattedText = new FormattedText(imageText, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 12, Brushes.Black)
                                {
                                    MaxTextWidth = 180,
                                    MaxTextHeight = 180,
                                    Trimming = TextTrimming.None,
                                    TextAlignment = TextAlignment.Center
                                };

            var drawingVisual = new DrawingVisual();
            var drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawRectangle(Brushes.WhiteSmoke, new Pen(Brushes.LightGray, 1), new Rect(0, 0, 200, 200));
            drawingContext.DrawText(formattedText, new Point(10, 10));
            drawingContext.Close();

            var renderTargetBitmap = new RenderTargetBitmap(200, 200, 96, 96, PixelFormats.Default);
            renderTargetBitmap.Render(drawingVisual);

            var bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            var bitmapImageStream = new MemoryStream();
            bitmapEncoder.Save(bitmapImageStream);

            return bitmapImageStream;
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
                    bitmap.Save(result, bitmap.RawFormat);

                    return result;
                }
            }
            catch
            {
                return image;
            }
        }
        private static void ApplySize(Image image, dynamic size)
        {
            if (size != null)
            {
                double width, height;

                if (BuildHelper.TryToSizeInPixels(size.Width, size.SizeUnit, out width))
                {
                    image.Width = width;
                }

                if (BuildHelper.TryToSizeInPixels(size.Height, size.SizeUnit, out height))
                {
                    image.Height = height;
                }
            }
        }

        private static void ApplyStretch(Image image, dynamic stretch)
        {
            string stretchString;

            if (ConvertHelper.TryToNormString(stretch, out stretchString))
            {
                switch (stretchString)
                {
                    case "none":
                        image.Stretch = Stretch.None;
                        break;
                    case "fill":
                        image.Stretch = Stretch.Fill;
                        break;
                    case "uniform":
                        image.Stretch = Stretch.Uniform;
                        break;
                }
            }
        }
    }
}