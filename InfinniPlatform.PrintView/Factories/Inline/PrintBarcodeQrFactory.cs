using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Defaults;
using InfinniPlatform.PrintView.Model.Inline;

using ZXing;
using ZXing.Common;
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

            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Width = width,
                    Height = height,
                    PureBarcode = !showText
                }
            };

            switch (errorCorrection)
            {
                case PrintBarcodeQrErrorCorrection.Low:
                    barcodeWriter.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.L);
                    break;
                case PrintBarcodeQrErrorCorrection.Medium:
                    barcodeWriter.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.M);
                    break;
                case PrintBarcodeQrErrorCorrection.Quartile:
                    barcodeWriter.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.Q);
                    break;
                case PrintBarcodeQrErrorCorrection.High:
                    barcodeWriter.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);
                    break;
                default:
                    barcodeWriter.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.L);
                    break;
            }

            using (var barcodeBitmap = barcodeWriter.Write(barcodeText))
            {
                barcodeImage.Data = FactoryHelper.GetBitmapBytes(barcodeBitmap);
            }

            barcodeImage.Size.Height = height;
            barcodeImage.Size.Width = width;
        }
    }
}