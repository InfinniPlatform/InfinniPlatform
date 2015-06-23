using System.Collections;
using System.Linq;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data.DataSources
{
    /// <summary>
    ///     Стратегия режима работы источника данных для представлений с редактором элемента.
    /// </summary>
    internal sealed class EditDataSourceStrategy : BaseDataSourceStrategy
    {
        public EditDataSourceStrategy(IDataSource dataSource)
            : base(dataSource)
        {
        }

        public override void OnPageNumberChanged(dynamic value)
        {
        }

        public override void OnPageSizeChanged(dynamic value)
        {
        }

        public override void OnSelectedItemChanged(dynamic value)
        {
            InvokeEvent(DataSource.OnSelectedItemChanged, value);
        }

        public override void OnPropertyFiltersChanged(dynamic value)
        {
        }

        public override void OnTextFilterChanged(dynamic value)
        {
        }

        public override void OnItemSaved(dynamic value)
        {
            InvokeEvent(DataSource.OnItemSaved, value);
        }

        public override void OnItemDeleted(dynamic value)
        {
            InvokeEvent(DataSource.OnItemDeleted, value);
        }

        public override void OnItemsUpdated(dynamic value)
        {
            InvokeEvent(DataSource.OnItemsUpdated, value);

            // Выделение первого элемента в списке, чтобы сработала привязка данных
            DataSource.SetSelectedItem((value != null) ? Enumerable.FirstOrDefault(value) : null);
        }

        public override void OnError(dynamic value)
        {
            InvokeEvent(DataSource.OnError, value);
        }

        public override IEnumerable GetItems(IDataProvider dataProvider)
        {
            object item = null;

            var itemId = DataSource.GetIdFilter();

            if (string.IsNullOrEmpty(itemId) == false)
            {
                item = dataProvider.GetItem(itemId);
            }

            if (item == null)
            {
                item = DataSource.GetFillCreatedItem()
                    ? dataProvider.CreateItem()
                    : new DynamicWrapper();
            }

            return new[] {item};
        }
    }
}