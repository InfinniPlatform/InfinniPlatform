using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ContextMenu
{
	sealed class ContextMenuItemSeparatorBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var element = new ContextMenuItemSeparator(parent);
			element.ApplyElementMeatadata((object)metadata);

			return element;
		}
	}
}