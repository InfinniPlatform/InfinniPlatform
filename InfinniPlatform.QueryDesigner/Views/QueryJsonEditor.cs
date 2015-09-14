using System;
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

        public string JsonQueryText
        {
            get { return JsonEdit.Text; }
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
    }
}