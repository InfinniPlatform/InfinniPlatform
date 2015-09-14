using System.Windows;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.DataNavigation
{
    /// <summary>
    ///     Элемент представления для панели навигации по данным.
    /// </summary>
    public sealed class DataNavigationElement : BaseElement<DataNavigationControl>
    {
        // DataSource

        private string _dataSource;

        public DataNavigationElement(View view)
            : base(view)
        {
            Control.OnUpdateItems += OnUpdateItemsHandler;
            Control.OnSetPageNumber += OnSetPageNumberHandler;
            Control.OnSetPageSize += OnSetPageSizeHandler;
        }

        // OnUpdateItems

        /// <summary>
        ///     Возвращает или устанавливает обработчик события запроса на обновление страницы.
        /// </summary>
        public ScriptDelegate OnUpdateItems { get; set; }

        // PageNumber

        /// <summary>
        ///     Возвращает или устанавливает обработчик события запроса на переход к заданной странице.
        /// </summary>
        public ScriptDelegate OnSetPageNumber { get; set; }

        // PageSize

        /// <summary>
        ///     Возвращает или устанавливает обработчик события запроса на установку заданного размера страницы.
        /// </summary>
        public ScriptDelegate OnSetPageSize { get; set; }

        private void OnUpdateItemsHandler(object sender, RoutedEventArgs e)
        {
            InvokeEventHandler(OnUpdateItems, null);
        }

        private void OnSetPageNumberHandler(object sender, RoutedEventArgs e)
        {
            InvokeEventHandler(OnSetPageNumber, Control.PageNumber);
        }

        private void OnSetPageSizeHandler(object sender, RoutedEventArgs e)
        {
            InvokeEventHandler(OnSetPageSize, Control.PageSize);
        }

        private void InvokeEventHandler(ScriptDelegate handler, object value)
        {
            this.InvokeScript(handler, args =>
            {
                args.DataSource = GetDataSource();
                args.Value = value;
            });
        }

        /// <summary>
        ///     Возвращает наименование источника данных.
        /// </summary>
        public string GetDataSource()
        {
            return _dataSource;
        }

        /// <summary>
        ///     Устанавливает наименование источника данных.
        /// </summary>
        public void SetDataSource(string value)
        {
            _dataSource = value;
        }

        /// <summary>
        ///     Осуществляет программное нажатие на кнопку обновления страницы.
        /// </summary>
        public void UpdateItems()
        {
            Control.PerformUpdateItems();
        }

        // PageCount

        /// <summary>
        ///     Возвращает количество страниц.
        /// </summary>
        public int? GetPageCount()
        {
            return Control.PageCount;
        }

        /// <summary>
        ///     Устанавливает количество страниц.
        /// </summary>
        public void SetPageCount(int? value)
        {
            Control.PageCount = value;
        }

        /// <summary>
        ///     Возвращает номер страницы.
        /// </summary>
        public int GetPageNumber()
        {
            return Control.PageNumber;
        }

        /// <summary>
        ///     Устанавливает номер страницы.
        /// </summary>
        public void SetPageNumber(int value)
        {
            Control.PageNumber = value;
        }

        /// <summary>
        ///     Возвращает размер страницы.
        /// </summary>
        public int? GetPageSize()
        {
            return Control.PageSize;
        }

        /// <summary>
        ///     Устанавливает размер страницы.
        /// </summary>
        public void SetPageSize(int? value)
        {
            Control.PageSize = value;
        }

        // AvailablePageSizes

        /// <summary>
        ///     Возвращает список доступных размеров страниц.
        /// </summary>
        public int[] GetAvailablePageSizes()
        {
            return Control.AvailablePageSizes;
        }

        /// <summary>
        ///     Устанавливает список доступных размеров страниц.
        /// </summary>
        public void SetAvailablePageSizes(int[] value)
        {
            Control.AvailablePageSizes = value;
        }
    }
}