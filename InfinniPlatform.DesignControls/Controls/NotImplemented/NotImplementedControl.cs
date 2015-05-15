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
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.DesignControls.PropertyEditors;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.DesignControls.Controls.NotImplemented
{
	public partial class NotImplementedControl : UserControl, IPropertiesProvider, ILayoutProvider, IClientHeightProvider, IInspectedItem
	{
		private readonly string _controlType;

		public NotImplementedControl()
		{
			InitializeComponent();

			InitProperties();
		}

		public NotImplementedControl(string controlType)
		{
			_controlType = controlType;

			InitializeComponent();

			InitProperties();

			
		}

        private readonly Dictionary<string, IControlProperty> _simpleProperties = new Dictionary<string, IControlProperty>();

		private void InitProperties()
		{
			_simpleProperties.Add("ControlLayout", new SimpleProperty(new DynamicWrapper()));
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
			ScriptEditor.Script = value.ToString();
		}

		public Dictionary<string, Func<IPropertyEditor>> GetPropertyEditors()
		{
			return new Dictionary<string, Func<IPropertyEditor>>()
				       {
					       {"ControlLayout", () => new JsonObjectEditor()}
				       };
		}

		public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
		{
			return new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>();
		}

		public dynamic GetLayout()
		{
			return DynamicWrapperExtensions.ToDynamic((string)_simpleProperties["ControlLayout"].Value.ToString());
		}

		public void SetLayout(dynamic value)
		{
			_simpleProperties["ControlLayout"].Value = value != null ? value : new DynamicWrapper();
		}

		public string GetPropertyName()
		{
			return _controlType;
		}

		public int GetClientHeight()
		{
			return 120;
		}

		public bool IsFixedHeight()
		{
			return true;
		}

		public ObjectInspectorTree ObjectInspector { get; set; }
	}
}
