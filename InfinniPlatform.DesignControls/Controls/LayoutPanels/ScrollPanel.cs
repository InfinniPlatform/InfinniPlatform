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
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.DesignControls.Controls.Properties;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.DesignControls.PropertyEditors;

namespace InfinniPlatform.DesignControls.Controls.LayoutPanels
{
	public partial class ScrollPanel : UserControl, IPropertiesProvider, ILayoutProvider, ILayoutContainer, IAlignment, IControlHost, IInspectedItem
	{
		public ScrollPanel()
		{
			InitializeComponent();

			_compositPanel = new CompositPanel();
			_compositPanel.Dock = DockStyle.Fill;
			Controls.Add(_compositPanel);

			InitProperties();
		}

        private readonly Dictionary<string, IControlProperty> _properties = new Dictionary<string, IControlProperty>();
		private readonly CompositPanel _compositPanel;

		private void InitProperties()
		{
			_properties.InheritBaseElementSimpleProperties();
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
			return new Dictionary<string, Func<IPropertyEditor>>().InheritBaseElementPropertyEditors(ObjectInspector);
		}

		public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
		{
			return new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>().InheritBaseElementValidators("ScrollPanel");
		}


		public dynamic GetLayout()
		{
			dynamic instanceLayout = new DynamicWrapper();

			DesignerExtensions.SetSimplePropertiesToInstance(this, instanceLayout);
			instanceLayout.LayoutPanel = _compositPanel.GetLayout().FirstOrDefault();
			return instanceLayout;
		}

		public void SetLayout(dynamic value)
		{
			_compositPanel.InsertLayout(value.LayoutPanel);
		}

		public virtual string GetPropertyName()
		{
			return "ScrollPanel";
		}

		public void InsertLayout(dynamic layout)
		{
			_compositPanel.InsertLayout(layout);
		}

		public void AlignControls()
		{
			_compositPanel.Height = Height - Padding.Top - Padding.Bottom;
			_compositPanel.AlignControls();
		}

		public CompositPanel GetHost()
		{
			return _compositPanel;
		}

		public ObjectInspectorTree ObjectInspector
		{
			get
			{
				return _compositPanel.ObjectInspector;
			}
			set
			{
				_compositPanel.ObjectInspector = value;
			}
		}
	}
}
