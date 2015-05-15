using InfinniPlatform.Api.Extensions;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.StackPanel
{
	sealed class StackPanelElementBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var stackPanel = new StackPanelElement(parent);
			stackPanel.ApplyElementMeatadata((object)metadata);
			stackPanel.SetOrientation((metadata.Orientation as string).ToEnum(StackPanelOrientation.Vertical));

			var items = context.BuildMany(parent, metadata.Items);

			if (items != null)
			{
				foreach (var item in items)
				{
					stackPanel.AddItem(item);
				}
			}

			if (parent != null && metadata.OnLoaded != null)
			{
				stackPanel.OnLoaded += parent.GetScript(metadata.OnLoaded);
			}

			return stackPanel;
		}
	}
}