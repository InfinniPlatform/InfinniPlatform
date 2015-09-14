using System;
using System.Windows.Forms;

namespace InfinniPlatform.QueryDesigner.Forms
{
    public partial class QueryExecutorForm : Form
    {
        public QueryExecutorForm()
        {
            InitializeComponent();
        }

        public string QueryText
        {
            get { return queryExecutor1.QueryText; }
            set { queryExecutor1.QueryText = value; }
        }

        public string QueryResult
        {
            get { return queryExecutor1.QueryResultJSON; }
        }

        private void ButtonCloseClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}