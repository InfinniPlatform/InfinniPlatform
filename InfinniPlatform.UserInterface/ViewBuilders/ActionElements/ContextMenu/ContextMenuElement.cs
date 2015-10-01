using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ContextMenu
{
    /// <summary>
    ///     Элемент представления для контекстного меню.
    /// </summary>
    public sealed class ContextMenuElement : BaseElement<System.Windows.Controls.ContextMenu>
    {
        // Items

        private readonly ObservableCollection<FrameworkElement> _items;

        public ContextMenuElement(View view)
            : base(view)
        {
            _items = new ObservableCollection<FrameworkElement>();

            Control.ItemsSource = _items;

            Control.ContextMenuOpening += OnOpeningHandler;
            Control.Opened += OnOpenedHandler;

            Control.ContextMenuClosing += OnClosingHandler;
            Control.Closed += OnClosedHandler;
        }

        // Events

        /// <summary>
        ///     Возвращает или устанавливает обработчик события попытки открытия контекстного меню.
        /// </summary>
        public ScriptDelegate OnOpening { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает обработчик события открытия контекстного меню.
        /// </summary>
        public ScriptDelegate OnOpened { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает обработчик события попытки закрытия контекстного меню.
        /// </summary>
        public ScriptDelegate OnClosing { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает обработчик события закрытия контекстного меню.
        /// </summary>
        public ScriptDelegate OnClosed { get; set; }

        private void OnOpeningHandler(object sender, ContextMenuEventArgs e)
        {
            dynamic arguments = null;

            this.InvokeScript(OnOpening, a => arguments = a);

            if (arguments != null && arguments.IsCancel == true)
            {
                e.Handled = true;
            }
        }

        private void OnOpenedHandler(object sender, RoutedEventArgs e)
        {
            this.InvokeScript(OnOpened);
        }

        private void OnClosingHandler(object sender, ContextMenuEventArgs e)
        {
            dynamic arguments = null;

            this.InvokeScript(OnClosing, a => arguments = a);

            if (arguments != null && arguments.IsCancel == true)
            {
                e.Handled = true;
            }
        }

        private void OnClosedHandler(object sender, RoutedEventArgs e)
        {
            this.InvokeScript(OnClosed);
        }

        /// <summary>
        ///     Добавляет элемент в список.
        /// </summary>
        public void AddItem(IContextMenuItem item)
        {
            _items.Add(item.GetControl<FrameworkElement>());
        }

        /// <summary>
        ///     Удаляет элемент из списка.
        /// </summary>
        public void RemoveItem(IContextMenuItem item)
        {
            _items.Remove(item.GetControl<FrameworkElement>());
        }

        /// <summary>
        ///     Возвращает элемент по имени.
        /// </summary>
        public IContextMenuItem GetItem(string name)
        {
            var itemControl = _items.FirstOrDefault(i => i.Name == name);

            return (itemControl != null) ? GetMenuItem(itemControl) : null;
        }

        /// <summary>
        ///     Возвращает список элементов.
        /// </summary>
        public IEnumerable<IContextMenuItem> GetItems()
        {
            return _items.Select(GetMenuItem).ToArray();
        }

        // Methods

        public void Open()
        {
            Control.IsOpen = true;
        }

        public void Close()
        {
            Control.IsOpen = false;
        }

        // Elements

        public static IContextMenuItem GetMenuItem(FrameworkElement control)
        {
            return control.Tag as IContextMenuItem;
        }

        public override IEnumerable<IElement> GetChildElements()
        {
            return GetItems();
        }
    }
}