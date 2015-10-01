using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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

        public Action<Control> OnItemAdded { get; set; }
        public Action<Control> OnItemDeleted { get; set; }

        public Control AddItem()
        {
            if (ControlType == null)
            {
                throw new ArgumentException("Control type should not be empty");
            }

            var instance = (Control) Activator.CreateInstance(ControlType);
            ScrollControl.Controls.Add(instance);
            instance.Dock = DockStyle.Top;


            if (OnItemAdded != null)
            {
                OnItemAdded(instance);
            }

            return instance;
        }

        public void DeleteItem(Control control)
        {
            ScrollControl.Controls.Remove(control);
            if (OnItemDeleted != null)
            {
                OnItemDeleted(control);
            }
        }

        public void ClearItems()
        {
            ScrollControl.Controls.Clear();
        }
    }
}