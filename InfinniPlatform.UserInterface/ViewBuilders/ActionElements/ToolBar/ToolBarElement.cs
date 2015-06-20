using System.Collections.Generic;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ToolBar
{
    /// <summary>
    ///     Элемент представления для панели инструментов.
    /// </summary>
    public sealed class ToolBarElement : BaseElement<BarManager>
    {
        // Items

        private readonly ToolBarItemCollection _items;

        public ToolBarElement(View view)
            : base(view)
        {
            Control.Height = 25;
            Control.AllowCustomization = false;
            Control.AllowHotCustomization = false;
            Control.AllowQuickCustomization = false;
            Control.ToolbarGlyphSize = GlyphSize.Small;

            var bar = new Bar
            {
                AllowDrop = false,
                ShowDragWidget = false,
                AllowCustomizationMenu = false,
                AllowHide = DefaultBoolean.False,
                UseWholeRow = DefaultBoolean.False,
                AllowQuickCustomization = DefaultBoolean.False,
                DockInfo = {ContainerType = BarContainerType.Top}
            };

            Control.Bars.Add(bar);

            _items = new ToolBarItemCollection(bar);
        }

        /// <summary>
        ///     Добавляет элемент в список.
        /// </summary>
        public void AddItem(IToolBarItem item)
        {
            _items.AddItem(item);
        }

        /// <summary>
        ///     Удаляет элемент из списка.
        /// </summary>
        public void RemoveItem(IToolBarItem item)
        {
            _items.RemoveItem(item);
        }

        /// <summary>
        ///     Возвращает элемент по имени.
        /// </summary>
        public IToolBarItem GetItem(string name)
        {
            return _items.GetItem(name);
        }

        /// <summary>
        ///     Возвращает список элементов.
        /// </summary>
        public IEnumerable<IToolBarItem> GetItems()
        {
            return _items.GetItems();
        }

        // Elements

        public override IEnumerable<IElement> GetChildElements()
        {
            return GetItems();
        }
    }
}