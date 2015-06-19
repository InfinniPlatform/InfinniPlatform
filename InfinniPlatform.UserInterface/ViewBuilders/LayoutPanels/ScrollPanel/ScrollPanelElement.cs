using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.ScrollPanel
{
    /// <summary>
    ///     Контейнер элементов представления в виде области, содержимое которой может прокручиваться по вертикали и
    ///     горизонтали.
    /// </summary>
    public sealed class ScrollPanelElement : BaseElement<ScrollViewer>, ILayoutPanel
    {
        // HorizontalScroll

        private ScrollVisibility _horizontalScroll;
        // LayoutPanel

        private ILayoutPanel _layoutPanel;
        // VerticalScroll

        private ScrollVisibility _verticalScroll;

        public ScrollPanelElement(View view)
            : base(view)
        {
            SetVerticalScroll(ScrollVisibility.Auto);
            SetHorizontalScroll(ScrollVisibility.Auto);
        }

        // Elements

        public override IEnumerable<IElement> GetChildElements()
        {
            var layoutPanel = GetLayoutPanel();

            if (layoutPanel != null)
            {
                return new[] {layoutPanel};
            }

            return null;
        }

        /// <summary>
        ///     Возвращает видимость полосы прокрутки по вертикали.
        /// </summary>
        public ScrollVisibility GetVerticalScroll()
        {
            return _verticalScroll;
        }

        /// <summary>
        ///     Устанавливает видимость полосы прокрутки по вертикали.
        /// </summary>
        public void SetVerticalScroll(ScrollVisibility value)
        {
            _verticalScroll = value;

            switch (value)
            {
                case ScrollVisibility.Auto:
                    Control.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                    break;
                case ScrollVisibility.Visible:
                    Control.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                    break;
                case ScrollVisibility.Hidden:
                    Control.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    break;
            }
        }

        /// <summary>
        ///     Возвращает видимость полосы прокрутки по горизонтали.
        /// </summary>
        public ScrollVisibility GetHorizontalScroll()
        {
            return _horizontalScroll;
        }

        /// <summary>
        ///     Устанавливает видимость полосы прокрутки по горизонтали.
        /// </summary>
        public void SetHorizontalScroll(ScrollVisibility value)
        {
            _horizontalScroll = value;

            switch (value)
            {
                case ScrollVisibility.Auto:
                    Control.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                    break;
                case ScrollVisibility.Visible:
                    Control.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                    break;
                case ScrollVisibility.Hidden:
                    Control.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    break;
            }
        }

        /// <summary>
        ///     Возвращает контейнер элементов.
        /// </summary>
        public ILayoutPanel GetLayoutPanel()
        {
            return _layoutPanel;
        }

        /// <summary>
        ///     Устанавливает контейнер элементов.
        /// </summary>
        public void SetLayoutPanel(ILayoutPanel value)
        {
            _layoutPanel = value;

            Control.Content = value.GetControl<UIElement>();
        }
    }
}