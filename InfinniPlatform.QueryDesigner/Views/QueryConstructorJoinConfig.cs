using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using InfinniPlatform.QueryDesigner.Contracts;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.QueryDesigner.Views
{
    public partial class QueryConstructorJoinConfig : UserControl, IQueryBlockProvider
    {
        private static int itemsCounter;
        private string _configuration;

        public QueryConstructorJoinConfig()
        {
            InitializeComponent();
            PathConstructor.ControlType = typeof (QueryConstructorPathToProperty);
            PathConstructor.AddItem();
            ((QueryConstructorPathToProperty) PathConstructor.Items.First()).Caption = "Document field (Section FROM)";

            itemsCounter++;
            Name = string.Format("{0}{1}", Name, itemsCounter);
        }

        private string Path
        {
            get { return ((QueryConstructorPathToProperty) PathConstructor.Items.First()).Path; }
        }

        public Action<string> OnDocumentValueChanged
        {
            get { return IndexConfigPart.OnDocumentValueChanged; }
            set { IndexConfigPart.OnDocumentValueChanged = value; }
        }

        public Action<string> OnConfigurationValueChanged
        {
            get { return IndexConfigPart.OnConfigurationValueChanged; }
            set { IndexConfigPart.OnConfigurationValueChanged = value; }
        }

        public Action<string> OnAliasValueChanged
        {
            get { return IndexConfigPart.OnAliasValueChanged; }
            set { IndexConfigPart.OnAliasValueChanged = value; }
        }

        public ConstructOrder GetConstructOrder()
        {
            return ConstructOrder.ConstructJoin;
        }

        public void ProcessQuery(dynamic query)
        {
            if (IndexConfigPart.Configuration != null && IndexConfigPart.Document != null)
            {
                if (query.Join == null)
                {
                    query.Join = new List<dynamic>();
                }

                dynamic join = new DynamicWrapper();

                join.Index = IndexConfigPart.Configuration;
                join.Type = IndexConfigPart.Document;
                if (!string.IsNullOrEmpty(IndexConfigPart.Alias))
                {
                    join.Alias = IndexConfigPart.Alias;
                }
                join.Path = Path;

                query.Join.Add(join);
            }
        }

        public bool DefinitionCompleted()
        {
            return !string.IsNullOrEmpty(IndexConfigPart.Configuration) &&
                   !string.IsNullOrEmpty(IndexConfigPart.Document) && !string.IsNullOrEmpty(Path) &&
                   !string.IsNullOrEmpty(IndexConfigPart.Alias);
        }

        public string GetErrorMessage()
        {
            return "Some required field in block 'JOIN' unsettled.";
        }

        public void NotifyFromConfigurationChanged(string configuration)
        {
            _configuration = configuration;
            var item = (QueryConstructorPathToProperty) PathConstructor.Items.First();
            item.Clear();
        }

        public void NotifyFromDocumentChanged(string document)
        {
            var item = (QueryConstructorPathToProperty) PathConstructor.Items.First();
            item.Clear();
            dynamic pathItem = new DynamicWrapper();
            pathItem.Configuration = _configuration;
            pathItem.Document = document;
            item.SetPathItems(new[]
            {
                pathItem
            });
        }
    }
}