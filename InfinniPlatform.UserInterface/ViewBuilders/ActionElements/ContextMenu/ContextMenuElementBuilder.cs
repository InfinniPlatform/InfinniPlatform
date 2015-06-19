using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ContextMenu
{
    internal sealed class ContextMenuElementBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var element = new ContextMenuElement(parent);
            element.ApplyElementMeatadata((object) metadata);

            var items = context.BuildMany(parent, metadata.Items);

            if (items != null)
            {
                foreach (var item in items)
                {
                    element.AddItem(item);
                }
            }

            if (parent != null && metadata.OnOpening != null)
            {
                element.OnOpening += parent.GetScript(metadata.OnOpening);
            }

            if (parent != null && metadata.OnOpened != null)
            {
                element.OnOpened += parent.GetScript(metadata.OnOpened);
            }

            if (parent != null && metadata.OnClosing != null)
            {
                element.OnClosing += parent.GetScript(metadata.OnClosing);
            }

            if (parent != null && metadata.OnClosed != null)
            {
                element.OnClosed += parent.GetScript(metadata.OnClosed);
            }

            return element;
        }
    }
}