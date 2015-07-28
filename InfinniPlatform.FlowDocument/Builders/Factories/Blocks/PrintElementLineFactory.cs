﻿//using System.Windows;
//using System.Windows.Documents;
//using System.Windows.Media;
using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Builders.Factories.Blocks
{
	sealed class PrintElementLineFactory : IPrintElementFactory
	{
		public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
		{
			var element = new Paragraph
						  {
							  FontSize = 0.1,
							  Margin = BuildHelper.DefaultMargin,
							  Padding = BuildHelper.DefaultPadding,
							  BorderBrush = Brush.Black,
							  BorderThickness = new Thickness(0, 0, 0, 1)
						  };

			BuildHelper.ApplyTextProperties(element, buildContext.ElementStyle);
			BuildHelper.ApplyTextProperties(element, elementMetadata);

			BuildHelper.ApplyBlockProperties(element, buildContext.ElementStyle);
			BuildHelper.ApplyBlockProperties(element, elementMetadata);

			return element;
		}
	}
}