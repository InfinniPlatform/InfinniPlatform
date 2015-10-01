using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Actions
{
    internal sealed class CancelActionBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var action = new BaseAction(parent);
            action.SetAction(() => ExecuteAction(parent));
            return action;
        }

        private static void ExecuteAction(View parent)
        {
            parent.SetDialogResult(DialogResult.Canceled);
            parent.Close();
        }
    }
}