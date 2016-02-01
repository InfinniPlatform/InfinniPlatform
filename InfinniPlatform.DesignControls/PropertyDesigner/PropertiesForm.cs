using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraVerticalGrid.Events;
using DevExpress.XtraVerticalGrid.Rows;
using DevExpress.XtraVerticalGrid.ViewInfo;

using InfinniPlatform.Core.Validation;
using InfinniPlatform.DesignControls.PropertyEditors;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DesignControls.PropertyDesigner
{
    public partial class PropertiesForm : Form
    {
        private Dictionary<string, CollectionProperty> _collectionProperties;
        private Dictionary<string, IControlProperty> _simpleProperties;
        private Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> _validationRules;
        private readonly List<CollectionEditor> _collectionEditors = new List<CollectionEditor>();
        private readonly Dictionary<string, BaseRow> _hideProperties = new Dictionary<string, BaseRow>();
        private readonly EditorRepository _repository;
        private readonly Dictionary<string, dynamic> _snapshotCollectionProperties = new Dictionary<string, dynamic>();
        private readonly Dictionary<string, dynamic> _snapshotSimpleProperties = new Dictionary<string, dynamic>();

        public PropertiesForm()
        {
            _repository = new EditorRepository(GetItemProperty);

            InitializeComponent();
        }

        public string ParentProperty { get; set; }

        public Dictionary<string, BaseRow> HideProperties
        {
            get { return _hideProperties; }
        }

        protected dynamic GetItemProperty(string propertyName)
        {
            return SimplePropertiesGrid.Rows.Cast<BaseRow>().ToList()
                .Where(r => r.Name == propertyName)
                .Select(r => (r as EditorRow).Properties.Value)
                .FirstOrDefault();
        }

        public void SetSimpleProperties(Dictionary<string, IControlProperty> properties)
        {
            _simpleProperties = properties;
            _snapshotSimpleProperties.Clear();

            SimplePropertiesGrid.Rows.Clear();
            SimplePropertiesGrid.RowHeaderWidth = 300;
            SimplePropertiesGrid.RecordWidth = 350;
            foreach (var property in properties)
            {
                var editorRow = new EditorRow();
                editorRow.Properties.Caption = property.Key;
                editorRow.Name = property.Key;
                var propertyObjectValue = property.Value as ObjectProperty;
                if (propertyObjectValue != null)
                {
                    editorRow.Properties.RowEdit =
                        DesignerExtensions.CreateRepositoryItem(repositoryItemButtonEdit_ButtonClick);
                    editorRow.Properties.RowEdit.Tag = propertyObjectValue;
                    editorRow.Properties.Value = propertyObjectValue.Value;

                    _snapshotSimpleProperties.Add(property.Key, propertyObjectValue.Value);
                }
                else
                {
                    editorRow.Properties.RowEdit = _repository.GetRepositoryItem(property.Key, property.Value) ??
                                                   (!string.IsNullOrEmpty(ParentProperty)
                                                       ? _repository.GetRepositoryItem(
                                                           ParentProperty + "." + property.Key, property.Value)
                                                       : null);

                    editorRow.Properties.Value = property.Value.Value;
                    _snapshotSimpleProperties.Add(property.Key, property.Value.Value);
                }

                SimplePropertiesGrid.Rows.Add(editorRow);
            }
        }

        public void FillSimpleProperties()
        {
            foreach (var row in SimplePropertiesGrid.Rows)
            {
                var editorRow = row as EditorRow;


                if (editorRow != null)
                {
                    if (_simpleProperties[editorRow.Properties.Caption] is SimpleProperty)
                    {
                        _simpleProperties[editorRow.Properties.Caption].Value = editorRow.Properties.Value;
                    }
                    else
                    {
                        _simpleProperties[editorRow.Properties.Caption] =
                            editorRow.Properties.RowEdit.Tag as ObjectProperty;
                    }
                }
            }
        }

        public void FillCollectionProperties()
        {
            _collectionProperties.Clear();
            foreach (var collectionEditor in _collectionEditors)
            {
                _collectionProperties.Add(collectionEditor.PropertyName, collectionEditor.CollectionProperty);
            }
        }

        public void RevertChanges()
        {
            foreach (var snapshotObjectProperty in _snapshotSimpleProperties)
            {
                _simpleProperties[snapshotObjectProperty.Key].Value = snapshotObjectProperty.Value;
            }

            foreach (var snapshotCollectionProperty in _snapshotCollectionProperties)
            {
                _collectionProperties[snapshotCollectionProperty.Key] = snapshotCollectionProperty.Value;
            }
        }

        public void SetCollectionProperties(Dictionary<string, CollectionProperty> properties)
        {
            _collectionProperties = properties;
            _collectionEditors.Clear();
            _snapshotCollectionProperties.Clear();

            foreach (var collectionProperty in properties)
            {
                tabControl1.TabPages.Add(collectionProperty.Key, collectionProperty.Key);
                var addedPage = tabControl1.TabPages[collectionProperty.Key];

                var collectionEditor = new CollectionEditor();

                collectionEditor.Dock = DockStyle.Fill;
                collectionEditor.SetPropertyEditors(_repository);
                collectionEditor.PropertyName = collectionProperty.Key;
                collectionEditor.CollectionProperty = collectionProperty.Value;

                _snapshotCollectionProperties.Add(collectionProperty.Key, collectionProperty.Value);

                addedPage.Controls.Add(collectionEditor);

                _collectionEditors.Add(collectionEditor);
            }
        }

        private void repositoryItemButtonEdit_RemoveClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Tag != null && e.Button.Tag.ToString() == "RemoveProperty")
            {
                var propertyName = SimplePropertiesGrid.FocusedRow.Name;
                var rowEdit =
                    SimplePropertiesGrid.Rows.Cast<BaseRow>()
                        .FirstOrDefault(r => r.Name == propertyName);
                SimplePropertiesGrid.HideEditor();

                if (MessageBox.Show(string.Format("Remove property: {0} ?", propertyName), "NeedConfirm",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    RemoveEditor(rowEdit, propertyName);
                }
            }
        }

        private void RemoveEditor(BaseRow rowEdit, string propertyName)
        {
            SimplePropertiesGrid.Rows.Remove(rowEdit);
            SimplePropertiesGrid.Refresh();
            HideProperties.Add(propertyName, rowEdit);
        }

        private void repositoryItemButtonEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Delete)
            {
                ((BaseEdit) sender).EditValue = new DynamicWrapper();
            }
            else if (e.Button.Kind == ButtonPredefines.Glyph && e.Button.Tag == null)
            {
                var value = ((BaseEdit) sender).EditValue;
                var form = new ValueEdit();
                form.ReadOnly = true;
                form.Value = value != null ? value.ToString() : string.Empty;
                form.ShowDialog();
            }
            else if (e.Button.Kind == ButtonPredefines.Ellipsis)
            {
                var properties = (ObjectProperty) ((RepositoryItemButtonEdit) ((ButtonEdit) sender).Tag).Tag;

                var propertiesForm = new PropertiesForm();

                DesignerExtensions.SetSimplePropertiesFromInstance(properties.SimpleProperties, properties.Value);
                DesignerExtensions.SetCollectionPropertiesFromInstance(properties.CollectionProperties, properties.Value);


                propertiesForm.ParentProperty = SimplePropertiesGrid.FocusedRow.Name;
                propertiesForm.SetValidationRules(properties.ValidationRules);
                propertiesForm.SetPropertyEditors(_repository.Editors);
                propertiesForm.SetSimpleProperties(properties.SimpleProperties);
                propertiesForm.SetCollectionProperties(properties.CollectionProperties);
                if (propertiesForm.ShowDialog() == DialogResult.OK)
                {
                    dynamic instance = ((ButtonEdit) sender).EditValue;

                    DesignerExtensions.SetSimplePropertiesToInstance(properties.SimpleProperties, instance);
                    DesignerExtensions.SetCollectionPropertiesToInstance(properties.CollectionProperties, instance);


                    ((ButtonEdit) sender).Refresh();
                }
                else
                {
                    propertiesForm.RevertChanges();
                }
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            FillSimpleProperties();
            FillCollectionProperties();
            if (!CheckRowInfo())
            {
                MessageBox.Show("See validation errors. Can't apply properties for control.", "Fail to apply properties",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private bool CheckRowInfo()
        {
            var rows = SimplePropertiesGrid.Rows.OfType<EditorRow>().ToList();
            foreach (var editorRow in rows)
            {
                var rowInfo = GetRowInfo(editorRow);
                if (rowInfo != null)
                {
                    var editViewInfo = rowInfo.ValuesInfo[0].EditorViewInfo;
                    if (editViewInfo.ShowErrorIcon)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void SetPropertyEditors(Dictionary<string, Func<IPropertyEditor>> propertyEditors)
        {
            foreach (var propertyEditor in propertyEditors)
            {
                _repository.RegisterEditor(propertyEditor.Key, propertyEditor.Value);
            }
        }

        public void SetValidationRules(Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> validationRules)
        {
            _validationRules = validationRules;
        }

        private void SimplePropertiesGrid_CustomDrawRowValueCell(object sender, CustomDrawRowValueCellEventArgs e)
        {
            var propertyName = e.Row.Name;
            var validationMessage = string.Empty;

            var rule =
                _validationRules.Where(
                    v =>
                        v.Key == propertyName ||
                        (ParentProperty != null && ParentProperty + "." + propertyName == v.Key))
                    .Select(r => r.Value)
                    .FirstOrDefault();
            if (rule != null)
            {
                try
                {
                    var validationResult = rule(GetItemProperty);
                    if (!validationResult.IsValid)
                    {
                        validationMessage = string.Join(";", validationResult.Items.Select(i => i.ToString()));
                    }

                    else
                    {
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
            else
            {
                return;
            }

            var rowInfo = GetRowInfo(e.Row);
            var editViewInfo = GetEditorViewInfo(rowInfo, e);
            editViewInfo.ErrorIconText = validationMessage;
            editViewInfo.ShowErrorIcon = true;
            editViewInfo.FillBackground = true;
            editViewInfo.ErrorIcon = DXErrorProvider.GetErrorIconInternal(ErrorType.Critical);
            editViewInfo.CalcViewInfo(e.Graphics);
        }

        private BaseRowViewInfo GetRowInfo(BaseRow row)
        {
            var rowsViewInfo = SimplePropertiesGrid.ViewInfo.RowsViewInfo;
            for (var i = 0; i < rowsViewInfo.Count; i++)
            {
                var info = rowsViewInfo[i];
                if (info.Row == row)
                    return info;
            }
            return null;
        }

        public BaseEditViewInfo GetEditorViewInfo(BaseRowViewInfo rowInfo, CustomDrawRowValueCellEventArgs e)
        {
            if (rowInfo == null) return null;
            for (var i = 0; i < rowInfo.ValuesInfo.Count; i++)
            {
                var valuesInfo = rowInfo.ValuesInfo[i];
                if (valuesInfo.RecordIndex == e.RecordIndex && valuesInfo.RowCellIndex == e.CellIndex)
                    return valuesInfo.EditorViewInfo;
            }
            return null;
        }
    }
}