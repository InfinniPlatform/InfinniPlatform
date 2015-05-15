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

namespace InfinniPlatform.DesignControls.Controls.DataElements
{
	public partial class LabelElement : UserControl, IPropertiesProvider, ILayoutProvider, IClientHeightProvider, IInspectedItem
	{
        private readonly Dictionary<string, IControlProperty> _properties = new Dictionary<string, IControlProperty>();
 
        private readonly Dictionary<string,CollectionProperty> _collectionProperties = new Dictionary<string, CollectionProperty>(); 

		public LabelElement()
		{
			InitializeComponent();

		    InitProperties();
		}

	    private void InitProperties()
	    {
	        _properties.InheritBaseElementSimpleProperties();

	        _properties["Text"].Value = Label.Text;
	    }

	    public void ApplySimpleProperties()
		{
            Label.Text = _properties["Text"].Value.ToString();
            Label.Name = _properties["Name"].Value.ToString();
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
            return _collectionProperties;
	    }

	    public void LoadProperties(dynamic value)
	    {
	        DesignerExtensions.SetSimplePropertiesFromInstance(_properties, value);
	    }

		public Dictionary<string, Func<IPropertyEditor>> GetPropertyEditors()
		{
			return new Dictionary<string, Func<IPropertyEditor>>()
				.InheritBaseElementPropertyEditors(ObjectInspector)
				.InheritBindingPropertyEditors(ObjectInspector);
		}

		public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
		{
			return new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>().InheritBaseElementValidators("Label");
		}


		public dynamic GetLayout()
	    {
	        dynamic instanceLayout = new DynamicWrapper();
            DesignerExtensions.SetSimplePropertiesToInstance(_properties,instanceLayout);
	        return instanceLayout;
	    }

	    public void SetLayout(dynamic value)
	    {
	        ApplySimpleProperties();
	    }

	    public string GetPropertyName()
	    {
	        return "Label";
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

		public string LabelText
		{
			get { return Label.Text; }
			set { Label.Text = value; }
		}
	}


}
