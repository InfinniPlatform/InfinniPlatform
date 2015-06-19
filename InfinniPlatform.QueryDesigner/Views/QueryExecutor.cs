using System;
using System.Diagnostics;
using System.Windows.Forms;
using InfinniPlatform.QueryDesigner.Contracts;

namespace InfinniPlatform.QueryDesigner.Views
{
    public partial class QueryExecutor : UserControl
    {
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

        public long QueryElapsedMilliseconds { get; private set; }

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
                var result = IndexQueryExecutor.ExecuteQuery(null, QueryText, DenormalizeCheckEdit.Checked);

                MemoEditResultJSON.Text = string.Join(Environment.NewLine, result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Executing query raise an error: {0}", ex.Message));
                return;
            }
            stopwatch.Stop();

            QueryElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            ElapsedMillisecondsText.EditValue = QueryElapsedMilliseconds.ToString();
        }
    }
}