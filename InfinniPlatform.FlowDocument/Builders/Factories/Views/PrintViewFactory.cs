using System;
using System.Collections.Generic;
//using System.Windows;
using InfinniPlatform.FlowDocument.Model;
//using FrameworkFlowDocument = System.Windows.Documents.FlowDocument;
using FrameworkFlowDocument = InfinniPlatform.FlowDocument.Model.Views.FlowDocument;

namespace InfinniPlatform.FlowDocument.Builders.Factories.Views
{
	sealed class PrintViewFactory : IPrintElementFactory
	{
		public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
		{
			var element = new FrameworkFlowDocument
						  {
							  FontFamily = BuildHelper.DefautlFontFamily
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
			element.ColumnWidth = contentWidth;

			// Генерация содержимого печатного представления

			var contentContext = buildContext.Create(contentWidth);
			var blocks = buildContext.ElementBuilder.BuildElements(contentContext, elementMetadata.Blocks);

			if (blocks != null)
			{
				element.Blocks.AddRange(blocks);
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

		private static void ApplyPageSize(FrameworkFlowDocument element, dynamic pageSize)
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

			element.PageWidth = pageWidth;
			element.PageHeight = pageHeight;
		}

		private static void ApplyPagePadding(FrameworkFlowDocument element, dynamic pagePadding)
		{
			// По умолчанию отступ на странице 1см
			const double pagePadding1Cm = 1.0 * SizeUnits.Cm;

			Thickness pagePaddingThickness;

			if (!BuildHelper.TryToThickness(pagePadding, out pagePaddingThickness))
			{
				pagePaddingThickness = new Thickness(pagePadding1Cm);
			}

			element.PagePadding = pagePaddingThickness;
		}

		private static double CalcContentWidth(FrameworkFlowDocument element)
		{
			return Math.Max(element.PageWidth
							- element.PagePadding.Left
							- element.PagePadding.Right,
							0);
		}
	}
}