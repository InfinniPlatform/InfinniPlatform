using System;

using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.Panel
{
	sealed class PanelElementBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var panel = new PanelElement(parent);
			panel.ApplyElementMeatadata((object)metadata);

			panel.SetImage(metadata.Image);
			panel.SetCollapsible(Convert.ToBoolean(metadata.Collapsible));
			panel.SetCollapsed(Convert.ToBoolean(metadata.Collapsed));

			var items = context.BuildMany(parent, metadata.Items);

			if (items != null)
			{
				foreach (var item in items)
				{
					panel.AddItem(item);
				}
			}

			if (parent != null)
			{
				if (metadata.OnLoaded != null)
				{
					panel.OnLoaded += parent.GetScript(metadata.OnLoaded);
				}

				if (metadata.OnExpanded != null)
				{
					panel.OnExpanded += parent.GetScript(metadata.OnExpanded);
				}

				if (metadata.OnCollapsed != null)
				{
					panel.OnCollapsed += parent.GetScript(metadata.OnCollapsed);
				}
			}

			return panel;
		}
	}
}