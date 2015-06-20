using System;
using System.Collections;
using System.Windows;
using InfinniPlatform.UserInterface.ViewBuilders.DataElements;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.DocumentSelector
{
    public sealed class DocumentSelectorElement : BaseElement<DocumentSelectorControl>
    {
        // ConfigId

        private string _configId;
        // ItemsFunc

        private Func<string, string, IEnumerable> _documentsFunc;
        // Value

        private object _value;
        private string _version;

        public DocumentSelectorElement(View view)
            : base(view)
        {
            Control.Loaded += OnLoadedHandler;
            Control.RefreshClick += OnRefreshHandler;
            Control.EditValueChanged += OnEditValueChangedHandler;
        }

        // Events

        /// <summary>
        ///     Возвращает или устанавливает обработчик события изменения значения.
        /// </summary>
        public ScriptDelegate OnValueChanged { get; set; }

        /// <summary>
        ///     Возвращает функцию загрузки списка документов.
        /// </summary>
        public Func<string, string, IEnumerable> GetDocumentsFunc()
        {
            return _documentsFunc;
        }

        /// <summary>
        ///     Устанавливает функцию загрузки списка документов.
        /// </summary>
        public void SetDocumentsFunc(Func<string, string, IEnumerable> value)
        {
            _documentsFunc = value;
        }

        /// <summary>
        ///     Возвращает идентификатор конфигурации.
        /// </summary>
        public string GetConfigId()
        {
            return _configId;
        }

        /// <summary>
        ///     Возвращает версию конфигурации
        /// </summary>
        public string GetVersion()
        {
            return _version;
        }

        /// <summary>
        ///     Устанавливает идентификатор конфигурации.
        /// </summary>
        public void SetConfigId(string value)
        {
            if (!Equals(_configId, value))
            {
                _configId = value;

                LoadItems(false);
            }
        }

        public void SetVersion(string value)
        {
            if (!Equals(_version, value))
            {
                _version = value;

                LoadItems(false);
            }
        }

        /// <summary>
        ///     Возвращает значение.
        /// </summary>
        public object GetValue()
        {
            return _value;
        }

        /// <summary>
        ///     Устанавливает значение.
        /// </summary>
        public void SetValue(object value)
        {
            _value = value;

            Control.InvokeControl(() => Control.EditValue = value);
        }

        private void OnEditValueChangedHandler(object sender, ValueChangedRoutedEventArgs e)
        {
            _value = e.NewValue;

            this.InvokeScript(OnValueChanged, args => args.Value = e.NewValue);
        }

        // Methods

        public void Refresh()
        {
            LoadItems(true);
        }

        private void OnRefreshHandler(object sender, RoutedEventArgs e)
        {
            LoadItems(true);
        }

        private void OnLoadedHandler(object sender, RoutedEventArgs e)
        {
            LoadItems(false);
        }

        private void LoadItems(bool refresh)
        {
            IEnumerable items = null;

            var configId = GetConfigId();

            var version = GetVersion();

            if (!string.IsNullOrWhiteSpace(configId))
            {
                var cacheKey = "Documents_" + configId + version;

                items = ControlCache.Get<IEnumerable>(cacheKey);

                if (refresh || items == null)
                {
                    var itemsFunc = GetDocumentsFunc();

                    if (itemsFunc != null)
                    {
                        try
                        {
                            items = itemsFunc(configId, version);

                            ControlCache.Set(cacheKey, items);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            else
            {
                Control.EditValue = null;
            }

            Control.ItemsSource = items;
        }
    }
}