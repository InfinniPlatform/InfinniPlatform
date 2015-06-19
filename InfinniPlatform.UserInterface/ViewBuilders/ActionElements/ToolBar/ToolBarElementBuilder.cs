using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ToolBar
{
    internal sealed class ToolBarElementBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var toolBar = new ToolBarElement(parent);
            toolBar.ApplyElementMeatadata((object) metadata);

            var items = context.BuildMany(parent, metadata.Items);

            if (items != null)
            {
                foreach (var item in items)
                {
                    toolBar.AddItem(item);
                }
            }

            return toolBar;
        }
    }
}