using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ToolBar
{
	sealed class ToolBarButtonItemBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var buttonItem = new ToolBarButtonItem(parent);
			buttonItem.ApplyElementMeatadata((object)metadata);
			buttonItem.SetImage(metadata.Image);
			buttonItem.SetHotkey(metadata.Hotkey);

			var action = context.Build(parent, metadata.Action);
			buttonItem.SetAction(action);

			if (parent != null && metadata.OnClick != null)
			{
				buttonItem.OnClick += parent.GetScript(metadata.OnClick);
			}

			return buttonItem;
		}
	}
}