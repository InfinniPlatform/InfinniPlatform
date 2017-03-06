using System.Collections.Generic;

using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Defaults;
using InfinniPlatform.PrintView.Model.Inline;

using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace InfinniPlatform.PrintView.Factories.Inline
{
    internal class PrintBarcodeQrFactory : PrintBarcodeFactory<PrintBarcodeQr>
    {
        protected override string PrepareBarcodeText(string barcodeText)
        {
            return string.IsNullOrEmpty(barcodeText) ? PrintViewDefaults.BarcodeQr.NullText : barcodeText;
        }

        protected override void CreateBarcodeImage(PrintBarcodeQr template, PrintImage barcodeImage, string barcodeText)
        {
            barcodeImage.Size = template.Size ?? PrintViewDefaults.BarcodeQr.Size();
            barcodeImage.Rotation = template.Rotation ?? PrintViewDefaults.BarcodeQr.Rotation;

            var width = (int)PrintSizeUnitConverter.ToUnifiedSize(barcodeImage.Size.Width, barcodeImage.Size.SizeUnit);
            var height = (int)PrintSizeUnitConverter.ToUnifiedSize(barcodeImage.Size.Height, barcodeImage.Size.SizeUnit);
            var showText = template.ShowText ?? PrintViewDefaults.BarcodeQr.ShowText;
            var errorCorrection = template.ErrorCorrection ?? PrintViewDefaults.BarcodeQr.ErrorCorrection;

            var barcodeWriter = new QRCodeWriter();

            var barcodeHints = new Dictionary<EncodeHintType, object>
                                 {
                                     { EncodeHintType.PURE_BARCODE, !showText }
                                 };

            switch (errorCorrection)
            {
                case PrintBarcodeQrErrorCorrection.Low:
                    barcodeHints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.L);
                    break;
                case PrintBarcodeQrErrorCorrection.Medium:
                    barcodeHints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.M);
                    break;
                case PrintBarcodeQrErrorCorrection.Quartile:
                    barcodeHints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.Q);
                    break;
                case PrintBarcodeQrErrorCorrection.High:
                    barcodeHints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);
                    break;
                default:
                    barcodeHints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.L);
                    break;
            }

            var barcode = barcodeWriter.encode(barcodeText, BarcodeFormat.QR_CODE, width, height, barcodeHints);

            barcodeImage.Data = GetBarcodeImageData(barcode);
            barcodeImage.Size.Height = height;
            barcodeImage.Size.Width = width;
        }
    }
}