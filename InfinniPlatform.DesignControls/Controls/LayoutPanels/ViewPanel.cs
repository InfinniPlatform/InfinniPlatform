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

namespace InfinniPlatform.DesignControls.Controls.LayoutPanels
{
    public partial class ViewPanel : UserControl, IPropertiesProvider, ILayoutProvider, IInspectedItem
    {
        public ViewPanel()
        {
            InitializeComponent();

            InitProperties();
        }

        private Dictionary<string, IControlProperty> _properties = new Dictionary<string, IControlProperty>();

	    private void InitProperties()
        {
            _properties.InheritBaseElementSimpleProperties();

            _properties.Add("View", new ObjectProperty(new Dictionary<string, IControlProperty>().GetLinkViews(), new Dictionary<string, CollectionProperty>()));
        }

        public void ApplySimpleProperties()
        {
	        ScriptEditor.ViewMode = true;
	        ScriptEditor.Script = ((ObjectProperty) _properties["View"]).Value.ToString();
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
			return new Dictionary<string, Func<IPropertyEditor>>()
				.InheritBindingPropertyEditors(ObjectInspector)
				.InheritViewPropertyEditors(ObjectInspector)
				.InheritBaseElementPropertyEditors(ObjectInspector);
		}

	    public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
	    {
		    return new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>()
			           {
				           {"View",Common.CreateNullOrEmptyValidator("ViewPanel","View")}
			           }.InheritBaseElementValidators("ViewPanel");
	    }


	    public dynamic GetLayout()
        {
            dynamic instanceLayout = new DynamicWrapper();

            foreach (var simpleProperty in _properties)
            {
                var objectProperty = simpleProperty.Value as ObjectProperty;
                if (objectProperty != null)
                {
                    instanceLayout[simpleProperty.Key] = objectProperty.Value;
                }
                else
                {
                    instanceLayout[simpleProperty.Key] = simpleProperty.Value.Value;
                }
            }

            return instanceLayout;

        }

        public void SetLayout(dynamic value)
        {
	        
        }

        public string GetPropertyName()
        {
            return "ViewPanel";
        }

	    public ObjectInspectorTree ObjectInspector { get; set; }
    }
}
