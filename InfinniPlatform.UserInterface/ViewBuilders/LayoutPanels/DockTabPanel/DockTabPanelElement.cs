using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Docking.Base;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.DockTabPanel
{
    public sealed class DockTabPanelElement : BaseElement<DockTabPanelControl>, ITabPanel
    {
        // HeaderLocation

        private TabHeaderLocation _headerLocation;
        // HeaderOrientation

        private TabHeaderOrientation _headerOrientation;

        public DockTabPanelElement(View view)
            : base(view)
        {
            SetHeaderLocation(TabHeaderLocation.Top);
            SetHeaderOrientation(TabHeaderOrientation.Horizontal);

            Control.SelectedPageChanged += OnSelectedPageChanged;
        }

        /// <summary>
        ///     Возвращает расположение закладок.
        /// </summary>
        public TabHeaderLocation GetHeaderLocation()
        {
            return _headerLocation;
        }

        /// <summary>
        ///     Устанавливает расположение закладок.
        /// </summary>
        public void SetHeaderLocation(TabHeaderLocation value)
        {
            _headerLocation = value;

            switch (value)
            {
                case TabHeaderLocation.None:
                    Control.HeaderLocation = CaptionLocation.Default;
                    break;
                case TabHeaderLocation.Left:
                    Control.HeaderLocation = CaptionLocation.Left;
                    break;
                case TabHeaderLocation.Top:
                    Control.HeaderLocation = CaptionLocation.Top;
                    break;
                case TabHeaderLocation.Right:
                    Control.HeaderLocation = CaptionLocation.Right;
                    break;
                case TabHeaderLocation.Bottom:
                    Control.HeaderLocation = CaptionLocation.Bottom;
                    break;
            }
        }

        /// <summary>
        ///     Возвращает ориентацию закладок.
        /// </summary>
        public TabHeaderOrientation GetHeaderOrientation()
        {
            return _headerOrientation;
        }

        /// <summary>
        ///     Устанавливает ориентацию закладок.
        /// </summary>
        public void SetHeaderOrientation(TabHeaderOrientation value)
        {
            _headerOrientation = value;

            switch (value)
            {
                case TabHeaderOrientation.Horizontal:
                    Control.HeaderOrientation = Orientation.Horizontal;
                    break;
                case TabHeaderOrientation.Vertical:
                    Control.HeaderOrientation = Orientation.Vertical;
                    break;
            }
        }

        // SelectedPage

        /// <summary>
        ///     Возвращает выделенную страницу.
        /// </summary>
        public ITabPage GetSelectedPage()
        {
            var tabPage = Control.SelectedPage;

            if (tabPage != null)
            {
                return (ITabPage) tabPage.Tag;
            }

            return null;
        }

        /// <summary>
        ///     Устанавливает выделенную страницу.
        /// </summary>
        public void SetSelectedPage(ITabPage page)
        {
            var tabPage = page.GetControl<BaseLayoutItem>();

            if (tabPage != null)
            {
                Control.Dispatcher.BeginInvoke((Action) (() =>
                {
                    Control.SelectedPage = tabPage;
                    tabPage.Focus();
                }));
            }
        }

        // Pages

        /// <summary>
        ///     Создает страницу.
        /// </summary>
        public ITabPage CreatePage(View view)
        {
            return new DockTabPageElement(view);
        }

        /// <summary>
        ///     Добавляет указанную страницу.
        /// </summary>
        public void AddPage(ITabPage page)
        {
            var tabPage = page.GetControl<BaseLayoutItem>();
            tabPage.Tag = page;

            Control.AddPage(tabPage);
            page.SetParent(this);
        }

        /// <summary>
        ///     Удаляет указанную страницу.
        /// </summary>
        public void RemovePage(ITabPage page)
        {
            var tabPage = page.GetControl<BaseLayoutItem>();

            Control.RemovePage(tabPage);
            page.SetParent(null);
        }

        /// <summary>
        ///     Возвращает страницу с указанным именем.
        /// </summary>
        public ITabPage GetPage(string name)
        {
            return GetPages().FirstOrDefault(p => p.GetName() == name);
        }

        /// <summary>
        ///     Возвращает список страниц.
        /// </summary>
        public IEnumerable<ITabPage> GetPages()
        {
            return Control.GetPages().Select(p => (ITabPage) p.Tag);
        }

        // Events

        /// <summary>
        ///     Возвращает или устанавливает обработчик события изменения выделенной страницы.
        /// </summary>
        public ScriptDelegate OnSelectionChanged { get; set; }

        // Elements

        public override IEnumerable<IElement> GetChildElements()
        {
            return GetPages();
        }

        private void OnSelectedPageChanged(object sender, DockItemActivatedEventArgs e)
        {
            this.InvokeScript(OnSelectionChanged);
        }
    }
}