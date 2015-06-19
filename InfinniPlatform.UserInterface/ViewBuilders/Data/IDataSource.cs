using System.Collections;
using System.Collections.Generic;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data
{
    /// <summary>
    ///     Источник данных представления.
    /// </summary>
    public interface IDataSource : IViewChild
    {
        // Events

        /// <summary>
        ///     Возвращает или устанавливает обработчик события изменения номер страницы.
        /// </summary>
        ScriptDelegate OnPageNumberChanged { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает обработчик события изменения размера страницы.
        /// </summary>
        ScriptDelegate OnPageSizeChanged { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает обработчик события изменения выделенного элемента.
        /// </summary>
        ScriptDelegate OnSelectedItemChanged { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает обработчик события изменения фильтра по свойствам элементов.
        /// </summary>
        ScriptDelegate OnPropertyFiltersChanged { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает обработчик события изменения фильтра полнотекстового поиска.
        /// </summary>
        ScriptDelegate OnTextFilterChanged { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает обработчик события сохранения элемента в источнике.
        /// </summary>
        ScriptDelegate OnItemSaved { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает обработчик события удаления элемента из источника.
        /// </summary>
        ScriptDelegate OnItemDeleted { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает обработчик события обновления списка элементов.
        /// </summary>
        ScriptDelegate OnItemsUpdated { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает обработчик события возникновения ошибки.
        /// </summary>
        ScriptDelegate OnError { get; set; }

        // Metadata

        /// <summary>
        ///     Возвращает наименование источника данных.
        /// </summary>
        string GetName();

        /// <summary>
        ///     Устанавливает наименование источника данных.
        /// </summary>
        void SetName(string value);

        /// <summary>
        ///     Возвращает свойство элемента источника данных, которое хранит уникальный идентификатор элемента.
        /// </summary>
        string GetIdProperty();

        /// <summary>
        ///     Возвращает значение, определяющее нужно ли заполнять создаваемые элементы источника данных значениями по умолчанию.
        /// </summary>
        bool GetFillCreatedItem();

        /// <summary>
        ///     Устанавливает значение, определяющее, нужно ли заполнять создаваемые элементы источника данных значениями по
        ///     умолчанию.
        /// </summary>
        void SetFillCreatedItem(bool value);

        /// <summary>
        ///     Возвращает идентификатор конфигурации элементов источника данных.
        /// </summary>
        string GetConfigId();

        /// <summary>
        ///     Устанавливает идентификатор конфигурации элементов источника данных.
        /// </summary>
        void SetConfigId(string value);

        /// <summary>
        ///     Возвращает идентфикатор документа элементов источника данных.
        /// </summary>
        string GetDocumentId();

        /// <summary>
        ///     Устанавливает идентфикатор документа элементов источника данных.
        /// </summary>
        void SetDocumentId(string value);

        /// <summary>
        ///     Возвращает идентификатор версии конфигурации
        /// </summary>
        /// <returns></returns>
        string GetVersion();

        /// <summary>
        ///     Устанавливает идентификатор версии элемента источника данных
        /// </summary>
        /// <param name="version"></param>
        void SetVersion(string version);

        // State

        /// <summary>
        ///     Запрещает обновление списка элементов.
        /// </summary>
        void SuspendUpdate();

        /// <summary>
        ///     Разрешает обновление списка элементов.
        /// </summary>
        void ResumeUpdate();

        // Pages

        /// <summary>
        ///     Возвращает номер страницы.
        /// </summary>
        int GetPageNumber();

        /// <summary>
        ///     Устанавливает номер страницы.
        /// </summary>
        void SetPageNumber(int value);

        /// <summary>
        ///     Возвращает размер страницы.
        /// </summary>
        int GetPageSize();

        /// <summary>
        ///     Устанавливает размер страницы.
        /// </summary>
        void SetPageSize(int value);

        // Modified

        /// <summary>
        ///     Возвращает значение, определяющее, есть ли не сохраненные элементы в источнике данных.
        /// </summary>
        bool IsModified();

        /// <summary>
        ///     Возвращает значение, определяющее, есть ли не сохраненные изменения у элемента источника данных.
        /// </summary>
        bool IsModified(object item);

        /// <summary>
        ///     Устанавливает признак изменения элемента источника данных.
        /// </summary>
        void SetModified(object item);

        /// <summary>
        ///     Сбрасывает признак изменения элемента источника данных.
        /// </summary>
        void ResetModified(object item);

        // Items

        /// <summary>
        ///     Сохраняет элемент в источник данных.
        /// </summary>
        void SaveItem(object item);

        /// <summary>
        ///     Удаляет элемент из источника данных.
        /// </summary>
        void DeleteItem(string itemId);

        /// <summary>
        ///     Возвращает список элементов источника данных.
        /// </summary>
        IEnumerable GetItems();

        /// <summary>
        ///     Обновляет список элементов источника данных.
        /// </summary>
        void UpdateItems();

        // SelectedItem

        /// <summary>
        ///     Возвращает выделенный элемент.
        /// </summary>
        object GetSelectedItem();

        /// <summary>
        ///     Устанавливает выделенный элемент.
        /// </summary>
        void SetSelectedItem(object value);

        // Filters

        /// <summary>
        ///     Устанваливает режим работы источника данных для представлений с редактором элемента.
        /// </summary>
        void SetEditMode();

        /// <summary>
        ///     Устанваливает режим работы источника данных для представлений со списком элементов.
        /// </summary>
        void SetListMode();

        /// <summary>
        ///     Возвращает фильтр по уникальному идентификатору элемента.
        /// </summary>
        string GetIdFilter();

        /// <summary>
        ///     Устанавливает фильтр по уникальному идентификатору элемента.
        /// </summary>
        void SetIdFilter(string value);

        /// <summary>
        ///     Возвращает фильтр по свойствам элементов.
        /// </summary>
        IEnumerable GetPropertyFilters();

        /// <summary>
        ///     Устанавливает фильтр по свойствам элементов.
        /// </summary>
        void SetPropertyFilters(IEnumerable value);

        /// <summary>
        ///     Возвращает фильтр полнотекстового поиска.
        /// </summary>
        string GetTextFiliter();

        /// <summary>
        ///     Устанавливает фильтр полнотекстового поиска.
        /// </summary>
        void SetTextFilter(string value);

        // DataBindings

        /// <summary>
        ///     Возвращает список привязок источника данных.
        /// </summary>
        IEnumerable<ISourceDataBinding> GetDataBindings();

        /// <summary>
        ///     Добавляет привязку в список привязок источника данных.
        /// </summary>
        void AddDataBinding(ISourceDataBinding dataBinding);

        /// <summary>
        ///     Удаляет привязку из списка привязок источника данных.
        /// </summary>
        void RemoveDataBinding(ISourceDataBinding dataBinding);
    }
}