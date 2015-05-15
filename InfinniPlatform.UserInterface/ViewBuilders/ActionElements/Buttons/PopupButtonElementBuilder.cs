using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.Buttons
{
	sealed class PopupButtonElementBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var popupButton = new PopupButtonElement(parent);
			popupButton.ApplyElementMeatadata((object)metadata);
			popupButton.SetImage(metadata.Image);

			var action = context.Build(parent, metadata.Action);
			popupButton.SetAction(action);

			if (parent != null && metadata.OnClick != null)
			{
				popupButton.OnClick += parent.GetScript(metadata.OnClick);
			}

			var items = context.BuildMany(parent, metadata.Items);

			if (items != null)
			{
				foreach (var item in items)
				{
					popupButton.AddItem(item);
				}
			}

			return popupButton;
		}
	}
}