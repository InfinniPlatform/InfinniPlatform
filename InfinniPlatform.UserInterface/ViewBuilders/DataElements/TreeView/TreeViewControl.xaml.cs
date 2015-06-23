using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Grid;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.UserInterface.ViewBuilders.Data;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.TreeView
{
    /// <summary>
    ///     Элемент управления для древовидного списка.
    /// </summary>
    sealed partial class TreeViewControl : UserControl
    {
        private string _displayProperty;
        private string _idProperty;
        private string _imageProperty;
        private string _keyProperty;
        private string _parentProperty;
        private string _valueProperty;

        private readonly ConcurrentCollection _dataRows
            = new ConcurrentCollection();

        public TreeViewControl()
        {
            InitializeComponent();

            TreeListView.NodeImageSelector = ImagePropertySelector.Instance;

            TreeList.ItemsSource = _dataRows;
        }

        /// <summary>
        ///     Возвращает или устанавливает выделенный элемент в списке.
        /// </summary>
        public object SelectedItem
        {
            get
            {
                var selectedRow = TreeList.GetRow(TreeListView.FocusedRowHandle) as TreeViewDataRow;
                return (selectedRow != null) ? selectedRow.Item : null;
            }
            set
            {
                var selectedRow = FindDataRowByItem(value);
                this.InvokeControl(() => TreeList.CurrentItem = selectedRow);
            }
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
        ///     Возвращает или устанавливает значение, определяющее, возможен ли выбор нескольких значений.
        /// </summary>
        public bool MultiSelect
        {
            get { return (bool) GetValue(MultiSelectProperty); }
            set { SetValue(MultiSelectProperty, value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает значение, определяющее, нужно ли отображать изображения элементов.
        /// </summary>
        public bool ShowNodeImages
        {
            get { return (bool) GetValue(ShowNodeImagesProperty); }
            set { SetValue(ShowNodeImagesProperty, value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает свойство элемента источника данных, которое хранит уникальный идентификатор элемента.
        /// </summary>
        public string IdProperty
        {
            get { return (string) GetValue(IdPropertyProperty); }
            set { SetValue(IdPropertyProperty, _idProperty = value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает свойство элемента источника данных, которое хранит идентификатор элемента.
        /// </summary>
        public string KeyProperty
        {
            get { return (string) GetValue(KeyPropertyProperty); }
            set { SetValue(KeyPropertyProperty, _keyProperty = value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает свойство элемента источника данных, которое хранит идентификатор родителя.
        /// </summary>
        public string ParentProperty
        {
            get { return (string) GetValue(ParentPropertyProperty); }
            set { SetValue(ParentPropertyProperty, _parentProperty = value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает свойство элемента источника данных, которое хранит значение элемента.
        /// </summary>
        public string ValueProperty
        {
            get { return (string) GetValue(ValuePropertyProperty); }
            set { SetValue(ValuePropertyProperty, _valueProperty = value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает свойство элемента источника данных, которое хранит наименование элемента.
        /// </summary>
        public string DisplayProperty
        {
            get { return (string) GetValue(DisplayPropertyProperty); }
            set { SetValue(DisplayPropertyProperty, _displayProperty = value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает свойство элемента источника данных, которое хранит изображение элемента.
        /// </summary>
        public string ImageProperty
        {
            get { return (string) GetValue(ImagePropertyProperty); }
            set { SetValue(ImagePropertyProperty, _imageProperty = value); }
        }

        /// <summary>
        ///     Событие выделения элемента в списке.
        /// </summary>
        public event RoutedEventHandler OnSetSelectedItem
        {
            add { TreeListView.AddHandler(DataViewBase.FocusedRowHandleChangedEvent, value); }
            remove { TreeListView.RemoveHandler(DataViewBase.FocusedRowHandleChangedEvent, value); }
        }

        private static void OnDataNavigationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TreeViewControl;

            if (control != null)
            {
                var navigationPanel = e.NewValue as FrameworkElement;
                control.TreeListDataNavigation.Child = navigationPanel;
                control.TreeListDataNavigation.Height = (navigationPanel != null) ? navigationPanel.Height : 0;
            }
        }

        private static void OnMultiSelectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TreeViewControl;

            if (control != null)
            {
                control.TreeListView.ShowCheckboxes = (bool) e.NewValue;
            }
        }

        private static void OnShowNodeImagesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TreeViewControl;

            if (control != null)
            {
                control.TreeListView.ShowNodeImages = (bool) e.NewValue;
            }
        }

        /// <summary>
        ///     Событие выделения элемента в списке.
        /// </summary>
        public event RoutedEventHandler OnDoubleClick
        {
            add { AddHandler(OnDoubleClickEvent, value); }
            remove { RemoveHandler(OnDoubleClickEvent, value); }
        }

        private void OnDoubleClickHandler(object sender, RowDoubleClickEventArgs e)
        {
            if (e.HitInfo.InRow)
            {
                RaiseEvent(new RoutedEventArgs(OnDoubleClickEvent));
            }
        }

        /// <summary>
        ///     Добавляет элемент.
        /// </summary>
        public void AddItem(object item)
        {
            var dataRow = CreateDataRow(item);

            _dataRows.AddOrUpdate(dataRow);
        }

        /// <summary>
        ///     Удаляет элемент.
        /// </summary>
        public void RemoveItem(object itemId)
        {
            var dataRow = FindDataRowById(itemId);

            if (dataRow != null)
            {
                _dataRows.Remove(dataRow);
            }
        }

        /// <summary>
        ///     Удаляет элемент и все вложенные.
        /// </summary>
        public void RemoveItem(object itemId, bool removeChildren)
        {
            var dataRow = FindDataRowById(itemId);

            if (dataRow != null)
            {
                var children = new List<TreeViewDataRow>();
                FindAllChildren(dataRow, children);

                _dataRows.RemoveByPredicate(children.Contains);
            }
        }

        private void FindAllChildren(TreeViewDataRow parent, ICollection<TreeViewDataRow> children)
        {
            children.Add(parent);

            foreach (TreeViewDataRow dataRow in _dataRows)
            {
                if (dataRow.Parent == parent.Key)
                {
                    FindAllChildren(dataRow, children);
                }
            }
        }

        /// <summary>
        ///     Возвращает список элементов.
        /// </summary>
        public IEnumerable GetItems()
        {
            return _dataRows.Cast<TreeViewDataRow>().Select(i => i.Item);
        }

        /// <summary>
        ///     Устанавливает список элементов.
        /// </summary>
        public void SetItems(IEnumerable items)
        {
            var newDataRows = (items != null) ? items.Cast<object>().Select(CreateDataRow).ToArray() : null;

            _dataRows.ReplaceAll(newDataRows);
        }

        /// <summary>
        ///     Обновляет указанный элемент.
        /// </summary>
        public void RefreshItem(object item)
        {
            var dataRow = FindDataRowByItem(item);

            if (dataRow != null)
            {
                RefreshDataRow(dataRow);

                TreeList.InvokeControl(() =>
                {
                    var dataRowNode = TreeListView.GetNodeByContent(dataRow);

                    if (dataRowNode != null)
                    {
                        TreeList.RefreshRow(dataRowNode.RowHandle);
                    }
                });
            }
        }

        /// <summary>
        ///     Обновляет список элементов.
        /// </summary>
        public void RefreshItems()
        {
            foreach (TreeViewDataRow dataRow in _dataRows)
            {
                RefreshDataRow(dataRow);
            }

            TreeList.InvokeControl(() => TreeList.RefreshData());
        }

        /// <summary>
        ///     Перемещает указанный элемент.
        /// </summary>
        public void MoveItem(object item, int delta)
        {
            var dataRow = FindDataRowByItem(item);

            if (dataRow != null)
            {
                var dataRowNode = TreeListView.GetNodeByContent(dataRow);

                if (dataRowNode != null)
                {
                    var dataRowNodes = (dataRowNode.ParentNode != null)
                        ? dataRowNode.ParentNode.Nodes
                        : TreeListView.Nodes;
                    var dataRowNodeOldIndex = dataRowNodes.IndexOf(dataRowNode);
                    var dataRowNodeNewIndex = dataRowNodeOldIndex + delta;

                    if (dataRowNodeNewIndex >= 0 && dataRowNodeNewIndex < dataRowNodes.Count)
                    {
                        dataRowNodes.RemoveAt(dataRowNodeOldIndex);
                        dataRowNodes.Insert(dataRowNodeNewIndex, dataRowNode);

                        TreeListView.FocusedNode = dataRowNode;
                    }
                }
            }
        }

        /// <summary>
        ///     Разворачивает указанный элемент.
        /// </summary>
        public void ExpandItem(object item)
        {
            var dataRow = FindDataRowByItem(item);

            if (dataRow != null)
            {
                RefreshDataRow(dataRow);

                TreeList.InvokeControl(() =>
                {
                    var dataRowNode = TreeListView.GetNodeByContent(dataRow);

                    if (dataRowNode != null)
                    {
                        TreeListView.ExpandNode(dataRowNode.RowHandle);
                    }
                });
            }
        }

        /// <summary>
        ///     Сворачивает указанный элемент.
        /// </summary>
        public void CollapseItem(object item)
        {
            var dataRow = FindDataRowByItem(item);

            if (dataRow != null)
            {
                RefreshDataRow(dataRow);

                TreeList.InvokeControl(() =>
                {
                    var dataRowNode = TreeListView.GetNodeByContent(dataRow);

                    if (dataRowNode != null)
                    {
                        TreeListView.CollapseNode(dataRowNode.RowHandle);
                    }
                });
            }
        }

        private TreeViewDataRow CreateDataRow(object item)
        {
            var dataRow = new TreeViewDataRow();
            FillDataRow(dataRow, item);
            return dataRow;
        }

        private void RefreshDataRow(TreeViewDataRow dataRow)
        {
            FillDataRow(dataRow, dataRow.Item);
        }

        private void FillDataRow(TreeViewDataRow dataRow, object item)
        {
            dataRow.Id = item.GetProperty(_idProperty);
            dataRow.Key = item.GetProperty(_keyProperty);
            dataRow.Parent = item.GetProperty(_parentProperty);
            dataRow.Image = item.GetProperty(_imageProperty);
            dataRow.Value = item.GetProperty(_valueProperty);
            dataRow.Display = item.GetProperty(_displayProperty);
            dataRow.Item = item;

            this.InvokeControl(() => InvokeRenderItem(dataRow, item));
        }

        private TreeViewDataRow FindDataRowById(object itemId)
        {
            return _dataRows.Cast<TreeViewDataRow>().FirstOrDefault(r => Equals(r.Id, itemId));
        }

        private TreeViewDataRow FindDataRowByItem(object item)
        {
            var id = item.GetProperty(_idProperty);

            return _dataRows.Cast<TreeViewDataRow>().FirstOrDefault(r => Equals(r.Id, id));
        }

        /// <summary>
        ///     Событие отрисовки элемента.
        /// </summary>
        public event RenderItemRoutedEventHandler OnRenderItem
        {
            add { AddHandler(OnRenderItemEvent, value); }
            remove { RemoveHandler(OnRenderItemEvent, value); }
        }

        private void InvokeRenderItem(TreeViewDataRow dataRow, object item)
        {
            var value = item.GetProperty(_valueProperty);
            var display = item.GetProperty(_displayProperty);

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

        public static readonly DependencyProperty DataNavigationProperty = DependencyProperty.Register(
            "DataNavigation", typeof (UIElement), typeof (TreeViewControl),
            new FrameworkPropertyMetadata(null, OnDataNavigationChanged));

        public static readonly DependencyProperty MultiSelectProperty = DependencyProperty.Register("MultiSelect",
            typeof (bool), typeof (TreeViewControl), new FrameworkPropertyMetadata(false, OnMultiSelectChanged));

        public static readonly DependencyProperty ShowNodeImagesProperty = DependencyProperty.Register(
            "ShowNodeImages", typeof (bool), typeof (TreeViewControl),
            new FrameworkPropertyMetadata(false, OnShowNodeImagesChanged));

        public static readonly DependencyProperty IdPropertyProperty = DependencyProperty.Register("IdProperty",
            typeof (string), typeof (TreeViewControl));

        public static readonly DependencyProperty KeyPropertyProperty = DependencyProperty.Register("KeyProperty",
            typeof (string), typeof (TreeViewControl));

        public static readonly DependencyProperty ParentPropertyProperty = DependencyProperty.Register(
            "ParentProperty", typeof (string), typeof (TreeViewControl));

        public static readonly DependencyProperty ValuePropertyProperty = DependencyProperty.Register("ValueProperty",
            typeof (string), typeof (TreeViewControl));

        public static readonly DependencyProperty DisplayPropertyProperty =
            DependencyProperty.Register("DisplayProperty", typeof (string), typeof (TreeViewControl));

        public static readonly DependencyProperty ImagePropertyProperty = DependencyProperty.Register("ImageProperty",
            typeof (string), typeof (TreeViewControl));

        // OnDoubleClick

        public static readonly RoutedEvent OnDoubleClickEvent = EventManager.RegisterRoutedEvent("OnDoubleClick",
            RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (TreeViewControl));

        // OnRenderItem

        public static readonly RoutedEvent OnRenderItemEvent = EventManager.RegisterRoutedEvent("OnRenderItem",
            RoutingStrategy.Bubble, typeof (RenderItemRoutedEventHandler), typeof (TreeViewControl));
    }
}