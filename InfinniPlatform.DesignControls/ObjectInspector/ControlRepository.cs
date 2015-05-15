using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfinniPlatform.DesignControls.Controls.ActionElements;
using InfinniPlatform.DesignControls.Controls.DataElements;
using InfinniPlatform.DesignControls.Controls.LayoutPanels;
using InfinniPlatform.DesignControls.Controls.LayoutPanels.GridPanels;
using InfinniPlatform.DesignControls.Controls.NotImplemented;
using Button = InfinniPlatform.DesignControls.Controls.ActionElements.Button;
using Panel = InfinniPlatform.DesignControls.Controls.LayoutPanels.Panel;
using ToolBar = InfinniPlatform.DesignControls.Controls.ActionElements.ToolBar;

namespace InfinniPlatform.DesignControls.ObjectInspector
{
	/// <summary>
	///   Репозиторий контролов, доступных для создания в дизайнере
	/// </summary>
	public sealed class ControlRepository
	{
		private static readonly List<Tuple<string, Type, string, IEnumerable<EnabledItems>>> _controlTypes = new List<Tuple<string, Type, string, IEnumerable<EnabledItems>>>()
			                                                {
																new Tuple<string, Type, string, IEnumerable<EnabledItems>>("PopupButton",typeof(PopupButton),"Action",new List<EnabledItems>()),
																new Tuple<string, Type, string,IEnumerable<EnabledItems>>("Button", typeof(Button),"Action",new List<EnabledItems>()),
																new Tuple<string, Type, string,IEnumerable<EnabledItems>>("ToggleButton",typeof(ToggleButtonElement),"Action",new List<EnabledItems>()),
																new Tuple<string, Type, string,IEnumerable<EnabledItems>>("MenuBar",typeof(MenuBar),"Action",new List<EnabledItems>()),
																new Tuple<string, Type, string,IEnumerable<EnabledItems>>("ToolBar", typeof(ToolBar),"Action",new List<EnabledItems>()),																
																								
																new Tuple<string, Type, string,IEnumerable<EnabledItems>>("TabPanel", typeof(TabPanel),"Layout",new List<EnabledItems>()),
																new Tuple<string, Type, string,IEnumerable<EnabledItems>>("GridPanel", typeof(GridPanel),"Layout",new List<EnabledItems>()),
																new Tuple<string, Type, string,IEnumerable<EnabledItems>>("ViewPanel", typeof(ViewPanel),"Layout",new List<EnabledItems>()),
																new Tuple<string, Type, string,IEnumerable<EnabledItems>>("Panel",typeof(Panel),"Layout", new List<EnabledItems>() {EnabledItems.Action,EnabledItems.Data,EnabledItems.Layout}),
																new Tuple<string, Type, string,IEnumerable<EnabledItems>>("ScrollPanel",typeof(ScrollPanel),"Layout", new List<EnabledItems>() {EnabledItems.Action,EnabledItems.Data,EnabledItems.Layout}),
																new Tuple<string, Type, string,IEnumerable<EnabledItems>>("StackPanel", typeof(StackPanel),"Layout",new List<EnabledItems>() {EnabledItems.Action,EnabledItems.Data,EnabledItems.Layout}),
																								
																new Tuple<string, Type, string,IEnumerable<EnabledItems>>("SearchPanel", typeof(SearchPanelElement),"Data",new List<EnabledItems>()),
																new Tuple<string, Type, string,IEnumerable<EnabledItems>>("FilterPanel", typeof(FilterPanelElement),"Data",new List<EnabledItems>()),
																new Tuple<string, Type, string,IEnumerable<EnabledItems>>("ComboBox", typeof(ComboBoxElement),"Data",new List<EnabledItems>()),
																new Tuple<string, Type, string,IEnumerable<EnabledItems>>("TextBox", typeof(TextEditElement),"Data",new List<EnabledItems>()),
																new Tuple<string, Type, string,IEnumerable<EnabledItems>>("CheckBox", typeof(CheckBoxElement),"Data",new List<EnabledItems>()),
																new Tuple<string, Type, string,IEnumerable<EnabledItems>>("DatePicker",typeof(DatePickerElement),"Data",new List<EnabledItems>()),
																new Tuple<string, Type, string,IEnumerable<EnabledItems>>("ListBox",typeof(ListBoxElement),"Data",new List<EnabledItems>()),
				                                                new Tuple<string, Type, string,IEnumerable<EnabledItems>>("Label", typeof(LabelElement),"Data",new List<EnabledItems>()),
																new Tuple<string, Type, string,IEnumerable<EnabledItems>>("DataGrid", typeof(DataGridElement),"Data",new List<EnabledItems>())
			                                                };


		public static List<Tuple<string, Type, string, IEnumerable<EnabledItems>>> ControlTypes
		{
			get { return _controlTypes; }
		}

		public static List<Tuple<string, Type, string, IEnumerable<EnabledItems>>> GetActionControls
		{
			get { return _controlTypes.Where(c => c.Item3 == "Action").ToList(); }
		}

		public static List<Tuple<string, Type, string, IEnumerable<EnabledItems>>> GetDataControls
		{
			get { return _controlTypes.Where(c => c.Item3 == "Data").ToList(); }
		}

		public static List<Tuple<string, Type, string, IEnumerable<EnabledItems>>> GetLayoutControls
		{
			get { return _controlTypes.Where(c => c.Item3 == "Layout").ToList(); }
		}

        private static readonly Dictionary<ObjectInspectorTree,ControlRepository> ControlRepositories = new Dictionary<ObjectInspectorTree, ControlRepository>();

		public static ControlRepository Instance(ObjectInspectorTree inspector)
		{
            if (ControlRepositories.ContainsKey(inspector))
            {
                return ControlRepositories[inspector];
            }
		    var instance = new ControlRepository();
            ControlRepositories.Add(inspector,instance);
		    return instance;
		}

		private int _index;


		public PropertiesNode CreateControl(string controlType)
		{
			var ctr = FindConstructor(controlType);
			if (ctr != null)
			{
				return CreateControl(ctr);
			}
			return null;
		}

		public PropertiesNode CreateControl(Tuple<string, Type, string, IEnumerable<EnabledItems>> controlConstructor)
		{
			var control = Activator.CreateInstance(controlConstructor.Item2) as Control;
			control.Dock = DockStyle.Fill;

			var propertiesNode = new PropertiesNode(control);
			propertiesNode.ControlName = controlConstructor.Item1 + _index++.ToString();
			propertiesNode.EnabledLayoutTypes = controlConstructor.Item4;

			return propertiesNode;
		}

		public PropertiesNode CreateNotImplementedControl(string key)
		{
			var control = new NotImplementedControl(key);
			control.Dock = DockStyle.Fill;

			var propertiesNode = new PropertiesNode(control);
			propertiesNode.ControlName = key + _index++.ToString();
			propertiesNode.EnabledLayoutTypes = new List<EnabledItems>();

			return propertiesNode;
		}


		public Tuple<string, Type, string, IEnumerable<EnabledItems>> FindConstructor(string controlType)
		{
			return _controlTypes.FirstOrDefault(f => f.Item1 == controlType);
		}


		private int index = 0;

		public string GetName(string control)
		{
			index++;
			return control + index;
		}


		public bool HasControl(string key)
		{
			return _controlTypes.FirstOrDefault(f => f.Item1 == key) != null;
		}

	    public static void RemoveInstance(ObjectInspectorTree objectInspector)
	    {
	        if (ControlRepositories.ContainsKey(objectInspector))
	        {
	            ControlRepositories.Remove(objectInspector);
	        }
	    }
	}
}
