using System;
using System.Drawing;
using System.Linq;

using InfinniPlatform.PrintView.Model;

using ZXing;
using ZXing.Common;

namespace InfinniPlatform.PrintView.Factories.Inlines
{
    internal class PrintElementBarcodeEan13Factory : PrintElementBarcodeBaseFactory
    {
        protected override Bitmap CreateBarcode(dynamic elementMetadata, string textString, PrintElementSize imageSize, bool showText)
        {
            var height = (imageSize.Height != null) ? Convert.ToInt32(imageSize.Height) : 64;

            var width = (imageSize.Width != null) ? Convert.ToInt32(imageSize.Width) : 128;

            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.EAN_13,
                Options = new EncodingOptions { Height = height, Width = width, PureBarcode = !showText }
            };

            imageSize.Height = height;
            imageSize.Width = width;

            var bitmap = barcodeWriter.Write(textString);

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