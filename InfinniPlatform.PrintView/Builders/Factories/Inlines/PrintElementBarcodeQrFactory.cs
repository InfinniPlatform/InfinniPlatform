using System;
using System.Drawing;

using InfinniPlatform.FlowDocument.Model;

using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;

namespace InfinniPlatform.FlowDocument.Builders.Factories.Inlines
{
    internal sealed class PrintElementBarcodeQrFactory : PrintElementBarcodeBaseFactory
    {
        protected override Bitmap CreateBarcode(dynamic elementMetadata, string textString, PrintElementSize imageSize, bool showText)
        {
            var height = imageSize.Height == null
                             ? 116
                             : Convert.ToInt32(imageSize.Height);

            var width = imageSize.Width == null
                            ? 116
                            : Convert.ToInt32(imageSize.Width);

            var barcodeWriter = new BarcodeWriter
                                {
                                    Format = BarcodeFormat.QR_CODE,
                                    Options = new EncodingOptions { Height = height, Width = width, PureBarcode = !showText }
                                };

            imageSize.Height = height;
            imageSize.Width = width;

            ApplyErrorCorrection(barcodeWriter, elementMetadata.ErrorCorrection);
            var bitmap = barcodeWriter.Write(textString);

            return bitmap;
        }

        protected override string PrepareText(string barcodeText)
        {
            return !string.IsNullOrEmpty(barcodeText)
                       ? barcodeText
                       : "0";
        }

        private static void ApplyErrorCorrection(BarcodeWriter barcodeWriter, dynamic errorCorrection)
        {
            string errorCorrectionString;
            ConvertHelper.TryToNormString(errorCorrection, out errorCorrectionString);

            switch (errorCorrectionString)
            {
                case "low":
                    barcodeWriter.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.L);
                    break;
                case "medium":
                    barcodeWriter.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.M);

                    break;
                case "quartile":
                    barcodeWriter.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.Q);
                    break;
                case "high":
                    barcodeWriter.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);

                    break;
                default:
                    barcodeWriter.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.L);
                    break;
            }
        }
    }
}