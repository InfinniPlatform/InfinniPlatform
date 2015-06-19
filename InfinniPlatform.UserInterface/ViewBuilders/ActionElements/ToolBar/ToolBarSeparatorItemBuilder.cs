using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ToolBar
{
    internal sealed class ToolBarSeparatorItemBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var separatorItem = new ToolBarSeparatorItem(parent);
            separatorItem.ApplyElementMeatadata((object) metadata);

            return separatorItem;
        }
    }
}