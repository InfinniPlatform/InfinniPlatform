using FastReport.Barcode;

namespace InfinniPlatform.FlowDocument.Builders.Factories.Inlines
{
	sealed class PrintElementBarcodeQrFactory : PrintElementBarcodeBaseFactory
	{
		protected override BarcodeBase CreateBarcode(dynamic elementMetadata)
		{
			var barcode = new BarcodeQR();
			ApplyErrorCorrection(barcode, elementMetadata.ErrorCorrection);

			return barcode;
		}

		protected override string PrepareText(string barcodeText)
		{
		    if (!string.IsNullOrEmpty(barcodeText))
		    {
		        return barcodeText;
		    }

			return "0";
		}

		private static void ApplyErrorCorrection(BarcodeQR barcode, dynamic errorCorrection)
		{
			string errorCorrectionString;
			ConvertHelper.TryToNormString(errorCorrection, out errorCorrectionString);

			switch (errorCorrectionString)
			{
				case "low":
					barcode.ErrorCorrection = QRCodeErrorCorrection.L;
					break;
				case "medium":
					barcode.ErrorCorrection = QRCodeErrorCorrection.M;
					break;
				case "quartile":
					barcode.ErrorCorrection = QRCodeErrorCorrection.Q;
					break;
				case "high":
					barcode.ErrorCorrection = QRCodeErrorCorrection.H;
					break;
				default:
					barcode.ErrorCorrection = QRCodeErrorCorrection.L;
					break;
			}
		}
	}
}