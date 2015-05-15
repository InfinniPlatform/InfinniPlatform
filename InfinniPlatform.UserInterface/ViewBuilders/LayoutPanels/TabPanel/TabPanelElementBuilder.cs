using System;

using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.TabPanel
{
	sealed class TabPanelElementBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var tabPanel = new TabPanelElement(parent);
			tabPanel.ApplyElementMeatadata((object)metadata);

			SetHeaderLocation(tabPanel, metadata.TabHeaderLocation);
			SetHeaderOrientation(tabPanel, metadata.HeaderOrientation);

			var tabPages = context.BuildTabPages(parent, (object)metadata.Pages);

			if (tabPages != null)
			{
				foreach (ITabPage tabPage in tabPages)
				{
					tabPanel.AddPage(tabPage);
				}
			}

			if (metadata.DefaultPage != null)
			{
				var defaultTabPage = tabPanel.GetPage(metadata.DefaultPage as string);

				if (defaultTabPage != null)
				{
					tabPanel.SetSelectedPage(defaultTabPage);
				}
			}

			if (parent != null)
			{
				if (metadata.OnLoaded != null)
				{
					tabPanel.OnLoaded += parent.GetScript(metadata.OnLoaded);
				}

				if (metadata.OnSelectionChanged != null)
				{
					tabPanel.OnSelectionChanged += parent.GetScript(metadata.OnSelectionChanged);
				}
			}

			return tabPanel;
		}

		private static void SetHeaderLocation(ITabPanel tabPanel, dynamic headerLocationValue)
		{
			TabHeaderLocation headerLocation;

			if (Enum.TryParse(headerLocationValue as string, true, out headerLocation) == false)
			{
				headerLocation = TabHeaderLocation.Top;
			}

			tabPanel.SetHeaderLocation(headerLocation);
		}

		private static void SetHeaderOrientation(ITabPanel tabPanel, dynamic headerOrientationValue)
		{
			TabHeaderOrientation headerOrientation;

			if (Enum.TryParse(headerOrientationValue as string, true, out headerOrientation) == false)
			{
				headerOrientation = TabHeaderOrientation.Horizontal;
			}

			tabPanel.SetHeaderOrientation(headerOrientation);
		}
	}
}