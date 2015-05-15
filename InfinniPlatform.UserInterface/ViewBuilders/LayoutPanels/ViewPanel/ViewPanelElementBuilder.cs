using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.LinkViews;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.ViewPanel
{
	sealed class ViewPanelElementBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var viewPanel = new ViewPanelElement(parent, () => CreateView(context, parent, metadata));
			viewPanel.ApplyElementMeatadata((object)metadata);

			if (parent != null && metadata.OnLoaded != null)
			{
				viewPanel.OnLoaded += parent.GetScript(metadata.OnLoaded);
			}

			return viewPanel;
		}

		private static View CreateView(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			LinkView linkView = context.Build(parent, metadata.View);

			if (linkView != null)
			{
				linkView.SetOpenMode(OpenMode.None);

				return linkView.CreateView();
			}

			return null;
		}
	}
}