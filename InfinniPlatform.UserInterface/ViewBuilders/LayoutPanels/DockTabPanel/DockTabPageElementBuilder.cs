using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.DockTabPanel
{
	sealed class DockTabPageElementBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var tabPage = new DockTabPageElement(parent);
			tabPage.ApplyElementMeatadata((object)metadata);

			tabPage.SetImage(metadata.Image);
			tabPage.SetCanClose(metadata.CanClose == true);

			var layoutPanel = context.Build(parent, metadata.LayoutPanel);
			tabPage.SetLayoutPanel(layoutPanel);

			if (parent != null)
			{
				if (metadata.OnLoaded != null)
				{
					tabPage.OnLoaded += parent.GetScript(metadata.OnLoaded);
				}

				if (metadata.OnClosing != null)
				{
					tabPage.OnClosing += parent.GetScript(metadata.OnClosing);
				}

				if (metadata.OnClosed != null)
				{
					tabPage.OnClosed += parent.GetScript(metadata.OnClosed);
				}
			}

			return tabPage;
		}
	}
}