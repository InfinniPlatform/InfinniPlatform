using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.DeployDesigner
{
	sealed class DeployDesignerElementBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var editor = new DeployDesignerElement(parent);
			editor.ApplyElementMeatadata((object)metadata);

			return editor;
		}
	}
}