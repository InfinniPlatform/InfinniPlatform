using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using InfinniPlatform.Api.Schema;
using InfinniPlatform.QueryDesigner.Contracts;

namespace InfinniPlatform.QueryDesigner.Views
{
    public partial class QueryConstructorPathToProperty : UserControl
    {
        private IEnumerable<object> _pathItems;

        public QueryConstructorPathToProperty()
        {
            InitializeComponent();
        }

        public PathResolveType PathResolveType { get; set; }

        public string Path
        {
            get { return ComboBoxPath.EditValue != null ? ComboBoxPath.Text : string.Empty; }
        }

        public SchemaObject SchemaObject
        {
            get { return ComboBoxPath.EditValue as SchemaObject; }
        }

        public IDataProvider DataProvider { get; set; }

        public string Caption
        {
            get { return CaptionLabel.Text; }
            set { CaptionLabel.Text = value; }
        }

        public void SetPathItems(IEnumerable<dynamic> pathItems)
        {
            _pathItems = pathItems;
            RefreshSchema();
        }

        public void RefreshSchema()
        {
            ComboBoxPath.Properties.Items.Clear();


            foreach (dynamic pathItem in _pathItems)
            {
                IEnumerable<SchemaObject> paths = DataProvider.GetPropertyPaths(pathItem.Configuration,
                    pathItem.Document, pathItem.Alias, PathResolveType);

                ComboBoxPath.Properties.Items.AddRange(paths.Select(p => new ImageComboBoxItem(
                    string.IsNullOrEmpty(pathItem.Alias) ? p.GetFullPath() : pathItem.Alias + "." + p.GetFullPath(), p))
                    .ToList());
            }
        }

        public void Clear()
        {
            ComboBoxPath.Properties.Items.Clear();
        }
    }
}