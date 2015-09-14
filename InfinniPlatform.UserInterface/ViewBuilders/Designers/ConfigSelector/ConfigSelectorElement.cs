using System;
using System.Collections;
using System.Windows;
using InfinniPlatform.UserInterface.ViewBuilders.DataElements;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigSelector
{
    public sealed class ConfigSelectorElement : BaseElement<ConfigSelectorControl>
    {
        // ItemsFunc

        private Func<IEnumerable> _configurationsFunc;
        // Value

        private object _value;

        public ConfigSelectorElement(View view)
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
        ///     Возвращает функцию загрузки списка конфигураций.
        /// </summary>
        public Func<IEnumerable> GetConfigurationsFunc()
        {
            return _configurationsFunc;
        }

        /// <summary>
        ///     Устанавливает функцию загрузки списка конфигураций.
        /// </summary>
        public void SetConfigurationsFunc(Func<IEnumerable> value)
        {
            _configurationsFunc = value;
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
            const string cacheKey = "Configurations";

            var items = ControlCache.Get<IEnumerable>(cacheKey);

            if (refresh || items == null)
            {
                var itemsFunc = GetConfigurationsFunc();

                if (itemsFunc != null)
                {
                    try
                    {
                        items = itemsFunc();

                        ControlCache.Set(cacheKey, items);
                    }
                    catch
                    {
                    }
                }
            }

            Control.ItemsSource = items;
        }
    }
}