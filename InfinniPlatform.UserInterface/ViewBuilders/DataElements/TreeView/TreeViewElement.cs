using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.TreeView
{
    /// <summary>
    ///     Элемент представления для древовидного списка.
    /// </summary>
    public sealed class TreeViewElement : BaseElement<TreeViewControl>
    {
        // ContextMenu

        private IElement _contextMenu;
        // DataNavigation

        private IElement _dataNavigationPanel;

        public TreeViewElement(View view)
            : base(view)
        {
            Control.OnSetSelectedItem += OnSetSelectedItemHandler;
            Control.OnDoubleClick += OnDoubleClickHandler;
            Control.OnRenderItem += OnRenderItemHandler;
        }

        // Events

        /// <summary>
        ///     Возвращает или устанавливает обработчик события запроса на выделение элемента в списке.
        /// </summary>
        public ScriptDelegate OnSetSelectedItem { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает обработчик события двойного клика по элементу списка.
        /// </summary>
        public ScriptDelegate OnDoubleClick { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает обработчик события отрисовки элемента.
        /// </summary>
        public ScriptDelegate OnRenderItem { get; set; }

        private void OnSetSelectedItemHandler(object sender, RoutedEventArgs e)
        {
            InvokeEventHandler(OnSetSelectedItem, Control.SelectedItem);
        }

        private void OnDoubleClickHandler(object sender, RoutedEventArgs e)
        {
            InvokeEventHandler(OnDoubleClick, Control.SelectedItem);
        }

        private void InvokeEventHandler(ScriptDelegate handler, object value)
        {
            this.InvokeScript(handler, args => { args.Value = value; });
        }

        private void OnRenderItemHandler(object sender, RenderItemRoutedEventArgs e)
        {
            dynamic arguments = null;

            this.InvokeScript(OnRenderItem, args =>
            {
                args.Item = e.Item;
                args.Value = e.Value;
                args.Display = e.Display;
                arguments = args;
            });

            if (arguments != null)
            {
                e.Item = arguments.Item;
                e.Value = arguments.Value;
                e.Display = arguments.Display;
            }
        }

        // MultiSelect

        /// <summary>
        ///     Возвращает значение, определяющее, возможен ли выбор нескольких значений.
        /// </summary>
        public bool GetMultiSelect()
        {
            return Control.MultiSelect;
        }

        /// <summary>
        ///     Устанавливает значение, определяющее, возможен ли выбор нескольких значений.
        /// </summary>
        public void SetMultiSelect(bool? value)
        {
            Control.MultiSelect = value ?? false;
        }

        // ShowNodeImages

        /// <summary>
        ///     Возвращает значение, определяющее, нужно ли отображать изображения элементов.
        /// </summary>
        public bool GetShowNodeImages()
        {
            return Control.ShowNodeImages;
        }

        /// <summary>
        ///     Устанавливает значение, определяющее, нужно ли отображать изображения элементов.
        /// </summary>
        public void SetShowNodeImages(bool? value)
        {
            Control.ShowNodeImages = value ?? false;
        }

        // IdProperty

        /// <summary>
        ///     Возвращает свойство элемента источника данных, которое хранит уникальный идентификатор элемента.
        /// </summary>
        public string GetIdProperty()
        {
            return Control.IdProperty;
        }

        /// <summary>
        ///     Устанавливает свойство элемента источника данных, которое хранит уникальный идентификатор элемента.
        /// </summary>
        public void SetIdProperty(string value)
        {
            Control.IdProperty = value;
        }

        // KeyProperty

        /// <summary>
        ///     Возвращает свойство элемента источника данных, которое хранит идентификатор элемента.
        /// </summary>
        public string GetKeyProperty()
        {
            return Control.KeyProperty;
        }

        /// <summary>
        ///     Устанавливает свойство элемента источника данных, которое хранит идентификатор элемента.
        /// </summary>
        public void SetKeyProperty(string value)
        {
            Control.KeyProperty = value;
        }

        // ParentProperty

        /// <summary>
        ///     Возвращает свойство элемента источника данных, которое хранит идентификатор родителя.
        /// </summary>
        public string GetParentProperty()
        {
            return Control.ParentProperty;
        }

        /// <summary>
        ///     Устанавливает свойство элемента источника данных, которое хранит идентификатор родителя.
        /// </summary>
        public void SetParentProperty(string value)
        {
            Control.ParentProperty = value;
        }

        // ValueProperty

        /// <summary>
        ///     Возвращает свойство элемента источника данных, которое хранит значение элемента.
        /// </summary>
        public string GetValueProperty()
        {
            return Control.ValueProperty;
        }

        /// <summary>
        ///     Устанавливает свойство элемента источника данных, которое хранит значение элемента.
        /// </summary>
        public void SetValueProperty(string value)
        {
            Control.ValueProperty = value;
        }

        // DisplayProperty

        /// <summary>
        ///     Возвращает свойство элемента источника данных, которое хранит наименование элемента.
        /// </summary>
        public string GetDisplayProperty()
        {
            return Control.DisplayProperty;
        }

        /// <summary>
        ///     Устанавливает свойство элемента источника данных, которое хранит наименование элемента.
        /// </summary>
        public void SetDisplayProperty(string value)
        {
            Control.DisplayProperty = value;
        }

        // ImageProperty

        /// <summary>
        ///     Возвращает свойство элемента источника данных, которое хранит изображение элемента.
        /// </summary>
        public string GetImageProperty()
        {
            return Control.ImageProperty;
        }

        /// <summary>
        ///     Устанавливает свойство элемента источника данных, которое хранит изображение элемента.
        /// </summary>
        public void SetImageProperty(string value)
        {
            Control.ImageProperty = value;
        }

        // Items

        /// <summary>
        ///     Добавляет элемент.
        /// </summary>
        public void AddItem(object item)
        {
            Control.AddItem(item);
        }

        /// <summary>
        ///     Удаляет элемент.
        /// </summary>
        public void RemoveItem(object item)
        {
            Control.RemoveItem(item);
        }

        /// <summary>
        ///     Удаляет элемент и все вложенные.
        /// </summary>
        public void RemoveItem(object item, bool removeChildren)
        {
            Control.RemoveItem(item, removeChildren);
        }

        /// <summary>
        ///     Возвращает список элементов.
        /// </summary>
        public IEnumerable GetItems()
        {
            return Control.GetItems();
        }

        /// <summary>
        ///     Устанавливает список элементов.
        /// </summary>
        public void SetItems(IEnumerable items)
        {
            Control.SetItems(items);
        }

        /// <summary>
        ///     Обновляет указанный элемент.
        /// </summary>
        public void RefreshItem(object item)
        {
            Control.RefreshItem(item);
        }

        /// <summary>
        ///     Обновляет список элементов.
        /// </summary>
        public void RefreshItems()
        {
            Control.RefreshItems();
        }

        /// <summary>
        ///     Перемещает указанный элемент.
        /// </summary>
        public void MoveItem(object item, int delta)
        {
            Control.MoveItem(item, delta);
        }

        /// <summary>
        ///     Разворачивает указанный элемент.
        /// </summary>
        public void ExpandItem(object item)
        {
            Control.ExpandItem(item);
        }

        /// <summary>
        ///     Сворачивает указанный элемент.
        /// </summary>
        public void CollapseItem(object item)
        {
            Control.CollapseItem(item);
        }

        // SelectedItem

        /// <summary>
        ///     Возвращает выделенный элемент.
        /// </summary>
        public object GetSelectedItem()
        {
            return Control.SelectedItem;
        }

        /// <summary>
        ///     Устанавливает выделенный элемент.
        /// </summary>
        public void SetSelectedItem(object value)
        {
            Control.SelectedItem = value;
        }

        /// <summary>
        ///     Возвращает панель навигации по данным.
        /// </summary>
        public IElement GetDataNavigation()
        {
            return _dataNavigationPanel;
        }

        /// <summary>
        ///     Устанавливает панель навигации по данным.
        /// </summary>
        public void SetDataNavigation(IElement value)
        {
            _dataNavigationPanel = value;

            Control.DataNavigation = value.GetControl<UIElement>();
        }

        /// <summary>
        ///     Возвращает контекстное меню.
        /// </summary>
        public IElement GetContextMenu()
        {
            return _contextMenu;
        }

        /// <summary>
        ///     Устанавливает контекстное меню.
        /// </summary>
        public void SetContextMenu(IElement value)
        {
            _contextMenu = value;

            Control.ContextMenu = value.GetControl<ContextMenu>();
        }

        // Elements

        public override IEnumerable<IElement> GetChildElements()
        {
            return (_dataNavigationPanel != null) ? new[] {_dataNavigationPanel} : null;
        }
    }
}