using System;
using System.Windows;
using InfinniPlatform.UserInterface.Properties;

namespace InfinniPlatform.UserInterface.ViewBuilders.Actions
{
    internal sealed class DeleteItemActionBuilder : BaseItemActionBuilder
    {
        protected override void ExecuteAction(ObjectBuilderContext context, BaseItemAction action, dynamic metadata)
        {
            var selectedItem = action.GetSelectedItem();

            if (selectedItem != null && (Convert.ToBoolean(metadata.Accept) == false
                                         ||
                                         MessageBox.Show(Resources.DeleteItemActionQuestion, action.GetView().GetText(),
                                             MessageBoxButton.YesNo, MessageBoxImage.Question,
                                             MessageBoxResult.No) == MessageBoxResult.Yes))
            {
                action.RemoveItem(selectedItem);
            }
        }
    }
}