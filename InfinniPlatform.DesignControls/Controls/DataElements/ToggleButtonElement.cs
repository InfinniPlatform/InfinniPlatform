using System;
using System.Collections.Generic;
using System.Windows.Forms;

using InfinniPlatform.Core.Validation;
using InfinniPlatform.DesignControls.Controls.Properties;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.DesignControls.PropertyEditors;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DesignControls.Controls.DataElements
{
    public partial class ToggleButtonElement : UserControl, IPropertiesProvider, ILayoutProvider, IClientHeightProvider,
        IInspectedItem
    {
        private readonly Dictionary<string, IControlProperty> _simpleProperties =
            new Dictionary<string, IControlProperty>();

        public ToggleButtonElement()
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
            ApplyDesignProperties();
        }

        public string GetPropertyName()
        {
            return "ToggleButton";
        }

        public void ApplySimpleProperties()
        {
            ApplyDesignProperties();
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
            return new Dictionary<string, Func<IPropertyEditor>>()
                .InheritBaseElementPropertyEditors(ObjectInspector)
                .InheritBindingPropertyEditors(ObjectInspector);
        }

        public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
        {
            return new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>
            {
                {"TextOn", Common.CreateNullOrEmptyValidator("ToggleButton", "TextOn")},
                {"TextOff", Common.CreateNullOrEmptyValidator("ToggleButton", "TextOff")}
            };
        }

        private void InitProperties()
        {
            _simpleProperties.InheritBaseElementSimpleProperties();
            _simpleProperties.InheritBaseElementValueBinding();
            _simpleProperties.Add("ReadOnly", new SimpleProperty(false));
            _simpleProperties.Add("TextOn", new SimpleProperty("ON"));
            _simpleProperties.Add("TextOff", new SimpleProperty("OFF"));
            _simpleProperties.Add("OnValueChanged", new ObjectProperty(new Dictionary<string, IControlProperty>
            {
                {"Name", new SimpleProperty(string.Empty)}
            }, new Dictionary<string, CollectionProperty>()));
        }

        private void ApplyDesignProperties()
        {
            ToggleSwitch.Properties.OffText = _simpleProperties["TextOff"].ToString();
            ToggleSwitch.Properties.OnText = _simpleProperties["TextOn"].ToString();
        }
    }
}