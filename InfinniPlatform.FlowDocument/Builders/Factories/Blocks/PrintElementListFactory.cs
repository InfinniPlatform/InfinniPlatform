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
                              MarkerStyle = PrintElementListMarkerStyle.None
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
                    var listItem = new PrintElementSection();
                    listItem.Blocks.Add(staticItem);
                    element.Items.Add(listItem);
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
                            var listItem = new PrintElementSection();
                            listItem.Blocks.Add(dynamicItem);
                            element.Items.Add(listItem);
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
                        var listItem = new PrintElementSection();
                        listItem.Blocks.Add(dynamicItem);
                        element.Items.Add(listItem);
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
                        element.MarkerStyle = PrintElementListMarkerStyle.None;
                        break;
                    case "disc":
                        element.MarkerStyle = PrintElementListMarkerStyle.Disc;
                        break;
                    case "circle":
                        element.MarkerStyle = PrintElementListMarkerStyle.Circle;
                        break;
                    case "square":
                        element.MarkerStyle = PrintElementListMarkerStyle.Square;
                        break;
                    case "box":
                        element.MarkerStyle = PrintElementListMarkerStyle.Box;
                        break;
                    case "lowerroman":
                        element.MarkerStyle = PrintElementListMarkerStyle.LowerRoman;
                        break;
                    case "upperroman":
                        element.MarkerStyle = PrintElementListMarkerStyle.UpperRoman;
                        break;
                    case "lowerlatin":
                        element.MarkerStyle = PrintElementListMarkerStyle.LowerLatin;
                        break;
                    case "upperlatin":
                        element.MarkerStyle = PrintElementListMarkerStyle.UpperLatin;
                        break;
                    case "decimal":
                        element.MarkerStyle = PrintElementListMarkerStyle.Decimal;
                        break;
                }
            }
        }

        private static void MarkerOffsetStyle(PrintElementList element, dynamic markerOffsetSize, dynamic markerOffsetSizeUnit)
        {
            double markerOffset;

            if (BuildHelper.TryToSizeInPixels(markerOffsetSize, markerOffsetSizeUnit, out markerOffset))
            {
                element.MarkerOffsetSize = markerOffset;
            }
        }

        private static PrintElementBuildContext CreateItemContext(PrintElementList element, PrintElementBuildContext buildContext)
        {
            var contentWidth = BuildHelper.CalcContentWidth(buildContext.ElementWidth, element.Margin, element.Padding, element.BorderThickness, new PrintElementThickness(element.MarkerOffsetSize.Value));
            return buildContext.Create(contentWidth);
        }
    }
}