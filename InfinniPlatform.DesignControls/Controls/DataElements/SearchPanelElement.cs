﻿using System;
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
	public partial class SearchPanelElement : UserControl, IPropertiesProvider, ILayoutProvider, IClientHeightProvider, IInspectedItem
	{
		public SearchPanelElement()
		{
			InitializeComponent();

			InitProperties();
		}

        private readonly Dictionary<string, IControlProperty> _simpleProperties = new Dictionary<string, IControlProperty>();

		private void InitProperties()
		{
			_simpleProperties.InheritBaseElementSimpleProperties();
		    _simpleProperties.InheritBaseElementValueBinding();

			_simpleProperties.Add("DataSource", new SimpleProperty(null));
            _simpleProperties.Add("OnValueChanged", new ObjectProperty(new Dictionary<string, IControlProperty>()
                {
                    {"Name", new SimpleProperty(string.Empty)}
                }, new Dictionary<string, CollectionProperty>()));


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
			return new Dictionary<string, Func<IPropertyEditor>>()
				       {
					       {"DataSource", () => new DataSourceEditor(ObjectInspector)}
				       }
				.InheritBaseElementPropertyEditors(ObjectInspector);
		}

		public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
		{
			return new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>()
			           {
				           {"DataSource",Common.CreateNullOrEmptyValidator("SearchPanel","DataSource")},
						   {"Value",Common.CreateNullOrEmptyValidator("SearchPanel","Value")},
			           }.InheritBaseElementValidators("SearchPanel");
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
			return "SearchPanel";
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
