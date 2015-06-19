using System.Collections.Generic;
using System.Windows.Forms;
using InfinniPlatform.Api.Schema;

namespace InfinniPlatform.QueryDesigner.Forms
{
    public partial class QueryDesignerForm : Form
    {
        public QueryDesignerForm()
        {
            InitializeComponent();
        }

        public string Query
        {
            get { return designControl1.GetQuery(); }
        }

        public IEnumerable<SchemaObject> SelectObjects
        {
            get { return designControl1.GetSelectObjects(); }
        }
    }
}