using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.DesignControls.Controls.Properties;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.DesignControls.PropertyEditors;

namespace InfinniPlatform.DesignControls.Controls.ActionElements
{
    public partial class MenuBar : UserControl, IPropertiesProvider, ILayoutProvider, IInspectedItem, IClientHeightProvider
    {
        public MenuBar()
        {
            InitializeComponent();

            InitProperties();
        }

        private readonly Dictionary<string, IControlProperty> _simpleProperties = new Dictionary<string, IControlProperty>(); 

        private void InitProperties()
        {
            _simpleProperties.InheritBaseElementSimpleProperties();
            _simpleProperties.Add("ConfigId", new SimpleProperty(string.Empty));
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
			return new Dictionary<string, Func<IPropertyEditor>>().InheritBaseElementPropertyEditors(ObjectInspector);
		}

	    public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
	    {
		    return new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>()
			           {
				           {"ConfigId", Common.CreateNullOrEmptyValidator("MenuBar","ConfigId")}
			           }.InheritBaseElementValidators("MenuBar");
	    }


	    public dynamic GetLayout()
        {
            dynamic instanceLayout = new DynamicWrapper();
            DesignerExtensions.SetSimplePropertiesToInstance(_simpleProperties, instanceLayout);
            return instanceLayout;
        }

        public void SetLayout(dynamic value)
        {
        }

        public string GetPropertyName()
        {
            return "MenuBar";
        }

	    public ObjectInspectorTree ObjectInspector { get; set; }
	    public int GetClientHeight()
	    {
		    return 80;
	    }

	    public bool IsFixedHeight()
	    {
		    return true;
	    }
    }
}
