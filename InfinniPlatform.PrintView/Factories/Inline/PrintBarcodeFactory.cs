using InfinniPlatform.PrintView.Model.Inline;

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

        protected abstract string PrepareBarcodeText(string barcodeText);

        protected abstract void CreateBarcodeImage(TBarcode template, PrintImage barcodeImage, string barcodeText);
    }
}