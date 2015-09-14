using System;
using System.Collections.Generic;
using System.Windows.Forms;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.DesignControls.Controls.Properties;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.DesignControls.PropertyEditors;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Validations;

namespace InfinniPlatform.DesignControls.Controls.DataElements
{
    public partial class TextEditElement : UserControl, IPropertiesProvider, ILayoutProvider, IClientHeightProvider,
        IInspectedItem
    {
        private readonly Dictionary<string, IControlProperty> _simpleProperties =
            new Dictionary<string, IControlProperty>();

        public TextEditElement()
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
            return "TextBox";
        }

        public void ApplySimpleProperties()
        {
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
                {"HorizontalTextAlignment", () => new AlignmentEditor()},
                {"VerticalTextAlignment", () => new AlignmentEditor()}
            }
                .InheritBaseElementPropertyEditors(ObjectInspector)
                .InheritBindingPropertyEditors(ObjectInspector);
        }

        public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
        {
            return new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>
            {
                {"HorizontalTextAlignment", Common.CreateNullOrEmptyValidator("TextBox", "HorizontalTextAlignment")},
                {"VerticalTextAlignment", Common.CreateNullOrEmptyValidator("TextBox", "VerticalTextAlignment")}
            }.InheritBaseElementValidators("TextBox");
        }

        private void InitProperties()
        {
            _simpleProperties.InheritBaseElementSimpleProperties();
            _simpleProperties.InheritBaseElementValueBinding();
            _simpleProperties.Add("EditMask", new ObjectProperty(new Dictionary<string, IControlProperty>
            {
                {
                    "TemplateEditMask", new ObjectProperty(new Dictionary<string, IControlProperty>
                    {
                        {"Mask", new SimpleProperty(string.Empty)}
                    }, new Dictionary<string, CollectionProperty>())
                }
            }, new Dictionary<string, CollectionProperty>()));
            _simpleProperties.Add("HorizontalTextAlignment", new SimpleProperty("Left"));
            _simpleProperties.Add("VerticalTextAlignment", new SimpleProperty("Center"));
            _simpleProperties.Add("Multiline", new SimpleProperty(false));
            _simpleProperties.Add("LineCount", new SimpleProperty(0));
            _simpleProperties.Add("ReadOnly", new SimpleProperty(false));
            _simpleProperties.Add("OnValueChanged", new ObjectProperty(new Dictionary<string, IControlProperty>
            {
                {"Name", new SimpleProperty(string.Empty)}
            }, new Dictionary<string, CollectionProperty>()));
            //_simpleProperties.Add("Value", _itemsValue);
        }
    }
}