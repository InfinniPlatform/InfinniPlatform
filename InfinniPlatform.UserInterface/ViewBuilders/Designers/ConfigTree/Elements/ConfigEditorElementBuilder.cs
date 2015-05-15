using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Controls;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.ViewPanel;
using InfinniPlatform.UserInterface.ViewBuilders.LinkViews;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Elements
{
	sealed class ConfigEditorElementBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var element = new ConfigEditorElement(parent);
			element.ApplyElementMeatadata((object)metadata);

			var editPanelElement = CreateEditPanel(context, parent);
			var editPanel = new ConfigElementEditPanel(editPanelElement.GetControl(), (elementType, elementEditor) => CreateEditorView(context, editPanelElement.GetContentView(), elementType, elementEditor));

			element.SetEditPanel(editPanel);

			return element;
		}

		private static ViewPanelElement CreateEditPanel(ObjectBuilderContext context, View parent)
		{
			dynamic editPanel = new DynamicWrapper();
			editPanel.ViewPanel = new DynamicWrapper();
			editPanel.ViewPanel.View = CreateLinkView("Common", "EditPanelView");

			return (ViewPanelElement)context.Build(parent, editPanel);
		}

		private static LinkView CreateEditorView(ObjectBuilderContext context, View parent, string elementType, string elementEditor)
		{
			var editorLinkView = CreateLinkView(elementType, elementEditor);

			return (LinkView)context.Build(parent, editorLinkView);
		}

		private static dynamic CreateLinkView(string elementType, string elementEditor)
		{
			dynamic linkView = new DynamicWrapper();
			linkView.ExistsView = new DynamicWrapper();
			linkView.ExistsView.ConfigId = "Designer";
			linkView.ExistsView.DocumentId = elementType;
			linkView.ExistsView.ViewId = elementEditor;

			return linkView;
		}
	}
}