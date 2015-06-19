using System.Collections.Generic;
using InfinniPlatform.Sdk.Application.Dynamic;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Actions
{
    /// <summary>
    ///     Действие для редактирования списка.
    /// </summary>
    public sealed class BaseItemAction : BaseAction
    {
        // Items

        private object _items;
        // SelectedItem

        private object _selectedItem;

        public BaseItemAction(View view)
            : base(view)
        {
        }

        // Events

        /// <summary>
        ///     Возвращает или устанавливает обработчик события изменения значения.
        /// </summary>
        public ScriptDelegate OnValueChanged { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает обработчик события запроса на выделение элемента в списке.
        /// </summary>
        public ScriptDelegate OnSetSelectedItem { get; set; }

        /// <summary>
        ///     Возвращает текущий элемент в списке.
        /// </summary>
        public object GetSelectedItem()
        {
            return _selectedItem;
        }

        /// <summary>
        ///     Устанавливает текущий элемент в списке.
        /// </summary>
        public void SetSelectedItem(object value)
        {
            if (Equals(_selectedItem, value) == false)
            {
                _selectedItem = value;

                InvokeEventHandler(OnSetSelectedItem, value);
            }
        }

        /// <summary>
        ///     Возвращает список элементов.
        /// </summary>
        public object GetItems()
        {
            return _items;
        }

        /// <summary>
        ///     Устанавливает список элементов.
        /// </summary>
        public void SetItems(object value)
        {
            _items = value;
        }

        /// <summary>
        ///     Добавляет элемент в список.
        /// </summary>
        public void AddItem(object item)
        {
            if (_items == null)
            {
                _items = new List<object>();
            }

            _items.AddItem(item);

            InvokeEventHandler(OnValueChanged, _items);
        }

        /// <summary>
        ///     Заменяет элемент в списке.
        /// </summary>
        public void ReplaceItem(object item, object newItem)
        {
            if (_items == null)
            {
                _items = new List<object>();
            }

            _items.ReplaceItem(item, newItem);

            InvokeEventHandler(OnValueChanged, _items);
        }

        /// <summary>
        ///     Удаляет элемент из списка.
        /// </summary>
        public void RemoveItem(object item)
        {
            if (_items != null)
            {
                _items.RemoveItem(item);

                InvokeEventHandler(OnValueChanged, _items);
            }
        }

        private void InvokeEventHandler(ScriptDelegate handler, object value)
        {
            this.InvokeScript(handler, args => { args.Value = value; });
        }
    }
}