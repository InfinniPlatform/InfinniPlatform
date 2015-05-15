using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfinniPlatform.QueryDesigner.Contracts;

namespace InfinniPlatform.QueryDesigner.Views
{
	public partial class QueryConstructorControlsContainer : UserControl
	{
		private Control _selectedControl;

		public QueryConstructorControlsContainer()
		{
			InitializeComponent();

		}


		public Type ControlType { get; set; }

		public IEnumerable<Control> Items
		{
			get { return ScrollControl.Controls.Cast<Control>().ToList(); }
		} 
		
		public Control AddItem()
		{
			if (ControlType == null)
			{
				throw new ArgumentException("Control type should not be empty");
			}

			var instance = (Control)Activator.CreateInstance(ControlType);
			ScrollControl.Controls.Add(instance);
			instance.Dock = DockStyle.Top;


			if (OnItemAdded != null)
			{
				OnItemAdded(instance);
			}

			return instance;
		}

		public Action<Control> OnItemAdded { get; set; }



		public void DeleteItem(Control control)
		{
			ScrollControl.Controls.Remove(control);
			if (OnItemDeleted != null)
			{
				OnItemDeleted(control);
			}
		}

		public Action<Control> OnItemDeleted { get; set; }

		public void ClearItems()
		{
			ScrollControl.Controls.Clear();			
		}
	}
}
