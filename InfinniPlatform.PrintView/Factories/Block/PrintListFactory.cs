using System.Collections;

using InfinniPlatform.PrintView.Abstractions;
using InfinniPlatform.PrintView.Abstractions.Block;
using InfinniPlatform.PrintView.Abstractions.Defaults;

namespace InfinniPlatform.PrintView.Factories.Block
{
    internal class PrintListFactory : PrintElementFactoryBase<PrintList>
    {
        public override object Create(PrintElementFactoryContext context, PrintList template)
        {
            var markerOffsetSize = template.MarkerOffsetSize ?? PrintViewDefaults.List.MarkerOffsetSize;
            var markerOffsetSizeUnit = template.MarkerOffsetSizeUnit ?? PrintViewDefaults.List.MarkerOffsetSizeUnit;

            var element = new PrintList
            {
                StartIndex = template.StartIndex ?? PrintViewDefaults.List.StartIndex,
                MarkerStyle = template.MarkerStyle ?? PrintViewDefaults.List.MarkerStyle,
                MarkerOffsetSize = markerOffsetSize,
                MarkerOffsetSizeUnit = markerOffsetSizeUnit
            };

            FactoryHelper.ApplyElementProperties(element, template, context.ElementStyle);
            FactoryHelper.ApplyBlockProperties(element, template, context.ElementStyle);

            var markerPadding = new PrintThickness(markerOffsetSize, 0, 0, 0, markerOffsetSizeUnit);
            var itemContext = context.CreateContentContext(element.Margin, element.Padding, element.Border?.Thickness, markerPadding);

            // Создание явно объявленных элементов списка

            var staticItems = context.Factory.BuildElements(itemContext, template.Items);

            if (staticItems != null)
            {
                foreach (PrintBlock staticItem in staticItems)
                {
                    var listItem = new PrintSection();
                    listItem.Blocks.Add(staticItem);
                    element.Items.Add(listItem);
                }
            }

            // Создание элементов списка по данным источника

            var listItemTemplate = template.ItemTemplate;
            var listSource = context.ElementSourceValue;

            if (listItemTemplate != null)
            {
                if (ConvertHelper.ObjectIsCollection(listSource))
                {
                    foreach (var itemSource in (IEnumerable)listSource)
                    {
                        itemContext.ElementSourceValue = itemSource;

                        var dynamicItem = (PrintBlock)context.Factory.BuildElement(itemContext, listItemTemplate);

                        var listItem = new PrintSection();
                        listItem.Blocks.Add(dynamicItem);
                        element.Items.Add(listItem);
                    }
                }
                else if (context.IsDesignMode)
                {
                    // Отображение шаблона элемента в дизайнере

                    itemContext.ElementSourceValue = null;

                    var dynamicItem = (PrintBlock)context.Factory.BuildElement(itemContext, listItemTemplate);

                    var listItem = new PrintSection();
                    listItem.Blocks.Add(dynamicItem);
                    element.Items.Add(listItem);
                }
            }

            FactoryHelper.ApplyTextCase(element, element.TextCase);

            return element;
        }
    }
}