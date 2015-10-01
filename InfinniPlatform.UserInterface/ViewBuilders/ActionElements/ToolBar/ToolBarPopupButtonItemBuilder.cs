using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ToolBar
{
    internal sealed class ToolBarPopupButtonItemBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var popupButtonItem = new ToolBarPopupButtonItem(parent);
            popupButtonItem.ApplyElementMeatadata((object) metadata);
            popupButtonItem.SetImage(metadata.Image);
            popupButtonItem.SetHotkey(metadata.Hotkey);

            var action = context.Build(parent, metadata.Action);
            popupButtonItem.SetAction(action);

            if (parent != null && metadata.OnClick != null)
            {
                popupButtonItem.OnClick += parent.GetScript(metadata.OnClick);
            }

            var subItems = context.BuildMany(parent, metadata.Items);

            if (subItems != null)
            {
                foreach (var subItem in subItems)
                {
                    popupButtonItem.AddItem(subItem);
                }
            }

            return popupButtonItem;
        }
    }
}