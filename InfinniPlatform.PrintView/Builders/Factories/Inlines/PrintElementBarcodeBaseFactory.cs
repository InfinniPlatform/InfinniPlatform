using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.Model.Inlines;

using ZXing;

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

        private static Bitmap ApplyRotation(Bitmap bitmap, dynamic rotation, PrintElementSize imageSize)
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

        private static Bitmap RotateImage(Bitmap image, RotateFlipType rotation, PrintElementSize imageSize)
        {
            try
            {
                image.RotateFlip(rotation);

                imageSize.Width = image.Width;
                imageSize.Height = image.Height;

                return image;
            }
            catch
            {
                return image;
            }
        }

        private Bitmap CreateBarcodeImageStream(PrintElementBuildContext buildContext, dynamic elementMetadata, PrintElementSize imageSize)
        {
            string textSting = BuildHelper.FormatValue(buildContext, elementMetadata.Text, elementMetadata.SourceFormat);

            textSting = PrepareText(textSting);

            if (textSting != null)
            {
                bool showText;
                showText = !ConvertHelper.TryToBool(elementMetadata.ShowText, out showText) || showText;

                try
                {
                    return CreateBarcode(elementMetadata, textSting, imageSize, showText);
                }
                catch
                {
                }
            }

            return null;
        }


        protected abstract Bitmap CreateBarcode(dynamic metadata, string elementMetadata, PrintElementSize imageSize, bool showText);

        protected abstract string PrepareText(string barcodeText);
    }
}