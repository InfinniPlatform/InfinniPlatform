using System;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data.DataSources
{
    internal abstract class BaseDataSourceBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            IDataSource dataSource = CreateDataSource(parent, metadata);
            dataSource.SetName(metadata.Name);
            dataSource.SetConfigId(metadata.ConfigId);
            dataSource.SetDocumentId(metadata.DocumentId);
            dataSource.SetFillCreatedItem(Convert.ToBoolean(metadata.FillCreatedItem));

            // До загрузки представления источник данных не активен
            dataSource.SuspendUpdate();

            // Публикация сообщений в шину при возникновении событий
            dataSource.NotifyWhenEventAsync(i => i.OnPageNumberChanged);
            dataSource.NotifyWhenEventAsync(i => i.OnPageSizeChanged);
            dataSource.NotifyWhenEventAsync(i => i.OnSelectedItemChanged);
            dataSource.NotifyWhenEventAsync(i => i.OnPropertyFiltersChanged);
            dataSource.NotifyWhenEventAsync(i => i.OnTextFilterChanged);
            dataSource.NotifyWhenEventAsync(i => i.OnItemSaved);
            dataSource.NotifyWhenEventAsync(i => i.OnItemDeleted);
            dataSource.NotifyWhenEventAsync(i => i.OnItemsUpdated);
            dataSource.NotifyWhenEventAsync(i => i.OnError);

            // Подписка на сообщения шины от внешних элементов
            dataSource.SubscribeOnEvent(OnLoaded);
            dataSource.SubscribeOnEvent(OnSetPageNumber);
            dataSource.SubscribeOnEvent(OnSetPageSize);
            dataSource.SubscribeOnEvent(OnSetSelectedItem);
            dataSource.SubscribeOnEvent(OnSetPropertyFilters);
            dataSource.SubscribeOnEvent(OnSetTextFilter);
            dataSource.SubscribeOnEvent(OnSaveItem);
            dataSource.SubscribeOnEvent(OnDeleteItem);
            dataSource.SubscribeOnEvent(OnUpdateItems);

            return dataSource;
        }

        protected abstract IDataSource CreateDataSource(View parent, dynamic metadata);
        // Обработчики сообщений шины (внимание: наименования обработчиков совпадают с наименованиями событий)

        private static void OnLoaded(IDataSource dataSource, dynamic arguments)
        {
            dataSource.ResumeUpdate();
        }

        private static void OnSetPageNumber(IDataSource dataSource, dynamic arguments)
        {
            if (CanHandle(dataSource, arguments))
            {
                dataSource.SetPageNumber(arguments.Value);
            }
        }

        private static void OnSetPageSize(IDataSource dataSource, dynamic arguments)
        {
            if (CanHandle(dataSource, arguments))
            {
                dataSource.SetPageSize(arguments.Value);
            }
        }

        private static void OnSetSelectedItem(IDataSource dataSource, dynamic arguments)
        {
            if (CanHandle(dataSource, arguments) && string.IsNullOrEmpty(arguments.Property))
            {
                dataSource.SetSelectedItem(arguments.Value);
            }
        }

        private static void OnSetPropertyFilters(IDataSource dataSource, dynamic arguments)
        {
            if (CanHandle(dataSource, arguments))
            {
                dataSource.SetPropertyFilters(arguments.Value);
            }
        }

        private static void OnSetTextFilter(IDataSource dataSource, dynamic arguments)
        {
            if (CanHandle(dataSource, arguments))
            {
                dataSource.SetTextFilter(arguments.Value);
            }
        }

        private static void OnSaveItem(IDataSource dataSource, dynamic arguments)
        {
            if (CanHandle(dataSource, arguments))
            {
                dataSource.SaveItem(arguments.Value);
            }
        }

        private static void OnDeleteItem(IDataSource dataSource, dynamic arguments)
        {
            if (CanHandle(dataSource, arguments))
            {
                dataSource.DeleteItem(arguments.Value);
            }
        }

        private static void OnUpdateItems(IDataSource dataSource, dynamic arguments)
        {
            if (CanHandle(dataSource, arguments))
            {
                dataSource.UpdateItems();
            }
        }

        private static bool CanHandle(IDataSource dataSource, dynamic arguments)
        {
            return (arguments.DataSource == dataSource.GetName());
        }
    }
}