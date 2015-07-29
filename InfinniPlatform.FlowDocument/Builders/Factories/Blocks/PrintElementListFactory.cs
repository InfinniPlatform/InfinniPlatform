using System.Collections;
using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Builders.Factories.Blocks
{
	sealed class PrintElementListFactory : IPrintElementFactory
	{
		public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
		{
			var element = new PrintElementList
						  {
							  Margin = BuildHelper.DefaultMargin,
							  Padding = BuildHelper.DefaultPadding,
							  MarkerStyle = TextMarkerStyle.None
						  };

			BuildHelper.ApplyTextProperties(element, buildContext.ElementStyle);
			BuildHelper.ApplyTextProperties(element, elementMetadata);

			BuildHelper.ApplyBlockProperties(element, buildContext.ElementStyle);
			BuildHelper.ApplyBlockProperties(element, elementMetadata);

			ApplyStartIndex(element, elementMetadata.StartIndex);
			ApplyMarkerStyle(element, elementMetadata.MarkerStyle);
			MarkerOffsetStyle(element, elementMetadata.MarkerOffsetSize, elementMetadata.MarkerOffsetSizeUnit);

			var itemContext = CreateItemContext(element, buildContext);

			// Генерация явно объявленных элементов списка

			var staticItems = buildContext.ElementBuilder.BuildElements(itemContext, elementMetadata.Items);

			if (staticItems != null)
			{
				foreach (var staticItem in staticItems)
				{
					var listItem = new ListItem();
					listItem.Blocks.Add(staticItem);
					element.ListItems.Add(listItem);
				}
			}

			// Генерация элементов списка по данным источника

			var listItemTemplate = elementMetadata.ItemTemplate;
			var listSource = buildContext.ElementSourceValue;

			if (listItemTemplate != null)
			{
				if (ConvertHelper.ObjectIsCollection(listSource))
				{
					foreach (var itemSource in (IEnumerable)listSource)
					{
						itemContext.ElementSourceValue = itemSource;

						var dynamicItem = buildContext.ElementBuilder.BuildElement(itemContext, listItemTemplate);

						if (dynamicItem is PrintElementBlock)
						{
							var listItem = new ListItem();
							listItem.Blocks.Add(dynamicItem);
							element.ListItems.Add(listItem);
						}
					}
				}
				else if (buildContext.IsDesignMode)
				{
					// Отображение шаблона элемента в дизайнере

					itemContext.ElementSourceValue = null;

					var dynamicItem = buildContext.ElementBuilder.BuildElement(itemContext, listItemTemplate);

					if (dynamicItem is PrintElementBlock)
					{
						var listItem = new ListItem();
						listItem.Blocks.Add(dynamicItem);
						element.ListItems.Add(listItem);
					}
				}
			}

			BuildHelper.PostApplyTextProperties(element, buildContext.ElementStyle);
			BuildHelper.PostApplyTextProperties(element, elementMetadata);

			return element;
		}

		private static void ApplyStartIndex(PrintElementList element, dynamic startIndex)
		{
			int startIndexInt;

			if (ConvertHelper.TryToInt(startIndex, out startIndexInt))
			{
				element.StartIndex = startIndexInt;
			}
		}

		private static void ApplyMarkerStyle(PrintElementList element, dynamic markerStyle)
		{
			string markerStyleString;

			if (ConvertHelper.TryToNormString(markerStyle, out markerStyleString))
			{
				switch (markerStyleString)
				{
					case "none":
						element.MarkerStyle = TextMarkerStyle.None;
						break;
					case "disc":
						element.MarkerStyle = TextMarkerStyle.Disc;
						break;
					case "circle":
						element.MarkerStyle = TextMarkerStyle.Circle;
						break;
					case "square":
						element.MarkerStyle = TextMarkerStyle.Square;
						break;
					case "box":
						element.MarkerStyle = TextMarkerStyle.Box;
						break;
					case "lowerroman":
						element.MarkerStyle = TextMarkerStyle.LowerRoman;
						break;
					case "upperroman":
						element.MarkerStyle = TextMarkerStyle.UpperRoman;
						break;
					case "lowerlatin":
						element.MarkerStyle = TextMarkerStyle.LowerLatin;
						break;
					case "upperlatin":
						element.MarkerStyle = TextMarkerStyle.UpperLatin;
						break;
					case "decimal":
						element.MarkerStyle = TextMarkerStyle.Decimal;
						break;
				}
			}
		}

		private static void MarkerOffsetStyle(PrintElementList element, dynamic markerOffsetSize, dynamic markerOffsetSizeUnit)
		{
			double markerOffset;

			if (BuildHelper.TryToSizeInPixels(markerOffsetSize, markerOffsetSizeUnit, out markerOffset))
			{
				element.MarkerOffset = markerOffset;
			}
		}

		private static PrintElementBuildContext CreateItemContext(PrintElementList element, PrintElementBuildContext buildContext)
		{
			var contentWidth = BuildHelper.CalcContentWidth(buildContext.ElementWidth, element.Margin, element.Padding, element.BorderThickness, new Thickness(element.MarkerOffset));
			return buildContext.Create(contentWidth);
		}
	}
}