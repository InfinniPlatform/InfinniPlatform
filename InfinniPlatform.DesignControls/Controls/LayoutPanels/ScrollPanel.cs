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

namespace InfinniPlatform.DesignControls.Controls.LayoutPanels
{
    public partial class ScrollPanel : UserControl, IPropertiesProvider, ILayoutProvider, ILayoutContainer, IAlignment,
        IControlHost, IInspectedItem
    {
        private readonly CompositPanel _compositPanel;
        private readonly Dictionary<string, IControlProperty> _properties = new Dictionary<string, IControlProperty>();

        public ScrollPanel()
        {
            InitializeComponent();

            _compositPanel = new CompositPanel();
            _compositPanel.Dock = DockStyle.Fill;
            Controls.Add(_compositPanel);

            InitProperties();
        }

        public void AlignControls()
        {
            _compositPanel.Height = Height - Padding.Top - Padding.Bottom;
            _compositPanel.AlignControls();
        }

        public CompositPanel GetHost()
        {
            return _compositPanel;
        }

        public ObjectInspectorTree ObjectInspector
        {
            get { return _compositPanel.ObjectInspector; }
            set { _compositPanel.ObjectInspector = value; }
        }

        public void InsertLayout(dynamic layout)
        {
            _compositPanel.InsertLayout(layout);
        }

        public dynamic GetLayout()
        {
            dynamic instanceLayout = new DynamicWrapper();

            DesignerExtensions.SetSimplePropertiesToInstance(this, instanceLayout);
            instanceLayout.LayoutPanel = _compositPanel.GetLayout().FirstOrDefault();
            return instanceLayout;
        }

        public void SetLayout(dynamic value)
        {
            _compositPanel.InsertLayout(value.LayoutPanel);
        }

        public virtual string GetPropertyName()
        {
            return "ScrollPanel";
        }

        public void ApplySimpleProperties()
        {
        }

        public void ApplyCollections()
        {
        }

        public Dictionary<string, IControlProperty> GetSimpleProperties()
        {
            return _properties;
        }

        public Dictionary<string, CollectionProperty> GetCollections()
        {
            return new Dictionary<string, CollectionProperty>();
        }

        public void LoadProperties(dynamic value)
        {
            DesignerExtensions.SetSimplePropertiesFromInstance(_properties, value);
        }

        public Dictionary<string, Func<IPropertyEditor>> GetPropertyEditors()
        {
            return new Dictionary<string, Func<IPropertyEditor>>().InheritBaseElementPropertyEditors(ObjectInspector);
        }

        public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
        {
            return
                new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>().InheritBaseElementValidators(
                    "ScrollPanel");
        }

        private void InitProperties()
        {
            _properties.InheritBaseElementSimpleProperties();
        }
    }
}