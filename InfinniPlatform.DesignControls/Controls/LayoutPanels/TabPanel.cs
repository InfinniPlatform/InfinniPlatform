using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraTab;
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
    public partial class TabPanel : UserControl, IPropertiesProvider, ILayoutProvider, IAlignment, IInspectedItem
    {
        private CompositPanel _compositPanel;
        private FocusedNodeMemento _memento;
        private ObjectInspectorTree _objectInspector;

        private readonly Dictionary<string, CollectionProperty> _collectionProperties =
            new Dictionary<string, CollectionProperty>();

        private readonly CollectionProperty _pages = new CollectionProperty(new Dictionary<string, IControlProperty>
        {
            {"Image", new SimpleProperty(string.Empty)},
            {"CanClose", new SimpleProperty(false)}
        }.InheritBaseElementSimpleProperties());

        private readonly List<CompositPanel> _panels = new List<CompositPanel>();
        private readonly Dictionary<string, IControlProperty> _properties = new Dictionary<string, IControlProperty>();

        public TabPanel()
        {
            InitializeComponent();

            InitProperties();
        }

        public void AlignControls()
        {
            for (var i = 0; i < _panels.Count; i++)
            {
                TabControl.SelectedTabPageIndex = i;
                _panels[i].AlignControls();
            }
        }

        public ObjectInspectorTree ObjectInspector
        {
            get { return _objectInspector; }
            set
            {
                _objectInspector = value;
                _memento = new FocusedNodeMemento(_objectInspector);
            }
        }

        public dynamic GetLayout()
        {
            dynamic instanceLayout = new DynamicWrapper();
            DesignerExtensions.SetSimplePropertiesToInstance(_properties, instanceLayout);


            instanceLayout.Pages = new List<dynamic>();
            for (var i = 0; i < _collectionProperties["Pages"].Items.Count; i++)
            {
                dynamic instancePage = _collectionProperties["Pages"].Items[i];
                //DesignerExtensions.SetSimplePropertiesToInstance(_collectionProperties["Pages"].Items[i],instancePage);


                var layoutPanel = TabControl.TabPages[i].Controls.OfType<CompositPanel>().FirstOrDefault();
                if (layoutPanel != null)
                {
                    IEnumerable<dynamic> layout = layoutPanel.GetLayout();
                    instancePage.LayoutPanel = layout.FirstOrDefault() ?? new DynamicWrapper();
                }
                instanceLayout.Pages.Add(instancePage);
            }

            return instanceLayout;
        }

        public void SetLayout(dynamic value)
        {
            CreatePages();

            _memento.BeginUpdate();
            for (var i = 0; i < value.Pages.Count(); i++)
            {
                ObjectInspector.SelectElement(_panels[i]);
                var layoutPanel = value.Pages[i].LayoutPanel;
                if (layoutPanel != null)
                {
                    _panels[i].SetLayout(new[] {layoutPanel});
                }
            }
            AlignControls();

            _memento.EndUpdate();
        }

        public string GetPropertyName()
        {
            return "TabPanel";
        }

        public void ApplySimpleProperties()
        {
        }

        public void ApplyCollections()
        {
            CreatePages();
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

            var pages = (value.Pages as IEnumerable);
            if (pages != null)
            {
                _collectionProperties["Pages"].Items = pages.OfType<dynamic>().ToList();
            }
        }

        public Dictionary<string, Func<IPropertyEditor>> GetPropertyEditors()
        {
            return new Dictionary<string, Func<IPropertyEditor>>()
                .InheritBaseElementPropertyEditors(ObjectInspector);
        }

        public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
        {
            return
                new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>().InheritBaseElementValidators(
                    "TabPanel");
        }

        private void InitProperties()
        {
            _properties.InheritBaseElementSimpleProperties();
            _properties.Add("HeaderLocation", new SimpleProperty("Left"));
            _properties.Add("HeaderOrientation", new SimpleProperty("Horizontal"));
            _properties.Add("DefaultPage", new SimpleProperty(null));

            _compositPanel = new CompositPanel();
            _compositPanel.Dock = DockStyle.Fill;
            Controls.Add(_compositPanel);

            _collectionProperties.Add("Pages", _pages);
        }

        private void CreatePages()
        {
            var property = _collectionProperties["Pages"];

            foreach (var item in property.Items)
            {
                if (TabControl.TabPages.FirstOrDefault(p => p.Name == item.Name) == null)
                {
                    TabControl.TabPages.Add(new XtraTabPage
                    {
                        Name = item.Name,
                        Text = item.Text
                    });

                    var panel = new CompositPanel();
                    panel.Dock = DockStyle.Fill;
                    _panels.Add(panel);

                    var page = TabControl.TabPages.FirstOrDefault(p => p.Name == item.Name);
                    page.Controls.Add(panel);


                    _memento.BeginUpdate();
                    if (ObjectInspector != null)
                    {
                        var propertyNode = new PropertiesNode(panel)
                        {
                            ControlName = item.Name,
                            EnabledLayoutTypes = new List<EnabledItems>
                            {
                                EnabledItems.Layout
                            }
                        };
                        ObjectInspector.AddNode(propertyNode);
                    }
                    _memento.EndUpdate();
                }
            }
        }
    }
}