using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;

using InfinniPlatform.Core.Validation;
using InfinniPlatform.DesignControls.Controls.Properties;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.DesignControls.PropertyEditors;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DesignControls.Controls.DataElements
{
    public partial class DataGridElement : UserControl, IPropertiesProvider, ILayoutProvider, IInspectedItem,
        IClientHeightProvider
    {
        private Dictionary<string, CollectionProperty> _collectionProperties;
        private IEnumerable<dynamic> _columnsItems = new List<dynamic>();
        private Dictionary<string, IControlProperty> _properties;

        private readonly CollectionProperty _columns = new CollectionProperty(new Dictionary<string, IControlProperty>
        {
            {"Name", new SimpleProperty("Column1")},
            {"Text", new SimpleProperty("Caption")},
            {"DisplayProperty", new SimpleProperty("Here should be a field name")},
            {
                "ItemFormat", new ObjectProperty(new Dictionary<string, IControlProperty>
                {
                    {
                        "CustomFormat", new ObjectProperty(new Dictionary<string, IControlProperty>
                        {
                            {"FormatString", new SimpleProperty(string.Empty)}
                        }, new Dictionary<string, CollectionProperty>())
                    }
                }, new Dictionary<string, CollectionProperty>())
            }
        });

        private readonly ObjectProperty _items = new ObjectProperty(new Dictionary<string, IControlProperty>
        {
            {
                "PropertyBinding", new ObjectProperty(new Dictionary<string, IControlProperty>
                {
                    {"DataSource", new SimpleProperty(string.Empty)},
                    {"Property", new SimpleProperty(string.Empty)},
                    {"DefaultValue", new SimpleProperty(string.Empty)}
                }, new Dictionary<string, CollectionProperty>())
            }
        }, new Dictionary<string, CollectionProperty>());

        private readonly ObjectProperty _onValueChanged = new ObjectProperty(new Dictionary<string, IControlProperty>
        {
            {"Name", new SimpleProperty(string.Empty)}
        }, new Dictionary<string, CollectionProperty>());

        public DataGridElement()
        {
            InitializeComponent();

            InitProperties();
        }

        public int GetClientHeight()
        {
            return 120;
        }

        public bool IsFixedHeight()
        {
            return true;
        }

        public ObjectInspectorTree ObjectInspector { get; set; }

        public dynamic GetLayout()
        {
            dynamic instance = new DynamicWrapper();

            DesignerExtensions.SetSimplePropertiesToInstance(this, instance);
            instance.Columns = _columnsItems;

            return instance;
        }

        public void SetLayout(dynamic value)
        {
            //no inner layout contained
        }

        public string GetPropertyName()
        {
            return "DataGrid";
        }

        public void ApplySimpleProperties()
        {
            //визуальное применение настроек байдингов для Items
            ScriptEditor.Script = ((ObjectProperty) _properties["Items"]).Value.PropertyBinding != null
                ? ((ObjectProperty) _properties["Items"]).Value.PropertyBinding.ToString()
                : "";
        }

        public void ApplyCollections()
        {
            _columnsItems = _collectionProperties["Columns"].Items;
            DataGridView.Columns.Clear();
            foreach (var item in _columnsItems)
            {
                var column = new GridColumn();
                column.Caption = item.Text;
                column.Name = item.Name;
                column.Visible = true;
                column.Width = (DataGridControl.Width/_columnsItems.Count()) - 5;
                DataGridView.Columns.Add(column);
            }
        }

        public Dictionary<string, IControlProperty> GetSimpleProperties()
        {
            return _properties;
        }

        public Dictionary<string, CollectionProperty> GetCollections()
        {
            return _collectionProperties;
        }

        public void LoadProperties(dynamic value)
        {
            DesignerExtensions.SetSimplePropertiesFromInstance(_properties, value);

            var columns = (value.Columns as IEnumerable);
            if (columns != null)
            {
                _collectionProperties["Columns"].Items = columns.OfType<dynamic>().ToList();
            }
        }

        public Dictionary<string, Func<IPropertyEditor>> GetPropertyEditors()
        {
            return new Dictionary<string, Func<IPropertyEditor>>()
                .InheritBaseElementPropertyEditors(ObjectInspector)
                .InheritBindingPropertyEditors(ObjectInspector);
        }

        public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
        {
            return
                new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>().InheritBaseElementValidators(
                    "DataGrid");
        }

        private void InitProperties()
        {
            _properties = new Dictionary<string, IControlProperty>();
            _properties.InheritBaseElementSimpleProperties();
            _properties.Add("KeyProperty", new SimpleProperty("Id"));
            _properties.Add("ValueProperty", new SimpleProperty(string.Empty));
            _properties.Add("Items", _items);
            _properties.Add("OnValueChanged", _onValueChanged);

            _collectionProperties = new Dictionary<string, CollectionProperty>();
            _collectionProperties.Add("Columns", _columns);
        }
    }
}