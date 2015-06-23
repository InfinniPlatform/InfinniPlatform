using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.DesignControls.Controls.Properties;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.DesignControls.PropertyEditors;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DesignControls.Controls.DataElements
{
    public partial class ComboBoxElement : UserControl, IPropertiesProvider, ILayoutProvider, IClientHeightProvider,
        IInspectedItem
    {
        private readonly Dictionary<string, CollectionProperty> _collectionProperties =
            new Dictionary<string, CollectionProperty>();

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

        public ComboBoxElement()
        {
            InitializeComponent();

            InitProperties();
        }

        public int GetClientHeight()
        {
            return 32;
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
            //no inner layout
        }

        public string GetPropertyName()
        {
            return "ComboBox";
        }

        public void ApplySimpleProperties()
        {
            var buttons = ComboBoxEdit.Properties.Buttons.Cast<EditorButton>().ToList();
            ComboBoxEdit.Properties.Buttons.Cast<EditorButton>().ToList().ForEach(
                f => f.IsLeft = true);

            buttons.First(b => b.Kind == ButtonPredefines.Combo).Visible =
                Convert.ToBoolean(_simpleProperties.GetValue("ShowPopup"));
            buttons.First(b => b.Kind == ButtonPredefines.Ellipsis).Visible =
                Convert.ToBoolean(_simpleProperties.GetValue("ShowSelect"));
            buttons.First(b => b.Kind == ButtonPredefines.Delete).Visible =
                Convert.ToBoolean(_simpleProperties.GetValue("ShowClear"));
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
            return _collectionProperties;
        }

        public void LoadProperties(dynamic value)
        {
            DesignerExtensions.SetSimplePropertiesFromInstance(_simpleProperties, value);
        }

        public Dictionary<string, Func<IPropertyEditor>> GetPropertyEditors()
        {
            return new Dictionary<string, Func<IPropertyEditor>>
            {
                {"AutoComplete", () => new ValueListEditor(new[] {"None", "Client", "Server"})}
            }
                .InheritBaseElementPropertyEditors(ObjectInspector)
                .InheritBindingPropertyEditors(ObjectInspector);
        }

        public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
        {
            return
                new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>().InheritBaseElementValidators(
                    "ComboBox");
        }

        private void InitProperties()
        {
            _simpleProperties.InheritBaseElementSimpleProperties();
            _simpleProperties.InheritBaseElementValueBinding();
            _simpleProperties.Add("ShowPopup", new SimpleProperty(true));
            _simpleProperties.Add("ShowSelect", new SimpleProperty(false));
            _simpleProperties.Add("ShowClear", new SimpleProperty(false));
            _simpleProperties.Add("AutoComplete", new SimpleProperty("None"));
            _simpleProperties.Add("SelectView",
                new ObjectProperty(new Dictionary<string, IControlProperty>().GetLinkViews(),
                    new Dictionary<string, CollectionProperty>()));
            _simpleProperties.Add("MultiSelect", new SimpleProperty(false));
            _simpleProperties.Add("ReadOnly", new SimpleProperty(false));
            _simpleProperties.Add("ValueProperty", new SimpleProperty(string.Empty));
            _simpleProperties.Add("DisplayProperty", new SimpleProperty(string.Empty));
            //TODO: Add ItemFormat
            //TODO: Add ItemTemplate
            _simpleProperties.Add("Items", _itemsItems);
            //TODO: Add DataNavigation
            _simpleProperties.Add("OnValueChanged", new ObjectProperty(new Dictionary<string, IControlProperty>
            {
                {"Name", new SimpleProperty(string.Empty)}
            }, new Dictionary<string, CollectionProperty>()));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ComboBoxEdit.Width = Width - Margin.Left - Margin.Right;
        }
    }
}