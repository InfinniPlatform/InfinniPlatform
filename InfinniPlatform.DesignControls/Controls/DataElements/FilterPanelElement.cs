using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using InfinniPlatform.Core.Validation;
using InfinniPlatform.DesignControls.Controls.LayoutPanels.GridPanels;
using InfinniPlatform.DesignControls.Controls.Properties;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.DesignControls.PropertyEditors;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DesignControls.Controls.DataElements
{
    public partial class FilterPanelElement : UserControl, IPropertiesProvider, ILayoutProvider, IInspectedItem,
        IClientHeightProvider
    {
        private Dictionary<string, CollectionProperty> _collectionProperties;
        private int _fullRowCount;
        private FocusedNodeMemento _memento;
        private ObjectInspectorTree _objectInspector;

        private readonly CollectionProperty _additionalProperties =
            new CollectionProperty(new Dictionary<string, IControlProperty>
            {
                {"Text", new SimpleProperty(string.Empty)},
                {"Property", new SimpleProperty(string.Empty)},
                {"DefaultOperator", new SimpleProperty(1)},
                {
                    "Operators", new CollectionProperty(new Dictionary<string, IControlProperty>
                    {
                        {"Operator", new SimpleProperty(1)},
                        {
                            "Editor",
                            new ObjectProperty(new Dictionary<string, IControlProperty>().GetDataElements(),
                                new Dictionary<string, CollectionProperty>())
                        }
                    })
                }
            });

        private readonly CollectionProperty _generalProperties =
            new CollectionProperty(new Dictionary<string, IControlProperty>
            {
                {"Text", new SimpleProperty(string.Empty)},
                {"Property", new SimpleProperty(string.Empty)},
                {"DefaultOperator", new SimpleProperty(1)},
                {
                    "Operators", new CollectionProperty(new Dictionary<string, IControlProperty>
                    {
                        {"Operator", new SimpleProperty(1)},
                        {
                            "Editor",
                            new ObjectProperty(new Dictionary<string, IControlProperty>().GetDataElements(),
                                new Dictionary<string, CollectionProperty>())
                        }
                    })
                }
            });

        private readonly Dictionary<string, IControlProperty> _simpleProperties =
            new Dictionary<string, IControlProperty>();

        public FilterPanelElement()
        {
            InitializeComponent();

            InitProperties();
        }

        public int GetClientHeight()
        {
            return _fullRowCount == 0 ? 120 : _fullRowCount*50 + LabelGeneral.Height + LabelAdditional.Height + 10;
        }

        public bool IsFixedHeight()
        {
            return true;
        }

        public ObjectInspectorTree ObjectInspector
        {
            get { return _objectInspector; }
            set
            {
                _objectInspector = value;
                GridPanelAdditional.ObjectInspector = value;
                GridPanelGeneral.ObjectInspector = value;
                _memento = new FocusedNodeMemento(_objectInspector);
            }
        }

        public dynamic GetLayout()
        {
            dynamic instanceLayout = new DynamicWrapper();
            DesignerExtensions.SetSimplePropertiesToInstance(_simpleProperties, instanceLayout);

            CheckOperators(_generalProperties.Items);
            CheckOperators(_additionalProperties.Items);

            instanceLayout.GeneralProperties = _generalProperties.Items;
            instanceLayout.AdditionalProperties = _additionalProperties.Items;
            return instanceLayout;
        }

        public void SetLayout(dynamic value)
        {
        }

        public string GetPropertyName()
        {
            return "FilterPanel";
        }

        public void ApplySimpleProperties()
        {
        }

        public void ApplyCollections()
        {
            _fullRowCount = 0;


            foreach (var collectionProperty in _collectionProperties)
            {
                if (collectionProperty.Key == "GeneralProperties")
                {
                    _memento.BeginUpdate();
                    ApplyGeneralProperties();
                    _memento.EndUpdate();
                }
                else
                {
                    _memento.BeginUpdate();
                    ApplyAdditionalProperties();
                    _memento.EndUpdate();
                }
            }


            var alignment = Parent.Parent.Parent.Parent as IAlignment;
            if (alignment != null)
            {
                alignment.AlignControls();
            }
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

            var generalProperties = (value.GeneralProperties as IEnumerable);
            if (generalProperties != null)
            {
                _collectionProperties["GeneralProperties"].Items = generalProperties.OfType<dynamic>().ToList();
            }

            var additionalProperties = (value.AdditionalProperties as IEnumerable);
            if (additionalProperties != null)
            {
                _collectionProperties["AdditionalProperties"].Items = additionalProperties.OfType<dynamic>().ToList();
            }
        }

        public Dictionary<string, Func<IPropertyEditor>> GetPropertyEditors()
        {
            return new Dictionary<string, Func<IPropertyEditor>>
            {
                {"DataSource", () => new DataSourceEditor(ObjectInspector)},
                {"Operators.Operator", () => new CriteriaTypeEditor()},
                {"DefaultOperator", () => new CriteriaTypeEditor()}
            }
                .InheritBaseElementPropertyEditors(ObjectInspector);
        }

        public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
        {
            return new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>
            {
                {"DataSource", Common.CreateNullOrEmptyValidator("FilterPanel", "DataSource")}
            }.InheritBaseElementValidators("FilterPanel");
        }

        private int ItemsInRow()
        {
            return GridPanelGeneral.Width/240;
        }

        private void ApplyAdditionalProperties()
        {
            ApplyProperties(_additionalProperties, GridPanelAdditional);
        }

        private void ApplyGeneralProperties()
        {
            ApplyProperties(_generalProperties, GridPanelGeneral);
        }

        private void ApplyProperties(CollectionProperty property, GridPanel panelToAdd)
        {
            panelToAdd.Clear();

            var itemsCount = property.Items.Count;

            var panelRowCount = itemsCount/ItemsInRow();

            var additionalItems = itemsCount - panelRowCount*ItemsInRow();

            if (additionalItems > 0)
            {
                panelRowCount++;
            }


            _fullRowCount += panelRowCount;

            panelToAdd.SetGrid(panelRowCount, ItemsInRow());

            for (var i = 0; i < property.Items.Count; i++)
            {
                var rowIndex = i/ItemsInRow();
                var colIndex = i%ItemsInRow();

                //простите, люди, очень хочется спать
                var control = (PropertiesControl) panelToAdd.Panels[rowIndex].GetCell(colIndex);
                var stubCriteria = new CriteriaStubLayout();
                stubCriteria.Dock = DockStyle.Fill;
                stubCriteria.Caption = property.Items[i].Text;
                control.Control.Controls[0].Controls.Add(stubCriteria);
            }
            panelToAdd.Height = panelRowCount*50;
            panelToAdd.AlignControls();
        }

        private void InitProperties()
        {
            _simpleProperties.InheritBaseElementSimpleProperties();
            _simpleProperties.Add("AllowAdvancedMode", new SimpleProperty(false));
            _simpleProperties.Add("DataSource", new SimpleProperty(null));

            _collectionProperties = new Dictionary<string, CollectionProperty>();
            _collectionProperties.Add("GeneralProperties", _generalProperties);
            _collectionProperties.Add("AdditionalProperties", _additionalProperties);
        }

        private void CheckOperators(List<dynamic> items)
        {
            for (var i = 0; i < items.Count; i++)
            {
                items[i].Operators = DynamicWrapperExtensions.ToDynamicList(items[i].Operators.ToString());
            }
        }
    }
}