using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ContextMenu
{
	sealed class ContextMenuItemBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var element = new ContextMenuItem(parent);
			element.ApplyElementMeatadata((object)metadata);
			element.SetImage(metadata.Image);
			element.SetHotkey(metadata.Hotkey);

			var action = context.Build(parent, metadata.Action);
			element.SetAction(action);

			if (parent != null && metadata.OnClick != null)
			{
				element.OnClick += parent.GetScript(metadata.OnClick);
			}

			var subItems = context.BuildMany(parent, metadata.Items);

			if (subItems != null)
			{
				foreach (var subItem in subItems)
				{
					element.AddItem(subItem);
				}
			}

			return element;
		}
	}
}