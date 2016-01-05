using System;
using System.Collections.Generic;
using System.Windows.Forms;
using InfinniPlatform.DesignControls.Controls.Properties;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.DesignControls.PropertyEditors;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Validations;

namespace InfinniPlatform.DesignControls.Controls.DataElements
{
    public partial class ListBoxElement : UserControl, IPropertiesProvider, ILayoutProvider, IInspectedItem,
        IClientHeightProvider
    {
        private readonly ObjectProperty _itemsItems = new ObjectProperty(new Dictionary<string, IControlProperty>
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

        private readonly Dictionary<string, IControlProperty> _simpleProperties =
            new Dictionary<string, IControlProperty>();

        public ListBoxElement()
        {
            InitializeComponent();

            InitProperties();
        }

        public int GetClientHeight()
        {
            return 90;
        }

        public bool IsFixedHeight()
        {
            return true;
        }

        public ObjectInspectorTree ObjectInspector { get; set; }

        public dynamic GetLayout()
        {
            dynamic instanceLayout = new DynamicWrapper();
            DesignerExtensions.SetSimplePropertiesToInstance(_simpleProperties, instanceLayout);

            return instanceLayout;
        }

        public void SetLayout(dynamic value)
        {
            CreateDesignProperties();
        }

        public string GetPropertyName()
        {
            return "ListBox";
        }

        public void ApplySimpleProperties()
        {
            CreateDesignProperties();
        }

        public void ApplyCollections()
        {
        }

        public Dictionary<string, IControlProperty> GetSimpleProperties()
        {
            return _simpleProperties;
        }

        public Dictionary<string, CollectionProperty> GetCollections()
        {
            return new Dictionary<string, CollectionProperty>();
        }

        public void LoadProperties(dynamic value)
        {
            DesignerExtensions.SetSimplePropertiesFromInstance(_simpleProperties, value);
        }

        public Dictionary<string, Func<IPropertyEditor>> GetPropertyEditors()
        {
            return new Dictionary<string, Func<IPropertyEditor>>
            {
                {"ReadOnly", () => new BooleanEditor()},
                {"ItemTemplate", () => new JsonObjectEditor()},
                {"ToolBar", () => new JsonObjectEditor()}
            }
                .InheritBaseElementPropertyEditors(ObjectInspector)
                .InheritBindingPropertyEditors(ObjectInspector);
        }

        public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
        {
            return new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>
            {
                {"Items", Common.CreateNullOrEmptyValidator("ListBox", "Items")}
            }
                .InheritBaseElementValidators("ListBox");
        }

        private void InitProperties()
        {
            _simpleProperties.InheritBaseElementSimpleProperties();
            _simpleProperties.Add("MultiSelect", new SimpleProperty(false));
            _simpleProperties.Add("ReadOnly", new SimpleProperty(false));
            _simpleProperties.Add("ValueProperty", new SimpleProperty(string.Empty));
            _simpleProperties.Add("DisplayProperty", new SimpleProperty(string.Empty));
            _simpleProperties.Add("LineCount", new SimpleProperty(0));

            _simpleProperties.Add("ItemFormat", new SimpleProperty(null));
            _simpleProperties.Add("OnValueChanged", new ObjectProperty(new Dictionary<string, IControlProperty>
            {
                {"Name", new SimpleProperty(string.Empty)}
            }, new Dictionary<string, CollectionProperty>()));

            _simpleProperties.InheritBaseElementValueBinding();
            _simpleProperties.Add("ItemTemplate", new SimpleProperty(null));
            _simpleProperties.Add("Items", _itemsItems);
            _simpleProperties.Add("ToolBar",
                new ObjectProperty(new Dictionary<string, IControlProperty>(),
                    new Dictionary<string, CollectionProperty>()));
        }

        private void CreateDesignProperties()
        {
            ListBox.Items.Clear();
            ListBox.Items.Add("item1");
            ListBox.Items.Add("item2");
            ListBox.Items.Add("item3");
        }
    }
}