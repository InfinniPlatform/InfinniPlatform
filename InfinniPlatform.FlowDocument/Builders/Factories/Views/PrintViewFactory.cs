using System;
using System.Collections.Generic;
using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.Model.Font;
using InfinniPlatform.FlowDocument.Model.Views;

namespace InfinniPlatform.FlowDocument.Builders.Factories.Views
{
    sealed class PrintViewFactory : IPrintElementFactory
    {
        public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            var element = new PrintViewDocument
                          {
                              Font = new PrintElementFont { Family = BuildHelper.DefautlFontFamily }
                          };

            // Установка стилей печатного представления
            ApplyPrintViewStyles(buildContext, elementMetadata.Styles);

            // Установка общих свойств печатного представления
            BuildHelper.ApplyTextProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyTextProperties(element, elementMetadata);

            // Настройка размеров страницы печатного представления
            ApplyPageSize(element, elementMetadata.PageSize);
            ApplyPagePadding(element, elementMetadata.PagePadding);

            // Настройка размеров колонки печатного представления
            var contentWidth = CalcContentWidth(element);

            // Генерация содержимого печатного представления

            var contentContext = buildContext.Create(contentWidth);
            var blocks = buildContext.ElementBuilder.BuildElements(contentContext, elementMetadata.Blocks);

            if (blocks != null)
            {
                foreach (var block in blocks)
                {
                    element.Blocks.Add(block);
                }
            }

            BuildHelper.PostApplyTextProperties(element, buildContext.ElementStyle);
            BuildHelper.PostApplyTextProperties(element, elementMetadata);

            return element;
        }


        private static void ApplyPrintViewStyles(PrintElementBuildContext buildContext, dynamic styles)
        {
            if (styles != null)
            {
                var printViewStyles = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

                foreach (var style in styles)
                {
                    string name;

                    if (ConvertHelper.TryToNormString(style.Name, out name))
                    {
                        printViewStyles[name] = style;
                    }
                }

                buildContext.PrintViewStyles = printViewStyles;
            }
        }

        private static void ApplyPageSize(PrintViewDocument element, dynamic pageSize)
        {
            // По умолчанию размер страницы A4
            const double pageWidthA4 = 21.0 * SizeUnits.Cm;
            const double pageHeightA4 = 29.7 * SizeUnits.Cm;

            var pageWidth = pageWidthA4;
            var pageHeight = pageHeightA4;

            if (pageSize != null)
            {
                if (!BuildHelper.TryToSizeInPixels(pageSize.Width, pageSize.SizeUnit, out pageWidth))
                {
                    pageWidth = pageWidthA4;
                }

                if (!BuildHelper.TryToSizeInPixels(pageSize.Height, pageSize.SizeUnit, out pageHeight))
                {
                    pageHeight = pageHeightA4;
                }
            }

            element.PageSize = new PrintElementSize { Width = pageWidth, Height = pageHeight };
        }

        private static void ApplyPagePadding(PrintViewDocument element, dynamic pagePadding)
        {
            // По умолчанию отступ на странице 1см
            const double pagePadding1Cm = 1.0 * SizeUnits.Cm;

            PrintElementThickness pagePaddingThickness;

            if (!BuildHelper.TryToThickness(pagePadding, out pagePaddingThickness))
            {
                pagePaddingThickness = new PrintElementThickness(pagePadding1Cm);
            }

            element.PagePadding = pagePaddingThickness;
        }

        private static double CalcContentWidth(PrintViewDocument element)
        {
            return Math.Max((double)element.PageSize.Width
                            - (double)element.PagePadding.Left
                            - (double)element.PagePadding.Right,
                            0);
        }
    }
}