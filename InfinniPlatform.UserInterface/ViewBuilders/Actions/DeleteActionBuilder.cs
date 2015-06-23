using System.Windows;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Actions
{
    internal sealed class DeleteActionBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var action = new BaseAction(parent);
            action.SetAction(() => ExecuteAction(parent, metadata));
            return action;
        }

        private static void ExecuteAction(View parent, dynamic metadata)
        {
            // Определение источника данных родительского представления
            IDataSource parentDataSource = parent.GetDataSource(metadata.DataSource);

            if (parentDataSource != null)
            {
                // Определение выделенного элемента в родительском представлении 
                var editItem = parentDataSource.GetSelectedItem();
                var idProperty = parentDataSource.GetIdProperty();

                if (editItem != null && string.IsNullOrEmpty(idProperty) == false)
                {
                    // Определение уникального идентификатора выделенного элемента, поскольку родительское
                    // представление может отображать только проекцию данных редактируемого агрегата
                    var editItemId = editItem.GetProperty(idProperty) as string;

                    if (editItemId != null && (metadata.Accept == false
                                               || MessageBox.Show(Resources.DeleteActionQuestion, parent.GetText(),
                                                   MessageBoxButton.YesNo, MessageBoxImage.Question,
                                                   MessageBoxResult.No) == MessageBoxResult.Yes))
                    {
                        try
                        {
                            // Операция вызывается синхронно, так как нет смысла усложнять этот момент
                            parentDataSource.DeleteItem(editItemId);
                        }
                        catch
                        {
                            // Обработка ошибок сохранения находится на уровне представления
                        }
                    }
                }
            }
        }
    }
}