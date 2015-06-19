using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Sdk.Application.Dynamic;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data.DataSources
{
    /// <summary>
    ///     Базовый класс источника данных представления.
    /// </summary>
    public sealed class DataSource : IDataSource
    {
        // FillCreatedItem

        private bool _fillCreatedItem;
        private string _idFilter;
        // Name

        private string _name;
        // Pages

        private int _pageNumber;
        private int _pageSize = 20;
        private IEnumerable _propertyFilters;
        // SelectedItem

        private object _selectedItem;
        // Filters

        private BaseDataSourceStrategy _strategy;
        // State

        private bool _suspended;
        private string _textFilter;
        // DataBindings

        private readonly List<ISourceDataBinding> _dataBindings
            = new List<ISourceDataBinding>();

        private readonly ConcurrentCollection _dataItems;
        private readonly IDataProvider _dataProvider;
        private readonly BaseDataSourceStrategy _editStrategy;
        private readonly string _idProperty;
        private readonly BaseDataSourceStrategy _listStrategy;
        // Modified

        private readonly Dictionary<object, bool> _modifiedItems
            = new Dictionary<object, bool>();

        private readonly View _view;

        public DataSource(View view, string idProperty, IDataProvider dataProvider)
        {
            _view = view;
            _idProperty = idProperty;
            _dataProvider = dataProvider;

            _dataItems = new ConcurrentCollection(idProperty);
            _editStrategy = new EditDataSourceStrategy(this);
            _listStrategy = new ListDataSourceStrategy(this);

            SetListMode();

            // Подписка на события для обновления привязок данных

            OnItemsUpdated += OnItemsUpdatedHandler;
            OnSelectedItemChanged += OnSelectedItemChangedHandler;
        }

        // View

        public View GetView()
        {
            return _view;
        }

        public string GetName()
        {
            return _name;
        }

        public void SetName(string value)
        {
            _name = value;
        }

        // IdProperty

        public string GetIdProperty()
        {
            return _idProperty;
        }

        public bool GetFillCreatedItem()
        {
            return _fillCreatedItem;
        }

        public void SetFillCreatedItem(bool value)
        {
            _fillCreatedItem = value;
        }

        // ConfigId

        public string GetConfigId()
        {
            return _dataProvider.GetConfigId();
        }

        public void SetConfigId(string value)
        {
            _dataProvider.SetConfigId(value);
        }

        // DocumentId

        public string GetDocumentId()
        {
            return _dataProvider.GetDocumentId();
        }

        public void SetDocumentId(string value)
        {
            _dataProvider.SetDocumentId(value);
        }

        public string GetVersion()
        {
            return _dataProvider.GetVersion();
        }

        public void SetVersion(string version)
        {
            _dataProvider.SetVersion(version);
        }

        public void SuspendUpdate()
        {
            _suspended = true;
        }

        public void ResumeUpdate()
        {
            if (_suspended)
            {
                _suspended = false;

                UpdateItems();
            }
        }

        public int GetPageNumber()
        {
            return _pageNumber;
        }

        public void SetPageNumber(int value)
        {
            if (value < 0)
            {
                value = 0;
            }

            if (_pageNumber != value)
            {
                _pageNumber = value;

                _strategy.OnPageNumberChanged(value);

                UpdateItems();
            }
        }

        public int GetPageSize()
        {
            return _pageSize;
        }

        public void SetPageSize(int value)
        {
            if (value < 0)
            {
                value = 0;
            }
            else if (value > 1000)
            {
                value = 1000;
            }

            if (_pageSize != value)
            {
                _pageNumber = 0;
                _pageSize = value;

                _strategy.OnPageNumberChanged(0);
                _strategy.OnPageSizeChanged(value);

                UpdateItems();
            }
        }

        public bool IsModified()
        {
            return (_modifiedItems.Count > 0);
        }

        public bool IsModified(object item)
        {
            return (item != null) && _modifiedItems.ContainsKey(item);
        }

        public void SetModified(object item)
        {
            if (item != null)
            {
                _modifiedItems[item] = true;
            }
        }

        public void ResetModified(object item)
        {
            if (item != null)
            {
                _modifiedItems.Remove(item);
            }
        }

        // Items

        public void SaveItem(object item)
        {
            InvokeAction(() => _dataProvider.ReplaceItem(item));

            _dataItems.AddOrUpdate(item);

            _strategy.OnItemSaved(item);

            ResetModified(item);
        }

        public void DeleteItem(string itemId)
        {
            InvokeAction(() => _dataProvider.DeleteItem(itemId));

            _dataItems.RemoveById(itemId);

            _strategy.OnItemDeleted(itemId);
        }

        public IEnumerable GetItems()
        {
            return _dataItems;
        }

        public void UpdateItems()
        {
            if (_suspended == false)
            {
                var items = LoadItems();

                _dataItems.ReplaceAll(items);

                _strategy.OnItemsUpdated(items);
            }
        }

        public object GetSelectedItem()
        {
            return _selectedItem;
        }

        public void SetSelectedItem(object value)
        {
            if (Equals(_selectedItem, value) == false)
            {
                _selectedItem = value;

                _strategy.OnSelectedItemChanged(value);
            }
        }

        public void SetEditMode()
        {
            _strategy = _editStrategy;
        }

        public void SetListMode()
        {
            _strategy = _listStrategy;
        }

        public string GetIdFilter()
        {
            return _idFilter;
        }

        public void SetIdFilter(string value)
        {
            if (Equals(_idFilter, value) == false)
            {
                _idFilter = value;

                UpdateItems();
            }
        }

        public IEnumerable GetPropertyFilters()
        {
            return _propertyFilters;
        }

        public void SetPropertyFilters(IEnumerable value)
        {
            if (Equals(_propertyFilters, value) == false)
            {
                _pageNumber = 0;
                _propertyFilters = value;

                _strategy.OnPageNumberChanged(0);
                _strategy.OnPropertyFiltersChanged(value);

                UpdateItems();
            }
        }

        public string GetTextFiliter()
        {
            return _textFilter;
        }

        public void SetTextFilter(string value)
        {
            if (Equals(_textFilter, value) == false)
            {
                _pageNumber = 0;
                _textFilter = value;

                _strategy.OnPageNumberChanged(0);
                _strategy.OnTextFilterChanged(value);

                UpdateItems();
            }
        }

        public IEnumerable<ISourceDataBinding> GetDataBindings()
        {
            return _dataBindings.AsReadOnly();
        }

        public void AddDataBinding(ISourceDataBinding dataBinding)
        {
            _dataBindings.Add(dataBinding);

            dataBinding.OnSetPropertyValue += OnSetPropertyValueHandler;
        }

        public void RemoveDataBinding(ISourceDataBinding dataBinding)
        {
            if (_dataBindings.Remove(dataBinding))
            {
                dataBinding.OnSetPropertyValue -= OnSetPropertyValueHandler;
            }
        }

        // Events

        public ScriptDelegate OnPageNumberChanged { get; set; }
        public ScriptDelegate OnPageSizeChanged { get; set; }
        public ScriptDelegate OnSelectedItemChanged { get; set; }
        public ScriptDelegate OnPropertyFiltersChanged { get; set; }
        public ScriptDelegate OnTextFilterChanged { get; set; }
        public ScriptDelegate OnItemSaved { get; set; }
        public ScriptDelegate OnItemDeleted { get; set; }
        public ScriptDelegate OnItemsUpdated { get; set; }
        public ScriptDelegate OnError { get; set; }

        private IEnumerable<object> LoadItems()
        {
            var items = InvokeAction(() => _strategy.GetItems(_dataProvider));

            return (items != null) ? items.Cast<object>().ToList() : null;
        }

        private void InvokeAction(Action action)
        {
            try
            {
                action();
            }
            catch (Exception error)
            {
                _strategy.OnError(error.Message);

                throw;
            }
        }

        private TResult InvokeAction<TResult>(Func<TResult> action)
        {
            try
            {
                return action();
            }
            catch (Exception error)
            {
                _strategy.OnError(error.Message);

                throw;
            }
        }

        private void OnSetPropertyValueHandler(dynamic context, dynamic arguments)
        {
            var source = arguments.Source;
            var force = arguments.Force;
            var propertyName = arguments.Property;
            var propertyValue = arguments.Value;

            if (string.IsNullOrEmpty(propertyName) == false)
            {
                // Изменение свойства выделенного элемента

                var selectedItem = GetSelectedItem();

                if (selectedItem != null)
                {
                    if (propertyName == "{this}")
                    {
                        if (selectedItem != propertyValue)
                        {
                            ObjectHelper.ReplaceProperties(selectedItem, propertyValue);
                        }
                    }
                    else
                    {
                        ObjectHelper.SetProperty(selectedItem, propertyName, propertyValue);
                    }

                    SetModified(selectedItem);
                }

                // Оповещение всех привязок, которые связаны со свойством

                foreach (var dataBinding in GetDataBindings())
                {
                    if (dataBinding != source && dataBinding.GetProperty() == propertyName)
                    {
                        dataBinding.PropertyValueChanged(propertyValue, force == true);
                    }
                }
            }
        }

        private void OnItemsUpdatedHandler(dynamic context, dynamic arguments)
        {
            var items = arguments.Value;

            // Оповещение всех привязок, которые связаны с самим источником данных

            foreach (var dataBinding in GetDataBindings())
            {
                if (string.IsNullOrEmpty(dataBinding.GetProperty()))
                {
                    dataBinding.PropertyValueChanged(items);
                }
            }
        }

        private void OnSelectedItemChangedHandler(dynamic context, dynamic arguments)
        {
            var selectedItem = arguments.Value;

            // Оповещение всех привязок, которые связаны со свойствами выделенного элемента

            foreach (var dataBinding in GetDataBindings())
            {
                var propertyName = dataBinding.GetProperty();

                if (string.IsNullOrEmpty(propertyName) == false)
                {
                    var propertyValue = (propertyName != "{this}")
                        ? ObjectHelper.GetProperty(selectedItem, propertyName)
                        : selectedItem;

                    dataBinding.PropertyValueChanged(propertyValue);
                }
            }
        }
    }
}