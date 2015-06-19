using InfinniPlatform.UserInterface.ViewBuilders.Data;

namespace InfinniPlatform.UserInterface.ViewBuilders.Actions
{
    internal sealed class AddItemActionBuilder : BaseItemActionBuilder
    {
        protected override void ExecuteAction(ObjectBuilderContext context, BaseItemAction action, dynamic metadata)
        {
            ViewHelper.ShowView(null,
                () => context.Build(action.GetView(), metadata.View),
                childDataSource => OnInitializeChildView(childDataSource),
                childDataSource => OnAcceptedChildView(action, childDataSource));
        }

        private static void OnInitializeChildView(IDataSource childDataSource)
        {
            childDataSource.SuspendUpdate();
            childDataSource.SetEditMode();
            childDataSource.ResumeUpdate();
        }

        private static void OnAcceptedChildView(BaseItemAction action, IDataSource childDataSource)
        {
            var newItem = childDataSource.GetSelectedItem();

            if (newItem != null)
            {
                action.AddItem(newItem);
                action.SetSelectedItem(newItem);
            }
        }
    }
}