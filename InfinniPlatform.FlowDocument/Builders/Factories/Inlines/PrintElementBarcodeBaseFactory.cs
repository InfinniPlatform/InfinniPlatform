using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using FastReport;
using FastReport.Barcode;
using FastReport.Utils;
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
            var imageStream = CreateBarcodeImageStream(buildContext, elementMetadata);

            try
            {
                imageStream = ApplyRotation(imageStream, elementMetadata.Rotation);

                return new PrintElementImage(imageStream);
            }
            catch
            {
            }

            return null;
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

        private Stream CreateBarcodeImageStream(PrintElementBuildContext buildContext, dynamic elementMetadata)
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