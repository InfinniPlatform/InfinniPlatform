using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.QueryDesigner.Contracts;

namespace InfinniPlatform.QueryDesigner.Views
{
	public partial class QueryExecutor : UserControl
	{
		private long _queryElapsedMilliseconds;

		public QueryExecutor()
		{
			InitializeComponent();

			IndexQueryExecutor = new Contracts.Implementation.QueryExecutor();
		}

		public string QueryText
		{
			get { return MemoQuery.Text; }
			set { MemoQuery.Text = value; }
		}

		public long QueryElapsedMilliseconds
		{
			get { return _queryElapsedMilliseconds; }
		}

		public string QueryResultJSON
		{
			get { return MemoEditResultJSON.Text; }
		}

		public IQueryExecutor IndexQueryExecutor { get; set; }

		private void ButtonExecuteClick(object sender, EventArgs e)
		{
			var stopwatch = Stopwatch.StartNew();
			try
			{
				var result = IndexQueryExecutor.ExecuteQuery(QueryText, DenormalizeCheckEdit.Checked);

                MemoEditResultJSON.Text = string.Join(Environment.NewLine, result);
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format("Executing query raise an error: {0}", ex.Message));
				return;
			}
			stopwatch.Stop();

			_queryElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
			ElapsedMillisecondsText.EditValue = _queryElapsedMilliseconds.ToString();
		}
	}
}
