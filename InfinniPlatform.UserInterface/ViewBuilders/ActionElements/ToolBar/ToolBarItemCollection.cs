using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpf.Bars;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ToolBar
{
    /// <summary>
    ///     Коллекция элементов панели инструментов.
    /// </summary>
    internal sealed class ToolBarItemCollection : IEnumerable<IToolBarItem>
    {
        private readonly List<IToolBarItem> _items = new List<IToolBarItem>();
        private readonly dynamic _itemsParent;

        /// <summary>
        ///     Конструктор.
        /// </summary>
        /// <param name="itemsParent">Родительский контейнер для элементов.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ToolBarItemCollection(dynamic itemsParent)
        {
            if (itemsParent == null)
            {
                throw new ArgumentNullException("itemsParent");
            }

            _itemsParent = itemsParent;
        }

        public IEnumerator<IToolBarItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Добавляет элемент в список.
        /// </summary>
        public void AddItem(IToolBarItem item)
        {
            _items.Add(item);

            // Добавление элемента

            var barItem = AddToolBarItem(item);

            _itemsParent.ItemLinks.Add(barItem);

            // Добавление дочерних элементов

            var subItems = item.GetAllChildElements();

            if (subItems != null)
            {
                foreach (var subItem in subItems.OfType<IToolBarItem>())
                {
                    AddToolBarItem(subItem);
                }
            }
        }

        private BarItem AddToolBarItem(IToolBarItem item)
        {
            var barItem = item.GetControl<BarItem>();

            // Регистрация элемента в менеджере

            BarManager itemsManager = _itemsParent.Manager;

            if (itemsManager != null)
            {
                itemsManager.Items.Add(barItem);
            }

            return barItem;
        }

        /// <summary>
        ///     Удаляет элемент из списка.
        /// </summary>
        public void RemoveItem(IToolBarItem item)
        {
            _items.Remove(item);

            // Удаление дочерних элементов

            var subItems = item.GetAllChildElements();

            if (subItems != null)
            {
                foreach (var subItem in subItems.OfType<IToolBarItem>())
                {
                    RemoveToolBarItem(subItem);
                }
            }

            // Удаление элемента

            RemoveToolBarItem(item);
        }

        private void RemoveToolBarItem(IToolBarItem item)
        {
            var barItem = item.GetControl<BarItem>();
            var barItemLinks = barItem.Links.ToArray();

            // Удаление всех ссылок на элемент
            foreach (var barItemLink in barItemLinks)
            {
                dynamic parent = barItemLink.Parent;

                if (parent != null)
                {
                    parent.ItemLinks.Remove(barItemLink);
                }
            }

            // Удаление элемента из менеджера

            BarManager itemsManager = _itemsParent.Manager;

            if (itemsManager != null)
            {
                itemsManager.Items.Remove(barItem);
            }
        }

        /// <summary>
        ///     Возвращает элемент по имени.
        /// </summary>
        public IToolBarItem GetItem(string name)
        {
            return _items.FirstOrDefault(i => i.GetName() == name);
        }

        /// <summary>
        ///     Возвращает список элементов.
        /// </summary>
        public IEnumerable<IToolBarItem> GetItems()
        {
            return _items.AsReadOnly();
        }
    }
}