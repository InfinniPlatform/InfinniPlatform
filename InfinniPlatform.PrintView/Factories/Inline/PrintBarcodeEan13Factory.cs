using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Defaults;
using InfinniPlatform.PrintView.Model.Inline;

using ZXing;
using ZXing.OneD;

namespace InfinniPlatform.PrintView.Factories.Inline
{
    internal class PrintBarcodeEan13Factory : PrintBarcodeFactory<PrintBarcodeEan13>
    {
        protected override string PrepareBarcodeText(string barcodeText)
        {
            if (string.IsNullOrWhiteSpace(barcodeText))
            {
                return PrintViewDefaults.BarcodeEan13.NullText;
            }

            barcodeText = barcodeText.Trim();

            return barcodeText.All(char.IsDigit) ? barcodeText : PrintViewDefaults.BarcodeEan13.NullText;
        }

        protected override void CreateBarcodeImage(PrintBarcodeEan13 template, PrintImage barcodeImage, string barcodeText)
        {
            barcodeImage.Size = template.Size ?? PrintViewDefaults.BarcodeEan13.Size();
            barcodeImage.Rotation = template.Rotation ?? PrintViewDefaults.BarcodeEan13.Rotation;

            var width = (int)PrintSizeUnitConverter.ToUnifiedSize(barcodeImage.Size.Width, barcodeImage.Size.SizeUnit);
            var height = (int)PrintSizeUnitConverter.ToUnifiedSize(barcodeImage.Size.Height, barcodeImage.Size.SizeUnit);
            var showText = template.ShowText ?? PrintViewDefaults.BarcodeEan13.ShowText;

            var barcodeWriter = new EAN13Writer();

            var barcodeHints = new Dictionary<EncodeHintType, object>
                                 {
                                     { EncodeHintType.PURE_BARCODE, !showText }
                                 };

            var barcode = barcodeWriter.encode(barcodeText, BarcodeFormat.EAN_13, width, height, barcodeHints);

            barcodeImage.Data = GetBarcodeImageData(barcode);
            barcodeImage.Size.Height = height;
            barcodeImage.Size.Width = width;
        }
    }
}