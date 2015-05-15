using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.DesignControls.Controls.Properties;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.DesignControls.PropertyEditors;

namespace InfinniPlatform.DesignControls.Controls.ActionElements
{
    public partial class ToolBar : UserControl, IPropertiesProvider, ILayoutProvider, IClientHeightProvider, IInspectedItem
    {
        public ToolBar()
        {
            InitializeComponent();

            InitProperties();
        }

        private readonly Dictionary<string, IControlProperty> _simpleProperties = new Dictionary<string, IControlProperty>();

        private readonly Dictionary<string, CollectionProperty> _collectionProperties = new Dictionary<string, CollectionProperty>(); 

        private void InheritToolBarButton()
        {
            _simpleProperties.InheritBaseElementSimpleProperties();
            _collectionProperties.InheritToolBarButtonCollectionProperties();
        }

        private void InitProperties()
        {
            InheritToolBarButton();
        }

        public void ApplySimpleProperties()
        {
            
        }

        public void ApplyCollections()
        {
            var items = _collectionProperties["Items"].Items;
			Bar.ClearLinks();
            CreateToolbarButtons(items,null);
        }

        private void CreateToolbarButtons(IEnumerable<dynamic> items, BarSubItem parentItem)
        {
            bool beginGroup = false;
			Bar.BeginUpdate();

            foreach (dynamic item in items)
            {
                if (!DesignerExtensions.IsEmpty(item.ToolBarButton))
                {
                    AddBarButton(item.ToolBarButton, parentItem, beginGroup);
                    beginGroup = false;
                }
                else if (!DesignerExtensions.IsEmpty(item.ToolBarPopupButton))
                {
                    var barSubItem = AddBarPopupButton(item.ToolBarPopupButton, parentItem, beginGroup);
                    IEnumerable<dynamic> innerItems =
                        DesignerExtensions.GetCollection(item.ToolBarPopupButton, "Items").ToList();
                    CreateToolbarButtons(innerItems,barSubItem);
                    beginGroup = false;
                }
                else if (!DesignerExtensions.IsEmpty(item.ToolBarSeparator))
                {
                    beginGroup = true;
                }
            }
			Bar.EndUpdate();
        }

        private BarSubItem AddBarPopupButton(dynamic toolBarPopupButton, BarSubItem barSubItem, bool beginGroup)
        {
            BarItemLink itemLink = null;
            if (barSubItem != null)
            {
                itemLink = barSubItem.ItemLinks.Add(new BarSubItem()
                    {
                        Caption = toolBarPopupButton.Text,                        
                    });
            }
            else
            {
                itemLink = Bar.AddItem(new BarSubItem()
                    {
                        Caption = toolBarPopupButton.Text
                    });                
            }
            if (itemLink != null)
            {
                itemLink.BeginGroup = beginGroup;
            }
            return itemLink.Item as BarSubItem;
        }

        private void AddBarButton(dynamic toolBarButton, BarSubItem barSubItem, bool beginGroup)
        {
            BarItemLink itemLink = null;
            if (barSubItem != null)
            {
                itemLink = barSubItem.ItemLinks.Add(new BarButtonItem()
                    {
                        Caption = toolBarButton.Text
                    });
            }
            else
            {
                itemLink = Bar.AddItem(new BarButtonItem()
                    {
                        Caption = toolBarButton.Text
                    });
            }
            if (itemLink != null)
            {
                itemLink.BeginGroup = beginGroup;
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
            DesignerExtensions.SetSimplePropertiesFromInstance(_simpleProperties,value);
            var items = (value.Items as IEnumerable);
            if (items != null)
            {
                _collectionProperties["Items"].Items = items.OfType<dynamic>().ToList();
            }
        }

		public Dictionary<string, Func<IPropertyEditor>> GetPropertyEditors()
		{
			return new Dictionary<string, Func<IPropertyEditor>>()
				.InheritBaseElementPropertyEditors(ObjectInspector)
				.InheritBindingPropertyEditors(ObjectInspector)
				.InheritViewPropertyEditors(ObjectInspector)
				;
		}

	    public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
	    {
		    return new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>().InheritBaseElementValidators("ToolBar");
	    }


	    public dynamic GetLayout()
        {
            dynamic instance = new DynamicWrapper();

            DesignerExtensions.SetSimplePropertiesToInstance(this, instance);
            instance.Items = _collectionProperties["Items"].Items;

            return instance;
        }

        public void SetLayout(dynamic value)
        {

        }

        public string GetPropertyName()
        {
            return "ToolBar";
        }

        public int GetClientHeight()
        {
            return 42;
        }

	    public bool IsFixedHeight()
	    {
		    return true;
	    }

	    public ObjectInspectorTree ObjectInspector { get; set; }
    }
}
