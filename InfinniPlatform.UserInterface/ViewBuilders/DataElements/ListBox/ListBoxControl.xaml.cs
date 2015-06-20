using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InfinniPlatform.Sdk.Application.Dynamic;
using InfinniPlatform.UserInterface.ViewBuilders.Data;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.ListBox
{
    /// <summary>
    ///     Элемент управления для списка.
    /// </summary>
    sealed partial class ListBoxControl : UserControl
    {
        // State Helpers

        private int _updateDataRows;
        private int _updateEditValue;
        // Items

        private readonly ConcurrentCollection _dataRows = new ConcurrentCollection();

        public ListBoxControl()
        {
            InitializeComponent();

            ListBoxElement.ItemsSource = _dataRows;
        }

        /// <summary>
        ///     Возвращает или устанавливает значение, определяющее, разрешен ли выбор нескольких элементов.
        /// </summary>
        public bool MultiSelect
        {
            get { return (bool) GetValue(MultiSelectProperty); }
            set { SetValue(MultiSelectProperty, value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает значение, определяющее, разрешен ли редактирование значения.
        /// </summary>
        public bool ReadOnly
        {
            get { return (bool) GetValue(ReadOnlyProperty); }
            set { SetValue(ReadOnlyProperty, value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает свойство элемента источника данных, которое хранит значение для выбора.
        /// </summary>
        public string ValueProperty
        {
            get { return (string) GetValue(ValuePropertyProperty); }
            set { SetValue(ValuePropertyProperty, value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает свойство элемента источника данных, которое хранит значение для отображения.
        /// </summary>
        public string DisplayProperty
        {
            get { return (string) GetValue(DisplayPropertyProperty); }
            set { SetValue(DisplayPropertyProperty, value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает навигационную панель.
        /// </summary>
        public UIElement DataNavigation
        {
            get { return (UIElement) GetValue(DataNavigationProperty); }
            set { SetValue(DataNavigationProperty, value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает редактируемое значение.
        /// </summary>
        public object EditValue
        {
            get { return GetValue(EditValueProperty); }
            set { SetValue(EditValueProperty, value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает коллекцию элементов.
        /// </summary>
        public IEnumerable Items
        {
            get { return (IEnumerable) GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // SelectedItem

        /// <summary>
        ///     Возвращает или устанавливает выделенный элемент в списке.
        /// </summary>
        public object SelectedItem
        {
            get
            {
                var dataRow = (ListBoxDataRow) ListBoxElement.SelectedItem;
                return (dataRow != null) ? dataRow.Item : null;
            }
            set
            {
                var dataRow = GetExistsDataRow(value);
                SelectDataRow(dataRow);
            }
        }

        // Event Handlers

        /// <summary>
        ///     Обработчик события пометки элемента в списке.
        /// </summary>
        private void OnCheckedChangedHandler(object sender, RoutedEventArgs e)
        {
            UpdateDataRows(() =>
            {
                if (MultiSelect)
                {
                    var checkBox = sender as FrameworkElement;

                    if (checkBox != null)
                    {
                        var dataRow = (ListBoxDataRow) checkBox.Tag;

                        var oldEditValue = EditValue;
                        var newEditValue = new List<object>();
                        var oldEditValueCollection = ObjectAsCollection(oldEditValue);

                        if (oldEditValueCollection != null)
                        {
                            newEditValue.AddRange(oldEditValueCollection);
                        }

                        if (dataRow.Checked)
                        {
                            newEditValue.Add(dataRow.Value);
                        }
                        else
                        {
                            newEditValue.Remove(dataRow.Value);
                        }

                        EditValue = newEditValue;

                        UpdateCheckDataRows();
                    }
                }
            });
        }

        /// <summary>
        ///     Обработчик события выделения элемента в списке.
        /// </summary>
        private void OnSelectionChangedHandler(object sender, SelectionChangedEventArgs e)
        {
            UpdateEditValue(() =>
            {
                if (!MultiSelect)
                {
                    var dataRow = (ListBoxDataRow) ListBoxElement.SelectedItem;
                    var newEditValue = (dataRow != null) ? dataRow.Value : null;

                    EditValue = newEditValue;

                    UpdateCheckDataRows();
                }

                InvokeSetSelectedItem();
            });
        }

        private static void OnMultiSelectChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ListBoxControl;

            if (control != null)
            {
                var multiSelect = (bool) e.NewValue;
                var oldEditValue = control.EditValue;

                // При переключении между режимами

                if (multiSelect)
                {
                    if (oldEditValue != null)
                    {
                        control.EditValue = new List<object> {oldEditValue};
                    }
                }
                else
                {
                    var oldEditValueCollection = ObjectAsCollection(oldEditValue);

                    control.EditValue = (oldEditValueCollection != null)
                        ? oldEditValueCollection.FirstOrDefault()
                        : null;
                }
            }
        }

        private static void OnDataNavigationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ListBoxControl;

            if (control != null)
            {
                var navigationPanel = e.NewValue as FrameworkElement;
                control.ListBoxDataNavigation.Child = navigationPanel;
                control.ListBoxDataNavigation.Height = (navigationPanel != null) ? navigationPanel.Height : 0;
            }
        }

        private static object CoerceEditValueCallback(DependencyObject d, object newValue)
        {
            var control = d as ListBoxControl;

            if (control != null)
            {
                if (control.ReadOnly)
                {
                    newValue = control.EditValue;
                }
                else
                {
                    var oldValue = control.EditValue;

                    if (!Equals(oldValue, newValue))
                    {
                        newValue = control.InvokeEditValueChanging(oldValue, newValue);
                    }
                }
            }

            return newValue;
        }

        private static void OnEditValueChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ListBoxControl;

            if (control != null && !Equals(e.OldValue, e.NewValue))
            {
                control.UpdateCheckDataRows();
                control.InvokeEditValueChanged(e.OldValue, e.NewValue);
            }
        }

        private static void OnItemsChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ListBoxControl;

            if (control != null)
            {
                var oldItems = e.OldValue as INotifyCollectionChanged;
                var newItems = e.NewValue as INotifyCollectionChanged;

                if (oldItems != null)
                {
                    oldItems.CollectionChanged -= control.OnItemsChangedHandler;
                }

                control.ReplaceDataRows((IEnumerable) e.NewValue);
                control.UpdateCheckDataRows();

                if (newItems != null)
                {
                    newItems.CollectionChanged += control.OnItemsChangedHandler;
                }
            }
        }

        private void OnItemsChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    _dataRows.InsertOrUpdate(CreateDataRows(e.NewItems), e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Move:
                    _dataRows.InsertOrUpdate(GetExistsDataRows(e.NewItems), e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    _dataRows.Replace(GetExistsDataRows(e.OldItems), CreateDataRows(e.NewItems));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    _dataRows.Remove(GetExistsDataRows(e.OldItems));
                    break;
                default:
                    ReplaceDataRows((IEnumerable) sender);
                    break;
            }

            UpdateCheckDataRows();
        }

        /// <summary>
        ///     Событие начала изменения значения.
        /// </summary>
        public event ValueChangingRoutedEventHandler OnEditValueChanging
        {
            add { AddHandler(OnEditValueChangingEvent, value); }
            remove { RemoveHandler(OnEditValueChangingEvent, value); }
        }

        private object InvokeEditValueChanging(object oldValue, object newValue)
        {
            var valueChangingArgs = new ValueChangingRoutedEventArgs(OnEditValueChangingEvent)
            {
                IsCancel = false,
                OldValue = oldValue,
                NewValue = newValue
            };

            RaiseEvent(valueChangingArgs);

            return valueChangingArgs.IsCancel ? oldValue : valueChangingArgs.NewValue;
        }

        /// <summary>
        ///     Событие окончания изменения значения.
        /// </summary>
        public event ValueChangedRoutedEventHandler OnEditValueChanged
        {
            add { AddHandler(OnEditValueChangedEvent, value); }
            remove { RemoveHandler(OnEditValueChangedEvent, value); }
        }

        private void InvokeEditValueChanged(object oldValue, object newValue)
        {
            var valueChangedArgs = new ValueChangedRoutedEventArgs(OnEditValueChangedEvent)
            {
                OldValue = oldValue,
                NewValue = newValue
            };

            RaiseEvent(valueChangedArgs);
        }

        /// <summary>
        ///     Событие выделения элемента в списке.
        /// </summary>
        public event RoutedEventHandler OnSetSelectedItem
        {
            add { AddHandler(OnSetSelectedItemEvent, value); }
            remove { RemoveHandler(OnSetSelectedItemEvent, value); }
        }

        private void InvokeSetSelectedItem()
        {
            RaiseEvent(new RoutedEventArgs(OnSetSelectedItemEvent));
        }

        /// <summary>
        ///     Событие отрисовки элемента.
        /// </summary>
        public event RenderItemRoutedEventHandler OnRenderItem
        {
            add { AddHandler(OnRenderItemEvent, value); }
            remove { RemoveHandler(OnRenderItemEvent, value); }
        }

        private void InvokeRenderItem(ListBoxDataRow dataRow, object item)
        {
            var value = item.GetProperty(ValueProperty);
            var display = item.GetProperty(DisplayProperty);

            var itemUpdateArgs = new RenderItemRoutedEventArgs(OnRenderItemEvent)
            {
                Item = item,
                Value = value,
                Display = display
            };

            RaiseEvent(itemUpdateArgs);

            dataRow.Item = item;
            dataRow.Value = itemUpdateArgs.Value;
            dataRow.Display = itemUpdateArgs.Display;
        }

        // Helpers

        private static List<object> ObjectAsCollection(object value)
        {
            var editValue = value as IEnumerable;

            return (editValue != null) ? editValue.Cast<object>().ToList() : null;
        }

        private void ReplaceDataRows(IEnumerable items)
        {
            _dataRows.ReplaceAll(CreateDataRows(items));
        }

        private IEnumerable GetExistsDataRows(IEnumerable items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    var dataRow = GetExistsDataRow(item);

                    if (dataRow != null)
                    {
                        yield return dataRow;
                    }
                }
            }
        }

        private object GetExistsDataRow(object item)
        {
            return _dataRows.Cast<ListBoxDataRow>().FirstOrDefault(r => Equals(r.Item, item));
        }

        private IEnumerable CreateDataRows(IEnumerable items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    yield return CreateDataRow(item);
                }
            }
        }

        private object CreateDataRow(object item)
        {
            var dataRow = new ListBoxDataRow();
            InvokeRenderItem(dataRow, item);
            return dataRow;
        }

        private void SelectDataRow(object dataRow)
        {
            ListBoxElement.SelectedItem = dataRow;

            if (dataRow != null)
            {
                ListBoxElement.ScrollIntoView(dataRow);
            }
        }

        private void UpdateCheckDataRows()
        {
            UpdateDataRows(() =>
            {
                var editValue = EditValue;

                if (MultiSelect)
                {
                    var checkedValues = ObjectAsCollection(editValue);

                    foreach (ListBoxDataRow dataRow in _dataRows)
                    {
                        var dataRowChecked = (checkedValues != null) && checkedValues.Contains(dataRow.Value);

                        dataRow.Checked = dataRowChecked;
                    }
                }
                else
                {
                    ListBoxDataRow selectedDataRow = null;

                    foreach (ListBoxDataRow dataRow in _dataRows)
                    {
                        var dataRowChecked = Equals(dataRow.Value, editValue);

                        if (dataRowChecked)
                        {
                            selectedDataRow = dataRow;
                        }

                        dataRow.Checked = dataRowChecked;
                    }

                    SelectDataRow(selectedDataRow);
                }
            }, true);
        }

        private void UpdateDataRows(Action action, bool reenter = false)
        {
            if (_updateDataRows == 0 || reenter)
            {
                ++_updateDataRows;

                try
                {
                    action();
                }
                finally
                {
                    --_updateDataRows;
                }
            }
        }

        private void UpdateEditValue(Action action, bool reenter = false)
        {
            if (_updateEditValue == 0 || reenter)
            {
                ++_updateEditValue;

                try
                {
                    action();
                }
                finally
                {
                    --_updateEditValue;
                }
            }
        }

        // MultiSelect

        public static readonly DependencyProperty MultiSelectProperty = DependencyProperty.Register("MultiSelect",
            typeof (bool), typeof (ListBoxControl), new FrameworkPropertyMetadata(false, OnMultiSelectChangedHandler));

        // ReadOnly

        public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register("ReadOnly",
            typeof (bool), typeof (ListBoxControl), new FrameworkPropertyMetadata(false));

        // ValueProperty

        public static readonly DependencyProperty ValuePropertyProperty = DependencyProperty.Register("ValueProperty",
            typeof (string), typeof (ListBoxControl));

        // DisplayProperty

        public static readonly DependencyProperty DisplayPropertyProperty =
            DependencyProperty.Register("DisplayProperty", typeof (string), typeof (ListBoxControl));

        // DataNavigation

        public static readonly DependencyProperty DataNavigationProperty = DependencyProperty.Register(
            "DataNavigation", typeof (UIElement), typeof (ListBoxControl),
            new FrameworkPropertyMetadata(null, OnDataNavigationChanged));

        // EditValue

        public static readonly DependencyProperty EditValueProperty = DependencyProperty.Register("EditValue",
            typeof (object), typeof (ListBoxControl),
            new FrameworkPropertyMetadata(null, OnEditValueChangedHandler, CoerceEditValueCallback));

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items",
            typeof (IEnumerable), typeof (ListBoxControl), new FrameworkPropertyMetadata(null, OnItemsChangedHandler));

        // OnEditValueChanging

        public static readonly RoutedEvent OnEditValueChangingEvent =
            EventManager.RegisterRoutedEvent("OnEditValueChanging", RoutingStrategy.Bubble,
                typeof (ValueChangingRoutedEventHandler), typeof (ListBoxControl));

        // OnEditValueChanged

        public static readonly RoutedEvent OnEditValueChangedEvent =
            EventManager.RegisterRoutedEvent("OnEditValueChanged", RoutingStrategy.Bubble,
                typeof (ValueChangedRoutedEventHandler), typeof (ListBoxControl));

        // OnSetSelectedItem

        public static readonly RoutedEvent OnSetSelectedItemEvent = EventManager.RegisterRoutedEvent(
            "OnSetSelectedItem", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (ListBoxControl));

        // OnRenderItem

        public static readonly RoutedEvent OnRenderItemEvent = EventManager.RegisterRoutedEvent("OnRenderItem",
            RoutingStrategy.Bubble, typeof (RenderItemRoutedEventHandler), typeof (ListBoxControl));
    }
}