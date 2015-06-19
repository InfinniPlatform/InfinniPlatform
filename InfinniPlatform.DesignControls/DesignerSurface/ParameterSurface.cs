using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
using InfinniPlatform.DesignControls.Controls.Parameters;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.PropertyDesigner;

namespace InfinniPlatform.DesignControls.DesignerSurface
{
    public partial class ParameterSurface : UserControl, ILayoutProvider
    {
        private readonly List<ParameterObject> _parameters = new List<ParameterObject>();
        private readonly EditorRepository _repository;

        public ParameterSurface()
        {
            InitializeComponent();

            gridBinding.DataSource = _parameters;

            _repository = new EditorRepository(GetItemProperty);
        }

        public dynamic GetLayout()
        {
            dynamic instanceLayout = new List<dynamic>();

            foreach (ILayoutProvider parameterObject in _parameters)
            {
                dynamic instance = parameterObject.GetLayout();
                instanceLayout.Add(instance);
            }
            return instanceLayout;
        }

        public void SetLayout(dynamic value)
        {
        }

        public string GetPropertyName()
        {
            return "Parameters";
        }

        protected dynamic GetItemProperty(string propertyName)
        {
            return GridViewParameters
                .GetFocusedDataRow().Field<object>(propertyName);
        }

        private void repositoryItemButtonEditSource_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            var dataSource = _parameters.ElementAt(GridViewParameters.FocusedRowHandle).Parameter as IPropertiesProvider;
            if (dataSource != null)
            {
                var form = new PropertiesForm();
                var simpleProperties = dataSource.GetSimpleProperties();
                form.SetSimpleProperties(simpleProperties);
                var collectionProperties = dataSource.GetCollections();
                form.SetCollectionProperties(collectionProperties);
                form.SetValidationRules(dataSource.GetValidationRules());
                form.SetPropertyEditors(_repository.Editors);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    dataSource.ApplySimpleProperties();
                    dataSource.ApplyCollections();
                    GridViewParameters.HideEditor();
                    GridViewParameters.RefreshData();
                }
                else
                {
                    form.RevertChanges();
                }
            }
        }

        private void AddScriptButton_Click(object sender, EventArgs e)
        {
            var parameterObject = new ParameterObject();
            _parameters.Add(parameterObject);
            parameterObject.Parameter = new Parameter();
            GridViewParameters.RefreshData();
        }

        private void DeleteScriptButton_Click(object sender, EventArgs e)
        {
            if (GridViewParameters.FocusedRowHandle >= 0)
            {
                _parameters.RemoveAt(GridViewParameters.FocusedRowHandle);
                GridViewParameters.RefreshData();
            }
        }

        private void GEtLayoutButton_Click(object sender, EventArgs e)
        {
            dynamic value = GetLayout();

            var valueEdit = new ValueEdit();
            valueEdit.Value = value.ToString();
            valueEdit.ShowDialog();
        }

        private void GridViewScripts_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            var value = (ParameterObject) GridViewParameters.GetRow(e.RowHandle);
            if (value == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(value.ParameterName))
            {
                e.DisplayText = "<Parameter name not specified>";
            }
            else
            {
                e.DisplayText = value.ParameterName;
            }
        }

        public void ProcessJson(dynamic value)
        {
            _parameters.Clear();
            foreach (var source in value)
            {
                var parameterObject = new ParameterObject();

                var parameter = new Parameter();
                parameter.LoadProperties(source);
                parameterObject.Parameter = parameter;

                _parameters.Add(parameterObject);
            }
            GridViewParameters.RefreshData();
        }

        public void Clear()
        {
            _parameters.Clear();
            GridViewParameters.RefreshData();
        }
    }
}