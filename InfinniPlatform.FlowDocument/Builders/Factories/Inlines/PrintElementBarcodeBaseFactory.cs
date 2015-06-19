using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using FastReport;
using FastReport.Barcode;
using FastReport.Utils;
using Image = System.Windows.Controls.Image;

namespace InfinniPlatform.FlowDocument.Builders.Factories.Inlines
{
    internal abstract class PrintElementBarcodeBaseFactory : IPrintElementFactory
    {
        public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            var element = new InlineUIContainer();

            BuildHelper.ApplyTextProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyTextProperties(element, elementMetadata);

            BuildHelper.ApplyInlineProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyInlineProperties(element, elementMetadata);

            element.Child = CreateBarcodeImage(buildContext, elementMetadata);

            return element;
        }

        private Image CreateBarcodeImage(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            var imageStream = CreateBarcodeImageStream(buildContext, elementMetadata);

            try
            {
                var imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.StreamSource = imageStream;
                ApplyRotation(imageSource, elementMetadata.Rotation);
                imageSource.EndInit();

                var imageControl = new Image();
                imageControl.BeginInit();
                imageControl.Width = imageSource.Width;
                imageControl.Height = imageSource.Height;
                imageControl.Source = imageSource;
                imageControl.EndInit();

                return imageControl;
            }
            catch
            {
            }

            return null;
        }

        private static void ApplyRotation(BitmapImage bitmap, dynamic rotation)
        {
            string rotationString;

            if (ConvertHelper.TryToNormString(rotation, out rotationString))
            {
                switch (rotationString)
                {
                    case "rotate0":
                        bitmap.Rotation = Rotation.Rotate0;
                        break;
                    case "rotate90":
                        bitmap.Rotation = Rotation.Rotate90;
                        break;
                    case "rotate180":
                        bitmap.Rotation = Rotation.Rotate180;
                        break;
                    case "rotate270":
                        bitmap.Rotation = Rotation.Rotate270;
                        break;
                }
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
                        using (var codeBmp = new Bitmap((int) barcode.Width, (int) barcode.Height))
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