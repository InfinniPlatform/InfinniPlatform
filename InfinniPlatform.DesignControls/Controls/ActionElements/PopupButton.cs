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
	public partial class PopupButton : UserControl, IPropertiesProvider, ILayoutProvider, IClientHeightProvider, IInspectedItem
	{


        private readonly CollectionProperty _collectionProperty = new CollectionProperty(new Dictionary<string, IControlProperty>().GetActionElements());
	    private Dictionary<string, CollectionProperty> _collectionProperties = new Dictionary<string, CollectionProperty>();
        private Dictionary<string, IControlProperty> _properties = new Dictionary<string, IControlProperty>();

	    public PopupButton()
		{
			InitializeComponent();

	        InitProperties();
		}

	    private void InitProperties()
	    {
            _properties.InheritBaseElementSimpleProperties();

            _collectionProperties.Add("Items", _collectionProperty);
	    }

	    public void ApplySimpleProperties()
		{

			ButtonDropDown.Text = _properties["Text"].ToString();
			ButtonDropDown.Enabled = _properties["Enabled"].ToString().ToLowerInvariant() != "false";
		}

		public void ApplyCollections()
		{
		    CreatePopupMenuItems();
		}

	    private void CreatePopupMenuItems()
	    {
	        popupMenu.ItemLinks.Clear();
	        barManager1.Items.Clear();
	        var collection = _collectionProperties["Items"].Items;
	        foreach (var o in collection)
	        {
	            BarButtonItem barButtonItem = null;
	            if (!DesignerExtensions.IsEmpty(o.Button))
	            {
	                barButtonItem = new BarButtonItem(barManager1, o.Button.Text);
	            }
	            else if (!DesignerExtensions.IsEmpty(o.PopupButton))
	            {
	                barButtonItem = new BarButtonItem(barManager1, o.PopupButton.Text);
	            }

	            if (barButtonItem != null)
	            {
	                popupMenu.ItemLinks.Add(barButtonItem);
	            }
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

            var items = (value.Items as IEnumerable);
            if (items != null)
            {
                _collectionProperties["Items"].Items = items.OfType<dynamic>().ToList();
            }
	    }

		public Dictionary<string, Func<IPropertyEditor>> GetPropertyEditors()
		{
			return new Dictionary<string, Func<IPropertyEditor>>().InheritBaseElementPropertyEditors(ObjectInspector);
		}

		public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
		{
			return new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>().InheritBaseElementValidators("PopupButton");
		}

		public dynamic GetLayout()
	    {
	        dynamic instance = new DynamicWrapper();
	        foreach (var property in _properties)
	        {
	            var objectProperty = property.Value as ObjectProperty;
                if (objectProperty != null)
                {
                    instance[property.Key] = objectProperty.Value;
                }

                else
                {
                    instance[property.Key] = property.Value;
                }
	        }

            instance.Items = _collectionProperties["Items"].Items;
	        return instance;
	    }

	    public void SetLayout(dynamic value)
	    {
	        CreatePopupMenuItems();
	    }

	    public string GetPropertyName()
	    {
	        return "PopupButton";
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
	}
}
