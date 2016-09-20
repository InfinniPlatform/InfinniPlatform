using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

using InfinniPlatform.FlowDocument.Model;

using ZXing;
using ZXing.Common;

namespace InfinniPlatform.FlowDocument.Builders.Factories.Inlines
{
    internal sealed class PrintElementBarcodeEan13Factory : PrintElementBarcodeBaseFactory
    {
        protected override Bitmap CreateBarcode(dynamic elementMetadata, string textString, PrintElementSize imageSize, bool showText)
        {
            //TODO: Реализация Zxing отличается от FastReport, необходимы уточнения сценариев.
            //			ApplyCalcCheckSum(barcode, elementMetadata.CalcCheckSum);
            //			ApplyWideBarRatio(barcode, elementMetadata.WideBarRatio);

            var height = imageSize.Height == null
                             ? 64
                             : Convert.ToInt32(imageSize.Height);

            var width = imageSize.Width == null
                            ? 128
                            : Convert.ToInt32(imageSize.Width);

            var barcodeWriter = new BarcodeWriter
                                {
                                    Format = BarcodeFormat.EAN_13,
                                    Options = new EncodingOptions { Height = 64, Width = 128, PureBarcode = !showText }
                                };

            imageSize.Height = height;
            imageSize.Width = width;

            var bitmap = barcodeWriter.Write(textString);
            bitmap.Save(@"C:\Projects\InfinniPlatform\Assemblies\ean.png", ImageFormat.Png);
            return bitmap;
        }

        protected override string PrepareText(string barcodeText)
        {
            if (string.IsNullOrWhiteSpace(barcodeText))
            {
                return "0";
            }

            barcodeText = barcodeText.Trim();

            return barcodeText.All(char.IsDigit)
                       ? barcodeText
                       : "0";
        }
    }
}