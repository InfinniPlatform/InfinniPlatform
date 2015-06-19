using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Bars;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.DataNavigation
{
    /// <summary>
    ///     Элемент управления для панели навигации по данным.
    /// </summary>
    sealed partial class DataNavigationControl : UserControl
    {
        // PageNumber

        private bool _pageNumberChanging;
        // PageSize

        private bool _pageSizeChanging;

        public DataNavigationControl()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Возвращает или устанавливает количество страниц.
        /// </summary>
        public int? PageCount
        {
            get { return (int?) GetValue(PageCountProperty); }
            set { SetValue(PageCountProperty, value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает номер страницы.
        /// </summary>
        public int PageNumber
        {
            get { return (int) GetValue(PageNumberProperty); }
            set { this.InvokeControl(() => SetValue(PageNumberProperty, value)); }
        }

        /// <summary>
        ///     Возвращает или устанавливает размер страницы.
        /// </summary>
        public int? PageSize
        {
            get { return (int?) GetValue(PageSizeProperty); }
            set { this.InvokeControl(() => SetValue(PageSizeProperty, value)); }
        }

        /// <summary>
        ///     Возвращает или устанавливает список доступных размеров страниц.
        /// </summary>
        public int[] AvailablePageSizes
        {
            get { return (int[]) GetValue(AvailablePageSizesProperty); }
            set { SetValue(AvailablePageSizesProperty, value); }
        }

        /// <summary>
        ///     Событие обновления страницы.
        /// </summary>
        public event RoutedEventHandler OnUpdateItems
        {
            add { AddHandler(OnUpdateItemsEvent, value); }
            remove { RemoveHandler(OnUpdateItemsEvent, value); }
        }

        /// <summary>
        ///     Вызывает событие обновления страницы.
        /// </summary>
        public void PerformUpdateItems()
        {
            UpdateItemsButton.PerformClick();
        }

        private void OnUpdateItemsClick(object sender, ItemClickEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OnUpdateItemsEvent));
        }

        /// <summary>
        ///     Событие изменения количества страниц.
        /// </summary>
        public event RoutedEventHandler OnSetPageCount
        {
            add { AddHandler(OnSetPageCountEvent, value); }
            remove { RemoveHandler(OnSetPageCountEvent, value); }
        }

        private static object OnCoercePageCount(DependencyObject d, object value)
        {
            var control = d as DataNavigationControl;

            if (control != null)
            {
                var pageCount = value as int?;

                value = (pageCount > 0) ? value : null;
            }

            return value;
        }

        private static void OnPageCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DataNavigationControl;

            if (control != null)
            {
                var pageCount = e.NewValue as int?;

                control.PageCountEditor.Content = (pageCount != null) ? string.Format("/ {0}", pageCount) : "/ ?";
                control.RaiseEvent(new RoutedEventArgs(OnSetPageCountEvent));

                if (pageCount != null && pageCount <= control.PageNumber)
                {
                    control.PageNumber = pageCount.Value - 1;
                }
            }
        }

        /// <summary>
        ///     Событие изменения номера страницы.
        /// </summary>
        public event RoutedEventHandler OnSetPageNumber
        {
            add { AddHandler(OnSetPageNumberEvent, value); }
            remove { RemoveHandler(OnSetPageNumberEvent, value); }
        }

        private static object OnCoercePageNumber(DependencyObject d, object value)
        {
            var control = d as DataNavigationControl;

            if (control != null)
            {
                var pageNumber = (int) value;

                if (pageNumber < 0)
                {
                    pageNumber = 0;
                }
                else if (control.PageCount != null && control.PageCount <= pageNumber)
                {
                    pageNumber = control.PageCount.Value - 1;
                }

                value = pageNumber;
            }

            return value;
        }

        private static void OnPageNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DataNavigationControl;

            if (control != null)
            {
                var pageNumber = (int) e.NewValue;

                control._pageNumberChanging = true;

                try
                {
                    control.PageNumberEditor.EditValue = pageNumber + 1;
                }
                finally
                {
                    control._pageNumberChanging = false;
                }

                control.RaiseEvent(new RoutedEventArgs(OnSetPageNumberEvent));
            }
        }

        private void OnFirstPage(object sender, ItemClickEventArgs e)
        {
            PageNumber = 0;
        }

        private void OnPreviousPage(object sender, ItemClickEventArgs e)
        {
            PageNumber--;
        }

        private void OnNextPage(object sender, ItemClickEventArgs e)
        {
            PageNumber++;
        }

        private void OnLastPage(object sender, ItemClickEventArgs e)
        {
            var pageCount = PageCount;

            if (pageCount != null)
            {
                PageNumber = pageCount.Value - 1;
            }
        }

        private void OnPageNumberEditor(object sender, RoutedEventArgs e)
        {
            if (_pageNumberChanging == false)
            {
                var displayPageNumber = 0;

                if (PageNumberEditor.EditValue != null)
                {
                    int.TryParse(PageNumberEditor.EditValue.ToString(), out displayPageNumber);
                }

                PageNumber = (displayPageNumber > 0) ? displayPageNumber - 1 : 0;
            }
        }

        /// <summary>
        ///     Событие изменения размера страницы.
        /// </summary>
        public event RoutedEventHandler OnSetPageSize
        {
            add { AddHandler(OnSetPageSizeEvent, value); }
            remove { RemoveHandler(OnSetPageSizeEvent, value); }
        }

        private static object OnCoercePageSize(DependencyObject d, object value)
        {
            var control = d as DataNavigationControl;

            if (control != null)
            {
                var pageSize = (int?) value;
                var availablePageSizes = control.AvailablePageSizes;

                if (availablePageSizes == null || availablePageSizes.Length == 0)
                {
                    pageSize = null;
                }
                else if (pageSize == null || availablePageSizes.Contains(pageSize.Value) == false)
                {
                    pageSize = control.PageSize;
                }

                value = pageSize;
            }

            return value;
        }

        private static void OnPageSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DataNavigationControl;

            if (control != null)
            {
                var pageSize = (int?) e.NewValue;

                control._pageSizeChanging = true;

                try
                {
                    control.PageSizeEditor.EditValue = pageSize;
                }
                finally
                {
                    control._pageSizeChanging = false;
                }

                control.RaiseEvent(new RoutedEventArgs(OnSetPageSizeEvent));
            }
        }

        private void OnPageSizeEditor(object sender, RoutedEventArgs e)
        {
            if (_pageSizeChanging == false)
            {
                PageSize = (PageSizeEditor.EditValue as int?);
            }
        }

        /// <summary>
        ///     Событие изменения списка доступных размеров страниц.
        /// </summary>
        public event RoutedEventHandler OnSetAvailablePageSizes
        {
            add { AddHandler(OnSetAvailablePageSizesEvent, value); }
            remove { RemoveHandler(OnSetAvailablePageSizesEvent, value); }
        }

        private static object OnCoerceAvailablePageSizes(DependencyObject d, object value)
        {
            var control = d as DataNavigationControl;

            if (control != null)
            {
                var availablePageSizes = (int[]) value;

                value = (availablePageSizes != null)
                    ? availablePageSizes.Where(i => i > 0).OrderBy(i => i).ToArray()
                    : null;
            }

            return value;
        }

        private static void OnAvailablePageSizesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DataNavigationControl;

            if (control != null)
            {
                var pageSize = control.PageSize;

                var availablePageSizes = (int[]) e.NewValue;
                control.PageSizeEditorList.ItemsSource = availablePageSizes;
                control.RaiseEvent(new RoutedEventArgs(OnSetAvailablePageSizesEvent));

                if (availablePageSizes == null || availablePageSizes.Length == 0)
                {
                    control.PageSizePanel.Visibility = Visibility.Collapsed;

                    control.PageSize = null;
                }
                else
                {
                    control.PageSizePanel.Visibility = Visibility.Visible;

                    if (pageSize == null || availablePageSizes.Contains(pageSize.Value) == false)
                    {
                        control.PageSize = availablePageSizes.FirstOrDefault();
                    }
                }
            }
        }

        // UpdateItems

        public static readonly RoutedEvent OnUpdateItemsEvent = EventManager.RegisterRoutedEvent("OnUpdateItems",
            RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (DataNavigationControl));

        // PageCount

        public static readonly DependencyProperty PageCountProperty = DependencyProperty.Register("PageCount",
            typeof (int?), typeof (DataNavigationControl),
            new FrameworkPropertyMetadata(null, OnPageCountChanged, OnCoercePageCount));

        public static readonly RoutedEvent OnSetPageCountEvent = EventManager.RegisterRoutedEvent("OnSetPageCount",
            RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (DataNavigationControl));

        public static readonly DependencyProperty PageNumberProperty = DependencyProperty.Register("PageNumber",
            typeof (int), typeof (DataNavigationControl),
            new FrameworkPropertyMetadata(0, OnPageNumberChanged, OnCoercePageNumber));

        public static readonly RoutedEvent OnSetPageNumberEvent = EventManager.RegisterRoutedEvent("OnSetPageNumber",
            RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (DataNavigationControl));

        public static readonly DependencyProperty PageSizeProperty = DependencyProperty.Register("PageSize",
            typeof (int?), typeof (DataNavigationControl),
            new FrameworkPropertyMetadata(null, OnPageSizeChanged, OnCoercePageSize));

        public static readonly RoutedEvent OnSetPageSizeEvent = EventManager.RegisterRoutedEvent("OnSetPageSize",
            RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (DataNavigationControl));

        // AvailablePageSizes

        public static readonly DependencyProperty AvailablePageSizesProperty =
            DependencyProperty.Register("AvailablePageSizes", typeof (int[]), typeof (DataNavigationControl),
                new FrameworkPropertyMetadata(null, OnAvailablePageSizesChanged, OnCoerceAvailablePageSizes));

        public static readonly RoutedEvent OnSetAvailablePageSizesEvent =
            EventManager.RegisterRoutedEvent("OnSetAvailablePageSizes", RoutingStrategy.Bubble,
                typeof (RoutedEventHandler), typeof (DataNavigationControl));
    }
}