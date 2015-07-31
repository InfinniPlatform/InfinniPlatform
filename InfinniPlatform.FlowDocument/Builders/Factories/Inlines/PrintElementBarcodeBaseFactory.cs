using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using FastReport;
using FastReport.Barcode;
using FastReport.Utils;
using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.FlowDocument.Builders.Factories.Inlines
{
    abstract class PrintElementBarcodeBaseFactory : IPrintElementFactory
    {
        public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            PrintElementImage element = CreateBarcodeImage(buildContext, elementMetadata);

            BuildHelper.ApplyTextProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyTextProperties(element, elementMetadata);

            BuildHelper.ApplyInlineProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyInlineProperties(element, elementMetadata);

            return element;
        }

        private PrintElementImage CreateBarcodeImage(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            var imageSize = new PrintElementSize();
            var imageStream = CreateBarcodeImageStream(buildContext, elementMetadata, imageSize);

            try
            {
                imageStream = ApplyRotation(imageStream, elementMetadata.Rotation, imageSize);
                return new PrintElementImage(imageStream) { Size = imageSize };
            }
            catch
            {
            }

            return null;
        }

        private static Stream ApplyRotation(Stream bitmap, dynamic rotation, PrintElementSize imageSize)
        {
            string rotationString;

            if (ConvertHelper.TryToNormString(rotation, out rotationString))
            {
                switch (rotationString)
                {
                    case "rotate90":
                        return RotateImage(bitmap, RotateFlipType.Rotate90FlipNone, imageSize);
                    case "rotate180":
                        return RotateImage(bitmap, RotateFlipType.Rotate180FlipNone, imageSize);
                    case "rotate270":
                        return RotateImage(bitmap, RotateFlipType.Rotate270FlipNone, imageSize);
                }
            }

            return bitmap;
        }

        private static Stream RotateImage(Stream image, RotateFlipType rotation, PrintElementSize imageSize)
        {
            try
            {
                using (var bitmap = new Bitmap(image))
                {
                    bitmap.RotateFlip(rotation);

                    imageSize.Width = bitmap.Width;
                    imageSize.Height = bitmap.Height;

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

        private Stream CreateBarcodeImageStream(PrintElementBuildContext buildContext, dynamic elementMetadata, PrintElementSize imageSize)
        {
            string textSting = BuildHelper.FormatValue(buildContext, elementMetadata.Text, elementMetadata.SourceFormat);

            textSting = PrepareText(textSting);

            if (!string.IsNullOrEmpty(textSting))
            {
                bool showText;
                showText = !ConvertHelper.TryToBool(elementMetadata.ShowText, out showText) || showText;

                // Для генерации штрих-кода используется функциональность FastReport

                try
                {
                    var barcode = new BarcodeObject
                                  {
                                      Barcode = CreateBarcode(elementMetadata),
                                      ShowText = showText,
                                      Text = textSting,
                                      Height = 64
                                  };

                    // Для получения размеров штрих-кода рисуем его первый раз
                    using (var codeBmp = new Bitmap(1, 1))
                    using (var graphics = Graphics.FromImage(codeBmp))
                    using (var graphicCache = new GraphicCache())
                    {
                        barcode.Draw(new FRPaintEventArgs(graphics, 1, 1, graphicCache));
                    }

                    // Теперь, зная размеры штрих-кода, рисуем его второй раз
                    if (barcode.Width > 0 && barcode.Height > 0)
                    {
                        using (var codeBmp = new Bitmap((int)barcode.Width, (int)barcode.Height))
                        using (var graphics = Graphics.FromImage(codeBmp))
                        using (var graphicCache = new GraphicCache())
                        {
                            graphics.Clear(Color.White);
                            barcode.Draw(new FRPaintEventArgs(graphics, 1, 1, graphicCache));

                            imageSize.Width = codeBmp.Width;
                            imageSize.Height = codeBmp.Height;

                            var codeStream = new MemoryStream();
                            codeBmp.Save(codeStream, ImageFormat.Bmp);

                            return codeStream;
                        }
                    }
                }
                catch
                {
                }
            }

            return null;
        }


        protected abstract BarcodeBase CreateBarcode(dynamic elementMetadata);

        protected abstract string PrepareText(string barcodeText);
    }
}