using System;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.DisplayFormats;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.TreeView
{
    internal sealed class TreeViewElementBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var treeView = new TreeViewElement(parent);
            treeView.ApplyElementMeatadata((object) metadata);
            treeView.SetMultiSelect(Convert.ToBoolean(metadata.MultiSelect));
            treeView.SetShowNodeImages(Convert.ToBoolean(metadata.ShowNodeImages));
            treeView.SetKeyProperty(metadata.KeyProperty);
            treeView.SetParentProperty(metadata.ParentProperty);
            treeView.SetImageProperty(metadata.ImageProperty);
            treeView.SetValueProperty(metadata.ValueProperty);
            treeView.SetDisplayProperty(metadata.DisplayProperty);

            // Установка формата отображения элемента

            var itemFormat = context.Build(parent, metadata.ItemFormat) as DataFormat;

            if (itemFormat != null)
            {
                treeView.OnRenderItem += (c, a) => { a.Display = itemFormat.Format(a.Item); };
            }

            // Установка панели навигации по данным

            if (metadata.DataNavigation != null && metadata.DataNavigation.DataNavigation != null
                && metadata.Items != null && metadata.Items.PropertyBinding != null)
            {
                metadata.DataNavigation.DataNavigation.DataSource = metadata.Items.PropertyBinding.DataSource;
            }

            var navigationPanel = context.Build(parent, metadata.DataNavigation);
            treeView.SetDataNavigation(navigationPanel);

            // Установка контекстного меню

            var contextMenu = context.Build(parent, metadata.ContextMenu);
            treeView.SetContextMenu(contextMenu);

            // Привязка к источнику данных списка элементов
            IElementDataBinding itemsDataBinding = context.Build(parent, metadata.Items);
            itemsDataBinding.OnPropertyValueChanged += (c, a) => treeView.SetItems(a.Value);

            // Todo: ReadOnly, Format, Value, OnValueChanged and etc.

            var sourceItemsDataBinding = itemsDataBinding as ISourceDataBinding;

            // Если источник данных для списка элементов определен
            if (sourceItemsDataBinding != null)
            {
                var dataSourceName = sourceItemsDataBinding.GetDataSource();
                var propertyName = sourceItemsDataBinding.GetProperty();
                var dataSource = parent.GetDataSource(dataSourceName);

                if (dataSource != null)
                {
                    treeView.SetIdProperty(dataSource.GetIdProperty());
                }

                // Публикация сообщений в шину при возникновении событий

                treeView.NotifyWhenEventAsync(i => i.OnSetSelectedItem, a =>
                {
                    a.DataSource = dataSourceName;
                    a.Property = propertyName;
                    return true;
                });

                // Подписка на сообщения шины от внешних элементов

                Func<dynamic, bool> filter = a => !Equals(a.Source, treeView)
                                                  && a.DataSource == dataSourceName
                                                  && a.Property == propertyName;

                treeView.SubscribeOnEvent(OnItemDeleted, filter);
                treeView.SubscribeOnEvent(OnSetSelectedItem, filter);
            }

            if (metadata.OnDoubleClick != null)
            {
                treeView.OnDoubleClick += parent.GetScript(metadata.OnDoubleClick);
            }

            return treeView;
        }

        // Обработчики сообщений шины (внимание: наименования обработчиков совпадают с наименованиями событий)

        private static void OnItemDeleted(TreeViewElement treeView, dynamic arguments)
        {
            treeView.RemoveItem(arguments.Value);
        }

        private static void OnSetSelectedItem(TreeViewElement treeView, dynamic arguments)
        {
            treeView.SetSelectedItem(arguments.Value);
        }
    }
}