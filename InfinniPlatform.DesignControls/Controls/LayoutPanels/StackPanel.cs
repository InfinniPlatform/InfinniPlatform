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

namespace InfinniPlatform.DesignControls.Controls.LayoutPanels
{
    public partial class StackPanel : UserControl, IPropertiesProvider, ILayoutProvider, ILayoutContainer, IAlignment,
        IControlHost, IInspectedItem
    {
        private readonly CompositPanel _compositPanel;

        private readonly Dictionary<string, IControlProperty> _simpleProperties =
            new Dictionary<string, IControlProperty>();

        public StackPanel()
        {
            InitializeComponent();

            _compositPanel = new CompositPanel();
            _compositPanel.Dock = DockStyle.Fill;
            Controls.Add(_compositPanel);

            InitProperties();
        }

        public void AlignControls()
        {
            _compositPanel.AlignControls();
        }

        public CompositPanel GetHost()
        {
            return _compositPanel;
        }

        public ObjectInspectorTree ObjectInspector { get; set; }

        public void InsertLayout(dynamic layout)
        {
            _compositPanel.InsertLayout(layout);
        }

        public dynamic GetLayout()
        {
            dynamic instanceLayout = new DynamicWrapper();

            DesignerExtensions.SetSimplePropertiesToInstance(this, instanceLayout);

            instanceLayout.Items = new List<dynamic>();
            foreach (var layoutProvider in _compositPanel.GetLayoutControls())
            {
                dynamic instanceItem = new DynamicWrapper();
                instanceLayout.Items.Add(instanceItem);
                instanceItem[layoutProvider.GetPropertyName()] = layoutProvider.GetLayout();
            }
            return instanceLayout;
        }

        public void SetLayout(dynamic value)
        {
            _compositPanel.SetLayout(value.Items);
        }

        public string GetPropertyName()
        {
            return "StackPanel";
        }

        public void ApplySimpleProperties()
        {
            ApplyOrientationPanels();
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
            return new Dictionary<string, Func<IPropertyEditor>>().InheritBaseElementPropertyEditors(ObjectInspector);
        }

        public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
        {
            return new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>
            {
                {"Orientation", Common.CreateNullOrEmptyValidator("StackPanel", "Orientation")}
            }.InheritBaseElementValidators("StackPanel");
        }

        private void InitProperties()
        {
            _simpleProperties.InheritBaseElementSimpleProperties();
            _simpleProperties.Add("Orientation", new SimpleProperty("Vertical"));
        }

        private void ApplyOrientationPanels()
        {
            var orientation = _simpleProperties["Orientation"].Value.ToString();
            _compositPanel.ControlOrientation = orientation;
        }
    }
}