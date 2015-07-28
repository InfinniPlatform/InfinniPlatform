using System.Linq;
using FastReport.Barcode;

namespace InfinniPlatform.FlowDocument.Builders.Factories.Inlines
{
	sealed class PrintElementBarcodeEan13Factory : PrintElementBarcodeBaseFactory
	{
		protected override BarcodeBase CreateBarcode(dynamic elementMetadata)
		{
			var barcode = new BarcodeEAN13();
			ApplyCalcCheckSum(barcode, elementMetadata.CalcCheckSum);
			ApplyWideBarRatio(barcode, elementMetadata.WideBarRatio);

			return barcode;
		}

		protected override string PrepareText(string barcodeText)
		{
			if (!string.IsNullOrEmpty(barcodeText))
			{
				barcodeText = barcodeText.Trim();

				if (barcodeText.Length == 0 || barcodeText.Any(c => !char.IsDigit(c)))
				{
					barcodeText = "0";
				}
			}

			return barcodeText;
		}

		private static void ApplyCalcCheckSum(BarcodeEAN13 barcode, dynamic calcCheckSum)
		{
			bool calcCheckSumBool;

			if (!ConvertHelper.TryToBool(calcCheckSum, out calcCheckSumBool))
			{
				calcCheckSumBool = true;
			}

			barcode.CalcCheckSum = calcCheckSumBool;
		}

		private static void ApplyWideBarRatio(BarcodeEAN13 barcode, dynamic wideBarRatio)
		{
			double wideBarRatioDouble;

			if (!ConvertHelper.TryToDouble(wideBarRatio, out wideBarRatioDouble) || wideBarRatioDouble < 2)
			{
				wideBarRatioDouble = 2;
			}

			barcode.WideBarRatio = (float)wideBarRatioDouble;
		}
	}
}