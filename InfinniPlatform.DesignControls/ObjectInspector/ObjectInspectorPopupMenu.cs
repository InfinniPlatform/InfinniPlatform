using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace InfinniPlatform.DesignControls.ObjectInspector
{
	public partial class ObjectInspectorPopupMenu : Component
	{

		public ObjectInspectorPopupMenu()
		{
			InitializeComponent();

			
		}

		public ObjectInspectorPopupMenu(IContainer container)
		{
			container.Add(this);

			InitializeComponent();		
				
		}


		public void SetItemLinks()
		{
			popupMenu.ItemLinks.Clear();

			var barSubItemDataElements = new BarSubItem()
				                             {
					                             Name = "DataElements",
					                             Caption = "Data Elements"
				                             };
			popupMenu.ItemLinks.Add(barSubItemDataElements);

			var barSubItemActionElements = new BarSubItem()
				                               {
					                               Name = "ActionElements",
					                               Caption = "Action Elements"
				                               };
			popupMenu.ItemLinks.Add(barSubItemActionElements);

			var barSubItemLayoutElements = new BarSubItem()
				                               {
					                               Name = "LayoutElements",
					                               Caption = "Layout Elements"
				                               };
			popupMenu.ItemLinks.Add(barSubItemLayoutElements);

			FillCategoryLinks(barSubItemLayoutElements, ControlRepository.GetLayoutControls);
			FillCategoryLinks(barSubItemActionElements, ControlRepository.GetActionControls);
			FillCategoryLinks(barSubItemDataElements, ControlRepository.GetDataControls);
		}


		private void FillCategoryLinks(BarSubItem barSubItem, IEnumerable<Tuple<string, Type, string, IEnumerable<EnabledItems>>> handlers )
		{
			foreach (var handler in handlers)
			{
				var item = new BarButtonItem()
					           {
						           Caption = handler.Item1
					           };

				Tuple<string, Type, string, IEnumerable<EnabledItems>> handler1 = handler;
				item.ItemClick += (sender, args) =>
					                  {
						                  var propertiesNode = ControlRepository.CreateControl(handler1);
					                  										  
										  AfterCreateControlHandler(propertiesNode);
					                  };
				barSubItem.ItemLinks.Add(item);
			}
		}

		public ControlRepository ControlRepository { get; set; }

		public Form Form
		{
			get { return (Form)barManager1.Form; }
			set
			{
				barManager1.Form = value;								
			}
		}

		public Action<PropertiesNode> AfterCreateControlHandler { get; set; }

		public void ShowPopup(Point coordinates)
		{			
			popupMenu.ShowPopup(coordinates);			
		}

		public void Prepare(PropertiesNode nodeSettings)
		{
			popupMenu.ItemLinks[0].Item.Enabled = nodeSettings.EnabledLayoutTypes.Contains(EnabledItems.Action);
			popupMenu.ItemLinks[1].Item.Enabled = nodeSettings.EnabledLayoutTypes.Contains(EnabledItems.Data);
			popupMenu.ItemLinks[2].Item.Enabled = nodeSettings.EnabledLayoutTypes.Contains(EnabledItems.Layout);
		}

		public void ClosePopup()
		{
			popupMenu.HidePopup();
		}
	}
}
