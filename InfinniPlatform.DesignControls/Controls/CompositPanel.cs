using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.DesignControls.Controls.LayoutPanels.GridPanels;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace InfinniPlatform.DesignControls.Controls
{
	public partial class CompositPanel : UserControl, ILayoutProvider, ILayoutContainer, IAlignment, IInspectedItem, IControlHost
	{
		public CompositPanel()
		{
			InitializeComponent();

			BorderStyle = BorderStyle.None;

			_controlOrientation = "Vertical";

		}


		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			if (Parent is GridPanel)
			{
				PanelContainer.LookAndFeel.UseDefaultLookAndFeel = true;
			}

		}


		public void ClearControls()
		{
			_spanCells.Clear();

			PanelContainer.Controls.Clear();

			_initialOrder.Clear();
		}

		private List<int> _spans = new List<int>() { 12 };

		public void SetColumnSpan(IEnumerable<int> columnSpans)
		{
			_spans = columnSpans.ToList();

            

			ClearControls();

			CreateSpanCells();

			AlignSpans();

		}

		private void AlignSpans()
		{
			if (_inCreateCells)
			{
				return;
			}

			if (_spanCells.Count == 0)
			{
				return;
			}

			var fullWidth = GetClientWidth();

			var widthOfOneControl = Math.Floor((double)fullWidth / 12);
			

			for (int i = 0; i < ColumnSpans.Count(); i++)
			{
				var size = Convert.ToInt32((widthOfOneControl) * ColumnSpans[i]);
				if (i == ColumnSpans.Count - 1)
				{
					_spanCells[i].ControlWrapper.Width = size + 2;					
				}
				else
				{
					_spanCells[i].ControlWrapper.Width = size;

				}

			}
		}



		private void CreateSpanCells()
		{
			_inCreateCells = true;
			for (int i = 0; i < ColumnSpans.Count(); i++)
			{
				var panel = new CompositPanel();

				//создаем обертку PropertiesControl
				var propertiesNode = RegisterSpanCell(panel);

				propertiesNode.ControlWrapper.Dock = DockStyle.Left;

				_spanCells.Add(propertiesNode);
			}
			_inCreateCells = false;
		}

		private readonly List<PropertiesNode> _spanCells = new List<PropertiesNode>();

		private PropertiesNode RegisterSpanCell(CompositPanel panel)
		{
			_memento.BeginUpdate();

			var propertiesNode = new PropertiesNode(panel)
									 {
										 ControlName = ObjectInspector.ControlRepository.GetName("GridPanelCell"),
										 EnabledLayoutTypes = new List<EnabledItems>()
						                                          {
							                                          EnabledItems.Action,
							                                          EnabledItems.Data,
							                                          EnabledItems.Layout
						                                          },
										 OnCopy = () => false,
									 };
			if (ObjectInspector != null)
			{
				ObjectInspector.AddNode(propertiesNode);
			}
			_memento.EndUpdate();
			//возвращаем обертку PropertiesControl над добавляемой панелью
			return propertiesNode;
		}


		private string _controlOrientation;


		public string ControlOrientation
		{
			get { return _controlOrientation; }
			set
			{
				_controlOrientation = value;
				AlignControls();
			}
		}


		public ILayoutProvider GetCell(int index)
		{
			return _initialOrder[index] as ILayoutProvider;
		}

		public void AlignControls()
		{
			if (_inCreateCells)
			{
				return;
			}

			AlignSpans();

			var widthOfOneControl = GetOneControlWidth();
			var controlsHeights = GetAllControlsHeights();

			var controlList = PanelContainer.Controls.OfType<ILayoutProvider>().OfType<Control>().ToList().Except(_spanCells.Select(s => s.ControlWrapper)).ToList();
			for (int i = 0; i < controlList.Count; i++)
			{
				
				controlList[i].Dock = DockStyle.None;
				controlList[i].Width = widthOfOneControl;
				controlList[i].Height = GetControlHeight(controlList[i], controlsHeights);
			}

			

			for (int i = 0; i < controlList.Count; i++)
			{
				if (_controlOrientation == "Vertical")
				{
					controlList[i].Dock = DockStyle.Top;
				}
				else
				{
					controlList[i].Dock = DockStyle.Left;
				}

			}

			var alignments = PanelContainer.Controls.OfType<IAlignment>().ToList();

			foreach (IAlignment alignment in alignments)
			{
				alignment.AlignControls();
			}

			_initialOrder.ToList().ForEach(i => i.BringToFront());

			
		}



		private int GetControlHeight(Control control, Dictionary<Control, int> heights)
		{
			if (_controlOrientation == "Horizontal")
			{
				return GetContainerHeight();
			}

			return heights[control];
		}

		private Dictionary<Control, int> GetAllControlsHeights()
		{
			var heightDictionary = new Dictionary<Control, int>();

			foreach (ILayoutProvider provider in PanelContainer.Controls.OfType<ILayoutProvider>())
			{
				var control = provider as Control;
				if (control != null)
				{
					heightDictionary.Add(control, 0);
					var clientHeightProvider = control as IClientHeightProvider;
					
					if (clientHeightProvider != null && clientHeightProvider.IsFixedHeight())
					{
						heightDictionary[control] = clientHeightProvider.GetClientHeight();
					}
				}
			}

			var usedClientHeight = heightDictionary.Sum(c => c.Value);

			var restControlHeight = GetContainerHeight() - usedClientHeight;

			var notAllocatedHeightControls = heightDictionary.Where(h => h.Value == 0).Select(h => h.Key).ToList();

			if (notAllocatedHeightControls.Count > 0)
			{

				var heightOfOneNotAllocatedControl = Convert.ToInt32(restControlHeight / notAllocatedHeightControls.Count);

				foreach (Control notAllocatedHeightControl in notAllocatedHeightControls)
				{
					heightDictionary[notAllocatedHeightControl] = heightOfOneNotAllocatedControl;
				}
			}

			return heightDictionary;

		}

		public int GetContainerHeight()
		{
			return Height - Padding.Top - Padding.Bottom - 5;
		}

		private int GetOneControlWidth()
		{
			if (_controlOrientation == "Vertical")
			{
				return GetClientWidth();
			}

			var controlsCount = PanelContainer.Controls.OfType<ILayoutProvider>().OfType<Control>().Count();
			if (controlsCount > 0)
			{
				return GetClientWidth() / controlsCount;
			}
			return GetClientWidth();
		}

		private int GetClientWidth()
		{
			return PanelContainer.Width;
		}

		private readonly List<Control> _initialOrder = new List<Control>();

		public PropertiesControl AddControl(Control innerControl)
		{
			var control = new PropertiesControl();
			control.ObjectInspector = ObjectInspector;
			innerControl.Dock = DockStyle.Fill;
			control.Control = innerControl;
			control.Dock = DockStyle.Fill;
			_initialOrder.Add(control);
			PanelContainer.Controls.Add(control);
			return control;
		}

		//добавление контролов



		public dynamic GetLayout()
		{
			var controls = PanelContainer.Controls.OfType<ILayoutProvider>().Reverse();

			dynamic items = new List<dynamic>();
			foreach (var item in controls)
			{
				dynamic instanceItem = new DynamicWrapper();
				instanceItem[item.GetPropertyName()] = item.GetLayout();
				items.Add(instanceItem);
			}
			return items;
		}

	    public void InsertLayout(dynamic layout)
	    {
	        _memento.BeginUpdate();

	        foreach (var layoutProperty in layout)
	        {
	            PropertiesNode propertiesNode = null;
                propertiesNode = ObjectInspector.ControlRepository.HasControl(layoutProperty.Key) ? 
                    ObjectInspector.ControlRepository.CreateControl(layoutProperty.Key) :
                    ObjectInspector.ControlRepository.CreateNotImplementedControl(layoutProperty.Key);

                propertiesNode.ControlName = string.IsNullOrEmpty(layoutProperty.Value.Name)
	                ? propertiesNode.ControlName
                    : layoutProperty.Value.Name;

	            ObjectInspector.AddNode(propertiesNode);

	            var propertiesProvider = propertiesNode.GetControl() as IPropertiesProvider;
	            var layoutProvider = propertiesNode.GetControl() as ILayoutProvider;

	            if (propertiesProvider != null)
	            {
                    propertiesProvider.LoadProperties(layoutProperty.Value);
	                propertiesProvider.ApplySimpleProperties();
	                propertiesProvider.ApplyCollections();
	            }

	            if (layoutProvider != null)
	            {
                    layoutProvider.SetLayout(layoutProperty.Value);
	            }
	        }

	        _memento.EndUpdate();
	    }


	    public void SetLayout(dynamic value)
		{
			if (value == null)
			{
				return;
			}

            foreach (var control in value)
			{
				InsertLayout(control);
			}


		}

		public string GetPropertyName()
		{
			return "Items";
		}


		public IEnumerable<ILayoutProvider> GetLayoutControls()
		{
			return PanelContainer.Controls.OfType<ILayoutProvider>().Reverse().ToList();

		}

		private ObjectInspectorTree _objectInspector;
		private FocusedNodeMemento _memento;
		private bool _inCreateCells;

	    public ObjectInspectorTree ObjectInspector
		{
			get
			{
				return _objectInspector;
			}
			set
			{
				_objectInspector = value;
				_memento = new FocusedNodeMemento(_objectInspector);
			}
		}

	    public List<int> ColumnSpans
	    {
	        get { return _spans; }
	    }

	    public bool ExcludeResize
	    {
	        get { return _excludeResize; }
	    }


	    public CompositPanel GetHost()
		{
			return this;
		}

	    private bool _excludeResize;

	    public void DoNotCheckInResize()
	    {
	        _excludeResize = true;
	    }
	}
}
