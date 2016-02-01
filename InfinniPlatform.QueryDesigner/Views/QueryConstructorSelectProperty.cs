using System;
using System.Collections.Generic;
using System.Windows.Forms;

using InfinniPlatform.Core.Schema;
using InfinniPlatform.QueryDesigner.Contracts;

namespace InfinniPlatform.QueryDesigner.Views
{
    public partial class QueryConstructorSelectProperty : UserControl
    {
        public QueryConstructorSelectProperty()
        {
            InitializeComponent();
        }

        public Action<Control> OnDeleteItem { get; set; }

        public string Caption
        {
            get { return SelectPropertyPart.Caption; }
            set { SelectPropertyPart.Caption = value; }
        }

        public string SelectValue
        {
            get { return SelectPropertyPart.Path; }
        }

        public SchemaObject SelectedObject
        {
            get { return SelectPropertyPart.SchemaObject; }
        }

        public IDataProvider DataProvider
        {
            get { return SelectPropertyPart.DataProvider; }
            set { SelectPropertyPart.DataProvider = value; }
        }

        public PathResolveType PathResolveType
        {
            get { return SelectPropertyPart.PathResolveType; }
            set { SelectPropertyPart.PathResolveType = value; }
        }

        private void RemoveButtonClick(object sender, EventArgs e)
        {
            if (OnDeleteItem != null)
            {
                OnDeleteItem(this);
            }
        }

        public void SetPathItems(IEnumerable<dynamic> pathItems)
        {
            SelectPropertyPart.SetPathItems(pathItems);
        }
    }
}