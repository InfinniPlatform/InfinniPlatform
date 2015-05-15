using System;
using System.Collections.Generic;
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

namespace InfinniPlatform.DesignControls.Controls.ChildViews
{
	public sealed class ChildView : IPropertiesProvider, ILayoutProvider, IInspectedItem
	{
		public ChildView()
		{
			InitProperties();
		}

        private readonly Dictionary<string, IControlProperty> _properties = new Dictionary<string, IControlProperty>();


		public string Name
		{
			get { return _properties["Name"].Value.ToString(); }
		}

		public ObjectInspectorTree ObjectInspector { get; set; }

		private void InitProperties()
		{
			_properties.Add("Name", new SimpleProperty(string.Empty));
			_properties.Add("LinkView", new SimpleProperty(null));
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
			return new Dictionary<string, Func<IPropertyEditor>>()
				       {
					       {"LinkView", () => new JsonObjectEditor()}
				       };
		}

		public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
		{
			return new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>();
		}

		public override string ToString()
		{
			return GetLayout().ToString();
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

			try
			{
				if (instanceLayout.LinkView != null)
				{
					instanceLayout.LinkView = ((object) instanceLayout.LinkView.ToString()).ToDynamic();
				}
			}
			catch (Exception e)
			{
				MessageBox.Show("Fail to parse child view");
				instanceLayout.LinkView = null;
			}

			return instanceLayout;
		}

		public void SetLayout(dynamic value)
		{
			
		}

		public string GetPropertyName()
		{
			return "ChildView";
		}
	}
}
