using System;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.ScrollPanel
{
    internal sealed class ScrollPanelElementBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var scrollPanel = new ScrollPanelElement(parent);
            scrollPanel.ApplyElementMeatadata((object) metadata);

            scrollPanel.SetVerticalScroll(GetScrollVisibility(metadata.VerticalScroll));
            scrollPanel.SetHorizontalScroll(GetScrollVisibility(metadata.HorizontalScroll));

            var layoutPanel = context.Build(parent, metadata.LayoutPanel);
            scrollPanel.SetLayoutPanel(layoutPanel);

            if (parent != null && metadata.OnLoaded != null)
            {
                scrollPanel.OnLoaded += parent.GetScript(metadata.OnLoaded);
            }

            return scrollPanel;
        }

        private static ScrollVisibility GetScrollVisibility(dynamic metadataValue)
        {
            ScrollVisibility scrollVisibility;

            if (Enum.TryParse(metadataValue as string, true, out scrollVisibility) == false)
            {
                scrollVisibility = ScrollVisibility.Auto;
            }

            return scrollVisibility;
        }
    }
}