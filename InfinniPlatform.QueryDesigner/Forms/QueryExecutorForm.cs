using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
