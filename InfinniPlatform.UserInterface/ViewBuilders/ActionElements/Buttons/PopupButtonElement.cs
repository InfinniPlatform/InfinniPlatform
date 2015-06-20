using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using InfinniPlatform.UserInterface.ViewBuilders.Actions;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Images;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.Buttons
{
    /// <summary>
    ///     Элемент представления для кнопки со всплывающим окном.
    /// </summary>
    public sealed class PopupButtonElement : BaseElement<PopupButtonControl>
    {
        // Action

        private BaseAction _action;
        // Image

        private string _image;
        // Items

        private readonly List<IElement> _items
            = new List<IElement>();

        public PopupButtonElement(View view)
            : base(view)
        {
            Control.Click += OnClickButton;
        }

        // OnClick

        /// <summary>
        ///     Возвращает или устанавливает обработчик события нажатия на кнопку.
        /// </summary>
        public ScriptDelegate OnClick { get; set; }

        private void OnClickButton(object sender, EventArgs e)
        {
            this.InvokeScript(OnClick);

            var action = GetAction();

            if (action != null)
            {
                action.Execute();
            }
        }

        // Text

        public override void SetText(string value)
        {
            base.SetText(value);

            Control.Text = value;
        }

        /// <summary>
        ///     Возвращает изображение кнопки.
        /// </summary>
        public string GetImage()
        {
            return _image;
        }

        /// <summary>
        ///     Устанавливает изображение кнопки.
        /// </summary>
        public void SetImage(string value)
        {
            _image = value;

            Control.Image = ImageRepository.GetImage(value);
        }

        /// <summary>
        ///     Возвращает действие при нажатии на кнопку.
        /// </summary>
        public BaseAction GetAction()
        {
            return _action;
        }

        /// <summary>
        ///     Устанавливает действие при нажатии на кнопку.
        /// </summary>
        public void SetAction(BaseAction value)
        {
            _action = value;
        }

        // Click

        /// <summary>
        ///     Осуществляет программное нажатие на кнопку.
        /// </summary>
        public void Click()
        {
            Control.PerformClick();
        }

        /// <summary>
        ///     Добавляет элемент во всплывающее окно.
        /// </summary>
        public void AddItem(IElement item)
        {
            _items.Add(item);

            Control.AddItem(item.GetControl<UIElement>());
        }

        /// <summary>
        ///     Удаляет элемент из всплывающего окна.
        /// </summary>
        public void RemoveItem(IElement item)
        {
            _items.Remove(item);

            Control.RemoveItem(item.GetControl<UIElement>());
        }

        /// <summary>
        ///     Возвращает элемент всплывающего окна по имени.
        /// </summary>
        public IElement GetItem(string name)
        {
            return _items.FirstOrDefault(i => i.GetName() == name);
        }

        /// <summary>
        ///     Возвращает элементы всплывающего окна.
        /// </summary>
        public IEnumerable<IElement> GetItems()
        {
            return _items.AsReadOnly();
        }

        // Elements

        public override IEnumerable<IElement> GetChildElements()
        {
            return GetItems();
        }
    }
}