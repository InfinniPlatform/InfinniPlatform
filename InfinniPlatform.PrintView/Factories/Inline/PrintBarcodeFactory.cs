using ImageSharp;

using InfinniPlatform.PrintView.Inline;

using ZXing.Common;

namespace InfinniPlatform.PrintView.Factories.Inline
{
    internal abstract class PrintBarcodeFactory<TBarcode> : PrintElementFactoryBase<TBarcode> where TBarcode : PrintBarcode
    {
        public override object Create(PrintElementFactoryContext context, TBarcode template)
        {
            var element = CreateBarcodeImage(context, template);

            FactoryHelper.ApplyElementProperties(element, template, context.ElementStyle);
            FactoryHelper.ApplyInlineProperties(element, template, context.ElementStyle);

            return element;
        }


        private PrintImage CreateBarcodeImage(PrintElementFactoryContext context, TBarcode template)
        {
            var element = new PrintImage();

            CreateBarcodeImage(context, template, element);

            if (element.Data != null)
            {
                FactoryHelper.ApplyRotation(element, updateImageSize: true);
            }

            return element;
        }

        private void CreateBarcodeImage(PrintElementFactoryContext context, TBarcode template, PrintImage barcodeImage)
        {
            var barcodeText = FactoryHelper.FormatValue(context, template.Text, template.SourceFormat);

            barcodeText = PrepareBarcodeText(barcodeText);

            if (!string.IsNullOrEmpty(barcodeText))
            {
                try
                {
                    CreateBarcodeImage(template, barcodeImage, barcodeText);
                }
                catch
                {
                }
            }
        }

        protected static byte[] GetBarcodeImageData(BitMatrix barcode)
        {
            using (var image = new Image(barcode.Width, barcode.Height))
            {
                using (var pixels = image.Lock())
                {
                    for (var y = 0; y < barcode.Height; ++y)
                    {
                        for (var x = 0; x < barcode.Width; ++x)
                        {
                            pixels[x, y] = barcode[x, y] ? Color.Black : Color.White;
                        }
                    }
                }

                return FactoryHelper.GetBitmapBytes(image);
            }
        }


        protected abstract string PrepareBarcodeText(string barcodeText);

        protected abstract void CreateBarcodeImage(TBarcode template, PrintImage barcodeImage, string barcodeText);
    }
}