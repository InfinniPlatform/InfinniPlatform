using System;
using System.Collections.Generic;

using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Defaults;

namespace InfinniPlatform.PrintView.Factories
{
    internal class PrintDocumentFactory : PrintElementFactoryBase<PrintDocument>
    {
        public override object Create(PrintElementFactoryContext context, PrintDocument template)
        {
            var pageSize = template.PageSize ?? PrintViewDefaults.Document.PageSize;
            var pagePadding = template.PagePadding ?? PrintViewDefaults.Document.PagePadding;

            var element = new PrintDocument
            {
                PageSize = pageSize,
                PagePadding = pagePadding
            };

            // Установка стилей печатного представления
            ApplyPrintViewStyles(context, template.Styles);

            FactoryHelper.ApplyElementProperties(element, template, context.ElementStyle);

            // Настройка размеров содержимого печатного представления
            context.ElementWidth = PrintSizeUnitConverter.ToUnifiedSize(pageSize.Width, pageSize.SizeUnit);
            var contentContext = context.CreateContentContext(pagePadding);

            // Создание содержимого печатного представления

            var blocks = context.Factory.BuildElements(contentContext, template.Blocks);

            if (blocks != null)
            {
                foreach (PrintBlock block in blocks)
                {
                    element.Blocks.Add(block);
                }
            }

            FactoryHelper.ApplyTextCase(element, element.TextCase);

            return element;
        }


        private static void ApplyPrintViewStyles(PrintElementFactoryContext context, IEnumerable<PrintStyle> styles)
        {
            if (styles != null)
            {
                var printViewStyles = new Dictionary<string, PrintStyle>(StringComparer.OrdinalIgnoreCase);

                foreach (var style in styles)
                {
                    if (!string.IsNullOrEmpty(style.Name))
                    {
                        printViewStyles[style.Name] = style;
                    }
                }

                context.Styles = printViewStyles;
            }
        }
    }
}