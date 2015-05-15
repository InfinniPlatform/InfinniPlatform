using System;
using System.Windows.Documents;

namespace InfinniPlatform.FlowDocument.Builders.Factories.Inlines
{
	sealed class PrintElementHyperlinkFactory : IPrintElementFactory
	{
		public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
		{
			var element = new Hyperlink();

			BuildHelper.ApplyTextProperties(element, buildContext.ElementStyle);
			BuildHelper.ApplyTextProperties(element, elementMetadata);

			BuildHelper.ApplyInlineProperties(element, buildContext.ElementStyle);
			BuildHelper.ApplyInlineProperties(element, elementMetadata);

			ApplyReference(element, buildContext, elementMetadata);

			// Генерация содержимого элемента

			var inlines = buildContext.ElementBuilder.BuildElements(buildContext, elementMetadata.Inlines);

			if (inlines != null)
			{
				element.Inlines.AddRange(inlines);
			}

			BuildHelper.PostApplyTextProperties(element, buildContext.ElementStyle);
			BuildHelper.PostApplyTextProperties(element, elementMetadata);

			return element;
		}

		private static void ApplyReference(Hyperlink element, PrintElementBuildContext buildContext, dynamic elementMetadata)
		{
			string referenceSting = BuildHelper.FormatValue(buildContext, elementMetadata.Reference, elementMetadata.SourceFormat);

			if (!string.IsNullOrEmpty(referenceSting))
			{
				Uri referenceUri;

				if (Uri.TryCreate(referenceSting, UriKind.RelativeOrAbsolute, out referenceUri))
				{
					element.NavigateUri = referenceUri;
				}
			}
		}
	}
}