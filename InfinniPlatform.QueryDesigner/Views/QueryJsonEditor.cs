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
	public partial class QueryJsonEditor : UserControl, IQueryBlockProvider
	{
		public QueryJsonEditor()
		{
			InitializeComponent();
		}

		public ConstructOrder GetConstructOrder()
		{
			return ConstructOrder.ConstructFullQuery;			
		}

		public void ProcessQuery(dynamic query)
		{
			JsonEdit.Text = query.ToString();
		}

		public bool DefinitionCompleted()
		{
			return true;
		}

		public string GetErrorMessage()
		{
			throw new NotImplementedException();
		}

		public string JsonQueryText
		{
			get { return JsonEdit.Text; }
		}
	}
}
