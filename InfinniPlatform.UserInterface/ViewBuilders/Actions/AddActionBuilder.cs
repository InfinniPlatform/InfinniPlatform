using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Actions
{
    internal sealed class AddActionBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var action = new BaseAction(parent);
            action.SetAction(() => ExecuteAction(context, parent, metadata));
            return action;
        }

        private static void ExecuteAction(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            IDataSource parentDataSource = parent.GetDataSource(metadata.DataSource);

            if (parentDataSource != null)
            {
                ViewHelper.ShowView(null,
                    () => context.Build(parent, metadata.View),
                    childDataSource => OnInitializeChildView(parentDataSource, childDataSource),
                    childDataSource => OnAcceptedChildView(parentDataSource));
            }
        }

        private static void OnInitializeChildView(IDataSource parentDataSource, IDataSource childDataSource)
        {
            childDataSource.SuspendUpdate();
            childDataSource.SetEditMode();
            childDataSource.SetConfigId(parentDataSource.GetConfigId());
            childDataSource.SetDocumentId(parentDataSource.GetDocumentId());
            childDataSource.ResumeUpdate();
        }

        private static void OnAcceptedChildView(IDataSource parentDataSource)
        {
            parentDataSource.UpdateItems();
        }
    }
}