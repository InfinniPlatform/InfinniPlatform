using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Actions
{
	sealed class OpenViewActionBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var action = new BaseAction(parent);
			action.SetAction(() => ExecuteAction(context, parent, metadata));
			return action;
		}

		private static void ExecuteAction(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			ViewHelper.ShowView(null, () => context.Build(parent, metadata.View));
		}
	}
}