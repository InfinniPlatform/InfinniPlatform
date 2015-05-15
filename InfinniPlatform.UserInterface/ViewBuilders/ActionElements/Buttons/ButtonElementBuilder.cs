using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.Buttons
{
	sealed class ButtonElementBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var button = new ButtonElement(parent);
			button.ApplyElementMeatadata((object)metadata);
			button.SetImage(metadata.Image);

			var action = context.Build(parent, metadata.Action);
			button.SetAction(action);

			if (parent != null && metadata.OnClick != null)
			{
				button.OnClick += parent.GetScript(metadata.OnClick);
			}

			return button;
		}
	}
}