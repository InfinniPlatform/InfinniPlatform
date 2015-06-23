using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DesignControls.PropertyDesigner
{
    public partial class CollectionEditor : UserControl
    {
        private CollectionProperty _collectionProperty;
        private List<dynamic> _dataSource = new List<dynamic>();
        private EditorRepository _repository;
        private readonly Dictionary<dynamic, CollectionProperty> _rows = new Dictionary<dynamic, CollectionProperty>();

        public CollectionEditor()
        {
            _repository = new EditorRepository(GetItemProperty);

            InitializeComponent();
        }

        public CollectionProperty CollectionProperty
        {
            get
            {
                _collectionProperty.Items = ConvertDataSourceFromDataTable(_dataSource);
                return _collectionProperty;
            }
            set
            {
                _collectionProperty = value;
                if (_collectionProperty != null)
                {
                    _dataSource = ConvertDataSourceToDataTable(_collectionProperty.Items);
                }
                else
                {
                    _dataSource = new List<dynamic>();
                }
                RefreshDataSource();
            }
        }

        public IEnumerable<dynamic> DataSource
        {
            get { return _dataSource.ToList(); }
        }

        public string PropertyName { get; set; }

        protected dynamic GetItemProperty(string propertyName)
        {
            return PropertiesView
                .GetFocusedDataRow().Field<object>(propertyName);
        }

        private List<dynamic> ConvertDataSourceToDataTable(IEnumerable<dynamic> items)
        {
            //из-за того, что для Grid используется DataTable( грид не может показывать динамические объекты)
            //приходится выполнять обратное преобразование при возврате элементов коллекции
            var result = new List<dynamic>();
            foreach (var o in items)
            {
                dynamic instance = new DynamicWrapper();
                _rows.Add(instance, _collectionProperty.Clone(true));


                foreach (var property in _rows[instance].Properties)
                {
                    var objectProperty = property.Value as ObjectProperty;
                    if (objectProperty != null)
                    {
                        var propertyValue = ObjectHelper.GetProperty(o, property.Key) ?? new DynamicWrapper();
                        throw new ArgumentException("Необходим рефакторинг");
                        //objectProperty.SimpleProperties.SetSimplePropertiesFromInstance(propertyValue);
                        //objectProperty.CollectionProperties.SetCollectionPropertiesFromInstance(propertyValue);

                        instance[property.Key] = propertyValue;
                        continue;
                    }


                    var collectionProperty = property.Value as CollectionProperty;
                    if (collectionProperty != null)
                    {
                        var arr =
                            DynamicWrapperExtensions.ToDynamicList(ObjectHelper.GetProperty(o, property.Key).ToString());
                        collectionProperty.Items = arr.ToList();
                        instance[property.Key] = ObjectHelper.GetProperty(o, property.Key).ToString();
                    }
                    else
                    {
                        instance[property.Key] = ObjectHelper.GetProperty(o, property.Key);
                    }
                }
                result.Add(instance);
            }
            return result;
        }

        private List<dynamic> ConvertDataSourceFromDataTable(IEnumerable<dynamic> dataSource)
        {
            //из-за того, что для Grid используется DataTable( грид не может показывать динамические объекты)
            //приходится выполнять обратное преобразование при возврате элементов коллекции
            var result = new List<dynamic>();
            foreach (var o in dataSource)
            {
                dynamic instance = new DynamicWrapper();
                foreach (var property in _rows[o].Properties)
                {
                    var objectProperty = property.Value as ObjectProperty;
                    if (objectProperty != null)
                    {
                        string value = ObjectHelper.GetProperty(o, property.Key).ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            dynamic instanceAdd = value.ToDynamic();
                            instance[property.Key] = instanceAdd;
                        }
                        continue;
                    }

                    var collectionProperty = property.Value as CollectionProperty;
                    if (collectionProperty != null)
                    {
                        instance[property.Key] = ObjectHelper.GetProperty(o, property.Key).ToString();
                    }

                    else
                    {
                        instance[property.Key] = ObjectHelper.GetProperty(o, property.Key);
                    }
                }
                result.Add(instance);
            }
            return result;
        }

        private void RefreshDataSource()
        {
            CustomizeColumns(_collectionProperty);

            gridBinding.DataSource = _dataSource.ToDataTable();

            CollectionPropertiesGrid.RefreshDataSource();
        }

        private void CustomizeColumns(CollectionProperty collectionProperty)
        {
            PropertiesView.Columns.Clear();
            foreach (var property in collectionProperty.Properties)
            {
                var column = new GridColumn();
                column.FieldName = property.Key;
                var objectProperty = property.Value as ObjectProperty;
                if (objectProperty != null)
                {
                    column.ColumnEdit = DesignerExtensions.CreateRepositoryItem(repositoryItemButtonEdit_ButtonClick);
                }
                else
                {
                    var innerCollectionProperty = property.Value as CollectionProperty;
                    if (innerCollectionProperty != null)
                    {
                        column.ColumnEdit = DesignerExtensions.CreateRepositoryItem(repositoryItemButtonEdit_ButtonClick);
                    }
                    else
                    {
                        column.ColumnEdit = _repository.GetRepositoryItem(property.Key, property.Value) ??
                                            (!string.IsNullOrEmpty(PropertyName)
                                                ? _repository.GetRepositoryItem(PropertyName + "." + property.Key,
                                                    property.Value)
                                                : null);
                    }
                }

                column.Visible = true;
                PropertiesView.Columns.Add(column);
            }
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            dynamic inst = new DynamicWrapper();

            var newRowProperty = _collectionProperty.Clone(true);

            foreach (var property in newRowProperty.Properties)
            {
                var objectProperty = property.Value as ObjectProperty;
                if (objectProperty != null)
                {
                    inst[property.Key] = objectProperty.Value;
                    continue;
                }

                var collectionProperty = property.Value as CollectionProperty;
                if (collectionProperty != null)
                {
                    inst[property.Key] = collectionProperty.Items;
                }
                else
                {
                    inst[property.Key] = property.Value.Value;
                }
            }

            _dataSource.Add(inst);
            _rows.Add(inst, newRowProperty);
            RefreshDataSource();
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (PropertiesView.FocusedRowHandle >= 0)
            {
                var item = _dataSource.ToArray()[PropertiesView.FocusedRowHandle];
                _dataSource.Remove(item);
                _rows.Remove(item);
                RefreshDataSource();
            }
        }

        private void PropertiesView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            _dataSource.ToArray()[e.RowHandle].SetProperty(e.Column.FieldName, e.Value);
        }

        private void repositoryItemButtonEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Delete)
            {
                ((BaseEdit) sender).EditValue = new DynamicWrapper().ToString();
            }
            else if (e.Button.Kind == ButtonPredefines.Glyph)
            {
                var value = ((BaseEdit) sender).EditValue;
                var form = new ValueEdit();
                form.Value = value != null ? value.ToString() : string.Empty;
                form.ShowDialog();
            }
            else if (e.Button.Kind == ButtonPredefines.Ellipsis)
            {
                var instance = _dataSource.ToArray()[PropertiesView.FocusedRowHandle];
                CollectionProperty property = _rows[instance];

                //если разбираем свойство являющееся объектом
                var valueProperty = property.Properties[PropertiesView.FocusedColumn.FieldName] as ObjectProperty;


                dynamic propertyValue = null;
                try
                {
                    propertyValue =
                        DynamicWrapperExtensions.ToDynamic(
                            (string)
                                ObjectHelper.GetProperty(instance, PropertiesView.FocusedColumn.FieldName).ToString());
                    DesignerExtensions.SetSimplePropertiesFromInstance(valueProperty.SimpleProperties, propertyValue);
                    DesignerExtensions.SetCollectionPropertiesFromInstance(valueProperty.CollectionProperties,
                        propertyValue);
                }
                catch (Exception)
                {
                    propertyValue =
                        DynamicWrapperExtensions.ToDynamicList(
                            (string)
                                ObjectHelper.GetProperty(instance, PropertiesView.FocusedColumn.FieldName).ToString());
                }


                if (valueProperty != null)
                {
                    SelectObjectProperty(sender, valueProperty);
                }

                var collectionProperty =
                    property.Properties[PropertiesView.FocusedColumn.FieldName] as CollectionProperty;

                if (collectionProperty != null)
                {
                    SelectCollectionProperty(sender, collectionProperty);
                }
            }
        }

        private void SelectCollectionProperty(object sender, CollectionProperty collectionProperty)
        {
            var collectionForm = new CollectionForm();
            collectionForm.CollectionEditor.SetPropertyEditors(_repository);
            collectionForm.CollectionEditor.PropertyName = PropertiesView.FocusedColumn.FieldName;
            var cloneProperty = collectionProperty;
            collectionForm.CollectionEditor.CollectionProperty = cloneProperty;
            collectionForm.ShowDialog();

            var array = collectionForm.CollectionEditor.CollectionProperty.Items;
            ((ButtonEdit) sender).EditValue = array.ToString();
        }

        private void SelectObjectProperty(object sender, ObjectProperty valueProperty)
        {
            var propertiesForm = new PropertiesForm();
            propertiesForm.ParentProperty = PropertiesView.FocusedColumn.FieldName;
            propertiesForm.SetPropertyEditors(_repository.Editors);

            propertiesForm.SetSimpleProperties(valueProperty.SimpleProperties);
            propertiesForm.SetCollectionProperties(valueProperty.CollectionProperties);
            propertiesForm.SetValidationRules(valueProperty.ValidationRules);

            if (propertiesForm.ShowDialog() == DialogResult.OK)
            {
                //долбаный грид заворачивает объект в строку при создании DataRow
                dynamic instance = PropertiesView.GetFocusedValue().ToString().ToDynamic();
                DesignerExtensions.SetSimplePropertiesToInstance(valueProperty.SimpleProperties, instance);
                DesignerExtensions.SetCollectionPropertiesToInstance(valueProperty.CollectionProperties, instance);

                ((ButtonEdit) sender).EditValue = instance.ToString();
                ((ButtonEdit) sender).Refresh();
            }
            else
            {
                propertiesForm.RevertChanges();
            }
        }

        public void SetPropertyEditors(EditorRepository repository)
        {
            _repository = repository;
        }
    }

    public static class GridExtensions
    {
        public static DataTable ToDataTable(this IEnumerable<dynamic> items)
        {
            var tempData = new List<Dictionary<string, dynamic>>();

            foreach (var item in items)
            {
                tempData.Add(item);
            }

            var data = tempData.ToArray();
            if (!data.Any()) return null;

            var dt = new DataTable();

            var columns = data.SelectMany(item => item.Select(it => it.Key)).Distinct().ToList();

            foreach (var key in columns)
            {
                dt.Columns.Add(key);
            }

            foreach (var d in data)
            {
                var values = d.Select(p => p.Value).ToArray();
                dt.Rows.Add(values);
            }
            return dt;
        }
    }
}