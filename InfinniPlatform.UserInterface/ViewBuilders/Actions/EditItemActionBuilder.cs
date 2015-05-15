using InfinniPlatform.UserInterface.ViewBuilders.Data;

namespace InfinniPlatform.UserInterface.ViewBuilders.Actions
{
	sealed class EditItemActionBuilder : BaseItemActionBuilder
	{
		protected override void ExecuteAction(ObjectBuilderContext context, BaseItemAction action, dynamic metadata)
		{
			var selectedItem = action.GetSelectedItem();

			if (selectedItem != null)
			{
				ViewHelper.ShowView(selectedItem,
									() => context.Build(action.GetView(), metadata.View),
									childDataSource => OnInitializeChildView(childDataSource, selectedItem),
									childDataSource => OnAcceptedChildView(action, childDataSource, selectedItem));
			}
		}

		private static void OnInitializeChildView(IDataSource childDataSource, dynamic selectedItem)
		{
			childDataSource.SuspendUpdate();
			childDataSource.SetEditMode();
			childDataSource.ResumeUpdate();
			childDataSource.SetSelectedItem(selectedItem.Clone());
		}

		private static void OnAcceptedChildView(BaseItemAction action, IDataSource childDataSource, dynamic selectedItem)
		{
			var newItem = childDataSource.GetSelectedItem();

			if (newItem != null)
			{
				action.ReplaceItem(selectedItem, newItem);
				action.SetSelectedItem(newItem);
			}
		}
	}
}